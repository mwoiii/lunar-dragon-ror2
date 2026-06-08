using EntityStates;
using HG;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class ElementalBlitz : BaseState, SteppedSkillDef.IStepSetter {

        public enum Cannon {
            Left,
            Right,
            Middle
        }

        private static Vector3 projectileOriginOffset = Vector3.up * 1.5f;

        public GameObject projectilePrefab;

        public float baseDuration = 0.4f;

        public float paddingBetweenAttacks = 0.6f; // total delay is this plus baseduration

        public string attackSoundString;

        public string attackSoundStringAlt;

        public float attackSoundPitch;

        private float duration;

        private float maxDistance = 9999f;

        private bool hasFiredBlitz;

        private string muzzleString;

        private Transform muzzleTransform;

        private Cannon cannon;

        public static float recoilAmplitude;

        private string animationStateName;

        private GameObject muzzleflashEffectPrefab;

        private static Wave shakeWave = new Wave() {
            amplitude = 0.2f,
            frequency = 14f
        };

        private static GameObject muzzleflashLeft = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Common_VFX.OmniExplosionVFXQuick_prefab).WaitForCompletion();

        private static GameObject muzzleflashRight = LunarDragonAssets.iceballMuzzlePrefab;

        private static GameObject muzzleFlashCenter = LunarDragonAssets.laserMuzzlePrefab;

        public override void OnEnter() {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            characterBody.SetAimTimer(2f);

            switch (cannon) {
                case Cannon.Left:
                    muzzleString = "MuzzleLeft";
                    animationStateName = "PrimaryShoot1";
                    projectilePrefab = LunarDragonAssets.fireballPrefab;
                    muzzleflashEffectPrefab = muzzleflashLeft;
                    PlayCrossfade("Gesture1, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
                case Cannon.Right:
                    muzzleString = "MuzzleRight";
                    animationStateName = "PrimaryShoot2";
                    projectilePrefab = LunarDragonAssets.iceballPrefab;
                    muzzleflashEffectPrefab = muzzleflashRight;
                    PlayCrossfade("Gesture2, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
                case Cannon.Middle:
                    muzzleString = "MuzzleCenter";
                    animationStateName = "PrimaryShoot3";
                    muzzleflashEffectPrefab = muzzleFlashCenter;
                    PlayCrossfade("FullBody, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
            }

            if (GetModelChildLocator() is ChildLocator childLocator) {
                muzzleTransform = childLocator.FindChild(muzzleString);
            }
        }

        private void FireBlitzProjectile() {
            if (!hasFiredBlitz) {

                if (muzzleflashEffectPrefab) {
                    EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, gameObject, muzzleString, false);
                }

                if (!isAuthority) {
                    return;
                }

                Ray ray = GetAimRay();

                Vector3 direction = ray.direction;
                direction = TrajectoryAimAssist.ApplyTrajectoryAimAssist(direction, ray.origin, maxDistance, gameObject, gameObject, 1f);

                if (direction == ray.direction) {
                    GetMuzzleDirectionFromAimRay(ray, ref direction);
                }

                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                    projectilePrefab = projectilePrefab,
                    position = muzzleTransform.position + projectileOriginOffset,
                    rotation = Util.QuaternionSafeLookRotation(direction),
                    owner = gameObject,
                    damage = damageStat * LunarDragonStaticValues.primaryDamageCoefficient,
                    force = 150f,
                    crit = Util.CheckRoll(critStat, base.characterBody.master),
                    damageColorIndex = DamageColorIndex.Default,
                    damageTypeOverride = DamageTypeCombo.GenericPrimary
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);

                ApplyAirborneKnockback(ray.direction, 900f);
                AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            }
        }

        private void FireBlitzFinisher() {
            if (!hasFiredBlitz) {

                if (muzzleflashEffectPrefab) {
                    EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, gameObject, muzzleString, false);
                }

                ShakeEmitter.CreateSimpleShakeEmitter(transform.position, shakeWave, 0.15f, 120f, true);

                characterBody?.StartCoroutine(PlayLaserSFX());

                if (!isAuthority) {
                    return;
                }

                Ray ray = GetAimRay();

                Vector3 direction = ray.direction;

                GetMuzzleDirectionFromAimRay(ray, ref direction);

                BulletAttack bullet = new BulletAttack {
                    owner = characterBody.gameObject,
                    weapon = characterBody.gameObject,
                    origin = muzzleTransform.position + projectileOriginOffset,
                    aimVector = direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    bulletCount = 1U,
                    procCoefficient = 1f,
                    damage = characterBody.damage * LunarDragonStaticValues.primaryFinisherDamageCoefficient,
                    force = 250f,
                    radius = 5f,
                    falloffModel = BulletAttack.FalloffModel.None,
                    muzzleName = muzzleString,
                    isCrit = RollCrit(),
                    HitEffectNormal = false,
                    smartCollision = true,
                    maxDistance = maxDistance,
                    stopperMask = LayerIndex.world.mask,
                    damageType = new DamageTypeCombo {
                        damageType = DamageType.Stun1s,
                        damageTypeExtended = DamageTypeExtended.Generic,
                        damageSource = DamageSource.Primary,
                    },
                    tracerEffectPrefab = LunarDragonAssets.laserTracerPrefab,
                    hitEffectPrefab = LunarDragonAssets.laserHitEffectPrefab,
                };
                bullet.Fire();
                ApplyAirborneKnockback(ray.direction, 1500f);
                AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            }
        }

        private IEnumerator PlayLaserSFX() {
            if (gameObject) {
                Util.PlaySound("Play_falseson_skill4_laser_start", gameObject);
            }

            yield return new WaitForSeconds(0.5f);

            if (gameObject) {
                Util.PlaySound("Stop_falseson_skill4_laser_loop", gameObject);
                Util.PlaySound("Stop_falseson_skill4_laser_charge_loop", gameObject);
                Util.PlaySound("Play_falseson_skill4_laser_end", gameObject);
            }
        }

        private void GetMuzzleDirectionFromAimRay(Ray aimRay, ref Vector3 direction) {
            RaycastHit[] hits = Physics.RaycastAll(aimRay.origin, direction, 9999f, LayerIndex.CommonMasks.bullet);
            Array.Sort(hits, (hitA, hitB) => hitA.distance.CompareTo(hitB.distance));
            for (int i = 0; i < hits.Length; i++) {
                HurtBox hurtBox = hits[i].collider.AsValidOrNull()?.GetComponent<HurtBox>();
                if (hurtBox == null || (hurtBox && hurtBox.teamIndex != teamComponent.teamIndex)) {
                    direction = hits[i].point - (muzzleTransform.position + projectileOriginOffset);
                    break;
                }
            }
        }

        protected virtual bool OnBulletImpact(BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo) {
            return BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
        }

        private void ApplyAirborneKnockback(Vector3 aimDirection, float maxForce) {
            if (isGrounded) {
                return;
            }

            characterBody.characterMotor.ApplyForce(aimDirection * -maxForce);
        }


        public override void FixedUpdate() {
            base.FixedUpdate();
            if (fixedAge >= duration * 0.2f || hasFiredBlitz) {
                if (cannon == Cannon.Middle && !hasFiredBlitz) {
                    Util.PlayAttackSpeedSound(attackSoundStringAlt, gameObject, attackSoundPitch);
                    FireBlitzFinisher();
                    hasFiredBlitz = true;
                } else if (!hasFiredBlitz) {
                    Util.PlayAttackSpeedSound(attackSoundString, gameObject, attackSoundPitch);
                    FireBlitzProjectile();
                    hasFiredBlitz = true;
                }
                if (isAuthority && fixedAge >= (duration + paddingBetweenAttacks) / attackSpeedStat) {
                    outer.SetNextStateToMain();
                }
            }
        }

        public void SetStep(int i) {
            cannon = (Cannon)i;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
            writer.Write((byte)cannon);
        }

        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
            cannon = (Cannon)reader.ReadByte();
        }
    }
}