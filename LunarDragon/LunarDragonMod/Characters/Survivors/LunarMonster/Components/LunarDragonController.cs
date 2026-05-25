using UnityEngine.Networking;

namespace LunarDragonMod.Survivors.LunarDragon.Components {
    internal class LunarDragonController : NetworkBehaviour {

        public ElecSecondaryController elecSecondaryController;

        /*
        [SerializeField]
        public float lowerCamThreshold = 0.25f;

        [SerializeField]
        public float upperCamThreshold = 0.6f;

        [SerializeField]
        public float yOffset = 6f;        

        private static int kCmdCmdSetRolyPolyGearCharge;

        private static int kCmdCmdSetRolyPolyStarted;

        private int _rolyPolyGearCharge;

        [SyncVar]
        private bool _rolyPolyStarted;

        public bool blockOtherSkills;

        public bool rolyPolyActive;

        public int Network_rolyPolyGearCharge {
            get {
                return _rolyPolyGearCharge;
            }
            [param: In]
            set {
                SetSyncVar(value, ref _rolyPolyGearCharge, 4u);
            }
        }

        public bool Network_rolyPolyStarted {
            get {
                return _rolyPolyStarted;
            }
            [param: In]
            set {
                SetSyncVar(value, ref _rolyPolyStarted, 8u);
            }
        }

        public int rolyPolyGearCharge {
            get {
                return _rolyPolyGearCharge;
            }
            set {
                if (base.hasAuthority) {
                    Network_rolyPolyGearCharge = value;
                    CallCmdSetRolyPolyGearCharge(value);
                }
            }
        }

        public bool rolyPolyStarted {
            get {
                return _rolyPolyStarted;
            }
            set {
                if (base.hasAuthority) {
                    Network_rolyPolyStarted = value;
                    CallCmdSetRolyPolyStarted(value);
                }
            }
        }

        public void CallCmdSetRolyPolyGearCharge(int gearCharge) {
            if (!NetworkClient.active) {
                Debug.LogError("Command function CmdSetRolyPolyGearCharge called on server.");
                return;
            }
            if (base.isServer) {
                CmdSetRolyPolyGearCharge(gearCharge);
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write((short)0);
            networkWriter.Write((short)5);
            networkWriter.WritePackedUInt32((uint)kCmdCmdSetRolyPolyGearCharge);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint)gearCharge);
            SendCommandInternal(networkWriter, 0, "CmdSetRolyPolyGearCharge");
        }

        public void CallCmdSetRolyPolyStarted(bool started) {
            if (!NetworkClient.active) {
                Debug.LogError("Command function CmdSetRolyPolyStarted called on server.");
                return;
            }
            if (base.isServer) {
                CmdSetRolyPolyStarted(started);
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write((short)0);
            networkWriter.Write((short)5);
            networkWriter.WritePackedUInt32((uint)kCmdCmdSetRolyPolyStarted);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(started);
            SendCommandInternal(networkWriter, 0, "CmdSetRolyPolyStarted");
        }

        private void CmdSetRolyPolyGearCharge(int gearCharge) {
            Network_rolyPolyGearCharge = gearCharge;
        }

        [Command]
        private void CmdSetRolyPolyStarted(bool started) {
            Network_rolyPolyStarted = started;
        }

        protected static void InvokeCmdCmdSetRolyPolyGearCharge(NetworkBehaviour obj, NetworkReader reader) {
            if (!NetworkServer.active) {
                Debug.LogError("Command CmdSetRolyPolyGearCharge called on client.");
            } else {
                ((LunarDragonController)obj).CmdSetRolyPolyGearCharge((int)reader.ReadPackedUInt32());
            }
        }
        protected static void InvokeCmdCmdSetRolyPolyStarted(NetworkBehaviour obj, NetworkReader reader) {
            if (!NetworkServer.active) {
                Debug.LogError("Command CmdSetRolyPolyStarted called on client.");
            } else {
                ((LunarDragonController)obj).CmdSetRolyPolyStarted(reader.ReadBoolean());
            }
        }
        */
    }
}