using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using System;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    // based off chef code

    public class BurstThrustersCharge : BaseState {

        public float baseChargeDuration = 1.5f;

        public float minChargeForChargedAttack = 0.1f;

        public float penaltyCoefficient = 0.1f;

        private LunarDragonController controller;

        // a value 0-1 of how far through the charging process
        public float gearCharge;

        // proportion thresholds for the charge
        private static readonly float[] chargeThresholds = new float[] {
            0.3f, 0.6f, 0.9f
        };

        // index pointing to current threshold that needs reaching
        // doubles as the current charge level
        private int currentCharge;

        private bool hasFinishedCharging;

        protected float chargeDuration { get; private set; }

        public override void OnEnter() {
            base.OnEnter();
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
                canExecuteSkills = currentCharge == 0
            };
            if (currentCharge == 0) {
                nextState.speedMultiplier = 1.8f;
            }
            skillLocator.utility.temporaryCooldownPenalty = currentCharge * 5; // 2, 7, 10
            EntityStateMachine bodyStateMachine = FindSiblingStateMachine("Body");
            if (bodyStateMachine) {
                bodyStateMachine.SetNextState(nextState);
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
                if (currentCharge == 1) {
                    PlayCrossfade("FullBody, Override", "UtilityCharge", "Charge.playbackRate", chargeDuration * 0.5f, chargeDuration * 0.2f);
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

            //HandleRotation();
        }

        private void ExitChargeThrusters() {
            if (gearCharge < chargeThresholds[0]) {
                PlayAnimation("FullBody, Override", "UtilityLoop");
            } else if (gearCharge < chargeThresholds[^1]) {
                PlayCrossfade("FullBody, Override", "UtilityFire", 0.005f);
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
