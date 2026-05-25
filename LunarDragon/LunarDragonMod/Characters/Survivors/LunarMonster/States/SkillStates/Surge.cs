using LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates.BaseStates;
using LunarDragonMod.Survivors.LunarDragon;
using LunarDragonMod.Survivors.LunarDragon.Components;
using UnityEngine;

namespace LunarDragonMod.Characters.Survivors.LunarMonster.States.SkillStates {

    public class Surge : SecondaryBase {

        protected override GameObject projectilePrefab => LunarDragonAssets.heavyPlasmaballPrefab;

        protected override float damageCoefficient => LunarDragonStaticValues.secondaryPlasmaBlastDamageCoefficient;

        // protected override float baseDuration => 0.7f;

        protected override string attackSoundString => "";

        // protected override float attackSoundPitch => 1f;

        protected override string muzzleString => "MuzzleCenter";

        protected override string animationLayerName => "FullBody, Additive";

        protected override string animationStateName => "PrimaryShoot3";

        protected override bool canLeaveStateAfterFire => false;

        // protected override float recoilAmplitude => 0f;

        private ElecSecondaryController elecSecondaryController;

        private int searchingCountdown = -1;

        private bool hasFired;

        public override void OnEnter() {
            base.OnEnter();
        }

        protected override void FireBlitzProjectile() {
            base.FireBlitzProjectile();
            hasFired = true;
            searchingCountdown = 5;
        }

        public override void Update() {
            base.Update();
            if (hasFired) {
                if (searchingCountdown > 0 && elecSecondaryController == null) {
                    elecSecondaryController = GetComponent<LunarDragonController>()?.elecSecondaryController;
                    if (elecSecondaryController == null) {
                        searchingCountdown--;
                    } else {
                        searchingCountdown = -1;
                    }
                } else if (elecSecondaryController != null) {
                    if (IsNewKeyDownAuthority) {
                        elecSecondaryController.Detonate();
                    }
                } else {
                    SkillBlocker nextState = new SkillBlocker();
                    outer.SetNextState(nextState);
                }
            }
        }
    }
}