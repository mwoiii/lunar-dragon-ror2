using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushDescending : BaseState {

        private float stopwatch;

        private float lifetime = 3f;

        public HurtBoxGroup hurtBoxGroup;

        public Vector3 targetFootPosition;

        private AnimationCurve xCurve = LunarDragonAssets.specialAmbushDescendingData.xCurve;

        private AnimationCurve yCurve = LunarDragonAssets.specialAmbushDescendingData.yCurve;

        private AnimationCurve zCurve = LunarDragonAssets.specialAmbushDescendingData.zCurve;

        public Transform modelTransform;

        public Vector3 center;

        public Vector3 up;

        public Vector3 forward;

        public Vector3 right;

        protected virtual bool doTeleport => true;

        public override void OnEnter() {
            cameraTargetParams.AddLerpRequest(0.5f);
            if (doTeleport) {
                TeleportHelper.TeleportBody(characterBody, targetFootPosition, true);
            }
            base.OnEnter();
            if (TryGetComponent(out LunarDragonController controller)) {
                controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOnFrontTrailLight)));
            }
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (isAuthority) {
                if (stopwatch >= lifetime) {
                    outer.SetNextState(new DracoAmbushLand() {
                        hurtBoxGroup = hurtBoxGroup,
                    });
                }
            }
            if (modelTransform && stopwatch < lifetime) {
                float scaledTime = stopwatch / lifetime;
                Vector3 offset = (
                    forward * zCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationXMult +
                    right * xCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationZMult +
                    up * yCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationYMult
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