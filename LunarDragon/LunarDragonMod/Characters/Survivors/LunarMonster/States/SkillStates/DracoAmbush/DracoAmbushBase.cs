using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class DracoAmbushBase : BaseSkillState {

        protected LunarDragonController controller;

        protected Interactor interactor;

        protected Animator animator;

        protected Transform modelTransform;

        public HurtBoxGroup hurtBoxGroup;

        protected bool authorityFinished = false;

        public override void OnEnter() {
            base.OnEnter();
            animator = GetModelAnimator();
            interactor = GetComponent<Interactor>();
            controller = GetComponent<LunarDragonController>();
            if (modelLocator) {
                modelTransform = modelLocator.modelTransform;
            }
        }
        protected void RevertStateChanges() {
            PlayAnimation("FullBody, Override", "SpecialDiveEnd");
            if (controller) {
                controller.DisableFireAura();
            }
            if (modelLocator) {
                modelLocator.autoUpdateModelTransform = true;
                if (modelTransform) {
                    Util.PlaySound("Stop_UI_podDescentLoop", modelTransform.gameObject);
                    Util.PlaySound("Stop_lemurianBruiser_m1_fly_loop", modelTransform.gameObject);
                }
            }
            if (interactor) {
                interactor.isRemoteOp = false;
            }
            if (isAuthority) {
                if (characterMotor) {
                    characterMotor.useGravity = true;
                }
                if (controller) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOff)));
                    controller.ResetAllSkillStateMachines();
                }
                if (hurtBoxGroup) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter--;
                }
            }
            if (animator) {
                animator.SetBool(LunarDragonAnimationParameters.forceIdle, false);
            }
        }

        public override void OnExit() {
            base.OnEnter();
            if (isAuthority && !authorityFinished && controller) {
                // can be any state machine other than body
                // (as that is the only one the game outright expects and sometimes uses)
                // ((that is the pure reason this failsafe needs to exist))
                // (((from cases where it interrupts the body state)))
                // ((((which dracoambush states run on))))
                controller.utilityStateMachine.SetNextState(new DracoAmbushCancel() {
                    hurtBoxGroup = hurtBoxGroup
                });
            } else if (isAuthority && authorityFinished && !controller) {
                Log.Error("Couldn't run DracoAmbush failsafe as controller wasn't found!");
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}
