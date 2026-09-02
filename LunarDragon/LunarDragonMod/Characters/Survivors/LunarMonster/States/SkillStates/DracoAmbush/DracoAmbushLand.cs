using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushLand : DracoAmbushBase {

        private float stopwatch;

        private float lifetime = 0.75f;

        public override void OnEnter() {
            base.OnEnter();
            OnLand();
        }

        private void OnLand() {
            PlayAnimation("FullBody, Override", "SpecialDiveEnd");
            if (controller) {
                if (isAuthority) {
                    controller.jetpackStateMachine.SetNextState(EntityStateCatalog.InstantiateState(typeof(JetsOff)));
                }
                controller.DisableFireAura();
            }
            if (characterBody) {
                EffectManager.SpawnEffect(LunarDragonAssets.specialLandingExplosionEffect, new EffectData {
                    origin = characterBody.footPosition,
                    rotation = characterBody.transform.rotation
                }, false);
            }
            if (isAuthority) {
                if (interactor) {
                    interactor.isRemoteOp = false;
                }
                FireExplosion();
            }
            if (modelLocator) {
                modelLocator.autoUpdateModelTransform = true;
                if (modelTransform) {
                    Util.PlaySound("Stop_UI_podDescentLoop", modelLocator.modelTransform.gameObject);
                    Util.PlaySound("Stop_lemurianBruiser_m1_fly_loop", modelLocator.modelTransform.gameObject);
                    Util.PlaySound("Play_captain_R_impact", modelLocator.modelTransform.gameObject);
                    Util.PlaySound("Play_falseson_skill1_impact_full", modelLocator.modelTransform.gameObject);
                    Util.PlaySound("Play_LunarDragonAmbushImpact", modelLocator.modelTransform.gameObject);
                }
            }
            if (hurtBoxGroup) {
                hurtBoxGroup.hurtBoxesDeactivatorCounter--;
            }
        }

        public override void OnExit() {
            base.OnExit();
            if (isAuthority && characterMotor) {
                characterMotor.useGravity = true;
            }
            Animator animator = GetModelAnimator();
            if (animator) {
                animator.SetBool(LunarDragonAnimationParameters.forceIdle, false);
            }
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (stopwatch >= lifetime) {
                PlayAnimation("OuterCannons, Override", "FlipCannons");
                if (isAuthority) {
                    authorityFinished = true;
                    if (controller) {
                        controller.ResetAllSkillStateMachines();
                    }
                    outer.SetNextStateToMain();
                }
            }
        }

        private void FireExplosion() {
            BlastAttack blastAttack = new BlastAttack {
                attacker = gameObject,
                baseDamage = characterBody.damage * LunarDragonStaticValues.specialAmbushLandDamageCoefficient,
                crit = characterBody.RollCrit(),
                position = characterBody.transform.position,
                falloffModel = BlastAttack.FalloffModel.HalfLinear,
                inflictor = gameObject,
                procChainMask = default,
                procCoefficient = 1f,
                radius = 55f,
                teamIndex = characterBody.teamComponent.teamIndex,
            };
            blastAttack.damageType |= DamageType.IgniteOnHit;
            blastAttack.Fire();
        }
    }
}