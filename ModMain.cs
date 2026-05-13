using Il2Cpp;
using Il2CppLogicalGraphNodes;
using Il2CppPantheonPersist;
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

        public override void OnInitializeMelon() { }

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

                    EntityManager.UpdateDurationRemaining();
                    // Removed Zombied Debuffs (possibly not needed)
                    //EntityManager.RemoveZombiedDebuffs();

                    // Call the entitiy manager and get it to update the uptime timers
                    //EntityManager.UpdateLocalUpTime();

                    // Call the entitiy manager and get it to update the uptime timers
                    EntityManager.UpdateEncounterUpTime();

//                    MelonLogger.Warning($"OnUpdate() 1b gCurrentTargetNetworkId = {gCurrentTargetNetworkId}");
                    // If gCurrentTargetNetworkId is not populated there is no point updating the display
                    if (!gCurrentTargetNetworkId.Equals(""))
                    {
//                        MelonLogger.Warning($"OnUpdate 2 gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");

                        // Do not update the screen for dead enemies
                        EntityData entityData = EntityManager.GetEntityData(gCurrentTargetNetworkId);
                        // If the current network id despawns whilst targetted, dont try and update anything
                        if (entityData == null)
                        {
                            MelonLogger.Error($"OnUpdate() NO ENTITY DATA IN ONUPDATE gCurrentTargetNetworkId = {gCurrentTargetNetworkId}");
                            gCurrentTargetNetworkId = "";
                            return;
                        }

//                        MelonLogger.Warning($"OnUpdate() 2 entityData.isDead = {entityData.isDead}");
                        // We do not update the screen if the entity is dead
                        if (entityData.isDead == false)
                        {
                            // If we have a valid debuff list for the current target, update the screen
                            gDebuffPanel.UpdateDebuffPanel(entityData);
                        }
                    }
                }
            }
        }

        // This function adds the new debuff panel to the UI
        public static void AddDebuffPanelToUI()
        {
            // Build the panel, attach it to the offensive target panel
            gDebuffPanel.DisplayPanel(debuffPanelName, UIPanelRoots.Instance.Mid.transform, new Vector2(Globals.PanelWidth, Globals.PanelHeight));
        }

        // Used to tear down all the resources allocated by the panel on logout / character change
        public static void RemoveDebuffPanelFromUI()
        {
            gDebuffPanel.RemovePanel();
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

        public static void UpdateEnemyDeadStatus(string networkId, bool isDead)
        {
            EntityManager.UpdateEnemyDeadStatus(networkId, isDead);
        }

        // This function is called in the following conditions (at least)
        // Make sure we dont re-add an existing buff to the buff list and you handle all the different conditions it can be called
        public static void OnAddOrRefreshBuff(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
        {
            //MelonLogger.Warning($"OnAddOrRefreshBuff() 0a buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}, isRefresh = {isRefresh}, inBackground = {inBackground}, isItemBuff = {isItemBuff}");
            //MelonLogger.Warning($"OnAddOrRefreshBuff() 0b buff.Target?.NetworkId.ToString() = {buff.Target?.NetworkId.ToString()}, gCurrentTargetNetworkId = {gCurrentTargetNetworkId}");

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
                    MelonLogger.Error($"OnAddOrRefreshBuff() NULL ENTRY FOUND FOR Target = {buff.Target?.Nameplate?.nameText.text}, Source = {buff.Caster?.Nameplate?.nameText.text} {buff.BuffData.DisplayName.ToString()}, MAKING NOW. TODO GET RID OF THIS AND SEE IF IT ACTUALLY MATTERS OR WHY THIS EVEN HAPPENS"); // TODO - What happens if we just return here instead and say tough shit
                    return;
//                    EntityManager.AddMonsterIfMissing(buff.Target.NetworkId.ToString());
//                    entityData = EntityManager.GetEntityData(buff.Target.NetworkId.ToString());
//                    entityData.monsterNetworkId = buff.Target.NetworkId.ToString();
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
                    gDebuffPanel.ResetDebuffPanel();
                    return;
                }

                // If we are a refresh or a buff that was applied in the past but has since expired
                bool found = false;
                foreach (DebuffData debuff in entityData.debuffData)
                {
                    if (debuff.debuffName == buff.BuffData.DisplayName.ToString())
                    {
                        //MelonLogger.Warning($"OnAddOrRefreshBuff 1 (isRefresh = {isRefresh}, debuff.debuffName = {debuff.debuffName}, buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}");
                        found = true;
                        debuff.debuffDurationRemaining = buff.BuffData.Duration;
                        gDebuffPanel.ResetDebuffPanel();
                        gDebuffPanel.UpdateDebuffPanel(entityData);

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
                        EntityManager.addMonsterToUniqueDebuffs(buff.Target?.NetworkId.ToString(), newDebuff.debuffName);
                        gDebuffPanel.UpdateDebuffPanel(entityData);
                    }
                }
            }
        }

        // This function is called in the following conditions (at least)
        // 1) When a debuff expires an enemy you already have targetted
        // 2) When a debuff expires an enemy you do not have targetted
        public static void OnRemoveBuff(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
        {
            //MelonLogger.Warning($"OnRemoveBuff() 1 buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}, isRefresh = {isRefresh}, inBackground = {moveToBackground}");
            //MelonLogger.Warning($"OnRemoveBuff() 1 buff.Target?.Nameplate?.name.ToString()() = {buff.Target?.Nameplate?.name.ToString()}");
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

                gDebuffPanel.ResetDebuffPanel();
                gDebuffPanel.UpdateDebuffPanel(entityData);
            }
        }

        // This fires on at least the following conditions
        // 1) User selects a new target
        // 2) Current selected moster despawns 
        public static void OffensiveTargetSelected(Targets.Logic targetLogic)
        {
            //MelonLogger.Warning($"OffensiveTargetSelected 1 gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");
            if (targetLogic.Offensive == null)
            {
                //MelonLogger.Warning($"OffensiveTargetSelected 2");
                // Either the user has pressed ESC so they are targetting nothing or something has gone wrong somewhere
                gCurrentTargetNetworkId = "";
                gDebuffPanel.ResetDebuffPanel();
                return;
            }

            //MelonLogger.Warning($"OffensiveTargetSelected 3 targetLogic.Offensive.NetworkId.ToString() = {targetLogic.Offensive.NetworkId.ToString()}");
            // Identify the new target, make sure we have a row in the dictionary for it, this is an explicit handling of a weakness in the detect of new NPC entities
            EntityManager.AddMonsterIfMissing(targetLogic.Offensive.NetworkId.ToString());
            // Get the entity data
            EntityData entityData = EntityManager.GetEntityData(targetLogic.Offensive.NetworkId.ToString());
            if (entityData == null)
            {
                //MelonLogger.Warning($"OffensiveTargetSelected 3b");
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
            gDebuffPanel.ResetDebuffPanel();
            gDebuffPanel.UpdateDebuffPanel(entityData);

            // Store this for use in OnUpdate()
            //MelonLogger.Warning($"OffensiveTargetSelected 6 targetLogic.Offensive.NetworkId.ToString() = {targetLogic.Offensive.NetworkId.ToString()}");
            gCurrentTargetNetworkId = targetLogic.Offensive.NetworkId.ToString();
            //MelonLogger.Warning($"OffensiveTargetSelected 7 gCurrentTargetNetworkId = {gCurrentTargetNetworkId.ToString()}");
        }
    }
}
