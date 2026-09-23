using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushLand : DracoAmbushBase {

        private float stopwatch;

        private float lifetime = 0.75f;

        private float bonusRadius;

        private float bonusDamage;

        private const float baseExplosionRadius = 55f;

        public override void OnEnter() {
            base.OnEnter();
            OnLand();
        }

        private void OnLand() {
            ApplyAmbushLand();
            if (isAuthority && characterBody) {
                if (controller && controller.minigameInstance) {
                    bonusDamage = controller.minigameInstance.bonusDamage;
                    bonusRadius = controller.minigameInstance.bonusRadius;
                    ApplyCooldownReduction();
                    //Log.Info($"bonusDamage: {bonusDamage}");
                    //Log.Info($"bonusRadius: {bonusRadius}");
                    //Log.Info($"final damage coeff: {LunarDragonStaticValues.specialAmbushLandDamageCoefficient + bonusDamage}");
                    //Log.Info($"final radius: {baseExplosionRadius + bonusRadius}");
                    //Log.Info($"final scale: {LunarDragonAssets.specialLandingExplosionEffect.transform.localScale.x * (1f + (bonusRadius / baseExplosionRadius))}");
                }
                FireExplosion();
                EffectManager.SpawnEffect(LunarDragonAssets.specialLandingExplosionEffect, new EffectData {
                    origin = characterBody.footPosition,
                    rotation = characterBody.transform.rotation,
                    scale = LunarDragonAssets.specialLandingExplosionEffect.transform.localScale.x * (1f + (bonusRadius / baseExplosionRadius))
                }, transform);
            }
            if (modelTransform) {
                Util.PlaySound("Play_captain_R_impact", modelLocator.modelTransform.gameObject);
                Util.PlaySound("Play_falseson_skill1_impact_full", modelLocator.modelTransform.gameObject);
                Util.PlaySound("Play_LunarDragonAmbushImpact", modelLocator.modelTransform.gameObject);
            }
        }

        private void ApplyCooldownReduction() {
            if (skillLocator && skillLocator.special) {
                skillLocator.special.rechargeStopwatch += controller.minigameInstance.bonusCDReduction;
            }
        }

        public override void OnExit() {
            base.OnExit();
            ApplyAmbushEnd();
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
                baseDamage = characterBody.damage * (LunarDragonStaticValues.specialAmbushLandDamageCoefficient + bonusDamage),
                crit = characterBody.RollCrit(),
                position = characterBody.transform.position,
                falloffModel = BlastAttack.FalloffModel.HalfLinear,
                inflictor = gameObject,
                procChainMask = default,
                procCoefficient = 1f,
                radius = baseExplosionRadius + bonusRadius,
                teamIndex = characterBody.teamComponent.teamIndex,
            };
            blastAttack.damageType |= DamageType.IgniteOnHit;
            blastAttack.Fire();
        }
    }
}