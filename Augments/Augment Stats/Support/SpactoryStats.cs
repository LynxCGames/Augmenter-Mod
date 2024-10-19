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
    public class IntermediateSpactoryStats : SpactoryStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "HeatedSpikes")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.WhiteHotSpikes))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            foreach (var behavior in towerModel.GetAttackModels().ToArray())
                            {
                                behavior.weapons[0].projectile.GetDamageModel().damage += augment.StackIndex;
                            }
                        }
                    }
                }

                if (augment.Name == "SpikeSprayer")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.DirectedSpikes))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.WhiteHotSpikes))
                            {
                                var spikes = Game.instance.model.GetTowerFromId("SpikeFactory-220").GetAttackModel().Duplicate();
                                spikes.name = "SpikeSprayer_";
                                spikes.weapons[0].rate /= 2f;

                                int i = 0;
                                while (i < augment.StackIndex - 1)
                                {
                                    spikes.weapons[0].rate /= 1.15f;
                                    i++;
                                }

                                spikes.weapons[0].projectile.pierce = 3;
                                towerModel.AddBehavior(spikes);
                            }
                            else
                            {
                                var spikes = Game.instance.model.GetTowerFromId("SpikeFactory-020").GetAttackModel().Duplicate();
                                spikes.name = "SpikeSprayer_";
                                spikes.weapons[0].rate /= 2f;

                                int i = 0;
                                while (i < augment.StackIndex - 1)
                                {
                                    spikes.weapons[0].rate /= 1.15f;
                                    i++;
                                }

                                spikes.weapons[0].projectile.pierce = 3;
                                towerModel.AddBehavior(spikes);
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedSpactoryStats : SpactoryStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Minefield")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SpikedMines))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var bombs = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
                            bombs.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
                            bombs.pierce = 999;
                            bombs.GetBehavior<TravelStraitModel>().Lifespan = 0.1f;
                            bombs.GetDamageModel().damage = 0;
                            bombs.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            bombs.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().Duplicate();
                            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
                            var explosions = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
                            effect.effectModel = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
                            explosions.GetDamageModel().damage = (1 + 2 * augment.StackIndex);
                            bombs.AddBehavior(new CreateProjectileOnExpireModel("Minefield_", explosions, new SingleEmissionModel("", null), false));
                            bombs.AddBehavior(effect);
                            bombs.AddBehavior(sound);

                            var createExplosion = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                            createExplosion.name = "Minefield_";
                            createExplosion.projectile = bombs;
                            createExplosion.emission = new ArcEmissionModel("", 3, 0, 360, null, true, false);

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(createExplosion);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasterySpactoryStats : SpactoryStat
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
