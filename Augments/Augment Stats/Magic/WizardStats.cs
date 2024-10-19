using AugmentSentries;
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

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateWizardStats : WizardStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "FireElementals")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.WallOfFire))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            foreach (var behavior in Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetAttackModels().ToArray())
                            {
                                if (behavior.name.Contains("Spawner"))
                                {
                                    var spawner = behavior.Duplicate();
                                    spawner.range = towerModel.range;
                                    spawner.name = "FireElementals_";
                                    spawner.weapons[0].rate *= 1.25f;
                                    spawner.weapons[0].projectile.RemoveBehavior<CreateTowerModel>();
                                    spawner.weapons[0].projectile.AddBehavior(new CreateTowerModel("SentryPlace", GetTowerModel<FireElemental>().Duplicate(), 0f, true, false, false, true, true));
                                    spawner.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
                                    spawner.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = augment.StackIndex;

                                    if (towerModel.appliedUpgrades.Contains(UpgradeType.MonkeySense))
                                    {
                                        spawner.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                                    }

                                    towerModel.AddBehavior(spawner);
                                }
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedWizardStats : WizardStat
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

    public class MasteryWizardStats : WizardStat
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
