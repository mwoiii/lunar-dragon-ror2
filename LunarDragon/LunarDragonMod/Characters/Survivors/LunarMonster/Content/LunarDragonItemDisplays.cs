using LunarDragonMod.Modules;
using LunarDragonMod.Modules.Characters;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

/* for custom copy format in keb's helper
		            {childName},
                    {localPos}, 
                    {localAngles},
                    {localScale}
*/

namespace LunarDragonMod.Survivors.LunarDragon {
    public class LunarDragonItemDisplays : ItemDisplaysBase {
        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules) {
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AlienHead"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAlienHead"),
                    "CannonL1",
                    new Vector3(0.82792F, 3.10470F, 0.03930F),
                    new Vector3(316.88250F, 287.34330F, 164.40480F),
                    new Vector3(2.40778F, 2.40778F, 2.40778F))
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorPlate"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                    "ShoulderFR",
                    new Vector3(-0.34584F, 0.32750F, -0.02358F),
                    new Vector3(300.60430F, 88.41927F, 193.22100F),
                    new Vector3(1.02704F, 1.09778F, 1.09778F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ArmorReductionOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarhammer"),
                    "JawLower",
                    new Vector3(-0.26724F, 0.30916F, 0.56068F),
                    new Vector3(0.00000F, 0.00000F, 0.00000F),
                    new Vector3(0.44802F, 0.44802F, 0.44802F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedAndMoveSpeed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCoffee"),
                    "FootRFront",
                    new Vector3(-0.13100F, 0.24104F, 0.20698F),
                    new Vector3(61.89130F, 346.12800F, 274.51650F),
                    new Vector3(0.45064F, 0.45064F, 0.45064F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                    "Head",
                    new Vector3(-0.42706F, 0.16506F, -0.04978F),
                    new Vector3(275.21820F, 340.24960F, 110.48220F),
                    new Vector3(1.66370F, 1.66370F, 1.66370F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AutoCastEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFossil"),
                    "CannonM",
                    new Vector3(-0.59998F, 1.46458F, -0.22008F),
                    new Vector3(34.88758F, 287.75320F, 0.01251F),
                    new Vector3(1.53794F, 1.53794F, 1.53794F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Bandolier"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBandolier"),
                    "Stomach",
                    new Vector3(-0.26200F, 0.52924F, -0.04716F),
                    new Vector3(61.25806F, 86.95718F, 258.57220F),
                    new Vector3(1.98072F, 2.54664F, 2.23224F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrooch"),
                    "CannonR2",
                    new Vector3(-0.75718F, 0.83054F, 0.11266F),
                    new Vector3(4.88236F, 68.83440F, 103.58880F),
                    new Vector3(1.53794F, 1.53794F, 1.53794F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnOverHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAegis"),
                    "CannonL2",
                    new Vector3(-0.53448F, 0.75718F, 0.24628F),
                    new Vector3(2.57513F, 247.94160F, 78.05787F),
                    new Vector3(0.58164F, 0.58164F, 0.58164F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Bear"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBear"),
                    "Tail1",
                    new Vector3(-0.06550F, 0.50042F, 0.61832F),
                    new Vector3(339.19490F, 343.31320F, 185.24050F),
                    new Vector3(0.58164F, 0.58164F, 0.58164F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BearVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBearVoid"),
                    "Tail1",
                    new Vector3(-0.06550F, 0.50042F, 0.61832F),
                    new Vector3(353.37330F, 350.11930F, 182.76030F),
                    new Vector3(0.58164F, 0.58164F, 0.58164F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BeetleGland"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                    "CannonR1",
                    new Vector3(0.04978F, 0.87770F, -0.19650F),
                    new Vector3(0.35301F, 26.97074F, 51.45611F),
                    new Vector3(0.18864F, 0.18864F, 0.18864F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Behemoth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBehemoth"),
                    "CannonM",
                    new Vector3(-1.31524F, 1.62178F, -0.03930F),
                    new Vector3(17.14912F, 264.85210F, 357.80300F),
                    new Vector3(0.24890F, 0.24890F, 0.24890F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTip"),
                    "Chest",
                    new Vector3(-1.24450F, -0.15982F, -0.64976F),
                    new Vector3(348.08230F, 58.84056F, 356.44040F),
                    new Vector3(0.68644F, 0.68644F, 0.68644F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitAndExplode"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                    "CannonR2",
                    new Vector3(0.31964F, 0.09694F, -0.27772F),
                    new Vector3(0.16112F, 0.12192F, 53.17397F),
                    new Vector3(0.19388F, 0.19388F, 0.19388F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BleedOnHitVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
                    "Chest",
                    new Vector3(-1.54580F, -0.20960F, -0.89604F),
                    new Vector3(351.28290F, 52.10210F, 358.26870F),
                    new Vector3(0.68644F, 0.68644F, 0.68644F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BonusGoldPackOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTome"),
                    "Stomach",
                    new Vector3(-0.43230F, -0.00786F, 0.54496F),
                    new Vector3(1.79905F, 345.14040F, 87.14014F),
                    new Vector3(0.17554F, 0.17554F, 0.17554F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAPRound"),
                    "CannonL2",
                    new Vector3(-0.12314F, 0.86460F, -0.39562F),
                    new Vector3(74.82944F, 39.17511F, 16.24889F),
                    new Vector3(1.87854F, 1.87854F, 1.87854F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BounceNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHook"),
                    "CannonM",
                    new Vector3(-0.67858F, 0.24628F, 0.00786F),
                    new Vector3(276.45010F, 105.03430F, 341.86510F),
                    new Vector3(1.39646F, 1.39646F, 1.39646F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ChainLightning"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkulele"),
                    "CannonM",
                    new Vector3(-0.21484F, 0.41658F, 0.57902F),
                    new Vector3(0.30762F, 337.55280F, 61.48467F),
                    new Vector3(1.87854F, 1.87854F, 1.87854F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ChainLightningVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
                    "CannonM",
                    new Vector3(-0.21484F, 0.41658F, 0.57902F),
                    new Vector3(0.30762F, 337.55280F, 61.48467F),
                    new Vector3(1.87854F, 1.87854F, 1.87854F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Clover"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayClover"),
                    "CannonR2",
                    new Vector3(-1.17114F, 1.02180F, 0.00524F),
                    new Vector3(7.74383F, 358.73860F, 64.37773F),
                    new Vector3(1.87854F, 1.87854F, 1.87854F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CloverVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCloverVoid"),
                    "CannonR2",
                    new Vector3(-1.17114F, 1.02180F, 0.00524F),
                    new Vector3(7.74383F, 358.73860F, 64.37773F),
                    new Vector3(1.87854F, 1.87854F, 1.87854F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CooldownOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkull"),
                    "LowerLegBL",
                    new Vector3(-0.04716F, 0.03930F, 0.07336F),
                    new Vector3(75.69096F, 312.58460F, 157.87410F),
                    new Vector3(0.72836F, 0.89080F, 0.80172F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritDamage"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserSight"),
                    "CannonM",
                    new Vector3(-0.64452F, 1.51960F, 0.07336F),
                    new Vector3(8.67031F, 245.59830F, 267.83950F),
                    new Vector3(0.39562F, 0.39562F, 0.39562F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritGlasses"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlasses"),
                    "JawUpper",
                    new Vector3(-0.20960F, 0.47160F, 0.00000F),
                    new Vector3(296.71230F, 277.82540F, 173.25690F),
                    new Vector3(0.56592F, 0.56592F, 0.51090F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritGlassesVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
                    "JawUpper",
                    new Vector3(-0.20960F, 0.47160F, 0.00000F),
                    new Vector3(296.71230F, 277.82540F, 173.25690F),
                    new Vector3(0.56592F, 0.56592F, 0.51090F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Crowbar"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCrowbar"),
                    "UpperLegFL",
                    new Vector3(-0.21484F, 1.02966F, -0.05240F),
                    new Vector3(331.09450F, 120.60270F, 257.65210F),
                    new Vector3(0.74146F, 0.74146F, 0.74146F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Dagger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDagger"),
                    "CannonL1",
                    new Vector3(0.10218F, 2.04098F, -0.16506F),
                    new Vector3(348.26370F, 256.50580F, 315.24050F),
                    new Vector3(1.87330F, -1.87330F, 1.87330F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DeathMark"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathMark"),
                    "LowerLegBR",
                    new Vector3(-0.26724F, -0.06026F, -0.05502F),
                    new Vector3(67.35581F, 8.77935F, 300.37530F),
                    new Vector3(0.05240F, 0.05240F, 0.05240F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ElementalRingVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVoidRing"),
                    "Tail3",
                    new Vector3(0.02096F, 0.20698F, 0.15720F),
                    new Vector3(74.68880F, 175.65840F, 160.00180F),
                    new Vector3(1.55628F, 1.55628F, 1.55628F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSun"],
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Head",
                    new Vector3(-0.06026F, 0.13886F, 0.07860F),
                    new Vector3(359.20100F, 223.17240F, 187.41200F),
                    new Vector3(-4.50116F, -4.50116F, -4.50116F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Head",
                    new Vector3(-0.14672F, 0.50304F, 0.02882F),
                    new Vector3(0.00000F, 0.00000F, 0.00000F),
                    new Vector3(-1.76588F, -1.76588F, -1.76588F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EnergizedOnEquipmentUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarHorn"),
                    "Tail1",
                    new Vector3(0.52662F, 0.40610F, 0.45326F),
                    new Vector3(353.89850F, 58.72745F, 2.95061F),
                    new Vector3(1.08468F, 1.08468F, 1.08468F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EquipmentMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBattery"),
                    "CannonL1",
                    new Vector3(-0.05240F, 0.55544F, -0.15196F),
                    new Vector3(62.71756F, 26.30354F, 323.92300F),
                    new Vector3(0.30654F, 0.30654F, 0.30654F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EquipmentMagazineVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
                    "CannonL1",
                    new Vector3(-0.05240F, 0.55544F, -0.15196F),
                    new Vector3(62.71756F, 26.30354F, 323.92300F),
                    new Vector3(0.30654F, 0.30654F, 0.30654F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExecuteLowHealthElite"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGuillotine"),
                    "CannonL1",
                    new Vector3(-0.01310F, 1.54842F, 0.21746F),
                    new Vector3(359.24780F, 242.77100F, 80.86945F),
                    new Vector3(0.29606F, 0.29606F, 0.29606F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExplodeOnDeath"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                    "CannonR1",
                    new Vector3(0.03930F, 0.77552F, 0.20698F),
                    new Vector3(359.69130F, 330.58680F, 88.44695F),
                    new Vector3(0.10218F, 0.10218F, 0.10218F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExplodeOnDeathVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
                    "CannonR1",
                    new Vector3(0.03930F, 0.77552F, 0.20698F),
                    new Vector3(359.69130F, 330.58680F, 88.44695F),
                    new Vector3(0.10218F, 0.10218F, 0.10218F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraLife"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippo"),
                    "Head",
                    new Vector3(-0.57902F, 0.13624F, -0.01834F),
                    new Vector3(323.50770F, 272.70490F, 174.45730F),
                    new Vector3(0.51614F, 0.51614F, 0.51614F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraLifeVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippoVoid"),
                    "Head",
                    new Vector3(-0.57902F, 0.13624F, -0.01834F),
                    new Vector3(323.50770F, 272.70490F, 174.45730F),
                    new Vector3(0.51614F, 0.51614F, 0.51614F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FallBoots"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "LowerLegFR",
                    new Vector3(0.05240F, 0.47946F, -0.00262F),
                    new Vector3(15.72228F, 133.51440F, 184.64210F),
                    new Vector3(1.04800F, 1.04800F, 1.04800F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "LowerLegFL",
                    new Vector3(-0.11528F, 0.52400F, -0.04716F),
                    new Vector3(359.48060F, 188.13820F, 166.59090F),
                    new Vector3(1.04800F, 1.04800F, 1.04800F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "LowerLegBL",
                    new Vector3(0.00262F, 0.55020F, -0.03144F),
                    new Vector3(359.52250F, 38.73606F, 168.15050F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "LowerLegBR",
                    new Vector3(0.00262F, 0.42182F, -0.11266F),
                    new Vector3(30.70843F, 139.94600F, 192.64920F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Feather"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFeather"),
                    "Chest",
                    new Vector3(-0.96940F, -0.63928F, -0.05764F),
                    new Vector3(62.46412F, 99.55613F, 187.34500F),
                    new Vector3(0.05764F, 0.05764F, 0.05764F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireballsOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                    "CannonL1",
                    new Vector3(0.05240F, -0.01834F, 0.00000F),
                    new Vector3(89.23128F, 156.90260F, 247.02690F),
                    new Vector3(0.13362F, 0.13362F, 0.13362F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireRing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireRing"),
                    "CannonL1",
                    new Vector3(0.15720F, 1.82614F, -0.02882F),
                    new Vector3(271.18050F, 80.53051F, 270.34100F),
                    new Vector3(1.66632F, 1.66632F, 1.66632F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Firework"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFirework"),
                    "Tail1",
                    new Vector3(-0.58950F, 0.56330F, 0.22532F),
                    new Vector3(4.54549F, 5.76840F, 233.68320F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FlatHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                    "JawLower",
                    new Vector3(-0.31178F, 0.35370F, -0.01310F),
                    new Vector3(16.32465F, 282.02920F, 245.79860F),
                    new Vector3(0.15720F, 0.15720F, 0.15720F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FocusConvergence"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                    "BodyMesh",
                    new Vector3(2.709F, -0.695F, 4.226F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.28975F, 0.28975F, 0.28975F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FragileDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
                    "UpperLegFR",
                    new Vector3(-0.07074F, 0.37204F, -0.02882F),
                    new Vector3(276.69380F, 229.10190F, 242.89530F),
                    new Vector3(1.77636F, 3.62608F, 1.77636F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FreeChest"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
                    "CannonR2",
                    new Vector3(-0.60522F, 0.74932F, -0.19388F),
                    new Vector3(346.34300F, 114.48860F, 257.97950F),
                    new Vector3(1.39384F, 1.39384F, 1.39384F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GhostOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMask"),
                    "Head",
                    new Vector3(-0.33274F, 0.64976F, -0.00786F),
                    new Vector3(320.34380F, 270.21910F, 178.27220F),
                    new Vector3(0.54758F, 0.54758F, 0.54758F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                    "Neck",
                    new Vector3(-0.40086F, 0.07336F, 0.00000F),
                    new Vector3(287.30500F, 110.83960F, 341.44230F),
                    new Vector3(2.79292F, 2.79292F, 2.30560F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldOnHurt"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
                    "LowerLegFR",
                    new Vector3(-0.35370F, 0.26200F, -0.12314F),
                    new Vector3(8.99135F, 245.18530F, 272.47060F),
                    new Vector3(1.03490F, 1.03490F, 1.03490F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HalfAttackSpeedHalfCooldowns"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
                    "ShoulderFL",
                    new Vector3(-0.35894F, 0.30130F, 0.10480F),
                    new Vector3(8.21673F, 199.43620F, 246.47390F),
                    new Vector3(1.85496F, 1.85496F, 1.85496F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HalfSpeedDoubleHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
                    "ShoulderBR",
                    new Vector3(-0.35370F, 0.21746F, -0.04192F),
                    new Vector3(352.62200F, 169.84310F, 232.28870F),
                    new Vector3(1.32310F, 1.32310F, 1.32310F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HeadHunter"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkullcrown"),
                    "CannonR1",
                    new Vector3(0.08908F, 1.19472F, 0.01048F),
                    new Vector3(4.77789F, 261.80470F, 0.75820F),
                    new Vector3(1.02704F, 0.35632F, 0.46898F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealingPotion"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHealingPotion"),
                    "CannonL1",
                    new Vector3(0.52138F, 2.35276F, 0.49518F),
                    new Vector3(354.44390F, 341.47520F, 74.05693F),
                    new Vector3(0.10480F, 0.10480F, 0.10480F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealOnCrit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScythe"),
                    "CannonM",
                    new Vector3(0.09694F, 0.58688F, -0.60260F),
                    new Vector3(357.97890F, 117.79320F, 97.02751F),
                    new Vector3(0.60784F, 0.60784F, 0.60784F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealWhileSafe"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySnail"),
                    "Chest",
                    new Vector3(-1.16066F, -0.34322F, -0.21484F),
                    new Vector3(342.05660F, 128.66910F, 259.30240F),
                    new Vector3(0.17030F, 0.17030F, 0.17030F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Hoof"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHoof"),
                    "LowerLegBR",
                    new Vector3(0.09956F, 0.45064F, 0.05240F),
                    new Vector3(68.93609F, 294.44850F, 162.88850F),
                    new Vector3(0.19912F, 0.20174F, 0.17292F)
                    ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IceRing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIceRing"),
                    "CannonR1",
                    new Vector3(0.15720F, 1.82614F, -0.02882F),
                    new Vector3(271.18050F, 80.53051F, 270.34100F),
                    new Vector3(1.66632F, 1.66632F, 1.66632F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Icicle"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                    "BodyMesh",
                    new Vector3(-3.296F, -0.525F, 4.183F),
                    new Vector3(16.60143F, 342.1269F, 131.5411F),
                    new Vector3(3.79752F, 3.79752F, 3.79752F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IgniteOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasoline"),
                    "CannonL1",
                    new Vector3(0.09694F, 0.83840F, 0.19126F),
                    new Vector3(0.17455F, 241.78090F, 173.49000F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ImmuneToDebuff"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
                    "Tail2",
                    new Vector3(-0.04716F, 0.45326F, 0.18078F),
                    new Vector3(345.85190F, 183.56190F, 176.65270F),
                    new Vector3(2.20342F, 1.92570F, 2.75362F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreaseHealing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "CannonM",
                    new Vector3(-0.24628F, 1.76588F, 0.20436F),
                    new Vector3(22.67947F, 1.26200F, 70.31944F),
                    new Vector3(1.17900F, 1.17900F, 1.17900F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "CannonM",
                    new Vector3(-0.11266F, 1.67680F, -0.22794F),
                    new Vector3(356.66190F, 183.33130F, 281.10730F),
                    new Vector3(-1.17900F, 1.17900F, 1.17900F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Incubator"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
                    "CannonR2",
                    new Vector3(0.63142F, 1.13970F, -0.50828F),
                    new Vector3(359.33030F, 359.86400F, 275.13760F),
                    new Vector3(0.05764F, 0.05764F, 0.05764F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Infusion"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInfusion"),
                    "CannonR1",
                    new Vector3(0.01310F, 0.51090F, -0.12838F),
                    new Vector3(6.51738F, 201.55870F, 270.93780F),
                    new Vector3(1.07944F, 1.07944F, 1.11350F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["JumpBoost"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaxBird"),
                    "Tail1",
                    new Vector3(0.03930F, -0.30392F, -0.09170F),
                    new Vector3(281.65570F, 204.95820F, 332.27300F),
                    new Vector3(1.51174F, 1.51174F, 1.51174F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["KillEliteFrenzy"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                    "Head",
                    new Vector3(-0.88294F, 0.35894F, -0.01834F),
                    new Vector3(7.13956F, 1.18677F, 268.91760F),
                    new Vector3(0.65762F, 0.65762F, 0.38252F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Knurl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKnurl"),
                    "CannonR2",
                    new Vector3(0.51876F, -0.38252F, 0.21746F),
                    new Vector3(35.67677F, 3.67156F, 153.61390F),
                    new Vector3(0.14672F, 0.14672F, 0.14672F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LaserTurbine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                    "CannonR2",
                    new Vector3(-0.01048F, 0.60522F, 0.42968F),
                    new Vector3(10.23441F, 343.62240F, 359.40980F),
                    new Vector3(1.31524F, 1.31524F, 1.31524F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LightningStrikeOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
                    "CannonR1",
                    new Vector3(0.05240F, -0.01834F, 0.00000F),
                    new Vector3(5.55781F, 269.50890F, 0.85269F),
                    new Vector3(1.92308F, 1.92308F, 1.92308F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarDagger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                    "CannonL1",
                    new Vector3(-0.11528F, 0.93272F, -0.07860F),
                    new Vector3(68.35986F, 308.61880F, 285.33610F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarPrimaryReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdEye"),
                    "Head",
                    new Vector3(-0.09956F, 0.43492F, 0.00000F),
                    new Vector3(1.28788F, 353.12680F, 189.96730F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSecondaryReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdClaw"),
                    "CannonR2",
                    new Vector3(-0.05764F, 0.91438F, -0.48208F),
                    new Vector3(13.81396F, 196.96570F, 283.33720F),
                    new Vector3(1.87330F, 1.87330F, 1.87330F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarSpecialReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdHeart"),
                    "BodyMesh",
                    new Vector3(2.305F, 1.019F, 3.66F),
                    new Vector3(1.13576F, 12.43992F, 201.2514F),
                    new Vector3(0.52817F, 0.52817F, 0.52817F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarTrinket"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeads"),
                    "FootLFront",
                    new Vector3(0.06026F, 0.36418F, 0.04716F),
                    new Vector3(319.53530F, 344.07040F, 294.08110F),
                    new Vector3(3.85664F, 4.43566F, 4.28632F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarUtilityReplacement"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                    "CannonL2",
                    new Vector3(0.54758F, 1.38336F, -0.48994F),
                    new Vector3(0.58266F, 185.79010F, 318.26010F),
                    new Vector3(1.60344F, 1.60344F, 1.60344F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Medkit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMedkit"),
                    "Chest",
                    new Vector3(0.79386F, 0.39300F, 0.17554F),
                    new Vector3(40.45152F, 333.59780F, 73.38712F),
                    new Vector3(1.15542F, 1.15542F, 1.15542F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MinorConstructOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
                    "BodyMesh",
                    new Vector3(-2.648F, 1.176F, 3.533F),
                    new Vector3(86.66463F, 180F, 180F),
                    new Vector3(0.91457F, 0.91457F, 0.91457F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Missile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                    "CannonM",
                    new Vector3(-1.05586F, 0.95630F, -0.94582F),
                    new Vector3(279.08110F, 273.23810F, 122.26550F),
                    new Vector3(0.25676F, 0.25676F, 0.25676F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MissileVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
                    "CannonM",
                    new Vector3(-1.05586F, 0.95630F, -0.94582F),
                    new Vector3(279.08110F, 273.23810F, 122.26550F),
                    new Vector3(0.25676F, 0.25676F, 0.25676F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MonstersOnShrineUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                    "Tail5",
                    new Vector3(0.02096F, 0.29082F, 0.27510F),
                    new Vector3(319.35130F, 106.70370F, 346.61710F),
                    new Vector3(0.09170F, 0.09170F, 0.09170F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MoreMissile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayICBM"),
                    "CannonM",
                    new Vector3(-0.19912F, 1.07682F, -0.54758F),
                    new Vector3(13.74205F, 327.34320F, 6.21417F),
                    new Vector3(0.27248F, 0.27248F, 0.27248F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MoveSpeedOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrappleHook"),
                    "CannonL2",
                    new Vector3(0.33012F, 1.22092F, 0.48470F),
                    new Vector3(283.97560F, 166.46090F, 266.93890F),
                    new Vector3(0.47160F, 0.47160F, 0.47160F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Mushroom"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroom"),
                    "Tail5",
                    new Vector3(0.00524F, 0.72836F, 0.15720F),
                    new Vector3(0.00000F, 0.00000F, 0.00000F),
                    new Vector3(0.13624F, 0.13624F, 0.13624F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MushroomVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
                    "Tail5",
                    new Vector3(0.00524F, 0.72836F, 0.15720F),
                    new Vector3(0.00000F, 0.00000F, 0.00000F),
                    new Vector3(0.13624F, 0.13624F, 0.13624F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NearbyDamageBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDiamond"),
                    "CannonM",
                    new Vector3(-0.97988F, 2.09600F, -0.00786F),
                    new Vector3(333.34770F, 87.73821F, 317.38460F),
                    new Vector3(0.13100F, 0.13100F, 0.13100F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "CannonL2",
                    new Vector3(-0.60260F, 0.95368F, 0.17030F),
                    new Vector3(357.56020F, 250.04680F, 15.19442F),
                    new Vector3(1.57200F, 1.57200F, 1.57200F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "CannonR2",
                    new Vector3(-0.59998F, 0.88032F, -0.09694F),
                    new Vector3(2.52490F, 283.23070F, 333.98180F),
                    new Vector3(-1.57200F, 1.57200F, 1.57200F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["NovaOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                    "CannonL1",
                    new Vector3(-0.00786F, 1.25760F, 0.15982F),
                    new Vector3(328.38060F, 104.99700F, 358.88420F),
                    new Vector3(0.39038F, 0.39038F, 0.39038F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["OutOfCombatArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
                    "CannonL2",
                    new Vector3(-0.73360F, 0.90652F, -0.16768F),
                    new Vector3(355.94480F, 19.20143F, 78.05345F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ParentEgg"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayParentEgg"),
                    "CannonL1",
                    new Vector3(0.25938F, 0.61308F, 0.00262F),
                    new Vector3(1.27060F, 86.67559F, 1.38419F),
                    new Vector3(0.06026F, 0.06026F, 0.06026F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Pearl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPearl"),
                    "BodyMesh",
                    new Vector3(0F, -1.704F, 4.411F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.00399F, 0.00399F, 0.00399F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PermanentDebuffOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScorpion"),
                    "CannonL2",
                    new Vector3(-1.21830F, 1.08730F, -0.00262F),
                    new Vector3(1.86174F, 92.22080F, 2.64290F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PersonalShield"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                    "Chest",
                    new Vector3(-1.12660F, -0.69692F, 0.02096F),
                    new Vector3(337.48430F, 314.83800F, 346.21950F),
                    new Vector3(0.42444F, 0.42444F, 0.42444F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Phasing"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStealthkit"),
                    "ShoulderBL",
                    new Vector3(-0.11528F, 0.46112F, 0.24104F),
                    new Vector3(302.32570F, 5.99898F, 295.09340F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Plant"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                    "Tail1",
                    new Vector3(0.19388F, 0.11266F, 0.74146F),
                    new Vector3(339.47840F, 10.29468F, 39.97167F),
                    new Vector3(0.11004F, 0.11004F, 0.11004F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PrimarySkillShuriken"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShuriken"),
                    "LowerLegFR",
                    new Vector3(-0.33012F, -0.27248F, -0.17030F),
                    new Vector3(335.55210F, 64.18762F, 350.73180F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomDamageZone"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                    "Chest",
                    new Vector3(-1.48816F, -0.58164F, 0.06288F),
                    new Vector3(13.24056F, 91.40051F, 4.70540F),
                    new Vector3(0.14672F, 0.14672F, 0.14672F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomEquipmentTrigger"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBottledChaos"),
                    "CannonL2",
                    new Vector3(0.58950F, -0.13624F, 0.52662F),
                    new Vector3(7.21557F, 333.58610F, 81.84316F),
                    new Vector3(0.31964F, 0.31964F, 0.31964F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RandomlyLunar"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDomino"),
                    "BodyMesh",
                    new Vector3(2.01F, 3.169F, 3.378F),
                    new Vector3(0.00001F, -0.00002F, 129.148F),
                    new Vector3(3.15331F, 3.15331F, 3.15331F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RegeneratingScrap"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
                    "CannonL2",
                    new Vector3(0.03144F, 0.16506F, 0.62094F),
                    new Vector3(9.76024F, 343.56590F, 88.28815F),
                    new Vector3(0.34060F, 0.34060F, 0.34060F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["RepeatHeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCorpseflower"),
                    "CannonM",
                    new Vector3(0.10742F, 1.58772F, 0.47160F),
                    new Vector3(82.76582F, 3.02234F, 23.77436F),
                    new Vector3(0.62356F, 0.62356F, 0.62356F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SecondarySkillMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                    "CannonR2",
                    new Vector3(0.68644F, 0.02358F, 0.01310F),
                    new Vector3(292.98650F, 255.66760F, 13.85610F),
                    new Vector3(0.28558F, 0.28558F, 0.28558F)
                    ),
                 ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                    "CannonL2",
                    new Vector3(0.68644F, 0.02358F, 0.01310F),
                    new Vector3(292.98650F, 255.66760F, 13.85610F),
                    new Vector3(0.28558F, 0.28558F, 0.28558F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Seed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySeed"),
                    "LowerLegFR",
                    new Vector3(-0.28820F, 0.26986F, 0.30654F),
                    new Vector3(0.29025F, 282.56230F, 46.82766F),
                    new Vector3(0.08908F, 0.08908F, 0.08908F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShieldOnly"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(-0.38776F, 0.34584F, -0.08384F),
                    new Vector3(344.04460F, 346.87450F, 103.41230F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(-0.40086F, 0.31702F, 0.01310F),
                    new Vector3(18.95777F, 13.81494F, 107.22600F),
                    new Vector3(0.78600F, 0.78600F, -0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShinyPearl"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                    "BodyMesh",
                    new Vector3(0F, -1.752F, 4.848F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.00386F, 0.00386F, 0.00386F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShockNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                    "Neck",
                    new Vector3(-0.58164F, 0.17816F, 0.00000F),
                    new Vector3(0.00000F, 0.00000F, 88.48837F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SiphonOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                    "CannonR2",
                    new Vector3(0.61308F, 1.29952F, 0.53448F),
                    new Vector3(278.99360F, 87.35855F, 2.60902F),
                    new Vector3(0.13624F, 0.13624F, 0.13624F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SlowOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBauble"),
                    "CannonR2",
                    new Vector3(1.11874F, 1.49078F, -0.01572F),
                    new Vector3(81.06794F, 85.24843F, 184.17810F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SlowOnHitVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
                    "CannonR2",
                    new Vector3(1.11874F, 1.49078F, -0.01572F),
                    new Vector3(81.06794F, 85.24843F, 184.17810F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBuckler"),
                    "CannonR2",
                    new Vector3(0.07860F, 0.82006F, -0.36942F),
                    new Vector3(6.34373F, 199.52040F, 218.05820F),
                    new Vector3(0.55020F, 0.55020F, 0.55020F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintBonus"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySoda"),
                    "CannonL2",
                    new Vector3(0.50042F, 0.42182F, 0.52400F),
                    new Vector3(353.06500F, 252.44870F, 0.00000F),
                    new Vector3(0.59474F, 0.59736F, 0.59736F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintOutOfCombat"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWhip"),
                    "CannonL2",
                    new Vector3(-0.52662F, -0.06812F, 0.02620F),
                    new Vector3(359.81430F, 70.94402F, 7.52900F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SprintWisp"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                    "Neck",
                    new Vector3(-0.11004F, 0.44016F, 0.44540F),
                    new Vector3(0.00000F, 0.00000F, 121.96540F),
                    new Vector3(0.31178F, 0.31178F, 0.31178F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Squid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                    "Chest",
                    new Vector3(-0.98512F, -0.59212F, -0.42968F),
                    new Vector3(291.40290F, 193.72960F, 204.63200F),
                    new Vector3(0.09956F, 0.09956F, 0.09956F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StickyBomb"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                    "CannonL1",
                    new Vector3(0.32488F, 0.60522F, 0.27510F),
                    new Vector3(4.86035F, 334.55830F, 271.67240F),
                    new Vector3(0.50828F, 0.50828F, 0.50828F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StrengthenBurn"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasTank"),
                    "CannonL1",
                    new Vector3(-0.07336F, 0.45064F, 0.08122F),
                    new Vector3(342.04610F, 340.88900F, 79.25803F),
                    new Vector3(0.28558F, 0.28558F, 0.23318F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StunChanceOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                    "CannonR2",
                    new Vector3(-0.49256F, 0.19126F, -0.14148F),
                    new Vector3(25.40139F, 296.82410F, 0.00001F),
                    new Vector3(2.17198F, 2.17198F, 2.17198F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Syringe"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                    "CannonM",
                    new Vector3(-0.0917F, 1.18162F, 0.39824F),
                    new Vector3(70.68916F, 39.55191F, 67.02256F),
                    new Vector3(0.31301F, 0.31301F, 0.31301F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Talisman"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTalisman"),
                    "BodyMesh",
                    new Vector3(-2.575F, -3.235F, 3.879F),
                    new Vector3(89.98022F, 0F, 0F),
                    new Vector3(2.60422F, 2.60422F, 2.60422F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Thorns"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
                    "UpperLegBL",
                    new Vector3(-0.02882F, -0.15982F, 0.17030F),
                    new Vector3(297.64790F, 196.30780F, 169.95460F),
                    new Vector3(1.67942F, 1.59820F, 0.70740F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TitanGoldDuringTP"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                    "CannonR1",
                    new Vector3(0.33536F, 0.81482F, 0.00000F),
                    new Vector3(0.00000F, 104.20620F, 0.00000F),
                    new Vector3(0.45588F, 0.45588F, 0.45588F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tooth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                    "Neck",
                    new Vector3(0.52924F, 0.51352F, 0.02096F),
                    new Vector3(294.85430F, 259.54250F, 189.52590F),
                    new Vector3(10.18918F, 10.18918F, 10.18918F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Neck",
                    new Vector3(0.48470F, 0.47684F, -0.20960F),
                    new Vector3(352.61090F, 12.55494F, 353.95230F),
                    new Vector3(6.37708F, 6.37708F, 6.37708F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Neck",
                    new Vector3(0.36680F, 0.42444F, -0.30916F),
                    new Vector3(345.23800F, 36.65842F, 355.50120F),
                    new Vector3(3.60250F, 3.60250F, 3.60250F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Neck",
                    new Vector3(0.35894F, 0.41134F, 0.40086F),
                    new Vector3(13.21370F, 327.17710F, 355.72220F),
                    new Vector3(3.60250F, 3.60250F, 3.60250F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Neck",
                    new Vector3(0.48470F, 0.47160F, 0.26986F),
                    new Vector3(7.86193F, 341.12950F, 358.11770F),
                    new Vector3(6.37708F, 6.37708F, 6.37708F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TPHealingNova"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                    "CannonR2",
                    new Vector3(-0.69430F, 1.12922F, 0.16768F),
                    new Vector3(296.29660F, 271.36030F, 82.22460F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TreasureCache"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKey"),
                    "CannonL2",
                    new Vector3(0.62880F, 1.21568F, -0.19126F),
                    new Vector3(359.88830F, 18.77502F, 181.60390F),
                    new Vector3(2.72742F, 2.72742F, 2.72742F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TreasureCacheVoid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKeyVoid"),
                    "CannonL2",
                    new Vector3(0.62880F, 1.21568F, -0.19126F),
                    new Vector3(359.88830F, 18.77502F, 181.60390F),
                    new Vector3(2.72742F, 2.72742F, 2.72742F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["UtilitySkillMagazine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "CannonR2",
                    new Vector3(-0.11266F, 0.76504F, -0.00524F),
                    new Vector3(359.01660F, 359.36860F, 80.49159F),
                    new Vector3(2.33704F, 2.33704F, 2.33704F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "CannonL2",
                    new Vector3(-0.11266F, 0.76504F, -0.00524F),
                    new Vector3(359.01660F, 359.36860F, 80.49159F),
                    new Vector3(2.33704F, 2.33704F, 2.33704F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["VoidMegaCrabItem"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
                    "CannonM",
                    new Vector3(0.22270F, 1.11088F, 0.73884F),
                    new Vector3(333.88600F, 335.37970F, 333.77180F),
                    new Vector3(0.28034F, 0.28034F, 0.28034F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WarCryOnMultiKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPauldron"),
                    "CannonM",
                    new Vector3(0.59736F, 1.65846F, 0.60260F),
                    new Vector3(338.90910F, 278.23040F, 281.40400F),
                    new Vector3(1.10302F, 1.10302F, 1.10302F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WardOnLevel"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarbanner"),
                    "Tail3",
                    new Vector3(-0.05240F, 0.90652F, 0.56068F),
                    new Vector3(48.16163F, 187.74010F, 102.27350F),
                    new Vector3(0.57902F, 0.57902F, 0.57902F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BFG"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBFG"),
                    "CannonL2",
                    new Vector3(-0.04716F, 0.80696F, 0.19912F),
                    new Vector3(281.67650F, 202.77060F, 313.87590F),
                    new Vector3(1.04014F, 1.04014F, 1.08992F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Blackhole"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravCube"),
                    "BodyMesh",
                    new Vector3(1.596F, -3.302F, 4.176F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.53954F, 1.53954F, 1.53954F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunter"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornGhost"),
                    "Head",
                    new Vector3(-0.55544F, 0.30130F, 0.00786F),
                    new Vector3(299.18870F, 93.28080F, 353.20240F),
                    new Vector3(1.48816F, 1.48816F, 1.48816F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
                    "BodyMesh",
                    new Vector3(1.864F, -3.263F, 3.579F),
                    new Vector3(358.1156F, 180F, 180F),
                    new Vector3(2.14851F, 2.14851F, 2.14851F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BossHunterConsumed"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornUsed"),
                    "Head",
                    new Vector3(-0.55544F, 0.30130F, 0.00786F),
                    new Vector3(299.18870F, 93.28080F, 353.20240F),
                    new Vector3(1.48816F, 1.48816F, 1.48816F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BurnNearby"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPotion"),
                    "CannonL2",
                    new Vector3(0.01572F, -0.13100F, 0.28820F),
                    new Vector3(1.19055F, 345.80430F, 234.49910F),
                    new Vector3(-0.07074F, -0.07074F, 0.05764F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Cleanse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaterPack"),
                    "Tail3",
                    new Vector3(-0.02882F, 0.26200F, 0.75456F),
                    new Vector3(15.61748F, 179.86630F, 7.61687F),
                    new Vector3(0.22794F, 0.22794F, 0.22794F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CommandMissile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileRack"),
                    "Stomach",
                    new Vector3(-0.82268F, 0.05240F, 0.02096F),
                    new Vector3(60.92501F, 92.29205F, 180.00000F),
                    new Vector3(1.23140F, 1.23140F, 1.23140F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CrippleWard"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEffigy"),
                    "FootLFront",
                    new Vector3(-0.25414F, 0.27510F, -0.09170F),
                    new Vector3(340.56120F, 28.73879F, 257.67500F),
                    new Vector3(0.95106F, 0.95106F, 0.95106F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritOnUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                    "Head",
                    new Vector3(-0.41920F, 1.34930F, 0.02358F),
                    new Vector3(277.71480F, 278.87240F, 168.41870F),
                    new Vector3(1.28118F, 1.28118F, 1.28118F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DeathProjectile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                    "FootRFront",
                    new Vector3(-0.34584F, 0.16768F, -0.13100F),
                    new Vector3(17.94687F, 247.13930F, 246.86680F),
                    new Vector3(0.15982F, 0.15982F, 0.15982F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DroneBackup"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRadio"),
                    "CannonR1",
                    new Vector3(0.12314F, 1.70562F, -0.29344F),
                    new Vector3(7.36770F, 196.10690F, 307.87550F),
                    new Vector3(1.05062F, 1.05062F, 1.05062F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteEarthEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
                    "Head",
                    new Vector3(-0.35108F, 0.26724F, -0.00786F),
                    new Vector3(283.01520F, 99.35869F, 349.30760F),
                    new Vector3(1.02966F, 1.02966F, 1.02966F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteFireEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Head",
                    new Vector3(-0.38776F, 0.42182F, -0.09956F),
                    new Vector3(3.43290F, 352.09560F, 116.47050F),
                    new Vector3(-0.17816F, 0.16768F, 0.16768F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Head",
                    new Vector3(-0.41920F, 0.42182F, 0.11790F),
                    new Vector3(1.98497F, 0.57470F, 117.11090F),
                    new Vector3(-0.17816F, 0.16768F, -0.16768F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteHauntedEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                    "Head",
                    new Vector3(-0.68644F, 0.32226F, -0.00262F),
                    new Vector3(349.52360F, 270.09850F, 178.44300F),
                    new Vector3(0.11790F, 0.11790F, 0.11790F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteIceEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                    "Head",
                    new Vector3(-0.68644F, 0.32226F, -0.00262F),
                    new Vector3(349.50000F, 270.00000F, 178.44000F),
                    new Vector3(0.05240F, 0.05240F, 0.05240F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLightningEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Head",
                    new Vector3(-0.39300F, 0.53448F, 0.00262F),
                    new Vector3(340.14960F, 267.44950F, 178.39060F),
                    new Vector3(0.51876F, 0.51876F, 0.51876F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Head",
                    new Vector3(-0.43754F, 0.41134F, -0.00262F),
                    new Vector3(340.00000F, 267.00000F, 178.39000F),
                    new Vector3(0.29868F, 0.29868F, 0.29868F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteLunarEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteLunar,Eye"),
                    "Head",
                    new Vector3(-0.19912F, 0.22794F, -0.02620F),
                    new Vector3(89.15836F, 180.00000F, 180.00000F),
                    new Vector3(0.87770F, 0.87770F, 0.87770F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ElitePoisonEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                    "Head",
                    new Vector3(-0.39824F, 0.19126F, 0.01048F),
                    new Vector3(0.00000F, 270.00000F, 0.00000F),
                    new Vector3(0.05764F, 0.10218F, 0.14148F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteVoidEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAffixVoid"),
                    "Head",
                    new Vector3(-0.09694F, 0.49256F, -0.00262F),
                    new Vector3(342.23880F, 94.14795F, 358.24450F),
                    new Vector3(0.19912F, 0.19912F, 0.19912F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["FireBallDash"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEgg"),
                    "CannonL2",
                    new Vector3(-0.39038F, 0.47422F, -0.17554F),
                    new Vector3(0.00000F, 290.28020F, 0.00000F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Fruit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFruit"),
                    "CannonR2",
                    new Vector3(-0.14672F, 0.18602F, -0.37990F),
                    new Vector3(311.69130F, 358.65700F, 180.00000F),
                    new Vector3(0.39562F, 0.39562F, 0.39562F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GainArmor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                    "Tail1",
                    new Vector3(0.42706F, 0.70478F, 0.49780F),
                    new Vector3(75.91399F, 57.11669F, 11.81289F),
                    new Vector3(1.07682F, 1.07682F, 1.07682F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Gateway"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVase"),
                    "CannonR1",
                    new Vector3(-0.50828F, 1.78160F, -0.04978F),
                    new Vector3(359.66310F, 16.92542F, 97.58683F),
                    new Vector3(0.58688F, 0.58688F, 0.58688F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GoldGat"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldGat"),
                    "CannonL2",
                    new Vector3(-0.07336F, 0.51352F, 1.04800F),
                    new Vector3(4.27607F, 259.73530F, 238.08990F),
                    new Vector3(0.39824F, 0.39824F, 0.39824F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["GummyClone"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGummyClone"),
                    "LowerLegFL",
                    new Vector3(-0.38252F, -0.00524F, 0.18602F),
                    new Vector3(352.04500F, 110.25200F, 102.72270F),
                    new Vector3(0.45850F, 0.45850F, 0.45850F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IrradiatingLaser"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
                    "CannonL2",
                    new Vector3(-0.20436F, 0.89604F, 0.29868F),
                    new Vector3(278.59230F, 238.19610F, 280.16380F),
                    new Vector3(0.48732F, 0.48732F, 0.48732F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Jetpack"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBugWings"),
                    "Chest",
                    new Vector3(-0.63928F, -0.54496F, -0.05764F),
                    new Vector3(301.46830F, 93.77295F, 2.57802F),
                    new Vector3(0.41658F, 0.41658F, 0.41658F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LifestealOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                    "Chest",
                    new Vector3(-0.84102F, -0.07336F, -0.75980F),
                    new Vector3(357.41740F, 14.34857F, 357.15810F),
                    new Vector3(0.22008F, 0.22008F, 0.22008F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Lightning"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
                    "ShoulderFR",
                    new Vector3(-0.41920F, 1.46720F, 0.13624F),
                    new Vector3(302.26500F, 50.39008F, 237.48360F),
                    new Vector3(2.78506F, 2.78506F, 2.78506F)
                    ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LunarPortalOnUse"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
                    "BodyMesh",
                    new Vector3(1.901F, -3.363F, 4.001F),
                    new Vector3(70.50439F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Meteor"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMeteor"),
                    "BodyMesh",
                    new Vector3(1.773F, -3.411F, 3.807F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.63545F, 1.63545F, 1.63545F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Molotov"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMolotov"),
                    "CannonL1",
                    new Vector3(-0.13624F, 1.21306F, -0.12838F),
                    new Vector3(327.98580F, 16.95079F, 91.76473F),
                    new Vector3(0.46636F, 0.46636F, 0.46636F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MultiShopCard"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
                    "Neck",
                    new Vector3(-0.22008F, 0.49780F, -0.36942F),
                    new Vector3(294.22830F, 87.33830F, 282.63640F),
                    new Vector3(0.86198F, 0.86198F, 0.86198F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["QuestVolatileBattery"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                    "Chest",
                    new Vector3(0.73098F, -0.25152F, -0.08646F),
                    new Vector3(340.71110F, 277.72200F, 354.86150F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Recycle"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRecycler"),
                    "CannonR1",
                    new Vector3(0.12576F, 1.81828F, -0.37204F),
                    new Vector3(283.59490F, 142.78010F, 332.66090F),
                    new Vector3(0.19126F, 0.19126F, 0.19126F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Saw"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
                    "BodyMesh",
                    new Vector3(2.174F, -2.857F, 3.993F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.00303F, 0.00303F, 0.00303F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Scanner"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScanner"),
                    "CannonR2",
                    new Vector3(0.12052F, -0.02358F, -0.01310F),
                    new Vector3(5.45162F, 257.43520F, 263.47600F),
                    new Vector3(0.90652F, 0.90652F, 0.90652F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TeamWarCry"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                    "Tail4",
                    new Vector3(-0.03930F, -0.02096F, 0.63666F),
                    new Vector3(316.13260F, 349.40940F, 183.97430F),
                    new Vector3(0.17292F, 0.17292F, 0.17292F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Tonic"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTonic"),
                    "CannonL2",
                    new Vector3(-0.29082F, 0.18602F, 0.20174F),
                    new Vector3(8.24342F, 330.94960F, 89.20451F),
                    new Vector3(0.45588F, 0.45588F, 0.45588F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["VendingMachine"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVendingMachine"),
                    "CannonR2",
                    new Vector3(-0.71002F, 0.30654F, 0.09170F),
                    new Vector3(348.21010F, 208.65190F, 260.88730F),
                    new Vector3(0.37990F, 0.37990F, 0.37990F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteAurelioniteEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteAurelioniteEquipment"),
                    "Head",
                    new Vector3(-0.52138F, 0.50304F, -0.00262F),
                    new Vector3(270.44720F, 270.19960F, 180.00040F),
                    new Vector3(0.79910F, 0.79910F, 0.79910F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteBeadEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteBeadSpike"),
                    "Head",
                    new Vector3(-0.46112F, 0.06812F, 0.07860F),
                    new Vector3(350.17320F, 10.66442F, 79.18069F),
                    new Vector3(0.04978F, 0.04978F, 0.04454F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["AttackSpeedPerNearbyAllyOrEnemy"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRageCrystal"),
                    "CannonR2",
                    new Vector3(0.63666F, 1.18948F, 0.00000F),
                    new Vector3(273.77550F, 332.47240F, 117.79380F),
                    new Vector3(2.26106F, 2.26106F, 2.26106F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrageOnBoss"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTreasuryDividends"),
                    "CannonL1",
                    new Vector3(-0.02358F, 2.01216F, -0.28034F),
                    new Vector3(354.89130F, 28.95214F, 43.96020F),
                    new Vector3(3.39028F, 3.39028F, 3.39028F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BoostAllStats"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrowthNectar"),
                    "CannonL2",
                    new Vector3(0.04978F, 0.92224F, -0.02358F),
                    new Vector3(89.97202F, 28.53281F, 0.00000F),
                    new Vector3(1.09254F, 1.09254F, 1.09254F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrowthNectar"),
                    "CannonR2",
                    new Vector3(0.04978F, 0.92224F, 0.00000F),
                    new Vector3(90.00000F, 150.66650F, 0.00000F),
                    new Vector3(1.09254F, 1.09254F, 1.09254F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DelayedDamage"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDelayedDamage"),
                    "CannonL2",
                    new Vector3(-0.50304F, 0.44016F, -0.14672F),
                    new Vector3(11.66458F, 201.67170F, 315.51040F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraShrineItem"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayChanceDoll"),
                    "Tail1",
                    new Vector3(0.42706F, 0.67596F, 0.40872F),
                    new Vector3(347.85410F, 57.77008F, 103.76120F),
                    new Vector3(0.59998F, 0.59998F, 0.59998F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraStatsOnLevelUp"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPrayerBeads"),
                    "Neck",
                    new Vector3(-0.23580F, 0.58164F, 0.00262F),
                    new Vector3(328.13050F, 92.57101F, 5.72211F),
                    new Vector3(4.34134F, -1.06634F, 4.34134F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreaseDamageOnMultiKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIncreaseDamageOnMultiKill"),
                    "Tail3",
                    new Vector3(-0.02882F, 0.59736F, 0.50828F),
                    new Vector3(56.84046F, 356.04570F, 171.70520F),
                    new Vector3(0.46898F, 0.46898F, 0.46898F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["IncreasePrimaryDamage"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIncreasePrimaryDamage"),
                    "CannonM",
                    new Vector3(0.27510F, 1.83662F, 0.02358F),
                    new Vector3(273.19640F, 295.71040F, 62.28433F),
                    new Vector3(1.05586F, 1.26546F, 1.05586F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ItemDropChanceOnKill"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySonorousEcho"),
                    "Chest",
                    new Vector3(-0.23056F, 1.26284F, -0.55282F),
                    new Vector3(322.47250F, 165.61390F, 285.80830F),
                    new Vector3(0.80696F, 0.80696F, 0.80696F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["KnockBackHitEnemies"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKnockbackFin"),
                    "Tail4",
                    new Vector3(-0.03930F, 0.15196F, 0.40348F),
                    new Vector3(14.97208F, 180.65250F, 184.17770F),
                    new Vector3(0.96416F, 0.96416F, 0.96416F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["LowerPricedChests"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLowerPricedChests"),
                    "BodyMesh",
                    new Vector3(-1.395F, -1.796F, 5.595F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(2.65439F, 3.29392F, 2.65439F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MeteorAttackOnHighDamage"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMeteorAttackOnHighDamage"),
                    "CannonR2",
                    new Vector3(-0.73884F, 0.29344F, 0.12314F),
                    new Vector3(301.57960F, 338.83820F, 136.93950F),
                    new Vector3(1.93094F, 1.93094F, 1.93094F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["OnLevelUpFreeUnlock"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOnLevelUpFreeUnlockTablet"),
                    "Tail3",
                    new Vector3(0.02358F, 0.15720F, -0.31178F),
                    new Vector3(7.88656F, 349.61400F, 20.78131F),
                    new Vector3(2.92392F, 2.92392F, 2.92392F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOnLevelUpFreeUnlock"),
                    "BodyMesh",
                    new Vector3(-2.052F, 2.942F, 3.168F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(3.83951F, 3.83951F, 3.83951F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SpeedBoostPickup"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElusiveAntlersLeft"),
                    "Head",
                    new Vector3(-0.39038F, 0.09170F, 0.05240F),
                    new Vector3(340.24690F, 91.76913F, 346.17350F),
                    new Vector3(1.10826F, 1.10826F, 1.10826F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElusiveAntlersRight"),
                    "Head",
                    new Vector3(-0.38776F, 0.14934F, -0.06288F),
                    new Vector3(338.39470F, 90.15759F, 5.57984F),
                    new Vector3(1.10826F, 1.10826F, 1.10826F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["StunAndPierce"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElectricBoomerang"),
                    "CannonM",
                    new Vector3(0.07074F, 1.48030F, -0.52138F),
                    new Vector3(278.39780F, 352.09990F, 31.21556F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TeleportOnLowHealth"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeleportOnLowHealth"),
                    "CannonR2",
                    new Vector3(-0.07860F, 0.03668F, -0.26986F),
                    new Vector3(9.54000F, 207.89460F, 267.24960F),
                    new Vector3(2.31608F, 2.31608F, 2.06456F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["TriggerEnemyDebuffs"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayNoxiousThorn"),
                    "Tail5",
                    new Vector3(0.03144F, 0.19126F, 0.15982F),
                    new Vector3(0.22180F, 281.20850F, 31.65718F),
                    new Vector3(1.85234F, 2.42612F, 2.42612F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["HealAndRevive"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHealAndRevive"),
                    "CannonR2",
                    new Vector3(0.34846F, -0.28034F, -0.37728F),
                    new Vector3(71.76885F, 296.38760F, 24.36410F),
                    new Vector3(1.64798F, 1.64798F, 1.64798F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BarrierOnCooldown"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBarrierOnCooldown"),
                    "CannonL2",
                    new Vector3(0.76766F, 0.63404F, 0.56592F),
                    new Vector3(0.48402F, 353.04540F, 83.90771F),
                    new Vector3(0.22008F, 0.22008F, 0.22008F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CritAtLowerElevation"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("CritAtLowerElevationDisplay"),
                    "Stomach",
                    new Vector3(0.38252F, 0.59998F, 0.48470F),
                    new Vector3(7.01462F, 11.83633F, 93.31834F),
                    new Vector3(0.35108F, 0.35108F, 0.35108F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["DronesDropDynamite"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DronesDropDynamiteDisplay"),
                    "Stomach",
                    new Vector3(-0.74932F, 0.01572F, 0.02882F),
                    new Vector3(276.69200F, 248.22980F, 202.39590F),
                    new Vector3(0.38776F, 0.38776F, 0.38776F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["JumpDamageStrike"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJumpDamageStrike"),
                    "FootLBack",
                    new Vector3(0.01310F, -0.01572F, 0.00262F),
                    new Vector3(325.25760F, 47.84559F, 182.76510F),
                    new Vector3(3.09684F, 3.09684F, 3.09684F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJumpDamageStrike"),
                    "FootRBack",
                    new Vector3(0.01048F, -0.15720F, 0.00000F),
                    new Vector3(312.59260F, 136.04270F, 180.84020F),
                    new Vector3(3.09684F, 3.09684F, 3.09684F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SpeedOnPickup"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("SpeedOnPickupDisplay"),
                    "Tail4",
                    new Vector3(-0.26724F, 0.01834F, 0.34584F),
                    new Vector3(0.39702F, 272.04760F, 271.96290F),
                    new Vector3(0.41396F, 0.41396F, 0.41396F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PhysicsProjectile"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("PhysicsProjectileDisplay"),
                    "Tail1",
                    new Vector3(-0.43754F, 0.24890F, 0.59736F),
                    new Vector3(1.20883F, 298.93220F, 271.66280F),
                    new Vector3(0.31178F, 0.31178F, 0.31178F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Duplicator"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDuplicator"),
                    "UpperLegFL",
                    new Vector3(0.42706F, 0.68120F, 0.68644F),
                    new Vector3(8.64880F, 210.77560F, 185.40430F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["SharedSuffering"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("SharedSufferingDisplay"),
                    "CannonL1",
                    new Vector3(0.21746F, 0.91438F, 0.00786F),
                    new Vector3(84.88052F, 287.75360F, 196.24840F),
                    new Vector3(0.38514F, 0.45850F, 0.45850F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Parry"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("ParryDisplay"),
                    "CannonR2",
                    new Vector3(0.30654F, -0.15982F, 0.37990F),
                    new Vector3(4.56943F, 331.99740F, 98.52043F),
                    new Vector3(3.40338F, 3.40338F, 3.40338F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ExtraEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayExtraEquipment"),
                    "Tail4",
                    new Vector3(0.33536F, 0.00000F, -0.36680F),
                    new Vector3(0.00000F, 14.56803F, 0.00000F),
                    new Vector3(0.78600F, 0.78600F, 0.78600F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["ShockDamageAura"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("ShockDamageAuraDisplay"),
                    "CannonM",
                    new Vector3(-0.41396F, 0.60522F, 0.34322F),
                    new Vector3(1.22085F, 332.34530F, 244.62400F),
                    new Vector3(0.57640F, 0.57640F, 0.57640F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PowerPyramid"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPowerPyramid"),
                    "BodyMesh",
                    new Vector3(1.618F, -1.899F, 4.594F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.3051F, 1.3051F, 1.3051F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["PowerCube"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPowerCube"),
                    "BodyMesh",
                    new Vector3(0.913F, -1.721F, 4.039F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(2.86463F, 2.86463F, 2.86463F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["MasterBattery"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPowerOrbSphere"),
                    "BodyMesh",
                    new Vector3(1.81294F, -1.72476F, 3.72166F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.47239F, 1.47239F, 1.47239F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["BonusHealthBoost"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayQuickFix"),
                    "CannonR1",
                    new Vector3(0.00524F, 0.32226F, 0.05502F),
                    new Vector3(282.70600F, 273.21460F, 239.26390F),
                    new Vector3(0.42182F, 0.42182F, 0.42182F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["CookedSteak"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCookedSteakFlat"),
                    "JawLower",
                    new Vector3(-0.28034F, 0.34584F, 0.02358F),
                    new Vector3(0.00002F, 92.44570F, 36.74599F),
                    new Vector3(0.17030F, 0.17030F, 0.17030F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["Stew"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("StewDisplay"),
                    "LowerLegFR",
                    new Vector3(-0.20960F, -0.13100F, 0.19388F),
                    new Vector3(62.73107F, 142.51880F, 163.88690F),
                    new Vector3(0.30654F, 0.30654F, 0.30654F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["UltimateMeal"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("UltimateMealDisplay"),
                    "BodyMesh",
                    new Vector3(0.028F, -1.904F, 4.625F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1.08312F, 1.08312F, 1.08312F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["WyrmOnHit"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWyrmOnHit"),
                    "Neck",
                    new Vector3(-0.25414F, 0.52400F, -0.01310F),
                    new Vector3(359.54970F, 25.29783F, 2.76417F),
                    new Vector3(0.27510F, 0.27510F, 0.27510F)
                    )
                ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(ItemDisplays.KeyAssets["EliteCollectiveEquipment"],
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteCollectiveHorn"),
                    "Head",
                    new Vector3(-0.19912F, 0.12314F, -0.65500F),
                    new Vector3(52.31466F, 0.00000F, 0.00000F),
                    new Vector3(0.73360F, 0.73360F, 0.73360F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteCollectiveHorn"),
                    "Head",
                    new Vector3(-0.27510F, 0.11790F, 0.56068F),
                    new Vector3(307.94250F, 6.47392F, 349.38800F),
                    new Vector3(0.73360F, 0.73360F, -0.73360F)
                    ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteCollectiveRing"),
                    "Head",
                    new Vector3(-0.11266F, 0.48994F, 0.04454F),
                    new Vector3(277.60630F, 270.00000F, 72.52927F),
                    new Vector3(0.59998F, 0.59998F, 0.59998F)
                    )
                ));
        }
    }
}