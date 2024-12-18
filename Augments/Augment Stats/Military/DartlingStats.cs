using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
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
    public class IntermediateDartlingStats : DartlingStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "LightningShock")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.LaserShock))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var lightning = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").projectile.Duplicate();
                            lightning.pierce = (2 + augment.StackIndex);
                            lightning.GetDamageModel().damage = augment.StackIndex;
                            lightning.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                            lightning.GetBehavior<LightningModel>().splitRange = 20;
                            lightning.GetBehavior<LightningModel>().splits = 1;

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedTargeting))
                            {
                                lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("LightningShock_", lightning, new ArcEmissionModel("", 1, 0, 0, null, false, false), true, false, true));
                        }
                    }
                }

                if (augment.Name == "PunchDarts")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.PowerfulDarts))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new WindModel("PunchDarts_", 1, (3 + 2 * augment.StackIndex), (0.3f + 0.05f * augment.StackIndex), false, null, 0, null, 1));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedDartlingStats : DartlingStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "RocketBarrage")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.RocketStorm))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount += augment.StackIndex;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.MAD))
                            {
                                towerModel.GetAttackModel().weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount = 1;
                                var rockets = Game.instance.model.GetTowerFromId("DartlingGunner-040").GetAttackModel().Duplicate();
                                rockets.name = "RocketBarrage_";
                                rockets.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate;
                                rockets.weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount = augment.StackIndex;
                                towerModel.AddBehavior(rockets);
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryDartlingStats : DartlingStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "RayofBoom")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.RayOfDoom))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var explosion = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                            var sound = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                            var effect = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                            explosion.GetDamageModel().damage = 5 + (5 * augment.StackIndex);
                            explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            explosion.pierce = 10;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedTargeting))
                            {
                                explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            foreach (var behavior in Game.instance.model.GetTowerFromId("GlueGunner-500").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                            {
                                if (behavior.name.Contains("GlueLinger"))
                                {
                                    var emit = behavior.Duplicate();
                                    emit.name = "RayofBoom_";
                                    emit.layers = 1;
                                    emit.lifespan = 999f;
                                    emit.overlayType = null;
                                    emit.GetBehavior<EmitOnDestroyModel>().projectile = explosion;
                                    emit.GetBehavior<EmitOnDestroyModel>().projectile.AddBehavior(sound);
                                    emit.GetBehavior<EmitOnDestroyModel>().projectile.AddBehavior(effect);
                                    towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(emit);
                                    towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
                                }
                            }
                        }
                    }
                }

                if (augment.Name == "NoBloonsLand")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BloonExclusionZone))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var missiles = Game.instance.model.GetTowerFromId("BombShooter-040").GetAttackModel().weapons[0].projectile.Duplicate();
                            missiles.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 5;
                            missiles.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, (5 + 10 * augment.StackIndex), false, false) { name = "MoabModifier_" });
                            missiles.GetBehavior<TravelStraitModel>().lifespan = Game.instance.model.GetTowerFromId("DartlingGunner-005").GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan * 2;
                            missiles.scale /= 2;

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.AdvancedTargeting))
                            {
                                missiles.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            foreach (var attack in towerModel.GetAttackModels())
                            {
                                var assassins = attack.Duplicate();
                                assassins.name = "NoBloonsLand_";
                                assassins.weapons[0].projectile = missiles;
                                assassins.weapons[0].GetDescendant<RandomEmissionModel>().count = 4;
                                towerModel.AddBehavior(assassins);
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
