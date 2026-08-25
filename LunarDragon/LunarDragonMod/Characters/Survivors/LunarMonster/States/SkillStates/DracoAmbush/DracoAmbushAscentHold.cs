using EntityStates;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushAscentHold : BaseState {

        private CharacterModel characterModel;

        private HurtBoxGroup hurtBoxGroup;

        private float lifetime = 2f;

        private float stopwatch;

        public Vector3 targetFootPosition;

        public override void OnEnter() {
            base.OnEnter();
            OnTakeoff();
        }

        private void OnTakeoff() {
            if (modelLocator && modelLocator.modelTransform) {
                if (modelLocator.modelTransform.TryGetComponent(out characterModel)) {
                    characterModel.invisibilityCount++;
                }
                if (modelLocator.modelTransform.TryGetComponent(out hurtBoxGroup)) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter++;
                }
            }
        }

        public override void Update() {
            base.Update();
            if (isAuthority) {
                stopwatch += Time.deltaTime;
                if (stopwatch >= lifetime) {
                    outer.SetNextState(new DracoAmbushDescentHold() {
                        targetFootPosition = targetFootPosition,
                        characterModel = characterModel,
                        hurtBoxGroup = hurtBoxGroup,
                    });
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}