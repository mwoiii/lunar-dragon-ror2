using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates.BaseStates {

    public class SecondaryBase : BaseSkillState {

        // public GameObject muzzleflashEffectPrefab;

        protected virtual GameObject projectilePrefab => LunarDragonAssets.heavyFireballPrefab;

        protected virtual float damageCoefficient => LunarDragonStaticValues.secondaryFireBlastDamageCoefficient;

        protected virtual float baseDuration => 0.7f;

        protected virtual string attackSoundString => "";

        protected virtual float attackSoundPitch => 1f;

        protected virtual string muzzleString => "MuzzleLeft";

        protected virtual GameObject muzzleflashEffectPrefab => null;

        protected virtual string animationLayerName => "Gesture1, Additive";

        protected virtual string animationStateName => "PrimaryShoot1";

        protected virtual float recoilAmplitude => 0f;

        protected virtual float fireDelay => 0.1f;

        protected virtual bool canLeaveStateAfterFire => true;

        private float duration;

        private Transform muzzleTransform;

        private Animator animator;

        private ChildLocator childLocator;

        private bool hasFiredHeavyBlitz;

        private bool holdingKeyFromStart = true;

        protected bool IsNewKeyDownAuthority => IsKeyDownAuthority() && !holdingKeyFromStart;

        public override void OnEnter() {
            base.OnEnter();

            duration = baseDuration / attackSpeedStat;

            characterBody.SetAimTimer(2f);

            animator = GetModelAnimator();
            if ((bool)animator) {
                childLocator = animator.GetComponent<ChildLocator>();
            }

            PlayCrossfade(animationLayerName, animationStateName, "Blitz.playbackRate", duration, 0.025f);

        }

        public override void OnExit() {
            base.OnExit();
        }

        protected virtual void FireBlitzProjectile() {
            if (!hasFiredHeavyBlitz) {
                // base.characterBody.AddSpreadBloom(bloom);
                Ray ray = GetAimRay();
                // TrajectoryAimAssist.ApplyTrajectoryAimAssist(ref ray, projectilePrefab, base.gameObject);

                if ((bool)childLocator) {
                    muzzleTransform = childLocator.FindChild(muzzleString);
                }

                if (muzzleflashEffectPrefab) {
                    EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, gameObject, muzzleString, transmit: false);
                }

                // if aiming at something up close, adjust direction so projectile will still hit despite muzzle distance
                Vector3 direction = ray.direction;
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 600f, LayerIndex.world.mask)) {
                    direction = hitInfo.point - muzzleTransform.position;
                }

                if (isAuthority) {
                    FireProjectileInfo fireProjectileInfo = default;
                    fireProjectileInfo.projectilePrefab = projectilePrefab;
                    fireProjectileInfo.position = muzzleTransform.position;
                    fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(direction);
                    fireProjectileInfo.owner = gameObject;
                    fireProjectileInfo.damage = damageStat * damageCoefficient;
                    fireProjectileInfo.force = 300f;
                    fireProjectileInfo.crit = Util.CheckRoll(critStat, characterBody.master);
                    fireProjectileInfo.damageColorIndex = DamageColorIndex.Default;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
                ApplyAirborneKnockback(ray.direction, 1800f);
                AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            }
        }

        private void ApplyAirborneKnockback(Vector3 aimDirection, float maxForce) {
            if (isGrounded) {
                return;
            }

            characterBody.characterMotor.ApplyForce(aimDirection * -maxForce);
        }


        public override void FixedUpdate() {
            base.FixedUpdate();
            if (fixedAge >= duration * fireDelay || hasFiredHeavyBlitz) {
                if (!hasFiredHeavyBlitz) {
                    Util.PlayAttackSpeedSound(attackSoundString, gameObject, attackSoundPitch);
                    FireBlitzProjectile();
                    hasFiredHeavyBlitz = true;
                }
                if (isAuthority && fixedAge >= duration / attackSpeedStat && canLeaveStateAfterFire) {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void Update() {
            base.Update();
            if (!IsKeyDownAuthority() && holdingKeyFromStart) {
                holdingKeyFromStart = false;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}