using Il2Cpp;
using Il2CppPantheonPersist;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;


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
        private static string gCurrentTargetNetworkId = null;
        private const float UpdateInterval = 1.0f; // Update interval in seconds
        private static float _timeSinceLastUpdate;

        private static GameObject gTextGO = null;
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

                    // If gCurrentTargetNetworkId is there is no point updating the display
                    if (gCurrentTargetNetworkId != null)
                    {
                        // Update the text values in the panel
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
            MelonLogger.Warning($"OnAddOrRefreshBuff 1");
            // TODO - Update here when we want better tracking of Player Debuffs! This is how we detect them and send them on to the modified UI elements
            // We do not want buffs that go onto players even if those are from monsters to players
            if (!buff.Target.Info.AccessLevel.Equals(AccessLevel.Player))
            {
                // TODO Make this a blacklist
                // Do not process Traits
                if (buff.BuffData.DisplayName.ToString().Contains("Trait:"))
                {
                    return;
                }
                // Get the Text field game object
                var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();

                // Get the list for the current enemy
                gCurrentTargetNetworkId = buff.Target.NetworkId.ToString();
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);

                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    MelonLogger.Error($"OnAddOrRefreshBuff unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
                    // update the debuff list to be empty
                    gDebuffPanel.ResetDebuffPanel();
                    textComponent.text = "0";
                    return;
                }

                // This function is also called on change of offensive target, so we can't assume this is actually a new buff or a refresh of a buff
                // If we are not a refresh and have a the same buff already, do nothing
                if (isRefresh == true)
                {
                    MelonLogger.Warning($"OnAddOrRefreshBuff() REFRESH DETECTED");
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
                        MelonLogger.Warning($"OnAddOrRefreshBuff() TARGET SWITCH DETECTED");
                        textComponent.text = debuffList.Count.ToString();
                        return;
                    }
                }

                MelonLogger.Warning($"OnAddOrRefreshBuff() NEW BUFF DETECTED.");
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
            MelonLogger.Warning($"OnRemoveBuff()");
            // Right now we do not want buffs that go onto players
            if (!buff.Target.Info.AccessLevel.Equals(AccessLevel.Player))
            {
                // TODO Make this a blacklist
                // Do not process Traits
                if (buff.BuffData.DisplayName.ToString().Contains("Trait:"))
                {
                    return;
                }

                // TODO - Handle the consequtive target chance use case here so we dont add the same debuff multiple times
                // Find the debuff lst for this specific enemy
                gCurrentTargetNetworkId = buff.Target.NetworkId.ToString();
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
                var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();

                // Reset the debuff list
                gDebuffPanel.ResetDebuffPanel();
                textComponent.text = "0";

                 // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    MelonLogger.Error($"OnRemoveBuff() unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
                    return;
                }

                // Remove this specific debuff from the list
                for (int i = 0; i < debuffList.Count; i++)
                {
                    DebuffData debuff = debuffList[i];
                    // We must remove a specific debuff for a specific target cast by a specific person
                    if ((debuff.casterNetworkId == buff.Caster.NetworkId.ToString()) && (debuff.targetNetworkId == buff.Target.NetworkId.ToString()) && (debuff.debuffName == buff.BuffData.DisplayName.ToString()))
                    {
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
                        textComponent.text = debuffList.Count.ToString();

                        MelonLogger.Error($"OnRemoveBuff() - debuffList.Count {debuffList.Count}");
                        // Check the number of debuffs left, if none, reset the panel.  This will also catch dead enemies as Target.Nameplate.isDead is False when this trigger fires and as such is unusable here
                        if (debuffList.Count == 0)
                        {
                            gDebuffPanel.ResetDebuffPanel();
                            textComponent.text = "0";
                        }
                        // Only process the first one
                        break;
                    }
                }


                // TODO - DEBUG ONLY - Remove LATER - update UI with number of debuffs in the list
                textComponent.text = debuffList.Count.ToString();
                // update the debuff list
                gDebuffPanel.UpdateDebuffPanel(debuffList);
            }
        }

        // This function adds the new debuff panel to the UI
        public static void AddDebuffPanelToUI(UIWindowPanel OffensiveTargetPanel)
        {
            // TODO - Is this good enough, or should we do a Find()?
            UnityEngine.Transform iTransform = OffensiveTargetPanel.transform.GetChild(0);
            UnityEngine.Transform jTransform = iTransform.transform.GetChild(4);
            // Add some text to the debuff bar to prove we can change what is on the screen as we apply buffs
            GameObject textGO = new GameObject("EDT_CustomTextGO_EDT");
            textGO.transform.SetParent(jTransform.transform);
            TextMeshProUGUI textComponent = textGO.AddComponent<TextMeshProUGUI>();
            textComponent.text = "Dux Is The Best";
            textComponent.fontSize = 16;
            textComponent.alignment = TextAlignmentOptions.Left;
            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(100, 20);
            textRect.anchoredPosition = new Vector2(0, 0);
            textRect.anchorMin = new Vector2(0f, 0f);
            textRect.anchorMax = new Vector2(0f, 0f);
            textRect.pivot = new Vector2(0f, 0f);
            // Update the gloabl GO that holds the text to update (CAN BE DELETED LATER WHEN OUT OF PROOF OF CONCEPT)
            gTextGO = textGO;

            // Build the panel, attach it to the offensive target panel
            gDebuffPanel.DisplayPanel(debuffPanelName, UIPanelRoots.Instance.Mid.transform, new Vector2(Globals.PanelWidth, Globals.PanelHeight));
        }

        // Returns is a current target is dead or not
        private static bool CheckIfMonsterIsDead(Pools.Logic Pools)
        {
            // Get the isDead status
            return (Pools.Entity.Nameplate.isDead == true) ? true : false;            
        }


        // This fires on at least the following conditions
        // 1) User selects a new target
        // 2) Current selected moster despawns 
        public static void OffensiveTargetSelected(Targets.Logic targetLogic)
        {
            MelonLogger.Warning($"OffensiveTargetSelected");
            // TODO - Show the debuff panel right now, maybe remove later
            gDebuffPanel.ShowDebuffPanel();
            gDebuffPanel.ResetDebuffPanel();

            var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();
      
            // Offensive goes to null when a monster despawns
            if (targetLogic.Offensive == null)
            {
                textComponent.text = "0";
                gDebuffPanel.ResetDebuffPanel();
                return;
            }

            // Identify the new target, make sure we have a row in the dictionary for it, this is an explicit handling of a weakness in the detect of new NPC entities
            gCurrentTargetNetworkId = targetLogic.Offensive.NetworkId.ToString();
            EntityManager.AddMonsterIfMissing(gCurrentTargetNetworkId);
            List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(gCurrentTargetNetworkId);
            if (debuffList == null)
            {
                return;
            }

            // This should be zero on a new element, or on a known element should be the correct number of buffs
            textComponent.text = debuffList.Count.ToString();
            gDebuffPanel.UpdateDebuffPanel(debuffList);

            // Check if we are dead, if we are clear the panel
            bool isDead = CheckIfMonsterIsDead(targetLogic.Offensive.Pools);
            if (isDead)
            {
                textComponent.text = "0";
                gDebuffPanel.ResetDebuffPanel();
            }
            
            // Set the debuff count to the actual count
            textComponent.text = debuffList.Count.ToString();
            gDebuffPanel.UpdateDebuffPanel(debuffList);
        }
    }
}
