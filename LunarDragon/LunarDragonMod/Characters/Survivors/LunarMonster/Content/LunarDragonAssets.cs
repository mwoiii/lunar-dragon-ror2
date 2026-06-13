using LunarDragonMod.Characters.Survivors.LunarMonster.Components;
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

        public static GameObject jetEffectPrefab;

        public static GameObject fireballPrefab;

        public static GameObject iceballPrefab;

        public static GameObject iceballMuzzlePrefab;

        public static GameObject heavyFireballPrefab;

        public static GameObject heavyFireballMuzzlePrefab;

        public static GameObject heavyFireballImpactPrefab;

        public static GameObject heavyFireballPlumePrefab;

        public static GameObject plumeShakeSFX;

        public static GameObject heavyIceballPrefab;

        public static GameObject heavyPlasmaballPrefab;

        public static GameObject laserTracerPrefab;

        public static GameObject laserMuzzlePrefab;

        public static GameObject laserHitEffectPrefab;

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

            TryBuildAsset("Jet Effect", CreateJetEffect);

            TryBuildAsset("Primary Fireball", CreateFireball);
            TryBuildAsset("Secondary Fireball", CreateHeavyFireball); // depends on above to be made first

            TryBuildAsset("Primary Iceball", CreateIceball);
            TryBuildAsset("Secondary Iceball", CreateHeavyIceball);

            TryBuildAsset("Primary Laser", CreateLaser);
            TryBuildAsset("Secondary Plasmaball", CreateHeavyPlasmaball);
        }

        private static void TryBuildAsset(string assetName, System.Action buildAction) {
            try {
                buildAction();
            } catch (System.Exception e) {
                Log.Warning($"Failed to complete building asset {assetName}!\n\n{e}");
            }
        }

        private static void CreateJetEffect() {
            jetEffectPrefab = PrefabAPI.CreateEmptyPrefab("JetEffect", false);
            jetEffectPrefab.transform.position = new Vector3(0.001f, 0f, 0f);
            jetEffectPrefab.transform.localScale = new Vector3(0.0007f, 0.0007f, 0.006f);
            jetEffectPrefab.transform.eulerAngles = new Vector3(0f, 90f, 0f);

            TryBuildAsset("Jet Effect Components", () => {
                #region MageBody
                GameObject jetEffect = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageBody_prefab).WaitForCompletion()
                .GetComponent<ModelLocator>().modelChildLocator.FindChild("JetOn").gameObject, jetEffectPrefab.transform, false);
                jetEffect.transform.localPosition = Vector3.zero;
                jetEffect.gameObject.name = "JetSFX";
                jetEffect.SetActive(true);

                Object.Destroy(jetEffect.transform.Find("JetsL").gameObject);
                jetEffect.transform.Find("JetsR").gameObject.name = "MainJet";

                for (int i = jetEffect.transform.childCount - 1; i >= 0; i--) {
                    Transform child = jetEffect.transform.GetChild(i);
                    child.localPosition = Vector3.zero;
                    child.SetParent(jetEffectPrefab.transform, false);
                }
                #endregion
            });
        }

        private static void CreateFireball() {
            fireballPrefab = assetBundle.LoadAsset<GameObject>("FireballProjectile");

            GameObject dragonFireballGhost = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageLightningBombGhost_prefab).WaitForCompletion(), "DragonFireballGhost", false);
            TryBuildAsset("Primary Fireball Ghost", () => {
                #region MageLightningBombGhost
                Object.Destroy(dragonFireballGhost.transform.Find("Sparks, Trail").gameObject);

                Object.Destroy(dragonFireballGhost.transform.Find("Point light").gameObject);

                GameObject dragonFireballBase = dragonFireballGhost.transform.Find("Base").gameObject;
                ParticleSystemRenderer psr = dragonFireballBase.GetComponent<ParticleSystemRenderer>();
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

                //dragonFireballGhost.AddComponent<EffectComponent>();

                //Content.CreateAndAddEffectDef(dragonFireballGhost);
                #endregion
            });

            fireballPrefab.GetComponent<ProjectileController>().ghostPrefab = dragonFireballGhost;

            fireballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_LemurianBruiser.OmniExplosionVFXLemurianBruiserFireballImpact_prefab).WaitForCompletion();

            Content.AddProjectilePrefab(fireballPrefab);
        }

        private static void CreateIceball() {
            iceballPrefab = assetBundle.LoadAsset<GameObject>("IceballProjectile");

            GameObject dragonIceballGhost = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageIceBombGhost_prefab).WaitForCompletion(), "DragonIceballGhost", false);
            TryBuildAsset("Primary Iceball Ghost", () => {
                #region MageIceBombGhost
                GameObject dragonIceballOrbCore = dragonIceballGhost.transform.Find("OrbCore").gameObject;
                dragonIceballOrbCore.transform.localScale = Vector3.one * 0.5f;
                dragonIceballGhost.transform.Find("Beams").gameObject.SetActive(true);
                dragonIceballGhost.transform.Find("Base").gameObject.SetActive(true);
                #endregion

                #region MageIceExplosion
                iceballMuzzlePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Mage.MageIceExplosion_prefab).WaitForCompletion(), "IceCannonMuzzleVFX", false);

                iceballMuzzlePrefab.GetComponent<EffectComponent>().parentToReferencedTransform = false;

                Object.Destroy(iceballMuzzlePrefab.transform.Find("IceMesh").gameObject);

                Object.Destroy(iceballMuzzlePrefab.transform.Find("RuneRings").gameObject);
                #endregion
            });

            Content.CreateAndAddEffectDef(iceballMuzzlePrefab);

            iceballPrefab.GetComponent<ProjectileController>().ghostPrefab = dragonIceballGhost;

            GameObject impactEffect = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_Chef.BoostedProjectileExplosionVFX_prefab).WaitForCompletion(), "IceBallExplosionVFX", false);
            TryBuildAsset("Primary Iceball Impact Explosion", () => {
                #region BoostedProjectileExplosionVFXimpactEffect.GetComponent<EffectComponent>().soundName = "Play_mage_shift_wall_explode";
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
                #endregion
            });

            Content.CreateAndAddEffectDef(impactEffect);
            iceballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = impactEffect;

            Content.AddProjectilePrefab(iceballPrefab);
        }

        private static void CreateHeavyFireball() {
            heavyFireballPrefab = assetBundle.LoadAsset<GameObject>("HeavyFireballProjectile");

            GameObject heavyFireballGhost = PrefabAPI.InstantiateClone(fireballPrefab.GetComponent<ProjectileController>().ghostPrefab, "DragonFireballHeavyGhost", false);
            TryBuildAsset("Secondary Fireball Ghost", () => {
                #region DragonFireballGhost
                heavyFireballGhost.transform.localScale = Vector3.one * 1.5f;
                MeshRenderer meshRenderer = heavyFireballGhost.transform.Find("OrbCore").GetComponent<MeshRenderer>();
                Material matOrbCore = new Material(meshRenderer.sharedMaterial);
                matOrbCore.SetColor("_TintColor", new Color(0f, 0.9f, 1f));
                matOrbCore.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC3_SolusAmalgamator.texRampSolusBlueFlame_png).WaitForCompletion());
                matOrbCore.SetFloat("_AlphaBoost", 6f);
                meshRenderer.sharedMaterial = matOrbCore;

                ParticleSystemRenderer psr = heavyFireballGhost.transform.Find("Base").GetComponent<ParticleSystemRenderer>();
                Material matBase = new Material(psr.sharedMaterial);
                matBase.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texElusiveAntlersRamp_png).WaitForCompletion());
                matBase.SetFloat("_AlphaBoost", 7f);
                psr.sharedMaterial = matBase;

                heavyFireballGhost.transform.Find("FireballGhost(Clone)/Point light").GetComponent<Light>().color = new Color(0f, 0.8f, 1f);
                Transform flames = heavyFireballGhost.transform.Find("FireballGhost(Clone)/Flames");
                flames.localScale = Vector3.one * 1.5f;
                ParticleSystem.ColorOverLifetimeModule colorOverLifetime = flames.GetComponent<ParticleSystem>().colorOverLifetime;
                Gradient gradient = colorOverLifetime.color.gradient;
                GradientColorKey[] colorKeys = gradient.colorKeys;
                colorKeys[0] = new GradientColorKey(new Color(0.02f, 0.7f, 1f), colorKeys[0].time);
                colorKeys[1] = new GradientColorKey(new Color(0.02f, 0.23f, 0.91f), colorKeys[1].time);
                gradient.colorKeys = colorKeys;
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

                GameObject chefFireball = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_Chef.BoostedSearFireballGhost_prefab).WaitForCompletion());
                chefFireball.transform.Find("Particles/FireOutter").SetParent(heavyFireballGhost.transform, false);
                Object.Destroy(chefFireball);
                #endregion
            });

            heavyFireballPrefab.GetComponent<ProjectileController>().ghostPrefab = heavyFireballGhost;

            heavyFireballMuzzlePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Common_VFX.OmniExplosionVFXQuick_prefab).WaitForCompletion(), "FireballHeavyMuzzleVFX", false);
            TryBuildAsset("Secondary Fireball Muzzle VFX", () => {
                #region OmniExplosionVFXQuick
                heavyFireballMuzzlePrefab.transform.localScale = Vector3.one * 1.5f;

                ParticleSystemRenderer psr = heavyFireballMuzzlePrefab.transform.Find("ScaledHitsparks 1").GetComponent<ParticleSystemRenderer>();
                Material matScaledHitsparks = new Material(psr.sharedMaterial);
                matScaledHitsparks.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texSpeedBoostPickupThornRamp_png).WaitForCompletion());
                psr.sharedMaterial = matScaledHitsparks;

                psr = heavyFireballMuzzlePrefab.transform.Find("Unscaled Flames").GetComponent<ParticleSystemRenderer>();
                Material matUnscaledFlames = new Material(psr.sharedMaterial);
                matUnscaledFlames.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texSpeedBoostPickupThornRamp_png).WaitForCompletion());
                psr.sharedMaterial = matUnscaledFlames;

                heavyFireballMuzzlePrefab.transform.Find("Point Light").GetComponent<Light>().color = new Color(0f, 0.8f, 1f);
                #endregion
            });

            Content.CreateAndAddEffectDef(heavyFireballMuzzlePrefab);

            heavyFireballImpactPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_LemurianBruiser.OmniExplosionVFXLemurianBruiserFireballImpact_prefab).WaitForCompletion(), "FireballHeavyImpactVFX", false);
            TryBuildAsset("Secondary Fireball Impact VFX", () => {

                #region OmniExplosionVFXLemurianBruiserFireballImpact
                Object.Destroy(heavyFireballImpactPrefab.transform.Find("ScaledHitsparks 1").gameObject); // can't change it idk why
                //ParticleSystemRenderer psr = scaledHitsparks.GetComponent<ParticleSystemRenderer>();
                //Material matScaledHitsparks = new Material(psr.sharedMaterials[0]);
                //matScaledHitsparks.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texSpeedBoostPickupThornRamp_png).WaitForCompletion());
                //psr.sharedMaterials[0] = matScaledHitsparks;


                //ParticleSystem.MainModule main = scaledHitsparks.GetComponent<ParticleSystem>().main;
                //main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 1f, 1f), new Color(0f, 0.08f, 0.35f));

                ParticleSystemRenderer psr = heavyFireballImpactPrefab.transform.Find("UnscaledHitsparks 1").GetComponent<ParticleSystemRenderer>();
                Material matUnscaledHitsparks = new Material(psr.sharedMaterial);
                matUnscaledHitsparks.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texSpeedBoostPickupThornRamp_png).WaitForCompletion());
                psr.sharedMaterial = matUnscaledHitsparks;

                ParticleSystem.ColorOverLifetimeModule colorOverLifetime = heavyFireballImpactPrefab.transform.Find("ScaledSmoke, Billboard").GetComponent<ParticleSystem>().colorOverLifetime;
                Gradient gradient = colorOverLifetime.color.gradient;
                GradientColorKey[] colorKeys = gradient.colorKeys;
                colorKeys[0] = new GradientColorKey(new Color(0.62f, 0.8f, 1f), colorKeys[0].time);
                colorKeys[1] = new GradientColorKey(new Color(0.38f, 0.44f, 0.64f), colorKeys[1].time);
                gradient.colorKeys = colorKeys;
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

                colorOverLifetime = heavyFireballImpactPrefab.transform.Find("ScaledSmokeRing, Mesh").GetComponent<ParticleSystem>().colorOverLifetime;
                gradient = colorOverLifetime.color.gradient;
                colorKeys = gradient.colorKeys;
                colorKeys[0] = new GradientColorKey(new Color(0.62f, 0.8f, 1f), colorKeys[0].time);
                colorKeys[1] = new GradientColorKey(new Color(0.38f, 0.44f, 0.64f), colorKeys[1].time);
                gradient.colorKeys = colorKeys;
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

                colorOverLifetime = heavyFireballImpactPrefab.transform.Find("Unscaled Smoke, Billboard").GetComponent<ParticleSystem>().colorOverLifetime;
                gradient = colorOverLifetime.color.gradient;
                colorKeys = gradient.colorKeys;
                colorKeys[0] = new GradientColorKey(new Color(0.62f, 0.8f, 1f), colorKeys[0].time);
                colorKeys[1] = new GradientColorKey(new Color(0.38f, 0.44f, 0.64f), colorKeys[1].time);
                gradient.colorKeys = colorKeys;
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

                psr = heavyFireballImpactPrefab.transform.Find("AreaIndicatorRing, Billboard").GetComponent<ParticleSystemRenderer>();
                Material matAreaIndicatorRingBillboard = new Material(psr.sharedMaterial);
                matAreaIndicatorRingBillboard.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texSpeedBoostPickupThornRamp_png).WaitForCompletion());
                psr.sharedMaterial = matAreaIndicatorRingBillboard;

                ParticleSystem.MainModule main = heavyFireballImpactPrefab.transform.Find("Physics Sparks").GetComponent<ParticleSystem>().main;
                main.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0.41f, 0.72f), new Color(0.05f, 0.64f, 0.8f));

                colorOverLifetime = heavyFireballImpactPrefab.transform.Find("Flash, Soft Glow").GetComponent<ParticleSystem>().colorOverLifetime;
                gradient = colorOverLifetime.color.gradient;
                colorKeys = gradient.colorKeys;
                colorKeys[0] = new GradientColorKey(new Color(0.62f, 0.8f, 1f), colorKeys[0].time);
                colorKeys[1] = new GradientColorKey(new Color(0.38f, 0.44f, 0.64f), colorKeys[1].time);
                gradient.colorKeys = colorKeys;
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

                psr = heavyFireballImpactPrefab.transform.Find("Unscaled Flames").GetComponent<ParticleSystemRenderer>();
                Material matUnscaledFlames = new Material(psr.sharedMaterial);
                matUnscaledFlames.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC2_Items_SpeedBoostPickup.texSpeedBoostPickupThornRamp_png).WaitForCompletion());
                psr.sharedMaterial = matUnscaledFlames;

                main = heavyFireballImpactPrefab.transform.Find("Dash, Bright").GetComponent<ParticleSystem>().main;
                main.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0.32f, 0.58f));

                heavyFireballImpactPrefab.transform.Find("Point Light").GetComponent<Light>().color = new Color(0f, 0.42f, 0.81f);
                #endregion
            });

            Content.CreateAndAddEffectDef(heavyFireballImpactPrefab);
            heavyFireballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = heavyFireballImpactPrefab;

            heavyFireballPlumePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_Scorchling.VFXScorchlingBurrowing_prefab).WaitForCompletion(), "FireballHeavyPlumeVFX", false);
            TryBuildAsset("Secondary Fireball Plume VFX", () => {
                #region VFXScorchlingBurrowing

                Transform sparks = heavyFireballPlumePrefab.transform.Find("ParticleLoop/Sparks");
                sparks.localScale = Vector3.one * 1.5f;
                ParticleSystem.MainModule main = sparks.GetComponent<ParticleSystem>().main;
                main.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0.67f, 0.75f), new Color(0f, 0.14f, 1f));

                heavyFireballPlumePrefab.transform.Find("ParticleLoop/Debris, 3D").localScale = Vector3.one * 1.5f;

                Transform magma = heavyFireballPlumePrefab.transform.Find("ParticleLoop/Magma, Billboard");
                magma.localScale = Vector3.one * 1.5f;
                ParticleSystemRenderer psr = magma.GetComponent<ParticleSystemRenderer>();
                Material matMagma = new Material(psr.sharedMaterial);
                matMagma.SetColor("_TintColor", new Color(0.06f, 1f, 0.93f));
                matMagma.SetColor("_EmissionColor", new Color(0f, 0.35f, 1f));
                psr.sharedMaterial = matMagma;

                heavyFireballPlumePrefab.transform.Find("ParticleLoop/Dust, Billboard").localScale = Vector3.one * 1.5f;

                Transform dirtMounts = heavyFireballPlumePrefab.transform.Find("ParticleLoop/DirtMounts");
                dirtMounts.localScale = Vector3.one * 3f;
                psr = dirtMounts.GetComponent<ParticleSystemRenderer>();
                Material matDirtMounts = new Material(psr.sharedMaterial);
                matDirtMounts.SetTexture("_FlowHeightRamp", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampLunarWispFire_png).WaitForCompletion());
                matDirtMounts.SetTexture("_GreenChannelTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_skymeadow.texSMRockSide3_png).WaitForCompletion());
                psr.sharedMaterial = matDirtMounts;

                LightIntensityCurve lightCurve = heavyFireballPlumePrefab.transform.Find("ParticleLoop/Point Light").gameObject.AddComponent<LightIntensityCurve>();
                lightCurve.timeMax = 0.5f;
                lightCurve.curve = new AnimationCurve(
                    new Keyframe(0f, 1f, 0f, 0f),
                    new Keyframe(1f, 0f, -5f, 0)
                );
                lightCurve.GetComponent<Light>().color = new Color(0.14f, 0.4f, 1f);

                foreach (Transform child in heavyFireballPlumePrefab.transform.Find("ParticleLoop")) {
                    ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                    if (particleSystem) {
                        main = particleSystem.main;
                        main.duration = 0.25f;
                        main.loop = false;
                    }
                }
                #endregion

                #region LunarWispTrackingBombExplosion
                GameObject lunarExplosion = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_LunarWisp.LunarWispTrackingBombExplosion_prefab).WaitForCompletion());
                Transform lunarExplosionBurst = lunarExplosion.transform.Find("InitialBurst");
                lunarExplosionBurst.SetParent(heavyFireballPlumePrefab.transform, false);
                Object.Destroy(lunarExplosion);

                Object.Destroy(lunarExplosionBurst.Find("Point light").gameObject);

                Object.Destroy(lunarExplosionBurst.Find("Ring_Ps").gameObject);

                Object.Destroy(lunarExplosionBurst.Find("Sparks_Ps").gameObject);

                Transform flames = lunarExplosionBurst.Find("Flames_Ps");
                ParticleSystem ps = flames.GetComponent<ParticleSystem>();
                ParticleSystem.ShapeModule shape = ps.shape;
                shape.radius = 10f;
                main = ps.main;
                main.startDelay = 0f;
                main.startSize3D = true;
                main.startSizeX = new ParticleSystem.MinMaxCurve(5f, 10f);
                main.startSizeY = new ParticleSystem.MinMaxCurve(10f, 15f);
                main.startSizeZ = new ParticleSystem.MinMaxCurve(5f, 10f);
                main.startRotation = 0f;
                #endregion
            });

            Content.CreateAndAddEffectDef(heavyFireballPlumePrefab);

            plumeShakeSFX = PrefabAPI.CreateEmptyPrefab("FireballHeavyPlume Shake, SFX", false);
            TryBuildAsset("Secondary Fireball Plume Shake, SFX", () => {
                EffectComponent effectComponent = plumeShakeSFX.AddComponent<EffectComponent>();
                effectComponent.applyScale = true;
                effectComponent.soundName = "Play_lunar_wisp_attack2_explode";

                VFXAttributes vfxAttributes = plumeShakeSFX.AddComponent<VFXAttributes>();
                vfxAttributes.vfxIntensity = VFXAttributes.VFXIntensity.Low;
                vfxAttributes.vfxPriority = VFXAttributes.VFXPriority.Always;

                plumeShakeSFX.AddComponent<DestroyOnTimer>().duration = 2f;

                ShakeEmitter shake = plumeShakeSFX.AddComponent<ShakeEmitter>();
                shake.wave.amplitude = 0.6f;
                shake.wave.frequency = 20f;
                shake.duration = 0.5f;
                shake.radius = 80f;
                shake.scaleShakeRadiusWithLocalScale = true;
                shake.amplitudeTimeDecay = true;

                //PlaySoundOnDelay secondarySound = plumeShakeSFX.AddComponent<PlaySoundOnDelay>();
                //secondarySound.soundString = "Play_grandparent_attack3_sun_spawn";
                //secondarySound.delay = 0f;
            });

            Content.CreateAndAddEffectDef(plumeShakeSFX);

            Content.AddProjectilePrefab(heavyFireballPrefab);
        }

        private static void CreateHeavyIceball() {
            heavyIceballPrefab = assetBundle.LoadAsset<GameObject>("HeavyIceballProjectile");
            Content.AddProjectilePrefab(heavyIceballPrefab);
        }

        private static void CreateHeavyPlasmaball() {
            heavyPlasmaballPrefab = assetBundle.LoadAsset<GameObject>("HeavyPlasmaballProjectile");
            heavyPlasmaballPrefab.GetComponent<ProjectileDamage>().damageType.AddModdedDamageType(Stun3s.damageType);
            Content.AddProjectilePrefab(heavyPlasmaballPrefab);
        }

        private static void CreateLaser() {
            laserTracerPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Toolbot.TracerToolbotRebar_prefab).WaitForCompletion(), "DragonLaserTracer", false);
            TryBuildAsset("Primary Laser Tracer", () => {
                #region TracerToolbotRebar
                Object.Destroy(laserTracerPrefab.transform.Find("StickEffect").gameObject);
                Object.Destroy(laserTracerPrefab.GetComponent<LineRenderer>());
                Object.Destroy(laserTracerPrefab.GetComponent<BeamPointsFromTransforms>());

                Transform beamObject = laserTracerPrefab.transform.Find("BeamObject");
                beamObject.localScale = new Vector3(12f, 12f, 1f);
                laserTracerPrefab.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
                ParticleSystemRenderer particleSystemRenderer = beamObject.GetComponent<ParticleSystemRenderer>();
                Material matBeamObject = new Material(particleSystemRenderer.sharedMaterials[1]);
                matBeamObject.SetColor("_TintColor", new Color(1f, 1f, 1f));
                matBeamObject.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC1_Common_ColorRamps.texRampMinorConstructProjectile_png).WaitForCompletion());
                Material[] mats = particleSystemRenderer.sharedMaterials;
                mats[1] = matBeamObject;
                particleSystemRenderer.sharedMaterials = mats;
                AnimateShaderAlpha beamAlpha = beamObject.gameObject.AddComponent<AnimateShaderAlpha>();
                beamAlpha.timeMax = 1f;
                beamAlpha.continueExistingAfterTimeMaxIsReached = true;
                beamAlpha.alphaCurve = new AnimationCurve(
                    new Keyframe(0f, 1f, 0f, -5f),
                    new Keyframe(1f, 0f, 0f, 0f)
                );
                #endregion

                #region LaserMajorConstruct
                GameObject laserBeam = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(RoR2_DLC1_MajorAndMinorConstruct.LaserMajorConstruct_prefab).WaitForCompletion());

                Transform lineBeam = laserBeam.transform.Find("LaserStart");
                lineBeam.gameObject.name = "LineBeam";
                lineBeam.SetParent(laserTracerPrefab.transform, false);
                lineBeam.localPosition = Vector3.zero;
                AnimateShaderAlpha laserAlpha = lineBeam.GetComponent<AnimateShaderAlpha>();
                laserAlpha.timeMax = 0.5f;
                laserAlpha.alphaCurve = new AnimationCurve(
                    new Keyframe(0f, 1f),
                    new Keyframe(1f, 0f)
                );
                laserAlpha.continueExistingAfterTimeMaxIsReached = true;
                Object.Destroy(lineBeam.GetComponent<LineBetweenTransforms>());
                ScaleLineToTracer scaleLineToTracer = lineBeam.gameObject.AddComponent<ScaleLineToTracer>();
                LineRenderer lineRenderer = lineBeam.GetComponent<LineRenderer>();
                scaleLineToTracer.lineRenderer = lineBeam.GetComponent<LineRenderer>();
                scaleLineToTracer.targetTracer = laserTracerPrefab.GetComponent<Tracer>();
                foreach (Transform child in lineBeam.transform) {
                    Object.Destroy(child.gameObject);
                }
                Material matLineBeam = new Material(lineRenderer.sharedMaterials[1]);
                matLineBeam.SetColor("_TintColor", new Color(1f, 0.99f, 0.64f));
                matLineBeam.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC1_Common_ColorRamps.texRampMinorConstructProjectile_png).WaitForCompletion());
                mats = lineRenderer.sharedMaterials;
                mats[1] = matLineBeam;
                lineRenderer.sharedMaterials = mats;

                Transform laserEnd = laserBeam.transform.Find("LaserEnd");
                laserEnd.SetParent(laserTracerPrefab.transform, false);
                laserEnd.localPosition = Vector3.zero;
                Object.Destroy(laserEnd.transform.Find("PP").gameObject);
                Object.Destroy(laserEnd.transform.Find("AreaIndicator").gameObject);
                foreach (Transform child in laserEnd.transform) {
                    if (child.gameObject.name == "Point light") {
                        Object.Destroy(child.GetComponent<FlickerLight>());
                        child.GetComponent<Light>().intensity *= 2f;
                        child.GetComponent<Light>().range *= 2f;
                        LightIntensityCurve lightCurve = child.gameObject.AddComponent<LightIntensityCurve>();
                        lightCurve.timeMax = 0.6f;
                        lightCurve.curve = new AnimationCurve(
                            new Keyframe(0f, 1f, 0f, -5f),
                            new Keyframe(0.5f, 0f, 0f, 0f)
                        );
                        continue;
                    }
                    ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                    if (!particleSystem) {
                        continue;
                    }
                    ParticleSystem.MainModule main = particleSystem.main;
                    main.duration = 0.25f;
                    main.loop = false;
                }
                ShakeEmitter shakeEmitter = laserEnd.GetComponent<ShakeEmitter>();
                shakeEmitter.wave.amplitude = 0.2f;
                shakeEmitter.wave.frequency = 20f;
                shakeEmitter.duration = 0.15f;
                shakeEmitter.radius = 120f;

                Object.Destroy(laserBeam);
                #endregion
            });

            Content.CreateAndAddEffectDef(laserTracerPrefab);

            laserMuzzlePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Huntress.HuntressFireArrowRain_prefab).WaitForCompletion(), "DragonLaserMuzzleVFX", false);
            TryBuildAsset("Secondary Laser Muzzle VFX", () => {
                #region HuntressFireArrowRain
                laserMuzzlePrefab.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Medium;

                foreach (Transform child in laserMuzzlePrefab.transform) {
                    child.localScale = Vector3.one * 2f;
                }

                ParticleSystemRenderer flashRenderer = laserMuzzlePrefab.transform.Find("Flash, White (1)").GetComponent<ParticleSystemRenderer>();
                Material matFlash = new Material(flashRenderer.sharedMaterial);
                matFlash.SetColor("_TintColor", new Color(1f, 0.70f, 0.08f));
                flashRenderer.sharedMaterial = matFlash;

                ParticleSystemRenderer beamsRenderer = laserMuzzlePrefab.transform.Find("Beams").GetComponent<ParticleSystemRenderer>();
                Material matBeams = new Material(beamsRenderer.sharedMaterial);
                matBeams.SetColor("_TintColor", new Color(1f, 0.70f, 0.08f));
                beamsRenderer.sharedMaterial = matBeams;

                Transform dash = laserMuzzlePrefab.transform.Find("Dash");
                dash.localEulerAngles = new Vector3(270f, 0f, 0f);
                dash.localScale *= 1.5f;
                ParticleSystemRenderer dashRenderer = dash.GetComponent<ParticleSystemRenderer>();
                Material matDash = new Material(dashRenderer.sharedMaterial);
                matDash.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC1_Common_ColorRamps.texRampMinorConstructProjectile_png).WaitForCompletion());
                dashRenderer.sharedMaterial = matDash;

                ParticleSystemRenderer ringsRenderer = laserMuzzlePrefab.transform.Find("DashRings").GetComponent<ParticleSystemRenderer>();
                Material matRings = new Material(ringsRenderer.sharedMaterial);
                matRings.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC1_Common_ColorRamps.texRampMinorConstructProjectile_png).WaitForCompletion());
                ringsRenderer.sharedMaterial = matRings;

                ParticleSystemRenderer shockwaveRenderer = laserMuzzlePrefab.transform.Find("Shockwave").GetComponent<ParticleSystemRenderer>();
                Material matShockwave = new Material(shockwaveRenderer.sharedMaterial);
                matShockwave.SetTexture("_MainTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_ArtifactCompounds.texArtifactCompoundCircleMask_png).WaitForCompletion());
                matShockwave.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_DLC1_Common_ColorRamps.texRampMinorConstructProjectile_png).WaitForCompletion());
                shockwaveRenderer.sharedMaterial = matShockwave;
                shockwaveRenderer.alignment = ParticleSystemRenderSpace.View;

                laserMuzzlePrefab.transform.Find("Point light").GetComponent<Light>().color = new Color(1f, 0.70f, 0.08f);
                #endregion
            });

            Content.CreateAndAddEffectDef(laserMuzzlePrefab);

            laserHitEffectPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Common_VFX.OmniImpactVFXLightning_prefab).WaitForCompletion(), "DragonLaserHitVFX", false);
            TryBuildAsset("Secondary Laser Hit VFX", () => {
                #region OmniImpactVFXLightning
                foreach (Transform child in laserHitEffectPrefab.transform) {
                    ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule main = particleSystem.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.78f, 0.4f), new Color(1f, 0.47f, 0f));
                    child.transform.localScale = Vector3.one * 3f;
                }
                #endregion
            });

            Content.CreateAndAddEffectDef(laserHitEffectPrefab);
        }
    }
}
