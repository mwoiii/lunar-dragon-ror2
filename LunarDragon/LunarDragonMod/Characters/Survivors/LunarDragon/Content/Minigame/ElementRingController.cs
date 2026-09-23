using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class ElementRingController : RingController {

        private const float hitRingSpeed = 2.5f;

        private const float cycleStartPosition = 1f;

        private const float cycleEndPosition = 0f;

        private static Material hitWhiteRing = LunarDragonAssets.assetBundle.LoadAsset<Material>("matWhiteRing");

        public enum Element {
            Blood,
            Design,
            Mass,
            Soul
        }

        [SerializeField]
        public Element element;

        private float cyclePosition = 1f;

        [SerializeField]
        private float speed = 1f;

        private bool hit;

        protected override void Start() {
            base.Start();
            image.color = new Color(0f, 0f, 0f, cyclePosition);
        }

        public bool HasPassed(float latestPosition) {
            return ringRadiusUpper < latestPosition;
        }

        public bool TryHit(float earliestPosition, float latestPosition) {
            if (ringRadiusLower < earliestPosition && ringRadiusUpper > latestPosition) {
                image.material = hitWhiteRing;
                hit = true;
                return true;
            } else {
                return false;
            }
        }

        private void FixedUpdate() {
            if (!hit) {
                UpdatePositionActive();
            } else {
                UpdatePositionHit();
            }
        }

        private void UpdatePositionActive() {
            cyclePosition -= speed * Time.deltaTime * 0.8f;
            image.color = new Color(0f, 0f, 0f, cyclePosition);
            if (cyclePosition <= cycleEndPosition) {
                gameObject.SetActive(false);
            }
        }

        private void UpdatePositionHit() {
            cyclePosition += hitRingSpeed * Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, cyclePosition);
            if (cyclePosition >= cycleStartPosition) {
                gameObject.SetActive(false);
            }
        }
    }
}
