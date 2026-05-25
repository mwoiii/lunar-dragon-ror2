using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class ElementalBlitz : BaseState, SteppedSkillDef.IStepSetter {
        // based off seeker
        // 
        // thanks, seeker

        public enum Cannon {
            Left,
            Right,
            Middle
        }

        public GameObject projectilePrefab;

        // public GameObject muzzleflashEffectPrefab;

        public float baseDuration = 0.4f;

        public float paddingBetweenAttacks = 0.6f; // total delay is this plus baseduration

        public string attackSoundString;

        public string attackSoundStringAlt;

        public float attackSoundPitch;

        private float duration;

        private float maxDistance = 600f;

        private bool hasFiredBlitz;

        private string muzzleString;

        private Transform muzzleTransform;

        private Animator animator;

        private ChildLocator childLocator;

        private Cannon cannon;

        public static float recoilAmplitude;

        private string animationStateName;

        public void SetStep(int i) {
            cannon = (Cannon)i;
        }

        public override void OnEnter() {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            base.characterBody.SetAimTimer(2f);

            animator = GetModelAnimator();
            if ((bool)animator) {
                childLocator = animator.GetComponent<ChildLocator>();
            }

            switch (cannon) {
                case Cannon.Left:
                    muzzleString = "MuzzleLeft";
                    animationStateName = "PrimaryShoot1";
                    PlayCrossfade("Gesture1, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
                case Cannon.Right:
                    muzzleString = "MuzzleRight";
                    animationStateName = "PrimaryShoot2";
                    PlayCrossfade("Gesture2, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
                case Cannon.Middle:
                    muzzleString = "MuzzleCenter";
                    animationStateName = "PrimaryShoot3";
                    PlayCrossfade("FullBody, Additive", animationStateName, "Blitz.playbackRate", duration, 0.025f);
                    break;
            }

            /*
            bool isMoving = animator.GetBool("isMoving");
            bool isGrounded = animator.GetBool("isGrounded");
            if (!isMoving && isGrounded) {
                PlayCrossfade("FullBody, Override", animationStateName, "FireGauntlet.playbackRate", duration, 0.025f);
                return;
            }
            PlayCrossfade("Gesture, Additive", animationStateName, "FireGauntlet.playbackRate", duration, 0.025f);
            */
        }

        public override void OnExit() {
            base.OnExit();
        }

        private void FireBlitzProjectile() {
            if (!hasFiredBlitz) {
                // base.characterBody.AddSpreadBloom(bloom);
                Ray ray = GetAimRay();
                // TrajectoryAimAssist.ApplyTrajectoryAimAssist(ref ray, projectilePrefab, base.gameObject);

                if ((bool)childLocator) {
                    muzzleTransform = childLocator.FindChild(muzzleString);
                }

                // if ((bool)muzzleflashEffectPrefab) {
                // EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, base.gameObject, muzzleString, transmit: false);
                // }

                switch (cannon) {
                    case Cannon.Left:
                        projectilePrefab = LunarDragonAssets.fireballPrefab;
                        break;
                    case Cannon.Right:
                        projectilePrefab = LunarDragonAssets.iceballPrefab;
                        break;
                }

                // if aiming at something up close, adjust direction so projectile will still hit despite muzzle distance
                Vector3 direction = ray.direction;
                if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, LayerIndex.world.mask)) {
                    direction = hitInfo.point - muzzleTransform.position;
                }

                direction = TrajectoryAimAssist.ApplyTrajectoryAimAssist(direction, muzzleTransform.position, maxDistance, gameObject, gameObject, 1f);


                if (base.isAuthority) {
                    FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                    fireProjectileInfo.projectilePrefab = projectilePrefab;
                    fireProjectileInfo.position = muzzleTransform.position;
                    fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(direction);
                    fireProjectileInfo.owner = base.gameObject;
                    fireProjectileInfo.damage = damageStat * LunarDragonStaticValues.primaryDamageCoefficient;
                    fireProjectileInfo.force = 150f;
                    fireProjectileInfo.crit = Util.CheckRoll(critStat, base.characterBody.master);
                    fireProjectileInfo.damageColorIndex = DamageColorIndex.Default;
                    fireProjectileInfo.damageTypeOverride = DamageTypeCombo.GenericPrimary;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
                ApplyAirborneKnockback(ray.direction, 900f);
                AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            }
        }

        private void FireBlitzFinisher() {
            if (!hasFiredBlitz) {
                Ray ray = GetAimRay();

                if (isAuthority) {
                    BulletAttack bullet = new BulletAttack {
                        owner = characterBody.gameObject,
                        weapon = characterBody.gameObject,
                        origin = ray.origin,
                        aimVector = ray.direction,
                        minSpread = 0f,
                        maxSpread = 0f,
                        bulletCount = 1U,
                        procCoefficient = 1f,
                        damage = characterBody.damage * LunarDragonStaticValues.primaryFinisherDamageCoefficient,
                        force = 250f,
                        radius = 7f,
                        falloffModel = BulletAttack.FalloffModel.None,
                        // tracerEffectPrefab = tracerEffectPrefab,
                        muzzleName = muzzleString,
                        // hitEffectPrefab = hitEffectPrefab,
                        isCrit = RollCrit(),
                        HitEffectNormal = false,
                        stopperMask = LayerIndex.world.mask,
                        smartCollision = true,
                        damageType = new DamageTypeCombo {
                            damageType = DamageType.Stun1s,
                            damageTypeExtended = DamageTypeExtended.Generic,
                            damageSource = DamageSource.Primary,
                        },
                        maxDistance = maxDistance
                    };
                    bullet.Fire();
                }
                ApplyAirborneKnockback(ray.direction, 1500f);
                AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            }
        }

        private void ApplyAirborneKnockback(Vector3 aimDirection, float maxForce) {
            if (base.isGrounded) {
                return;
            }

            characterBody.characterMotor.ApplyForce(aimDirection * -maxForce);
        }


        public override void FixedUpdate() {
            base.FixedUpdate();
            if (base.fixedAge >= duration * 0.2f || hasFiredBlitz) {
                if (cannon == Cannon.Middle && !hasFiredBlitz) {
                    Util.PlayAttackSpeedSound(attackSoundStringAlt, base.gameObject, attackSoundPitch);
                    FireBlitzFinisher();
                    hasFiredBlitz = true;
                } else if (!hasFiredBlitz) {
                    Util.PlayAttackSpeedSound(attackSoundString, base.gameObject, attackSoundPitch);
                    FireBlitzProjectile();
                    hasFiredBlitz = true;
                }
                if (base.isAuthority && base.fixedAge >= (duration + paddingBetweenAttacks) / attackSpeedStat) {
                    outer.SetNextStateToMain();
                }
            }
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