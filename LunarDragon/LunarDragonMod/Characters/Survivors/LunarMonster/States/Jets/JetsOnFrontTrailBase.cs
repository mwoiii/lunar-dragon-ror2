using System.Collections;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    public class JetsOnFrontTrailBase : JetsOnBase {

        protected TrailRenderer jetLeftTrail;

        protected TrailRenderer jetRightTrail;

        protected virtual Color trailColour { get; }

        protected virtual float trailTime { get; }

        public override void Reset() {
            base.Reset();
            jetLeftTrail = null;
            jetRightTrail = null;
        }

        protected override void GetJetEffects() {
            jetLeftEffect = FindModelChild("JetLeftFront");
            jetRightEffect = FindModelChild("JetRightFront");

            Transform leftTrailTransform = FindModelChild("JetTrailLeft");
            if (leftTrailTransform) {
                jetLeftTrail = leftTrailTransform.GetComponent<TrailRenderer>();
            }

            Transform rightTrailTransform = FindModelChild("JetTrailRight");
            if (rightTrailTransform) {
                jetRightTrail = rightTrailTransform.GetComponent<TrailRenderer>();
            }
        }

        public override void OnEnter() {
            base.OnEnter();

            if (jetLeftTrail) {
                jetLeftTrail.emitting = true;
                jetLeftTrail.time = trailTime;
                jetLeftTrail.material.SetColor("_TintColor", trailColour);
            }

            if (jetRightTrail) {
                jetRightTrail.emitting = true;
                jetRightTrail.time = trailTime;
                jetRightTrail.material.SetColor("_TintColor", trailColour);
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (characterBody) {
                characterBody.StartCoroutine(FadeOutTrails());
            }
        }

        private IEnumerator FadeOutTrails() {
            if (jetLeftTrail && jetRightTrail) {
                const float fadeSpeed = 12f;
                float prevTrailTime = jetLeftTrail.time;

                while (prevTrailTime > 0.1f) {
                    if (jetLeftTrail.time != prevTrailTime) { // detect an outside influence
                        yield break;
                    }

                    float newTrailTime = Mathf.Lerp(prevTrailTime, 0f, Time.deltaTime * fadeSpeed);

                    if (jetLeftTrail) {
                        jetLeftTrail.time = newTrailTime;
                    }

                    if (jetRightTrail) {
                        jetRightTrail.time = newTrailTime;
                    }

                    prevTrailTime = newTrailTime;

                    yield return null;
                }
            }

            if (jetLeftTrail) {
                jetLeftTrail.emitting = false;
            }

            if (jetRightTrail) {
                jetRightTrail.emitting = false;
            }
        }
    }
}
