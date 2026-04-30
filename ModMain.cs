using Il2Cpp;
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
    public class DebuffDataToRender()
    {
        public string casterName;
        public string debuffTargetName;

        public string debuffName;
        public string buffIconName;

        public float debuffDuration;
        public float tickIntervalS;
        public int numTicks;

        public int maxStacks;
        public int currentStacks;
        
    }


    public class ModMain : MelonMod
    {
        // Global to hold the list of all
        public static List<DebuffDataToRender> debuffDataToRenderList = new List<DebuffDataToRender>();
        public static DebuffDataToRender gDebuffDataToRender = new DebuffDataToRender();
        private static GameObject gTextGo = null;
        private static string gLatestName;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public static void UpdateOffensiveTargetPanel(UIWindowPanel OffensiveTargetPanel)
        {
            Transform iTransform = OffensiveTargetPanel.transform.GetChild(0);
            if (iTransform != null)
            {
                Transform jTransform = iTransform.transform.GetChild(4);
                if (jTransform != null)
                {
                    // THIS DOES NOT WORK.  gLatestName is always empty until you right click the enemy then all of a sudden it has the right value in it
                    if (!gLatestName.IsEmpty() && gLatestName != null)
                    {
                        var textComponent = gTextGo.GetComponent<TextMeshProUGUI>();
                        MelonLogger.Warning($"UpdateOffensiveTargetPanel currentText = {textComponent.text}");
                        MelonLogger.Warning($"UpdateOffensiveTargetPanel gLatestName = {gLatestName}");
                        textComponent.text = gLatestName;
                        MelonLogger.Warning($"UpdateOffensiveTargetPanel after update textComponent.text = {textComponent.text}");
                    }
                    
                }
            }
            MelonLogger.Warning($"UpdateOffensiveTargetPanel 7");
        }

        public static void UpdateOffensiveTarget()
        {
            MelonLogger.Warning($"UpdateOffensiveTarget 1");
        }

        public static void DebugOffensiveTargetPanel(UIWindowPanel OffensiveTargetPanel)
        {
            MelonLogger.Warning($"DebugOffensiveTargetPanel OffensiveTargetPanel.transform.childCount = {OffensiveTargetPanel.transform.childCount}");

            for (int i = 0; i < OffensiveTargetPanel.transform.childCount; i++)
            {
                UnityEngine.Transform iTransform = OffensiveTargetPanel.transform.GetChild(i);
                MelonLogger.Warning($"DebugOffensiveTargetPanel: iTransform[{i}].GetName() = {iTransform.GetName()}");
                MelonLogger.Warning($"DebugOffensiveTargetPanel: iTransform[{i}].GetType().ToString() = {iTransform.GetType().ToString()}");

                for (int j = 0; j < iTransform.transform.childCount; j++)
                {
                    UnityEngine.Transform jTransform = iTransform.transform.GetChild(j);
                    MelonLogger.Warning($"DebugOffensiveTargetPanel: jTransform[{i},{j}].GetName() = {jTransform.GetName()}");
                    MelonLogger.Warning($"DebugOffensiveTargetPanel: jTransform[{i},{j}].GetType().ToString() = {jTransform.GetType().ToString()}");

                    // Child 4 is the Debuff bar
                    if (j == 4)
                    {
                        MelonLogger.Warning($"DebugOffensiveTargetPanel: 0,4 jTransform.childCount = {jTransform.childCount}");
                        
                        // Add some text to the debuff bar to prove we can change what is on the screen as we apply buffs
                        var textGo = new GameObject("mopCustomTextGO");
                        gTextGo = textGo;
                        textGo.transform.SetParent(jTransform.transform);
                        var textComponent = textGo.AddComponent<TextMeshProUGUI>();
                        textComponent.text = "Dux Is The Best";
                        textComponent.fontSize = 16;
                        textComponent.alignment = TextAlignmentOptions.Left;

                        var textRect = textGo.GetComponent<RectTransform>();
                        textRect.sizeDelta = new Vector2(100, 20);
                        textRect.anchoredPosition = new Vector2(0, 0);
                        textRect.anchorMin = new Vector2(0f, 0f);
                        textRect.anchorMax = new Vector2(0f, 0f);
                        textRect.pivot = new Vector2(0f, 0f);

                        var debuffs = jTransform.GetComponent<Image>;
                        if (debuffs != null)
                        {
                            MelonLogger.Warning($"DebugOffensiveTargetPanel: debuffs.GetType = {debuffs.GetType() }");
                        }
                        MelonLogger.Warning($"DebugOffensiveTargetPanel: 4 Out");
                    }

                    for (int k = 0; k < jTransform.transform.childCount; k++)
                    {
                        UnityEngine.Transform kTransform = jTransform.transform.GetChild(k);
                        MelonLogger.Warning($"DebugOffensiveTargetPanel: kTransform[{i},{j},{k}].GetName() = {kTransform.GetName()}");
                        MelonLogger.Warning($"DebugOffensiveTargetPanel: kTransform[{i},{j},{k}].GetType().ToString() = {kTransform.GetType().ToString()}");

                        for (int l = 0; l < kTransform.transform.childCount; l++)
                        {
                            UnityEngine.Transform lTransform = kTransform.transform.GetChild(l);
                            MelonLogger.Warning($"DebugOffensiveTargetPanel: lTransform[{i},{j},{k},{l}].GetName() = {lTransform.GetName()}");
                            MelonLogger.Warning($"DebugOffensiveTargetPanel: lTransform[{i},{j},{k},{l}].GetType().ToString() = {lTransform.GetType().ToString()}");
                        }
                    }
                }
            }


            var buffbarArray = OffensiveTargetPanel.GetComponentsInChildren<UIBuffBar>(true);
            MelonLogger.Warning($"DebugOffensiveTargetPanel buffbarArray.Count = {buffbarArray.Count}");
            if ( buffbarArray.Count > 0) {
                foreach (var buff in buffbarArray)
                {
                    MelonLogger.Warning($"DebugOffensiveTargetPanel buff.Count = {buff.Count}");
                }
            }
        }

        public static void handleBuffLogicAdd(double time, ActiveBuff buff, bool putInBackground = false, bool isRefresh = false, bool isItemBuff = false)
        {
            MelonLogger.Warning($"Inside handleBuffLogicAdd");

        }

        public static void HandleBuffLogicMyActiveBuff(double time, ActiveBuff buff)
        {
            MelonLogger.Warning($"Inside HandleBuffLogicMyActiveBuff");
        }

        public static void DebugDefensiveTargetPanel(UIWindowPanel defensiveTargetPanel)
        {
            MelonLogger.Warning($"Inside DebugDefensiveTargetPanel");
        }



        public static void OffensiveTargetSelected(Targets.Logic TargetLogic)
        {
            MelonLogger.Warning($"OffensiveTargetSelected 1");
            MelonLogger.Warning($"OffensiveTargetSelected Debug Open");

            // Entity seems to be my character
            var entity = TargetLogic.Entity;
            MelonLogger.Warning($"OffensiveTargetSelected 2");
            if (entity != null)
            {
                MelonLogger.Warning($"OffensiveTargetSelected 3");
                var statemachine = entity.StateMachine;
                MelonLogger.Warning($"OffensiveTargetSelected 4");
                var entityBuffs = entity.Buffs;

                // HERE ARE MY BUFFS
                var myActiveBuffsList = entityBuffs.myActiveBuffs;
                //                foreach (var buff in myActiveBuffsList)
                //                {
                //                    var caster = buff.Caster;
                //                    var casterName = caster.Nameplate.nameText.text;
                //                    AbilityData castBySpell = buff.CreatedByAbility;
                //                    EntityAction actionType = castBySpell.ActionType;
                //                    MelonLogger.Warning($"myActiveBuffsList casterName = {casterName}");
                //                    MelonLogger.Warning($"myActiveBuffsList castBySpell = {castBySpell }");
                //                    MelonLogger.Warning($"myActiveBuffsList castBySpell.DisplayName = {castBySpell.DisplayName.ToString()}");
                //                    MelonLogger.Warning($"myActiveBuffsList actionType = {actionType}");
                //                    MelonLogger.Warning($"myActiveBuffsList buff.Target.Nameplate.nameText.text = {buff.Target.Nameplate.nameText.text}");
                //                    BuffData buffData = buff.BuffData;
                //                    MelonLogger.Warning($"myActiveBuffsList buffData.DisplayName.ToString() = {buffData.DisplayName.ToString()}");
                //                }
            }

            MelonLogger.Warning($"OffensiveTargetSelected 5");
            IEntity offensive = TargetLogic.Offensive;
            MelonLogger.Warning($"OffensiveTargetSelected 6");
            if (offensive.Buffs != null)
            {
                var activeBuffsOnMe = offensive.Buffs.activeBuffsOnMe;
                MelonLogger.Warning($"OffensiveTargetSelected 7");
                // DEBUFFS ON ENEMY
                foreach (var buff in activeBuffsOnMe)
                {
                    var caster = buff.Caster;
                    var casterName = caster.Nameplate.nameText.text;
                    AbilityData castBySpell = buff.CreatedByAbility;
                    EntityAction actionType = castBySpell.ActionType;
                    MelonLogger.Warning($"activeBuffsOnMe casterName = {casterName}");
                    MelonLogger.Warning($"activeBuffsOnMe castBySpell = {castBySpell}");
                    MelonLogger.Warning($"activeBuffsOnMe castBySpell.DisplayName = {castBySpell.DisplayName.ToString()}");
                    MelonLogger.Warning($"activeBuffsOnMe actionType = {actionType}");
                    MelonLogger.Warning($"activeBuffsOnMe buff.Target.Nameplate.nameText.text = {buff.Target.Nameplate.nameText.text}");
                    BuffData buffData = buff.BuffData;
                    MelonLogger.Warning($"activeBuffsOnMe buffData.DisplayName.ToString() = {buffData.DisplayName.ToString()}");
                    var buffGroupsList = buffData.buffGroups;
                    MelonLogger.Warning($"activeBuffsOnMe buffData.MaxStacks = {buffData.MaxStacks}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.Duration = {buffData.Duration}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.Description = {buffData.Description}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.buffData.Icon.IconName.ToString() = {buffData.Icon.IconName.ToString()}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.TickInterval = {buffData.TickInterval}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.TickOnApply = {buffData.TickOnApply}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.TickOnFinish = {buffData.TickOnFinish}");
                    MelonLogger.Warning($"activeBuffsOnMe buffData.Ticks = {buffData.Ticks}");

                    DebuffDataToRender currentDebuff = new DebuffDataToRender();
                    currentDebuff.buffIconName = buffData.Icon.IconName.ToString();
                    currentDebuff.debuffName = buffData.DisplayName.ToString();
                    currentDebuff.debuffDuration = buffData.Duration;
                    currentDebuff.debuffTargetName = buff.Target.Nameplate.nameText.text;
                    currentDebuff.casterName = casterName;
                    currentDebuff.currentStacks = 0;
                    currentDebuff.debuffDuration = buffData.Duration;
                    currentDebuff.numTicks = buffData.Ticks;
                    currentDebuff.tickIntervalS = buffData.TickInterval;

                    MelonLogger.Warning($"Setting gLatestName to {currentDebuff.debuffName}");
                    gLatestName = currentDebuff.debuffName;

                    debuffDataToRenderList.Add(currentDebuff);
                }

                //            var offensiveABL = offensive.Buffs.myActiveBuffs;
                //            // Enemy Actions
                //            foreach (var buff in offensiveABL)
                //            {
                //                var caster = buff.Caster;
                //                var casterName = caster.Nameplate.nameText.text;
                //                AbilityData castBySpell = buff.CreatedByAbility;
                //                EntityAction actionType = castBySpell.ActionType;
                //                MelonLogger.Warning($"offensiveABL casterName = {casterName}");
                //                MelonLogger.Warning($"offensiveABL castBySpell = {castBySpell }");
                //                MelonLogger.Warning($"offensiveABL castBySpell.DisplayName = {castBySpell.DisplayName.ToString()}");
                //                MelonLogger.Warning($"offensiveABL actionType = {actionType}");
                //                MelonLogger.Warning($"offensiveABL buff.Target.Nameplate.nameText.text = {buff.Target.Nameplate.nameText.text}");
                //                BuffData buffData = buff.BuffData;
                //                MelonLogger.Warning($"offensiveABL buffData.DisplayName.ToString() = {buffData.DisplayName.ToString()}");
                //            }
                //
            }

            MelonLogger.Warning($"OffensiveTargetSelected: offensive.Nameplate.nameText.name.ToString() { offensive.Nameplate.nameText.name.ToString()}");

            for (int i = 0; i < offensive.Transform.childCount; i++)
            {
                UnityEngine.Transform iTransform = offensive.Transform.GetChild(i);
//                MelonLogger.Warning($"OffensiveTargetSelected: iTransform[{i}].GetName() = {iTransform.GetName()}");
//                MelonLogger.Warning($"OffensiveTargetSelected: iTransform[{i}].GetType().ToString() = {iTransform.GetType().ToString()}");

                for (int j = 0; j < iTransform.transform.childCount; j++)
                {
                    UnityEngine.Transform jTransform = iTransform.transform.GetChild(j);
//                    MelonLogger.Warning($"OffensiveTargetSelected: jTransform[{j}].GetName() = {jTransform.GetName()}");
//                    MelonLogger.Warning($"OffensiveTargetSelected: jTransform[{j}].GetType().ToString() = {jTransform.GetType().ToString()}");

                    for (int k = 0; k < jTransform.transform.childCount; k++)
                    {
                        UnityEngine.Transform kTransform = jTransform.transform.GetChild(k);
//                        MelonLogger.Warning($"OffensiveTargetSelected: kTransform[{k}].GetName() = {kTransform.GetName()}");
//                        MelonLogger.Warning($"OffensiveTargetSelected: kTransform[{k}].GetType().ToString() = {kTransform.GetType().ToString()}");
                    }
                }
            }
        }

        public static void BuffLogicDebug(Buffs.Logic BuffLogic)
        {
            MelonLogger.Warning($"BuffLogic Debug Open");
            MelonLogger.Warning($"BuffLogic.GetType() = {BuffLogic.GetType()}");
            var myActiveBuffs = BuffLogic.myActiveBuffs ;
            if (myActiveBuffs != null)
            {
                foreach (var buff in myActiveBuffs)
                {
                    MelonLogger.Warning($" buffTarget = {buff.Target.Nameplate.nameText.text}");
                }
            }
        }

        public static void GroupLogicDebug(Group.Logic GroupLogic)
        {
            MelonLogger.Warning($"GroupLogic Debug Open");
            MelonLogger.Warning($"GroupLogic.GetType() = {GroupLogic.GetType()}");
            ActiveGroup activeGroup = GroupLogic.Current;
            if (activeGroup != null)
            {
                foreach (var member in activeGroup.members)
                {
                    MelonLogger.Warning($"member {member.Name} is leader = {member.IsLeader}");

                }
            }
        }

        // This function increzases the size of debuff icons in the Group/Party frame
        public static void GroupPanelDebug(UIWindowPanel GroupPanel)
        {
            MelonLogger.Warning($"GroupPanelDebug: GroupPanel Debug Open");
            MelonLogger.Warning($"GroupPanelDebug: GroupPanel.GetName() = {GroupPanel.GetName()}");
            MelonLogger.Warning($"GroupPanelDebug: GroupPanel.GetType() = {GroupPanel.GetType()}");
            MelonLogger.Warning($"GroupPanelDebug: GroupPanel.transform.childCount = {GroupPanel.transform.childCount}");

            Transform parent = GroupPanel.transform.parent;
            MelonLogger.Warning($"GroupPanelDebug: parent.Getname() = {parent.GetName() }");
            MelonLogger.Warning($"GroupPanelDebug: parent.transform.childCount = {parent.transform.childCount}");

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                UnityEngine.Transform iTransform = parent.transform.GetChild(i);
                MelonLogger.Warning($"GroupPanelDebug: iTransform[{i}].GetName() = {iTransform.GetName()}");
                MelonLogger.Warning($"GroupPanelDebug: iTransform[{i}].GetType().ToString() = {iTransform.GetType().ToString()}");

                for (int j = 0; j < iTransform.transform.childCount; j++)
                {
                    UnityEngine.Transform jTransform = iTransform.transform.GetChild(j);
                    MelonLogger.Warning($"GroupPanelDebug: jTransform[{j}].GetName() = {jTransform.GetName()}");
                    MelonLogger.Warning($"GroupPanelDebug: jTransform[{j}].GetType().ToString() = {jTransform.GetType().ToString()}");

                    for (int k = 0; k < jTransform.transform.childCount; k++)
                    {
                        UnityEngine.Transform kTransform = jTransform.transform.GetChild(k);
                        MelonLogger.Warning($"GroupPanelDebug: kTransform[{k}].GetName() = {kTransform.GetName()}");
                        MelonLogger.Warning($"GroupPanelDebug: kTransform[{k}].GetType().ToString() = {kTransform.GetType().ToString()}");
                    }
                }
            }
        }

        public static void PlayerDebug(UIWindowPanel PlayerPanel)
        {
            MelonLogger.Warning($"PlayerDebug: PlayerPanel Debug Open");
            MelonLogger.Warning($"PlayerDebug: PlayerPanel.GetName() = {PlayerPanel.GetName()}");
            MelonLogger.Warning($"PlayerDebug: PlayerPanel.GetType() = {PlayerPanel.GetType()}");
            MelonLogger.Warning($"PlayerDebug: PlayerPanel.transform.childCount = {PlayerPanel.transform.childCount}");

            Transform parent = PlayerPanel.transform.parent;
            MelonLogger.Warning($"PlayerDebug: parent.Getname() = {parent.GetName()}");
            MelonLogger.Warning($"PlayerDebug: parent.transform.childCount = {parent.transform.childCount}");

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                UnityEngine.Transform iTransform = parent.transform.GetChild(i);
                MelonLogger.Warning($"PlayerDebug: iTransform[{i}].GetName() = {iTransform.GetName()}");
                MelonLogger.Warning($"PlayerDebug: iTransform[{i}].GetType().ToString() = {iTransform.GetType().ToString()}");

                for (int j = 0; j < iTransform.transform.childCount; j++)
                {
                    UnityEngine.Transform jTransform = iTransform.transform.GetChild(j);
                    MelonLogger.Warning($"PlayerDebug: jTransform[{j}].GetName() = {jTransform.GetName()}");
                    MelonLogger.Warning($"PlayerDebug: jTransform[{j}].GetType().ToString() = {jTransform.GetType().ToString()}");

                    for (int k = 0; k < jTransform.transform.childCount; k++)
                    {
                        UnityEngine.Transform kTransform = jTransform.transform.GetChild(k);
                        MelonLogger.Warning($"PlayerDebug: kTransform[{k}].GetName() = {kTransform.GetName()}");
                        MelonLogger.Warning($"PlayerDebug: kTransform[{k}].GetType().ToString() = {kTransform.GetType().ToString()}");
                    }
                }
            }

        }
    }
}
