using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class JetsOnFrontTrailMedium : JetsOnFrontTrailBase {

        protected override Color trailColour => new Color(1f, 0.48f, 0f);

        protected override float trailTime => 0.6f;

    }
}
