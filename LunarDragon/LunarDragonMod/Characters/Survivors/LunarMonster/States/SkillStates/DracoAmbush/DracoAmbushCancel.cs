namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class DracoAmbushCancel : DracoAmbushBase {

        public override void OnEnter() {
            base.OnEnter();
            authorityFinished = true;
            RevertStateChanges();
            outer.SetNextStateToMain();
        }
    }
}
