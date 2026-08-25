using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using System;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class BurstThrustersCharge : BaseState {

        public float baseChargeDuration = 2f;

        public float minChargeForChargedAttack = 0.1f;

        public float penaltyCoefficient = 0.01f;

        private const float turnSpeedCharge = 80f;

        private const float turnSpeedDash = 220f;

        private LunarDragonController controller;

        private bool hasFinishedCharging;

        private float previousTurnSpeed;

        public float gearCharge; // a value 0-1 of how far through the charging process

        private int currentCharge; // index pointing to current threshold that needs reaching. doubles as the current charge level

        private float chargeDuration;

        private static readonly float[] chargeThresholds = new float[] { // proportion thresholds for the charge. for misc purposes (vfx/damage)
            0.3f, 0.6f, 0.7f, 0.9f
        };

        public override void OnEnter() {
            base.OnEnter();
            StartChargeThrusters();
        }

        public override void OnExit() {
            base.OnExit();
            ExitChargeThrusters();
        }

        private void StartChargeThrusters() {
            chargeDuration = baseChargeDuration / attackSpeedStat;
            controller = GetComponent<LunarDragonController>();

            if (isAuthority) {
                if (characterDirection) {
                    previousTurnSpeed = characterDirection.turnSpeed;
                }
                if (controller) {
                    controller.DisableWeaponStateMachine();
                    controller.canJump = false;
                }
                if (!characterMotor.isGrounded) {
                    hasFinishedCharging = true;
                }
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (isAuthority && hasFinishedCharging) {
                SetupDashState();
            } else {
                ChargeThrustersFixedUpdate();
            }
        }

        private void SetupDashState() {
            BurstThrustersDash nextState = currentCharge > 1 ? new BurstThrustersDashTrail() : new BurstThrustersDash();
            nextState.duration = Mathf.Max(0.5f, currentCharge * 2f); // 0.5, 2, 4
            nextState.damageCoefficient = currentCharge > 1 ? LunarDragonStaticValues.utilityBurstThrustersUpperDamageCoefficient : LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient;
            nextState.shouldActivateHitbox = currentCharge > 0;
            nextState.turnSpeed = currentCharge >= 1 ? turnSpeedDash : previousTurnSpeed;

            if (currentCharge == 0) {
                nextState.speedMultiplier = 1.8f;
            }

            GameObject startEffect = null;

            switch (currentCharge) {
                case 0:
                    startEffect = LunarDragonAssets.utilityDashLightEffect;
                    nextState.jetState = typeof(JetsOnFrontTrailLight);
                    break;
                case 1:
                    startEffect = LunarDragonAssets.utilityDashMediumEffect;
                    nextState.jetState = typeof(JetsOnFrontTrailMedium);
                    break;
                default:
                    startEffect = LunarDragonAssets.utilityDashHeavyEffect;
                    nextState.jetState = typeof(JetsOnFrontTrailHeavy);
                    break;
            }

            skillLocator.utility.temporaryCooldownPenalty = currentCharge * 5 * skillLocator.utilityBonusStockSkill.cooldownScale; // 2, 7, 10
            EntityStateMachine bodyStateMachine = FindSiblingStateMachine("Body");
            if (bodyStateMachine) {
                bodyStateMachine.SetNextState(nextState);
                if (characterBody && startEffect) {
                    EffectManager.SpawnEffect(startEffect, new EffectData {
                        origin = characterBody.corePosition
                    }, true);
                }
            }
            if (characterDirection) {
                characterDirection.turnSpeed = previousTurnSpeed;
            }
            outer.SetNextStateToMain();
        }

        private void ChargeThrustersFixedUpdate() {
            if (isAuthority && gearCharge > chargeThresholds[0]) {
                characterMotor.walkSpeedPenaltyCoefficient = penaltyCoefficient;
                characterBody.SetSpreadBloom(gearCharge);
                characterBody.SetAimTimer(3f);
            }

            gearCharge = Mathf.Clamp01(fixedAge / chargeDuration);

            // triggers each time a threshold is reached
            if (gearCharge >= minChargeForChargedAttack && gearCharge != 1f && gearCharge >= chargeThresholds[currentCharge]) {
                currentCharge = Math.Min(currentCharge + 1, chargeThresholds.Length - 1);
                switch (currentCharge) {
                    case 1:
                        PlayCrossfade("FullBody, Override", "UtilityCharge", "Charge.playbackRate", chargeDuration * 0.5f, chargeDuration * 0.2f);
                        if (isAuthority && characterDirection) {
                            characterDirection.turnSpeed = turnSpeedCharge;
                        }
                        break;
                    case 2:
                        if (isAuthority && controller && controller.bodyState != null) {
                            controller.bodyState.ForceJetsOn(typeof(JetsOnFront));
                        }
                        EffectManager.SpawnEffect(LunarDragonAssets.utilitySmokeEffect, new EffectData {
                            origin = characterBody.footPosition,
                            rotation = modelLocator.modelBaseTransform.rotation
                        }, false);
                        break;
                }
            }

            if (isAuthority && inputBank.skill3.justReleased || gearCharge >= chargeThresholds[^1]) {
                hasFinishedCharging = true;
            }
        }

        private void ExitChargeThrusters() {
            if (gearCharge < chargeThresholds[0]) {
                PlayAnimation("FullBody, Override", "UtilityLoop");
            } else if (gearCharge < chargeThresholds[^1]) {
                PlayCrossfade("FullBody, Override", "UtilityFire", 0.005f);
            }

            GetModelAnimator().SetBool("inUtilityLoop", true);

            if (isAuthority) {
                if (controller) {
                    controller.DisableWeaponStateMachine();
                }
                if (characterBody && characterBody.teamComponent) {
                    BlastAttack blastAttack = new BlastAttack {
                        attacker = characterBody.gameObject,
                        baseDamage = characterBody.damage * (2f + 12f * gearCharge),
                        crit = characterBody.RollCrit(),
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = characterBody.gameObject,
                        position = characterBody.transform.position,
                        procChainMask = default(ProcChainMask),
                        baseForce = 200f + 1200f * gearCharge,
                        procCoefficient = 1f,
                        radius = 4f + 10f * gearCharge,
                        teamIndex = characterBody.teamComponent.teamIndex,
                        damageType = DamageType.IgniteOnHit
                    };
                    blastAttack.Fire();
                }

                characterMotor.walkSpeedPenaltyCoefficient = 1f;
                if (controller) {
                    controller.canJump = true;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}
