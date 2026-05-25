using EntityStates;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    public class SpawnState : SpawnTeleporterState {

        public override void OnEnter() {
            base.OnEnter();
            modelLocator.normalizeToFloor = true;
        }
    }
}
