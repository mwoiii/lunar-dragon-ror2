using EntityStates;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushDescentHold : BaseState {

        private float stopwatch;

        private float lifetime = 3f;

        public CharacterModel characterModel;

        public HurtBoxGroup hurtBoxGroup;

        public Vector3 targetFootPosition;

        public override void OnEnter() {
            cameraTargetParams.AddLerpRequest(0.5f);
            TeleportHelper.TeleportBody(characterBody, targetFootPosition, true);
            base.OnEnter();
        }

        public override void Update() {
            base.Update();
            if (isAuthority) {
                stopwatch += Time.deltaTime;
                if (stopwatch >= lifetime) {
                    outer.SetNextState(new DracoAmbushDescent() {
                        targetFootPosition = targetFootPosition,
                        hurtBoxGroup = hurtBoxGroup,
                        characterModel = characterModel
                    });
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}