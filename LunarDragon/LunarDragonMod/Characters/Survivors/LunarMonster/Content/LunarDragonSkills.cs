using EntityStates;
using LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates;
using LunarDragonMod.Modules;
using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.SkillStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.Content {
    public static class LunarDragonSkills {

        private static GameObject bodyPrefab;

        private static AssetBundle assetBundle;

        private const string LUNAR_DRAGON_PREFIX = LunarDragonSurvivor.LUNAR_DRAGON_PREFIX;

        public static void Init(GameObject bodyPrefab) {
            LunarDragonSkills.bodyPrefab = bodyPrefab;
            assetBundle = LunarDragonAssets.assetBundle;
            //AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            AddSpecialSkills();
        }

        private static void AddPassiveSkill() {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill {
                enabled = true,
                skillNameToken = LUNAR_DRAGON_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_STUNNING",
                icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonPassive",
                skillNameToken = LUNAR_DRAGON_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon")
            });
            Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        private static void AddPrimarySkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "LunarDragonElementalBlitz",
                    LUNAR_DRAGON_PREFIX + "PRIMARY_ELEMENTAL_BLITZ_NAME",
                    LUNAR_DRAGON_PREFIX + "PRIMARY_ELEMENTAL_BLITZ_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimary1Icon"),
                    new SerializableEntityStateType(typeof(ElementalBlitz)),
                    "Weapon",
                    false
                ));

            primarySkillDef1.stepCount = 3;
            primarySkillDef1.stepGraceDuration = 1f;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private static void AddSecondarySkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonEruption",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SECONDARY_ERUPTION_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SECONDARY_ERUPTION_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondary1Icon"),

                activationState = new SerializableEntityStateType(typeof(Eruption)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 12f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            SkillDef secondarySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonSurge",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SECONDARY_SURGE_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SECONDARY_SURGE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texWIPIcon"),

                activationState = new SerializableEntityStateType(typeof(Surge)),
                activationStateMachineName = "Weapon2",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            SkillDef secondarySkillDef3 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonGlaciate",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SECONDARY_GLACIATE_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SECONDARY_GLACIATE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texWIPIcon"),

                activationState = new SerializableEntityStateType(typeof(Glaciate)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1, secondarySkillDef2, secondarySkillDef3);
            Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().secondary.skillFamily, null, LunarDragonUnlockables.wipSkillUnlockableDef, LunarDragonUnlockables.wipSkillUnlockableDef);
        }

        private static void AddUtilitySkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonBurstThrusters",
                skillNameToken = LUNAR_DRAGON_PREFIX + "UTILITY_BURST_THRUSTERS_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "UTILITY_BURST_THRUSTERS_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtility1Icon"),

                activationState = new SerializableEntityStateType(typeof(BurstThrustersCharge)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.PrioritySkill,

                baseRechargeInterval = 2f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            SkillDef utilitySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonFlowThrusters",
                skillNameToken = LUNAR_DRAGON_PREFIX + "UTILITY_FLOW_THRUSTERS_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "UTILITY_FLOW_THRUSTERS_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texWIPIcon"),

                activationState = new SerializableEntityStateType(typeof(FlowThrusters)),
                activationStateMachineName = "Body",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 7f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1, utilitySkillDef2);
            Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().utility.skillFamily, null, LunarDragonUnlockables.wipSkillUnlockableDef);
        }

        private static void AddSpecialSkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonAmbush",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SPECIAL_AMBUSH_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SPECIAL_AMBUSH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new SerializableEntityStateType(typeof(DracoAmbushAscent)),
                activationStateMachineName = "Body",
                interruptPriority = InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            SkillDef specialSkillDef2 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonRailgun",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SPECIAL_AMBUSH_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SPECIAL_AMBUSH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texWIPIcon"),

                activationState = new SerializableEntityStateType(typeof(DracoAmbushAscent)),
                activationStateMachineName = "Body",
                interruptPriority = InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1, specialSkillDef2);
            Skills.AddUnlockablesToFamily(bodyPrefab.GetComponent<SkillLocator>().special.skillFamily, null, LunarDragonUnlockables.wipSkillUnlockableDef);
        }
    }
}
