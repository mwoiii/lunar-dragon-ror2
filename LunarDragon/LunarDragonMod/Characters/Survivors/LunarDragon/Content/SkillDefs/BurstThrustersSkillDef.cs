using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon.SkillDefs {

    [CreateAssetMenu(fileName = "BurstThrustersSkillDef", menuName = "Lunar Dragon/SkillDefs/BurstThrustersSkillDef")]
    public class BurstThrustersSkillDef : SkillDef {

        public Queue<float> cooldownQueue = new Queue<float>();

        private int prevStock;

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime) {
            base.OnFixedUpdate(skillSlot, deltaTime);
            skillSlot.RecalculateFinalRechargeInterval();

            // preventing cooldown from changing until OnSkillCooldown has run
            // so that eclipse lite scales properly
            // does mean that the ui is wrong for one physics update. but this is an ok compromise
            int currentStock = skillSlot.stock;
            if (prevStock < currentStock) {
                for (int i = 0; i < currentStock - prevStock; i++) {
                    cooldownQueue.TryDequeue(out _);
                }
            }
            prevStock = currentStock;
        }

        public override float GetRechargeInterval([NotNull] GenericSkill skillSlot) {
            // ideally it'd come from the back of the queue
            // but I don't care to use a deque
            // scenario is niche enough anyway
            while (cooldownQueue.Count > skillSlot.maxStock) {
                cooldownQueue.TryDequeue(out _);
            }

            float rechargeInterval = baseRechargeInterval;
            if (cooldownQueue.Count > 0) {
                rechargeInterval = cooldownQueue.Peek();
            }

            if (attackSpeedBuffsRestockSpeed) {
                float num = skillSlot.characterBody.attackSpeed - skillSlot.characterBody.baseAttackSpeed;
                num *= attackSpeedBuffsRestockSpeed_Multiplier;
                num += 1f;
                if (num < 0.5f) {
                    num = 0.5f;
                }
                return rechargeInterval / num;
            }
            return rechargeInterval;
        }
    }
}
