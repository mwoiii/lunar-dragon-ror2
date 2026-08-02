using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States {

    public class JetsOnFrontTrailHeavy : JetsOnFrontTrailBase {

        protected override Color trailColour => new Color(1f, 0.55f, 0.22f);

        protected override float trailTime => 1.2f;

    }
}
