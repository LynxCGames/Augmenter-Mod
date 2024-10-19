using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
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
    public class IntermediateBombStats : BombStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Ignition")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.MissileLauncher))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var ignite = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().Duplicate();
                            ignite.range = towerModel.range;
                            ignite.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate;
                            ignite.weapons[0].projectile.GetDamageModel().damage = (1 + augment.StackIndex);

                            towerModel.AddBehavior(ignite);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedBombStats : BombStat
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

    public class MasteryBombStats : BombStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "MissileSilo")
                {
                    if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.MOABEliminator))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var missiles = Game.instance.model.GetTowerFromId("Rosalia 3").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Duplicate();
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().maxProjectileCount = 6;
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().damage = (6 + 2 * augment.StackIndex);
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.pierce = 25;
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetBehavior<SlowModel>().multiplier = 0.75f;
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.RemoveBehavior(missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.GetBehavior<SlowModifierForTagModel>());
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.hasDamageModifiers = true;
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.AddBehavior(new DamageModifierForTagModel("MissileSilo_", "Ceramic", 1, (5 * (augment.StackIndex - 1)), false, false) { name = "CeramicModifier_" });
                            missiles.projectile.GetBehavior<CreateProjectilesInAreaModel>().projectileModel.AddBehavior(new DamageModifierForTagModel("MissileSilo_", "Moabs", 1, (5 * (augment.StackIndex - 1)), false, false) { name = "MoabModifier_" });
                            missiles.rate = 3;

                            var silo = towerModel.GetAttackModel().Duplicate();
                            silo.name = "MissileSilo_";
                            silo.weapons[0] = missiles;
                            silo.range = 999;
                            towerModel.AddBehavior(silo);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
