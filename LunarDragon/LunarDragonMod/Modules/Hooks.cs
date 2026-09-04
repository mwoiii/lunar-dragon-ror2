using LunarDragonMod.Survivors.LunarDragon;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.CharacterSpeech;
using System;

namespace LunarDragonMod.Modules {
    public static class Hooks {
        public delegate void Handle_HealthComponentTakeDamageProcess(HealthComponent self, DamageInfo damageInfo);
        public static Handle_HealthComponentTakeDamageProcess Handle_HealthComponentTakeDamageProcess_Actions;

        public static void AddHooks() {
            if (Handle_HealthComponentTakeDamageProcess_Actions != null) {
                On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
            }
            // maybe someone should make an r2api submodule for this.......            
            IL.RoR2.CharacterSpeech.BrotherSpeechDriver.DoInitialSightResponse += AddSeeDragonDialogue;
            IL.RoR2.CharacterSpeech.BrotherSpeechDriver.OnBodyKill += AddKillDragonDialogue;
        }

        internal static void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo) {
            Handle_HealthComponentTakeDamageProcess_Actions.Invoke(self, damageInfo);
            orig(self, damageInfo);
        }

        private static void AddSeeDragonDialogue(ILContext il) {
            var c = new ILCursor(il);

            FieldReference fieldResponsePool = null;
            bool foundDragon = false;
            int bodyIndexLoc = 0;
            int displayClassLoc = 0;
            if (c.TryGotoNext(x => x.MatchLdfld<CharacterBody>(nameof(CharacterBody.bodyIndex))) &&
                c.TryGotoNext(MoveType.After, x => x.MatchStloc(out bodyIndexLoc))) {
                c.Emit(OpCodes.Ldloc, bodyIndexLoc);
                c.EmitDelegate<Action<BodyIndex>>((bodyIndex) => {
                    foundDragon |= bodyIndex == LunarDragonSurvivor.bodyIndex;
                });
                if (c.TryGotoNext(x => x.MatchBlt(out _)) &&
                    c.TryGotoNext(x => x.MatchLdloca(out displayClassLoc)) &&
                    c.TryGotoNext(MoveType.After, x => x.MatchStfld(out fieldResponsePool))) {
                    c.Emit(OpCodes.Ldloca, displayClassLoc);
                    c.Emit(OpCodes.Ldloc, displayClassLoc);
                    c.Emit(OpCodes.Ldfld, fieldResponsePool);
                    c.Emit(OpCodes.Ldarg_0);
                    c.EmitDelegate<Func<CharacterSpeechController.SpeechInfo[], BrotherSpeechDriver, CharacterSpeechController.SpeechInfo[]>>(
                        (responsePool, speechDriver) => {
                            // Nothing matters
                            if (foundDragon && responsePool.Length == 0 && speechDriver.gameObject.name != "BrotherHurtSpeechController(Clone)") {
                                foundDragon = false;
                                return LunarDragonAssets.seeDragonResponses;
                            } else {
                                return responsePool;
                            }
                        });
                    c.Emit(OpCodes.Stfld, fieldResponsePool);
                } else {
                    Log.Error("Part 2 of AddSeeDragonDialogue IL hook failed! Custom Mithrix entry dialogue will not work!");
                }
            } else {
                Log.Error("Part 1 of AddSeeDragonDialogue IL hook failed! Custom Mithrix entry dialogue will not work!");
            }
        }

        private static void AddKillDragonDialogue(ILContext il) {
            var c = new ILCursor(il);

            FieldReference fieldResponsePool = null;
            int displayClassLoc = 0;
            if (c.TryGotoNext(x => x.MatchLdloca(out displayClassLoc)) &&
                c.TryGotoNext(MoveType.After, x => x.MatchStfld(out fieldResponsePool))) {
                c.Emit(OpCodes.Ldloca, displayClassLoc);
                c.Emit(OpCodes.Ldloc, displayClassLoc);
                c.Emit(OpCodes.Ldfld, fieldResponsePool);
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<CharacterSpeechController.SpeechInfo[], BrotherSpeechDriver, DamageReport, CharacterSpeechController.SpeechInfo[]>>(
                    (responsePool, speechDriver, damageReport) => {
                        if (damageReport.victimBodyIndex == LunarDragonSurvivor.bodyIndex) {
                            // I'm going to end it all
                            if (speechDriver.gameObject.name == "BrotherHurtSpeechController(Clone)") {
                                return LunarDragonAssets.killHurtDragonResponses;
                            } else {
                                return LunarDragonAssets.killDragonResponses;
                            }
                        } else {
                            return responsePool;
                        }
                    });
                c.Emit(OpCodes.Stfld, fieldResponsePool);
            } else {
                Log.Error("AddKillDragonDialogue IL hook failed! Custom Mithrix death dialogue will not work!");
            }
        }
    }
}
