using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;
using System.Collections.Generic;
using UnityEngine;

namespace LunarDragonMod.Characters.LunarDragon {
    public class MinigameController : MonoBehaviour {

        private const int ringCount = 6;

        private const float duration = 2f;

        private const float graceWindow = 0.2f;

        [SerializeField]
        private GameObject bloodRingPrefab;

        [SerializeField]
        private GameObject designRingPrefab;

        [SerializeField]
        private GameObject massRingPrefab;

        [SerializeField]
        private GameObject soulRingPrefab;

        [SerializeField]
        private Transform ringParent;

        [SerializeField]
        private AnimateScale endingAnimation;

        private float stopwatch;

        private Queue<ElementRingController> activeRings = new Queue<ElementRingController>(ringCount);

        private List<float> ringTimings = new List<float>(ringCount);

        private GameObject[] randomRingSelection;

        private int currentTiming;

        private bool active = true;

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
            GameObject ringInstance = Instantiate(ringPrefab, ringParent);
            if (ringInstance.TryGetComponent(out ElementRingController controller)) {
                activeRings.Enqueue(controller);
            } else {
                Log.Error("Provided ring prefab does not have ElementRingController component!");
                Destroy(ringInstance);
            }
        }

        private void FixedUpdate() {
            if (!active) {
                return;
            }

            stopwatch += Time.deltaTime;
            if (stopwatch >= ringTimings[currentTiming]) {
                if (currentTiming + 1 < ringCount) {
                    SpawnRandomRing();
                    currentTiming++;
                } else {
                    SpawnRing(soulRingPrefab);
                    active = false;
                }
            }
        }

        private float kill = 0f;
        private void Update() {
            kill += Time.deltaTime;
            if (kill > 3.3f && !endingAnimation.enabled) {
                endingAnimation.enabled = true;
            }
            if (kill > 3.8f) {
                Object.Destroy(gameObject);
            }
        }
    }
}
