using EntityStates;
using HG;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class ElementalBlitz : BaseState, SteppedSkillDef.IStepSetter {
        // based off seeker

        public enum Cannon {
            Left,
            Right,
            Middle
        }

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

        public override void OnEnter() {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            characterBody.SetAimTimer(2f);

            switch (cannon) {
                case Cannon.Left:
                    muzzleString = "MuzzleLeft";
                    animationStateName = "PrimaryShoot1";
                    projectilePrefab = LunarDragonAssets.fireballPrefab;
                    PlayCrossfade("Gesture1, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
                case Cannon.Right:
                    muzzleString = "MuzzleRight";
                    animationStateName = "PrimaryShoot2";
                    projectilePrefab = LunarDragonAssets.iceballPrefab;
                    PlayCrossfade("Gesture2, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
                case Cannon.Middle:
                    muzzleString = "MuzzleCenter";
                    animationStateName = "PrimaryShoot3";
                    PlayCrossfade("FullBody, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
            }

            if (GetModelChildLocator() is ChildLocator childLocator) {
                muzzleTransform = childLocator.FindChild(muzzleString);
            }
        }

        private void FireBlitzProjectile() {
            if (!hasFiredBlitz && isAuthority) {
                Ray ray = GetAimRay();

                if (GetModelChildLocator() is ChildLocator childLocator) {
                    muzzleTransform = childLocator.FindChild(muzzleString);
                }

                // if ((bool)muzzleflashEffectPrefab) {
                // EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, base.gameObject, muzzleString, transmit: false);
                // }

                Vector3 direction = ray.direction;
                direction = TrajectoryAimAssist.ApplyTrajectoryAimAssist(direction, ray.origin, maxDistance, gameObject, gameObject, 2f);

                RaycastHit[] hits = Physics.RaycastAll(ray.origin, direction, 999f, LayerIndex.CommonMasks.bullet);
                for (int i = 0; i < hits.Length; i++) {
                    HurtBox hurtBox = hits[i].collider.AsValidOrNull()?.GetComponent<HurtBox>();
                    if (hurtBox && hurtBox.teamIndex != teamComponent.teamIndex) {
                        direction = hurtBox.transform.position - muzzleTransform.position;
                        break;
                    }
                }

                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                    projectilePrefab = projectilePrefab,
                    position = muzzleTransform.position,
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
            if (!hasFiredBlitz && isAuthority) {
                Ray ray = GetAimRay();
                BulletAttack bullet = new BulletAttack {
                    owner = characterBody.gameObject,
                    weapon = characterBody.gameObject,
                    origin = ray.origin + Vector3.up * 2f,
                    aimVector = ray.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    bulletCount = 1U,
                    procCoefficient = 1f,
                    damage = characterBody.damage * LunarDragonStaticValues.primaryFinisherDamageCoefficient,
                    force = 250f,
                    radius = 7f,
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
                    // tracerEffectPrefab = tracerEffectPrefab,
                    // hitEffectPrefab = hitEffectPrefab,
                };
                bullet.Fire();
                ApplyAirborneKnockback(ray.direction, 1500f);
                AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
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