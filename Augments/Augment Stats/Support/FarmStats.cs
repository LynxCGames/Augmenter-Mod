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
    public class IntermediateFarmStats : FarmStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "BountifulHarvest")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.GreaterProduction))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += (1 + augment.StackIndex);
                        }
                    }
                }

                if (augment.Name == "LifeFarm")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.ValuableBananas))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var lives = Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<BonusLivesPerRoundModel>().Duplicate();
                            lives.name = "LifeFarm_";
                            lives.amount = augment.StackIndex;
                            towerModel.AddBehavior(lives);
                        }
                    }
                }

                if (augment.Name == "RoundRobbing")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BananaSalvage))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var cash = Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<PerRoundCashBonusTowerModel>().Duplicate();
                            cash.name = "RoundRobbing_";
                            cash.cashPerRound = (25 + 25 * augment.StackIndex);
                            towerModel.AddBehavior(cash);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedFarmStats : FarmStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "BananaRepublic")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BananaPlantation))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= (1.1f + 0.1f * augment.StackIndex);
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= (1.1f + 0.1f * augment.StackIndex);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryFarmStats : FarmStat
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
