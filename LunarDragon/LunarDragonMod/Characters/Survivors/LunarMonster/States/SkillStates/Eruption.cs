using LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates.BaseStates;
using LunarDragonMod.Survivors.LunarDragon;
using RoR2;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class Eruption : SecondaryBase {

        protected override GameObject projectilePrefab => LunarDragonAssets.heavyFireballPrefab;

        protected override float damageCoefficient => LunarDragonStaticValues.secondaryFireBlastDamageCoefficient;

        protected override string attackSoundString => "Play_moonBrother_dash";

        protected override GameObject muzzleflashEffectPrefab => LunarDragonAssets.heavyFireballMuzzlePrefab;

        protected override string muzzleString => "MuzzleLeft";

        protected override string animationLayerName => "LeftCannon, Additive";

        protected override string animationStateName => "SecondaryShoot1";

        private static Wave shakeWave = new Wave() {
            amplitude = 0.5f,
            frequency = 20f
        };

        protected override void FireProjectile() {
            ShakeEmitter.CreateSimpleShakeEmitter(transform.position, shakeWave, 0.3f, 60f, true);
            base.FireProjectile();
        }

    }
}