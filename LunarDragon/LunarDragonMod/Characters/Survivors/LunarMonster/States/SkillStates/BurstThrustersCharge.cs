using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using System;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class BurstThrustersCharge : LunarDragonMain {
        // based off chef code

        public float baseChargeDuration = 2f;

        public float minChargeForChargedAttack = 0.1f;

        public float walkSpeedCoefficient = 0.65f;

        // a value 0-1 of how far through the charging process
        public float gearCharge;

        // proportion thresholds for the charge
        private static readonly float[] CHARGE_THRESHOLDS = new float[] {
            0.3f, 0.6f, 0.9f
        };

        // index pointing to current threshold that needs reaching
        // doubles as the current charge level
        private int currentCharge;

        private bool hasFinishedCharging;

        protected float chargeDuration { get; private set; }

        public override void OnEnter() {
            base.OnEnter();
            if (!characterMotor.isGrounded) {
                hasFinishedCharging = true;
            }
            StartChargeThrusters();
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void ProcessJump() {
        }

        private void StartChargeThrusters() {
            chargeDuration = baseChargeDuration / attackSpeedStat;
            // PlayAnimation("Body", "ChargeRolyPoly", "ChargeRolyPoly.playbackRate", 1f);

            // useRootMotion = ((bool)base.characterBody && base.characterBody.rootMotionInMainState && base.isGrounded) || (bool)base.railMotor;
            // hasCharacterMotor = base.characterMotor;
            // hasRailMotor = base.railMotor;fi
            // hasCharacterDirection = base.characterDirection;
            // Util.PlaySound("Play_chef_skill3_charge_start", base.gameObject);
        }

        private void HandleRotation() {
            moveVector = base.inputBank.moveVector;
            aimDirection = base.inputBank.aimDirection;
            /*
            if (useRootMotion) {
                if (hasCharacterMotor) {
                    base.characterMotor.moveDirection = Vector3.zero;
                }
                if (hasRailMotor) {
                    base.railMotor.inputMoveVector = moveVector;
                }
            } else {
                if (hasCharacterMotor) {
                    base.characterMotor.moveDirection = moveVector;
                }
                if (hasRailMotor) {
                    base.railMotor.inputMoveVector = moveVector;
                }
            }
            if (!hasRailMotor && hasCharacterDirection) {
                if (hasAimAnimator && aimAnimator.aimType == AimAnimator.AimType.Smart) {
                    Vector3 vector = ((moveVector == Vector3.zero) ? base.characterDirection.forward : moveVector);
                    float num = Vector3.Angle(aimDirection, vector);
                    float num2 = Mathf.Max(aimAnimator.pitchRangeMax + aimAnimator.pitchGiveupRange, aimAnimator.yawRangeMax + aimAnimator.yawGiveupRange);
                    base.characterDirection.moveVector = (((bool)base.characterBody && base.characterBody.shouldAim && num > num2) ? aimDirection : vector);
                } else {
                    base.characterDirection.moveVector = (((bool)base.characterBody && base.characterBody.shouldAim) ? aimDirection : moveVector);
                }
            }
            */
        }
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
            BurstThrustersDash nextState = new BurstThrustersDash();

            nextState.duration = Mathf.Max(0.5f, currentCharge * 2f); // 0.5, 2, 4
            nextState.damageCoefficient = currentCharge > 1 ? LunarDragonStaticValues.utilityBurstThrustersUpperDamageCoefficient : LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient;
            nextState.shouldActivateHitbox = currentCharge > 0;
            nextState.shouldFireTrail = currentCharge > 1;
            nextState.canExecuteSkills = currentCharge == 0;
            if (currentCharge == 0) {
                nextState.speedMultiplier = 1.8f;
            }
            skillLocator.utility.temporaryCooldownPenalty = currentCharge * 5; // 2, 7, 10

            outer.SetNextState(nextState);
        }

        private void ChargeThrustersFixedUpdate() {
            if (gearCharge > CHARGE_THRESHOLDS[0]) {
                base.characterMotor.walkSpeedPenaltyCoefficient = walkSpeedCoefficient;
            }

            gearCharge = Mathf.Clamp01(base.fixedAge / chargeDuration);
            base.characterBody.SetSpreadBloom(gearCharge);
            base.characterBody.SetAimTimer(3f);

            // triggers each time a threshold is reached
            if (gearCharge >= minChargeForChargedAttack && gearCharge != 1f && gearCharge >= CHARGE_THRESHOLDS[currentCharge]) {
                currentCharge = Math.Min(currentCharge + 1, CHARGE_THRESHOLDS.Length - 1);
            }

            ChargeThrustersAuthorityFixedUpdate();
        }

        private void ChargeThrustersAuthorityFixedUpdate() {
            if (!base.isAuthority) {
                return;
            }

            // baseskillstate for networking
            if (base.inputBank.skill3.justReleased || gearCharge >= CHARGE_THRESHOLDS[^1]) {
                Log.Info("released utility");
                hasFinishedCharging = true;
            }
            HandleRotation();
        }

        private void ExitChargeThrusters() {
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }

}
