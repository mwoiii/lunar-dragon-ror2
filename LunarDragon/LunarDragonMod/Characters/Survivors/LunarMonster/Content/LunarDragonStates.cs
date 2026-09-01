using LunarDragonMod.Survivors.LunarDragon.States;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonStates {
        public static void Init() {
            Modules.Content.AddEntityState(typeof(ElementalBlitz));

            Modules.Content.AddEntityState(typeof(SkillBlocker));

            Modules.Content.AddEntityState(typeof(BurstThrustersCharge));

            Modules.Content.AddEntityState(typeof(BurstThrustersDash));

            Modules.Content.AddEntityState(typeof(BurstThrustersDashTrail));

            Modules.Content.AddEntityState(typeof(Eruption));

            Modules.Content.AddEntityState(typeof(FlowThrusters));

            Modules.Content.AddEntityState(typeof(Glaciate));

            Modules.Content.AddEntityState(typeof(DracoAmbushAim));

            Modules.Content.AddEntityState(typeof(DracoAmbushAscend));

            Modules.Content.AddEntityState(typeof(DracoAmbushRising));

            Modules.Content.AddEntityState(typeof(DracoAmbushDescending));

            Modules.Content.AddEntityState(typeof(DracoAmbushLand));

            Modules.Content.AddEntityState(typeof(AmbushSpawn));

            Modules.Content.AddEntityState(typeof(DeathState));

            Modules.Content.AddEntityState(typeof(FloorNormalizedMain));

            Modules.Content.AddEntityState(typeof(LunarDragonMain));

            Modules.Content.AddEntityState(typeof(JetsOnBase));

            Modules.Content.AddEntityState(typeof(JetsOff));

            Modules.Content.AddEntityState(typeof(JetsOnBottom));

            Modules.Content.AddEntityState(typeof(JetsOnFront));

            Modules.Content.AddEntityState(typeof(JetsOnFrontTrailLight));

            Modules.Content.AddEntityState(typeof(JetsOnFrontTrailMedium));

            Modules.Content.AddEntityState(typeof(JetsOnFrontTrailHeavy));

            Modules.Content.AddEntityState(typeof(SpawnState));
        }
    }
}
