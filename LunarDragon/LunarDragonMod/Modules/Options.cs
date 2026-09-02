using BepInEx.Configuration;
using LunarDragonMod.Survivors.LunarDragon;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using UnityEngine;

namespace LunarDragonMod {

    public class Options {

        private static bool? _rooEnabled;

        public static bool rooEnabled {
            get {
                if (_rooEnabled == null) {
                    _rooEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
                }
                return (bool)_rooEnabled;
            }
        }

        public static ConfigEntry<bool> usePrimaryAimAssist { get; set; }

        public static void Init() {
            usePrimaryAimAssist = LunarDragonPlugin.config.Bind("Primary", "Aim Assist", true, "Whether or not the primary skill should automatically shift projectile trajectory toward enemies near the crosshair.");

            if (rooEnabled) {
                RoOInit();
            }
        }

        private static void RoOInit() {
            ModSettingsManager.AddOption(new CheckBoxOption(usePrimaryAimAssist, new CheckBoxConfig()));

            ModSettingsManager.SetModDescription("Config options relating to the Lunar Dragon survivor mod.");
            ModSettingsManager.SetModIcon(LunarDragonAssets.assetBundle.LoadAsset<Sprite>("texLunarDragonIcon"));
        }
    }
}