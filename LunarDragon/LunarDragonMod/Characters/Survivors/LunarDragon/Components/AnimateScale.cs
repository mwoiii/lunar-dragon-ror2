using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class AnimateScale : MonoBehaviour {
        [SerializeField]
        private Vector3 startScale;

        [SerializeField]
        private Vector3 endScale;

        [SerializeField]
        private AnimationCurve curve;

        [SerializeField]
        private float duration;

        private float stopwatch;

        private void Update() {
            stopwatch += Time.deltaTime;
            if (stopwatch >= duration) {
                transform.localScale = endScale;
                Destroy(this);
                return;
            }
            float lerpPos = curve.Evaluate(stopwatch / duration);
            transform.localScale = new Vector3(
                Mathf.LerpUnclamped(startScale.x, endScale.x, lerpPos),
                Mathf.LerpUnclamped(startScale.y, endScale.y, lerpPos),
                Mathf.LerpUnclamped(startScale.z, endScale.z, lerpPos)
            );
        }
    }
}
