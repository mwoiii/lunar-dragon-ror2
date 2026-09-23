using RoR2;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonUtils {
        public static void ApplyAirborneKnockback(CharacterMotor motor, Vector3 aimDirection, float force, float bonusDownMult = 1.5f) {
            if (!motor || motor.isGrounded) {
                return;
            }

            Vector3 forceVector = -aimDirection * force;
            if (aimDirection.y < 0f) {
                forceVector.y *= bonusDownMult;
            }

            motor.ApplyForce(forceVector);
        }
    }
}
