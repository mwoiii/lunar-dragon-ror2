using EntityStates;
using LunarDragonMod.Survivors.LunarDragon.Components;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class DracoAmbushAimConfirm : BaseSkillState {

        public override void OnEnter() {
            base.OnEnter();
            if (isAuthority) {
                if (TryGetComponent(out LunarDragonController controller) && controller.aimStateMachine.state is DracoAmbushAim aimState) {
                    aimState.TryActivateNextState();
                }
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Stun;
        }
    }
}
