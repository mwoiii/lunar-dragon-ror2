using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class FireSecondaryController : MonoBehaviour {


        private const float raycastOffset = 4f;

        private const float raycastDistance = 12f;

        private const float timeBetweenRings = 0.6f;

        private const float ring1Radius = 5f;

        private const float ring2Radius = 10f;

        private const float ring3Radius = 15f;


        private ProjectileController projectileController;

        private ProjectileDamage projectileDamage;

        private TeamFilter teamFilter;

        private bool firedRing1;

        private bool firedRing2;

        private bool firedRing3;

        private float stopwatch;

        public void Start() {
            DestoryIfAirborne();
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
            teamFilter = GetComponent<TeamFilter>();
        }

        private void DestoryIfAirborne() {
            Ray ray = new Ray(transform.position + Vector3.up * raycastOffset, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance, LayerIndex.world.mask)) {
                Destroy(gameObject);
            }
        }

        public void Update() {
            stopwatch += Time.deltaTime;

            if (!firedRing1 && stopwatch > timeBetweenRings) {
                FireRing(ring1Radius);
                firedRing1 = true;
            } else if (!firedRing2 && stopwatch > timeBetweenRings * 2f) {
                FireRing(ring2Radius);
                firedRing2 = true;
            } else if (!firedRing3 && stopwatch > timeBetweenRings * 3f) {
                FireRing(ring3Radius);
                firedRing3 = true;
            }
        }

        private void FireRing(float radius) {
            int boxes = (int)(1.9f * radius);
            for (int i = 0; i < boxes; i++) {
                float angle = 2f * Mathf.PI * i / boxes;
                Vector3 raycastPos = transform.position + new Vector3(radius * Mathf.Sin(angle), 0f, radius * Mathf.Cos(angle));
                Ray ray = new Ray(raycastPos + Vector3.up * raycastOffset, Vector3.down);
                Vector3 direction = (raycastPos - transform.position).normalized;
                if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance, LayerIndex.world.mask)) {
                    BlastAttack blastAttack = new BlastAttack {
                        attacker = projectileController.owner,
                        baseDamage = projectileDamage.damage * 0.25f,
                        crit = projectileDamage.crit,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = projectileController.owner,
                        position = hitInfo.point,
                        procChainMask = default(ProcChainMask),
                        bonusForce = (direction + Vector3.up) * 700f,
                        procCoefficient = 0.85f,
                        radius = 4.8f,
                        teamIndex = teamFilter.teamIndex,
                        damageType = DamageType.IgniteOnHit
                    };
                    blastAttack.Fire();
                }
            }
        }
    }
}