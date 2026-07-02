
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LunarDragonMod.Survivors.LunarDragon.Components {

    public class DamageTrailDynamic : MonoBehaviour {

        public float pointUpdateInterval = 0.2f;

        public float damageUpdateInterval = 0.2f;

        public float radius = 0.5f;

        public float height = 0.5f;

        public float pointLifetime = 3f;

        public DamageTypeCombo damageType = DamageTypeCombo.Generic;

        public LineRenderer lineRenderer;

        public bool active = true;

        public GameObject segmentPrefab;

        public bool destroyTrailSegments;

        // Whether or not to use optimizedDamageUpdateinterval instead of damageUpdateInterval
        public bool useOptimizedDamageUpdateInterval;

        // The damage coefficient of the trail, to delay damage assignment until Start. Overrides initial damagePerSecond value
        public float? dpsCoefficient = null;

        public float damagePerSecond;

        public GameObject owner;

        private HashSet<GameObject> ignoredObjects = new HashSet<GameObject>();

        private List<DamageTrail.TrailPoint> pointsList = new List<DamageTrail.TrailPoint>();

        private float localTime;

        private float nextTrailPointUpdate;

        private float nextTrailDamageUpdate;

        private bool wasActive;


        private void Start() {
            localTime = 0f;

            if (active) {
                AddPoint();
                AddPoint();
            }

            if (owner && dpsCoefficient != null && owner.TryGetComponent(out CharacterBody body)) {
                damagePerSecond = body.damage * (float)dpsCoefficient;
            }
        }

        private void OnDisable() {
            if (!EffectManager.UsePools) {
                return;
            }

            for (int i = pointsList.Count - 1; i >= 0; i--) {
                if (pointsList[i].segmentTransform) {
                    EffectManagerHelper effectManagerHelper = pointsList[i].segmentTransform.gameObject.GetComponent<EffectManagerHelper>();
                    if (effectManagerHelper && effectManagerHelper.OwningPool != null) {
                        effectManagerHelper.OwningPool.ReturnObject(effectManagerHelper);
                    }
                }
                pointsList.RemoveAt(i);
            }
        }


        private void FixedUpdate() {
            localTime += Time.deltaTime;

            if (localTime >= nextTrailPointUpdate) {
                nextTrailPointUpdate += pointUpdateInterval;
                UpdateTrail();
            }

            if (localTime >= nextTrailDamageUpdate) {
                float updateInterval = useOptimizedDamageUpdateInterval ? DamageTrail.optimizedDamageUpdateinterval : damageUpdateInterval;
                nextTrailDamageUpdate += updateInterval;
                DoDamage(updateInterval);
            }

            if (pointsList.Count > 0 && active) {
                UpdateFollowerTrailPoint();
            } else if (pointsList.Count > 0 && !active && wasActive) {
                UnparentTrailPoints();
            } else if (pointsList.Count <= 0 && !active) {
                Object.Destroy(gameObject); // lunar dragon specific
            }

            if (segmentPrefab && active) {
                InterpolatePoints();
            }

            wasActive = active;
        }

        private void UnparentTrailPoints() {
            foreach (DamageTrail.TrailPoint point in pointsList) {
                if (point.segmentTransform) {
                    point.segmentTransform.SetParent(null);
                }
            }
        }

        public void SkipToNextPointUpdate() {
            nextTrailPointUpdate = localTime;
        }

        private void UpdateFollowerTrailPoint() {
            DamageTrail.TrailPoint trailPoint = pointsList[^1];
            trailPoint.position = transform.position;
            //trailPoint.localEndTime = localTime + pointLifetime;

            if (trailPoint.segmentTransform) {
                trailPoint.segmentTransform.position = transform.position;
            }

            if (lineRenderer) {
                lineRenderer.SetPosition(pointsList.Count - 1, trailPoint.position);
            }
        }

        private void InterpolatePoints() {
            Vector3 position = transform.position;
            for (int i = pointsList.Count - 1; i >= 0; i--) {
                Transform segmentTransform = pointsList[i].segmentTransform;
                if (segmentTransform) {
                    segmentTransform.LookAt(position, Vector3.up);
                    Vector3 forwardDirection = pointsList[i].position - position;
                    segmentTransform.position = position + forwardDirection * 0.5f;
                    float lifetimeFraction = Mathf.Clamp01(Mathf.InverseLerp(pointsList[i].localStartTime, pointsList[i].localEndTime, localTime));
                    segmentTransform.localScale = new Vector3(radius * (1f - lifetimeFraction), radius * (1f - lifetimeFraction), forwardDirection.magnitude);
                    position = pointsList[i].position;
                }
            }
        }

        private void UpdateTrail() {
            while (pointsList.Count > 0 && pointsList[0].localEndTime <= localTime) {
                RemovePoint(0);
            }

            if (active) {
                AddPoint();
            }

            if (lineRenderer) {
                UpdateLineRenderer(lineRenderer);
            }
        }

        private void DoDamage(float damageInterval) {
            if (!NetworkServer.active || pointsList.Count <= 0) {
                return;
            }

            Vector3 vector = pointsList[pointsList.Count - 1].position;
            ignoredObjects.Clear();
            TeamIndex attackerTeamIndex = TeamIndex.Neutral;
            float damage = damagePerSecond * damageInterval;

            if (owner) {
                ignoredObjects.Add(owner);
                attackerTeamIndex = TeamComponent.GetObjectTeam(owner);
            }

            DamageInfo damageInfo = new DamageInfo();
            damageInfo.attacker = owner;
            damageInfo.inflictor = base.gameObject;
            damageInfo.crit = false;
            damageInfo.damage = damage;
            damageInfo.damageColorIndex = DamageColorIndex.Item;
            damageInfo.damageType = damageType;
            damageInfo.force = Vector3.zero;
            damageInfo.procCoefficient = 0f;

            for (int i = pointsList.Count - 1; i >= 0; i--) {

                // epic debug vfx
                // watch out it gets a little loud
                //EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Common_VFX.ExplosionVFX_prefab).WaitForCompletion(), new EffectData {
                //    origin = pointsList[i].position,
                //}, transmit: true);

                Vector3 position = pointsList[i].position;
                Vector3 forward = position - vector;
                Vector3 halfExtents = new Vector3(radius, height, forward.magnitude);
                Vector3 center = Vector3.Lerp(position, vector, 0.5f);
                Quaternion orientation = Util.QuaternionSafeLookRotation(forward);
                Collider[] colliders;
                int collisions = HGPhysics.OverlapBox(out colliders, center, halfExtents, orientation, LayerIndex.entityPrecise.mask);

                for (int j = 0; j < collisions; j++) {
                    HurtBox component = colliders[j].GetComponent<HurtBox>();
                    if (!component) {
                        continue;
                    }

                    HealthComponent healthComponent = component.healthComponent;
                    if (healthComponent) {
                        GameObject body = healthComponent.gameObject;
                        if (!ignoredObjects.Contains(body) && FriendlyFireManager.ShouldSplashHitProceed(healthComponent, attackerTeamIndex)) {
                            ignoredObjects.Add(body);
                            damageInfo.position = colliders[j].transform.position;
                            damageInfo.inflictedHurtbox = component;
                            healthComponent.TakeDamage(damageInfo);
                        }
                    }
                }
                HGPhysics.ReturnResults(colliders);
                vector = position;
            }
        }

        private void UpdateLineRenderer(LineRenderer lineRenderer) {
            lineRenderer.positionCount = pointsList.Count;
            for (int i = 0; i < pointsList.Count; i++) {
                lineRenderer.SetPosition(i, pointsList[i].position);
            }
        }

        private void AddPoint() {
            DamageTrail.TrailPoint trailPoint = new DamageTrail.TrailPoint {
                position = transform.position,
                localStartTime = localTime,
                localEndTime = localTime + pointLifetime
            };

            if (segmentPrefab) {
                if (!EffectManager.ShouldUsePooledEffect(segmentPrefab)) {
                    trailPoint.segmentTransform = Instantiate(segmentPrefab, transform).transform;
                } else {
                    EffectManagerHelper pooledEffect = EffectManager.GetAndActivatePooledEffect(segmentPrefab, transform, true);
                    trailPoint.segmentTransform = pooledEffect.gameObject.transform;
                }
            }

            pointsList.Add(trailPoint);
        }

        private void RemovePoint(int pointIndex) {
            if (destroyTrailSegments) {
                if (pointsList[pointIndex].segmentTransform) {
                    if (!EffectManager.UsePools) {
                        Destroy(pointsList[pointIndex].segmentTransform.gameObject);
                    } else {
                        GameObject gameObject = pointsList[pointIndex].segmentTransform.gameObject;
                        EffectManagerHelper component = gameObject.GetComponent<EffectManagerHelper>();
                        if (component != null && component.OwningPool != null) {
                            component.OwningPool.ReturnObject(component);
                        } else {
                            Destroy(gameObject);
                        }
                    }
                }
            } else if (EffectManager.UsePools && pointsList[pointIndex].segmentTransform) {
                pointsList[pointIndex].segmentTransform.gameObject.transform.SetParent(null);
            }
            pointsList.RemoveAt(pointIndex);
        }
    }
}
