using EntityStates;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class SpawnState : SpawnTeleporterState {

        public override void OnEnter() {
            base.OnEnter();
            modelLocator.normalizeToFloor = true;
        }
    }
}
