using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushDescending : DracoAmbushBase {

        private float stopwatch;

        private float lifetime = 3f;

        public Vector3 targetFootPosition;

        private AnimationCurve xCurve = LunarDragonAssets.specialAmbushDescendingData.xCurve;

        private AnimationCurve yCurve = LunarDragonAssets.specialAmbushDescendingData.yCurve;

        private AnimationCurve zCurve = LunarDragonAssets.specialAmbushDescendingData.zCurve;

        private Transform modelBaseTransform;

        protected virtual bool doTeleport => true;

        public override void OnEnter() {
            base.OnEnter();
            modelBaseTransform = GetModelBaseTransform();
            if (isAuthority) {
                if (doTeleport) {
                    cameraTargetParams.AddLerpRequest(0.5f);
                    TeleportHelper.TeleportBody(characterBody, targetFootPosition, true);
                }
                if (controller) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOnFrontTrailLight)));
                }
            }
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (isAuthority) {
                if (stopwatch >= lifetime) {
                    authorityFinished = true;
                    outer.SetNextState(new DracoAmbushLand() {
                        hurtBoxGroup = hurtBoxGroup,
                    });
                }
            }
            if (modelTransform && characterBody && stopwatch < lifetime) {
                float scaledTime = stopwatch / lifetime;
                Vector3 offset = (
                    modelBaseTransform.forward * zCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationXMult +
                    modelBaseTransform.right * xCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationZMult +
                    modelBaseTransform.up * yCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationYMult
                );
                modelTransform.LookAt(characterBody.footPosition + offset);
                modelTransform.position = characterBody.footPosition + offset;
            }
        }
    }
}