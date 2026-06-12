using EntityStates;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    public class LunarDragonMain : GenericCharacterMain {

        // Code copied from MageCharacterMain, modified for custom jetpack state

        protected EntityStateMachine jetpackStateMachine;

        public bool jumpButtonState;

        private bool heldPress;

        private float oldJumpHeldTime;

        private float jumpButtonHeldTime;

        public override void OnEnter() {
            base.OnEnter();
            jetpackStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Jet");
        }

        public override void ProcessJump() {
            if (hasCharacterMotor && hasInputBank && isAuthority) {
                if (NetworkUser.readOnlyLocalPlayersList[0]?.localUser?.userProfile.toggleArtificerHover ?? true) {

                    if (inputBank.jump.down) {
                        oldJumpHeldTime = jumpButtonHeldTime;
                        jumpButtonHeldTime += Time.deltaTime;
                        heldPress = oldJumpHeldTime < 0.5f && jumpButtonHeldTime >= 0.5f;
                    } else {
                        oldJumpHeldTime = 0f;
                        jumpButtonHeldTime = 0f;
                        heldPress = false;
                    }

                    if (!characterMotor.isGrounded) {
                        if (characterMotor.jumpCount == characterBody.maxJumpCount) {
                            if (inputBank.jump.justPressed) {
                                jumpButtonState = !jumpButtonState;
                            }
                        } else if (heldPress) {
                            jumpButtonState = !jumpButtonState;
                        }
                    } else {
                        jumpButtonState = false;
                    }

                } else {
                    jumpButtonState = inputBank.jump.down;
                }

                bool requestActivateJetpack = jumpButtonState && characterMotor.velocity.y < 3f && !characterMotor.isGrounded;
                bool jetpackIsActive = jetpackStateMachine.state.GetType() == typeof(JetsOn);

                if (requestActivateJetpack && !jetpackIsActive) {
                    jetpackStateMachine.SetNextState(new JetsOn());
                }

                if (!requestActivateJetpack && jetpackIsActive) {
                    jetpackStateMachine.SetNextState(new JetsOff());
                }
            }

            base.ProcessJump();
        }

        public override void OnExit() {
            if (isAuthority && jetpackStateMachine) {
                jetpackStateMachine.SetNextState(new Idle());
            }
            base.OnExit();
        }
    }
}
