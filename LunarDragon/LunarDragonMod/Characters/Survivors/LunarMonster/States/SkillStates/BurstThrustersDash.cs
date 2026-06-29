using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    // Based off chef code

    public class BurstThrustersDash : LunarDragonMain {

        public float duration;

        public float turnSpeed = 50f;

        public float speedMultiplier = 1.7f;

        public float damageCoefficient = LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient;

        public GameObject startEffect;

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
            return canExecuteSkills;
        }

        private void StartThrustersDash() {
            controller.bodyState.ForceJetsOn(JetDirection.Front);

            ownsFireTrail = HasBuff(RoR2Content.Buffs.AffixRed);
            if (NetworkServer.active && shouldFireTrail && !ownsFireTrail) {
                characterBody.AddBuff(RoR2Content.Buffs.AffixRed);
            }

            originalLayer = gameObject.layer;

            // idk what this is
            gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(teamComponent.teamIndex).intVal;
            characterMotor?.Motor.RebuildCollidableLayers();

            // Util.PlaySound("Play_chef_skill3_start", base.gameObject);


            if (isAuthority) {
                Vector2 vector = Util.Vector3XZToVector2XY(inputBank.aimDirection);
                characterDirection.moveVector = new Vector3(vector.x, 0f, vector.y).normalized;
            }

            if (modelLocator) {
                modelLocator.normalizeToFloor = true;
            }

            if (characterBody && isAuthority && startEffect) {
                EffectManager.SpawnEffect(startEffect, new EffectData {
                    origin = characterBody.corePosition
                }, true);
            }
            // Util.PlaySound("Stop_chef_skill3_charge_loop", base.gameObject);

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
                damage = damageCoefficient * damageStat,
                // attack.hitEffectPrefab = impactEffectPrefab;
                forceVector = Vector3.up * upwardForceMagnitude,
                pushAwayForce = knockbackForce,
                hitBoxGroup = hitBoxGroup,
                isCrit = RollCrit(),
                damageType = new DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.Utility),
                retriggerTimeout = 0.5f
            };
        }

        private void ExitThrustersDash() {
            if (NetworkServer.active && shouldFireTrail && !ownsFireTrail) {
                // won't detect getting fire elite mid-skill, if it matters
                // it probably matters just do it differently roo
                characterBody.RemoveBuff(RoR2Content.Buffs.AffixRed);
            }

            GetModelAnimator().SetBool("inUtilityLoop", false);

            // lunarDragonController.rolyPolyStarted = false;
            // lunarDragonController.rolyPolyGearCharge = 0;
            // lunarDragonController.rolyPolyActive = false;
            // lunarDragonController.blockOtherSkills = false;

            gameObject.layer = originalLayer;
            characterMotor?.Motor.RebuildCollidableLayers();
            if (!outer.destroying && characterBody) {
                // if ((bool)endEffectPrefab && base.isAuthority) {
                //     EffectManager.SpawnEffect(endEffectPrefab, new EffectData {
                //         origin = base.characterBody.corePosition
                //     }, transmit: true);
                // }
                characterBody.isSprinting = false;
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
            if (inputBank) {
                Vector3 moveVector3 = inputBank.moveVector;

                Vector2 moveVector2 = (!(moveVector == Vector3.zero)) ? Util.Vector3XZToVector2XY(moveVector) : Util.Vector3XZToVector2XY(inputBank.aimDirection);
                if (moveVector2 != Vector2.zero) {
                    moveVector2.Normalize();
                    idealDirection = new Vector3(moveVector2.x, 0f, moveVector2.y).normalized;
                } else {
                }
                characterDirection.moveVector = Vector3.Lerp(characterDirection.moveVector, idealDirection, Time.deltaTime * turnSpeed);
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
            if (dashStopWatch >= duration) {
                outer.SetNextStateToMain();
            } else {

                if (characterBody) {
                    characterBody.isSprinting = true;
                }

                if (skillLocator.special && inputBank.skill4.down) {
                    skillLocator.special.ExecuteIfReady();
                }

                UpdateDirection();

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
