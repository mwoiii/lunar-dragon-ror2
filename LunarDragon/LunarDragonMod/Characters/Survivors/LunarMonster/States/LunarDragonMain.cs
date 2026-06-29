using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    // Code copied from MageCharacterMain, modified for custom jetpack state
    // Should only ever be one in existence, on Body state machine

    public class LunarDragonMain : GenericCharacterMain {
        public enum JetDirection {
            Bottom,
            Front
        }

        public LunarDragonController controller;

        protected bool forceJetpack;

        protected JetDirection jetDirection;

        protected EntityStateMachine jetpackStateMachine;

        public bool jumpButtonState;

        private bool heldPress;

        private float oldJumpHeldTime;

        private float jumpButtonHeldTime;

        private bool canHover;

        public override void OnEnter() {
            base.OnEnter();
            controller = GetComponent<LunarDragonController>();
            if (controller) {
                controller.bodyState = this;
            }
            jetpackStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Jet");
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (!canHover) {
                canHover = characterMotor.velocity.y < 3f && !characterMotor.isGrounded;
            } else {
                canHover = !characterMotor.isGrounded;
            }
        }

        public override void ProcessJump() {
            if (hasCharacterMotor && hasInputBank && isAuthority) {

                if (jumpInputReceived && characterMotor.jumpCount < characterBody.maxJumpCount) {
                    canHover = false;
                }

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

                bool requestActivateJetpack = (jumpButtonState && canHover) || forceJetpack;
                bool jetpackIsActive = jetpackStateMachine.state.GetType() == typeof(JetsOn);

                if (requestActivateJetpack && !jetpackIsActive) {
                    jetpackStateMachine.SetNextState(new JetsOn(jetDirection));
                }

                if (!requestActivateJetpack && jetpackIsActive) {
                    jetpackStateMachine.SetNextState(new JetsOff());
                }
            }

            if (controller && controller.canJump) {
                base.ProcessJump();
            }
        }

        public override void OnExit() {
            if (isAuthority && jetpackStateMachine) {
                jetpackStateMachine.SetNextState(new Idle());
            }
            base.OnExit();
        }

        public void ForceJetsOn(JetDirection direction) {
            forceJetpack = true;
            jetDirection = direction;
        }
    }
}
