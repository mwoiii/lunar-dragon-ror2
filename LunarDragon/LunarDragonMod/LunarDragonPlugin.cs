using BepInEx;
using LunarDragonMod.Modules;
using LunarDragonMod.Survivors.LunarDragon;
using R2API.Utils;
using RoR2;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]


namespace LunarDragonMod {
    //[BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class LunarDragonPlugin : BaseUnityPlugin {
        public const string MODUID = "com.miyowi.LunarDragonMod";
        public const string MODNAME = "LunarMonsterMod";
        public const string MODVERSION = "1.0.0";

        public const string DEVELOPER_PREFIX = "MIYOWI";

        public static LunarDragonPlugin instance;

        void Awake() {
            instance = this;

            //easy to use logger
            Log.Init(Logger);

            // used when you want to properly set up language folders
            Modules.Language.Init();

            // character initialization
            new LunarMonsterSurvivor().Initialize();

            RoR2Application.onLoadFinished += OnLoadFinished;

            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();
        }
        private void OnLoadFinished() {
            DamageTypeCollection.Init();
            Hooks.AddHooks();
            LunarDragonAssets.AssignDamageTypes();
        }
    }
}
