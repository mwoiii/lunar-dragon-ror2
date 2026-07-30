using LunarDragonMod.Survivors.LunarDragon.Achievements;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonUnlockables {
        public static UnlockableDef characterUnlockableDef;

        public static UnlockableDef masterySkinUnlockableDef;

        public static UnlockableDef wipSkillUnlockableDef;


        public static void Init() {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockableDef(
                LunarDragonMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(LunarDragonMasteryAchievement.identifier),
                LunarDragonAssets.assetBundle.LoadAsset<Sprite>("texMasteryAchievement")
            );

            wipSkillUnlockableDef = Modules.Content.CreateAndAddUnlockableDef(
                LunarDragonWIPAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(LunarDragonWIPAchievement.identifier),
                LunarDragonAssets.assetBundle.LoadAsset<Sprite>("texWIPIcon")
            );
        }
    }
}
