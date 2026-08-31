using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushRising : BaseState {

        private HurtBoxGroup hurtBoxGroup;

        private float lifetime = 3f;

        private float stopwatch;

        public Vector3 targetFootPosition;

        private AnimationCurve xCurve = LunarDragonAssets.specialAmbushRisingData.xCurve;

        private AnimationCurve yCurve = LunarDragonAssets.specialAmbushRisingData.yCurve;

        private AnimationCurve zCurve = LunarDragonAssets.specialAmbushRisingData.zCurve;

        private Transform modelTransform;

        private Vector3 center;

        private Vector3 up;

        private Vector3 forward;

        private Vector3 right;

        public override void OnEnter() {
            base.OnEnter();
            OnTakeoff();
            if (TryGetComponent(out LunarDragonController controller)) {
                controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOnFrontTrailHeavy)));
            }
        }

        private void OnTakeoff() {
            if (modelLocator && modelLocator.modelTransform) {
                modelLocator.autoUpdateModelTransform = false;
                modelTransform = modelLocator.modelTransform;
                up = Vector3.up;
                forward = modelTransform.forward;
                right = modelTransform.right;
                center = modelTransform.position;
                if (modelLocator.modelTransform.TryGetComponent(out hurtBoxGroup)) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter++;
                }
            }

            if (isAuthority && characterBody && characterBody.teamComponent) {
                FireExplosion();
            }
        }

        private void FireExplosion() {
            BlastAttack blastAttack = new BlastAttack {
                attacker = characterBody.gameObject,
                baseDamage = characterBody.damage * LunarDragonStaticValues.specialAmbushTakeoffDamageCoefficient,
                crit = characterBody.RollCrit(),
                falloffModel = BlastAttack.FalloffModel.HalfLinear,
                inflictor = characterBody.gameObject,
                position = characterBody.transform.position,
                procChainMask = default(ProcChainMask),
                baseForce = 1500f,
                procCoefficient = 1f,
                radius = 18f,
                teamIndex = characterBody.teamComponent.teamIndex,
                damageType = DamageType.IgniteOnHit
            };
            blastAttack.Fire();
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (isAuthority) {
                if (stopwatch >= lifetime) {
                    outer.SetNextState(new DracoAmbushDescending() {
                        targetFootPosition = targetFootPosition,
                        hurtBoxGroup = hurtBoxGroup,
                        modelTransform = modelTransform,
                        forward = forward,
                        up = up,
                        right = right
                    });
                }
            }
            if (modelTransform) {
                float scaledTime = stopwatch / lifetime;
                Vector3 offset = (
                    forward * zCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationXMult +
                    right * xCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationZMult +
                    up * yCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationYMult
                );
                modelTransform.LookAt(center + offset);
                modelTransform.position = center + offset;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}