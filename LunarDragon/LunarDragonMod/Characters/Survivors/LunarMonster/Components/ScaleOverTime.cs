using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.Components {
    public class ScaleOverTime : MonoBehaviour {
        public bool separateAxes;

        public bool resetOnEnable = true;

        public AnimationCurve combinedCurve;

        public AnimationCurve xCurve = null;

        public AnimationCurve yCurve = null;

        public AnimationCurve zCurve = null;

        private float stopwatch;

        private void Update() {
            stopwatch += Time.deltaTime;

            if (!separateAxes && combinedCurve != null) {
                transform.localScale = Vector3.one * combinedCurve.Evaluate(stopwatch);
            } else {
                if (xCurve.length > 0) {
                    transform.localScale = new Vector3(xCurve.Evaluate(stopwatch), transform.localScale.y, transform.localScale.z);
                }
                if (yCurve.length > 0) {
                    transform.localScale = new Vector3(transform.localScale.x, yCurve.Evaluate(stopwatch), transform.localScale.z);
                }
                if (zCurve.length > 0) {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, zCurve.Evaluate(stopwatch));
                }
            }
        }

        private void OnEnable() {
            if (resetOnEnable) {
                stopwatch = 0f;
            }
        }
    }
}
