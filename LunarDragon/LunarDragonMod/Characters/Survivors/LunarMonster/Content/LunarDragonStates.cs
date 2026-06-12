using LunarDragonMod.Characters.Survivors.LunarMonster.States;
using LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates;
using LunarDragonMod.Survivors.LunarDragon.SkillStates;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonStates {
        public static void Init() {
            Modules.Content.AddEntityState(typeof(ElementalBlitz));

            Modules.Content.AddEntityState(typeof(SkillBlocker));

            Modules.Content.AddEntityState(typeof(BurstThrustersCharge));

            Modules.Content.AddEntityState(typeof(BurstThrustersDash));

            Modules.Content.AddEntityState(typeof(Eruption));

            Modules.Content.AddEntityState(typeof(FlowThrusters));

            Modules.Content.AddEntityState(typeof(Glaciate));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(DeathState));

            Modules.Content.AddEntityState(typeof(FloorNormalizedMain));

            Modules.Content.AddEntityState(typeof(LunarDragonMain));

            Modules.Content.AddEntityState(typeof(JetsOn));

            Modules.Content.AddEntityState(typeof(JetsOff));

            Modules.Content.AddEntityState(typeof(SpawnState));
        }
    }
}
