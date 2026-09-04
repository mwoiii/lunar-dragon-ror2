using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushAscend : DracoAmbushBase {

        private float stopwatch;

        private float duration;

        private const float groundDuration = 0.86f;

        private const float airDuration = 0.4f;

        private const float jetsTime = 0.4f; // air anim is just ground anim with first portion skipped

        private bool jetsActive;

        public Vector3 targetFootPosition;

        public override void OnEnter() {
            base.OnEnter();
            ApplyAmbushStart();
            if (isGrounded) {
                PlayCrossfade("FullBody, Override", "SpecialDiveStart", 0.005f);
                duration = groundDuration;
            } else {
                PlayCrossfade("FullBody, Override", "SpecialDiveStartAir", 0.005f);
                duration = airDuration;
            }
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (!jetsActive && stopwatch > duration - jetsTime) {
                if (isAuthority && controller) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOnFront)));
                }
                EffectManager.SpawnEffect(LunarDragonAssets.specialLiftoffSmokeEffect, new EffectData {
                    origin = characterBody.footPosition,
                    rotation = characterBody.transform.rotation
                }, false);
                jetsActive = true;
            }
            if (stopwatch >= duration && isAuthority) {
                authorityFinished = true;
                outer.SetNextState(new DracoAmbushRising() {
                    targetFootPosition = targetFootPosition,
                    hurtBoxGroup = hurtBoxGroup
                });
            }
        }

        public override void OnExit() {
            base.OnExit();
            EffectManager.SpawnEffect(LunarDragonAssets.specialLiftoffExplosionEffect, new EffectData {
                origin = characterBody.footPosition,
                rotation = characterBody.transform.rotation
            }, false);
        }
    }
}