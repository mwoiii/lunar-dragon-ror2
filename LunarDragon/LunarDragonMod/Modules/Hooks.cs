using LunarDragonMod.Survivors.LunarDragon;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.CharacterSpeech;
using System;
using System.Reflection;

namespace LunarDragonMod.Modules {
    public static class Hooks {
        public delegate void Handle_HealthComponentTakeDamageProcess(HealthComponent self, DamageInfo damageInfo);
        public static Handle_HealthComponentTakeDamageProcess Handle_HealthComponentTakeDamageProcess_Actions;

        public static void AddHooks() {
            if (Handle_HealthComponentTakeDamageProcess_Actions != null) {
                On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
            }
            // These methods could not possibly be any less friendly for modding
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
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            // Oh man
            var displayClass = typeof(BrotherSpeechDriver).GetNestedType("<>c__DisplayClass18_0", allFlags);
            if (displayClass == null) {
                Log.Error("BrotherSpeechDriver NestedType name changed! Aborting AddSeeDragonDialogue hook...");
                return;
            }
            var responsePool = displayClass.GetField("responsePool", allFlags);

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
                    c.TryGotoNext(MoveType.After, x => x.MatchStfld(displayClass, responsePool.Name))) {
                    c.Emit(OpCodes.Ldloca, displayClassLoc);
                    c.Emit(OpCodes.Ldloc, displayClassLoc);
                    c.Emit(OpCodes.Ldfld, responsePool);
                    // I can't be bothered to try call the actual function with il code right now I'm sorry
                    c.EmitDelegate<Func<CharacterSpeechController.SpeechInfo[], CharacterSpeechController.SpeechInfo[]>>((responsePool) => {
                        if (foundDragon && responsePool.Length == 0) {
                            foundDragon = false;
                            return LunarDragonAssets.seeDragonResponses;
                        } else {
                            return responsePool;
                        }
                    });
                    c.Emit(OpCodes.Stfld, responsePool);
                } else {
                    Log.Error("Part 2 of AddSeeDragonDialogue IL hook failed! Custom Mithrix entry dialogue will not work!");
                }
            } else {
                Log.Error("Part 1 of AddSeeDragonDialogue IL hook failed! Custom Mithrix entry dialogue will not work!");
            }
        }

        private static void AddKillDragonDialogue(ILContext il) {
            var c = new ILCursor(il);
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            // Oh geez
            var displayClass = typeof(BrotherSpeechDriver).GetNestedType("<>c__DisplayClass19_0", allFlags);
            if (displayClass == null) {
                Log.Error("BrotherSpeechDriver NestedType name changed! Aborting AddKillDragonDialogue hook...");
                return;
            }
            var responsePool = displayClass.GetField("responsePool", allFlags);

            int displayClassLoc = 0;
            if (c.TryGotoNext(x => x.MatchLdloca(out displayClassLoc)) &&
                c.TryGotoNext(MoveType.After, x => x.MatchStfld(displayClass, responsePool.Name))) {
                c.Emit(OpCodes.Ldloca, displayClassLoc);
                c.Emit(OpCodes.Ldloc, displayClassLoc);
                c.Emit(OpCodes.Ldfld, responsePool);
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                // still can't be bothered
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
                c.Emit(OpCodes.Stfld, responsePool);
            } else {
                Log.Error("AddKillDragonDialogue IL hook failed! Custom Mithrix death dialogue will not work!");
            }
        }
    }
}
