using Il2Cpp;
using Il2CppLogicalGraphNodes;
using Il2CppServiceStack.Text;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;


[assembly: MelonInfo(typeof(EnhancedDebuffTracking.ModMain), "EnhancedDebuffTracking", "1.0.0", "Anonymous", null)]
[assembly: MelonGame("Visionary Realms", "Pantheon")]

namespace EnhancedDebuffTracking
{
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
    }


    public class ModMain : MelonMod
    {
        // UI Elements
        private static DebuffPanel gDebuffPanel = new DebuffPanel();
        private static string gCurrentTargetNetworkId = null;
        private const float UpdateInterval = 1.0f; // Update interval in seconds
        private static float _timeSinceLastUpdate;
        private static readonly string[] Blacklist = { "Trait:" };

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
                    // Call the entitiy manager and get it to update all the timers
                    EntityManager.UpdateAllDurationTimers();

                    // If gCurrentTargetNetworkId is null there is no point updating the display
                    if (gCurrentTargetNetworkId != null)
                    {
                        // If we have a valid debuff list for the current target, update the screen
                        List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
                        if (debuffList != null)
                        {
                            gDebuffPanel.UpdateDebuffPanel(debuffList);
                        }
                    }
                    _timeSinceLastUpdate = 0f;
                }
            }
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
            return (buff.Target.Info.AccessLevel.Equals(AccessLevel.None) && buff.BuffData.CategoryType == BuffCategoryType.Harmful) ? true : false;
        }

        // This function is called in the following conditions (at least)
        // Make sure we dont re-add an existing buff to the buff list and you handle all the different conditions it can be called
        public static void OnAddOrRefreshBuff(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
        {
//            MelonLogger.Warning($"OnAddOrRefreshBuff 1 buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}, isRefresh = {isRefresh}, inBackground = {inBackground}, isItemBuff = {isItemBuff}");

            // Make sure we only track debuffs and only on monsters
            if (IsValidTarget(buff))
            {
                // Do not process anything in the blacklist
                if (Blacklist.Contains(buff.BuffData.DisplayName.ToString()))
                {
                    return;
                }
                
                // Get the list for the current enemy
                gCurrentTargetNetworkId = buff.Target.NetworkId.ToString();
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);

                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    // update the debuff list to be empty
                    gDebuffPanel.ResetDebuffPanel();
                    return;
                }

                // We do not have a debuff of this type in the list, make a new one
                DebuffData newDebuff = new DebuffData();
                newDebuff.casterName = buff.Caster.Nameplate.nameText.text;
                newDebuff.casterNetworkId = buff.Caster.NetworkId.ToString();
                newDebuff.targetName = buff.Target.Nameplate.nameText.text;
                newDebuff.targetNetworkId = buff.Target.NetworkId.ToString();
                newDebuff.targetClass = buff.Target.Info.Class.ToString();
                newDebuff.targetKind = buff.Target.Info.Kind.ToString();
                newDebuff.debuffName = buff.BuffData.DisplayName.ToString();
                newDebuff.debuffType = buff.BuffData.CategoryType.ToString(); // Not especially useful but its something at least
                newDebuff.debuffDuration = buff.BuffData.Duration;
                newDebuff.debuffDurationRemaining = buff.BuffData.Duration;
                newDebuff.debuffIconName = buff.BuffData.Icon.IconName.ToString();
                newDebuff.debuffDescription = buff.BuffData.Description.ToString();
                newDebuff.numStacks = buff.StackCount;
                newDebuff.maxStacks = buff.BuffData.MaxStacks;
                newDebuff.numTicks = buff.BuffData.Ticks;
                newDebuff.tickIntervalS = buff.BuffData.TickInterval;

                for (int j = 0; j < buff.BuffData.EntityStatus.Count; j++)
                {
                    newDebuff.entityStatus = buff.BuffData.EntityStatus[j].ToString();
//                    MelonLogger.Warning($"OnAddOrRefreshBuff() buff.BuffData.EntityStatus[j].ToString() = {buff.BuffData.EntityStatus[j].ToString()} ");
                }

//                MelonLogger.Warning($"OnAddOrRefreshBuff() newDebuff.debuffName = buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.AbilityType.AsString() = {buff.CreatedByAbility.AbilityType.AsString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.ActionType.ToString() = {buff.CreatedByAbility.ActionType.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.SpellType.ToString() = {buff.CreatedByAbility.SpellType.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.AbilitySchool.ToString() = {buff.CreatedByAbility.AbilitySchool.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.CastType.ToString() = {buff.CreatedByAbility.CastType.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.cachedRangeData.value.ToString() = {buff.CreatedByAbility.cachedRangeData.value.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.HasteAffectType.ToString() = {buff.CreatedByAbility.HasteAffectType.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.IsTechniqueAbility.toString() = {buff.CreatedByAbility.IsTechniqueAbility().ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.CreatedByAbility.TargetType.ToString() = {buff.CreatedByAbility.TargetType.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.Flags.ToString() = {buff.Flags.ToString()} ");
//                MelonLogger.Warning($"OnAddOrRefreshBuff() buff.BuffData.Description.ToString() = {buff.BuffData.Description.ToString()}");

                // Add this specific debuff
                debuffList.Add(newDebuff);

                // Update the debuff list
                gDebuffPanel.UpdateDebuffPanel(debuffList);
            }
        }

        // This function is called in the following conditions (at least)
        // 1) When a debuff expires an enemy you already have targetted
        // 2) When a debuff expires an enemy you do not have targetted
        public static void OnRemoveBuff(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
        {
//            MelonLogger.Warning($"OnRemoveBuff() 1 buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}, isRefresh = {isRefresh}, inBackground = {moveToBackground}");
            // Make sure this is something we want to track
            if (IsValidTarget(buff))
            {
                // Do not process anything in the blacklist
                if (Blacklist.Contains(buff.BuffData.DisplayName.ToString()))
                {
                    return;
                }

                // Find the debuff lst for this specific enemy
                gCurrentTargetNetworkId = buff.Target.NetworkId.ToString();
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);

                // Reset the debuff list
                gDebuffPanel.ResetDebuffPanel();

                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    // TODO - Get rid of this before releasing it, once released I am finished with this mod so no point filling adding to the log 
                    //MelonLogger.Error($"OnRemoveBuff() unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
                    return;
                }

                // Remove this specific debuff from the list
                for (int i = 0; i < debuffList.Count; i++)
                {
                    DebuffData debuff = debuffList[i];
                    // We must remove a specific debuff for a specific target cast by a specific person
                    if ((debuff.casterNetworkId == buff.Caster.NetworkId.ToString()) && (debuff.targetNetworkId == buff.Target.NetworkId.ToString()) && (debuff.debuffName == buff.BuffData.DisplayName.ToString()))
                    {
//                        MelonLogger.Warning($"OnRemoveBuff() buff.BuffData.DisplayName.ToString() = {buff.BuffData.DisplayName.ToString()}");
//                        MelonLogger.Warning($"OnRemoveBuff() debuff.numStacks = {debuff.numStacks}, debuff.maxStacks = {debuff.maxStacks}");

                        // There should be no duplicates
                        // Remove the entry, if something has gone wrong with the list then it might exception
                        try
                        {
                            // This invalidates debuff, we just deleted ourselves, so DO NOT USE debuff from this point forward
                            debuffList.RemoveAt(i);
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Error($"OnRemoveBuff() - Failed to remove debuff {buff.BuffData?.DisplayName.ToString()} from list");
                        }

                        // Update the debuff list
                        gDebuffPanel.UpdateDebuffPanel(debuffList);

                        // Check the number of debuffs left, if none, reset the panel.  This will also catch dead enemies as Target.Nameplate.isDead is False when this trigger fires and as such is unusable here
                        if (debuffList.Count == 0)
                        {
                            gDebuffPanel.ResetDebuffPanel();
                        }
                        // Only process the first one
                        break;
                    }
                }

                // update the debuff list
                gDebuffPanel.UpdateDebuffPanel(debuffList);
            }
        }

        // This function adds the new debuff panel to the UI
        public static void AddDebuffPanelToUI(UIWindowPanel OffensiveTargetPanel)
        {
            // TODO - Is this good enough, or should we do a Find()?
            Transform iTransform = OffensiveTargetPanel.transform.GetChild(0);
            Transform jTransform = iTransform.transform.GetChild(4);
            // Build the panel, attach it to the offensive target panel
            gDebuffPanel.DisplayPanel(debuffPanelName, UIPanelRoots.Instance.Mid.transform, new Vector2(Globals.PanelWidth, Globals.PanelHeight));
        }

        // Returns is a current target is dead or not
        private static bool CheckIfMonsterIsDead(Pools.Logic Pools)
        {
            // Get the isDead status
            return (Pools.Entity.Nameplate.isDead == true) ? true : false;            
        }

        // Used to tear down all the resources allocated by the panel on logout / character change
        public static void RemoveDebuffPanelFromUI()
        {
            gDebuffPanel.RemovePanel();
        }

        // This fires on at least the following conditions
        // 1) User selects a new target
        // 2) Current selected moster despawns 
        public static void OffensiveTargetSelected(Targets.Logic targetLogic)
        {
//            MelonLogger.Warning($"OffensiveTargetSelected() 1");
            ShowDebuffPanel();
            gDebuffPanel.ResetDebuffPanel();

//            MelonLogger.Warning($"OffensiveTargetSelected() 2");
            // Offensive goes to null when a monster despawns
            if (targetLogic.Offensive == null)
            {
//                MelonLogger.Warning($"OffensiveTargetSelected() 3");
                // Either the user has pressed ESC so they are targetting nothing or something has gone wrong somewhere
                gCurrentTargetNetworkId = null;
                return;
            }

//            MelonLogger.Warning($"OffensiveTargetSelected() 4 targetLogic.Offensive.Nameplate.nameText.text.ToString() = {targetLogic.Offensive.Nameplate.nameText.text.ToString()}");

            // Check if we are dead, if we are clear the panel
            bool isDead = CheckIfMonsterIsDead(targetLogic.Offensive.Pools);
            if (isDead)
            {
//                MelonLogger.Warning($"OffensiveTargetSelected() 5");
                return;
            }

//            MelonLogger.Warning($"OffensiveTargetSelected() 6");
            // Identify the new target, make sure we have a row in the dictionary for it, this is an explicit handling of a weakness in the detect of new NPC entities
            gCurrentTargetNetworkId = targetLogic.Offensive.NetworkId.ToString();
            EntityManager.AddMonsterIfMissing(gCurrentTargetNetworkId);
            List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
            if (debuffList == null)
            {
//                MelonLogger.Warning($"OffensiveTargetSelected() 7");
                return;
            }
//            MelonLogger.Warning($"OffensiveTargetSelected() 8");
            gDebuffPanel.UpdateDebuffPanel(debuffList);
        }
    }
}
