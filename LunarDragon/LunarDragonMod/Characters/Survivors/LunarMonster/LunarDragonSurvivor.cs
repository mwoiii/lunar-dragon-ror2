using LunarDragonMod.Modules.Characters;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class LunarDragonSurvivor : SurvivorBase<LunarDragonSurvivor> {
        public override string assetBundleName => "mwmwlunardragonbundle";

        public override string bodyName => "LunarDragonBody";

        private static string _bodyName;

        public override string masterName => "LunarDragonMonsterMaster";

        public override string displayPrefabName => "LunarDragonDisplay";

        public const string LUNAR_DRAGON_PREFIX = LunarDragonPlugin.DEVELOPER_PREFIX + "_LUNAR_DRAGON_";

        public override string survivorTokenPrefix => LUNAR_DRAGON_PREFIX;

        public override ItemDisplaysBase itemDisplays => new LunarDragonItemDisplays();

        public override string survivorDefName => "LunarDragon";

        public static BodyIndex bodyIndex;

        [SystemInitializer(new Type[] { typeof(BodyCatalog) })]
        private static void GetBodyIndex() {
            bodyIndex = BodyCatalog.FindBodyIndex(_bodyName);
        }

        public override void Init() {
            base.Init();
            _bodyName = bodyName;
        }

        public override void InitCharacter() {
            LunarDragonUnlockables.Init(assetBundle);

            base.InitCharacter();

            LunarDragonStates.Init();
            LunarDragonTokens.Init();

            LunarDragonAssets.Init(assetBundle);
            LunarDragonBuffs.Init(assetBundle);

            AdditionalBodySetup();
        }

        private void AdditionalBodySetup() {
            SetupAkBanks();
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
    }
}