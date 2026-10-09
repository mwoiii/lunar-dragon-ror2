using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Modules {
    internal static class ItemDisplays {
        public static Dictionary<Object, ItemDisplayRule[]> KeyAssetDisplayPrefabs = new Dictionary<Object, ItemDisplayRule[]>();
        public static Dictionary<string, Object> KeyAssets = new Dictionary<string, Object>();

        public static int queuedDisplays;

        public static bool initialized = false;

        public static void LazyInit() {
            if (initialized)
                return;
            initialized = true;

            PopulateDisplays();
        }

        internal static void DisposeWhenDone() {
            queuedDisplays--;
            if (queuedDisplays > 0)
                return;
            if (!initialized)
                return;
            initialized = false;
            KeyAssetDisplayPrefabs = null;
            KeyAssets = null;
        }

        internal static void PopulateDisplays() {
            PopulateFromBody(RoR2_Base_Loader.LoaderBody_prefab);

            //PopulateCustomLightningArm();
        }

        private static void PopulateFromBody(string bodyGUID) {
            ItemDisplayRuleSet itemDisplayRuleSet = Addressables.LoadAssetAsync<GameObject>(new AssetReferenceGameObject(bodyGUID)).WaitForCompletion().GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            ItemDisplayRuleSet.KeyAssetRuleGroup[] itemRuleGroups = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < itemRuleGroups.Length; i++) {
                ItemDisplayRule[] rules = itemRuleGroups[i].displayRuleGroup.rules;
                KeyAssetDisplayPrefabs[itemRuleGroups[i].keyAsset] = rules;
                KeyAssets[itemRuleGroups[i].keyAsset.name] = itemRuleGroups[i].keyAsset;
            }
        }

        #region add rule helpers

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(string itemName, params ItemDisplayRule[] rules) => CreateDisplayRuleGroupWithRules(GetKeyAssetFromString(itemName), rules);
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(Object keyAsset_, params ItemDisplayRule[] rules) {
            if (keyAsset_ == null)
                Log.Error("could not find keyasset");

            return new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = keyAsset_,
                displayRuleGroup = new DisplayRuleGroup {
                    rules = rules
                }
            };
        }

        public static ItemDisplayRule CreateDisplayRule(string assetGUID, string childName, Vector3 position, Vector3 rotation, Vector3 scale) => CreateDisplayRule(new AssetReferenceGameObject(assetGUID), childName, position, rotation, scale);
        public static ItemDisplayRule CreateDisplayRule(AssetReferenceGameObject itemDisplayAddress, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefabAddress = itemDisplayAddress,
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }
        public static ItemDisplayRule CreateLimbMaskDisplayRule(LimbFlags limb) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.LimbMask,
                limbMask = limb,
                childName = "",
                followerPrefab = null
                //localPos = Vector3.zero,
                //localAngles = Vector3.zero,
                //localScale = Vector3.zero
            };
        }

        private static Object GetKeyAssetFromString(string itemName) {
            Object itemDef = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName);

            if (itemDef == null) {
                itemDef = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/" + itemName);
            }

            if (itemDef == null) {
                Log.Error("Could not load keyasset for " + itemName);
            }

            return itemDef;
        }

        #endregion add rule helpers
    }
}