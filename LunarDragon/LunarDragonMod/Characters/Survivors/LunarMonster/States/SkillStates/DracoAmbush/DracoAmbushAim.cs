using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class DracoAmbushAim : BaseSkillState {

        public GameObject endpointVisualizerPrefab => Addressables.LoadAssetAsync<GameObject>(RoR2_Base_Huntress.HuntressArrowRainIndicator_prefab).WaitForCompletion();

        public GameObject dotCrosshair = Addressables.LoadAssetAsync<GameObject>(RoR2_Base_UI.SimpleDotCrosshair_prefab).WaitForCompletion();

        public float baseMinimumDuration => 0.15f;

        public string originOverrideString => "";

        public float maxDistance = 200f;

        public float rayRadius = 2.3f;

        public float endpointVisualizerRadiusScale = 4f;

        public bool toggleActivate = true;

        public LayerMask layerMask = LayerIndex.CommonMasks.bullet;

        private GameObject heldCrosshair;

        private bool holdingActivationKey = true;

        private bool holdingCancelKey = false;

        private bool stateFinished = false;

        private const int coarseSplit = 10;

        private const int fineSplit = 100; // this divides a coarse segment (max possible raycasts in absolute worst case is coarseSplit * fineSplit) ((super omega unlikely))

        private bool IsNewKeyDownAuthority => IsKeyDownAuthority() && !holdingActivationKey;

        protected GameObject _endpointVisualizerPrefab; // overcooked

        protected Transform endpointVisualizerTransform;

        protected Transform originOverride;

        protected float projectileBaseSpeed;

        protected float minimumDuration;

        private Transform mainCamera;

        private CameraTargetParams.AimRequest aimRequest;

        protected AimThrowableBase.TrajectoryInfo currentTrajectoryInfo;

        private bool hasPosition;

        private LunarDragonController controller;

        public override void OnEnter() {
            base.OnEnter();

            if (isAuthority) {
                if (TryGetComponent(out controller)) {
                    controller.DisableWeaponStateMachine();
                }

                // I have never heard of splitscreen in my entire life
                if (CameraRigController.readOnlyInstancesList.Count > 0) {
                    mainCamera = CameraRigController.readOnlyInstancesList[0].transform;
                }

                aimRequest = cameraTargetParams.RequestAimWithData(new Vector3(0f, 16f, -20f), 0.2f, 0.2f);

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
                aimRequest?.Dispose();

                SceneCamera.onSceneCameraPreRender -= OnPreRenderSceneCam;

                if (characterBody) {
                    characterBody._defaultCrosshairPrefab = heldCrosshair;
                }

                if (endpointVisualizerTransform) {
                    Destroy(endpointVisualizerTransform.gameObject);
                    endpointVisualizerTransform = null;
                }

                if (controller) {
                    controller.ResetWeaponStateMachine();
                }
            }

            base.OnExit();
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
                        if (holdingActivationKey && hasPosition) {

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

        private void NextState() {
            if (controller) {
                controller.bodyStateMachine.SetNextState(new DracoAmbushAscent() {
                    targetFootPosition = currentTrajectoryInfo.hitPoint
                });
            }
            outer.SetNextStateToMain();

            stateFinished = true;
        }

        protected void UpdateTrajectoryInfo() {
            Ray aimRay = GetAimRay();
            RaycastHit hitInfo = default;

            bool success = false;
            if (mainCamera) {
                aimRay.origin = mainCamera.position;
                aimRay.direction = mainCamera.forward;
            }

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
                hasPosition = true;
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
                endpointVisualizerTransform.gameObject.layer = sceneCam.cameraRigController.target == gameObject ? LayerIndex.defaultLayer.intVal : LayerIndex.noDraw.intVal;
            }
        }
    }
}
