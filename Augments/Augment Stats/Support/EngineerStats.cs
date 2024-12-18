using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Data.TrophyStore;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
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
using Il2CppAssets.Scripts.Data.Cosmetics.Pets;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateEngineerStats : EngineerStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "MoreSentries")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SentryGun))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            foreach (var behavior in towerModel.GetAttackModels().ToArray())
                            {
                                if (behavior.name.Contains("Spawner"))
                                {
                                    behavior.weapons[0].rate /= (1.15f + 0.1f * augment.StackIndex);
                                }
                            }
                        }
                    }
                }

                if (augment.Name == "NailShotgun")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.Pin))
                    {
                        if (augment.StackIndex == 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                        }

                        if (augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("", 5, 0, 0, 45, 0, null);
                        }

                        if (augment.StackIndex > 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += augment.StackIndex - 1;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedEngineerStats : EngineerStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "LaserCutter")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.Overclock))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var cutter = Game.instance.model.GetTowerFromId("BallOfLightTower").GetAttackModel().Duplicate();
                            cutter.weapons[0].projectile.GetDamageModel().damage = augment.StackIndex;
                            cutter.weapons[0].projectile.pierce = 1;
                            cutter.weapons[0].projectile.maxPierce = 1;
                            cutter.name = "LaserCutter_";
                            cutter.weapons[0].projectile.RemoveBehavior<DamageModifierForTagModel>();
                            cutter.weapons[0].rate *= 2f;
                            cutter.weapons[0].projectile.GetBehavior<DisplayModel>().positionOffset = new Il2CppAssets.Scripts.Simulation.SMath.Vector3(0, 0, 500);
                            cutter.weapons[0].animateOnMainAttack = false;
                            cutter.weapons[0].animation = 0;
                            cutter.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                            cutter.range = towerModel.GetAttackModel().range;
                            towerModel.AddBehavior(cutter);
                        }
                    }
                }

                if (augment.Name == "DemoCharges")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BloonTrap))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var charge = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().Duplicate();
                            charge.name = "DemoCharges_";
                            charge.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.hasDamageModifiers = true;
                            charge.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = (2 + augment.StackIndex);
                            charge.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("DemoCharges_", "Moab", 1, (3 + augment.StackIndex), false, false));
                            charge.weapons[0].rate = towerModel.GetAttackModel().range = towerModel.range;
                            charge.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate * 2.4f;
                            towerModel.AddBehavior(charge);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryEngineerStats : EngineerStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Destructobots")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.XXXLTrap))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var agemodel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
                            var summon = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
                            summon.name = "Destructobots_";
                            summon.weapons[0].projectile.name = "AttackModel_Summon3_";
                            summon.weapons[0].emission = new NecromancerEmissionModel("BaseDeploy_", 1, 1, 1, 1, 1, 1, 0, null, null, null, 1, 1, 1, 1, 2);
                            summon.weapons[0].rate = 16f;
                            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection = false;
                            summon.weapons[0].projectile.GetDamageModel().damage = 0;
                            summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            summon.weapons[0].projectile.pierce = 1;
                            summon.weapons[0].projectile.maxPierce = 1;
                            summon.range = 100000;
                            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames = 0;
                            summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 999999f;
                            summon.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
                            agemodel.lifespanFrames = 0;
                            agemodel.lifespan = 999999f;
                            agemodel.rounds = 9999;
                            summon.weapons[0].projectile.AddBehavior(agemodel);

                            foreach (var trophyStoreItem in GameData.Instance.trophyStoreItems.storeItems)
                            {
                                foreach (var trophyItemTypeData in trophyStoreItem.itemTypes.Where(data => data.itemType == TrophyItemType.TowerPet))
                                {
                                    if (trophyItemTypeData.itemTarget.IsType(out Pet pet))
                                    {
                                        if (pet.id.Contains("Engineer"))
                                        {
                                            summon.weapons[0].projectile.display = pet.display;
                                        }
                                    }
                                }
                            }

                            var explosion = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                            var sound = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                            var effect = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                            explosion.projectile.GetDamageModel().damage = 2 + (2 * augment.StackIndex);
                            explosion.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, (15 + (5 * augment.StackIndex)), false, false) { name = "MoabModifier_" });
                            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                            summon.weapons[0].projectile.AddBehavior(explosion);
                            summon.weapons[0].projectile.AddBehavior(sound);
                            summon.weapons[0].projectile.AddBehavior(effect);
                            summon.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                            int i = 0;
                            while (i < augment.StackIndex - 1)
                            {
                                summon.weapons[0].rate /= 1.1f;
                                i++;
                            }

                            towerModel.AddBehavior(summon);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
