using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
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
using static Il2CppSystem.Net.ServicePointManager;

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateSubStats : SubStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Torpedos")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.AirburstDarts))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bomb = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                            var blast = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                            bomb.name = "Torpedos_";
                            bomb.projectile.GetDamageModel().damage = (1 + augment.StackIndex);
                            bomb.projectile.pierce = 8;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedIntel))
                            {
                                bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(blast);
                            towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.display;
                            towerModel.GetAttackModel().weapons[0].projectile.scale /= 2;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedSubStats : SubStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "BetterArmorPierce")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.ArmorPiercingDarts))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("BetterArmorPierce_", "Fortified", 1, (2 * augment.StackIndex), false, false));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasterySubStats : SubStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "BattleCruiser")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SubCommander))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("", (2 + augment.StackIndex), 0, (6 + (3 * augment.StackIndex)), null, false, false);
                            towerModel.GetAttackModel().weapons[0].rate /= 1.2f;

                            int i = 0;
                            while (i < augment.StackIndex - 1)
                            {
                                towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                                i++;
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
