using System.Collections.Generic;
using LunarDragonMod.Modules.DamageTypes;
using R2API;

namespace LunarDragonMod.Modules {
    public static class DamageTypeCollection {
        internal static List<DamageAPI.ModdedDamageType> damageTypes = new List<DamageAPI.ModdedDamageType>();

        public static void Init() {
            Stun3s.Init();
        }
    }
}
