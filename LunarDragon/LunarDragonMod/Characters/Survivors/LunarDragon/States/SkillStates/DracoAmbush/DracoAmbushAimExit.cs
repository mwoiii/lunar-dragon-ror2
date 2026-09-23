using EntityStates;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class DracoAmbushAimExit : BaseSkillState {

        public override void OnEnter() {
            base.OnEnter();
            outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Stun;
        }
    }
}
