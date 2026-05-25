using LunarDragonMod.Survivors.LunarDragon.Achievements;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonUnlockables {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init() {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                LunarDragonMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(LunarDragonMasteryAchievement.identifier),
                LunarMonsterSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
