using EntityStates;
using LunarDragonMod;
using RoR2;
using UnityEngine;

public class DeathState : GenericCharacterDeath {
    private Vector3 previousPosition;

    private float upSpeedVelocity;

    private float upSpeed;

    private Animator modelAnimator;

    public override bool shouldAutoDestroy => false;

    public override void OnEnter() {
        base.OnEnter();
        Vector3 force = Vector3.up * 3f;
        if ((bool)base.characterMotor) {
            force += base.characterMotor.velocity;
            base.characterMotor.enabled = false;
        }
        if ((bool)base.cachedModelTransform) {
            RagdollController component = base.cachedModelTransform.GetComponent<RagdollController>();
            if ((bool)component) {
                Log.Info("BEGINNING RAGDOLL!!!!");
                component.BeginRagdoll(force);
                Log.Info($"animator: {(bool)component.animator}");
            }
        }
    }

    public override void PlayDeathAnimation(float crossfadeDuration = 0.1f) {
    }

    /*
    public override void FixedUpdate() {
        base.FixedUpdate();
        if (NetworkServer.active && base.fixedAge > 4f) {
            EntityState.Destroy(base.gameObject);
        }
    }


    public override InterruptPriority GetMinimumInterruptPriority() {
        return InterruptPriority.Death;
    }
    */
}
