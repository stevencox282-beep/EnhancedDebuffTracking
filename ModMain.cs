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
        private static GameObject gTextGO = null;
        private static string gLatestName = "";

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public static void OnAddOrRefreshBuff(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
        {
            MelonLogger.Warning($"OnAddOrRefreshBuff()++");
            var textComponent = gTextGO.GetComponent<TextMeshProUGUI>();
            MelonLogger.Warning($"OnAddOrRefreshBuff() Old Value = textComponent = {textComponent.text}");
            textComponent.text = buff.BuffData.DisplayName.ToString();
            MelonLogger.Warning($"OnAddOrRefreshBuff() New Value = textComponent.text {textComponent.text}");
            MelonLogger.Warning($"OnAddOrRefreshBuff()--");
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
                        var textGO = new GameObject("mopCustomTextGO");
                        
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

                        var debuffs = jTransform.GetComponent<Image>;
                        MelonLogger.Warning($"InitOffensiveTargetPanel() setting gTextGOs to textGO");
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

        public static void OffensiveTargetSelected(Targets.Logic TargetLogic)
        {
            MelonLogger.Warning($"OffensiveTargetSelected()++");
            // Entity seems to be my character
            var entity = TargetLogic.Entity;
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

            if (TargetLogic.Offensive == null)
            {
                return;
            }

            IEntity offensive = TargetLogic.Offensive;
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

                    MelonLogger.Warning($"OffensiveTargetSelected() Setting gLatestName to {currentDebuff.debuffName}");
                    gLatestName = currentDebuff.debuffName;

                    debuffDataToRenderList.Add(currentDebuff);
                }


                if (offensive.Buffs == null)
                {
                    return;
                }

                if (offensive.Buffs.myActiveBuffs == null) {
                    return;
                }

                // Enemy Actions
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
