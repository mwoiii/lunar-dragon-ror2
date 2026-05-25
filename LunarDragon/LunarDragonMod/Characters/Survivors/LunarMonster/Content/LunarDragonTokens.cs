using LunarDragonMod.Modules;
using LunarDragonMod.Survivors.LunarDragon.Achievements;
using System;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonTokens {
        public static void Init() {
            AddLunarDragonTokens();
        }

        public static void AddLunarDragonTokens() {
            string prefix = LunarMonsterSurvivor.LUNAR_DRAGON_PREFIX;

            string desc = "Lunar Dragon description. <color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Primary tip." + Environment.NewLine + Environment.NewLine
             + "< ! > Secondary tip." + Environment.NewLine + Environment.NewLine
             + "< ! > Utility tip." + Environment.NewLine + Environment.NewLine
             + "< ! > Special tip." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so it left.";
            string outroFailure = "..and so it vanished.";

            Language.Add(prefix + "NAME", "Lunar Dragon");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Forsaken Apostate");
            Language.Add(prefix + "LORE", "<style=cMono>\\\\\\MISSING LOG DATA\\\\\\</style>");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Dark Demise");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Lunar Wings");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "The Lunar Dragon can hover.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_ELEMENTAL_BLITZ_NAME", "Elemental Blitz");
            Language.Add(prefix + "PRIMARY_ELEMENTAL_BLITZ_DESCRIPTION", $"Fire heavy elemental blasts for <style=cIsDamage>{100f * LunarDragonStaticValues.primaryDamageCoefficient}% damage</style> each. " +
                $"Every 3rd attack fires a piercing bolt for <style=cIsDamage>{100f * LunarDragonStaticValues.primaryFinisherDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_ERUPTION_NAME", "Eruption");
            Language.Add(prefix + "SECONDARY_ERUPTION_DESCRIPTION", $"Launch a huge fireball for <style=cIsDamage>{100f * LunarDragonStaticValues.secondaryFireBlastDamageCoefficient}% damage</style>, " +
                $"causing an eruption of blazing rings if detonated on the ground.");

            Language.Add(prefix + "SECONDARY_SURGE_NAME", "Surge");
            Language.Add(prefix + "SECONDARY_SURGE_DESCRIPTION", $"{Tokens.stunningPrefix} Launch a huge plasmaball for <style=cIsDamage>{100f * LunarDragonStaticValues.secondaryPlasmaBlastDamageCoefficient}% damage</style>. " +
                $"Use the skill again before collision to detonate the projectile early.");

            Language.Add(prefix + "SECONDARY_GLACIATE_NAME", "Glaciate");
            Language.Add(prefix + "SECONDARY_GLACIATE_DESCRIPTION", $"Launch a huge ice chunk for <style=cIsDamage>{100f * LunarDragonStaticValues.secondaryIceBlastDamageCoefficient}% damage</style>, " +
                $"expanding on impact and inflicting <style=cIsUtility>Slow</style> to all nearby enemies caught in the glacier.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_BURST_THRUSTERS_NAME", "Burst Thrusters");
            Language.Add(prefix + "UTILITY_BURST_THRUSTERS_DESCRIPTION", $"Overcharge the cannons to <style=cIsUtility>speed</style> forward, dealing up to <style=cIsDamage>{100f * LunarDragonStaticValues.utilityBurstThrustersUpperDamageCoefficient}% damage</style>, " +
                $"and <style=cIsDamage>stunning</style> at maximum charge");

            Language.Add(prefix + "UTILITY_FLOW_THRUSTERS_NAME", "Flow Thrusters");
            Language.Add(prefix + "UTILITY_FLOW_THRUSTERS_DESCRIPTION", $"Steadily discharge the cannons to <style=cIsUtility>take flight<style=cIsDamage>. You can continue to attack for the duration.");//, dealing up to <style=cIsDamage>{100f * LunarDragonStaticValues.utilityBurstThrustersUpperDamageCoefficient}% damage</style>, " +
                                                                                                                                                                                                           //$"and <style=cIsDamage>stunning</style> at maximum charge");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Draco Ascent");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * LunarDragonStaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(LunarDragonMasteryAchievement.identifier), "Lunar Dragon: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(LunarDragonMasteryAchievement.identifier), "As Lunar Dragon, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
