namespace LunarDragonMod.Modules {
    internal static class Tokens {
        public const string agilePrefix = "<style=cIsUtility>Agile.</style>";
        public const string stunningPrefix = "<style=cIsDamage>Stunning.</style>";
        public const string freezingPrefix = "<style=cIsUtility>Freezing.</style>";

        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementNameToken(string identifier) {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementDescriptionToken(string identifier) {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}