using EntityStates;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    // Based off chef code

    public class BurstThrustersDash : LunarDragonMain {

        public float duration;

        public float speedMultiplier = 1.7f;

        public float damageCoefficient = LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient;

        public float turnSpeed;

        private const float upwardForceMagnitude = 2400f;

        private const float hitPauseDuration = 0.04f;

        private const float recoilAmplitude = 1f;

        private const float knockbackForce = 1800f;

        private const float smallHopVelocity = 14f;

        public bool shouldActivateHitbox;

        private float hitPauseTimer;

        private OverlapAttack attack;

        private bool inHitPause;

        private float dashStopWatch;

        private float previousTurnSpeed;

        private int originalLayer;

        private static GameObject hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>(RoR2_DLC2_Chef.ChefUtilityImpactVFX_prefab).WaitForCompletion();

        public override void OnEnter() {
            base.OnEnter();
            StartThrustersDash();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            ThrustersDashFixedUpdate();
        }

        public override void OnExit() {
            ExitThrustersDash();
            base.OnExit();
        }

        public override bool CanExecuteSkill(GenericSkill skillSlot) {
            return false;
        }

        private void StartThrustersDash() {
            if (characterDirection) {
                previousTurnSpeed = characterDirection.turnSpeed;
                characterDirection.turnSpeed = turnSpeed;
            }

            if (controller && controller.bodyState != null) {
                controller.bodyState.ForceJetsOn();
            }

            if (modelLocator) {
                modelLocator.normalizeToFloor = true;
            }

            if (isAuthority) {
                if (controller) {
                    controller.DisableWeaponStateMachine();
                }
                Vector2 vector = Util.Vector3XZToVector2XY(inputBank.aimDirection);
                characterDirection.moveVector = new Vector3(vector.x, 0f, vector.y).normalized;

                originalLayer = gameObject.layer;
                gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(teamComponent.teamIndex).intVal;
                if (characterMotor && characterMotor.Motor) {
                    characterMotor.Motor.RebuildCollidableLayers();
                }
            }

            if (shouldActivateHitbox) {
                PerformAttack();
            }
        }

        private void PerformAttack() {
            if (!isAuthority) {
                return;
            }

            HitBoxGroup hitBoxGroup = null;
            Transform transform = GetModelTransform();
            if (transform) {
                hitBoxGroup = Array.Find(transform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Charge");
            }

            attack = new OverlapAttack {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = GetTeam(),
                damage = damageStat * damageCoefficient * GetDamageBoostFromSpeed(),
                hitEffectPrefab = hitEffectPrefab,
                forceVector = Vector3.up * upwardForceMagnitude,
                pushAwayForce = knockbackForce,
                hitBoxGroup = hitBoxGroup,
                isCrit = RollCrit(),
                damageType = new DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.Utility),
                retriggerTimeout = 0.5f
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GetDamageBoostFromSpeed() {
            return Mathf.Max(1f, characterBody.moveSpeed / characterBody.baseMoveSpeed);
        }


        private void ExitThrustersDash() {
            if (isAuthority) {
                if (controller) {
                    controller.ResetWeaponStateMachine();
                }
                gameObject.layer = originalLayer;
                if (characterMotor && characterMotor.Motor) {
                    characterMotor.Motor.RebuildCollidableLayers();
                }

                if (characterDirection) {
                    characterDirection.turnSpeed = previousTurnSpeed;
                }

                if (!outer.destroying && characterBody) {
                    characterBody.isSprinting = false;
                }
            }

            if (modelAnimator) {
                modelAnimator.SetBool(LunarDragonAnimationParameters.forceIdle, false);
                if (isGrounded) {
                    PlayAnimation("OuterCannons, Override", "FlipCannons");
                }
            }
        }


        private Vector3 GetIdealVelocity() {
            return characterDirection.forward * characterBody.moveSpeed * characterBody.sprintingSpeedMultiplier * speedMultiplier;
        }

        private void ThrustersDashFixedUpdate() {
            if (!isAuthority) {
                return;
            }

            dashStopWatch += Time.deltaTime;
            if (dashStopWatch >= duration || inputBank.skill3.justPressed) {
                if (inputBank.skill3.justPressed) {
                    inputBank.skill3.hasPressBeenClaimed = true;
                }
                outer.SetNextStateToMain();
            } else {

                if (characterBody) {
                    characterBody.isSprinting = true;
                }

                if (skillLocator.special && inputBank.skill4.down) {
                    skillLocator.special.ExecuteIfReady();
                }

                if (!inHitPause) {
                    if (characterDirection && characterMotor && !characterMotor.disableAirControlUntilCollision) {
                        Vector3 velocity = characterBody.characterMotor.velocity;
                        Vector3 idealVelocity = GetIdealVelocity();
                        characterMotor.velocity = new Vector3(idealVelocity.x, velocity.y, idealVelocity.z);
                    }
                    if (shouldActivateHitbox && attack.Fire()) {
                        if (characterMotor.isGrounded) {
                            inHitPause = true;
                            hitPauseTimer = hitPauseDuration;
                            AddRecoil(-0.25f * recoilAmplitude, -0.25f * recoilAmplitude, -0.25f * recoilAmplitude, 0.25f * recoilAmplitude);
                        } else {
                            characterMotor.velocity.y = Mathf.Max(characterMotor.velocity.y, smallHopVelocity);
                        }
                    }
                } else {
                    characterMotor.velocity = Vector3.zero;
                    hitPauseTimer -= Time.deltaTime;
                    if (hitPauseTimer < 0f) {
                        inHitPause = false;
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}
