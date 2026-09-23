using LunarDragonMod.Characters.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.States;
using RoR2;
using RoR2.UI;
using System;
using System.Collections;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    public class LunarDragonController : MonoBehaviour {

        [NonSerialized]
        public ElecSecondaryController elecSecondaryController;

        [NonSerialized]
        public bool canJump = true;

        public LunarDragonMain bodyState;

        public EntityStateMachine bodyStateMachine;

        public EntityStateMachine weaponStateMachine;

        public EntityStateMachine utilityStateMachine;

        public EntityStateMachine jetpackStateMachine;

        public EntityStateMachine aimStateMachine;

        [NonSerialized]
        public MinigameController minigameInstance;

        [NonSerialized]
        public CameraRigController camera;

        public CharacterBody characterBody;

        [SerializeField]
        private ChildLocator childLocator;

        private GameObject fireAura;

        private const int assignAimTimeout = 50;

        private HUD localHUD;

        private void Awake() {
            AddJets();
        }

        private void Start() {
            if (characterBody && characterBody.hasAuthority && characterBody.isPlayerControlled) {
                StartCoroutine(TryAssignAimOriginTransform());
            }
        }

        private void TryEnsureHUD() {
            if (!localHUD) {
                foreach (HUD hud in HUD.instancesList) {
                    if (hud.targetBodyObject == gameObject) {
                        localHUD = hud;
                        break;
                    }
                }
            }
        }

        public void StartSpecialMinigame() {
            TryEnsureHUD();
            if (!localHUD) {
                Log.Error("Couldn't find HUD for local player! Skipping minigame...");
                return;
            }

            if (localHUD.TryGetComponent(out ChildLocator childLocator)) {
                Transform crosshairExtras = childLocator.FindChild("CrosshairExtras");
                if (crosshairExtras) {
                    minigameInstance = Instantiate(LunarDragonAssets.specialMinigamePrefab, crosshairExtras).GetComponent<MinigameController>();
                    minigameInstance.characterBody = characterBody;
                    RectTransform rectTransform = minigameInstance.GetComponent<RectTransform>();
                    rectTransform.anchorMin = Vector3.one * 0.5f;
                    rectTransform.anchorMax = Vector3.one * 0.5f;
                    minigameInstance.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void EnsureMinigameEnded() {
            if (minigameInstance) {
                Destroy(minigameInstance.gameObject);
            }
        }

        private IEnumerator TryAssignAimOriginTransform() {
            int attempt = 0;
            while (attempt < assignAimTimeout) {
                foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList) {
                    if (cameraRigController.target == gameObject) {
                        camera = cameraRigController;
                        yield break;
                    }
                }
                attempt++;
                yield return null;
            }
        }

        public void EnableFireAura() {
            if (!fireAura && childLocator) {
                fireAura = Instantiate(LunarDragonAssets.specialAscendingFireEffect, childLocator.FindChild("Chest"), false);
            }
        }

        public void DisableFireAura() {
            if (fireAura) {
                Destroy(fireAura);
            }
        }

        public void DisableWeaponStateMachine() {
            weaponStateMachine.SetNextState(new SkillBlocker());
        }

        public void ResetWeaponStateMachine() {
            weaponStateMachine.SetNextStateToMain();
        }

        public void DisableUtilityStateMachine() {
            utilityStateMachine.SetNextState(new SkillBlocker());
        }

        public void ResetUtilityStateMachine() {
            utilityStateMachine.SetNextStateToMain();
        }

        public void DisableAllSkillStateMachines() {
            DisableWeaponStateMachine();
            DisableUtilityStateMachine();
        }

        public void ResetAllSkillStateMachines() {
            ResetWeaponStateMachine();
            ResetUtilityStateMachine();
        }

        private void AddJets() {
            if (childLocator) {
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetLeftBottom"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetLeftFront"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetRightBottom"), false);
                Instantiate(LunarDragonAssets.jetEffectPrefab, childLocator.FindChild("JetRightFront"), false);
            } else {
                Log.Error("Couldn't find ChildLocator! Jet effects not added.");
            }
        }
    }
}