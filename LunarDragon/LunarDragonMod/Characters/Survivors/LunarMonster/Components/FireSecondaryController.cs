using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class FireSecondaryController : MonoBehaviour {


        private const float raycastOffset = 4f;

        private const float raycastDistance = 12f;

        private const float timeBetweenRings = 0.8f;

        private const float ring1Radius = 7f;

        private const float ring2Radius = 14f;

        private const float ring3Radius = 21f;

        private const float boxesToRadiusRatio = 1f;


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
            } else if (firedRing3) {
                Destroy(gameObject);
            }
        }

        private void FireRing(float radius) {
            int boxes = (int)(boxesToRadiusRatio * radius);
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
                        radius = 4f,
                        teamIndex = teamFilter.teamIndex,
                        damageType = DamageType.IgniteOnHit
                    };
                    blastAttack.Fire();

                    EffectManager.SpawnEffect(LunarDragonAssets.heavyFireballPlumePrefab, new EffectData {
                        origin = hitInfo.point,
                        rotation = Quaternion.identity,
                        scale = 1f
                    }, transmit: true);
                }
            }

            EffectManager.SpawnEffect(LunarDragonAssets.plumeShakeSFX, new EffectData {
                origin = transform.position,
                rotation = Quaternion.identity,
                scale = 1f
            }, transmit: true);
        }
    }
}