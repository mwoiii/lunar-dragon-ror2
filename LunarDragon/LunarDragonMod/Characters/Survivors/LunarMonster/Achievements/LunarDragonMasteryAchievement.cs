using LunarDragonMod.Modules.Achievements;
using RoR2;

namespace LunarDragonMod.Survivors.LunarDragon.Achievements {
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class LunarDragonMasteryAchievement : BaseMasteryAchievement {
        public const string identifier = LunarMonsterSurvivor.LUNAR_DRAGON_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = LunarMonsterSurvivor.LUNAR_DRAGON_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => LunarMonsterSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}