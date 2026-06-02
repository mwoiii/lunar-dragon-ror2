using RoR2;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.Components {

    public class ScaleLineToTracer : MonoBehaviour {

        public LineRenderer lineRenderer;

        public Tracer targetTracer;

        private void Start() {
            UpdatePositions();
        }

        private void Update() {
            UpdatePositions();
        }

        private void UpdatePositions() {
            if (lineRenderer && targetTracer && targetTracer.isActiveAndEnabled) {
                lineRenderer.SetPosition(0, targetTracer.startPos);
                lineRenderer.SetPosition(1, targetTracer.endPos);
            }
        }
    }
}
