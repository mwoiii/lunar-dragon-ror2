using RoR2;
using RoR2.Achievements;

namespace LunarDragonMod.Survivors.LunarDragon.Achievements {
    [RegisterAchievement(identifier, unlockableIdentifier, null, 0, null)]
    public class LunarDragonWIPAchievement : BaseAchievement {
        public const string identifier = LunarDragonSurvivor.LUNAR_DRAGON_PREFIX + "wipAchievement";
        public const string unlockableIdentifier = LunarDragonSurvivor.LUNAR_DRAGON_PREFIX + "wipUnlockable";
    }
}
