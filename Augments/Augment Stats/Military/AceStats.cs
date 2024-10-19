using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
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
    public class IntermediateAceStats : AceStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "SpineappleBombs")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.ExplodingPineapple))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var spike = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                            spike.GetDamageModel().damage = 2;
                            spike.pierce = 3;

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
                            {
                                spike.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                            {
                                if (weaponModel.name.Contains("WeaponModel_PineappleWeapon"))
                                {
                                    spike.GetBehavior<TravelStraitModel>().lifespan /= 1.75f;
                                    var createProjectile = weaponModel.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                                    createProjectile.name = "SpineappleBombs_";
                                    createProjectile.projectile = spike;
                                    createProjectile.emission = new ArcEmissionModel("Spineapples_", (2 + 2 * augment.StackIndex), 0, 360, null, false, true);
                                    weaponModel.projectile.AddBehavior(createProjectile);
                                }
                            }
                        }
                    }
                }

                if (augment.Name == "RocketDarts")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SharperDarts))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                            var blast = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                            bomb.name = "RocketDarts_";
                            bomb.projectile.GetDamageModel().damage = (1 + augment.StackIndex);
                            bomb.projectile.pierce = 8;

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
                            {
                                bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(blast);
                            towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-030").GetAttackModel().weapons[0].projectile.display;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedAceStats : AceStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "DeployFlares")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.FighterPlane))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bomb = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.Duplicate();
                            var flareExplosion = bomb.GetBehavior<CreateProjectileOnContactModel>().projectile;
                            flareExplosion.GetDamageModel().damage = 0;
                            flareExplosion.collisionPasses = new int[] { 0, -1 };
                            flareExplosion.AddBehavior(new SlowModel("DeployFlares_", 0f, (0.25f + 0.25f * augment.StackIndex), "Flare:stun", 3, "Stun", true, false, null, false, false, false));
                            flareExplosion.AddBehavior(new WindModel("DeployFlares_", 15, 35, (0.25f + 0.5f * augment.StackIndex), false, null, 0, null, 1));

                            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
                            var flareEffect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
                            effect.effectModel = flareEffect;

                            var flares = Game.instance.model.GetTowerFromId("MonkeyAce").GetAttackModel().Duplicate();
                            flares.name = "DeployFlares_";
                            flares.weapons[0].GetDescendant<ArcEmissionModel>().count = 5;
                            flares.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
                            flares.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan /= 4;
                            flares.weapons[0].projectile.GetDamageModel().damage = 0;
                            flares.weapons[0].projectile.pierce = 999;
                            flares.weapons[0].projectile.AddBehavior(effect);
                            flares.weapons[0].projectile.AddBehavior(new CreateProjectileOnExpireModel("DeployFlares_", flareExplosion, new SingleEmissionModel("", null), false));

                            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                            flares.weapons[0].projectile.AddBehavior(sound);

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
                            {
                                flareExplosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.AddBehavior(flares);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryAceStats : AceStat
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
