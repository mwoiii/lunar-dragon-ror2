using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

public class DeathState : GenericCharacterDeath {

    public override bool shouldAutoDestroy => false;

    private const float lifetime = 4f;

    public override void OnEnter() {
        base.OnEnter();
        Vector3 force = Vector3.up * 3f;

        if (characterMotor) {
            force += characterMotor.velocity;
            characterMotor.enabled = false;
        }

        if (cachedModelTransform && cachedModelTransform.TryGetComponent(out RagdollController ragdollController)) {
            ragdollController.BeginRagdoll(force);
        }
    }

    public override void PlayDeathAnimation(float crossfadeDuration = 0.1f) {
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        if (NetworkServer.active && fixedAge > lifetime) {
            EntityState.Destroy(gameObject);
        }
    }


    public override InterruptPriority GetMinimumInterruptPriority() {
        return InterruptPriority.Death;
    }

}
