using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    // Based off chef code

    public class BurstThrustersDash : LunarDragonMain {

        public float duration;

        public float speedMultiplier = 1.7f;

        public float damageCoefficient = LunarDragonStaticValues.utilityBurstThrustersLowerDamageCoefficient;

        public GameObject startEffect;

        public bool shouldFireTrail;

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

        private int originalLayer;

        private DamageTrailDynamic damageTrail;

        private float previousTurnSpeed;

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

        private void CreateDamageTrail() {
            if (!damageTrail && characterBody) {
                damageTrail = UnityEngine.Object.Instantiate(LunarDragonAssets.fireTrailPrefab, characterBody.transform).GetComponent<DamageTrailDynamic>();
                damageTrail.transform.position = characterBody.corePosition;
                damageTrail.owner = characterBody.gameObject;
                damageTrail.dpsCoefficient = 6f;
                //damageTrailNetID = damageTrail.GetComponent<NetworkIdentity>();
                //NetworkServer.Spawn(damageTrail.gameObject);
            }
        }

        private void StartThrustersDash() {
            if (characterDirection) {
                previousTurnSpeed = characterDirection.turnSpeed;
                characterDirection.turnSpeed = turnSpeed;
            }

            controller.bodyState.ForceJetsOn(JetDirection.Front);

            if (shouldFireTrail) {
                CreateDamageTrail();
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
                hitEffectPrefab = hitEffectPrefab,
                forceVector = Vector3.up * upwardForceMagnitude,
                pushAwayForce = knockbackForce,
                hitBoxGroup = hitBoxGroup,
                isCrit = RollCrit(),
                damageType = new DamageTypeCombo(DamageType.Stun1s, DamageTypeExtended.Generic, DamageSource.Utility),
                retriggerTimeout = 0.5f
            };
        }

        private void ExitThrustersDash() {
            if (characterDirection) {
                characterDirection.turnSpeed = previousTurnSpeed;
            }

            if (damageTrail) {
                damageTrail.active = false;
            }

            if (modelAnimator) {
                modelAnimator.SetBool("inUtilityLoop", false);
                if (isGrounded) {
                    PlayAnimation("Cannons, Override", "FlipCannons");
                }
            }

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
