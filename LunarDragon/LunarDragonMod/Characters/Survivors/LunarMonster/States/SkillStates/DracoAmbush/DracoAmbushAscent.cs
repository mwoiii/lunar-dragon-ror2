using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushAscent : BaseState {

        private float stopwatch;

        private float duration;

        public Vector3 targetFootPosition;

        public override void OnEnter() {
            base.OnEnter();
            if (isAuthority) {
                if (TryGetComponent(out LunarDragonController controller)) {
                    controller.DisableAllSkillStateMachines();
                }
                if (characterMotor) {
                    characterMotor.velocity = Vector3.zero;
                    characterMotor.useGravity = false;
                }
            }
            Animator animator = GetModelAnimator();
            if (animator) {
                animator.SetBool("isHovering", false);
            }
            if (isGrounded) {
                PlayCrossfade("FullBody, Override", "SpecialDiveStart", 0.005f);
                duration = 0.86f;
            } else {
                PlayCrossfade("FullBody, Override", "SpecialDiveStartAir", 0.005f);
                duration = 0.4f;
            }
        }

        public override void Update() {
            base.Update();
            if (isAuthority) {
                stopwatch += Time.deltaTime;
                if (stopwatch >= duration) {
                    outer.SetNextState(new DracoAmbushAscentHold() {
                        targetFootPosition = targetFootPosition,
                    });
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}