using LunarDragonMod.Survivors.LunarDragon.NetMessages;
using R2API.Networking;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonNetworking {
        public static void Init() {
            NetworkingAPI.RegisterMessageType<SyncSpecialHeal>();
        }
    }
}
