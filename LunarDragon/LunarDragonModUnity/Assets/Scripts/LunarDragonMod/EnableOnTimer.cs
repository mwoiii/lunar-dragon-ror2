using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarDragon.Components {
    public class EnableOnTimer : MonoBehaviour {
        public float duration;

        public GameObject target;

        public bool disableOnEnable = true;

        public bool resetOnEnable = true;
    }
}
