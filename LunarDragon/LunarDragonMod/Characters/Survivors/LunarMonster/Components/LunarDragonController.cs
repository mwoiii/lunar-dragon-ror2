using LunarDragonMod.Survivors.LunarDragon.States;
using RoR2;
using System.Collections;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class LunarDragonController : MonoBehaviour {

        public ElecSecondaryController elecSecondaryController;

        public bool canJump = true;

        public LunarDragonMain bodyState;

        public EntityStateMachine bodyStateMachine;

        public EntityStateMachine weaponStateMachine;

        public EntityStateMachine utilityStateMachine;

        public EntityStateMachine jetpackStateMachine;

        public CameraRigController camera;

        public CharacterBody characterBody;

        private ChildLocator childLocator;

        private GameObject fireAura;

        private const int assignAimTimeout = 50;

        private void Awake() {
            characterBody = GetComponent<CharacterBody>();
            bodyStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Body");
            weaponStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Weapon");
            utilityStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Utility");
            jetpackStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Jet");
            GetChildLocator();
            AddJets();
        }

        private void Start() {
            if (characterBody && characterBody.hasAuthority && characterBody.isPlayerControlled) {
                StartCoroutine(TryAssignAimOriginTransform());
            }
        }

        private IEnumerator TryAssignAimOriginTransform() {
            int attempt = 0;
            while (attempt < assignAimTimeout) {
                foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList) {
                    if (cameraRigController.target == gameObject) {
                        camera = cameraRigController;
                        Log.Info("Assigned aim origin!");
                        yield break;
                    }
                }
                attempt++;
                yield return null;
            }
        }

        public void EnableFireAura() {
            if (!fireAura && childLocator) {
                fireAura = Instantiate(LunarDragonAssets.specialAscendingFireEffect, childLocator.FindChild("Chest"), false);
            }
        }

        public void DisableFireAura() {
            if (fireAura) {
                Destroy(fireAura);
            }
        }

        public void DisableWeaponStateMachine() {
            weaponStateMachine.SetNextState(new SkillBlocker());
        }

        public void ResetWeaponStateMachine() {
            weaponStateMachine.SetNextStateToMain();
        }

        public void DisableUtilityStateMachine() {
            utilityStateMachine.SetNextState(new SkillBlocker());
        }

        public void ResetUtilityStateMachine() {
            utilityStateMachine.SetNextStateToMain();
        }

        public void DisableAllSkillStateMachines() {
            DisableWeaponStateMachine();
            DisableUtilityStateMachine();
        }

        public void ResetAllSkillStateMachines() {
            ResetWeaponStateMachine();
            ResetUtilityStateMachine();
        }

        private void GetChildLocator() {
            ModelLocator modelLocator = GetComponent<ModelLocator>();
            if (!modelLocator) {
                Log.Error("Couldn't find ModelLocator!");
                return;
            }
            childLocator = modelLocator.modelChildLocator;
        }

        private void AddJets() {
            if (childLocator) {
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetLeftBottom"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetLeftFront"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetRightBottom"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetRightFront"), false);
            } else {
                Log.Error("Couldn't find ChildLocator! Jet effects not added.");
            }
        }
    }
}