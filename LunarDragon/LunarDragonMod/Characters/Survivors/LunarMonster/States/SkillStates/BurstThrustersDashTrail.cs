using EntityStates;
using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {
    public class BurstThrustersDashTrail : BurstThrustersDash {

        private DamageTrailDynamic damageTrail;

        public override void OnEnter() {
            base.OnEnter();
            CreateDamageTrail();
        }

        public override void OnExit() {
            if (damageTrail) {
                damageTrail.active = false;
            }
            base.OnExit();
        }

        private void CreateDamageTrail() {
            if (!damageTrail && characterBody) {
                damageTrail = UnityEngine.Object.Instantiate(LunarDragonAssets.fireTrailPrefab, characterBody.transform).GetComponent<DamageTrailDynamic>();
                damageTrail.transform.position = characterBody.corePosition;
                damageTrail.owner = characterBody.gameObject;
                damageTrail.dpsCoefficient = 6f;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }

}
