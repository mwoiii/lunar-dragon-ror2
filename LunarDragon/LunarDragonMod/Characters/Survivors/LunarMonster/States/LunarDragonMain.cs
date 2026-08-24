using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using LunarDragonMod.Survivors.LunarDragon.States;
using RoR2;
using System;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {

    // Code copied from MageCharacterMain, modified for custom jetpack state
    // Should only ever be one in existence, on Body state machine

    public class LunarDragonMain : GenericCharacterMain {

        public LunarDragonController controller;

        protected bool forceJetpack;

        public Type jetState = typeof(JetsOnBottom);

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
                bool jetpackIsActive = jetpackStateMachine.state is JetsOnBase;

                if (requestActivateJetpack && !jetpackIsActive) {
                    jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(jetState));
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

        public void ForceJetsOn(Type jetState = null) {
            forceJetpack = true;
            if (jetState != null) {
                this.jetState = jetState;
            }
        }
    }
}
