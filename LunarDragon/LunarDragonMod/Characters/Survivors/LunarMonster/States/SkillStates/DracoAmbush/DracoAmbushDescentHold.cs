using EntityStates;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushDescentHold : BaseState {

        private float stopwatch;

        private float lifetime = 3f;

        public HurtBoxGroup hurtBoxGroup;

        public Vector3 targetFootPosition;

        private AnimationCurve xCurve = LunarDragonAssets.ascentDescendingData.xCurve;

        private AnimationCurve yCurve = LunarDragonAssets.ascentDescendingData.yCurve;

        private AnimationCurve zCurve = LunarDragonAssets.ascentDescendingData.zCurve;

        public Transform modelTransform;

        public Vector3 center;

        public Vector3 up;

        public Vector3 forward;

        public Vector3 right;

        public override void OnEnter() {
            cameraTargetParams.AddLerpRequest(0.5f);
            TeleportHelper.TeleportBody(characterBody, targetFootPosition, true);
            base.OnEnter();
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (isAuthority) {
                if (stopwatch >= lifetime) {
                    outer.SetNextState(new DracoAmbushDescent() {
                        targetFootPosition = targetFootPosition,
                        hurtBoxGroup = hurtBoxGroup,
                    });
                }
            }
            if (modelTransform && stopwatch < lifetime) {
                float scaledTime = stopwatch / lifetime;
                Vector3 offset = (
                    forward * zCurve.Evaluate(scaledTime) * 350f +
                    right * xCurve.Evaluate(scaledTime) * 350f +
                    up * yCurve.Evaluate(scaledTime) * 1000f
                );
                modelTransform.LookAt(targetFootPosition + offset);
                modelTransform.position = targetFootPosition + offset;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}