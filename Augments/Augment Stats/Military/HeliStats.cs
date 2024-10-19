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
    public class IntermediateHeliStats : HeliStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "HyperJets")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BiggerJets))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var movement = towerModel.GetBehavior<AirUnitModel>().GetBehavior<HeliMovementModel>();

                            movement.maxSpeed *= (1.1f + 0.1f * augment.StackIndex);
                            movement.movementForceStart *= (1.1f + 0.1f * augment.StackIndex);
                            movement.movementForceEnd *= (1.1f + 0.1f * augment.StackIndex);
                            movement.movementForceEndSquared = movement.movementForceEnd * movement.movementForceEnd;
                            movement.brakeForce *= (1.1f + 0.1f * augment.StackIndex);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedHeliStats : HeliStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "ClusterFire")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.ComancheDefense))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().weapons[0].projectile.display;
                            towerModel.GetAttackModel().weapons[0].emission = new RandomEmissionModel("", 5, 36, 0, null, true, 0.9f, 1.1f, 5, false);
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += augment.StackIndex;
                            towerModel.GetAttackModel().weapons[0].rate *= 1.5f;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                            {
                                towerModel.GetAttackModel().weapons[1].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().weapons[0].projectile.display;
                                towerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().damage += augment.StackIndex;
                                towerModel.GetAttackModel().weapons[1].rate *= 1.5f;
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryHeliStats : HeliStat
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
