using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
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
    public class IntermediateAlchemistStats : AlchemistStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "PotentAcid")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.PerishingPotions))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("PotentAcid_", "Acid_Bonus_Damage", 4f, augment.StackIndex, 999, true, false, false, "bleed"));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedAlchemistStats : AlchemistStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "MidasTouch")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.RubberToGold))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var goldField = Game.instance.model.GetTowerFromId("BoomerangMonkey-500").GetAttackModel(1).Duplicate();
                            goldField.name = "MidasTouch_";
                            goldField.weapons[0].projectile.GetDamageModel().damage = 0;
                            if (augment.StackIndex <= 29)
                            {
                                goldField.weapons[0].rate = (8.25f - 0.25f * augment.StackIndex);
                            }
                            else
                            {
                                goldField.weapons[0].rate = 1;
                            }
                            goldField.weapons[0].projectile.pierce = (4 + 1 * augment.StackIndex);
                            goldField.weapons[0].projectile.collisionPasses = new[] { -1, 0 };
                            goldField.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            goldField.weapons[0].projectile.radius = towerModel.range;
                            goldField.fireWithoutTarget = true;

                            var goldSound = Game.instance.model.GetTowerFromId("Alchemist-004").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                            var goldWorth = Game.instance.model.GetTowerFromId("Alchemist-004").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<IncreaseBloonWorthModel>().Duplicate();
                            goldField.weapons[0].projectile.AddBehavior(goldSound);
                            goldField.weapons[0].projectile.AddBehavior(goldWorth);
                            towerModel.AddBehavior(goldField);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryAlchemistStats : AlchemistStat
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
