using Harmony;
using Il2Cpp;
using Il2CppLogicalGraphNodes;
using Il2CppPantheonPersist;
using Il2CppServiceStack;
using Il2CppServiceStack.Text;
using Il2CppTMPro;
using MelonLoader;
using Microsoft.VisualBasic;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: MelonInfo(typeof(EnhancedDebuffTracking.ModMain), "EnhancedDebuffTracking", "1.0.0", "Anonymous", null)]
[assembly: MelonGame("Visionary Realms", "Pantheon")]

namespace EnhancedDebuffTracking
{
    // This class will be used to store all the information required to display the debuff data in the debuff panel
    public class DebuffData()
    {
        public string casterName; // Nameplate name of the caster
        public string casterNetworkId; // Unique ID of the caster
        public string targetName; // Nameplate name of the target
        public string targetNetworkId; // Unique ID of the target

        public string debuffName; // Base name of the debuff
        public string debuffIconName; // Debuff Icon

        public float debuffDuration; // Debuff duration
        public float debuffDurationRemaining; // Used in the panel to keep track of remaining duration
        public int   numTicks; // Number of ticks
        public float tickIntervalS; // Interval between Ticks
        public int numStacks; // Number of stacks
        public int maxStacks; // Max stacks
    }


    public class ModMain : MelonMod
    {
        // UI Elements
        private static DebuffPanel gDebuffPanel = new DebuffPanel();
        private static List<DebuffData> EMPTY_LIST = new List<DebuffData>();
        private static string gCurrentTargetNetworkId = null;
        private const float UpdateInterval = 1.0f; // Update interval in seconds
        private static float _timeSinceLastUpdate;

        private static GameObject gTextGO = null;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Enhanced Debuff Tracking Initialized.");
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
                if( (_timeSinceLastUpdate >= UpdateInterval) && (gCurrentTargetNetworkId != null))
                {
                    // Call the entitiy manager and get it to update all the timers
                    EntityManager.UpdateAllDurationTimers();

                    // Update the text values in the panel
                    List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
                    if (debuffList != null)
                    {
                        gDebuffPanel.UpdateDebuffPanel(debuffList);
                    }
                    _timeSinceLastUpdate = 0f;
                }
            }
        }

        // Called on hiding of the offensive target frame
        public static void HideDebuffPanel()
        {
            gDebuffPanel.HideDebuffPanel();
        }

        // This function is called in the following conditions (at least)
        // 1) When you add a buff to an enemy you already have targetted
        // 2) A buff expires on an enemy you already have targetted
        // 3) When you change target to a new target that already has debuffs on it
        // Make sure we dont re-add an existing buff to the buff list and you handle all the different conditions it can be called
        public static void OnAddOrRefreshBuff(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
        {
            // TODO - Update here when we want better tracking of Player Debuffs! This is how we detect them and send them on to the modified UI elements
            // We do not want buffs that go onto players even if those are from monsters to players
            if (!buff.Target.Info.AccessLevel.Equals(AccessLevel.Player))
            {
                MelonLogger.Warning($"==================");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.ObjectClass = {buff.Target.ObjectClass }");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Nameplate.isDead = {buff.Target.Nameplate.isDead }");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Nameplate.nameText.text = {buff.Target.Nameplate.nameText.text}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Currency.Current.Total.ToString() = {buff.Target.Currency.Current.Total.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Info.Class.ToString() = {buff.Target.Info.Class.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Info.AccountName.ToString() = {buff.Target.Info.AccountName.ToString() }");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Info.Kind.ToString() = {buff.Target.Info.Kind.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Info.AccessLevel.ToString() = {buff.Target.Info.AccessLevel.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Caster.NetworkView.NetworkId.ToString() = {buff.Caster.NetworkView.NetworkId.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Caster.Info.CharacterId = {buff.Caster.Info.CharacterId.ToString() }");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.Target.Info.CharacterId = {buff.Target.Info.CharacterId.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.buff.Caster.NetworkId = {buff.Caster.NetworkId}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.buff.Target.NetworkId = {buff.Target.NetworkId}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.buff.Caster.NetworkId.Tostring() = {buff.Caster.NetworkId.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.buff.Target.NetworkId.ToString() = {buff.Target.NetworkId.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.BuffData.Id.ToString() = {buff.BuffData.Id.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.BuffData.DisplayName.ToString(); = {buff.BuffData.DisplayName.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff buff.BuffData.DesignerId.ToString(); = {buff.BuffData.DesignerId.ToString()}");
                MelonLogger.Warning($"OnAddOrRefreshBuff inBackground; = {inBackground}");
                MelonLogger.Warning($"OnAddOrRefreshBuff isRefresh = {isRefresh}");

                // Get the Text field game object
                var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();

                // Get the list for the current enemy
                gCurrentTargetNetworkId = buff.Target.NetworkId.ToString();
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);

                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    MelonLogger.Warning($"OnAddOrRefreshBuff unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
                    // update the debuff list to be empty
                    gDebuffPanel.UpdateDebuffPanel(EMPTY_LIST);
                    textComponent.text = "0";
                    return;
                }

                // This function is also called on change of offensive target, so we can't assume this is actually a new buff or a refresh of a buff
                // If we are not a refresh and have a the same buff already, do nothing
                if (isRefresh == true)
                {
                    MelonLogger.Warning($"OnAddOrRefreshBuff() REFRESH DETECTED, DOING NOTHING");
                    textComponent.text = debuffList.Count.ToString();
                    // update the debuff list
                    gDebuffPanel.UpdateDebuffPanel(debuffList);
                    return;
                }

                // TODO - Make this check a common function we do it multiple times 
                foreach (var item in debuffList)
                {
                    if ((item.casterNetworkId == buff.Caster.NetworkId.ToString()) && (item.targetNetworkId == buff.Target.NetworkId.ToString()) && (item.debuffName == buff.BuffData.DisplayName.ToString()))
                    {
                        MelonLogger.Warning($"OnAddOrRefreshBuff() TARGET SWITCH DETECTED, DOING NOTHING");
                        textComponent.text = debuffList.Count.ToString();
                        return;
                    }
                }

                MelonLogger.Warning($"OnAddOrRefreshBuff() NEW BUFF DETECTED.  CONTINUING");
                DebuffData newDebuff = new DebuffData();
                newDebuff.casterName = buff.Caster.Nameplate.nameText.text;
                newDebuff.casterNetworkId = buff.Caster.NetworkId.ToString();
                newDebuff.targetName = buff.Target.Nameplate.nameText.text;
                newDebuff.targetNetworkId = buff.Target.NetworkId.ToString();
                newDebuff.debuffName = buff.BuffData.DisplayName.ToString();
                newDebuff.debuffDuration = buff.BuffData.Duration;
                newDebuff.debuffDurationRemaining = buff.BuffData.Duration;
                newDebuff.debuffIconName = buff.BuffData.Icon.IconName.ToString();
                newDebuff.numStacks = 1;
                newDebuff.maxStacks = buff.BuffData.MaxStacks;
                newDebuff.numTicks = buff.BuffData.Ticks;
                newDebuff.tickIntervalS = buff.BuffData.TickInterval;
                // Add this specific debuff
                debuffList.Add(newDebuff);

                //  Set it to the real number
                textComponent.text = debuffList.Count.ToString();
                // update the debuff list
                gDebuffPanel.UpdateDebuffPanel(debuffList);
            }
        }

        // This function is called in the following conditions (at least)
        // 1) When a debuff expires an enemy you already have targetted
        // 2) When a debuff expires an enemy you do not have targetted
        public static void OnRemoveBuff(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
        {
            // Right now we do not want buffs that go onto players even if those are from monsters to players
            if (!buff.Target.Info.AccessLevel.Equals(AccessLevel.Player))
            {
                // TODO - Handle the consequtive target chance use case here so we dont add the same debuff multiple times
                // Find the debuff lst for this specific enemy
                gCurrentTargetNetworkId = buff.Target.NetworkId.ToString();
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
                var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();
                
                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    MelonLogger.Warning($"OnRemoveBuff() unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
                    textComponent.text = "0";
                    // update the debuff list
                    gDebuffPanel.UpdateDebuffPanel(EMPTY_LIST);
                    return;
                }

                // Remove this specific debuff from the list
                int index = 0;
                foreach (var debuff in debuffList)
                {
                    // We must remove a specific debuff for a specific target cast by a specific person
                    if((debuff.casterNetworkId == buff.Caster.NetworkId.ToString()) && (debuff.targetNetworkId == buff.Target.NetworkId.ToString()) && (debuff.debuffName == buff.BuffData.DisplayName.ToString()))
                    {
                        // We must exift from the loop before we remove the entry as we can not change the size of the list we are currently iterating over
                        textComponent.text = debuffList.Count.ToString();
                        // update the debuff list
                        gDebuffPanel.UpdateDebuffPanel(debuffList);
                        break;
                    }
                    index++;
                }
                // Remove the entry, if something has gone wrong with the list then it might exception
                try
                {
                    debuffList.RemoveAt(index);
                }
                catch (Exception e)
                {
                    MelonLogger.Error($"OnRemoveBuff() - Failed to remove debuff {buff.BuffData?.DisplayName.ToString()} from list");
                    textComponent.text = debuffList.Count.ToString();
                }

                // TODO - DEBUG ONLY - Remove LATER - update UI with number of debuffs in the list
                textComponent.text = debuffList.Count.ToString();
                // update the debuff list
                gDebuffPanel.UpdateDebuffPanel(debuffList);
            }
        }

        public static void InitOffensiveTargetPanel(UIWindowPanel OffensiveTargetPanel)
        {
            MelonLogger.Warning($"InitOffensiveTargetPanel()++");
            UnityEngine.Transform iTransform = OffensiveTargetPanel.transform.GetChild(0);
            UnityEngine.Transform jTransform = iTransform.transform.GetChild(4);
            // Add some text to the debuff bar to prove we can change what is on the screen as we apply buffs
            var textGO = new GameObject("EDT_CustomTextGO_EDT");
            textGO.transform.SetParent(jTransform.transform);
            var textComponent = textGO.AddComponent<TextMeshProUGUI>();
            textComponent.text = "Dux Is The Best";
            textComponent.fontSize = 16;
            textComponent.alignment = TextAlignmentOptions.Left;
            var textRect = textGO.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(100, 20);
            textRect.anchoredPosition = new Vector2(0, 0);
            textRect.anchorMin = new Vector2(0f, 0f);
            textRect.anchorMax = new Vector2(0f, 0f);
            textRect.pivot = new Vector2(0f, 0f);
            // Update the gloabl GO that holds the text to update (CAN BE DELETED LATER WHEN OUT OF PROOF OF CONCEPT)
            gTextGO = textGO;

            // Build the panel, attach it to the offensive target panel
            gDebuffPanel.DisplayPanel("EDT_DebuffPanel_EDT", UIPanelRoots.Instance.Mid.transform, new Vector2(Globals.PanelWidth, Globals.PanelMeshHeight));

            MelonLogger.Warning($"InitOffensiveTargetPanel()--");
        }

        // This fires on at least the following conditions
        // 1) User selects a new target
        // 2) Current selected moster despawns 
        public static void OffensiveTargetSelected(Targets.Logic targetLogic)
        {
            MelonLogger.Warning($"OffensiveTargetSelected");
            // always show the panel on target change
            gDebuffPanel.ShowDebuffPanel();
            gDebuffPanel.UpdateDebuffPanel(EMPTY_LIST);


            var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();
      
            // Offensive gpoes to null when a monster despawns
            if (targetLogic.Offensive == null)
            {
                // Set to zero, this covers off the case where a monster dies that you are not targetting but you put debuffs on it
                textComponent.text = "0";
                gDebuffPanel.UpdateDebuffPanel(EMPTY_LIST);
                return;
            }

            // Identify the new target, make sure we have a row in the dictionary for it, this is an explicit handling of a weakness in the detect of new NPC entities that sometimes misses entries
            EntityManager.AddMonsterIfMissing(targetLogic.Offensive.NetworkId.ToString());
            gCurrentTargetNetworkId = targetLogic.Offensive.NetworkId.ToString();
            List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
            if (debuffList == null)
            {
                return;
            }

            // This should be zero on a new element, or a known element should be the correct number of buffs
            textComponent.text = debuffList.Count.ToString();
            gDebuffPanel.UpdateDebuffPanel(debuffList);


            // For reasons unclear, sometimes pantheon provides a valid offensive target with a null Pools, nothing to do but just return
            if (targetLogic.Offensive.Pools == null)
            {
                return;
            }
            // For reasons unclear, sometimes pantheon provides a valid offensive target with a null Pools, nothing to do but just return
            if (targetLogic.Offensive.Pools.pools == null)
            {
                return;
            }

            // I can not find a isDead property in TargetLogic, so check the HP bar and if it is zero it must be dead
            var pools = targetLogic.Offensive.Pools.pools;
            foreach (Pool pool in pools)
            {
                // For reasons unclear, sometimes pantheon provides a valid offensive target with a null Pools, nothing to do but just return
                if (pool == null) {
                    return;
                }
                if (pool.InternalPoolType.Equals(PoolType.Health))
                {
                    if (pool.Current.Equals(0f))
                    {
                        textComponent.text = "0";
                        gDebuffPanel.UpdateDebuffPanel(EMPTY_LIST);
                        return;
                    }
                }
            }

            // Set the debuff count to the actual count
            textComponent.text = debuffList.Count.ToString();
            gDebuffPanel.UpdateDebuffPanel(debuffList);
        }
    }
}
