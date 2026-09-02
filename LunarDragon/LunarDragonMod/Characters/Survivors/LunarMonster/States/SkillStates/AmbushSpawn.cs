using LunarDragonMod.Characters.Survivors.LunarMonster.Components;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class AmbushSpawn : DracoAmbushDescending {

        private DragonSpawnCameraController cameraController;

        protected override bool doTeleport => false;

        private void SetupSpawnState() {
            Animator animator = GetModelAnimator();
            if (animator) {
                PlayCrossfade("FullBody, Override", "SpecialDiveStartAir", 0f);
                animator.SetBool(LunarDragonAnimationParameters.isHovering, false);
                animator.SetBool(LunarDragonAnimationParameters.forceIdle, true);
            }
            if (TryGetComponent(out LunarDragonController controller)) {
                controller.EnableFireAura();
                if (isAuthority) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOnFrontTrailHeavy)));
                    controller.DisableAllSkillStateMachines();
                }
            }
            if (modelTransform) {
                modelLocator.autoUpdateModelTransform = false;
                if (isAuthority && modelTransform.TryGetComponent(out hurtBoxGroup)) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter++;
                }
            }
            if (isAuthority) {
                if (characterMotor) {
                    characterMotor.velocity = Vector3.zero;
                    characterMotor.useGravity = false;
                    characterMotor.Motor.ForceUnground();
                }
                if (interactor) {
                    interactor.isRemoteOp = true;
                }
            }
        }

        public override void OnEnter() {
            if (isAuthority) {
                cameraController = gameObject.AddComponent<DragonSpawnCameraController>();
            }
            base.OnEnter();
            SetupSpawnState();
        }

        public override void OnExit() {
            if (cameraController) {
                Object.Destroy(cameraController);
            }
            base.OnEnter();
        }
    }
}