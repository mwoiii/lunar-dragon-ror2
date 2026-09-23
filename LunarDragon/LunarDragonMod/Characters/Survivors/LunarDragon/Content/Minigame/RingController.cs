using UnityEngine;
using UnityEngine.UI;

namespace LunarDragonMod.Survivors.LunarDragon {
    public class RingController : MonoBehaviour {
        protected Image image;

        protected float borderWidth;

        protected RectTransform rectTransform;
        public float ringRadiusUpper => Mathf.Clamp((rectTransform.rect.yMax - rectTransform.rect.yMin) * image.color.a * transform.localScale.y, 0f, Mathf.Infinity);

        public float ringRadiusLower => Mathf.Clamp((rectTransform.rect.yMax - rectTransform.rect.yMin) * (image.color.a - borderWidth) * transform.localScale.y, 0f, Mathf.Infinity);

        protected virtual void Start() {
            // yes I am using the colours to store animation position don't talk to me 
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            borderWidth = image.material.GetFloat("_BorderWidth");
        }

        //protected void OnDrawGizmos() {
        //    if (!image) {
        //        image = GetComponent<Image>();
        //    }
        //    if (!rectTransform) {
        //        rectTransform = GetComponent<RectTransform>();
        //    }
        //    borderWidth = image.material.GetFloat("_BorderWidth");
        //    Gizmos.DrawWireSphere(transform.position, ringRadiusUpper);
        //    Gizmos.DrawWireSphere(transform.position, ringRadiusLower);
        //}
    }
}
