using LunarDragonMod.Survivors.LunarDragon.States;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class LunarDragonController : MonoBehaviour {

        public ElecSecondaryController elecSecondaryController;

        public bool canJump = true;

        public LunarDragonMain bodyState;

        public EntityStateMachine bodyStateMachine;

        public EntityStateMachine weaponStateMachine;

        public EntityStateMachine utilityStateMachine;

        private void Awake() {
            bodyStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Body");
            weaponStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Weapon");
            utilityStateMachine = EntityStateMachine.FindByCustomName(gameObject, "Utility");
            AddJets();
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

        private void AddJets() {
            ModelLocator modelLocator = GetComponent<ModelLocator>();
            if (!modelLocator) {
                Log.Error("Couldn't find ModelLocator! Jet effects not added.");
                return;
            }

            if (modelLocator.modelChildLocator) {
                Instantiate(LunarDragonAssets.jetEffectPrefab, modelLocator.modelChildLocator.FindChild("JetLeftBottom"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, modelLocator.modelChildLocator.FindChild("JetLeftFront"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, modelLocator.modelChildLocator.FindChild("JetRightBottom"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, modelLocator.modelChildLocator.FindChild("JetRightFront"), false);
            } else {
                Log.Error("Couldn't find ChildLocator! Jet effects not added.");
            }
        }
    }
}