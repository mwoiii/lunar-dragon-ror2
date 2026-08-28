using EntityStates;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushAscentHold : BaseState {

        private HurtBoxGroup hurtBoxGroup;

        private float lifetime = 3f;

        private float stopwatch;

        public Vector3 targetFootPosition;

        private AnimationCurve xCurve = LunarDragonAssets.ascentRisingData.xCurve;

        private AnimationCurve yCurve = LunarDragonAssets.ascentRisingData.yCurve;

        private AnimationCurve zCurve = LunarDragonAssets.ascentRisingData.zCurve;

        private Transform modelTransform;

        private Vector3 center;

        private Vector3 up;

        private Vector3 forward;

        private Vector3 right;

        public override void OnEnter() {
            base.OnEnter();
            OnTakeoff();
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
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (isAuthority) {
                if (stopwatch >= lifetime) {
                    outer.SetNextState(new DracoAmbushDescentHold() {
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
                    forward * zCurve.Evaluate(scaledTime) * 350f +
                    right * xCurve.Evaluate(scaledTime) * 350f +
                    up * yCurve.Evaluate(scaledTime) * 1000f
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