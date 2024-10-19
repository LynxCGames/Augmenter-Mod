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
    public class IntermediateSniperStats : SniperStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "ExplosiveRounds")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                            var blast = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                            bomb.name = "ExplosiveRounds_";
                            bomb.projectile.GetDamageModel().damage = augment.StackIndex;
                            bomb.projectile.pierce = 8;

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.NightVisionGoggles))
                            {
                                bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(blast);
                        }
                    }
                }

                if (augment.Name == "ShrapnelDart")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.ShrapnelShot))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var dart = Game.instance.model.GetTowerFromId("MonkeySub").GetAttackModel().weapons[0].projectile.Duplicate();
                            dart.pierce = 2;
                            dart.GetDamageModel().damage = augment.StackIndex;
                            dart.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new EmitOnDamageModel("ShrapnelDart_", new SingleEmissionModel("", null), dart));
                        }
                    }
                }

                if (augment.Name == "DobuleTap")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.EvenFasterFiring))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var snipe = Game.instance.model.GetTowerFromId("SniperMonkey").GetAttackModel().Duplicate();
                            snipe.weapons[0].projectile.GetDamageModel().damage = (1 + augment.StackIndex);
                            snipe.weapons[0].rate = towerModel.GetAttackModel(0).weapons[0].rate;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.NightVisionGoggles))
                            {
                                snipe.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.AddBehavior(snipe);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedSniperStats : SniperStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Bloonzooka")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.MaimMOAB))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var homing = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
                            homing.distance = 999;
                            homing.constantlyAquireNewTarget = false;

                            var rocket = Game.instance.model.GetTowerFromId("BombShooter-120").GetAttackModel().weapons[0].Duplicate();
                            rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().name = "Bloonzooka_";
                            rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 4 + (2 * augment.StackIndex);
                            rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            rocket.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
                            rocket.projectile.GetBehavior<TravelStraitModel>().Speed /= 1.45f;
                            rocket.projectile.AddBehavior(homing);
                            rocket.projectile.display = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.display;

                            var nail = Game.instance.model.GetTowerFromId("EngineerMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                            nail.GetDamageModel().damage = 1 + augment.StackIndex;
                            nail.pierce = 3;
                            rocket.projectile.AddBehavior(new CreateProjectileOnContactModel("Nail_", nail, new ArcEmissionModel("", 12, 0, 360, null, true, false), false, false, false));

                            var bloonzooka = Game.instance.model.GetTowerFromId("SniperMonkey").GetAttackModel().Duplicate();
                            bloonzooka.weapons[0] = rocket;
                            bloonzooka.name = "Bloonzooka_";
                            bloonzooka.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate * 3;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.NightVisionGoggles))
                            {
                                bloonzooka.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                                bloonzooka.weapons[0].projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.AddBehavior(bloonzooka);
                        }
                    }
                }
                if (augment.Name == "BouncierBullets")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BouncingBullet))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.maxPierce += (1 + augment.StackIndex);
                            towerModel.GetAttackModel().weapons[0].projectile.pierce += (1 + augment.StackIndex);
                        }
                    }
                }
                if (augment.Name == "CryoBullets")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SemiAutomatic))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            if (towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket)) { }
                            else { towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Lead; }
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new FreezeModel("CryoBullets_", 0, (0.4f + (0.1f * augment.StackIndex)), "CryoBullet", 3, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, (0.10f + (0.15f * augment.StackIndex)), true, false, false));
                            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                        }
                        if (augment.StackIndex >= 6)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FreezeModel>().canFreezeMoabs = true;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasterySniperStats : SniperStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {

            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
