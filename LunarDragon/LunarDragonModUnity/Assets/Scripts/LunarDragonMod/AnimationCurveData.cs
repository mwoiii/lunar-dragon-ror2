using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    [CreateAssetMenu(menuName = "LunarDragon/AnimationCurveData")]
    public class AnimationCurveData : ScriptableObject {
        [SerializeField]
        public AnimationCurve xCurve;

        [SerializeField]
        public AnimationCurve yCurve;

        [SerializeField]
        public AnimationCurve zCurve;
    }
}
