using RoR2;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.Components {
    public class PlaySoundOnDelay : MonoBehaviour {
        public float delay = 1f;

        public string soundString;

        public bool resetOnEnable = true;

        private float stopwatch;

        private bool activated;

        private void OnEnable() {
            if (resetOnEnable) {
                stopwatch = 0f;
                activated = false;
            }
        }

        public void Update() {
            if (!activated) {
                stopwatch += Time.deltaTime;
                if (stopwatch >= delay) {
                    Util.PlaySound(soundString, gameObject);
                    activated = true;
                }
            }
        }
    }
}
