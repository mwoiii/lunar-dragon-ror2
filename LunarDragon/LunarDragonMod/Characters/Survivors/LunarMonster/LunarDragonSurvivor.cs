using LunarDragonMod.Characters.Survivors.LunarMonster.States;
using LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates;
using LunarDragonMod.Modules;
using LunarDragonMod.Modules.Characters;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using RoR2.Skills;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class LunarMonsterSurvivor : SurvivorBase<LunarMonsterSurvivor> {
        //used to load the assetbundle for this character. must be unique

        public override string bodyName => "LunarDragonBody";

        public override string masterName => "LunarDragonMonsterMaster";

        public override string modelPrefabName => "mdlLunarDragon";

        public override string displayPrefabName => "LunarDragonDisplay";

        public const string LUNAR_DRAGON_PREFIX = LunarDragonPlugin.DEVELOPER_PREFIX + "_LunarDragon_";

        public override string survivorTokenPrefix => LUNAR_DRAGON_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo {
            bodyName = bodyName,
            bodyNameToken = LUNAR_DRAGON_PREFIX + "NAME",
            subtitleNameToken = LUNAR_DRAGON_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texLunarDragonIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 180f,
            healthRegen = 1f,
            armor = 20f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "BodyMesh",
                    material = assetBundle.LoadMaterial("matBody")
                },
                new CustomRendererInfo
                {
                    childName = "LimbsMesh",
                    material = assetBundle.LoadMaterial("matLimbs"),
                },
                new CustomRendererInfo
                {
                    childName = "LeftOrbMesh",
                    material = assetBundle.LoadMaterial("matOrbLeft"),
                },
                new CustomRendererInfo
                {
                    childName = "RightOrbMesh",
                    material = assetBundle.LoadMaterial("matOrbRight"),
                },
                new CustomRendererInfo
                {
                    childName = "CenterOrbMesh",
                    material = assetBundle.LoadMaterial("matOrbCenter"),
                },
                new CustomRendererInfo
                {
                    childName = "RocksMesh",
                    material = assetBundle.LoadMaterial("matCannon"),
                }
        };

        public override UnlockableDef characterUnlockableDef => LunarDragonUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => new LunarDragonItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Init() {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "LunarDragon");

            //if (!characterEnabled.Value)
            //    return;

            base.Init();
        }

        public override void InitializeCharacter() {
            LunarDragonAssets.Init();
            assetBundle = LunarDragonAssets.assetBundle;

            LunarDragonUnlockables.Init();

            base.InitializeCharacter();

            LunarDragonConfig.Init();
            LunarDragonStates.Init();
            LunarDragonTokens.Init();
            LunarDragonBuffs.Init(assetBundle);

            SetDeathBehaviour();
            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup() {
            AddHitboxes();
            bodyPrefab.AddComponent<LunarDragonController>();
            SetupAkBanks();
        }

        private void SetupAkBanks() {
            AkBank[] banksToLoad = {
                Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Croco.CrocoBody_prefab).WaitForCompletion()?.GetComponent<AkBank>(),
                Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageBody_prefab).WaitForCompletion()?.GetComponent<AkBank>(),
                Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_FalseSon.FalseSonBody_prefab).WaitForCompletion()?.GetComponent<AkBank>(),
            };
            foreach (AkBank bank in banksToLoad) {
                if (bank != null) {
                    AkBank akBank = bodyPrefab.AddComponent<AkBank>();
                    akBank.triggerList = bank.triggerList;
                    akBank.data.WwiseObjectReference = bank.data.WwiseObjectReference;
                    akBank.unloadTriggerList = bank.unloadTriggerList;
                }
            }
        }

        private void SetDeathBehaviour() {
            CharacterDeathBehavior deathBehavior = bodyPrefab.GetComponent<CharacterDeathBehavior>();
            if (deathBehavior == null) {
                deathBehavior = bodyPrefab.AddComponent<CharacterDeathBehavior>();
            }

            deathBehavior.deathState = new EntityStates.SerializableEntityStateType(typeof(DeathState));

        }

        public void AddHitboxes() {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "Charge", "BodyHitbox");
        }

        public override void InitializeEntityStateMachines() {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(MainState), typeof(SpawnState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your LunarDragonStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Jet");
        }

        #region skills
        public override void InitializeSkills() {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            //AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill() {
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
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "LunarDragonElementalBlitz",
                    LUNAR_DRAGON_PREFIX + "PRIMARY_ELEMENTAL_BLITZ_NAME",
                    LUNAR_DRAGON_PREFIX + "PRIMARY_ELEMENTAL_BLITZ_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(ElementalBlitz)),
                    "Weapon",
                    false
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 3;
            primarySkillDef1.stepGraceDuration = 1f;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonEruption",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SECONDARY_ERUPTION_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SECONDARY_ERUPTION_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Eruption)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

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

            SkillDef secondarySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonSurge",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SECONDARY_SURGE_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SECONDARY_SURGE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Surge)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

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
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Glaciate)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

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
        }

        private void AddUtilitySkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonBurstThrusters",
                skillNameToken = LUNAR_DRAGON_PREFIX + "UTILITY_BURST_THRUSTERS_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "UTILITY_BURST_THRUSTERS_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(BurstThrustersCharge)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

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
                forceSprintDuringState = true,
            });

            SkillDef utilitySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonFlowThrusters",
                skillNameToken = LUNAR_DRAGON_PREFIX + "UTILITY_FLOW_THRUSTERS_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "UTILITY_FLOW_THRUSTERS_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(FlowThrusters)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

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
        }

        private void AddSpecialSkills() {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo {
                skillName = "LunarDragonBomb",
                skillNameToken = LUNAR_DRAGON_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = LUNAR_DRAGON_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills

        #region skins
        public override void InitializeSkins() {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texDefaultSkinIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin

            SkinDef masterySkin = Skins.CreateSkinDef(LUNAR_DRAGON_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texMasterySkinIcon"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                LunarDragonUnlockables.masterySkinUnlockableDef);

            masterySkin.skinDefParams.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matBodyMastery");
            masterySkin.skinDefParams.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matLimbsMastery");
            masterySkin.skinDefParams.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matOrbLeftMastery");
            masterySkin.skinDefParams.rendererInfos[3].defaultMaterial = assetBundle.LoadMaterial("matOrbRightMastery");
            masterySkin.skinDefParams.rendererInfos[4].defaultMaterial = assetBundle.LoadMaterial("matOrbCenterMastery");
            masterySkin.skinDefParams.rendererInfos[5].defaultMaterial = assetBundle.LoadMaterial("matCannonMastery");

            skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster() {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            LunarDragonAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks() {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {

            if (sender.HasBuff(LunarDragonBuffs.armorBuff)) {
                args.armorAdd += 300;
            }
        }
    }
}