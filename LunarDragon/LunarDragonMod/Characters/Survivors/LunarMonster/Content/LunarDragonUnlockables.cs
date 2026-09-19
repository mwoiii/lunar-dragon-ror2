using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonUnlockables {
        public static void Init(AssetBundle assetBundle) {
            Modules.Content.AddUnlockableDef(assetBundle.LoadAsset<UnlockableDef>("MIYOWI_LUNAR_DRAGON_MASTERY_UNLOCKABLE"));
        }
    }
}
