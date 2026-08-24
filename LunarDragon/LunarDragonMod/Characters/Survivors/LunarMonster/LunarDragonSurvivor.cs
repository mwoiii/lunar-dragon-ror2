using LunarDragonMod.Characters.Survivors.LunarDragon.Content;
using LunarDragonMod.Modules;
using LunarDragonMod.Modules.Characters;
using LunarDragonMod.Survivors.LunarDragon.Components;
using LunarDragonMod.Survivors.LunarDragon.States;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class LunarDragonSurvivor : SurvivorBase<LunarDragonSurvivor> {
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
            bodyColor = new Color(0.67f, 0.65f, 0.74f),
            sortPosition = 100,

            crosshair = assetBundle.LoadAsset<GameObject>("LunarDragonCrosshair"), // Asset.LoadCrosshair("Standard"),
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
        }

        private void AdditionalBodySetup() {
            AddHitboxes();
            SetupAkBanks();
            bodyPrefab.AddComponent<LunarDragonController>();
            bodyPrefab.GetComponent<Interactor>().maxInteractionDistance = 8f;
            displayPrefab.GetComponent<InstantiatePrefabBehavior>().prefab = LunarDragonAssets.displayEffectPrefab;
        }

        private void SetupAkBanks() {
            AkBank[] banksToLoad = {
                Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Croco.CrocoBody_prefab).WaitForCompletion()?.GetComponent<AkBank>(),
                Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Toolbot.ToolbotBody_prefab).WaitForCompletion()?.GetComponent<AkBank>(),
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
            Prefabs.SetupHitBoxGroup(characterModelObject, "Charge", "BodyHitbox");
        }

        public override void InitializeEntityStateMachines() {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(FloorNormalizedMain), typeof(SpawnState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your LunarDragonStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Jet");
        }

        #region skills
        public override void InitializeSkills() {
            Skills.ClearGenericSkills(bodyPrefab);
            LunarDragonSkills.Init(bodyPrefab);
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

        public override void InitializeCharacterMaster() {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            LunarDragonAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }
    }
}