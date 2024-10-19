using AlternatePaths.Displays.Projectiles;
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
    public class IntermediateBoomerangStats : BoomerangStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "RazorGlaives")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.Glaives))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += augment.StackIndex;
                        }
                    }
                }

                if (augment.Name == "StrongArm")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.FasterThrowing))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.pierce += (1 + augment.StackIndex);
                            foreach (var augmentStack in ModContent.GetContent<AugmentTemplate>().ToList())
                            {
                                if (augmentStack.Name == "Splitrang" && augmentStack.StackIndex >= 1)
                                {
                                    towerModel.GetAttackModel().weapons[0].projectile.pierce -= (1 + augment.StackIndex);
                                    foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                    {
                                        if (behavior.name.Contains("Splitrang_"))
                                        {
                                            behavior.projectile.pierce += (1 + augment.StackIndex);
                                            behavior.projectile.GetBehavior<FollowPathModel>().speed *= (1.1f + 0.1f * augment.StackIndex);
                                        }
                                    }
                                }
                            }

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.GlaiveRicochet))
                            { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= (1.1f + 0.1f * augment.StackIndex); }
                            else
                            { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FollowPathModel>().speed *= (1.1f + 0.1f * augment.StackIndex); }
                        }
                    }
                }

                if (augment.Name == "WhiteHotRangs")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.RedHotRangs))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.
                                GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
                            fire.lifespan = (3 + augment.StackIndex);
                            fire.GetBehavior<DamageOverTimeModel>().damage = augment.StackIndex;
                            fire.GetBehavior<DamageOverTimeModel>().interval = 1;

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
                            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedBoomerangStats : BoomerangStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "MegaGlaives")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.MOARGlaives))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var glaives = Game.instance.model.GetTowerFromId("BoomerangMonkey-300").GetAttackModel().Duplicate();
                            glaives.name = "MegaGlaives_";
                            glaives.weapons[0].projectile.GetDamageModel().damage = 1;
                            glaives.weapons[0].projectile.hasDamageModifiers = true;
                            glaives.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("MegaGlaives_", "Moab", 1, (7 + 7 * augment.StackIndex), false, false));
                            glaives.weapons[0].projectile.ApplyDisplay<MegaGlaive>();
                            glaives.weapons[0].projectile.scale *= 3;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.RedHotRangs))
                            {
                                glaives.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
                            }

                            towerModel.AddBehavior(glaives);
                        }
                    }
                }

                if (augment.Name == "Multirang")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BioncBoomerang))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("aaa", (1 + augment.StackIndex), 0, (5 + 10 * augment.StackIndex), null, false, false);
                        }
                    }
                }

                if (augment.Name == "Splitrang")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.MOABPress))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var rang = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                            towerModel.GetAttackModel().weapons[0].projectile.pierce = 1;

                            rang.AddBehavior(new DamageModifierForTagModel("Splitrang_", "Moabs", 1, (4 * augment.StackIndex - 2), false, false));
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("Splitrang_", rang, new ArcEmissionModel("", 4, 0, 360, null, false, false), true, false, true));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryBoomerangStats : BoomerangStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "GlaiveDanger")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.GlaiveLord))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var radial = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().Duplicate();
                            radial.name = "GlaiveDanger_";
                            radial.fireWithoutTarget = true;
                            radial.weapons[0].projectile.GetDamageModel().damage = (2 + 2 * augment.StackIndex);
                            radial.weapons[0].projectile.pierce = 10;
                            radial.weapons[0].projectile.ApplyDisplay<ManiaGlaive>();
                            radial.weapons[0].projectile.scale *= 1.5f;
                            radial.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
                            radial.weapons[0].projectile.AddBehavior(new RotateModel("RotateModel_", -1440));
                            radial.weapons[0].GetDescendant<ArcEmissionModel>().count = (7 + augment.StackIndex);

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.RedHotRangs))
                            {
                                radial.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
                            }

                            towerModel.AddBehavior(radial);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
