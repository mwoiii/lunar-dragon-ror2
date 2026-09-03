using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace LunarDragonMod.Survivors.LunarDragon {
    public static class LunarDragonAI {
        public static void Init(GameObject bodyPrefab, string masterName) {
            GameObject master = Modules.Prefabs.CreateBlankMasterPrefab(bodyPrefab, masterName);

            BaseAI baseAI = master.GetComponent<BaseAI>();
            baseAI.aimVectorDampTime = 0.1f;
            baseAI.aimVectorMaxSpeed = 360;


            AISkillDriver utilityDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            utilityDriver.customName = "Use Utility";
            utilityDriver.skillSlot = SkillSlot.Utility;
            utilityDriver.requireSkillReady = true;
            utilityDriver.minDistance = 5;
            utilityDriver.maxDistance = 200;
            utilityDriver.selectionRequiresTargetLoS = true;
            utilityDriver.selectionRequiresOnGround = false;
            utilityDriver.selectionRequiresAimTarget = false;
            utilityDriver.maxTimesSelected = -1;

            //Behavior
            utilityDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilityDriver.activationRequiresTargetLoS = false;
            utilityDriver.activationRequiresAimTargetLoS = false;
            utilityDriver.activationRequiresAimConfirmation = false;
            utilityDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            utilityDriver.moveInputScale = 1;
            utilityDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            utilityDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver secondaryDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            secondaryDriver.customName = "Use Secondary";
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.minDistance = 0;
            secondaryDriver.maxDistance = 60;
            secondaryDriver.selectionRequiresTargetLoS = false;
            secondaryDriver.selectionRequiresOnGround = false;
            secondaryDriver.selectionRequiresAimTarget = false;
            secondaryDriver.maxTimesSelected = -1;

            //Behavior
            secondaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondaryDriver.activationRequiresTargetLoS = false;
            secondaryDriver.activationRequiresAimTargetLoS = false;
            secondaryDriver.activationRequiresAimConfirmation = true;
            secondaryDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            secondaryDriver.moveInputScale = 1;
            secondaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            secondaryDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            AISkillDriver specialDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            specialDriver.customName = "Use Special";
            specialDriver.skillSlot = SkillSlot.Special;
            specialDriver.requireSkillReady = true;
            specialDriver.minDistance = 0;
            specialDriver.maxDistance = 100;
            specialDriver.selectionRequiresTargetLoS = false;
            specialDriver.selectionRequiresOnGround = false;
            specialDriver.selectionRequiresAimTarget = false;
            specialDriver.maxTimesSelected = -1;

            //Behavior
            specialDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            specialDriver.activationRequiresTargetLoS = false;
            specialDriver.activationRequiresAimTargetLoS = false;
            specialDriver.activationRequiresAimConfirmation = false;
            specialDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            specialDriver.moveInputScale = 1;
            specialDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            specialDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            AISkillDriver primaryDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            primaryDriver.customName = "Use Primary";
            primaryDriver.skillSlot = SkillSlot.Primary;
            primaryDriver.requiredSkill = null; //usually used when you have skills that override other skillslots like engi harpoons
            primaryDriver.requireSkillReady = false; //usually false for primaries
            primaryDriver.requireEquipmentReady = false;
            primaryDriver.minUserHealthFraction = float.NegativeInfinity;
            primaryDriver.maxUserHealthFraction = float.PositiveInfinity;
            primaryDriver.minTargetHealthFraction = float.NegativeInfinity;
            primaryDriver.maxTargetHealthFraction = float.PositiveInfinity;
            primaryDriver.minDistance = 0;
            primaryDriver.maxDistance = 200;
            primaryDriver.selectionRequiresTargetLoS = false;
            primaryDriver.selectionRequiresOnGround = false;
            primaryDriver.selectionRequiresAimTarget = false;
            primaryDriver.maxTimesSelected = -1;

            //Behavior
            primaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryDriver.activationRequiresTargetLoS = false;
            primaryDriver.activationRequiresAimTargetLoS = false;
            primaryDriver.activationRequiresAimConfirmation = false;
            primaryDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primaryDriver.moveInputScale = 1;
            primaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            primaryDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            primaryDriver.shouldSprint = false;
            primaryDriver.shouldFireEquipment = false;
            primaryDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //Transition Behavior
            primaryDriver.driverUpdateTimerOverride = -1;
            primaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            primaryDriver.noRepeat = false;
            primaryDriver.nextHighPriorityOverride = null;




            AISkillDriver chaseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.minDistance = 0;
            chaseDriver.maxDistance = float.PositiveInfinity;

            //Behavior
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.activationRequiresAimTargetLoS = false;
            chaseDriver.activationRequiresAimConfirmation = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //recommend taking these for a spin in game, messing with them in runtimeinspector to get a feel for what they should do at certain ranges and such
        }
    }
}
