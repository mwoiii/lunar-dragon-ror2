using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class AnimationCurveData : ScriptableObject {
        [SerializeField]
        public AnimationCurve xCurve;

        [SerializeField]
        public AnimationCurve yCurve;

        [SerializeField]
        public AnimationCurve zCurve;
    }
}
