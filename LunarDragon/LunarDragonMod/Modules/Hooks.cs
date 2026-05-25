namespace LunarDragonMod.Modules {
    public static class Hooks {

        public delegate void Handle_HealthComponentTakeDamageProcess(RoR2.HealthComponent self, RoR2.DamageInfo damageInfo);
        public static Handle_HealthComponentTakeDamageProcess Handle_HealthComponentTakeDamageProcess_Actions;

        public static void AddHooks() {
            if (Handle_HealthComponentTakeDamageProcess_Actions != null) {
                On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
            }
        }

        internal static void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo) {
            Handle_HealthComponentTakeDamageProcess_Actions.Invoke(self, damageInfo);
            orig(self, damageInfo);
        }
    }
}
