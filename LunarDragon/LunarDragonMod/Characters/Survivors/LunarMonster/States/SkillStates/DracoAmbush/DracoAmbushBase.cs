using EntityStates;
using KinematicCharacterController;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class DracoAmbushBase : BaseSkillState {

        private static BuffDef immunityBuff = Addressables.LoadAssetAsync<BuffDef>(RoR2_DLC2.bdHiddenRejectAllDamage_asset).WaitForCompletion();

        protected LunarDragonController controller;

        protected InteractionDriver interactionDriver;

        protected Animator animator;

        protected Transform modelTransform;

        protected KinematicCharacterMotor kinematicMotor;

        protected Collider collider;

        protected Inventory inventory;

        public HurtBoxGroup hurtBoxGroup;

        protected bool authorityFinished = false;

        public override void OnEnter() {
            base.OnEnter();
            animator = GetModelAnimator();
            interactionDriver = GetComponent<InteractionDriver>();
            controller = GetComponent<LunarDragonController>();
            kinematicMotor = GetComponent<KinematicCharacterMotor>();
            collider = GetComponent<Collider>();
            if (characterBody) {
                inventory = characterBody.inventory;
            }
            if (modelLocator) {
                modelTransform = modelLocator.modelTransform;
            }
        }

        protected void ApplyAmbushStart() {
            if (isAuthority) {
                if (controller) {
                    controller.DisableAllSkillStateMachines();
                }
                if (characterMotor) {
                    characterMotor.velocity = Vector3.zero;
                    characterMotor.useGravity = false;
                    characterMotor.Motor.ForceUnground();
                }
                if (interactionDriver) {
                    interactionDriver.enabled = false;
                    interactionDriver.currentInteractable = null;
                }
                if (modelTransform.TryGetComponent(out hurtBoxGroup)) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter++;
                }
                if (kinematicMotor) {
                    kinematicMotor.CollidableLayers = 0;
                }
                if (collider) {
                    collider.isTrigger = true;
                }
            }
            if (NetworkServer.active) {
                if (inventory) {
                    inventory.SetEquipmentDisabled(true);
                }
                if (characterBody) {
                    characterBody.AddBuff(immunityBuff);
                }
            }
            if (animator) {
                animator.SetBool(LunarDragonAnimationParameters.isHovering, false);
                animator.SetBool(LunarDragonAnimationParameters.forceIdle, true);
            }
        }

        protected void ApplyAmbushAscend() {
            if (controller) {
                controller.EnableFireAura();
                if (isAuthority) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOnFrontTrailHeavy)));
                }
            }
            if (modelTransform) {
                Util.PlaySound("Play_UI_podDescentLoop", modelTransform.gameObject);
                Util.PlaySound("Play_lemurianBruiser_m1_fly_loop", modelTransform.gameObject);
                modelLocator.autoUpdateModelTransform = false;
            }
        }

        protected void ForceExitAmbush() {
            ApplyAmbushLand();
            ApplyAmbushEnd();
        }

        protected void ApplyAmbushEnd() {
            if (animator) {
                animator.SetBool(LunarDragonAnimationParameters.forceIdle, false);
            }
            if (isAuthority) {
                if (characterMotor) {
                    characterMotor.useGravity = characterMotor.gravityParameters.CheckShouldUseGravity();
                }
                if (kinematicMotor) {
                    kinematicMotor.RebuildCollidableLayers();
                }
                if (collider) {
                    collider.isTrigger = false;
                }
            }
            if (NetworkServer.active) {
                if (inventory) {
                    inventory.SetEquipmentDisabled(false);
                }
                if (characterBody) {
                    characterBody.RemoveBuff(immunityBuff);
                }
            }
        }

        protected void ApplyAmbushLand() {
            PlayAnimation("FullBody, Override", "SpecialDiveEnd");
            if (controller) {
                controller.DisableFireAura();
            }
            if (modelLocator) {
                modelLocator.autoUpdateModelTransform = true;
            }
            if (modelTransform) {
                Util.PlaySound("Stop_UI_podDescentLoop", modelTransform.gameObject);
                Util.PlaySound("Stop_lemurianBruiser_m1_fly_loop", modelTransform.gameObject);
            }
            if (isAuthority) {
                if (controller) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOff)));
                    controller.ResetAllSkillStateMachines();
                }
                if (hurtBoxGroup) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter--;
                }
                if (interactionDriver) {
                    interactionDriver.enabled = true;
                }
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
