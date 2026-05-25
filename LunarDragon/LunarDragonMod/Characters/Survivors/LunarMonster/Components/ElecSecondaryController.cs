using RoR2.Projectile;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class ElecSecondaryController : MonoBehaviour {

        LunarDragonController dragonController;

        public void Start() {
            dragonController = GetComponent<ProjectileController>()?.owner?.GetComponent<LunarDragonController>();
            if (dragonController != null) {
                dragonController.elecSecondaryController = this;
            }
        }

        public void Detonate() {
            GetComponent<ProjectileImpactExplosion>().lifetime = 0f;
            dragonController.elecSecondaryController = null;
        }
    }
}