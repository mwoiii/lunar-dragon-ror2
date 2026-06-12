using EntityStates.Mage;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {
    public class JetsOn : JetpackOn {

        private Animator animator;

        private const float transitionSpeed = 5f;

        private float hoveringDegree;

        private int hoverLayerIndex;

        public override void OnEnter() {
            base.OnEnter();
            animator = GetModelAnimator();
            if (animator) {
                animator.SetBool("isHovering", true);
                hoverLayerIndex = animator.GetLayerIndex("Hover");
                hoveringDegree = animator.GetLayerWeight(hoverLayerIndex);
            }
        }

        public override void Update() {
            base.Update();
            if (animator) {
                hoveringDegree = Mathf.Clamp01(Mathf.Lerp(hoveringDegree, 1f, transitionSpeed * Time.deltaTime));
                animator.SetLayerWeight(hoverLayerIndex, hoveringDegree);
            }
        }

        public override void OnExit() {
            base.OnEnter();
        }
    }
}
