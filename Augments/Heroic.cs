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
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2Cpp;
using AlternatePaths.Displays.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity.Powers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Unity.CollectionEvent;
using AugmentSentries;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Octokit;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using BTD_Mod_Helper.Api.Components;
using HarmonyLib;
using UnityEngine.Playables;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using System.Diagnostics.Metrics;
using Il2CppAssets.Scripts.Data.TrophyStore;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.Cosmetics.Pets;
using BTD_Mod_Helper.Api.Display;

namespace AugmentsMod.Augments
{
    public class DartParagon : AugmentTemplate
    {
        public override int SandboxIndex => 5;
        public override Rarity AugmentRarity => Rarity.Heroic;
        public override string AugmentName => "Dart Paragon";
        public override string Icon => VanillaSprites.ApexPlasmaMasterUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "test description";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.DartmonkeyParagon))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "")
                        {
                            if (augment.StackIndex == 1)
                            {

                            }

                            if (augment.StackIndex > 1)
                            {

                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class AceParagon : AugmentTemplate
    {
        public override int SandboxIndex => 5;
        public override Rarity AugmentRarity => Rarity.Heroic;
        public override string AugmentName => "Ace Paragon";
        public override string Icon => VanillaSprites.GoliathDoomshipUpgradeIcon;
        public override string TowerType => "Monkey Ace Augment";
        public override string AugmentDescription => "test description";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MonkeyaceParagon))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "")
                        {
                            if (augment.StackIndex == 1)
                            {

                            }

                            if (augment.StackIndex > 1)
                            {

                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class NinjaParagon : AugmentTemplate
    {
        public override int SandboxIndex => 5;
        public override Rarity AugmentRarity => Rarity.Heroic;
        public override string AugmentName => "Ninja Paragon";
        public override string Icon => VanillaSprites.AscendedShadowUpgradeIcon;
        public override string TowerType => "Ninja Monkey Augment";
        public override string AugmentDescription => "test description";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.NinjamonkeyParagon))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "")
                        {
                            if (augment.StackIndex == 1)
                            {

                            }

                            if (augment.StackIndex > 1)
                            {

                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class EngineerParagon : AugmentTemplate
    {
        public override int SandboxIndex => 5;
        public override Rarity AugmentRarity => Rarity.Heroic;
        public override string AugmentName => "Engineer Paragon";
        public override string Icon => VanillaSprites.MasterBuilderUpgradeIcon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "test description";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.EngineermonkeyParagon))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "")
                        {
                            if (augment.StackIndex == 1)
                            {

                            }

                            if (augment.StackIndex > 1)
                            {

                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class Heroic
    {
        public static List<string> HeroicAug = [];
        public static List<string> HeroicImg = [];
    }
}