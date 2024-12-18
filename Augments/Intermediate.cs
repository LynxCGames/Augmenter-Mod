using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Data.Gameplay.Mods;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using System.Diagnostics;
using Templates;
using AugmentsMod;
using JetBrains.Annotations;
using Il2CppAssets.Scripts.Unity.Towers.Weapons;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.Towers;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using static Templates.AugmentTemplate;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using AugmentSentries;
using Il2CppSteamNative;
using Il2CppAssets.Scripts.Models.Powers;

namespace AugmentsMod.Augments
{
    public class DartsNeverMiss : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Darts Never Miss";
        public override string Icon => VanillaSprites.SharpShotsUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Sharp Shots allow darts to home in on Bloons and gain 0 (+1 per stack) additional pierce.";
        public override void EditTower()
        {
            var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
            seeking.distance = 999;
            seeking.constantlyAquireNewTarget = true;
            seeking.turnRate *= 2;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SharpShots))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "DartsNeverMiss")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(seeking);
                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SpikeOPult)) { }
                                else
                                {
                                    towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3f;
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Enraged : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Enraged";
        public override string Icon => VanillaSprites.VeryQuickShotsUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Very Quick Shots makes Dart Monkeys attack up to 50% faster while they are attacking Bloons and gain 0 (+1 per stack) additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.VeryQuickShots))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Enraged")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(new DamageBasedAttackSpeedModel("Enraged_", 10, 100, 0.05f, 10));
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class CamoInstincts : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Camo Instincts";
        public override string Icon => VanillaSprites.EnhancedEyesightUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Enhanced Eyesight gives 1 (+1 per stack) bonus damage towards Camo Bloons.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.EnhancedEyesight))
                {
                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "CamoInstincts")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Camo", 1, 1, false, false) { name = "CamoInstincts_" });
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
                                {
                                    if (behavior.name.Contains("CamoInstincts_"))
                                    {
                                        behavior.damageAddative += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class RazorGlaives : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Razor Glaives";
        public override string Icon => VanillaSprites.GlaivesUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Glaives deal 1 (+1 per stack) additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Glaives))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class StrongArm : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Strong Arm";
        public override string Icon => VanillaSprites.FasterThrowingUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Faster Throwing makes boomerangs fly 20% (+10% per stack) faster and gain 2 (+1 per stack) pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.FasterThrowing))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "StrongArm")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.pierce += 2;
                                foreach (var augmentStack in ModContent.GetContent<AugmentTemplate>().ToList())
                                {
                                    if (augmentStack.Name == "Splitrang" && augmentStack.StackIndex >= 1)
                                    {
                                        towerModel.GetAttackModel().weapons[0].projectile.pierce -= 2;
                                        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                        {
                                            if (behavior.name.Contains("Splitrang_"))
                                            {
                                                behavior.projectile.pierce += 2;
                                                behavior.projectile.GetBehavior<FollowPathModel>().Speed *= 1.2f;
                                            }
                                        }
                                    }
                                }

                                if (towerModel.appliedUpgrades.Contains(UpgradeType.GlaiveRicochet))
                                { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 1.2f; }
                                else
                                { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FollowPathModel>().speed *= 1.2f; }
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                                foreach (var augmentStack in ModContent.GetContent<AugmentTemplate>().ToList())
                                {
                                    if (augmentStack.Name == "Splitrang" && augmentStack.StackIndex >= 1)
                                    {
                                        towerModel.GetAttackModel().weapons[0].projectile.pierce -= 1;
                                        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                        {
                                            if (behavior.name.Contains("Splitrang_"))
                                            {
                                                behavior.projectile.pierce += 1;
                                                behavior.projectile.GetBehavior<FollowPathModel>().Speed *= 1.1f;
                                            }
                                        }
                                    }
                                }

                                if (towerModel.appliedUpgrades.Contains(UpgradeType.GlaiveRicochet))
                                { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 1.1f; }
                                else
                                { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FollowPathModel>().speed *= 1.1f; }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class WhiteHotRangs : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "White Hot Rangs";
        public override string Icon => VanillaSprites.RedHotRangsUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Red Hot Rangs catch Bloons on fire dealing 1 (+1 per stack) damage per second for 4 (+1 per stack) seconds.";
        public override void EditTower()
        {
            var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
            fire.lifespan = 4;
            fire.GetBehavior<DamageOverTimeModel>().damage = 1;
            fire.GetBehavior<DamageOverTimeModel>().interval = 1;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.RedHotRangs))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "WhiteHotRangs")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
                                towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().lifespan += 1;
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 1;
                            }
                        }
                    }                    

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Ignition : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Ignition";
        public override string Icon => VanillaSprites.MissileLauncherUpgradeIcon;
        public override string TowerType => "Bomb Shooter Augment";
        public override string AugmentDescription => "Missile Launcher releases a burst of fire around it when it fires a missile dealing 2 (+1 per stack) damage to all Bloons nearby.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MissileLauncher))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    var ignite = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().Duplicate();
                    ignite.range = towerModel.range;
                    ignite.weapons[0].rate = towerModel.GetAttackModel(0).weapons[0].rate;
                    ignite.weapons[0].projectile.GetDamageModel().damage = 2;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Ignition")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(ignite);
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class StickyTacks : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Sticky Tacks";
        public override string Icon => VanillaSprites.LongRangeTacksUpgradeIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Long Range Tacks now slow down Bloons by 10% (+5% per stack up to 50%) for 1 (+0.25 per stack) seconds.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.LongRangeTacks))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "StickyTacks")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new SlowModel("StickyTacks_", 0.9f, 1, "Ice:Slow", 3, "GlueBasic", true, false, null, false, false, false));
                            }
                            if (augment.StackIndex > 1)
                            {
                                if (towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().multiplier > 0.5f)
                                {
                                    towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().multiplier -= 0.05f;
                                }
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().lifespan += 0.25f;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class FlechetteTacks : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Flechette Tacks";
        public override string Icon => VanillaSprites.EvenMoreTacksUpgradeIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Even More Tacks causes tacks to break into 3 additional tacks that deal 1 (+1 per stack) damage upon hitting a Bloon.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.EvenMoreTacks))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    var tack = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().weapons[0].projectile.Duplicate();
                    tack.GetDamageModel().damage = 1;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "FlechetteTacks")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("FlechetteTacks_", tack, new ArcEmissionModel("ArcEmissionModel_", 3, 0, 25, null, true, false), true, false, false));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("FlechetteTacks_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Crystalize : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Crystalize";
        public override string Icon => VanillaSprites.MetalFreezeUpgradeIcon;
        public override string TowerType => "Ice Monkey Augment";
        public override string AugmentDescription => "Cold Snap creates an icy aura that deals 1 (+1 per stack) damage periodically to all Bloons that enter it.";
        public override void EditTower()
        {
            var icicleOrbit = Game.instance.model.GetTowerFromId("BoomerangMonkey-500").GetBehavior<OrbitModel>().Duplicate();
            icicleOrbit.projectile.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
            icicleOrbit.projectile.scale /= 2f;
            icicleOrbit.count = 5;
            icicleOrbit.range = 24;

            var icicleDamage = Game.instance.model.GetTowerFromId("BoomerangMonkey-500").GetAttackModel(1).Duplicate();
            icicleDamage.name = "Crystalize_";
            icicleDamage.weapons[0].projectile.radius = 24;
            icicleDamage.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            icicleDamage.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            icicleDamage.weapons[0].projectile.GetDamageModel().damage = 1;
            icicleDamage.weapons[0].projectile.pierce = 20;

            foreach (var behavior in icicleDamage.weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
            {
                icicleDamage.weapons[0].projectile.RemoveBehavior(behavior);
            }

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    icicleDamage.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate / 2.4f;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Crystalize")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(icicleOrbit);
                                towerModel.AddBehavior(icicleDamage);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("Crystalize_"))
                                    {
                                        behavior.weapons[0].projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
public class GorillaGlue : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Gorilla Glue";
        public override string Icon => VanillaSprites.StrongerGlueUpgradeIcon;
        public override string TowerType => "Glue Gunner Augment";
        public override string AugmentDescription => "Stronger Glue briefly stuns Bloons hit for 0.5 (+0.15 per stack) seconds.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.StrongerGlue))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    var stun = Game.instance.model.GetTowerFromId("BombShooter-400").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<SlowModel>();
                    stun.name = "GorillaGlue_";
                    stun.Lifespan = 0.5f;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "GorillaGlue")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(stun);

                                if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter))
                                {
                                    towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(stun);
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<SlowModel>().ToArray())
                                {
                                    if (behavior.name.Contains("GorillaGlue_"))
                                    {
                                        behavior.lifespan += 0.15f;
                                    }
                                }

                                if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter))
                                {
                                    foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehaviors<SlowModel>().ToArray())
                                    {
                                        if (behavior.name.Contains("GorillaGlue_"))
                                        {
                                            behavior.lifespan += 0.15f;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class ExplosiveRounds : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Explosive Rounds";
        public override string Icon => VanillaSprites.FullMetalJacketUpgradeIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Full Metal Jacket rounds cause a small explosion that deals 1 (+1 per stack) damage when they hit a Bloon.";
        public override void EditTower()
        {
            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var blast = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            bomb.name = "ExplosiveRounds_";
            bomb.projectile.GetDamageModel().damage = 1;
            bomb.projectile.pierce = 8;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.NightVisionGoggles))
                    {
                        bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "ExplosiveRounds")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(blast);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("ExplosiveRounds_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class ShrapnelDart : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Shrapnel Dart";
        public override string Icon => VanillaSprites.ShrapnelShotUpgradeIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Shrapnel Shot releases a homing dart that deals 1 (+1 per stack) damage when it hits a Bloon. Darts can hit 2 Bloons each.";
        public override void EditTower()
        {
            var dart = Game.instance.model.GetTowerFromId("MonkeySub").GetAttackModel().weapons[0].projectile.Duplicate();
            dart.pierce = 2;
            dart.GetDamageModel().damage = 1;
            dart.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ShrapnelShot))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "ShrapnelDart")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new EmitOnDamageModel("ShrapnelDart_", new SingleEmissionModel("", null), dart));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behaviror in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<EmitOnDamageModel>().ToArray())
                                {
                                    if (behaviror.name.Contains("ShrapnelDart_"))
                                    {
                                        behaviror.projectile.GetDamageModel().damage += 1;
                                    }
                                }    
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class DobuleTap : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Double Tap";
        public override string Icon => VanillaSprites.EvenFasterFiringUpgradeIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Even Faster Firing shoots a second regular bullet that deals 0 (+1 per stack) additional damage.";
        public override void EditTower()
        {
            var snipe = Game.instance.model.GetTowerFromId("SniperMonkey").GetAttackModel().Duplicate();
            snipe.weapons[0].projectile.GetDamageModel().damage = 2;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.EvenFasterFiring))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    snipe.weapons[0].rate = towerModel.GetAttackModel(0).weapons[0].rate;

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.NightVisionGoggles))
                    {
                        snipe.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "DobuleTap")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(snipe);
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Torpedos : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Torpedoes";
        public override string Icon => VanillaSprites.AirburstDartsUpgradeIcon;
        public override string TowerType => "Monkey Sub Augment";
        public override string AugmentDescription => "Air-Burst Darts fire a mini torpedo instead of darts that explode on hitting a Bloon that deals 2 (+1 per stack) damage in a small area.";
        public override void EditTower()
        {
            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var blast = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            bomb.name = "Torpedos_";
            bomb.projectile.GetDamageModel().damage = 2;
            bomb.projectile.pierce = 8;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.AirburstDarts))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedIntel))
                    {
                        bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Torpedos")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(blast);
                                towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.display;
                                towerModel.GetAttackModel().weapons[0].projectile.scale /= 2;
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("Torpedos_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class GrapesAhoy : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Grapes Ahoy";
        public override string Icon => VanillaSprites.GrapeShotUpgradeIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Grape Shot fires 1 (+1 per stack) additional grapes.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.GrapeShot))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[2].GetDescendant<ArcEmissionModel>().count += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class SpineappleBombs : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Spineapple Bombs";
        public override string Icon => VanillaSprites.ExplodingPineappleUpgradeIcon;
        public override string TowerType => "Monkey Ace Augment";
        public override string AugmentDescription => "Exploding Pineapple creates a spread of 4 (+2 per stack) spikes when they explode that deal 2 damage each. **Selling Aces with this augment may softlock the game**";
        public override void EditTower()
        {
            var spike = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            spike.GetDamageModel().damage = 2;
            spike.pierce = 3;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ExplodingPineapple))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
                    {
                        spike.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "SpineappleBombs")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                                {
                                    if (weaponModel.name.Contains("WeaponModel_PineappleWeapon"))
                                    {
                                        spike.GetBehavior<TravelStraitModel>().lifespan /= 1.75f;
                                        var createProjectile = weaponModel.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                                        createProjectile.name = "SpineappleBombs_";
                                        createProjectile.projectile = spike;
                                        createProjectile.emission = new ArcEmissionModel("Spineapples_", 4, 0, 360, null, false, true);
                                        weaponModel.projectile.AddBehavior(createProjectile);
                                        weaponModel.projectile.AddBehavior(new DestroyProjectileIfTowerDestroyedModel(""));
                                    }
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                                {
                                    if (weaponModel.name.Contains("WeaponModel_PineappleWeapon"))
                                    {
                                        foreach (var behavior in weaponModel.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>())
                                        {
                                            if (behavior.name.Contains("SpineappleBombs_"))
                                            {
                                                behavior.GetDescendant<ArcEmissionModel>().count += 2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class RocketDarts : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Rocket Darts";
        public override string Icon => VanillaSprites.SharperDartsUpgradeIcon;
        public override string TowerType => "Monkey Ace Augment";
        public override string AugmentDescription => "Sharper Darts gives Monkey Aces rocket darts that explode when they hit a Bloon dealing 2 (+1 per stack) damage to nearby Bloons.";
        public override void EditTower()
        {
            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var blast = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            bomb.name = "RocketDarts_";
            bomb.projectile.GetDamageModel().damage = 2;
            bomb.projectile.pierce = 8;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SharperDarts))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
                    {
                        bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "RocketDarts")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(blast);
                                towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-030").GetAttackModel().weapons[0].projectile.display;
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("RocketDarts_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class HyperJets : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Hyper Jets";
        public override string Icon => VanillaSprites.BiggerJetsUpgradeIcon;
        public override string TowerType => "Heli Pilot Augment";
        public override string AugmentDescription => "Bigger Jets provide 20% (+10% per stack) additional move speed.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BiggerJets))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    var movement = towerModel.GetBehavior<AirUnitModel>().GetBehavior<HeliMovementModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "HyperJets")
                        {
                            if (augment.StackIndex == 1)
                            {
                                    movement.maxSpeed *= 1.2f;
                                    movement.movementForceStart *= 1.2f;
                                    movement.movementForceEnd *= 1.2f;
                                    movement.movementForceEndSquared = movement.movementForceEnd * movement.movementForceEnd;
                                    movement.brakeForce *= 1.2f;
                            }
                            if (augment.StackIndex > 1)
                            {
                                    movement.maxSpeed *= 1.1f;
                                    movement.movementForceStart *= 1.1f;
                                    movement.movementForceEnd *= 1.1f;
                                    movement.movementForceEndSquared = movement.movementForceEnd * movement.movementForceEnd;
                                    movement.brakeForce *= 1.1f;
                                }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class FlameBurst : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Flame Burst";
        public override string Icon => VanillaSprites.BurnyStuffUpgradeIcon;
        public override string TowerType => "Mortar Monkey Augment";
        public override string AugmentDescription => "Burny Stuff shells release 4 (+1 per stack) fireballs upon landing that deal 1 (+1 per stack) damage.";
        public override void EditTower()
        {
            var fireball = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            fireball.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
            fireball.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
            fireball.GetDamageModel().damage = 1;
            fireball.pierce = 4;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BurnyStuff))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "FlameBurst")
                        {
                            if (augment.StackIndex == 1)
                            {
                                var createProjectile = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                                createProjectile.name = "FlameBurst_";
                                createProjectile.projectile = fireball;
                                createProjectile.emission = new RandomArcEmissionModel("FireBurstEmission_", 4, 0, 0, 360, 0, null);
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(createProjectile);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>())
                                {
                                    if (behavior.name.Contains("FlameBurst_"))
                                    {
                                        behavior.GetDescendant<RandomArcEmissionModel>().count += 1;
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*public class LightningShock : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Lightning Shock";
        public override string Icon => VanillaSprites.LaserShockUpgradeIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Laser Shock darts create arcing lightning that can hit 3 (+1 per stack) Bloons and deal 1 (+1 per stack) damage.";
        public override void EditTower()
        {
            var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").projectile.Duplicate();

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.LaserShock))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    lightning.pierce = 3;
                    lightning.GetDamageModel().damage = 1;
                    lightning.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                    lightning.GetBehavior<LightningModel>().splitRange = 20;
                    lightning.GetBehavior<LightningModel>().splits = 1;

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedTargeting))
                    {
                        lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "LightningShock")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("LightningShock_", lightning, new ArcEmissionModel("", 1, 0, 0, null, false, false), true, false, true));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("LightningShock_"))
                                    {
                                        behavior.projectile.pierce += 1;
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class PunchDarts : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Punch Darts";
        public override string Icon => VanillaSprites.PowerfulDartsUpgradeIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Powerful Darts now have a 35% (+5% per stack) chance to knockback small Bloons hit by up to 5 (+2 per stack) units.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.PowerfulDarts))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "PunchDarts")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new WindModel("PunchDarts_", 1, 5, 0.35f, false, null, 0, null, 1));
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().distanceMax += 2;
                                if (towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().chance < 100)
                                {
                                    towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().chance += 5;
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class FireElementals : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Fire Elementals";
        public override string Icon => VanillaSprites.WallOfFireUpgradeIcon;
        public override string TowerType => "Wizard Monkey Augment";
        public override string AugmentDescription => "Wall of Fire periodically creates mini fire elementals that shoot fireballs at Bloons that deal 1 (+1 per stack) damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.WallOfFire))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "FireElementals")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var behavior in Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("Spawner"))
                                    {
                                        var spawner = behavior.Duplicate();
                                        spawner.GetBehavior<RandomPositionModel>().maxDistance = towerModel.range;
                                        spawner.range = towerModel.range;
                                        spawner.name = "FireElementals_";
                                        spawner.weapons[0].rate *= 1.25f;
                                        spawner.weapons[0].projectile.RemoveBehavior<CreateTowerModel>();
                                        spawner.weapons[0].projectile.AddBehavior(new CreateTowerModel("SentryPlace", GetTowerModel<FireElemental>().Duplicate(), 0f, true, false, false, true, true));
                                        spawner.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;

                                        if (towerModel.appliedUpgrades.Contains(UpgradeType.MonkeySense))
                                        {
                                            spawner.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                                        }

                                        towerModel.AddBehavior(spawner);
                                    }
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("FireElementals_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class HighTechLasers : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "High-Tech Lasers";
        public override string Icon => VanillaSprites.LaserBlastUpgradeIcon;
        public override string TowerType => "Super Monkey Augment";
        public override string AugmentDescription => "Laser Blasts deal 1 (+1 per stack) additional damage. Can pop purple Bloons at 3+ stacks.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.LaserBlasts))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "HighTechLasers")
                        {
                            if (augment.StackIndex >= 3)
                            {
                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.PlasmaBlasts))
                                { towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None; }
                                else
                                { towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead; }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class CaltropMines : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Caltrop Mines";
        public override string Icon => VanillaSprites.CaltropsUpgradeIcon;
        public override string TowerType => "Ninja Monkey Augment";
        public override string AugmentDescription => "Caltrops explode after they are depleted dealing 1 (+1 per stack) damage in a small area.";
        public override void EditTower()
        {
            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
            var effect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
            bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            bomb.GetDamageModel().damage = 1;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Caltrops))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    var createEffect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExhaustFractionModel>().Duplicate();
                    createEffect.effectModel = effect;
                    bomb.AddBehavior(createEffect);

                    var createProjectile = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                    createProjectile.name = "CaltropMines_";
                    createProjectile.projectile = bomb;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "CaltropMines")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel(1).weapons[0].projectile.AddBehavior(createProjectile);
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class PotentAcid : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Potent Acid";
        public override string Icon => VanillaSprites.PerishingPotionsUpgradeIcon;
        public override string TowerType => "Alchemist Augment";
        public override string AugmentDescription => "Perishing Potions cause Bloons to receive 1 (+1 per stack) additional damage from all sources for 4 seconds.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "PotentAcid")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("PotentAcid_", "Acid_Bonus_Damage", 4f, 1, 999, true, false, false, "bleed"));
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class RoseThorn : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Rose Thorn";
        public override string Icon => VanillaSprites.HeartOfOakUpgradeIcon;
        public override string TowerType => "Druid Augment";
        public override string AugmentDescription => "Heart of Oak throws 1 (+1 per stack) additional thorns.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.HeartOfOak))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class SplittingProngs : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Splitting Prongs";
        public override string Icon => "c2d408c737294434ca03e0b27d4aea61";
        public override string TowerType => "Mermonkey Augment";
        public override string AugmentDescription => "Sharper Prongs burst out 2 spikes upon hitting a Bloon that deal 1 (+1 per stack) damage each.";
        public override void EditTower()
        {
            var spike = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            spike.display = Game.instance.model.GetTowerFromId("Druid").GetAttackModel().weapons[0].projectile.display;
            spike.GetBehavior<TravelStraitModel>().Lifespan *= 2;
            spike.GetDamageModel().damage = 1;
            spike.pierce = 3;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SharperProngs))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.EchosensePrecision))
                    {
                        spike.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "SplittingProngs")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("SplittingProngs_", spike, new ArcEmissionModel("", 2, 0, 30, null, false, false), false, false, false));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("SplittingProngs_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class HeatedSpikes : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Super Heated Spikes";
        public override string Icon => VanillaSprites.WhiteHotSpikesUpgradeIcon;
        public override string TowerType => "Spike Factory Augment";
        public override string AugmentDescription => "White Hot Spikes deal 1 (+1 per stack) additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.WhiteHotSpikes))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var behavior in towerModel.GetAttackModels().ToArray())
                    {
                        behavior.weapons[0].projectile.GetDamageModel().damage += 1;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class SpikeSprayer : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Spike Sprayer";
        public override string Icon => VanillaSprites.DirectedSpikesUpgradeIcon;
        public override string TowerType => "Spike Factory Augment";
        public override string AugmentDescription => "Smart Spikes creates mini spike piles really fast that can pop 3 Bloons each. (gains +15% attack speed per stack)";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.DirectedSpikes))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "SpikeSprayer")
                        {
                            if (augment.StackIndex == 1)
                            {
                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.WhiteHotSpikes))
                                {
                                    var spikes = Game.instance.model.GetTowerFromId("SpikeFactory-220").GetAttackModel().Duplicate();
                                    spikes.name = "SpikeSprayer_";
                                    spikes.weapons[0].rate /= 2f;
                                    spikes.weapons[0].projectile.pierce = 3;
                                    towerModel.AddBehavior(spikes);
                                }
                                else
                                {
                                    var spikes = Game.instance.model.GetTowerFromId("SpikeFactory-020").GetAttackModel().Duplicate();
                                    spikes.name = "SpikeSprayer_";
                                    spikes.weapons[0].rate /= 2f;
                                    spikes.weapons[0].projectile.pierce = 3;
                                    towerModel.AddBehavior(spikes);
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("SpikeSprayer_"))
                                    {
                                        behavior.weapons[0].rate /= 1.15f;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class BountifulHarvest : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Bountiful Harvest";
        public override string Icon => VanillaSprites.GreaterProductionUpgradeIcon;
        public override string TowerType => "Banana Farm Augment";
        public override string AugmentDescription => "Greater Production produces 2 (+1 per stack) additional bananas per round.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.GreaterProduction))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "BountifulHarvest")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 2;
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class LifeFarm : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Life Farm";
        public override string Icon => VanillaSprites.ValuableBananasUpgradeIcon;
        public override string TowerType => "Banana Farm Augment";
        public override string AugmentDescription => "Valuable Bananas generates 1 (+1 per stack) life per round.";
        public override void EditTower()
        {
            var lives = Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<BonusLivesPerRoundModel>().Duplicate();
            lives.name = "LifeFarm_";
            lives.amount = 1;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ValuableBananas))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "LifeFarm")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(lives);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<BonusLivesPerRoundModel>())
                                {
                                    if (behavior.name.Contains("LifeFarm_"))
                                    {
                                        behavior.amount += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class RoundRobbing : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Round Robbing";
        public override string Icon => VanillaSprites.BananaSalvageUpgradeIcon;
        public override string TowerType => "Banana Farm Augment";
        public override string AugmentDescription => "Banana Salvage generates $50 (+$25 per stack) at the end of every round.";
        public override void EditTower()
        {
            var cash = Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<PerRoundCashBonusTowerModel>().Duplicate();
            cash.name = "RoundRobbing_";
            cash.cashPerRound = 50;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BananaSalvage))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "RoundRobbing")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(cash);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<PerRoundCashBonusTowerModel>())
                                {
                                    if (behavior.name.Contains("RoundRobbing_"))
                                    {
                                        behavior.cashPerRound += 25;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class StasisField : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Stasis Field";
        public override string Icon => VanillaSprites.GrowBlockerUpgradeIcon;
        public override string TowerType => "Monkey Village Augment";
        public override string AugmentDescription => "Grow Blocker gains a stasis field that slows Bloons in radius by 15% (+5% per stack up to 75% total).";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.GrowBlocker))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "StasisField")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(new SlowBloonsZoneModel("StasisField_", towerModel.range, "Ice:Regular:ArcticWind", true, null, 0.85f, 0, true, 0, "", false));
                            }
                            if (augment.StackIndex > 1 && augment.StackIndex <= 13)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<SlowBloonsZoneModel>())
                                {
                                    if (behavior.name.Contains("StasisField_"))
                                    {
                                        towerModel.RemoveBehavior(behavior);
                                        towerModel.AddBehavior(new SlowBloonsZoneModel("StasisField_", towerModel.range, "Ice:Regular:ArcticWind", true, null, (0.9f - 0.05f * augment.StackIndex), 0, true, 0, "", false));
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class MoreSentries : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "More Sentries";
        public override string Icon => VanillaSprites.SentryGunUpgradeIcon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Engineer Monkeys place sentries 25% (+10% per stack) faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SentryGun))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "MoreSentries")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("Spawner"))
                                    {
                                        behavior.weapons[0].rate /= 1.25f;
                                    }
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("Spawner"))
                                    {
                                        behavior.weapons[0].rate /= 1.1f;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class NailShotgun : AugmentTemplate
    {
        public override int SandboxIndex => 2;
        public override Rarity AugmentRarity => Rarity.Intermediate;
        public override string AugmentName => "Nail Shotgun";
        public override string Icon => VanillaSprites.PinUpgradeIcon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Pin fires a spread of 5 nails that deal 0 (+1 per stack) additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Pin))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "NailShotgun")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("", 5, 0, 0, 45, 0, null);
                                towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class Intermediate
    {
        public static List<string> IntermediateAug = [];
        public static List<string> IntermediateImg = [];
    }
}