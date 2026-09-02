using RoR2;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.Components {
    public class DragonSpawnCameraController : MonoBehaviour, ICameraStateProvider {

        public Transform modelTransform;

        public CharacterBody characterBody;

        public void Start() {
            modelTransform = GetComponent<ModelLocator>().modelTransform;
            characterBody = GetComponent<CharacterBody>();
            UpdateCamera();
        }

        public void Update() {
            UpdateCamera();
        }

        void ICameraStateProvider.GetCameraState(CameraRigController cameraRigController, ref CameraState cameraState) {
            Vector3 position = transform.position - transform.forward * 20f + transform.up * 5f;
            Vector3 target = new Vector3(transform.position.x, modelTransform.position.y, transform.position.z);
            cameraState = new CameraState {
                position = position,
                rotation = Quaternion.LookRotation(target - position, Vector3.up),
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
            foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList) {
                if (characterBody && cameraRigController.target == characterBody.gameObject) {
                    cameraRigController.SetOverrideCam(this, 0f);
                } else if (cameraRigController.IsOverrideCam(this)) {
                    cameraRigController.SetOverrideCam(null, 0f);
                }
            }
        }
    }
}
