using LunarDragonMod.Modules;
using LunarDragonMod.Modules.DamageTypes;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonAssets {

        public static AssetBundle assetBundle;

        public static GameObject fireballPrefab;

        public static GameObject iceballPrefab;

        public static GameObject iceballMuzzlePrefab;

        public static GameObject heavyFireballPrefab;

        public static GameObject heavyIceballPrefab;

        public static GameObject heavyPlasmaballPrefab;

        internal static void LoadAssetBundle(string bundleName) {

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
        }

        public static void Init() {
            LoadAssetBundle("lunardragonbundle");

            LunarDragonPlugin.instance.StartCoroutine(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(assetBundle));

            CreateFireball();
            CreateIceball();
            CreateHeavyFireball();
            CreateHeavyIceball();
            CreateHeavyPlasmaball();
        }

        private static void CreateFireball() {
            ParticleSystemRenderer psr;
            fireballPrefab = assetBundle.LoadAsset<GameObject>("FireballProjectile");

            #region MageLightningBombGhost
            GameObject dragonFireballGhost = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageLightningBombGhost_prefab).WaitForCompletion(), "DragonFireballGhost", false);

            Object.Destroy(dragonFireballGhost.transform.Find("Sparks, Trail").gameObject);

            Object.Destroy(dragonFireballGhost.transform.Find("Point light").gameObject);

            GameObject dragonFireballBase = dragonFireballGhost.transform.Find("Base").gameObject;
            psr = dragonFireballBase.GetComponent<ParticleSystemRenderer>();
            Material matBase = new Material(psr.sharedMaterial);
            matBase.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampMageFire_png).WaitForCompletion());
            matBase.SetFloat("_AlphaBoost", 5.7f);
            psr.sharedMaterial = matBase;
            dragonFireballBase.transform.localScale = Vector3.one * 3f;

            GameObject dragonFireballCore = dragonFireballGhost.transform.Find("OrbCore").gameObject;
            MeshRenderer meshRenderer = dragonFireballCore.GetComponent<MeshRenderer>();
            Material matOrbCore = new Material(Addressables.LoadAssetAsync<Material>(RoR2_DLC2_Child.matChildStarCore_mat).WaitForCompletion());
            matOrbCore.SetColor("_TintColor", new Color(1f, 0.74f, 0f));
            matOrbCore.renderQueue += 1;
            meshRenderer.sharedMaterial = matOrbCore;
            dragonFireballCore.transform.localScale = Vector3.one;
            #endregion

            #region FireballGhost
            GameObject lemFireball = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Lemurian.FireballGhost_prefab).WaitForCompletion(), dragonFireballGhost.transform);
            Object.Destroy(lemFireball.GetComponent<ProjectileGhostController>());
            Object.Destroy(lemFireball.GetComponent<DetachParticleOnDestroyAndEndEmission>());
            Object.Destroy(lemFireball.GetComponent<VFXAttributes>());
            lemFireball.transform.localScale = Vector3.one * 1.8f;

            lemFireball.transform.Find("Point light").GetComponent<Light>().range = 8f;
            lemFireball.transform.localPosition = Vector3.zero;
            #endregion

            fireballPrefab.GetComponent<ProjectileController>().ghostPrefab = dragonFireballGhost;
            fireballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_LemurianBruiser.OmniExplosionVFXLemurianBruiserFireballImpact_prefab).WaitForCompletion();
            Content.AddProjectilePrefab(fireballPrefab);
        }

        private static void CreateIceball() {
            iceballPrefab = assetBundle.LoadAsset<GameObject>("IceballProjectile");

            #region MageIceBombGhost
            GameObject dragonIceballGhost = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageIceBombGhost_prefab).WaitForCompletion(), "DragonIceballGhost", false);

            GameObject dragonIceballOrbCore = dragonIceballGhost.transform.Find("OrbCore").gameObject;
            dragonIceballOrbCore.transform.localScale = Vector3.one * 0.5f;
            dragonIceballGhost.transform.Find("Beams").gameObject.SetActive(true);
            dragonIceballGhost.transform.Find("Base").gameObject.SetActive(true);
            #endregion

            #region MageIceExplosion
            iceballMuzzlePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageIceExplosion_prefab).WaitForCompletion(), "IceCannonMuzzleVFX", false);

            Object.Destroy(iceballMuzzlePrefab.transform.Find("IceMesh").gameObject);

            Object.Destroy(iceballMuzzlePrefab.transform.Find("RuneRings").gameObject);

            Content.CreateAndAddEffectDef(iceballMuzzlePrefab);
            #endregion

            #region BoostedProjectileExplosionVFX
            GameObject impactEffect = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_Chef.BoostedProjectileExplosionVFX_prefab).WaitForCompletion(), "IceBallExplosionVFX", false);
            impactEffect.GetComponent<EffectComponent>().soundName = "Play_mage_shift_wall_explode";
            foreach (Transform child in impactEffect.transform.Find("Dash, Bright")) {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = particleSystem.main;
                child.localScale = Vector3.one * 0.5f;
                if (child.name == "Ring") {
                    main.startSizeX = new ParticleSystem.MinMaxCurve(6f);
                    main.startSizeY = new ParticleSystem.MinMaxCurve(6f);
                    main.startSizeZ = new ParticleSystem.MinMaxCurve(3f);
                    child.GetComponent<ParticleSystemRenderer>().alignment = ParticleSystemRenderSpace.World;
                    continue;
                }
                main.startSizeMultiplier = 0.5f;
            }

            ShakeEmitter shakeEmitter = impactEffect.GetComponent<ShakeEmitter>();
            shakeEmitter.wave.amplitude = 0.2f;
            shakeEmitter.wave.frequency = 12f;
            shakeEmitter.duration = 0.15f;
            shakeEmitter.radius = 120f;
            Content.CreateAndAddEffectDef(impactEffect);
            #endregion

            iceballPrefab.GetComponent<ProjectileController>().ghostPrefab = dragonIceballGhost;
            iceballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = impactEffect;
            Content.AddProjectilePrefab(fireballPrefab);
        }

        private static void CreateHeavyFireball() {
            heavyFireballPrefab = assetBundle.LoadAsset<GameObject>("HeavyFireballProjectile");
            Content.AddProjectilePrefab(fireballPrefab);
        }

        private static void CreateHeavyIceball() {
            heavyIceballPrefab = assetBundle.LoadAsset<GameObject>("HeavyIceballProjectile");
            Content.AddProjectilePrefab(fireballPrefab);
        }

        private static void CreateHeavyPlasmaball() {
            heavyPlasmaballPrefab = assetBundle.LoadAsset<GameObject>("HeavyPlasmaballProjectile");
            heavyPlasmaballPrefab.GetComponent<ProjectileDamage>().damageType.AddModdedDamageType(Stun3s.damageType);
            Content.AddProjectilePrefab(fireballPrefab);
        }

        public static void AssignDamageTypes() {
            heavyPlasmaballPrefab.GetComponent<ProjectileDamage>().damageType.AddModdedDamageType(Stun3s.damageType);
        }
    }
}
