using LunarDragonMod.Characters.Survivors.LunarMonster.Components;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class AmbushSpawn : DracoAmbushDescending {

        private DragonSpawnCameraController cameraController;

        protected override bool doTeleport => false;

        private void SetupSpawnState() {
            PlayCrossfade("FullBody, Override", "SpecialDiveStartAir", 0f);
            ApplyAmbushStart();
            ApplyAmbushAscend();
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