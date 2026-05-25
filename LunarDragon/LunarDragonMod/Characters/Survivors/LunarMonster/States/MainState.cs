using EntityStates.Mage;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    public class MainState : MageCharacterMain {
        /*

        private CameraTargetParams.AimRequest aimRequest;

        private bool aimAuraActive;

        private float currentCameraThreshold;

        private const float lowerCamThreshold = 0.2f;

        private const float upperCamThreshold = 0.5f;
        */

        public override void OnEnter() {
            base.OnEnter();
            // currentCameraThreshold = characterBody.GetComponent<LunarDragonController>().upperCamThreshold;
            modelLocator.normalizeToFloor = true;
        }

        /*
        public override void Update() {
            base.Update();
            
            if ((bool)inputBank && inputBank.aimDirection.y > currentCameraThreshold && !aimAuraActive) {
                Log.Info("Requesting that shit");
                aimAuraActive = true;
                currentCameraThreshold = characterBody.GetComponent<LunarDragonController>().lowerCamThreshold;
                aimRequest = RequestTailAim();
            } else if (inputBank.aimDirection.y < currentCameraThreshold && aimAuraActive) {
                aimAuraActive = false;
                currentCameraThreshold = characterBody.GetComponent<LunarDragonController>().upperCamThreshold;
                aimRequest?.Dispose();
            }
            
        }

        public AimRequest RequestTailAim() {
            CharacterCameraParamsData data = cameraTargetParams.cameraParams.data;
            data.idealLocalCameraPos.value += new Vector3(0f, characterBody.GetComponent<LunarDragonController>().yOffset, 0f);
            CameraParamsOverrideHandle overrideHandle = cameraTargetParams.AddParamsOverride(new CameraParamsOverrideRequest {
                cameraParamsData = data,
                priority = 0.1f
            }, 0.25f);

            return new AimRequest(AimType.Standard, delegate {
                cameraTargetParams.RemoveParamsOverride(overrideHandle, 0.25f);
            });
        }

        public override void OnExit() {
            base.OnExit();
            aimAuraActive = false;
            aimRequest?.Dispose();
        }
        */
    }
}
