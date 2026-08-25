using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushDescent : BaseState {

        private float stopwatch;

        private float lifetime = 0.75f;

        public CharacterModel characterModel;

        public HurtBoxGroup hurtBoxGroup;

        public Vector3 targetFootPosition;

        public override void OnEnter() {
            base.OnEnter();
            OnLand();
        }

        public override void OnExit() {
            base.OnExit();
            if (isAuthority && characterMotor) {
                characterMotor.useGravity = true;
            }
        }

        public override void Update() {
            base.Update();
            stopwatch += Time.deltaTime;
            if (stopwatch >= lifetime) {
                PlayAnimation("OuterCannons, Override", "FlipCannons");
                if (isAuthority) {
                    if (TryGetComponent(out LunarDragonController controller)) {
                        controller.ResetAllSkillStateMachines();
                    }
                    outer.SetNextStateToMain();
                }
            }
        }

        private void OnLand() {
            PlayAnimation("FullBody, Override", "SpecialDiveEnd");
            if (isAuthority) {
                FireExplosion();
            }
            if (characterModel) {
                characterModel.invisibilityCount--;
            }
            if (hurtBoxGroup) {
                hurtBoxGroup.hurtBoxesDeactivatorCounter--;
            }
        }

        private void FireExplosion() {
            BlastAttack blastAttack = new BlastAttack {
                attacker = gameObject,
                baseDamage = characterBody.damage * LunarDragonStaticValues.specialAscentDamageCoefficient,
                crit = characterBody.RollCrit(),
                position = characterBody.transform.position,
                falloffModel = BlastAttack.FalloffModel.Linear,
                inflictor = gameObject,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1f,
                radius = 40f,
                teamIndex = characterBody.teamComponent.teamIndex,
            };
            blastAttack.damageType |= DamageType.IgniteOnHit;
            blastAttack.Fire();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}