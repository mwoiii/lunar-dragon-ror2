using LunarDragonMod.Modules;
using LunarDragonMod.Modules.DamageTypes;
using R2API;
using RoR2.Projectile;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonAssets {

        public static AssetBundle _assetBundle;

        public static GameObject fireballPrefab;

        public static GameObject iceballPrefab;

        public static GameObject heavyFireballPrefab;

        public static GameObject heavyIceballPrefab;

        public static GameObject heavyPlasmaballPrefab;

        public static void Init(AssetBundle assetBundle) {

            _assetBundle = assetBundle;

            LunarDragonPlugin.instance.StartCoroutine(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(assetBundle));

            fireballPrefab = Asset.LoadAndAddProjectilePrefab(assetBundle, "FireballProjectile");

            iceballPrefab = Asset.LoadAndAddProjectilePrefab(assetBundle, "IceballProjectile");

            heavyFireballPrefab = Asset.LoadAndAddProjectilePrefab(assetBundle, "HeavyFireballProjectile");

            heavyIceballPrefab = Asset.LoadAndAddProjectilePrefab(assetBundle, "HeavyIceballProjectile");

            heavyPlasmaballPrefab = Asset.LoadAndAddProjectilePrefab(assetBundle, "HeavyPlasmaballProjectile");
        }

        public static void AssignDamageTypes() {
            heavyPlasmaballPrefab.GetComponent<ProjectileDamage>().damageType.AddModdedDamageType(Stun3s.damageType);
        }
    }
}
