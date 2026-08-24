using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class JetsOnFrontTrailLight : JetsOnFrontTrailBase {

        protected override Color trailColour => new Color(1f, 0.17f, 0f);

        protected override float trailTime => 0.4f;

    }
}
