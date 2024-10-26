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
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2Cpp;
using AlternatePaths.Displays.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity.Powers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Unity.CollectionEvent;
using AugmentSentries;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Octokit;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using BTD_Mod_Helper.Api.Components;
using HarmonyLib;
using UnityEngine.Playables;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using System.Diagnostics.Metrics;
using Il2CppAssets.Scripts.Data.TrophyStore;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.Cosmetics.Pets;
using BTD_Mod_Helper.Api.Display;

namespace AugmentsMod.Augments
{
    public class CrossbowChampion : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Crossbow Champion";
        public override string Icon => VanillaSprites.CrossBowMasterUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Crossbow Master fires 3 bolts at a time that deal 0 (+3 per stack) additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.CrossbowMaster))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "CrossbowChampion")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("aaa", 3, 0, 30, null, false, false);
                            }

                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 3;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class GlaiveDanger : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Glaive Danger";
        public override string Icon => VanillaSprites.GlaiveLordUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Glaive Lord continuously fires 8 (+1 per stack) glaives in all directions that deal 4 (+2 per stack) damage.";
        public override void EditTower()
        {
            var radial = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().Duplicate();
            radial.name = "GlaiveDanger_";
            radial.fireWithoutTarget = true;
            radial.weapons[0].projectile.GetDamageModel().damage = 4;
            radial.weapons[0].projectile.pierce = 10;
            radial.weapons[0].projectile.ApplyDisplay<ManiaGlaive>();
            radial.weapons[0].projectile.scale *= 1.5f;
            radial.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
            radial.weapons[0].projectile.AddBehavior(new RotateModel("RotateModel_", -1440));
            radial.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.GlaiveLord))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "GlaiveDanger")
                        {
                            if (augment.StackIndex == 1)
                            {
                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.RedHotRangs))
                                {
                                    radial.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
                                }

                                towerModel.AddBehavior(radial);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("GlaiveDanger_"))
                                    {
                                        behavior.weapons[0].GetDescendant<ArcEmissionModel>().count += 1;
                                        behavior.weapons[0].projectile.GetDamageModel().damage += 2;
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
    public class MissileSilo : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Missile Silo";
        public override string Icon => VanillaSprites.MoabEliminatorUpgradeIcon;
        public override string TowerType => "Bomb Shooter Augment";
        public override string AugmentDescription => "MOAB Eliminator periodically fires off a volley of 6 aerial missiles globally that each deal 8 (+2 per stack) damage and slow small Bloons hit by 25%. Missiles deal 0 (+5 per stack) additional damage to Ceramic and MOAB Bloons.";
        public override void EditTower()
        {
            var missiles = Game.instance.model.GetTowerFromId("Rosalia 3").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Duplicate();
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().maxProjectileCount = 6;
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().damage = 8;
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.pierce = 25;
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetBehavior<SlowModel>().multiplier = 0.75f;
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.RemoveBehavior(missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetBehavior<SlowModifierForTagModel>());
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.hasDamageModifiers = true;
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.AddBehavior(new DamageModifierForTagModel("MissileSilo_", "Ceramic", 1, 0, false, false) { name = "CeramicModifier_" });
            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.AddBehavior(new DamageModifierForTagModel("MissileSilo_", "Moabs", 1, 0, false, false) { name = "MoabModifier_" });
            missiles.rate = 3;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MOABEliminator))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    var silo = towerModel.GetAttackModel().Duplicate();
                    silo.name = "MissileSilo_";
                    silo.weapons[0] = missiles;
                    silo.range = 999;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "MissileSilo")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(silo);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<AttackModel>())
                                {
                                    if (behavior.name.Contains("MissileSilo_"))
                                    {
                                        var rocket = behavior.weapons[0].projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel;
                                        rocket.GetDamageModel().damage += 2;
                                        foreach (var damage in rocket.GetBehaviors<DamageModifierForTagModel>().ToArray())
                                        {
                                            damage.damageAddative += 5;
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
    public class ReaperShowtime : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Reaper's Showtime";
        public override string Icon => VanillaSprites.InfernoRingUpgradeIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Inferno Ring periodically causes fireballs to rain from the sky that explode in a large area and deal 8 (+4 per stack) damage. (Fires 15% faster per stack).";
        public override void EditTower()
        {
            var meteors = Game.instance.model.GetTowerFromId("MortarMonkey-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Duplicate();
            meteors.rate = 3;
            meteors.emission = new RandomTargetSpreadModel("Spread", 100, null, null);
            meteors.SetProjectile(Game.instance.model.GetTowerFromId("MonkeySub-030").GetAttackModel(1).weapons[0].projectile.Duplicate());
            meteors.projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
            meteors.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 8;
            meteors.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.radius = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius * 1.5f;
            meteors.projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().effectModel = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
            meteors.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.InfernoRing))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    var meteroRain = towerModel.GetAttackModel().Duplicate();
                    meteroRain.name = "ReaperShowtime_";
                    meteroRain.weapons[0] = meteors;
                    meteroRain.range = 999;
                    meteroRain.attackThroughWalls = true;
                    meteroRain.fireWithoutTarget = true;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "ReaperShowtime")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(meteroRain);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<AttackModel>())
                                {
                                    if (behavior.name.Contains("ReaperShowtime_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage += 4;
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
    public class LandmineBlades : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Landmine Blades";
        public override string Icon => VanillaSprites.SuperMaelstromUpgradeIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Super Maelstrom blades create mini landmines on the track after popping a Bloon that explode when Bloons get near dealing 4 (+2 per stack) damage in a moderate area.";
        public override void EditTower()
        {
            var explosion = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var sound = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
            var effect = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            explosion.projectile.GetDamageModel().damage = 4;
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SuperMaelstrom))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "LandmineBlades")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var behavior in Game.instance.model.GetTowerFromId("GlueGunner-500").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                                {
                                    if (behavior.name.Contains("GlueLinger"))
                                    {
                                        var emit = behavior.Duplicate();
                                        emit.name = "LandmineBlades_";
                                        emit.layers = 1;
                                        emit.lifespan = 0.5f;
                                        emit.overlayType = null;
                                        var landmine = emit.GetBehavior<EmitOnDestroyModel>().projectile;
                                        landmine.pierce = 1;
                                        landmine.GetDamageModel().damage = 0;
                                        landmine.GetBehavior<AgeModel>().lifespan = 10;
                                        landmine.display = Game.instance.model.GetTowerFromId("Rosalia 10").GetAbility(2).GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display;
                                        landmine.AddBehavior(explosion);
                                        landmine.AddBehavior(sound);
                                        landmine.AddBehavior(effect);
                                        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(emit);
                                        towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
                                    }
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                                {
                                    if (behavior.name.Contains("LandmineBlades_"))
                                    {
                                        behavior.GetBehavior<EmitOnDestroyModel>().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;
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
    public class BattleCruiser : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Battle Cruiser";
        public override string Icon => VanillaSprites.SubCommanderUpgradeIcon;
        public override string TowerType => "Monkey Sub Augment";
        public override string AugmentDescription => "Sub Commander fires 3 (+1 per stack) darts at a time and attacks 20% (+5% per stack) faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SubCommander))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "BattleCruiser")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("", 3, 0, 9, null, false, false);
                                towerModel.GetAttackModel().weapons[0].rate /= 1.2f;
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().count += 1;
                                towerModel.GetAttackModel().weapons[0].GetDescendant<ArcEmissionModel>().angle += 3;
                                towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class CarrierDefense : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Carrier Defense";
        public override string Icon => VanillaSprites.CarrierFlagshipUpgradeIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Carrier Flagship creates 2 (+1 per stack) additional elite fighter planes that deal significant damage to all Bloons.";
        public override void EditTower()
        {
            var carrier = Game.instance.model.GetTowerFromId("MonkeyBuccaneer-500").GetAttackModel(1).Duplicate();
            carrier.name = "CarrierDefense_";
            carrier.weapons[0].GetBehavior<SubTowerFilterModel>().maxNumberOfSubTowers = 2;

            var plane = carrier.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
            plane.GetBehavior<TowerExpireOnParentUpgradedModel>().parentTowerUpgradeTier = 5;
            plane.GetBehavior<AirUnitModel>().display = Game.instance.model.GetTowerFromId("BuccaneerParagonPlane").GetBehavior<AirUnitModel>().display;

            plane.GetAttackModel(0).weapons[0].projectile.GetDamageModel().damage += 2;
            plane.GetAttackModel(0).weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            plane.GetAttackModel(1).weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            plane.GetAttackModel(1).weapons[0].rate /= 2;
            plane.GetAttackModel(1).weapons[0].GetDescendant<ArcEmissionModel>().count *= 2;
            plane.GetAttackModel(1).weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 2;

            var bombs = plane.GetAttackModel(0).Duplicate();
            bombs.weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount = 1;
            bombs.weapons[0].projectile.GetDamageModel().damage = 0;
            bombs.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            bombs.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.display;
            bombs.weapons[0].projectile.scale /= 2;

            var explosion = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var sound = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
            var effect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            explosion.projectile.GetDamageModel().damage = 7;
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            bombs.weapons[0].projectile.AddBehavior(explosion);
            bombs.weapons[0].projectile.AddBehavior(sound);
            bombs.weapons[0].projectile.AddBehavior(effect);

            plane.GetDescendant<FighterMovementModel>().maxSpeed *= 3f;
            plane.RemoveBehavior(plane.GetAttackModel(2));
            plane.AddBehavior(bombs);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.CarrierFlagship))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towerModel.appliedUpgrades.Contains(UpgradeType.CrowsNest))
                    {
                        plane.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "CarrierDefense")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(carrier);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels())
                                {
                                    if (attack.name.Contains("CarrierDefense_"))
                                    {
                                        attack.weapons[0].GetBehavior<SubTowerFilterModel>().maxNumberOfSubTowers += 1;
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
    public class TradeNetwork : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Trade Network";
        public override string Icon => VanillaSprites.TradeEmpireUpgradeIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Trade Empire increases money generation globally by 10% (+5% per stack).";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.TradeEmpire))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "TradeNetwork")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.range = 999;
                                towerModel.AddBehavior(new MonkeyCityIncomeSupportModel("TradeNetwork_", true, 1.1f, null, "", ""));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<MonkeyCityIncomeSupportModel>().ToArray())
                                {
                                    if (behavior.name.Contains("TradeNetwork_"))
                                    {
                                        behavior.incomeModifier += 0.05f;
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
    public class RayofBoom : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Ray of Boom";
        public override string Icon => VanillaSprites.RayOfDoomUpgradeIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Ray of Doom causes Bloons hit to explode when they are popped dealing 10 (+5 per stack) damage to nearby Bloons.";
        public override void EditTower()
        {
            var explosion = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
            var sound = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
            var effect = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            explosion.GetDamageModel().damage = 10;
            explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            explosion.pierce = 10;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.RayOfDoom))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedTargeting))
                    {
                        explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "RayofBoom")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var behavior in Game.instance.model.GetTowerFromId("GlueGunner-500").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                                {
                                    if (behavior.name.Contains("GlueLinger"))
                                    {
                                        var emit = behavior.Duplicate();
                                        emit.name = "RayofBoom_";
                                        emit.layers = 1;
                                        emit.lifespan = 999f;
                                        emit.overlayType = null;
                                        emit.GetBehavior<EmitOnDestroyModel>().projectile = explosion;
                                        emit.GetBehavior<EmitOnDestroyModel>().projectile.AddBehavior(sound);
                                        emit.GetBehavior<EmitOnDestroyModel>().projectile.AddBehavior(effect);
                                        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(emit);
                                        towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
                                    }
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                                {
                                    if (behavior.name.Contains("RayofBoom_"))
                                    {
                                        behavior.GetBehavior<EmitOnDestroyModel>().projectile.GetDamageModel().damage += 5;
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
    public class NoBloonsLand : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "No Bloons Land";
        public override string Icon => VanillaSprites.BloonExclusionZoneUpgradeIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Bloon Exclusion Zone fires out MOAB assassin missiles that deal 5 damage and an extra 25 (+25 per stack) damage to MOABs.";
        public override void EditTower()
        {
            var missiles = Game.instance.model.GetTowerFromId("BombShooter-040").GetAttackModel().weapons[0].projectile.Duplicate();
            missiles.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 5;
            missiles.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 25, false, false) { name = "MoabModifier_" });
            missiles.GetBehavior<TravelStraitModel>().lifespan = Game.instance.model.GetTowerFromId("DartlingGunner-005").GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan * 2;
            missiles.scale /= 2;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BloonExclusionZone))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedTargeting))
                    {
                        missiles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "NoBloonsLand")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels())
                                {
                                    var assassins = attack.Duplicate();
                                    assassins.name = "NoBloonsLand_";
                                    assassins.weapons[0].projectile = missiles;
                                    assassins.weapons[0].GetDescendant<RandomEmissionModel>().count = 4;
                                    towerModel.AddBehavior(assassins);
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels())
                                {
                                    if (attack.name.Contains("NoBloonsLand_"))
                                    {
                                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 25;
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
    public class Smite : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Smite";
        public override string Icon => VanillaSprites.SuperStormUpgradeIcon;
        public override string TowerType => "Druid Augment";
        public override string AugmentDescription => "Superstorm fires a bolt of lightning from the sky that hits Bloons globally and deals 5 (+2 per stack) damage to all Bloons struck. (Gains +10% attack speed per stack)";
        public override void EditTower()
        {
            var bolt = Game.instance.model.GetTowerFromId("MonkeySub-030").GetAttackModel(1).Duplicate();
            bolt.name = "Smite_";
            bolt.weapons[0].rate = 4;
            bolt.range = 999;
            bolt.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
            bolt.weapons[0].projectile.RemoveBehavior<CreateSoundOnProjectileExpireModel>();
            bolt.weapons[0].projectile.RemoveBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>();

            var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").projectile.Duplicate();
            lightning.GetDamageModel().damage = 5;
            bolt.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile = lightning;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Superstorm))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Smite")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(bolt);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels())
                                {
                                    if (attack.name.Contains("Smite_"))
                                    {
                                        attack.weapons[0].rate /= 1.1f;
                                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage += 2;
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
    public class Destructobots : AugmentTemplate
    {
        public override int SandboxIndex => 4;
        public override Rarity AugmentRarity => Rarity.Mastery;
        public override string AugmentName => "Destructobots";
        public override string Icon => VanillaSprites.XXXLUpgradeIcon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "XXXL Trap periodically sends robots along the track that explode when they run into a Bloon. Explosion deals 6 (+2 per stack) damage and 20 (+5 per stack) additional damage to MOABs. (gains +10% attack speed per stack)";
        public override void EditTower()
        {
            var agemodel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
            var summon = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
            summon.name = "Destructobots_";
            summon.weapons[0].projectile.name = "AttackModel_Summon3_";
            summon.weapons[0].emission = new NecromancerEmissionModel("BaseDeploy_", 1, 1, 1, 1, 1, 1, 0, null, null, null, 1, 1, 1, 1, 2);
            summon.weapons[0].rate = 16f;
            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection = false;
            summon.weapons[0].projectile.GetDamageModel().damage = 0;
            summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            summon.weapons[0].projectile.pierce = 1;
            summon.weapons[0].projectile.maxPierce = 1;
            summon.range = 100000;
            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames = 0;
            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 999999f;
            summon.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
            agemodel.lifespanFrames = 0;
            agemodel.lifespan = 999999f;
            agemodel.rounds = 9999;
            summon.weapons[0].projectile.AddBehavior(agemodel);

            foreach (var trophyStoreItem in GameData.Instance.trophyStoreItems.storeItems)
            {
                foreach (var trophyItemTypeData in trophyStoreItem.itemTypes.Where(data => data.itemType == TrophyItemType.TowerPet))
                {
                    if (trophyItemTypeData.itemTarget.IsType(out Pet pet))
                    {
                        if (pet.id.Contains("Engineer"))
                        {
                            summon.weapons[0].projectile.display = pet.display;
                        }
                    }
                }
            }

            var explosion = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
            var sound = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
            var effect = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
            explosion.projectile.GetDamageModel().damage = 4;
            explosion.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 20, false, false) { name = "MoabModifier_" });
            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            summon.weapons[0].projectile.AddBehavior(explosion);
            summon.weapons[0].projectile.AddBehavior(sound);
            summon.weapons[0].projectile.AddBehavior(effect);
            summon.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.XXXLTrap))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Destructobots")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.RemoveBehaviors<NecromancerZoneModel>();
                                towerModel.RemoveBehaviors<AttackModel>();
                                towerModel.AddBehavior(summon);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels())
                                {
                                    if (attack.name.Contains("Destructobots_"))
                                    {
                                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;
                                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 5;
                                        attack.weapons[0].rate /= 1.1f;
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

    [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.SimulationBehaviors.NecroData), nameof(NecroData.RbePool))]
    internal static class Necro_RbePool
    {
        [HarmonyPrefix]
        private static bool Postfix(NecroData __instance, ref int __result)
        {
            var tower = __instance.tower;
            __result = 9999;
            return false;
        }
    }

    public class Mastery
    {
        public static List<string> MasteryAug = new List<string>();
        public static List<string> MasteryImg = new List<string>();
    }
}