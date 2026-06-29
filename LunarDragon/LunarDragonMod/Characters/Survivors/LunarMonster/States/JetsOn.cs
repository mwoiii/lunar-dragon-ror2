using EntityStates;
using UnityEngine;
using static LunarDragonMod.Characters.Survivors.LunarMonster.States.LunarDragonMain;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    // Code copied from JetpackOn, modified for custom jets
    public class JetsOn : BaseState {

        public JetDirection jetDirection;

        private Animator animator;

        private const float transitionSpeed = 5f;

        private float hoveringDegree;

        private int hoverLayerIndex;

        private const float hoverVelocity = -1f;

        private const float hoverAcceleration = 60f;

        private Transform jetLeftEffect;

        private Transform jetRightEffect;

        public JetsOn(JetDirection jetDirection) {
            this.jetDirection = jetDirection;
        }

        public override void Reset() {
            base.Reset();
            jetLeftEffect = null;
            jetRightEffect = null;
        }

        public override void OnEnter() {
            base.OnEnter();

            switch (jetDirection) {
                case JetDirection.Front:
                    jetLeftEffect = FindModelChild("JetLeftFront");
                    jetRightEffect = FindModelChild("JetRightFront");
                    break;
                default:
                    jetLeftEffect = FindModelChild("JetLeftBottom");
                    jetRightEffect = FindModelChild("JetRightBottom");
                    break;
            }

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
            base.OnEnter();

            if (jetLeftEffect) {
                jetLeftEffect.gameObject.SetActive(false);
            }

            if (jetRightEffect) {
                jetRightEffect.gameObject.SetActive(false);
            }
        }
    }
}
