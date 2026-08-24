using EntityStates;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    /// <summary>
    /// Heavily modified version of decompiled code from EntityStates.AimThrowableBase.
    /// Removed projectile-specific logic, has an option to be a toggle input (activated with primary) with the toggleActivate field.
    /// Not for use with primary skills
    /// </summary>
    public class DracoAmbushAim : BaseSkillState {

        public GameObject endpointVisualizerPrefab => Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Huntress.HuntressArrowRainIndicator_prefab).WaitForCompletion();

        public GameObject dotCrosshair = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_UI.SimpleDotCrosshair_prefab).WaitForCompletion();


        public float baseMinimumDuration => 0.15f;

        public string originOverrideString => "";

        public float maxDistance = 100f;

        public float rayRadius = 0.7f;

        public float endpointVisualizerRadiusScale = 4f;

        public bool toggleActivate = true;

        public LayerMask layerMask = LayerIndex.CommonMasks.bullet;

        private GameObject heldCrosshair;

        private bool holdingActivationKey = true;

        private bool holdingCancelKey = false;

        private bool stateFinished = false;

        private const int coarseSplit = 10;

        private const int fineSplit = 100; // this is a fraction of coarsesplit

        private bool IsNewKeyDownAuthority => IsKeyDownAuthority() && !holdingActivationKey;

        protected GameObject _endpointVisualizerPrefab; // overcooked

        protected Transform endpointVisualizerTransform;

        protected Transform originOverride;

        protected float projectileBaseSpeed;

        protected float minimumDuration;

        protected AimThrowableBase.TrajectoryInfo currentTrajectoryInfo;

        public override void OnEnter() {
            base.OnEnter();

            if (isAuthority) {

                _endpointVisualizerPrefab = endpointVisualizerPrefab;

                if (_endpointVisualizerPrefab) {
                    endpointVisualizerTransform = Object.Instantiate(_endpointVisualizerPrefab, transform.position, Quaternion.identity).transform;
                }

                if (characterBody) {
                    heldCrosshair = characterBody._defaultCrosshairPrefab;
                    characterBody._defaultCrosshairPrefab = dotCrosshair;
                }

                originOverride = FindModelChild(originOverrideString);
                minimumDuration = baseMinimumDuration / attackSpeedStat;
                UpdateVisualizers(currentTrajectoryInfo);
                SceneCamera.onSceneCameraPreRender += OnPreRenderSceneCam;
            }
        }

        public override void OnExit() {
            if (isAuthority) {
                SceneCamera.onSceneCameraPreRender -= OnPreRenderSceneCam;

                if (characterBody) {
                    characterBody._defaultCrosshairPrefab = heldCrosshair;
                }

                if (endpointVisualizerTransform) {
                    Destroy(endpointVisualizerTransform.gameObject);
                    endpointVisualizerTransform = null;
                }
            }

            base.OnExit();
        }

        protected virtual EntityState PickNextState() {
            return null;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Stun;
        }

        public override void Update() {
            base.Update();

            if (stateFinished) {
                return;
            }

            if (isAuthority) {

                UpdateTrajectoryInfo();
                UpdateVisualizers(currentTrajectoryInfo);

                if (!IsKeyDownAuthority()) {

                    if (!toggleActivate && age >= minimumDuration) {

                        // hold - activation by releasing
                        NextState();

                    } else if (toggleActivate) {
                        if (holdingActivationKey) {

                            // toggle - released from activation press
                            holdingActivationKey = false;

                        } else if (holdingCancelKey) {

                            // toggle - released from cancel press (confirmed cancel)
                            outer.SetNextStateToMain();
                            stateFinished = true;
                            return;

                        }
                    }


                } else if (toggleActivate) {
                    if (IsNewKeyDownAuthority && !holdingCancelKey) {

                        // toggle - second press of skill button (step before cancel)
                        holdingCancelKey = true;

                    }
                }

                if (toggleActivate && inputBank.skill1.justPressed && age >= minimumDuration) {

                    // toggle - activation with primary
                    NextState();

                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ExceedsDPThreshold(Vector3 normal, float threshold = 0.6f) {
            return Vector3.Dot(normal, Vector3.up) >= threshold;
        }

        protected virtual void NextState() {
            EntityState entityState = PickNextState();

            if (entityState != null) {
                outer.SetNextState(entityState);
            } else {
                outer.SetNextStateToMain();
            }

            stateFinished = true;
        }

        protected void UpdateTrajectoryInfo() {
            Ray aimRay = GetAimRay();
            RaycastHit hitInfo = default;

            bool success = false;

            // (1) checking if aimray is valid as it is
            bool collided = Util.CharacterSpherecast(gameObject, aimRay, rayRadius, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
            if (collided && ExceedsDPThreshold(hitInfo.normal)) {
                //Log.Info($"1b SUCCESS: {hitInfo.point}");
                success = true;
            }

            // (2) if no collision or bad angle, trying to project straight down
            Vector3 endPoint = collided ? hitInfo.point : aimRay.origin + aimRay.direction * maxDistance;
            if (!success) {
                Ray projectionRay = new Ray(endPoint, Vector3.down);
                if (Util.CharacterSpherecast(gameObject, projectionRay, rayRadius, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal) && ExceedsDPThreshold(hitInfo.normal)) {
                    success = true;
                    //Log.Info($"2b SUCCESS: {hitInfo.point}");
                }
            }

            // (3) if projection is also bad, trying optimized split method
            if (!success) {

                float distance = Vector2.Distance(new Vector2(endPoint.x, endPoint.z), new Vector2(aimRay.origin.x, aimRay.origin.z));
                float coarseMult = 1f / coarseSplit;
                float coarseRadius = distance * coarseMult;
                float fineMult = 1f / fineSplit;

                for (int i = coarseSplit - 1; i > 0; i--) {
                    Ray coarseProjectionRay = new Ray(aimRay.origin + (aimRay.direction * distance * coarseMult * i), Vector3.down);
                    if (Util.CharacterSpherecast(gameObject, coarseProjectionRay, coarseRadius, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal) && ExceedsDPThreshold(hitInfo.normal, 0.1f)) {
                        for (int j = 0; j <= fineSplit; j++) {
                            Ray fineProjectionRay = new Ray(coarseProjectionRay.origin - (aimRay.direction * coarseRadius * fineMult * j) + (aimRay.direction * coarseRadius), Vector3.down);
                            if (Util.CharacterSpherecast(gameObject, fineProjectionRay, rayRadius, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal) && ExceedsDPThreshold(hitInfo.normal)) {
                                success = true;
                                //Log.Info($"3a/4a SUCCESS: {i}, {j}");
                                break;
                            }
                        }
                        if (success) {
                            break;
                        }
                    }
                }
            }

            if (success) {
                currentTrajectoryInfo.hitPoint = hitInfo.point;
                currentTrajectoryInfo.hitNormal = hitInfo.normal;
            }
            //else {
            //    Log.Info($"3b FAILURE, USE LAST");
            //}
        }

        private void UpdateVisualizers(AimThrowableBase.TrajectoryInfo trajectoryInfo) {
            if (endpointVisualizerTransform) {
                endpointVisualizerTransform.SetPositionAndRotation(trajectoryInfo.hitPoint, Util.QuaternionSafeLookRotation(trajectoryInfo.hitNormal));
                if (!endpointVisualizerRadiusScale.Equals(0f)) {
                    endpointVisualizerTransform.localScale = new Vector3(endpointVisualizerRadiusScale, endpointVisualizerRadiusScale, endpointVisualizerRadiusScale);
                }
            }
        }

        private void OnPreRenderSceneCam(SceneCamera sceneCam) {
            if (endpointVisualizerTransform) {
                endpointVisualizerTransform.gameObject.layer = ((sceneCam.cameraRigController.target == gameObject) ? LayerIndex.defaultLayer.intVal : LayerIndex.noDraw.intVal);
            }
        }
    }
}
