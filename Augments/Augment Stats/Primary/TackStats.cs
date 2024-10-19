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
using static Il2CppSystem.Net.ServicePointManager;

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateTackStats : TackStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "StickyTacks")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.LongRangeTacks))
                    {
                        if (augment.StackIndex >= 1 && augment.StackIndex <= 9)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new SlowModel("StickyTacks_", (0.95f - 0.05f * augment.StackIndex), (0.75f + 0.25f * augment.StackIndex), "Ice:Slow", 3, "GlueBasic", true, false, null, false, false, false));
                        }
                        else if (augment.StackIndex > 13)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new SlowModel("StickyTacks_", 0.5f, (0.75f + 0.25f * augment.StackIndex), "Ice:Slow", 3, "GlueBasic", true, false, null, false, false, false));
                        }
                    }
                }

                if (augment.Name == "FlechetteTacks")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.EvenMoreTacks))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var tack = Game.instance.model.GetTowerFromId("TackShooter").GetAttackModel().weapons[0].projectile.Duplicate();
                            tack.GetDamageModel().damage = augment.StackIndex;

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("FlechetteTacks_", tack, new ArcEmissionModel("ArcEmissionModel_", 3, 0, 25, null, true, false), true, false, false));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedTackStats : TackStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "Ripper")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.BladeMaelstrom))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
                            seeking.distance = 999;
                            seeking.constantlyAquireNewTarget = true;
                            seeking.turnRate *= 2;

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(seeking);
                            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += augment.StackIndex;
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3f;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryTackStats : TackStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "ReaperShowtime")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.InfernoRing))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var meteors = Game.instance.model.GetTowerFromId("MortarMonkey-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].Duplicate();
                            meteors.rate = 3;
                            meteors.emission = new RandomTargetSpreadModel("Spread", 100, null, null);
                            meteors.SetProjectile(Game.instance.model.GetTowerFromId("MonkeySub-030").GetAttackModel(1).weapons[0].projectile.Duplicate());
                            meteors.projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
                            meteors.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = (4 + 4 * augment.StackIndex);
                            meteors.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.radius = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius * 1.5f;
                            meteors.projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().effectModel = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
                            meteors.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                            int i = 0;
                            while (i < augment.StackIndex - 1)
                            {
                                meteors.rate /= 1.15f;
                                i++;
                            }

                            var meteroRain = towerModel.GetAttackModel().Duplicate();
                            meteroRain.name = "ReaperShowtime_";
                            meteroRain.weapons[0] = meteors;
                            meteroRain.range = 999;
                            meteroRain.attackThroughWalls = true;
                            meteroRain.fireWithoutTarget = true;
                            towerModel.AddBehavior(meteroRain);
                        }
                    }
                }

                if (augment.Name == "LandmineBlades")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.SuperMaelstrom))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var explosion = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                            var sound = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                            var effect = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                            explosion.projectile.GetDamageModel().damage = (2 + 2 * augment.StackIndex);
                            explosion.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                            foreach (var behavior in Game.instance.model.GetTowerFromId("GlueGunner-500").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                            {
                                if (behavior.name.Contains("GlueLinger"))
                                {
                                    var emit = behavior.Duplicate();
                                    emit.name = "LandmineBlades_";
                                    emit.layers = 1;
                                    emit.lifespan = 0.5f;
                                    emit.overlayType = null;
                                    var landmine = emit.GetBehavior<EmitOnDestroyModel>().projectile;
                                    landmine.pierce = 1;
                                    landmine.GetDamageModel().damage = 0;
                                    landmine.GetBehavior<AgeModel>().lifespan = 10;
                                    landmine.display = Game.instance.model.GetTowerFromId("Rosalia 10").GetAbility(2).GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.display;
                                    landmine.AddBehavior(explosion);
                                    landmine.AddBehavior(sound);
                                    landmine.AddBehavior(effect);
                                    towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(emit);
                                    towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
                                }
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
