using LunarDragonMod.Characters.Survivors.LunarMonster.States;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class LunarDragonController : MonoBehaviour {

        public ElecSecondaryController elecSecondaryController;

        public bool canJump = true;

        public LunarDragonMain bodyState;

        private void Awake() {
            AddJets();
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