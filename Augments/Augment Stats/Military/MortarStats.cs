using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
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
    public class IntermediateMortaStats : MortarStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "FlameBurst")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BurnyStuff))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var fireball = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                            fireball.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
                            fireball.GetDamageModel().damage = augment.StackIndex;
                            fireball.pierce = 4;

                            var createProjectile = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                            createProjectile.name = "FlameBurst_";
                            createProjectile.projectile = fireball;
                            createProjectile.emission = new RandomArcEmissionModel("FireBurstEmission_", (3 + augment.StackIndex), 0, 0, 360, 0, null);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(createProjectile);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedMortaStats : MortarStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Aftershock")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.Shockwave))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bomb = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                            var mortar = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
                            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                            var explosion = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                            var bombEffect = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();

                            bombEffect.scale /= 2;
                            effect.effectModel = bombEffect;
                            explosion.radius /= 2;
                            explosion.pierce = 16;
                            explosion.GetDamageModel().damage = (2 * augment.StackIndex);

                            bomb.pierce = 9999;
                            bomb.GetDamageModel().damage = 0;
                            bomb.GetBehavior<TravelStraitModel>().Speed /= 1.75f;
                            bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            bomb.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
                            bomb.AddBehavior(new CreateProjectileOnExpireModel("ExpireExplosion", explosion, new ArcEmissionModel("", 1, 0, 0, null, true, false), false));
                            bomb.AddBehavior(effect);
                            bomb.AddBehavior(sound);

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("Aftershock_", bomb, new ArcEmissionModel("", 4, 0, 360, null, true, false), mortar.fraction, mortar.durationfraction, true, false, true));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryMortaStats : MortarStat
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
