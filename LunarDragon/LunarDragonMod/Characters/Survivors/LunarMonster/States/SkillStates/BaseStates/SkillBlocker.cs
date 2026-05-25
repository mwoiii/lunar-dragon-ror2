using EntityStates;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class SkillBlocker : BaseState {

        public float duration = 0.1f;

        public override void OnExit() {
            base.OnExit();
        }

        public override void Update() {
            base.Update();
            duration -= Time.deltaTime;
            if (duration <= 0f) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Death;
        }
    }
}