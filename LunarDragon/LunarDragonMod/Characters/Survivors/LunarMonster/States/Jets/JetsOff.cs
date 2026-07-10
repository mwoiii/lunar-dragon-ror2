using EntityStates;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {
    public class JetsOff : BaseState {

        private Animator animator;

        private const float transitionSpeed = 5f;

        private float hoveringDegree;

        private int hoverLayerIndex;

        public override void OnEnter() {
            base.OnEnter();
            animator = GetModelAnimator();
            if (animator) {
                hoverLayerIndex = animator.GetLayerIndex("Hover");
                hoveringDegree = animator.GetLayerWeight(hoverLayerIndex);
            }
        }

        public override void Update() {
            base.Update();
            if (animator) {
                float motorMult = 1f;
                if (characterMotor && characterMotor.isGrounded) {
                    motorMult = 5f;
                }
                hoveringDegree = Mathf.Clamp01(Mathf.Lerp(hoveringDegree, 0f, transitionSpeed * Time.deltaTime * motorMult));
                if (hoveringDegree <= 0.05) {
                    outer.SetNextStateToMain();
                } else {
                    animator.SetLayerWeight(hoverLayerIndex, hoveringDegree);
                }
            } else {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnEnter();
            if (animator) {
                animator.SetLayerWeight(hoverLayerIndex, 0f);
                animator.SetBool("isHovering", false);
            }
        }
    }
}
