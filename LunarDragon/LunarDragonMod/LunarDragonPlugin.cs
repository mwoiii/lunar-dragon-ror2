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
    [BepInDependency(R2API.DamageAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class LunarDragonPlugin : BaseUnityPlugin {
        public const string MODUID = "com.miyowi.LunarDragonMod";
        public const string MODNAME = "LunarDragon";
        public const string MODVERSION = "1.0.0";

        public const string DEVELOPER_PREFIX = "MIYOWI";

        public static LunarDragonPlugin instance;

        void Awake() {
            instance = this;

            Log.Init(Logger);

            Modules.Language.Init();

            DamageTypeCollection.Init();
            new LunarMonsterSurvivor().Init();

            RoR2Application.onLoadFinished += OnLoadFinished;

            new Modules.ContentPacks().Initialize();
        }
        private void OnLoadFinished() {
            Hooks.AddHooks();
            //LunarDragonAssets.AssignDamageTypes();
        }
    }
}
