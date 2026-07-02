using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using System;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

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
            if (characterDirection) {
                previousTurnSpeed = characterDirection.turnSpeed;
            }
            controller = GetComponent<LunarDragonController>();
            StartChargeThrusters();
        }

        public override void OnExit() {
            base.OnExit();
        }

        private void StartChargeThrusters() {
            if (controller) {
                controller.canJump = false;
            }

            chargeDuration = baseChargeDuration / attackSpeedStat;

            if (!characterMotor.isGrounded) {
                hasFinishedCharging = true;
            }

            // Util.PlaySound("Play_chef_skill3_charge_start", base.gameObject);
        }

        //private void HandleRotation() {
        //    moveVector = inputBank.moveVector;
        //    aimDirection = inputBank.aimDirection;
        //    
        //    if (useRootMotion) {
        //        if (hasCharacterMotor) {
        //            base.characterMotor.moveDirection = Vector3.zero;
        //        }
        //        if (hasRailMotor) {
        //            base.railMotor.inputMoveVector = moveVector;
        //        }
        //    } else {
        //        if (hasCharacterMotor) {
        //            base.characterMotor.moveDirection = moveVector;
        //        }
        //        if (hasRailMotor) {
        //            base.railMotor.inputMoveVector = moveVector;
        //        }
        //    }
        //    if (!hasRailMotor && hasCharacterDirection) {
        //        if (hasAimAnimator && aimAnimator.aimType == AimAnimator.AimType.Smart) {
        //            Vector3 vector = ((moveVector == Vector3.zero) ? base.characterDirection.forward : moveVector);
        //            float num = Vector3.Angle(aimDirection, vector);
        //            float num2 = Mathf.Max(aimAnimator.pitchRangeMax + aimAnimator.pitchGiveupRange, aimAnimator.yawRangeMax + aimAnimator.yawGiveupRange);
        //            base.characterDirection.moveVector = (((bool)base.characterBody && base.characterBody.shouldAim && num > num2) ? aimDirection : vector);
        //        } else {
        //            base.characterDirection.moveVector = (((bool)base.characterBody && base.characterBody.shouldAim) ? aimDirection : moveVector);
        //        }
        //    }
        //    
        //}

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (hasFinishedCharging) {
                ExitChargeThrusters();
                SetupDashState();
            } else {
                ChargeThrustersFixedUpdate();
            }
        }

        private void SetupDashState() {
            BurstThrustersDash nextState = new BurstThrustersDash {
                duration = Mathf.Max(0.5f, currentCharge * 2f), // 0.5, 2, 4
                damageCoefficient = currentCharge > 1 ? LunarDragonStaticValues.utilityBurstThrustersUpperDamageCoefficient : LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient,
                shouldActivateHitbox = currentCharge > 0,
                shouldFireTrail = currentCharge > 1,
                turnSpeed = currentCharge >= 1 ? turnSpeedDash : previousTurnSpeed
            };

            if (currentCharge == 0) {
                nextState.speedMultiplier = 1.8f;
            }

            switch (currentCharge) {
                case 0:
                    nextState.startEffect = LunarDragonAssets.utilityDashLightEffect;
                    break;
                case 1:
                    nextState.startEffect = LunarDragonAssets.utilityDashMediumEffect;
                    break;
                default:
                    nextState.startEffect = LunarDragonAssets.utilityDashHeavyEffect;
                    break;
            }

            skillLocator.utility.temporaryCooldownPenalty = currentCharge * 5 * skillLocator.utilityBonusStockSkill.cooldownScale; // 2, 7, 10
            EntityStateMachine bodyStateMachine = FindSiblingStateMachine("Body");
            if (bodyStateMachine) {
                bodyStateMachine.SetNextState(nextState);
            }
            if (characterDirection) {
                characterDirection.turnSpeed = previousTurnSpeed;
            }
            outer.SetNextStateToMain();
        }

        private void ChargeThrustersFixedUpdate() {
            if (gearCharge > chargeThresholds[0]) {
                characterMotor.walkSpeedPenaltyCoefficient = penaltyCoefficient;
            }

            gearCharge = Mathf.Clamp01(fixedAge / chargeDuration);
            characterBody.SetSpreadBloom(gearCharge);
            characterBody.SetAimTimer(3f);

            // triggers each time a threshold is reached
            if (gearCharge >= minChargeForChargedAttack && gearCharge != 1f && gearCharge >= chargeThresholds[currentCharge]) {
                currentCharge = Math.Min(currentCharge + 1, chargeThresholds.Length - 1);
                switch (currentCharge) {
                    case 1:
                        PlayCrossfade("FullBody, Override", "UtilityCharge", "Charge.playbackRate", chargeDuration * 0.5f, chargeDuration * 0.2f);
                        if (characterDirection) {
                            characterDirection.turnSpeed = turnSpeedCharge;
                        }
                        break;
                    case 2:
                        if (controller && controller.bodyState != null) {
                            controller.bodyState.ForceJetsOn(LunarDragonMain.JetDirection.Front);
                        }
                        break;
                }
            }

            ChargeThrustersAuthorityFixedUpdate();
        }

        private void ChargeThrustersAuthorityFixedUpdate() {
            if (!isAuthority) {
                return;
            }

            // baseskillstate for networking
            if (inputBank.skill3.justReleased || gearCharge >= chargeThresholds[^1]) {
                hasFinishedCharging = true;
            }
        }

        private void ExitChargeThrusters() {
            if (gearCharge < chargeThresholds[0]) {
                PlayAnimation("FullBody, Override", "UtilityLoop");
            } else if (gearCharge < chargeThresholds[^1]) {
                PlayCrossfade("FullBody, Override", "UtilityFire", 0.005f);
            }

            if (characterBody && characterBody.teamComponent) {
                BlastAttack blastAttack = new BlastAttack {
                    attacker = characterBody.gameObject,
                    baseDamage = characterBody.damage * (2f + 6f * gearCharge),
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

            GetModelAnimator().SetBool("inUtilityLoop", true);
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            if (controller) {
                controller.canJump = true;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}
