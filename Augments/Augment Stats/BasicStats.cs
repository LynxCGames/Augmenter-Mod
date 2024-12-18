using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
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
    public class BasicDartStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("DartMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "DartSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "DartRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "DartPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicBoomerangStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("BoomerangMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "BoomerangSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "BoomerangPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                        foreach (var augmentStack in ModContent.GetContent<AugmentTemplate>().ToList())
                        {
                            if (augmentStack.Name == "Splitrang" && augmentStack.StackIndex >= 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.pierce -= augment.StackIndex;
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("Splitrang_"))
                                    {
                                        behavior.projectile.pierce += augment.StackIndex;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicBombStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("BombShooter"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "BombSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                            i++;
                        }
                    }

                    if (augment.Name == "BombRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicTackStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("TackShooter"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "TackSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "TackRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "TackPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicIceStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("IceMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "IceSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                            i++;
                        }
                    }

                    /*if (augment.Name == "IceRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }*/
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicGlueStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("GlueGunner"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "GlueSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                            i++;
                        }
                    }

                    if (augment.Name == "GlueRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicSniperStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("SniperMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "SniperSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.1f;
                            i++;
                        }
                    }

                    if (augment.Name == "SniperDamage")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicSubStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("MonkeySub"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "SubSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "SubRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "SubPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicBuccaneerStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("MonkeyBuccaneer"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "BuccaneerSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                            {
                                weaponModel.rate /= 1.05f;
                            }
                            i++;
                        }
                    }

                    if (augment.Name == "BuccaneerRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null && weaponModel.name.Contains("DepthCharges_"))
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "BuccaneerPierce")
                    {
                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            weaponModel.projectile.pierce += 1;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicAceStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("MonkeyAce"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "AceSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
                            i++;
                        }
                    }

                    if (augment.Name == "AcePierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicHeliStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("HeliPilot"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "HeliSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.06f;

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                            {
                                towerModel.GetAttackModel().weapons[1].rate /= 1.06f;
                            }

                            i++;
                        }
                    }

                    if (augment.Name == "HeliPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;

                        if (towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                        {
                            towerModel.GetAttackModel().weapons[1].projectile.pierce += augment.StackIndex;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicMortaStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("MortarMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "MortarSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
                            i++;
                        }
                    }

                    if (augment.Name == "MortarRadius")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.radius *= (1 + 0.08f * augment.StackIndex);
                        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.pierce += (2 * augment.StackIndex);
                        //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().effectModel.scale *= 1.08f;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicDartlingStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("DartlingGunner"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "DartlingSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "DartlingPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicWizardStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("WizardMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "WizardSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "WizardRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "WizardPierce")
                    {
                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            weaponModel.projectile.pierce += augment.StackIndex;
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicSuperStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("SuperMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "SuperRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "SuperPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicNinjaStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("NinjaMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "NinjaSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "NinjaRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "NinjaPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicAlchemistStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("Alchemist"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "AlchemistSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                            i++;
                        }
                    }

                    /*if (augment.Name == "AlchemistRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }*/
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicDruidStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("Druid"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "DruidSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "DruidRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "DruidPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicMermonkeyStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("Mermonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "MermonkeySpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                            {
                                weaponModel.rate /= 1.05f;
                            }
                            i++;
                        }
                    }

                    if (augment.Name == "MermonkeyPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicSpactoryStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("SpikeFactory"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "SpactorySpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    /*if (augment.Name == "SpactoryRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }*/

                    if (augment.Name == "SpactoryPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicFarmStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("BananaFarm"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "FarmIncome")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= (1 + 0.05f * augment.StackIndex);
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicVillageStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("MonkeyVillage"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "VillageRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class BasicEngineerStats : BasicStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            if (tower.towerModel.name.Contains("EngineerMonkey"))
            {
                foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                {
                    if (augment.Name == "EngineerSpeed")
                    {
                        int i = 0;
                        while (i < augment.StackIndex - 1)
                        {
                            towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                            i++;
                        }
                    }

                    if (augment.Name == "EngineerRange")
                    {
                        towerModel.range *= (1 + 0.05f * augment.StackIndex);
                        towerModel.GetAttackModel().range *= (1 + 0.05f * augment.StackIndex);

                        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                        {
                            if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                            {
                                weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= (1 + 0.05f * augment.StackIndex);
                            }
                        }
                    }

                    if (augment.Name == "EngineerPierce")
                    {
                        towerModel.GetAttackModel().weapons[0].projectile.pierce += augment.StackIndex;
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }
}
