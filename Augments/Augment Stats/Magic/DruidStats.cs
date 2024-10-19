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
    public class IntermediateDruidStats : DruidStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "RoseThorn")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.HeartOfOak))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].GetDescendant<RandomEmissionModel>().count += augment.StackIndex;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedDruidStats : DruidStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "HarvestVine")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.JunglesBounty))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bananas = Game.instance.model.GetTowerFromId("BananaFarm").GetAttackModel().weapons[0].projectile.Duplicate();
                            bananas.GetBehavior<CashModel>().maximum = (5 + 5 * augment.StackIndex);
                            bananas.GetBehavior<CashModel>().minimum = (5 + 5 * augment.StackIndex);

                            foreach (var behavior in towerModel.GetAttackModels().ToArray())
                            {
                                if (behavior.name.Contains("JungleVine"))
                                {
                                    behavior.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("HarvestVine_", bananas, new SingleEmissionModel("", null), false, false, false));
                                }
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryDruidStats : DruidStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "HarvestVine")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.Superstorm))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bolt = Game.instance.model.GetTowerFromId("MonkeySub-030").GetAttackModel(1).Duplicate();
                            bolt.name = "Smite_";
                            bolt.weapons[0].rate = 4;
                            bolt.range = 999;
                            bolt.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
                            bolt.weapons[0].projectile.RemoveBehavior<CreateSoundOnProjectileExpireModel>();
                            bolt.weapons[0].projectile.RemoveBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>();

                            var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").projectile.Duplicate();
                            lightning.GetDamageModel().damage = 3 + (2 * augment.StackIndex);
                            bolt.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile = lightning;

                            int i = 0;
                            while (i < augment.StackIndex - 1)
                            {
                                bolt.weapons[0].rate /= 1.1f;
                                i++;
                            }

                            towerModel.AddBehavior(bolt);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
