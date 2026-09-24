using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class SelectionRingController : RingController {
        private enum FlashMode {
            Hit,
            Miss
        }

        [SerializeField]
        private Color hitColor;

        [SerializeField]
        private Color missColor;

        private Color baseColor;

        private float flashDuration = 0.2f;

        private float flashStopwatch = Mathf.Infinity;

        private FlashMode flashMode;

        protected override void Start() {
            base.Start();
            baseColor = image.color;
        }

        private void Update() {
            flashStopwatch += Time.deltaTime;

            float t = Mathf.Clamp01(flashStopwatch / flashDuration);
            Color newColor = Color.white;
            switch (flashMode) {
                case FlashMode.Hit:
                    newColor = Color.Lerp(hitColor, baseColor, t);
                    break;
                case FlashMode.Miss:
                    newColor = Color.Lerp(missColor, baseColor, t);
                    break;
            }
            newColor.a = image.color.a;

            image.color = newColor;
        }

        public void FlashSuccess() {
            flashStopwatch = 0f;
            flashMode = FlashMode.Hit;
        }

        public void FlashFailure() {
            flashStopwatch = 0f;
            flashMode = FlashMode.Miss;
        }
    }
}
