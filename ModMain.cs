using Harmony;
using Il2Cpp;
using Il2CppLogicalGraphNodes;
using Il2CppServiceStack;
using Il2CppTMPro;
using MelonLoader;
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
        public int numTicks; // Number of ticks
        public float tickIntervalS; // Interval between Ticks
        public int numStacks; // Number of stacks
        public int maxStacks; // Max stacks
    }


    public class ModMain : MelonMod
    {
        private static GameObject gTextGO = null;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Enhanced Debuff Tracking Initialized.");
        }

        public static void OnAddOrRefreshBuff(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
        {
            // TODO - Update here when we want better tracking of Player Debuffs! This is how we detect them and send them on to the modified UI elements
            // Right now we do not want buffs that go onto players even if those are from monsters to players
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

                // Get the list for the current enemy
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(buff.Target.NetworkId.ToString());

                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    MelonLogger.Warning($"OnAddOrRefreshBuff unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
                    return;
                }
                DebuffData newDebuff = new DebuffData();
                newDebuff.casterName = buff.Caster.Nameplate.nameText.text;
                newDebuff.casterNetworkId = buff.Caster.NetworkId.ToString();
                newDebuff.targetName = buff.Target.Nameplate.nameText.text;
                newDebuff.targetNetworkId = buff.Target.NetworkId.ToString();
                newDebuff.debuffName = buff.BuffData.DisplayName.ToString();
                newDebuff.debuffDuration = buff.BuffData.Duration;
                newDebuff.debuffIconName = buff.BuffData.Icon.IconName.ToString();
                newDebuff.numStacks = 1;
                newDebuff.maxStacks = buff.BuffData.MaxStacks;
                newDebuff.numTicks = buff.BuffData.Ticks;
                newDebuff.tickIntervalS = buff.BuffData.TickInterval;
                // Add this specific debuff
                debuffList.Add(newDebuff);

                // TODO - DEBUG ONLY REMOVE LATER ON
                var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();
                textComponent.text = debuffList.Count.ToString();
            }
        }

        public static void OnRemoveBuff(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
        {
            // TODO - Update here when we want better tracking of Player Debuffs! This is how we detect them and send them on to the modified UI elements
            // Right now we do not want buffs that go onto players even if those are from monsters to players
            if (!buff.Target.Info.AccessLevel.Equals(AccessLevel.Player))
            {
                // Find the debuff lst for this specific enemy
                List<DebuffData> debuffList = EntityManager.GetEnemyDebuffList(buff.Target.NetworkId.ToString());
                // If we can not find the list log a warning and exit
                if (debuffList == null)
                {
                    MelonLogger.Warning($"OnRemoveBuff unable to find debuff list for enemy {buff.Caster.NetworkId.ToString()}");
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
                    MelonLogger.Error($"OnRemoveBuff - Failed to remove debuff {buff.BuffData?.DisplayName.ToString()} from list");
                }

                // TODO - DEBUG ONLY - Remove LATER - update UI with number of debuffs in the list
                var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();
                textComponent.text = debuffList.Count.ToString();
            }
        }

        public static void InitOffensiveTargetPanel(UIWindowPanel OffensiveTargetPanel)
        {
//            MelonLogger.Warning($"DebugOffensiveTargetPanel OffensiveTargetPanel.transform.childCount = {OffensiveTargetPanel.transform.childCount}");

            for (int i = 0; i < OffensiveTargetPanel.transform.childCount; i++)
            {
                UnityEngine.Transform iTransform = OffensiveTargetPanel.transform.GetChild(i);
//                MelonLogger.Warning($"DebugOffensiveTargetPanel: iTransform[{i}].GetName() = {iTransform.GetName()}");
//                MelonLogger.Warning($"DebugOffensiveTargetPanel: iTransform[{i}].GetType().ToString() = {iTransform.GetType().ToString()}");

                for (int j = 0; j < iTransform.transform.childCount; j++)
                {
                    UnityEngine.Transform jTransform = iTransform.transform.GetChild(j);
//                    MelonLogger.Warning($"DebugOffensiveTargetPanel: jTransform[{i},{j}].GetName() = {jTransform.GetName()}");
//                    MelonLogger.Warning($"DebugOffensiveTargetPanel: jTransform[{i},{j}].GetType().ToString() = {jTransform.GetType().ToString()}");

                    // Child 4 is the Debuff bar
                    if (j == 4)
                    {
//                        MelonLogger.Warning($"DebugOffensiveTargetPanel: 0,4 jTransform.childCount = {jTransform.childCount}");
                        
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
                    }

                    for (int k = 0; k < jTransform.transform.childCount; k++)
                    {
                        UnityEngine.Transform kTransform = jTransform.transform.GetChild(k);
//                        MelonLogger.Warning($"InitOffensiveTargetPanel: kTransform[{i},{j},{k}].GetName() = {kTransform.GetName()}");
//                        MelonLogger.Warning($"InitOffensiveTargetPanel: kTransform[{i},{j},{k}].GetType().ToString() = {kTransform.GetType().ToString()}");

                        for (int l = 0; l < kTransform.transform.childCount; l++)
                        {
                            UnityEngine.Transform lTransform = kTransform.transform.GetChild(l);
//                            MelonLogger.Warning($"InitOffensiveTargetPanel: lTransform[{i},{j},{k},{l}].GetName() = {lTransform.GetName()}");
//                            MelonLogger.Warning($"InitOffensiveTargetPanel: lTransform[{i},{j},{k},{l}].GetType().ToString() = {lTransform.GetType().ToString()}");
                        }
                    }
                }
            }
        }

        // TODO - Almost everything in here probably isnt needed but it does tell us where everything is located inside the Offensive Target Panel
        public static void OffensiveTargetSelected(Targets.Logic targetLogic)
        {
            MelonLogger.Warning($"OffensiveTargetSelected()++");

            // Identify the new target, make sure we have a row in the dictionary for it, this is an explicit handling of a weakness in the detect of new NPC entities that misses enemys that are 
            // "in-range" when you load into the game or into a zone as as such do not trigger teh Hook in Entity NPC Game Hooks to cretae their required rows
            EntityManager.AddMonsterIfMissing(targetLogic.Offensive.NetworkId.ToString());

            // Entity seems to be my character
            var entity = targetLogic.Entity;
            if (entity != null)
            {
                var statemachine = entity.StateMachine;
                var entityBuffs = entity.Buffs;

                if (entityBuffs == null )
                {
                    return;
                }
                var myActiveBuffsList = entityBuffs.myActiveBuffs;
                if (myActiveBuffsList == null)
                {
                    return;
                }

                // HERE ARE MY BUFFS
                foreach (var buff in myActiveBuffsList)
                {
                    var caster = buff.Caster;
                    var casterName = caster.Nameplate.nameText.text;
                    AbilityData castBySpell = buff.CreatedByAbility;
                    EntityAction actionType = castBySpell.ActionType;
                    BuffData buffData = buff.BuffData;
                }
            }

            if (targetLogic.Offensive == null)
            {
                return;
            }

            IEntity offensive = targetLogic.Offensive;
            if (offensive != null && offensive?.Buffs != null)
            {
                var activeBuffsOnMe = offensive.Buffs.activeBuffsOnMe;

                // DEBUFFS ON ENEMY
                foreach (var buff in activeBuffsOnMe)
                {
                    var caster = buff.Caster;
                    var casterName = caster.Nameplate.nameText.text;
                    AbilityData castBySpell = buff.CreatedByAbility;
                    if (castBySpell != null)
                    {
                        EntityAction actionType = castBySpell.ActionType;
                    }
                    BuffData buffData = buff.BuffData;
                    var buffGroupsList = buffData.buffGroups;
                    DebuffData currentDebuff = new DebuffData();
                    currentDebuff.debuffIconName = buffData.Icon.IconName.ToString();
                    currentDebuff.debuffName = buffData.DisplayName.ToString();
                    currentDebuff.debuffDuration = buffData.Duration;
                    currentDebuff.targetName = buff.Target.Nameplate.nameText.text;
                    currentDebuff.casterName = casterName;
                    currentDebuff.numStacks = 0;
                    currentDebuff.debuffDuration = buffData.Duration;
                    currentDebuff.numTicks = buffData.Ticks;
                    currentDebuff.tickIntervalS = buffData.TickInterval;
                }


                if (offensive.Buffs == null)
                {
                    return;
                }

                if (offensive.Buffs.myActiveBuffs == null) {
                    return;
                }

                // ENEMY ACTIONS
                var offensiveABL = offensive.Buffs.myActiveBuffs;
                foreach (var buff in offensiveABL)
                {
                    var caster = buff.Caster;
                    if (caster != null)
                    {
                        var casterName = caster.Nameplate.nameText.text;
                        AbilityData castBySpell = buff.CreatedByAbility;
                        if (castBySpell != null)
                        {
                            EntityAction actionType = castBySpell.ActionType;
                            BuffData buffData = buff.BuffData;
                        }
                    }
                    
                }
            }

            for (int i = 0; i < offensive.Transform.childCount; i++)
            {
                UnityEngine.Transform iTransform = offensive.Transform.GetChild(i);
                for (int j = 0; j < iTransform.transform.childCount; j++)
                {
                    UnityEngine.Transform jTransform = iTransform.transform.GetChild(j);
                    for (int k = 0; k < jTransform.transform.childCount; k++)
                    {
                        UnityEngine.Transform kTransform = jTransform.transform.GetChild(k);
                    }
                }
            }
        }
    }
}
