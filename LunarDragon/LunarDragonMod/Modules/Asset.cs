using System.Reflection;
using UnityEngine;

namespace LunarDragonMod.Modules {
    internal static class Asset {

        internal static AssetBundle LoadAssetBundle(string bundleName) {
            AssetBundle assetBundle = null;
            try {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"LunarDragonMod.{bundleName}")) {
                    if (assetStream != null) {
                        assetBundle = AssetBundle.LoadFromStream(assetStream);
                    }
                }
                if (assetBundle == null) {
                    Log.Error("Couldn't find asset bundle!");
                }
            } catch (System.Exception e) {
                Log.Error($"Error loading asset bundle '{bundleName}'.\n{e}");
            }

            return assetBundle;
        }
    }
}