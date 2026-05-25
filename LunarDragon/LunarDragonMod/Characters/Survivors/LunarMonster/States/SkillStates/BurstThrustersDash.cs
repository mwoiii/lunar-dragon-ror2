using System;
using EntityStates;
using EntityStates.Mage;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class BurstThrustersDash : MageCharacterMain {
        // based off chef code

        public float duration;

        public float turnSpeed = 50f;

        public float speedMultiplier = 1.7f;

        public float damageCoefficient = LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient;

        public static float upwardForceMagnitude = 2400f;

        public static float hitPauseDuration = 0.04f;

        public static float recoilAmplitude = 1f;

        public static float knockbackForce = 1800f;

        public static float smallHopVelocity = 14f;

        public bool canExecuteSkills;

        public bool shouldFireTrail;

        public bool shouldActivateHitbox;

        private bool ownsFireTrail;

        private float hitPauseTimer;

        private Vector3 idealDirection;

        private OverlapAttack attack;

        private bool inHitPause;

        private float dashStopWatch;

        private int originalLayer;

        private Transform modelTransform;

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

        public override void ProcessJump() {
        }
        public override bool CanExecuteSkill(GenericSkill skillSlot) {
            return canExecuteSkills;
        }

        private void StartThrustersDash() {
            ownsFireTrail = HasBuff(RoR2Content.Buffs.AffixRed);
            if (NetworkServer.active && shouldFireTrail && !ownsFireTrail) {
                base.characterBody.AddBuff(RoR2Content.Buffs.AffixRed);
            }

            originalLayer = base.gameObject.layer;

            // idk what this is
            base.gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(base.teamComponent.teamIndex).intVal;
            base.characterMotor?.Motor.RebuildCollidableLayers();

            // Util.PlaySound("Play_chef_skill3_start", base.gameObject);
            // GetModelAnimator().SetBool("isInRolyPoly", value: true);
            // PlayAnimation("Body", "FireRolyPoly", "FireRolyPoly.playbackRate", 1f);

            if (base.isAuthority) {
                Vector2 vector = Util.Vector3XZToVector2XY(base.inputBank.aimDirection);
                base.characterDirection.moveVector = new Vector3(vector.x, 0f, vector.y).normalized;
            }
            if ((bool)base.modelLocator) {
                base.modelLocator.normalizeToFloor = true;
            }
            if ((bool)base.characterBody) {
                // if ((bool)startEffectPrefab && base.isAuthority) {
                //     EffectManager.SpawnEffect(startEffectPrefab, new EffectData {
                //         origin = base.characterBody.corePosition
                //     }, transmit: true);
                // }
                // if ((bool)midEffectPrefab) {
                //     midEffectInstance = UnityEngine.Object.Instantiate(midEffectPrefab, GetModelTransform());
                //     midEffectInstance.GetComponent<DestroyOnTimer>().duration = duration;
                // }
            }
            // Util.PlaySound("Stop_chef_skill3_charge_loop", base.gameObject);

            if (shouldActivateHitbox) {
                PerformAttack();
            }
        }

        private void PerformAttack() {
            if (!base.isAuthority) {
                return;
            }
            HitBoxGroup hitBoxGroup = null;
            Transform transform = GetModelTransform();
            if ((bool)transform) {
                hitBoxGroup = Array.Find(transform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Charge");
            }
            attack = new OverlapAttack();
            attack.attacker = base.gameObject;
            attack.inflictor = base.gameObject;
            attack.teamIndex = GetTeam();
            attack.damage = damageCoefficient * damageStat;
            // attack.hitEffectPrefab = impactEffectPrefab;
            attack.forceVector = Vector3.up * upwardForceMagnitude;
            attack.pushAwayForce = knockbackForce;
            attack.hitBoxGroup = hitBoxGroup;
            attack.isCrit = RollCrit();
            attack.damageType = new DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.Utility);
            attack.retriggerTimeout = 0.5f;
        }

        private void ExitThrustersDash() {
            if (NetworkServer.active && shouldFireTrail && !ownsFireTrail) {
                // won't detect getting fire elite mid-skill, if it matters
                base.characterBody.RemoveBuff(RoR2Content.Buffs.AffixRed);
            }

            // lunarDragonController.rolyPolyStarted = false;
            // lunarDragonController.rolyPolyGearCharge = 0;
            // lunarDragonController.rolyPolyActive = false;
            // lunarDragonController.blockOtherSkills = false;

            base.gameObject.layer = originalLayer;
            base.characterMotor?.Motor.RebuildCollidableLayers();
            if (!outer.destroying && (bool)base.characterBody) {
                // if ((bool)endEffectPrefab && base.isAuthority) {
                //     EffectManager.SpawnEffect(endEffectPrefab, new EffectData {
                //         origin = base.characterBody.corePosition
                //     }, transmit: true);
                // }
                base.characterBody.isSprinting = false;
            }

            /*
            if (midEffectInstance != null) {
                midEffectInstance.GetComponent<DestroyOnTimer>().duration = 0f;
            }
            if ((bool)base.modelLocator) {
                base.modelLocator.normalizeToFloor = false;
            }
            
            GetModelAnimator().SetBool("isInRolyPoly", value: false);
            GetModelAnimator().SetBool("isInBoostedRolyPoly", value: false);
            PlayCrossfade("Body", "ExitRolyPoly", 0.1f);
            AkSoundEngine.StopPlayingID(soundID);

            Util.PlaySound(endSoundString, base.gameObject);
            Util.PlaySound("Stop_chef_skill3_active_loop", base.gameObject);
            Util.PlaySound("Stop_chef_skill3_charge_loop", base.gameObject);
            */
        }

        private void UpdateDirection() {
            if ((bool)base.inputBank) {
                Vector3 moveVector3 = base.inputBank.moveVector;

                Vector2 moveVector2 = (!(moveVector == Vector3.zero)) ? Util.Vector3XZToVector2XY(moveVector) : Util.Vector3XZToVector2XY(base.inputBank.aimDirection);
                if (moveVector2 != Vector2.zero) {
                    moveVector2.Normalize();
                    idealDirection = new Vector3(moveVector2.x, 0f, moveVector2.y).normalized;
                } else {
                }
                base.characterDirection.moveVector = Vector3.Lerp(base.characterDirection.moveVector, idealDirection, Time.deltaTime * turnSpeed);
            }
        }

        private Vector3 GetIdealVelocity() {
            return base.characterDirection.forward * base.characterBody.moveSpeed * base.characterBody.sprintingSpeedMultiplier * speedMultiplier;
        }

        private void ThrustersDashFixedUpdate() {

            if (characterMotor.velocity.y < 0f && !characterMotor.isGrounded) {
                jetpackStateMachine.SetNextState(new JetpackOn());
            } else {
                jetpackStateMachine.SetNextState(new Idle());
            }

            dashStopWatch += GetDeltaTime();
            if (dashStopWatch >= duration) {
                outer.SetNextStateToMain();
            } else {
                if (!base.isAuthority) {
                    return;
                }
                if ((bool)base.characterBody) {
                    base.characterBody.isSprinting = true;
                }
                if ((bool)base.skillLocator.special && base.inputBank.skill4.down) {
                    base.skillLocator.special.ExecuteIfReady();
                }
                UpdateDirection();
                if (!inHitPause) {
                    if ((bool)base.characterDirection && (bool)base.characterMotor && !base.characterMotor.disableAirControlUntilCollision) {
                        Vector3 velocity = base.characterBody.characterMotor.velocity;
                        Vector3 idealVelocity = GetIdealVelocity();
                        base.characterMotor.velocity = new Vector3(idealVelocity.x, velocity.y, idealVelocity.z);
                    }
                    if (shouldActivateHitbox && attack.Fire()) {
                        if (base.characterMotor.isGrounded) {
                            inHitPause = true;
                            hitPauseTimer = hitPauseDuration;
                            AddRecoil(-0.25f * recoilAmplitude, -0.25f * recoilAmplitude, -0.25f * recoilAmplitude, 0.25f * recoilAmplitude);
                        } else {
                            base.characterMotor.velocity.y = Mathf.Max(base.characterMotor.velocity.y, smallHopVelocity);
                            // lunarDragonController.enemiesInAirHit++;
                            // Debug.Log(lunarDragonController.enemiesInAirHit);
                        }
                    }
                } else {
                    base.characterMotor.velocity = Vector3.zero;
                    hitPauseTimer -= GetDeltaTime();
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
