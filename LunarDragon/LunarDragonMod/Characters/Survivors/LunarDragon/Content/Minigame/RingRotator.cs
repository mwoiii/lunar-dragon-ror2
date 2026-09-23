using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class RingRotator : MonoBehaviour {
        [SerializeField]
        private bool randomizeStartRotation;

        [SerializeField]
        private bool rotateDuringLifetime;

        [SerializeField]
        private bool randomizeDirection;

        [SerializeField]
        private float speed;

        private float sign = 1f;

        private void Start() {
            if (randomizeStartRotation) {
                transform.rotation = Quaternion.Euler(0f, 0f, Random.value * 360f);
            }
            if (randomizeDirection) {
                sign = Mathf.Sign(Random.value - 0.5f);
            }
        }

        private void Update() {
            if (rotateDuringLifetime) {
                transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + speed * Time.deltaTime * sign);
            }
        }
    }
}
