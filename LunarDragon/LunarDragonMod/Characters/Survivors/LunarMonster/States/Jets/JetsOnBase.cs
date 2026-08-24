using EntityStates;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    // Code copied from JetpackOn, modified for custom jets
    public class JetsOnBase : BaseState {

        private Animator animator;

        private const float transitionSpeed = 5f;

        private float hoveringDegree;

        private int hoverLayerIndex;

        private const float hoverVelocity = -1f;

        private const float hoverAcceleration = 60f;

        protected Transform jetLeftEffect;

        protected Transform jetRightEffect;

        public override void Reset() {
            base.Reset();
            jetLeftEffect = null;
            jetRightEffect = null;
        }

        protected virtual void GetJetEffects() {
            jetLeftEffect = FindModelChild("JetLeftBottom");
            jetRightEffect = FindModelChild("JetRightBottom");
        }

        public override void OnEnter() {
            base.OnEnter();

            GetJetEffects();

            if (jetLeftEffect) {
                jetLeftEffect.gameObject.SetActive(true);
            }
            if (jetRightEffect) {
                jetRightEffect.gameObject.SetActive(true);
            }

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
        public override void FixedUpdate() {
            base.FixedUpdate();
            if (isAuthority) {
                float y = characterMotor.velocity.y;
                if (y < hoverVelocity) {
                    y = Mathf.MoveTowards(y, hoverVelocity, hoverAcceleration * GetDeltaTime());
                    characterMotor.velocity = new Vector3(characterMotor.velocity.x, y, characterMotor.velocity.z);
                }
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (jetLeftEffect) {
                jetLeftEffect.gameObject.SetActive(false);
            }

            if (jetRightEffect) {
                jetRightEffect.gameObject.SetActive(false);
            }
        }
    }
}
