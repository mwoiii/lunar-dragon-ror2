using UnityEngine;
using UnityEngine.UI;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class ElementRingController : MonoBehaviour {
        private float cyclePosition = 1f;

        private Image image;

        [SerializeField]
        private float speed = 1f;

        private float borderWidth;

        private RectTransform rectTransform;

        private float ringRadiusUpper => (rectTransform.rect.yMax - rectTransform.rect.yMin) * image.color.a;

        private float ringRadiusLower => (rectTransform.rect.yMax - rectTransform.rect.yMin) * (image.color.a - borderWidth);

        private void Start() {
            // yes I am using the colours to store animation position don't talk to me
            image = GetComponent<Image>();
            image.color = new Color(0f, 0f, 0f, cyclePosition);
            rectTransform = GetComponent<RectTransform>();
            borderWidth = image.material.GetFloat("_BorderWidth");
        }

        private void OnDrawGizmos() {
            if (!image) {
                image = GetComponent<Image>();
            }
            if (!rectTransform) {
                rectTransform = GetComponent<RectTransform>();
            }
            borderWidth = image.material.GetFloat("_BorderWidth");
            Gizmos.DrawWireSphere(transform.position, ((rectTransform.rect.yMax - rectTransform.rect.yMin)) * image.color.a);
            Gizmos.DrawWireSphere(transform.position, ((rectTransform.rect.yMax - rectTransform.rect.yMin)) * (image.color.a - borderWidth));
        }

        //public bool TryHit() { 
        //    if (image.color.r > 2f) {

        //    }
        //    Destroy(gameObject); 
        //} 

        private void FixedUpdate() {
            UpdatePosition();
        }

        private void UpdatePosition() {
            cyclePosition -= speed * Time.deltaTime * 0.8f;
            image.color = new Color(0f, 0f, 0f, cyclePosition);
        }
    }
}
