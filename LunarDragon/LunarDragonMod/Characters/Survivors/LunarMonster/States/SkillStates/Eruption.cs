using LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates.BaseStates;
using LunarDragonMod.Survivors.LunarDragon;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class Eruption : SecondaryBase {

        protected override GameObject projectilePrefab => LunarDragonAssets.heavyFireballPrefab;

        protected override float damageCoefficient => LunarDragonStaticValues.secondaryFireBlastDamageCoefficient;

        // protected override float baseDuration => 0.7f;

        protected override string attackSoundString => "";

        // protected override float attackSoundPitch => 1f;

        protected override string muzzleString => "MuzzleLeft";

        protected override string animationLayerName => "Gesture1, Additive";

        protected override string animationStateName => "PrimaryShoot1";

        // protected override float recoilAmplitude => 0f;

    }
}