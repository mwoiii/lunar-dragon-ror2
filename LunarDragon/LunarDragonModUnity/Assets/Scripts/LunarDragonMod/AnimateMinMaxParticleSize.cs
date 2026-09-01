using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class AnimateMinMaxParticleSize : MonoBehaviour {
        [SerializeField]
        private ParticleSystemRenderer psr;

        [SerializeField]
        private AnimationCurve animationCurve;

        [SerializeField]
        private float loopDuration;

        private float stopwatch;

        private float loopMult;

        private void Awake() {
            loopMult = 1f / loopDuration;
            animationCurve.postWrapMode = WrapMode.Loop;
        }

        private void Update() {
            stopwatch += Time.deltaTime;
            if (psr && animationCurve.keys.Length > 0) {
                float size = animationCurve.Evaluate(stopwatch * loopMult);
                psr.minParticleSize = size;
                psr.maxParticleSize = size;
            }
        }
    }
}
