using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Data.Gameplay.Mods;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using System.Diagnostics;
using Templates;
using AugmentsMod;
using JetBrains.Annotations;
using Il2CppAssets.Scripts.Unity.Towers.Weapons;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.Utils.ElasticSearch;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.PlacementBehaviors;
using UnityEngine.Playables;

namespace AugmentsMod.Augments
{/*
    public class DartSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Dart Speed";
        public override string Icon => VanillaSprites.DartMonkeyIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Dart Monkeys attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("DartMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class DartRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Dart Range";
        public override string Icon => VanillaSprites.DartMonkeyIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Dart Monkeys have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("DartMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class DartPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Dart Pierce";
        public override string Icon => VanillaSprites.DartMonkeyIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Dart Monkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("DartMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*
    public class BoomerangSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Boomerang Speed";
        public override string Icon => VanillaSprites.BoomerangMonkeyIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Boomerang Monkeys attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("BoomerangMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class BoomerangPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Boomerang Pierce";
        public override string Icon => VanillaSprites.BoomerangMonkeyIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Boomerang Monkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("BoomerangMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
                    {
                        if (augment.Name == "Splitrang" && augment.StackIndex >= 1)
                        {
                            towerModel.GetAttackModel().weapons[0].projectile.pierce -= 1;
                            foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                            {
                                if (behavior.name.Contains("Splitrang_"))
                                {
                                    behavior.projectile.pierce += 1;
                                }
                            }
                        }
                    }
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class BombSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Bomb Speed";
        public override string Icon => VanillaSprites.BombShooterIcon;
        public override string TowerType => "Bomb Shooter Augment";
        public override string AugmentDescription => "Bomb Shooters attack 7% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("BombShooter"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class BombRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Bomb Range";
        public override string Icon => VanillaSprites.BombShooterIcon;
        public override string TowerType => "Bomb Shooter Augment";
        public override string AugmentDescription => "Bomb Shooters have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("BombShooter"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/

    public class TackSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Tack Speed";
        public override string Icon => VanillaSprites.TackShooterIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Tack Shooters attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("TackShooter"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class TackRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Tack Range";
        public override string Icon => VanillaSprites.TackShooterIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Tack Shooters have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("TackShooter"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class TackPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Tack Pierce";
        public override string Icon => VanillaSprites.TackShooterIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Tack Shooters gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("TackShooter"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/

    public class IceSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Ice Speed";
        public override string Icon => VanillaSprites.IceMonkeyIcon;
        public override string TowerType => "Ice Monkey Augment";
        public override string AugmentDescription => "Ice Monkeys attack 7% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("IceMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class GlueSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Glue Speed";
        public override string Icon => VanillaSprites.GlueGunnerIcon;
        public override string TowerType => "Glue Gunner Augment";
        public override string AugmentDescription => "Glue Gunners attack 7% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("GlueGunner"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class GlueRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Glue Range";
        public override string Icon => VanillaSprites.GlueGunnerIcon;
        public override string TowerType => "Glue Gunner Augment";
        public override string AugmentDescription => "Glue Gunners have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("GlueGunner"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class SniperSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Sniper Speed";
        public override string Icon => VanillaSprites.SniperMonkeyIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Sniper Monkeys attack 10% faster";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("SniperMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.1f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class SniperDamage : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Sniper Damage";
        public override string Icon => VanillaSprites.SniperMonkeyIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Sniper Monkeys deal 1 additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("SniperMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*

    public class SubSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Sub Speed";
        public override string Icon => VanillaSprites.MonkeySubIcon;
        public override string TowerType => "Monkey Sub Augment";
        public override string AugmentDescription => "Monkey Subs attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeySub"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class SubRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Sub Range";
        public override string Icon => VanillaSprites.MonkeySubIcon;
        public override string TowerType => "Monkey Sub Augment";
        public override string AugmentDescription => "Monkey Subs have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeySub"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class SubPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Sub Pierce";
        public override string Icon => VanillaSprites.MonkeySubIcon;
        public override string TowerType => "Monkey Sub Augment";
        public override string AugmentDescription => "Monkey Subs gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeySub"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*

    public class BuccaneerSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Buccaneer Speed";
        public override string Icon => VanillaSprites.MonkeyBuccaneerIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Monkey Buccaneers attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeyBuccaneer"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        weaponModel.rate /= 1.05f;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class BuccaneerRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Buccaneer Range";
        public override string Icon => VanillaSprites.MonkeyBuccaneerIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Monkey Buccaneers have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeyBuccaneer"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class BuccaneerPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Buccaneer Pierce";
        public override string Icon => VanillaSprites.MonkeyBuccaneerIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Monkey Buccaneers gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeyBuccaneer"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        weaponModel.projectile.pierce += 1;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/

    public class AceSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Ace Speed";
        public override string Icon => VanillaSprites.MonkeyAceIcon;
        public override string TowerType => "Monkey Ace Augment";
        public override string AugmentDescription => "Monkey Aces attack 6% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeyAce"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class AcePierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Ace Pierce";
        public override string Icon => VanillaSprites.MonkeyAceIcon;
        public override string TowerType => "Monkey Ace Augment";
        public override string AugmentDescription => "Monkey Aces gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeyAce"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/

    public class HeliSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Heli Speed";
        public override string Icon => VanillaSprites.HeliPilotIcon;
        public override string TowerType => "Heli Pilot Augment";
        public override string AugmentDescription => "Heli Pilots attack 6% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("HeliPilot"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.06f;

                    if (towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                    {
                        towerModel.GetAttackModel().weapons[1].rate /= 1.06f;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class HeliPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Heli Pierce";
        public override string Icon => VanillaSprites.HeliPilotIcon;
        public override string TowerType => "Heli Pilot Augment";
        public override string AugmentDescription => "Heli Pilots gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("HeliPilot"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;

                    if (towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                    {
                        towerModel.GetAttackModel().weapons[1].projectile.pierce += 1;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class MortarSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Mortar Speed";
        public override string Icon => VanillaSprites.MortarMonkeyIcon;
        public override string TowerType => "Mortar Monkey Augment";
        public override string AugmentDescription => "Mortar Monkeys attack 6% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MortarMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.06f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class MortarRadius : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Mortar Radius";
        public override string Icon => VanillaSprites.MortarMonkeyIcon;
        public override string TowerType => "Mortar Monkey Augment";
        public override string AugmentDescription => "Mortar Monkeys have an 8% larger blast radius.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MortarMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.radius *= 1.08f;
                    //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().effectModel.scale *= 1.08f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*

    public class DartlingSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Dartling Speed";
        public override string Icon => VanillaSprites.DartlingGunnerIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Dartling Gunners attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("DartlingGunner"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class DartlingPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Dartling Pierce";
        public override string Icon => VanillaSprites.DartlingGunnerIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Dartling Gunners gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("DartlingGunner"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*
    public class WizardSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Wizard Speed";
        public override string Icon => VanillaSprites.WizardIcon;
        public override string TowerType => "Wizard Monkey Augment";
        public override string AugmentDescription => "Wizard Monkeys attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("WizardMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        weaponModel.rate /= 1.05f;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class WizardRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Wizard Range";
        public override string Icon => VanillaSprites.WizardIcon;
        public override string TowerType => "Wizard Monkey Augment";
        public override string AugmentDescription => "Wizard Monkeys have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("WizardMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class WizardPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Wizard Pierce";
        public override string Icon => VanillaSprites.WizardIcon;
        public override string TowerType => "Wizard Monkey Augment";
        public override string AugmentDescription => "Wizard Monkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("WizardMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        weaponModel.projectile.pierce += 1;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class SuperRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Super Range";
        public override string Icon => VanillaSprites.SuperMonkeyIcon;
        public override string TowerType => "Super Monkey Augment";
        public override string AugmentDescription => "Super Monkeys have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("SuperMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class SuperPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Super Pierce";
        public override string Icon => VanillaSprites.SuperMonkeyIcon;
        public override string TowerType => "Super Monkey Augment";
        public override string AugmentDescription => "Super Monkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("SuperMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class NinjaSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Ninja Speed";
        public override string Icon => VanillaSprites.NInjaMonkeyIcon;
        public override string TowerType => "Ninja Monkey Augment";
        public override string AugmentDescription => "Ninja Monkeys attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("NinjaMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class NinjaRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Ninja Range";
        public override string Icon => VanillaSprites.NInjaMonkeyIcon;
        public override string TowerType => "Ninja Monkey Augment";
        public override string AugmentDescription => "Ninja Monkeys have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("NinjaMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class NinjaPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Ninja Pierce";
        public override string Icon => VanillaSprites.NInjaMonkeyIcon;
        public override string TowerType => "Ninja Monkey Augment";
        public override string AugmentDescription => "Ninja Monkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("NinjaMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class AlchemistSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Alchemist Speed";
        public override string Icon => VanillaSprites.AlchemistIcon;
        public override string TowerType => "Alchemist Augment";
        public override string AugmentDescription => "Alchemists attack 7% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("Alchemist"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.07f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*
    public class DruidRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Druid Range";
        public override string Icon => VanillaSprites.DruidIcon;
        public override string TowerType => "Druid Augment";
        public override string AugmentDescription => "Druids have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("Druid"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class DruidSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Druid Speed";
        public override string Icon => VanillaSprites.DruidIcon;
        public override string TowerType => "Druid Augment";
        public override string AugmentDescription => "Druids attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("Druid"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class DruidPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Druid Pierce";
        public override string Icon => VanillaSprites.DruidIcon;
        public override string TowerType => "Druid Augment";
        public override string AugmentDescription => "Druids gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("Druid"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    */
    public class MermonkeySpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Mermonkey Speed";
        public override string Icon => VanillaSprites.MermonkeyIcon;
        public override string TowerType => "Mermonkey Augment";
        public override string AugmentDescription => "Mermonkeys attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("Mermonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        weaponModel.rate /= 1.05f;
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class MermonkeyPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Mermonkey Pierce";
        public override string Icon => VanillaSprites.MermonkeyIcon;
        public override string TowerType => "Mermonkey Augment";
        public override string AugmentDescription => "Mermonkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("Mermonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    */
    public class SpactorySpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Spactory Speed";
        public override string Icon => VanillaSprites.SpikeFactoryIcon;
        public override string TowerType => "Spike Factory Augment";
        public override string AugmentDescription => "Spike Factories attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("SpikeFactory"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }/*
    public class SpactoryPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Spactory Pierce";
        public override string Icon => VanillaSprites.SpikeFactoryIcon;
        public override string TowerType => "Spike Factory Augment";
        public override string AugmentDescription => "Spike Factories gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("SpikeFactory"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    */
    public class FarmIncome : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Farm Income";
        public override string Icon => VanillaSprites.BananaFarmIcon2;
        public override string TowerType => "Banana Farm Augment";
        public override string AugmentDescription => "Banana Farms produce 5% bonus income.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("BananaFarm"))
                {
                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "FarmIncome")
                        {
                            var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum /= (1 + 0.05f * (augment.StackIndex - 1));
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum /= (1 + 0.05f * (augment.StackIndex - 1));

                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= (1 + 0.05f * augment.StackIndex);
                            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= (1 + 0.05f * augment.StackIndex);
                            towers.UpdateRootModel(towerModel);
                        }
                    }
                }
            }
        }
    }

    public class VillageRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Village Range";
        public override string Icon => VanillaSprites.MonkeyVillageIcon;
        public override string TowerType => "Monkey Village Augment";
        public override string AugmentDescription => "Monkey Villages have 8% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("MonkeyVillage"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*
    public class EngineerSpeed : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Engineer Speed";
        public override string Icon => VanillaSprites.EngineerMonkeyicon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Egnineer Monkeys attack 5% faster.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("EngineerMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].rate /= 1.05f;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class EngineerRange : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Engineer Range";
        public override string Icon => VanillaSprites.EngineerMonkeyicon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Egnineer Monkeys have 5% increased range.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("EngineerMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.range *= 1.05f;
                    towerModel.GetAttackModel().range *= 1.05f;

                    foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
                    {
                        if (weaponModel.projectile.GetBehaviors<TravelStraitModel>() != null)
                        {
                            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.05f;
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class EngineerPierce : AugmentTemplate
    {
        public override int SandboxIndex => 1;
        public override Rarity AugmentRarity => Rarity.Basic;
        public override string AugmentName => "Engineer Pierce";
        public override string Icon => VanillaSprites.EngineerMonkeyicon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Egnineer Monkeys gain 1 pierce.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("EngineerMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class Basic
    {
        public static List<string> BasicAug = [];
        public static List<string> BasicImg = [];
    }
}