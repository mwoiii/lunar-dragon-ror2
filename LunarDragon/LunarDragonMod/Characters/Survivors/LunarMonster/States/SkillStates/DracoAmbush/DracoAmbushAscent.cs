using EntityStates;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {
    public class DracoAmbushAscent : BaseState {

        private float stopwatch;

        public override void OnEnter() {
            base.OnEnter();
            PlayCrossfade("FullBody, Override", "SpecialDiveStart", 0.005f);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            stopwatch += Time.deltaTime;
            if (isAuthority && stopwatch > 2f) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnExit();
            PlayAnimation("FullBody, Override", "SpecialDiveEnd");
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}