using LunarDragonMod.Characters.Survivors.LunarDragon.Components;
using LunarDragonMod.Modules;
using LunarDragonMod.Modules.DamageTypes;
using LunarDragonMod.Survivors.LunarDragon.Components;
using R2API;
using RoR2;
using RoR2.CharacterSpeech;
using RoR2.Projectile;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Reflection;
using ThreeEyedGames;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonAssets {

        public static AssetBundle assetBundle;

        public static GameObject jetEffectPrefab;

        public static GameObject fireballPrefab;

        public static GameObject fireballImpactPrefab;

        public static GameObject iceballPrefab;

        public static GameObject iceballImpactPrefab;

        public static GameObject iceballMuzzlePrefab;

        public static GameObject heavyFireballPrefab;

        public static GameObject heavyFireballMuzzlePrefab;

        public static GameObject heavyFireballImpactPrefab;

        public static GameObject heavyFireballPlumePrefab;

        public static GameObject heavyFireballPlumeLargePrefab;

        public static GameObject plumeShakeSFX;

        public static GameObject heavyIceballPrefab;

        public static GameObject heavyPlasmaballPrefab;

        public static GameObject laserTracerPrefab;

        public static GameObject laserMuzzlePrefab;

        public static GameObject laserHitEffectPrefab;

        public static GameObject utilitySmokeEffect;

        public static GameObject utilityDashLightEffect;

        public static GameObject utilityDashMediumEffect;

        public static GameObject utilityDashHeavyEffect;

        public static GameObject fireTrailPrefab;

        public static GameObject impactDecalBase;

        public static Material iceDecalMaterial;

        public static GameObject displayEffectPrefab;

        public static GameObject specialLiftoffSmokeEffect;

        public static GameObject specialLiftoffExplosionEffect;

        public static AnimationCurveData specialAmbushRisingData;

        public static AnimationCurveData specialAmbushDescendingData;

        public static CharacterSpeechController.SpeechInfo[] seeDragonResponses;

        public static CharacterSpeechController.SpeechInfo[] killDragonResponses;

        public static CharacterSpeechController.SpeechInfo[] killHurtDragonResponses;

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
            LoadAssetBundle("mwmwlunardragonbundle");

            LunarDragonPlugin.instance.StartCoroutine(ShaderSwapper.ShaderSwapper.UpgradeStubbedShadersAsync(assetBundle));

            TryBuildAsset("Impact Decal Base", CreateImpactDecalBase);

            TryBuildAsset("Jet Effect", CreateJetEffect);

            TryBuildAsset("Primary Fireball", CreateFireball);
            TryBuildAsset("Secondary Fireball", CreateHeavyFireball); // depends on above to be made first

            TryBuildAsset("Primary Iceball", CreateIceball);
            TryBuildAsset("Secondary Iceball", CreateHeavyIceball);

            TryBuildAsset("Primary Laser", CreateLaser);
            TryBuildAsset("Secondary Plasmaball", CreateHeavyPlasmaball);

            TryBuildAsset("Utility Smoke", CreateDashSmoke);
            TryBuildAsset("Utility Dash Explosions", CreateDashExplosions);
            TryBuildAsset("Utility Fire Trail", CreateFireTrail);

            TryBuildAsset("Display Effect", CreateDisplayEffect);

            TryBuildAsset("Special Liftoff Effects", CreateAmbushLiftoffEffects);
            TryBuildAsset("Special Motion Data", GetAmbushMotionData);

            TryBuildAsset("Mithrix Dialogue", CreateMithrixDialogue);
        }

        private static void TryBuildAsset(string assetName, System.Action buildAction) {
            try {
                buildAction();
            } catch (System.Exception e) {
                Log.Warning($"Failed to complete building asset {assetName}!\n\n{e}");
            }
        }

        private static void CreateImpactDecalBase() {
            impactDecalBase = PrefabAPI.CreateEmptyPrefab("ImpactDecal", false);

            TryBuildAsset("Impact Decal Components", () => {
                MeshRenderer renderer = impactDecalBase.AddComponent<MeshRenderer>();
                renderer.sharedMaterials = new Material[0];
                impactDecalBase.AddComponent<MeshFilter>().sharedMesh = Addressables.LoadAssetAsync<Mesh>(Decalicious.DecalCube_asset).WaitForCompletion();
                impactDecalBase.AddComponent<SetRandomRotation>().setRandomYRotation = true;
                Decal decal = impactDecalBase.AddComponent<Decal>();
                decal.Fade = 1f;
                decal.Material = Addressables.LoadAssetAsync<Material>(RoR2_Base_SurvivorPod.matPodImpactDecal_mat).WaitForCompletion();
                AnimateShaderAlpha shaderAlpha = impactDecalBase.AddComponent<AnimateShaderAlpha>();
                shaderAlpha.decal = decal;
                shaderAlpha.alphaCurve = new AnimationCurve(
                    new Keyframe(0f, 1f, 0f, -5f),
                    new Keyframe(3f, 0f, 0f, 0f)
                );
            });
        }

        private static void CreateJetEffect() {
            jetEffectPrefab = PrefabAPI.CreateEmptyPrefab("JetEffect", false);
            jetEffectPrefab.transform.localScale = new Vector3(0.1834f, 0.1834f, 1.572f);
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

            TryBuildAsset("Primary Fireball Impact Explosion", () => {
                fireballImpactPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_LemurianBruiser.OmniExplosionVFXLemurianBruiserFireballImpact_prefab).WaitForCompletion(), "DragonFireballImpact", false);
                GameObject impactDecal = Object.Instantiate(impactDecalBase, fireballImpactPrefab.transform, false);
                Material matDecal = new Material(Addressables.LoadAssetAsync<Material>(RoR2_DLC2_Chef.matChefOilPoolFireDecal_mat).WaitForCompletion());
                matDecal.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampDroneFire_png).WaitForCompletion());
                impactDecal.GetComponent<Decal>().Material = matDecal;
                impactDecal.name = "FireImpactDecal";
                impactDecal.transform.localScale = Vector3.one * 0.8f;
            });

            fireballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = fireballImpactPrefab;
            Content.CreateAndAddEffectDef(fireballImpactPrefab);

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

            iceballImpactPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_Chef.BoostedProjectileExplosionVFX_prefab).WaitForCompletion(), "IceBallExplosionVFX", false);
            TryBuildAsset("Primary Iceball Impact Explosion", () => {
                #region BoostedProjectileExplosionVFXimpactEffect.GetComponent<EffectComponent>().soundName = "Play_mage_shift_wall_explode";
                foreach (Transform child in iceballImpactPrefab.transform.Find("Dash, Bright")) {
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

                ShakeEmitter shakeEmitter = iceballImpactPrefab.GetComponent<ShakeEmitter>();
                shakeEmitter.wave.amplitude = 0.2f;
                shakeEmitter.wave.frequency = 12f;
                shakeEmitter.duration = 0.15f;
                shakeEmitter.radius = 120f;

                GameObject impactDecal = Object.Instantiate(impactDecalBase, iceballImpactPrefab.transform, false);
                Decal decal = impactDecal.GetComponent<Decal>();
                iceDecalMaterial = new Material(decal.Material);
                iceDecalMaterial.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampIce_png).WaitForCompletion());
                iceDecalMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 0.28f));
                decal.Material = iceDecalMaterial;
                impactDecal.name = "IceImpactDecal";
                impactDecal.transform.localScale = Vector3.one * 10f;
                #endregion
            });

            Content.CreateAndAddEffectDef(iceballImpactPrefab);
            iceballPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = iceballImpactPrefab;

            Content.AddProjectilePrefab(iceballPrefab);
        }

        private static void CreateHeavyFireball() {
            heavyFireballPrefab = assetBundle.LoadAsset<GameObject>("HeavyFireballProjectile");

            GameObject heavyFireballEruption = assetBundle.LoadAsset<GameObject>("FireballEruption");

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

                Object.Destroy(heavyFireballGhost.transform.Find("Sparks, Trail").gameObject);

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

                Object.Destroy(heavyFireballPlumePrefab.GetComponent<DestroyOnParticleEnd>());
                heavyFireballPlumePrefab.AddComponent<DestroyOnTimer>().duration = 2f;

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

                GameObject impactDecal = Object.Instantiate(impactDecalBase, heavyFireballPlumePrefab.transform, false); AnimateShaderAlpha impactAlpha = impactDecal.GetComponent<AnimateShaderAlpha>();
                impactAlpha.timeMax = 3f;
                impactAlpha.alphaCurve = new AnimationCurve(
                    new Keyframe(0f, 1f, 0f, -5f),
                    new Keyframe(1f, 0f, 0f, 0f)
                );
                Material matDecal = new Material(Addressables.LoadAssetAsync<Material>(RoR2_DLC2_Chef.matChefOilPoolFireDecal_mat).WaitForCompletion());
                matDecal.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampLunarWispFire_png).WaitForCompletion());
                impactDecal.GetComponent<Decal>().Material = matDecal;
                impactDecal.name = "LunarFireImpactDecal";
                impactDecal.transform.localScale = Vector3.one * 8f;
            });

            Content.CreateAndAddEffectDef(heavyFireballPlumePrefab);

            plumeShakeSFX = PrefabAPI.CreateEmptyPrefab("FireballHeavyPlume Shake, SFX", true);
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

            heavyFireballPlumeLargePrefab = heavyFireballPlumePrefab.InstantiateClone("FireballHeavyPlumeLargeVFX", false);
            TryBuildAsset("Secondary Fireball Plume Large VFX", () => {

                Object.Destroy(heavyFireballPlumeLargePrefab.transform.Find("LunarFireImpactDecal").gameObject);

                heavyFireballPlumePrefab.GetComponent<DestroyOnTimer>().duration = 5f;

                heavyFireballPlumeLargePrefab.transform.Find("ParticleLoop/Debris, 3D").localScale = Vector3.one * 2f;

                heavyFireballPlumeLargePrefab.transform.Find("ParticleLoop/DirtMounts").localScale *= 2f;

                Transform flames = heavyFireballPlumeLargePrefab.transform.Find("InitialBurst/Flames_Ps");
                ParticleSystem ps = flames.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = ps.main;
                main.startSizeX = new ParticleSystem.MinMaxCurve(8f, 12f);
                main.startSizeY = new ParticleSystem.MinMaxCurve(16f, 24f);
                main.startSizeZ = new ParticleSystem.MinMaxCurve(8f, 12f);

                GameObject impactDecal = Object.Instantiate(impactDecalBase, heavyFireballPlumeLargePrefab.transform, false);
                Material matDecal = new Material(Addressables.LoadAssetAsync<Material>(RoR2_DLC2_Chef.matChefOilPoolFireDecal_mat).WaitForCompletion());
                matDecal.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampLunarWispFire_png).WaitForCompletion());
                impactDecal.GetComponent<Decal>().Material = matDecal;
                impactDecal.name = "LunarFireImpactDecalLarge";
                impactDecal.transform.localScale = Vector3.one * 20f;

            });

            Content.CreateAndAddEffectDef(heavyFireballPlumeLargePrefab);

            PrefabAPI.RegisterNetworkPrefab(heavyFireballPrefab);
            Content.AddProjectilePrefab(heavyFireballPrefab);

            PrefabAPI.RegisterNetworkPrefab(heavyFireballEruption);
            Content.AddProjectilePrefab(heavyFireballEruption);
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

                GameObject impactDecal = Object.Instantiate(impactDecalBase, laserEnd, false);
                AnimateShaderAlpha impactAlpha = impactDecal.GetComponent<AnimateShaderAlpha>();
                impactAlpha.timeMax = 0.55f;
                impactAlpha.alphaCurve = new AnimationCurve(
                    new Keyframe(0f, 1f, 0f, 0f),
                    new Keyframe(0.5f, 0.7f, 0f, -5f),
                    new Keyframe(1f, 0f, 0f, 0f)
                );
                impactDecal.name = "LaserImpactDecal";
                impactDecal.transform.localScale = Vector3.one * 7f;

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

        private static void CreateDashSmoke() {
            utilitySmokeEffect = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Bandit2.Bandit2SmokeBomb_prefab).WaitForCompletion().InstantiateClone("DragonUtilitySmoke", false);
            TryBuildAsset("Utility Smoke Effect", () => {
                Transform core = utilitySmokeEffect.transform.Find("Core");
                core.transform.localScale = Vector3.one * 0.7f;
                core.Find("Debris, 3D").gameObject.SetActive(true);
                core.Find("Debris").gameObject.SetActive(true);
                core.Find("Dust").gameObject.SetActive(true);
                Object.Destroy(core.Find("Dust, CenterSphere").gameObject);
                foreach (Transform child in core) {
                    core.localScale = Vector3.one * 0.5f;
                }
            });

            Content.CreateAndAddEffectDef(utilitySmokeEffect);
        }

        private static void CreateDashExplosions() {

            utilityDashHeavyEffect = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_LunarGolem.LunarGolemDeath_prefab).WaitForCompletion().InstantiateClone("DragonUtilityExplosionHeavy", false);
            TryBuildAsset("Utility Heavy Explosion Effect", () => {
                utilityDashHeavyEffect.GetComponent<EffectComponent>().soundName = "Play_MULT_m1_grenade_launcher_explo";
                ShakeEmitter shake = utilityDashHeavyEffect.GetComponent<ShakeEmitter>();
                shake.startDelay = 0f;
                shake.duration = 0.4f;
                shake.wave.amplitude = 1.8f;
                shake.wave.frequency = 120f;
                Transform particles = utilityDashHeavyEffect.transform.Find("Particles");
                particles.transform.localScale = Vector3.one * 2f;
                Light pointLight = particles.Find("Point light").GetComponent<Light>();
                pointLight.color = new Color(1f, 0.72f, 0f);
                pointLight.range = 30f;
                ParticleSystemRenderer psr = particles.Find("Fire").GetComponent<ParticleSystemRenderer>();
                Material matFire = new Material(psr.sharedMaterial);
                matFire.shader = Addressables.LoadAssetAsync<Shader>(RoR2_Base_Shaders.HGOpaqueCloudRemap_shader).WaitForCompletion();
                matFire.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampMageFire_png).WaitForCompletion());
                psr.sharedMaterial = matFire;
                ParticleSystem ps = particles.Find("Sparks_Ps").GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = ps.main;
                main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.55f, 0.22f), new Color(0.74f, 0.16f, 0f));
                Object.Destroy(particles.Find("Fire, Linger").gameObject);
                psr = particles.Find("Flash, White").GetComponent<ParticleSystemRenderer>();
                psr.maxParticleSize = 50f;
                psr.transform.localScale = Vector3.one * 0.15f;
            });

            utilityDashMediumEffect = utilityDashHeavyEffect.InstantiateClone("DragonUtilityExplosionMedium", false);
            TryBuildAsset("Utility Medium Explosion Effect", () => {
                ShakeEmitter shake = utilityDashMediumEffect.GetComponent<ShakeEmitter>();
                shake.duration = 0.36f;
                shake.wave.amplitude = 1.4f;
                shake.wave.frequency = 80f;
                Transform particles = utilityDashMediumEffect.transform.Find("Particles");
                particles.transform.localScale = Vector3.one * 2f;
                particles.Find("Sparks_Ps").transform.localScale = Vector3.one * 0.5f;
                particles.Find("Point light").GetComponent<Light>().range = 15f;
                Object.Destroy(particles.Find("Fire, Linger").gameObject);
                Object.Destroy(particles.Find("RockBurst_Ps").gameObject);
            });

            utilityDashLightEffect = utilityDashMediumEffect.InstantiateClone("DragonUtilityExplosionLight", false);
            TryBuildAsset("Utility Light Explosion Effect", () => {
                utilityDashLightEffect.GetComponent<EffectComponent>().soundName = "";
                ShakeEmitter shake = utilityDashLightEffect.GetComponent<ShakeEmitter>();
                shake.duration = 0.3f;
                shake.wave.amplitude = 1f;
                shake.wave.frequency = 60f;
                Transform particles = utilityDashLightEffect.transform.Find("Particles");
                particles.transform.localScale = Vector3.one * 1.2f;
                particles.Find("Point light").GetComponent<Light>().range = 10f;
                Object.Destroy(particles.Find("Fire, Linger").gameObject);
                Object.Destroy(particles.Find("RockBurst_Ps").gameObject);
                Object.Destroy(particles.Find("Sparks_Ps").gameObject);
                Object.Destroy(particles.Find("PP").gameObject);
            });

            PlaySoundOnDelay extraSound = utilityDashHeavyEffect.AddComponent<PlaySoundOnDelay>();
            extraSound.soundString = "Play_GG_Tanker_PuddleIgnite";
            extraSound.delay = 0f;

            Content.CreateAndAddEffectDef(utilityDashLightEffect);
            Content.CreateAndAddEffectDef(utilityDashMediumEffect);
            Content.CreateAndAddEffectDef(utilityDashHeavyEffect);
        }

        private static void CreateFireTrail() {
            GameObject dragonFireSegment = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Common.FireTrailSegment_prefab).WaitForCompletion().InstantiateClone("DragonFireTrailSegment", false);
            TryBuildAsset("Utility Fire Trail Segment Effect", () => {
                dragonFireSegment.transform.localScale = new Vector3(1f, 10f, 10f);
                ParticleSystem ps = dragonFireSegment.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = ps.main;
                main.duration = 0.7f;
                main.startLifetime = new ParticleSystem.MinMaxCurve(1.5f, 2.5f);
                main.startSizeX = new ParticleSystem.MinMaxCurve(5f, 7.5f);
                main.startSizeY = new ParticleSystem.MinMaxCurve(1.25f, 2.5f);
                ParticleSystem.ShapeModule shape = ps.shape;
                shape.scale = Vector3.one;
                ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
                ParticleSystem.MinMaxGradient color = colorOverLifetime.color;
                color.gradient.alphaKeys = new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f),
                };
                ParticleSystemRenderer psr = dragonFireSegment.GetComponent<ParticleSystemRenderer>();
                Material matFire = new Material(psr.sharedMaterial);
                matFire.SetFloat("_AlphaBoost", 0.96f);
                psr.sharedMaterial = matFire;

                GameObject segmentSmoke = Object.Instantiate(dragonFireSegment, dragonFireSegment.transform, false);
                Object.Destroy(segmentSmoke.GetComponent<DestroyOnTimer>());
                segmentSmoke.transform.localScale = Vector3.one;
                segmentSmoke.transform.localPosition = Vector3.zero;
                ps = segmentSmoke.GetComponent<ParticleSystem>();
                main = ps.main;
                main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.98f, 0.05f, 0f), Color.black);
                colorOverLifetime = ps.colorOverLifetime;
                color = colorOverLifetime.color;
                color.gradient.colorKeys = new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.black, 0.6f),
                };
                colorOverLifetime.color = color;
                psr = segmentSmoke.GetComponent<ParticleSystemRenderer>();
                Material matSmoke = new Material(psr.sharedMaterial);
                matSmoke.shader = Addressables.LoadAssetAsync<Shader>(RoR2_Base_Shaders.HGOpaqueCloudRemap_shader).WaitForCompletion();
                matSmoke.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>(RoR2_Base_Common_ColorRamps.texRampTritone3_png).WaitForCompletion());
                matSmoke.SetFloat("_Cutoff", 0.73f);
                matSmoke.SetFloat("_AlphaBoost", 1.3f);
                matSmoke.SetInt("_RampInfo", 5);
                psr.sharedMaterial = matSmoke;
            });

            fireTrailPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Common.FireTrail_prefab).WaitForCompletion().InstantiateClone("DragonFireTrail", false);
            TryBuildAsset("Utility Fire Trail Prefab", () => {
                Object.Destroy(fireTrailPrefab.GetComponent<DamageTrail>());
                DamageTrailDynamic damageTrail = fireTrailPrefab.AddComponent<DamageTrailDynamic>();
                damageTrail.pointUpdateInterval = 0.25f;
                damageTrail.damageUpdateInterval = 0.2f;
                damageTrail.radius = 3f;
                damageTrail.height = 0.5f;
                damageTrail.pointLifetime = 1.5f;
                damageTrail.damageType = DamageType.Generic;
                damageTrail.segmentPrefab = dragonFireSegment;
            });

            //fireTrailPrefab.AddComponent<NetworkIdentity>();
            //fireTrailPrefab.RegisterNetworkPrefab();
            //dragonFireSegment.AddComponent<EffectComponent>();
            //Content.CreateAndAddEffectDef(dragonFireSegment);
        }

        private static void CreateDisplayEffect() {
            displayEffectPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Brother.BrotherSlamImpact_prefab).WaitForCompletion().InstantiateClone("DragonDisplayEffect", false);
            TryBuildAsset("Display Effect VFX", () => {
                ShakeEmitter[] shakes = displayEffectPrefab.GetComponents<ShakeEmitter>();
                foreach (ShakeEmitter shake in shakes) {
                    Object.Destroy(shake);
                }
                Object.Destroy(displayEffectPrefab.GetComponent<DestroyOnTimer>());

                Transform spikes = displayEffectPrefab.transform.Find("Spikes, Small");
                spikes.localPosition = Vector3.zero;
                spikes.localScale = new Vector3(0.25f, 0.25f, 0.2f);
                ScaleOverTime scaleOverTime = spikes.gameObject.AddComponent<ScaleOverTime>();
                scaleOverTime.separateAxes = true;
                scaleOverTime.zCurve = new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 2f),
                    new Keyframe(0.048f, 0.209f, 0f, 0f),
                    new Keyframe(0.246f, 0.16f, 0.2f, 0f)
                ) {
                    postWrapMode = WrapMode.ClampForever
                };
                ParticleSystem ps = spikes.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = ps.main;
                main.startLifetime = float.PositiveInfinity;
                ParticleSystem.ShapeModule shape = ps.shape;
                shape.radiusThickness = 0.5f;
                ParticleSystem.EmissionModule emission = ps.emission;
                emission.burstCount = 1;
                ParticleSystem.Burst burst = emission.GetBurst(0);
                burst.cycleCount = 1;
                burst.count = 16;
                emission.SetBurst(0, burst);
                ParticleSystem.SizeOverLifetimeModule size = ps.sizeOverLifetime;
                size.enabled = false;

                Transform decalTransform = displayEffectPrefab.transform.Find("Decal");
                decalTransform.localPosition = Vector3.zero;
                decalTransform.localScale = Vector3.one * 2f;
                Decal decal = decalTransform.GetComponent<Decal>();
                decal.Material = iceDecalMaterial;
                decal.Fade = 0.2f;
                decalTransform.gameObject.AddComponent<SetRandomRotation>().setRandomYRotation = true;
                Object.Destroy(decalTransform.GetComponent<AnimateShaderAlpha>());
            });
        }

        private static void CreateAmbushLiftoffEffects() {
            specialLiftoffExplosionEffect = utilityDashHeavyEffect.InstantiateClone("DragonSpecialExplosion", false);
            TryBuildAsset("Special Liftoff Explosion", () => {
                ShakeEmitter shake = specialLiftoffExplosionEffect.GetComponent<ShakeEmitter>();
                shake.radius = 40f;
                shake.duration = 0.8f;
                shake.wave.amplitude = 3.5f;

                Transform particles = specialLiftoffExplosionEffect.transform.Find("Particles");
                particles.localScale *= 3f;
                particles.Find("Fire").localScale = Vector3.one * 0.5f;
                Object.Destroy(particles.Find("Fire, Linger").gameObject);
            });

            Content.CreateAndAddEffectDef(specialLiftoffExplosionEffect);

            specialLiftoffSmokeEffect = utilitySmokeEffect.InstantiateClone("DragonSpecialSmoke", false);
            TryBuildAsset("Special Liftoff Smoke Effect", () => {
                Transform core = utilitySmokeEffect.transform.Find("Core");
                foreach (Transform child in core) {
                    core.localScale = Vector3.one * 5f;
                }
            });

            Content.CreateAndAddEffectDef(specialLiftoffSmokeEffect);
        }

        private static void GetAmbushMotionData() {
            specialAmbushRisingData = assetBundle.LoadAsset<AnimationCurveData>("RisingData");
            specialAmbushDescendingData = assetBundle.LoadAsset<AnimationCurveData>("DescendingData");
        }

        private static void CreateMithrixDialogue() {
            // duping the last entry because SendResponseFromPool has a bug where it be skipping the last one
            // they didn't nose that random.range is end exclusive... and still did length - 1.....
            seeDragonResponses = new CharacterSpeechController.SpeechInfo[] {
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixDialogueSee1,
                    duration = 2f,
                    maxWait = 0.5f,
                    priority = 10000,
                    mustPlay = true
                },
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixDialogueSee2,
                    duration = 2f,
                    maxWait = 0.5f,
                    priority = 10000,
                    mustPlay = true
                },
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixDialogueSee2,
                    duration = 2f,
                    maxWait = 0.5f,
                    priority = 10000,
                    mustPlay = true
                },
            };

            killDragonResponses = new CharacterSpeechController.SpeechInfo[] {
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixDialogueKill1,
                    duration = 1f,
                    maxWait = 0.1f,
                    priority = 10,
                    mustPlay = true
                },
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixDialogueKill2,
                    duration = 1f,
                    maxWait = 0.1f,
                    priority = 10,
                    mustPlay = true
                },
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixDialogueKill2,
                    duration = 1f,
                    maxWait = 0.1f,
                    priority = 10,
                    mustPlay = true
                },
            };

            killHurtDragonResponses = new CharacterSpeechController.SpeechInfo[] {
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixHurtDialogueKill1,
                    duration = 1f,
                    maxWait = 0.1f,
                    priority = 10,
                    mustPlay = true
                },
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixHurtDialogueKill2,
                    duration = 1f,
                    maxWait = 0.1f,
                    priority = 10,
                    mustPlay = true
                },
                new CharacterSpeechController.SpeechInfo() {
                    token = LunarDragonTokens.mithrixHurtDialogueKill2,
                    duration = 1f,
                    maxWait = 0.1f,
                    priority = 10,
                    mustPlay = true
                },
            };
        }
    }
}