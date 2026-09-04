using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushRising : DracoAmbushBase {

        private float lifetime = 3f;

        private float stopwatch;

        public Vector3 targetFootPosition;

        private AnimationCurve xCurve = LunarDragonAssets.specialAmbushRisingData.xCurve;

        private AnimationCurve yCurve = LunarDragonAssets.specialAmbushRisingData.yCurve;

        private AnimationCurve zCurve = LunarDragonAssets.specialAmbushRisingData.zCurve;

        private Transform modelBaseTransform;

        public override void OnEnter() {
            base.OnEnter();
            OnTakeoff();
        }

        private void OnTakeoff() {
            modelBaseTransform = GetModelBaseTransform();
            ApplyAmbushAscend();
            if (isAuthority && characterBody && characterBody.teamComponent) {
                FireExplosion();
            }
        }

        private void FireExplosion() {
            BlastAttack blastAttack = new BlastAttack {
                attacker = characterBody.gameObject,
                baseDamage = characterBody.damage * LunarDragonStaticValues.specialAmbushTakeoffDamageCoefficient,
                crit = characterBody.RollCrit(),
                falloffModel = BlastAttack.FalloffModel.HalfLinear,
                inflictor = characterBody.gameObject,
                position = characterBody.transform.position,
                procChainMask = default,
                baseForce = 1500f,
                procCoefficient = 1f,
                radius = 18f,
                teamIndex = characterBody.teamComponent.teamIndex,
                damageType = DamageType.IgniteOnHit
            };
            blastAttack.Fire();
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (isAuthority) {
                if (stopwatch >= lifetime) {
                    authorityFinished = true;
                    outer.SetNextState(new DracoAmbushDescending() {
                        targetFootPosition = targetFootPosition,
                        hurtBoxGroup = hurtBoxGroup,
                    });
                }
            }
            if (modelTransform) {
                float scaledTime = stopwatch / lifetime;
                Vector3 offset = (
                    modelBaseTransform.forward * zCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationXMult +
                    modelBaseTransform.right * xCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationZMult +
                    modelBaseTransform.up * yCurve.Evaluate(scaledTime) * LunarDragonStaticValues.specialAmbushAnimationYMult
                );
                modelTransform.LookAt(characterBody.footPosition + offset);
                modelTransform.position = characterBody.footPosition + offset;
            }
        }
    }
}