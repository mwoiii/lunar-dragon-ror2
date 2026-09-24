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

        public static ConfigEntry<bool> displaySpecialControls { get; set; }

        public static void Init() {
            displaySpecialControls = LunarDragonPlugin.config.Bind("Special", "Show Controls UI", true, "Whether or not the keys for 'Confirm' and 'Cancel' are prominently displayed when aiming with Draco Ascent.");

            if (rooEnabled) {
                RoOInit();
            }
        }

        private static void RoOInit() {
            ModSettingsManager.AddOption(new CheckBoxOption(displaySpecialControls, new CheckBoxConfig()));

            ModSettingsManager.SetModDescription("Config options relating to the Lunar Dragon survivor mod.");
            ModSettingsManager.SetModIcon(LunarDragonAssets.assetBundle.LoadAsset<Sprite>("texLunarDragonIcon"));
        }
    }
}