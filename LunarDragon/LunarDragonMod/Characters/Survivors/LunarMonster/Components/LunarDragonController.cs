using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    internal class LunarDragonController : MonoBehaviour {

        public ElecSecondaryController elecSecondaryController;

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
                Instantiate(LunarDragonAssets.jetEffectPrefab, modelLocator.modelChildLocator.FindChild("JetLeft"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, modelLocator.modelChildLocator.FindChild("JetRight"), false);
            } else {
                Log.Error("Couldn't find ChildLocator! Jet effects not added.");
            }
        }
    }
}