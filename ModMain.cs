using Il2Cpp;
using Il2CppLogicalGraphNodes;
using Il2CppPantheonPersist;
using Il2CppServiceStack;
using MelonLoader;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


[assembly: MelonInfo(typeof(EnhancedDebuffTracking.ModMain), "EnhancedDebuffTracking", "1.0.0", "Anonymous", null)]
[assembly: MelonGame("Visionary Realms", "Pantheon")]

namespace EnhancedDebuffTracking
{
    // TODO - When we are ready for uptime, we will need to recfactor to use EntityData and a way to track when we engage a new target to get the total time
    public class EntityData()
    {
        public bool isDead;
        public bool hasBeenDead; // The death status for a monster always changes changes to false when you move out of range, even if it is still dead, so now we track if it has ever been dead
        public List<DebuffData> debuffData = new List<DebuffData>();
        public long  encounterStartTime; // Total encounter time for this monster
        public string monsterNetworkId; // network id of this monster
        public long totalEncounterTime; // Time the monster has been engaged
        public string traits; // Concatenated string of all traits to be displayed
    }

    // This class will be used to store all the information required to display the debuff data in the debuff panel
    public class DebuffData()
    {
        public string debuffName; // Base name of the debuff
        public string debuffIconName; // Debuff Icon
        public string debuffType; // Debuff type, used to select what colour bar to display
        public float debuffDuration; // Debuff duration
        public float debuffDurationRemaining; // Used in the panel to keep track of remaining duration
        public string debuffDescription; // Could be used as a Tooltip

        public string targetName; // Nameplate name of the target
        public string targetNetworkId; // Unique ID of the target
        public string targetKind; // Humanoid, Undead etc.
        public string targetClass; // Rogue, Wizard etc.

        public string casterName; // Nameplate name of the caster
        public string casterNetworkId; // Unique ID of the caster

        public int numTicks; // Number of ticks
        public float tickIntervalS; // Interval between Ticks
        public int numStacks; // Number of stacks
        public int maxStacks; // Max stacks
        public string entityStatus; // Burning / Poisoned etc
        public SpellType spellType; // Spell Type (Nature, Corruption)

        public long localUptime; // Time this instance of this debuff has been up regardless of wether or not it has been cast before
        public float localUptimePercent; // Time the debuff has been up as a % of total encounter time
        public long consolidatedEncounterUptime; // Time the debuff has been up as a % of total encounter time
        public float consolidatedEncounterUptimePercent; // Time the debuff has been up as a % of total encounter time
    }


    // IMPORTANT INFORMATION ABOUT HOW THIS MOD WORKS AND WHY
    // This Mod collects all debuff information about all mobs that are currently in range, this is handle neatly in a single trigger.
    // The Pantheon Client does proves multiple remove triggers but none of them provide enough information to allow this mod to reliably remove debuffs (essential information is NULL or filled with default values under various conditions)
    // So to handle the removal of debuffs (expiry / monster dies) I have 2 solution in place:
    // 1) If the duration remaining reaches zero this mod delete the debuff itself, even if it have not receives a triogegr to do so (this is done in the OnUpdate() function and is the primary way debuffs are removed)
    // 2) The Remove Buff trigger for ONLY the current offensive target.

    // The key use-case scenarios for this mode are:
    // 1) A monster moves into range (EntityNpcGameObject.NetworkStart)
    // 2) A monster leaves range (EntityNpcGameObject.NetworkStop)
    // 3) A monster receices a debuff (Buffs.Logic.Add)
    // 4) A debuff on a monster expires (OnUpdate() if not the active target, or if the active target UIBuffBar.OnRemoveBuff)
    public class ModMain : MelonMod
    {
        // UI Elements
        private static DebuffPanel gDebuffPanel = new DebuffPanel();
        private static string gCurrentTargetNetworkId = ""; // Only update in offensive target select and only use in OnUpdate
        private const float UpdateInterval = 1.0f; // Update interval in seconds
        private static float _timeSinceLastUpdate;
        private static string TraitSubString = "Trait: ";
        private static readonly string[] Blacklist = { TraitSubString, "Mana Guzzle", "Taunt Immunity", "Feared", "Temporary Invulnerability" };
        private static string debuffPanelName = "EDT_DebuffPanel_EDT";
        private static EntityData testData = new EntityData();

        public override void OnInitializeMelon()
        {
            // Setup test data here
            testData.monsterNetworkId = "12345";
            testData.isDead = false;
            testData.hasBeenDead = false;
            testData.encounterStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            for (int i = 0; i < Globals.NumDisplayableDebuffs; i++)
            {
                testData.debuffData.Add(MakeTestDebuffData($"TestData_{i}"));
            }
        }

        private DebuffData MakeTestDebuffData(string text)
        {
            DebuffData newDebuff = new DebuffData();
            newDebuff.casterName = "CasterName";
            newDebuff.casterNetworkId = "CasterNID";
            newDebuff.targetName = "TargetName";
            newDebuff.targetNetworkId = "TargetNID";
            newDebuff.targetClass = "TargetClass";
            newDebuff.targetKind = "TargetKind";
            newDebuff.debuffName = $"Debuff{text}";
            newDebuff.debuffType = "Detrimental"; // Not especially useful but its something at least
            newDebuff.debuffDuration = 30.0f;
            newDebuff.debuffDurationRemaining = 30.0f;
            newDebuff.debuffDescription = $"Description{text}";
            newDebuff.numStacks = 1;
            newDebuff.maxStacks = 1;
            newDebuff.numTicks = 1;
            newDebuff.tickIntervalS = 1;
            newDebuff.spellType = SpellType.Conjuration;
            return newDebuff;
        }

        // Updates the duration timers on the panel.
        // WARNING:  This could be a problem if we get other process Hooks firing whilst updating the Lists through OnUpdate
        public override void OnUpdate()
        {
            // Only update if the player is loaded into the world
            if (Globals.PlayerIsLoaded == true)
            {
                // We only need update granularity of 1 second, save the CPU cycles
                _timeSinceLastUpdate += Time.deltaTime;
                if (_timeSinceLastUpdate >= UpdateInterval)
                {
                    // Update this immediatly so we dont flood in here
                    _timeSinceLastUpdate = 0f;

//                    MelonLogger.Warning($"OnUpdate() 1");

                    // Update the progress bars
                    EntityManager.UpdateDurationRemaining();

                    // Call the entitiy manager and get it to update the uptime timers
                    EntityManager.UpdateEncounterUpTime();

                    // If gCurrentTargetNetworkId is not populated there is no point updating the display
                    if (!gCurrentTargetNetworkId.Equals(""))
                    {
                        // Do not update the screen for dead enemies
                        EntityData entityData = EntityManager.GetEntityData(gCurrentTargetNetworkId);
                        // If the current network id despawns whilst targetted, dont try and update anything
                        if (entityData == null)
                        {
                            MelonLogger.Error($"OnUpdate() NO ENTITY DATA IN ONUPDATE gCurrentTargetNetworkId = {gCurrentTargetNetworkId}");
                            gCurrentTargetNetworkId = "";
                            return;
                        }

                        // We do not update the screen if the entity is dead
                        if (entityData.hasBeenDead == false && entityData.isDead == false)
                        {
//                            MelonLogger.Warning($"OnUpdate() 2 Enemy reprted as dead");

                            // If we have a valid debuff list for the current target, update the screen
                            gDebuffPanel.UpdatePanel(entityData);
                            //gDebuffPanel.UpdateDebuffPanel(testData);
                        }
                    }
                }
            }
        }

        // This function adds the new debuff panel to the UI
        public static void AddDebuffPanelToUI()
        {
            // This is a nasty hack but is required because I am too dumb to get ScrollRect working
            gDebuffPanel.PreserveRequiredTransforms();

            // Build the panel, attach it to the offensive target panel
            gDebuffPanel.DisplayPanel(debuffPanelName, UIPanelRoots.Instance.Mid.transform);
        }

        // Used to tear down all the resources allocated by the panel on logout / character change
        public static void ClearPanelLists()
        {
            gDebuffPanel.ClearPanelLists();
        }

        // Called to show the debuff panel
        public static void ShowDebuffPanel()
        {
            // Display the panel if the gloabl is set to allow it
            if (Globals.ShowDebuffPanel == true)
            {
                gDebuffPanel.ShowDebuffPanel();
            }
        }

        // Called to hide the debuff panel
        public static void HideDebuffPanel()
        {
            gDebuffPanel.HideDebuffPanel();
        }

        // This function takes the new number of rows and re-draws the panel with that number of rows
        public static void SetNumDebuffRows(string message)
        {
            string[] result = message.Split(Globals.SetNumberOfRowsCommand);
            int numRows = -1;
            // If we have something after the command name create that many rows
            if (result.Length > 1)
            {
                try
                {
                    // Minimum number of rows to display is 1
                    numRows = Int32.Parse(result[1]);
                    SanitiseNumRows(ref numRows);
                }
                catch (FormatException)
                {
                    return;
                }

                // Clear out the user visible data
                gDebuffPanel.ClearPanel();
                // Clear out the row data from the panel
                ClearPanelLists();

                // Set the new number of rows to be drawn (dont do this earlier, it can cause problems tearing down the correct number of TextMesh and Image tranforms)
                Globals.NumDisplayableDebuffs = numRows;
                gDebuffPanel.DisplayPanel(debuffPanelName, UIPanelRoots.Instance.Mid.transform);
            } // End of IF we have a value to parse
        }

        // Converts the provoided number to a valid number
        // We support the following.  1..10,15,20,25,30,35
        private static void SanitiseNumRows(ref int numRows)
        {
            // Ensure we have at least 1 row
            numRows = (numRows < 1) ? 1 : numRows;
            // Anything between 11 and 15 is set to 15
            numRows = (numRows > 10 && numRows < 16) ? 15 : numRows;
            // Anything between 16 and 20 is set to 20
            numRows = (numRows > 15 && numRows < 21) ? 20 : numRows;
            // Anything between 21 and 25 is set to 25
            numRows = (numRows > 20 && numRows < 26) ? 25 : numRows;
            // Anything between 26 and 30 is set to 30
            numRows = (numRows > 25 && numRows < 31) ? 30 : numRows;
            // Anything between 31 and 35 is set to 35
            numRows = (numRows > 30 && numRows < 36 ) ? 35 : numRows;
            // Ensure we have at least 1 row
            numRows = (numRows > 35) ? 35 : numRows;
        }

        // Determines if the target for a buff is valid for us to track
        private static bool IsValidTarget(ActiveBuff buff)
        {
            // We do not want buffs that go onto players even if those are from monsters to players
            // Monster have an access level of None so that is an easy way to detect its a monster not a player
            // TODO - Player Pets also have an access level of none but we dont want to track their debuffs, we need a way to filter them out, its not a problem if we do not but it would be better
            return (buff.Target.Info.AccessLevel.Equals(AccessLevel.None)) ? true : false;
        }

        // Determines if the target for a buff is valid for us to track
        private static bool IsValidDebuff(ActiveBuff buff)
        {
            // We do not want buffs that go onto players even if those are from monsters to players
            // Monster have an access level of None so that is an easy way to detect its a monster not a player
            // TODO - Player Pets also have an access level of none but we dont want to track their debuffs, we need a way to filter them out, its not a problem if we do not but it would be better
            return (buff.BuffData.CategoryType == BuffCategoryType.Harmful) ? true : false;
        }

        // This function is called in the following conditions (at least)
        // Make sure we dont re-add an existing buff to the buff list and you handle all the different conditions it can be called
        public static void OnAddOrRefreshBuff(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
        {
            MelonLogger.Warning($"OnAddOrRefreshBuff() 0a buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}, isRefresh = {isRefresh}, inBackground = {inBackground}, isItemBuff = {isItemBuff}");
            MelonLogger.Warning($"OnAddOrRefreshBuff() 0b buff.Target?.NetworkId.ToString() = {buff.Target?.NetworkId.ToString()}, gCurrentTargetNetworkId = {gCurrentTargetNetworkId}");

            // Make sure we only track debuffs and only on monsters
            if (IsValidTarget(buff) && IsValidDebuff(buff))
            {
                if (buff.BuffData == null || buff.Target == null || buff.Target.Nameplate == null || buff.Caster == null ||  buff.Caster.Nameplate == null)
                {
                    return;
                }

                // Do not process anything in the blacklist
                if (Blacklist.Contains(buff.BuffData.DisplayName.ToString()))
                {
                    return;
                }

                // Get the list for the current enemy
                EntityData entityData = EntityManager.GetEntityData(buff.Target.NetworkId.ToString());
                if (entityData == null)
                {
                    MelonLogger.Error($"OnAddOrRefreshBuff() NULL ENTRY FOUND FOR Target = {buff.Target?.Nameplate?.nameText.text}, Source = {buff.Caster?.Nameplate?.nameText.text} {buff.BuffData.DisplayName.ToString()}, DOES THIS EVEN MATTERS?");
                    return;
                }

                if (entityData.monsterNetworkId.IsEmpty() || entityData.monsterNetworkId.ToString().Equals(""))
                {
                    MelonLogger.Error($"OnAddOrRefreshBuff() entityData.monsterNetworkId is NULL");
                    return;
                }

                // Get the number of seconds since EPOCH from when the very first debuff lands
                if (entityData.encounterStartTime == 0L)
                {
                    entityData.encounterStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                }
                // If we can not find the list log a warning and exit
                if (entityData.debuffData == null)
                {
                    // update the debuff list to be empty
                    gDebuffPanel.ClearPanel();
                    return;
                }

                // If we are a refresh or a buff that was applied in the past but has since expired
                bool found = false;
                foreach (DebuffData debuff in entityData.debuffData)
                {
                    if (debuff.debuffName == buff.BuffData.DisplayName.ToString())
                    {
                        found = true;
                        debuff.debuffDurationRemaining = buff.BuffData.Duration;
                        //MelonLogger.Warning($"OnAddOrRefreshDebuff() 1 UpdateDebuffPanel entityData.monsterNetworkId = {entityData.monsterNetworkId}, buff.Target.NetworkId.ToString() = { buff.Target.NetworkId.ToString()}, gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");
                        gDebuffPanel.ClearPanel();
                        gDebuffPanel.UpdatePanel(entityData);
                        //gDebuffPanel.UpdateDebuffPanel(testData);

                    }
                }

                // This is not a refresh and it is not a previously applied buff that has expired
                if (found != true)
                {
                    // We do not have a debuff of this type in the list, make a new one
                    DebuffData newDebuff = new DebuffData();
                    // Sometimes Caster is null when it should not be, maybe they have logged out? Maybe its a bug in the game code?
                    if (buff.Caster != null && buff.Caster.Nameplate != null)
                    {
                        newDebuff.casterName = buff.Caster.Nameplate.nameText.text;
                        newDebuff.casterNetworkId = buff.Caster.NetworkId.ToString();
                    }
                    newDebuff.targetName = buff.Target.Nameplate.nameText.text;
                    newDebuff.targetNetworkId = buff.Target.NetworkId.ToString();
                    newDebuff.targetClass = buff.Target.Info.Class.ToString();
                    newDebuff.targetKind = buff.Target.Info.Kind.ToString();
                    newDebuff.debuffName = buff.BuffData.DisplayName.ToString();
                    newDebuff.debuffType = buff.BuffData.CategoryType.ToString(); // Not especially useful but its something at least
                    newDebuff.debuffDuration = buff.BuffData.Duration;
                    newDebuff.debuffDurationRemaining = buff.BuffData.Duration;
                    newDebuff.debuffDescription = buff.BuffData.Description.ToString();
                    newDebuff.numStacks = buff.StackCount;
                    newDebuff.maxStacks = buff.BuffData.MaxStacks;
                    newDebuff.numTicks = buff.BuffData.Ticks;
                    newDebuff.tickIntervalS = buff.BuffData.TickInterval;
                    newDebuff.spellType = buff.CreatedByAbility.SpellType;

                    for (int j = 0; j < buff.BuffData.EntityStatus.Count; j++)
                    {
                        newDebuff.entityStatus = buff.BuffData.EntityStatus[j].ToString();
                    }

                    //MelonLogger.Warning($"OnAddOrRefreshBuff() 10 newDebuff.debuffName = buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()} ");
                    //MelonLogger.Warning($"OnAddOrRefreshBuff() 10 buff.CreatedByAbility.AbilityType.AsString() = {buff.CreatedByAbility.AbilityType.AsString()} ");
                    //MelonLogger.Warning($"OnAddOrRefreshBuff() 10 buff.CreatedByAbility.ActionType.ToString() = {buff.CreatedByAbility.ActionType.ToString()} ");
                    //MelonLogger.Warning($"OnAddOrRefreshBuff() 10 buff.CreatedByAbility.SpellType.ToString() = {buff.CreatedByAbility.SpellType.ToString()} ");
                    //MelonLogger.Warning($"OnAddOrRefreshBuff() 10 buff.CreatedByAbility.AbilitySchool.ToString() = {buff.CreatedByAbility.AbilitySchool.ToString()} ");
                    //MelonLogger.Warning($"OnAddOrRefreshBuff() 10 buff.CreatedByAbility.CastType.ToString() = {buff.CreatedByAbility.CastType.ToString()} ");

                    entityData.debuffData.Add(newDebuff);
                    EntityManager.AddConsolidatedUptime(buff.Target.NetworkId.ToString(), newDebuff);

                    // Update the debuff list, dont update the display if this debuf isnt for the active target
                    if (gCurrentTargetNetworkId.Equals(buff.Target?.NetworkId.ToString()))
                    {
                        //MelonLogger.Warning($"OnAddOrRefreshDebuff() 2 UpdateDebuffPanel entityData.monsterNetworkId = {entityData.monsterNetworkId}, buff.Target.NetworkId.ToString() = {buff.Target.NetworkId.ToString()}, gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");
                        EntityManager.addMonsterToUniqueDebuffs(buff.Target?.NetworkId.ToString(), newDebuff.debuffName);
                        gDebuffPanel.UpdatePanel(entityData);
                        //gDebuffPanel.UpdateDebuffPanel(testData);
                    }
                }
            }
        }

        // This function is called in the following conditions (at least)
        // 1) When a debuff expires an enemy you already have targetted
        // 2) When a debuff expires an enemy you do not have targetted
        public static void OnRemoveBuff(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
        {
            MelonLogger.Warning($"OnRemoveBuff() 1 buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}, isRefresh = {isRefresh}, inBackground = {moveToBackground}");
            MelonLogger.Warning($"OnRemoveBuff() 1 buff.Target?.Nameplate?.name.ToString() = {buff.Target?.Nameplate?.name.ToString()}");
            // Make sure this is something we want to track
            if (IsValidTarget(buff))
            {
                // Do not process anything in the blacklist
                if (Blacklist.Contains(buff.BuffData?.DisplayName?.ToString()))
                {
                    return;
                }

                // Find the debuff list for this specific enemy
                EntityData entityData = EntityManager.GetEntityData(buff.Target.NetworkId.ToString());

                // If we can not find the list log a warning and exit
                if (entityData.debuffData == null)
                {
                    return;
                }

                // Remove this specific debuff from the list
                for (int i = 0; i < entityData.debuffData.Count; i++)
                {
                    DebuffData debuff = entityData.debuffData[i];
                    // We must remove a specific debuff for a specific target cast by a specific person
                    if ((debuff.casterNetworkId == buff.Caster?.NetworkId.ToString()) && (debuff.targetNetworkId == buff.Target.NetworkId.ToString()) && (debuff.debuffName == buff.BuffData?.DisplayName.ToString()))
                    {
                        // There should be no duplicates
                        // Remove the entry, if something has gone wrong with the list then it might exception
                        try
                        {
                            entityData.debuffData.RemoveAt(i);
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Error($"OnRemoveBuff() - Failed to remove debuff {buff.BuffData?.DisplayName.ToString()} from list");
                        }
                    }
                }

                //MelonLogger.Warning($"OnRemoveBuff() UpdateDebuffPanel entityData.monsterNetworkId = {entityData.monsterNetworkId}");
                gDebuffPanel.ClearPanel();
                gDebuffPanel.UpdatePanel(entityData);
                //gDebuffPanel.UpdateDebuffPanel(testData);
            }
        }

        // This fires on at least the following conditions
        // 1) User selects a new target
        // 2) Current selected moster despawns 
        public static void OffensiveTargetSelected(Targets.Logic targetLogic)
        {
            MelonLogger.Warning($"OffensiveTargetSelected 1 gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");
            if (targetLogic.Offensive == null)
            {
                //MelonLogger.Warning($"OffensiveTargetSelected 2");
                // Either the user has pressed ESC so they are targetting nothing or something has gone wrong somewhere
                gCurrentTargetNetworkId = "";
                gDebuffPanel.ClearPanel();
                return;
            }

            //MelonLogger.Warning($"OffensiveTargetSelected 3 targetLogic.Offensive.NetworkId.ToString() = {targetLogic.Offensive.NetworkId.ToString()}");
            // Identify the new target, make sure we have a row in the dictionary for it, this is an explicit handling of a weakness in the detect of new NPC entities
            MelonLogger.Warning($"OffensiveTargetSelected() 1 targetLogic.Offensive.NetworkId.ToString() = {targetLogic.Offensive.NetworkId.ToString()}");

            
            // Get the entity data
            EntityData entityData = EntityManager.GetEntityData(targetLogic.Offensive.NetworkId.ToString());
            if (entityData == null)
            {
                MelonLogger.Error($"OffensiveTargetSelected 3b");
                EntityManager.AddMonsterIfMissing(targetLogic.Offensive.NetworkId.ToString());
                entityData = EntityManager.GetEntityData(targetLogic.Offensive.NetworkId.ToString());
            }

            //MelonLogger.Warning($"OffensiveTargetSelected 3");
            // Check if we are dead, if we are dead just return, we have nothing to do
            if (entityData.isDead == true)
            {
                //MelonLogger.Warning($"OffensiveTargetSelected 4");
                return;
            }

            //MelonLogger.Warning($"OffensiveTargetSelected 5");
            // Reset the panel, we must do this to clear the window when somebody switches to a new target
            //MelonLogger.Warning($"OffensiveTargetSelected() UpdateDebuffPanel entityData.monsterNetworkId = {entityData.monsterNetworkId}");
            gDebuffPanel.ClearPanel();
            gDebuffPanel.UpdatePanel(entityData);
            //gDebuffPanel.UpdateDebuffPanel(testData);

            // Store this for use in OnUpdate()
            //MelonLogger.Warning($"OffensiveTargetSelected 6 targetLogic.Offensive.NetworkId.ToString() = {targetLogic.Offensive.NetworkId.ToString()}");
            gCurrentTargetNetworkId = targetLogic.Offensive.NetworkId.ToString();
            //MelonLogger.Warning($"OffensiveTargetSelected 7 gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");
        }
    }
}
