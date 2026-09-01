using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using System.Collections.ObjectModel;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class AmbushSpawn : DracoAmbushDescending, ICameraStateProvider {

        private void SetupSpawnState() {
            Animator animator = GetModelAnimator();
            targetFootPosition = characterBody.footPosition;
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
            if (modelLocator && modelLocator.modelTransform) {
                modelLocator.autoUpdateModelTransform = false;
                modelTransform = modelLocator.modelTransform;
                up = Vector3.up;
                forward = modelTransform.forward;
                right = modelTransform.right;
                center = modelTransform.position;
                if (isAuthority && modelLocator.modelTransform.TryGetComponent(out hurtBoxGroup)) {
                    hurtBoxGroup.hurtBoxesDeactivatorCounter++;
                }
            }
            if (isAuthority) {
                if (characterMotor) {
                    characterMotor.velocity = Vector3.zero;
                    characterMotor.useGravity = false;
                    characterMotor.Motor.ForceUnground();
                }
                if (TryGetComponent(out Interactor interactor)) {
                    interactor.isRemoteOp = true;
                }
            }
        }

        public override void OnEnter() {
            SetupSpawnState();
            UpdateCamera();
            base.OnEnter();
        }

        public override void Update() {
            base.Update();
            UpdateCamera();
        }

        protected override bool doTeleport => true;

        void ICameraStateProvider.GetCameraState(CameraRigController cameraRigController, ref CameraState cameraState) {
            cameraState.position = transform.position + transform.forward * 2f;
            cameraState.rotation = Quaternion.Euler((modelTransform.position - transform.position).normalized);
            Log.Info("PENIS!");
            cameraState = new CameraState {
                position = transform.position + transform.forward * 2f,
                rotation = Quaternion.Euler((modelTransform.position - transform.position).normalized),
                fov = 60f
            };
        }

        bool ICameraStateProvider.IsUserLookAllowed(CameraRigController cameraRigController) {
            return false;
        }

        bool ICameraStateProvider.IsUserControlAllowed(CameraRigController cameraRigController) {
            return false;
        }

        bool ICameraStateProvider.IsHudAllowed(CameraRigController cameraRigController) {
            return true;
        }

        private void UpdateCamera() {
            //foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList) {
            //    if (characterBody && cameraRigController.target == characterBody.gameObject) {
            //        cameraRigController.SetOverrideCam(this, 0f);
            //    } else if (cameraRigController.IsOverrideCam(this)) {
            //        cameraRigController.SetOverrideCam(null, 0f);
            //    }
            //}
            ReadOnlyCollection<CameraRigController> readOnlyInstancesList = CameraRigController.readOnlyInstancesList;
            for (int i = 0; i < readOnlyInstancesList.Count; i++) {
                CameraRigController cameraRigController = readOnlyInstancesList[i];
                if (!cameraRigController.hasOverride) {
                    cameraRigController.SetOverrideCam(this, 0f);
                }
            }
        }
    }
}