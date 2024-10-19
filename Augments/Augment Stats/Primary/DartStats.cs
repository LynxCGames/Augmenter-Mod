using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
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
    public class IntermediateDartStats : DartStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "DartsNeverMiss")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SharpShots))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
                            seeking.distance = 999;
                            seeking.constantlyAquireNewTarget = true;
                            seeking.turnRate *= 2;

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(seeking);

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.SpikeOPult)) { }
                            else { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3f; }
                        }

                        if (augment.StackIndex > 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.pierce += (augment.StackIndex - 1);
                        }
                    }
                }

                if (augment.Name == "Enraged")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.VeryQuickShots))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.AddBehavior(new DamageBasedAttackSpeedModel("Enraged_", 10, 100, 0.05f, 10));
                        }

                        if (augment.StackIndex > 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += (augment.StackIndex - 1);
                        }
                    }
                }

                if (augment.Name == "CamoInstincts")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.EnhancedEyesight))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Camo", 1, augment.StackIndex, false, false) { name = "CamoInstincts_" });
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedDartStats : DartStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Juggerbomb")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SpikeOPult))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().Duplicate();
                            var bomb = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                            var spike = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                            bomb.name = "Juggerbomb_";
                            bomb.projectile.GetDamageModel().damage = (2 * augment.StackIndex);
                            bomb.projectile.pierce = 16;

                            spike.GetBehavior<TravelStraitModel>().lifespan /= 1.75f;
                            spike.GetDamageModel().damage = augment.StackIndex;
                            spike.pierce = 3;

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel
                                ("", spike, new ArcEmissionModel("", 8, 0, 360, null, true, false), bomb.fraction, bomb.durationfraction, true, false, true)
                            { name = "JuggerbombSpikes_" });
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(effect);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(sound);
                        }
                    }
                }

                if (augment.Name == "SuperCrits")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SharpShooter))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].GetBehavior<CritMultiplierModel>().damage += (10 + 10 * augment.StackIndex);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryDartStats : DartStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "CrossbowChampion")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.CrossbowMaster))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("aaa", 3, 0, 30, null, false, false);
                        }

                        if (augment.StackIndex > 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += (2 * (augment.StackIndex - 1));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
