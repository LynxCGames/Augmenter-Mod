using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
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
    public class IntermediateIceStats : IceStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Crystalize")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var icicleOrbit = Game.instance.model.GetTowerFromId("BoomerangMonkey-500").GetBehavior<OrbitModel>().Duplicate();
                            icicleOrbit.projectile.display = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.display;
                            icicleOrbit.projectile.scale /= 2f;
                            icicleOrbit.count = 5;
                            icicleOrbit.range = 24;

                            var icicleDamage = Game.instance.model.GetTowerFromId("BoomerangMonkey-500").GetAttackModel(1).Duplicate();
                            icicleDamage.name = "Crystalize_";
                            icicleDamage.weapons[0].projectile.radius = 24;
                            icicleDamage.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                            icicleDamage.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            icicleDamage.weapons[0].projectile.GetDamageModel().damage = augment.StackIndex;
                            icicleDamage.weapons[0].projectile.pierce = 20;
                            icicleDamage.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate / 2.4f;

                            foreach (var behavior in icicleDamage.weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
                            {
                                icicleDamage.weapons[0].projectile.RemoveBehavior(behavior);
                            }

                            towerModel.AddBehavior(icicleOrbit);
                            towerModel.AddBehavior(icicleDamage);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedIceStats : IceStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "FrozenArtillery")
                {
                    if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.CryoCannon))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var mortar = Game.instance.model.GetTowerFromId("MortarMonkey-001").GetAttackModel().Duplicate();
                            mortar.name = "FrozenArtillery_";
                            mortar.RemoveBehavior<RotateToPointerModel>();
                            mortar.AddBehavior(new RotateToTargetModel("", true, false, true, 0, true, true));
                            mortar.weapons[0].projectile.scale -= 5f;
                            mortar.weapons[0].projectile.AddBehavior(new RotateModel("targetspin", -300.0f));
                            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = (1 + 2 * augment.StackIndex);
                            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
                            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new FreezeModel("FrozenArtillery_", 0f, 2f, "ArtilleryFreeze", 999, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, true, false));
                            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.collisionPasses = new int[] { 0, -1 };

                            if (tower.towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap))
                            {
                                mortar.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                            }

                            towerModel.AddBehavior(mortar);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryIceStats : IceStat
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
