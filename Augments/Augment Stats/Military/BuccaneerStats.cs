using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Templates;

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateBuccaneerStats : BuccaneerStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "GrapesAhoy")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.GrapeShot))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[2].GetDescendant<ArcEmissionModel>().count += augment.StackIndex;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedBuccaneerStats : BuccaneerStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "DepthCharges")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.Destroyer))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var mines = Game.instance.model.GetTowerFromId("AdmiralBrickell").GetAttackModel(1).Duplicate();
                            mines.name = "DepthCharges_";
                            mines.weapons[0].rate /= 1.25f;

                            int i = 0;
                            while (i < augment.StackIndex - 1)
                            {
                                mines.weapons[0].rate /= 1.1f;
                                i++;
                            }

                            mines.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (1 + augment.StackIndex);
                            mines.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = (1 + augment.StackIndex);
                            mines.range = towerModel.range;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.CrowsNest))
                            {
                                mines.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.AddBehavior(mines);
                        }
                    }
                }

                if (augment.Name == "SurplusCargo")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.FavoredTrades))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var crates = Game.instance.model.GetTowerFromId("BananaFarm-400").GetAttackModel().Duplicate();
                            crates.name = "SurplusCargo_";
                            crates.weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count = (2 + augment.StackIndex);
                            crates.weapons[0].projectile.GetBehavior<CashModel>().maximum = (75f + 25 * augment.StackIndex);
                            crates.weapons[0].projectile.GetBehavior<CashModel>().minimum = (75f + 25 * augment.StackIndex);
                            crates.range = towerModel.range;
                            towerModel.AddBehavior(crates);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryBuccaneerStats : BuccaneerStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "CarrierDefense")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.CarrierFlagship))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var carrier = Game.instance.model.GetTowerFromId("MonkeyBuccaneer-500").GetAttackModel(1).Duplicate();
                            carrier.name = "CarrierDefense_";
                            carrier.weapons[0].GetBehavior<SubTowerFilterModel>().maxNumberOfSubTowers = 4 + augment.StackIndex;
                            carrier.weapons[0].startInCooldown = true;
                            carrier.weapons[0].customStartCooldown = 3.5f;

                            var plane = carrier.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
                            plane.GetBehavior<TowerExpireOnParentUpgradedModel>().parentTowerUpgradeTier = 5;
                            plane.GetBehavior<AirUnitModel>().display = Game.instance.model.GetTowerFromId("BuccaneerParagonPlane").GetBehavior<AirUnitModel>().display;

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
                            explosion.projectile.GetDamageModel().damage = 4;
                            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                            bombs.weapons[0].projectile.AddBehavior(explosion);
                            bombs.weapons[0].projectile.AddBehavior(sound);
                            bombs.weapons[0].projectile.AddBehavior(effect);

                            plane.GetDescendant<FighterMovementModel>().maxSpeed *= 3f;
                            plane.RemoveBehavior(plane.GetAttackModel(2));
                            plane.AddBehavior(bombs);

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.CrowsNest))
                            {
                                plane.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.AddBehavior(carrier);
                        }
                    }
                }
                if (augment.Name == "TradeNetwork")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.TradeEmpire))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.range = 999;
                            towerModel.AddBehavior(new MonkeyCityIncomeSupportModel("TradeNetwork_", true, (1.15f + (0.1f * augment.StackIndex)), null, "", ""));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
