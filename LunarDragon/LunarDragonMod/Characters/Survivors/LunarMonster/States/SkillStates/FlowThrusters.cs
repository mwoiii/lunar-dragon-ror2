using EntityStates;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;


namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class FlowThrusters : GenericCharacterMain, ISkillState {

        public GenericSkill activatorSkillSlot { get; set; }

        public float duration = 7f;

        public float speed = 18f;

        private CameraTargetParams.AimRequest aimRequest;

        private float hopVelocity = 28f;

        private float hopCountdown = 0.25f;

        private bool thrustersStarted;

        private bool holdingKeyFromStart = true;


        public bool detonateOnCollision = false;

        public GameObject explosionEffectPrefab;

        public float blastDamageCoefficient;

        public float blastRadius;

        public float blastForce;

        public BlastAttack.FalloffModel blastFalloffModel;

        public DamageTypeCombo blastDamageType;

        public Vector3 blastBonusForce;

        public float blastProcCoefficient;

        public string explosionSoundString;


        public float overlapDamageCoefficient;

        public float overlapProcCoefficient;

        public float overlapForce;

        public float overlapFireFrequency;

        public float overlapResetFrequency;

        public float overlapVehicleDurationBonusPerHit;

        public GameObject overlapHitEffectPrefab;

        private bool hasDetonatedServer;

        private OverlapAttack overlapAttack;

        private float overlapFireAge;

        private float overlapResetAge;

        public override void OnEnter() {
            base.OnEnter();
            modelLocator.normalizeToFloor = false;
            if (characterMotor.isGrounded) {
                characterMotor.velocity.y = Mathf.Max(base.characterMotor.velocity.y, hopVelocity);
                characterMotor.Motor.ForceUnground();
            } else {
                StartThrusters();
            }
        }

        public override void ProcessJump() {
        }
        public override void HandleMovements() {
        }

        private void StartThrusters() {
            thrustersStarted = true;

            if ((bool)inputBank) {
                Vector3 aimDirection = inputBank.aimDirection;
                StartAimMode(7f, false);
                // modelLocator.currentNormal = aimDirection;
                characterMotor.velocity = aimDirection * speed;

                if ((bool)cameraTargetParams) {
                    aimRequest = cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
                }

                overlapAttack = new OverlapAttack {
                    attacker = characterBody.gameObject,
                    damage = overlapDamageCoefficient * characterBody.damage,
                    pushAwayForce = overlapForce,
                    isCrit = characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    inflictor = base.gameObject,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = overlapProcCoefficient,
                    teamIndex = characterBody.teamComponent.teamIndex,
                    hitBoxGroup = base.gameObject.GetComponent<HitBoxGroup>(),
                    hitEffectPrefab = overlapHitEffectPrefab
                };

            }
        }

        private void DetonateServer() {
            if (!hasDetonatedServer) {
                hasDetonatedServer = true;
                if ((bool)characterBody) {
                    EffectData effectData = new EffectData {
                        origin = base.transform.position,
                        scale = blastRadius
                    };
                    // EffectManager.SpawnEffect(explosionEffectPrefab, effectData, transmit: true);
                    BlastAttack blastAttack = new BlastAttack();
                    blastAttack.attacker = characterBody.gameObject;
                    blastAttack.baseDamage = blastDamageCoefficient * characterBody.damage;
                    blastAttack.baseForce = blastForce;
                    blastAttack.bonusForce = blastBonusForce;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    blastAttack.crit = characterBody.RollCrit();
                    blastAttack.damageColorIndex = DamageColorIndex.Item;
                    blastAttack.damageType = blastDamageType;
                    blastAttack.falloffModel = blastFalloffModel;
                    blastAttack.inflictor = base.gameObject;
                    blastAttack.position = base.transform.position;
                    blastAttack.procChainMask = default(ProcChainMask);
                    blastAttack.procCoefficient = blastProcCoefficient;
                    blastAttack.radius = blastRadius;
                    blastAttack.teamIndex = characterBody.teamComponent.teamIndex;
                    blastAttack.Fire();
                }
                PointSoundManager.EmitSoundLocal(explosionSoundString, base.gameObject.transform.position);
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!IsKeyDownAuthority() && holdingKeyFromStart) {
                holdingKeyFromStart = false;
            }

            if (thrustersStarted) {
                age += Time.fixedDeltaTime;
                overlapFireAge += Time.fixedDeltaTime;
                overlapResetAge += Time.fixedDeltaTime;
                // if (NetworkServer.active) {
                if (overlapFireAge > 1f / overlapFireFrequency) {
                    if (overlapAttack.Fire()) {
                        age = Mathf.Max(0f, age - overlapVehicleDurationBonusPerHit);
                    }
                    overlapFireAge = 0f;
                }
                if (overlapResetAge >= 1f / overlapResetFrequency) {
                    overlapAttack.ResetIgnoredHealthComponents();
                    overlapResetAge = 0f;
                }
                // }
                Ray aimRay = inputBank.GetAimRay();
                Vector3 velocity = characterMotor.velocity;
                Vector3 target = aimRay.direction * speed;

                characterMotor.velocity = target;
                characterDirection.moveVector = aimRay.direction;
                if (duration <= age || (age > 0.5f && IsKeyDownAuthority() && !holdingKeyFromStart)) {
                    modelLocator.normalizeToFloor = true;
                    aimRequest?.Dispose();
                    outer.SetNextStateToMain();
                    // DetonateServer();
                }
            } else {
                hopCountdown -= Time.fixedDeltaTime;
                if (hopCountdown < 0f) {
                    StartThrusters();
                }
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (detonateOnCollision && NetworkServer.active) {
                DetonateServer();
            }
        }
        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
            this.Serialize(base.skillLocator, writer);
        }

        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
            this.Deserialize(base.skillLocator, reader);
        }

        public bool IsKeyDownAuthority() {
            return this.IsKeyDownAuthority(base.skillLocator, base.inputBank);
        }
    }
}
