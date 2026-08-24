using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarDragon.Components {
    public class EnableOnTimer : MonoBehaviour {
        public float duration;

        public GameObject target;

        public bool disableOnEnable = true;

        public bool resetOnEnable = true;

        private float stopwatch;

        private void Update() {
            if (stopwatch < duration) {
                stopwatch += Time.deltaTime;
                if (stopwatch > duration && target) {
                    target.SetActive(true);
                }
            }
        }

        private void OnEnable() {
            target.SetActive(!disableOnEnable);
            if (resetOnEnable) {
                stopwatch = 0f;
            }
        }
    }
}
