using R2API;
using RoR2;

namespace LunarDragonMod.Modules.DamageTypes {
    public static class Stun3s {
        public static DamageAPI.ModdedDamageType damageType;

        public static void Init() {
            damageType = DamageAPI.ReserveDamageType();
            DamageTypeCollection.damageTypes.Add(damageType);
            Hooks.Handle_HealthComponentTakeDamageProcess_Actions += AddStun3sBuff;
        }

        private static void AddStun3sBuff(HealthComponent self, DamageInfo damageInfo) {
            if (self.body != null && damageInfo.HasModdedDamageType(damageType)) {
                SetStateOnHurt stunComponent = self.GetComponent<SetStateOnHurt>();
                if (stunComponent != null && stunComponent.canBeStunned) {
                    stunComponent.SetStun(3f);
                }
            }
        }
    }
}
