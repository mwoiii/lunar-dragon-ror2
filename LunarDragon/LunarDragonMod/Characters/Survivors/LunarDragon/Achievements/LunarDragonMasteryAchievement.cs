using LunarDragonMod.Modules.Achievements;
using RoR2;

namespace LunarDragonMod.Survivors.LunarDragon.Achievements {
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class LunarDragonMasteryAchievement : BaseMasteryAchievement {
        public const string identifier = LunarDragonSurvivor.LUNAR_DRAGON_PREFIX + "MASTERY_ACHIEVEMENT";
        public const string unlockableIdentifier = LunarDragonSurvivor.LUNAR_DRAGON_PREFIX + "MASTERY_UNLOCKABLE";

        public override string RequiredCharacterBody => LunarDragonSurvivor.instance.bodyName;

        public override float RequiredDifficultyCoefficient => 3;
    }
}