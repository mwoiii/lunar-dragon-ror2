using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;
using LunarDragonMod.Survivors.LunarDragon.NetMessages;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using static LunarDragonMod.Survivors.LunarDragon.ElementRingController;

namespace LunarDragonMod.Characters.LunarDragon {
    public class MinigameController : MonoBehaviour {

        private const int ringCount = 6;

        private const float duration = 2f;

        private const float graceWindow = 0.18f;

        private const float exitAnimAt = duration + 1.3f;

        public const float bloodDamageIncrease = 10f;

        public const float designCooldownReduction = 5f;

        public const float massRadiusIncrease = 10f;

        public const float soulHealIncrease = 0.25f / ringCount;

        [SerializeField]
        private GameObject bloodRingPrefab;

        [SerializeField]
        private GameObject designRingPrefab;

        [SerializeField]
        private GameObject massRingPrefab;

        [SerializeField]
        private GameObject soulRingPrefab;

        [SerializeField]
        private Transform activeParent;

        [SerializeField]
        private Transform effectParent;

        [SerializeField]
        private AnimateScale endingAnimation;

        [SerializeField]
        private SelectionRingController selectionRing;

        [HideInInspector]
        public CharacterBody characterBody;

        private float gameStopwatch;

        private Queue<ElementRingController> activeRings = new Queue<ElementRingController>(ringCount);

        private List<float> ringTimings = new List<float>(ringCount);

        private GameObject[] randomRingSelection;

        private int currentTiming;

        [HideInInspector]
        public float bonusDamage;

        [HideInInspector]
        public float bonusRadius;

        [HideInInspector]
        public float bonusHealing;

        [HideInInspector]
        public float bonusCDReduction;

        private float animStopwatch = 0f;

        private void Start() {
            PopulateRandomTimings();
            PopulateRandomRings();
        }

        private void PopulateRandomTimings() {
            float localDuration = duration - graceWindow;  // leaving space for soul ring always at the end (fixed time)
            float timeLeft = localDuration;

            for (int i = 0; i < ringCount - 1; i++) {
                if (timeLeft <= 0) {
                    Log.Warning("Ran out of available time for minigame setup??");
                    break;
                }

                float selection = Random.value * timeLeft;
                foreach (float timing in ringTimings) {
                    if (selection > timing - graceWindow) {
                        selection += graceWindow * 2f;
                    }
                }

                // adjusting time left - not subtracting full grace window if overlaps with a border

                // near border (0)
                if (selection - graceWindow > 0) {
                    timeLeft -= graceWindow;
                } else {
                    timeLeft -= selection;
                }

                // far border (localDuration)
                if (selection + graceWindow <= localDuration) {
                    timeLeft -= graceWindow;
                } else {
                    timeLeft -= localDuration - selection;
                }

                ringTimings.Add(selection);
                ringTimings.Sort();
            }
            ringTimings.Add(duration);
        }

        private void PopulateRandomRings() {
            randomRingSelection = new GameObject[] {
                bloodRingPrefab,
                designRingPrefab,
                massRingPrefab
            };
        }

        private void SpawnRandomRing() {
            SpawnRing(randomRingSelection[Random.RandomRangeInt(0, randomRingSelection.Length)]);
        }

        private void SpawnRing(GameObject ringPrefab) {
            GameObject ringInstance = Instantiate(ringPrefab, activeParent);
            if (ringInstance.TryGetComponent(out ElementRingController controller)) {
                activeRings.Enqueue(controller);
            } else {
                Log.Error("Provided ring prefab does not have ElementRingController component!");
                Destroy(ringInstance);
            }
        }

        private void Update() {
            animStopwatch += Time.deltaTime;
            if (animStopwatch > exitAnimAt && endingAnimation && !endingAnimation.enabled) {
                endingAnimation.enabled = true;
            }
        }

        private void FixedUpdate() {
            gameStopwatch += Time.deltaTime;

            HandleMinigameInputs();

            if (currentTiming >= ringTimings.Count) {
                return;
            }

            if (gameStopwatch >= ringTimings[currentTiming]) {
                if (currentTiming + 1 < ringCount) {
                    SpawnRandomRing();
                } else {
                    SpawnRing(soulRingPrefab);
                }
                currentTiming++;
            }
        }

        private void HandleMinigameInputs() {
            if (characterBody && characterBody.inputBank && characterBody.inputBank.skill1.justPressed) {
                bool cleanedQueue = false;
                while (!cleanedQueue) {
                    if (activeRings.Count > 0 && activeRings.Peek().HasPassed(selectionRing.ringRadiusLower)) {
                        activeRings.Dequeue();
                    } else {
                        cleanedQueue = true;
                    }
                }

                if (activeRings.Count > 0) {
                    ElementRingController currentRing = activeRings.Peek();
                    if (currentRing.TryHit(selectionRing.ringRadiusUpper, selectionRing.ringRadiusLower)) {
                        activeRings.Dequeue();
                        currentRing.transform.SetParent(effectParent, true);
                        ApplyRingEffect(currentRing.element);
                        if (currentRing.element == Element.Soul) {
                            Util.PlaySound("Play_LunarDragonMinigameBigHit", characterBody.gameObject);
                        } else {
                            Util.PlaySound("Play_LunarDragonMinigameSmallHit", characterBody.gameObject);
                        }
                        selectionRing.FlashSuccess();
                    } else {
                        ApplyMissPunishment();
                        Util.PlaySound("Play_LunarDragonMinigameMiss", characterBody.gameObject);
                        selectionRing.FlashFailure();
                    }
                }
            }
        }

        private void ApplyRingEffect(Element element) {
            switch (element) {
                case Element.Blood:
                    bonusDamage += bloodDamageIncrease;
                    bonusHealing += soulHealIncrease;
                    break;
                case Element.Design:
                    bonusCDReduction += designCooldownReduction;
                    bonusHealing += soulHealIncrease;
                    break;
                case Element.Mass:
                    bonusRadius += massRadiusIncrease;
                    bonusHealing += soulHealIncrease;
                    break;
                case Element.Soul:
                    bonusDamage += bloodDamageIncrease;
                    bonusRadius += massRadiusIncrease;
                    bonusCDReduction += designCooldownReduction;
                    bonusHealing += soulHealIncrease;
                    ApplySoulHeal();
                    break;
            }
        }

        private void ApplyMissPunishment() {
            bonusDamage = Mathf.Clamp(bonusDamage - bloodDamageIncrease, 0f, Mathf.Infinity);
            bonusRadius = Mathf.Clamp(bonusRadius - massRadiusIncrease, 0f, Mathf.Infinity);
            bonusHealing = Mathf.Clamp(bonusHealing - soulHealIncrease, 0f, Mathf.Infinity);
            bonusCDReduction = Mathf.Clamp(bonusCDReduction - designCooldownReduction, 0f, Mathf.Infinity);
        }

        private void ApplySoulHeal() {
            if (characterBody && characterBody.healthComponent) {
                new SyncSpecialHeal(characterBody.networkIdentity.netId, characterBody.healthComponent.fullHealth * bonusHealing).Send(NetworkDestination.Server);
            }
        }
    }
}
