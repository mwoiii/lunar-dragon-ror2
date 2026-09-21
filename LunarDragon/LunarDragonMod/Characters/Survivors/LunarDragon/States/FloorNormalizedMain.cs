namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class FloorNormalizedMain : LunarDragonMain {
        public override void OnEnter() {
            base.OnEnter();
            modelLocator.normalizeToFloor = true;
        }
    }
}
