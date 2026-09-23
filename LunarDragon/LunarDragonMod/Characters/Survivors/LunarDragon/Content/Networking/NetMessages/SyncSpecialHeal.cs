using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace LunarDragonMod.Survivors.LunarDragon.NetMessages {
    public class SyncSpecialHeal : INetMessage {

        NetworkInstanceId bodyNetId;
        float amount;

        public SyncSpecialHeal() {
        }

        public SyncSpecialHeal(NetworkInstanceId bodyNetId, float amount) {
            this.bodyNetId = bodyNetId;
            this.amount = amount;
        }

        public void Serialize(NetworkWriter writer) {
            writer.Write(bodyNetId);
            writer.Write(amount);
        }

        public void Deserialize(NetworkReader reader) {
            bodyNetId = reader.ReadNetworkId();
            amount = reader.ReadSingle();
        }

        public void OnReceived() {
            GameObject bodyObject = Util.FindNetworkObject(bodyNetId);
            if (bodyObject && bodyObject.TryGetComponent(out HealthComponent healthComponent)) {
                healthComponent.Heal(amount, default);
            }
        }
    }
}
