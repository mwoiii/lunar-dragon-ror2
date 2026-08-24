namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class JetsOnFront : JetsOnBase {

        protected override void GetJetEffects() {
            jetLeftEffect = FindModelChild("JetLeftFront");
            jetRightEffect = FindModelChild("JetRightFront");
        }
    }
}
