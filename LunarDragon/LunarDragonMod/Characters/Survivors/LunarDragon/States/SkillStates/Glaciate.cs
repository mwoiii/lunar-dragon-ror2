using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.States {

    public class Glaciate : SecondaryBase {

        protected override GameObject projectilePrefab => LunarDragonAssets.heavyIceballPrefab;

        protected override float damageCoefficient => LunarDragonStaticValues.secondaryIceBlastDamageCoefficient;

        // protected override float baseDuration => 0.7f;

        protected override string attackSoundString => "";

        // protected override float attackSoundPitch => 1f;

        protected override string muzzleString => "MuzzleRight";

        protected override string animationLayerName => "RightCannon, Additive";

        protected override string animationStateName => "PrimaryShoot2";

        // protected override float recoilAmplitude => 0f;

    }
}