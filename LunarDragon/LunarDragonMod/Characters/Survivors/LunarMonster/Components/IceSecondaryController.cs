using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class IceSecondaryController : MonoBehaviour {

        private const float raycastDistance = 12f;

        private const float groundedFireFreq = 0.5f;

        private const float groundedFireLifetime = 4.5f;


        private ProjectileController projectileController;

        private ProjectileDamage projectileDamage;

        private TeamFilter teamFilter;

        private float age;

        private float fireStopwatch;

        private bool airborne;

        private Vector3 groundedPos;

        public void Start() {
            CheckIfAirborne();
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
            teamFilter = GetComponent<TeamFilter>();
        }

        private void CheckIfAirborne() {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance, LayerIndex.world.mask)) {
                airborne = false;
                groundedPos = hitInfo.point;
            } else {
                Destroy(gameObject);
            }
        }

        public void Update() {
            age += Time.deltaTime;
            fireStopwatch += Time.deltaTime;

            if (!airborne) {
                if (fireStopwatch >= groundedFireFreq) {
                    fireStopwatch = 0f;
                    FireIceAOE();
                }
                if (age >= groundedFireLifetime) {
                    Destroy(gameObject);
                }
            }
        }

        private void FireIceAOE() {
            BlastAttack blastAttack = new BlastAttack {
                attacker = projectileController.owner,
                baseDamage = projectileDamage.damage * 0.1f,
                crit = projectileDamage.crit,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = projectileController.owner,
                position = groundedPos,
                procChainMask = default(ProcChainMask),
                procCoefficient = 0.7f,
                radius = 14f,
                teamIndex = teamFilter.teamIndex,
                damageType = DamageType.Freeze2s,
            };
            blastAttack.Fire();
        }
    }
}