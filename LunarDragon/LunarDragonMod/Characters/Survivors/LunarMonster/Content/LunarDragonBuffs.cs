using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonBuffs {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static void Init(AssetBundle assetBundle) {
            armorBuff = Modules.Content.CreateAndAddBuff("LunarDragonArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
