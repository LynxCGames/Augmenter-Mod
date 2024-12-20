﻿using AugmentsMod;
using AugmentsMod.Augments;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppNinjaKiwi.Common;
using Il2CppSystem.Collections.Generic;
using Il2CppTMPro;
using MelonLoader;
using System.Linq;
using Templates;
using UnityEngine;
using HarmonyLib;
using UnityEngine.UIElements;
using Il2CppAssets.Scripts.Unity.Gamepad;
//using Il2CppAssets.Scripts.Data.Quests.QuestDialogue;
using Il2Cpp;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Newtonsoft.Json;
using Il2CppAssets.Scripts.Simulation.Track;
using Augmenter;

[assembly: MelonInfo(typeof(AugmentsMod.Augmenter), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AugmentsMod;


public class Augmenter : BloonsTD6Mod
{
    internal static readonly ModSettingBool SandboxMode = new(false)
    {
        //requiresRestart = true,
        icon = VanillaSprites.SandboxBtn,
        description = "Enable Sandbox Mode"
    };


    public static Augmenter mod;
    public bool isSelected = false;
    public float basicCost = 500;
    public float intermediateCost = 2500;
    public float advancedCost = 6000;
    public float masteryCost = 14000;
    public float levelXp = 0;
    public int XPMax = 10;
    public bool activeBoon = false;
    public string boon = " ";
    //public float newAugmentCost = 100;
    //public float baseNewAugmentCost = 100;
    public float intermediateChance = 85;
    public float advancedChance = 100;
    public float masteryChance = 100;
    public bool selectingAugmentOpen = false;
    public bool panelOpen = false;
    public bool infoOpen = false;
    public float level = 0;
    public int newAugmentSlot = 3;
    public AugmentTemplate.Rarity minNewAugmentRarity = AugmentTemplate.Rarity.Basic;
    public AugmentTemplate.Rarity maxNewAugmentRarity = AugmentTemplate.Rarity.Heroic;
    public AugmentTemplate.Rarity minStrongAugmentRarity = AugmentTemplate.Rarity.Basic;
    public AugmentTemplate.Rarity maxStrongAugmentRarity = AugmentTemplate.Rarity.Heroic;


    public float questLevel = 0;
    public float transmitLevel = 0;
    public bool transmitOpen = false;
    public bool questOpen = false;
    public float questXp = 5;
    public int questXPMax = 5;


    public override void OnTitleScreen()
    {
        /*foreach (var trophyStoreItem in GameData.Instance.trophyStoreItems.storeItems)
        {
            foreach (var trophyItemTypeData in trophyStoreItem.itemTypes.Where(data => data.itemType == TrophyItemType.TowerPet))
            {
                if (trophyItemTypeData.itemTarget.IsType(out Pet pet))
                {
                    //pet.skinId = "";
                    //ModHelper.Msg<Augmenter>(pet.id);
                    //ModHelper.Msg<Augmenter>(pet.display);
                }
            }
        }*/
    }

    public override void OnApplicationStart()
    {
        mod = this;

        foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
        {
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Basic)
            {
                Basic.BasicAug.Add(augment.AugmentName);
                Basic.BasicImg.Add(augment.Icon);
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Intermediate)
            {
                Intermediate.IntermediateAug.Add(augment.AugmentName);
                Intermediate.IntermediateImg.Add(augment.Icon);
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Advanced)
            {
                Advanced.AdvancedAug.Add(augment.AugmentName);
                Advanced.AdvancedImg.Add(augment.Icon);

            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Mastery)
            {
                Mastery.MasteryAug.Add(augment.AugmentName);
                Mastery.MasteryImg.Add(augment.Icon);
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Heroic)
            {
                Heroic.HeroicAug.Add(augment.AugmentName);
                Heroic.HeroicImg.Add(augment.Icon);
            }
        }
    }
    public void Reset()
    {
        intermediateChance = 85;
        advancedChance = 100;
        masteryChance = 100;
        levelXp = 0;
        XPMax = 10;
        isSelected = false;
        selectingAugmentOpen = false;
        panelOpen = false;
        level = 0;
        newAugmentSlot = 3;
        activeBoon = false;
        boon = " ";

        foreach (var augmentContent in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
        {
            augmentContent.StackIndex = 0;
        }

        foreach (var boon in ModContent.GetContent<BoonTemplate>().OrderByDescending(c => c.mod == mod))
        {
            boon.cost = 2000;
        }
    }
    public void QuestReset()
    {
        basicCost = 500;
        intermediateCost = 2500;
        advancedCost = 6000;
        masteryCost = 14000;

        questLevel = 0;
        transmitLevel = 0;
        transmitOpen = false;
        questOpen = false;
        questXp = 5;
        questXPMax = 5;
    }
    public override void OnTowerSaved(Tower tower, TowerSaveDataModel saveData)
    {
        if (tower.towerModel.GetModTower()?.GetType() == typeof(AugmenterTower))
        {
            saveData.metaData["Augments"] = string.Join('^', ModContent.GetContent<AugmentTemplate>().OrderBy(a => a.Id).Select(a => a.StackIndex));
            saveData.metaData["Boons"] = string.Join('^', ModContent.GetContent<BoonTemplate>().OrderBy(a => a.Id).Select(a => a.cost));
            saveData.metaData["Level"] = $"{mod.level}";
            saveData.metaData["Xp"] = $"{mod.levelXp}";
            saveData.metaData["MaxXp"] = $"{mod.XPMax}";
        }
    }
    public override void OnTowerLoaded(Tower tower, TowerSaveDataModel saveData)
    {
        if (tower.towerModel.GetModTower()?.GetType() == typeof(AugmenterTower))
        {
            string savedDataAug = saveData.metaData["Augments"];
            var augParts = savedDataAug.Split('^');
            var augments = ModContent.GetContent<AugmentTemplate>().OrderBy(a => a.Id).ToArray();

            for (var i = 0; i < augments.Length; i++)
            {
                ((AugmentTemplate)ModContent.GetInstance(augments[i].GetType())).StackIndex = int.Parse(augParts[i]);
            }

            string savedDataBoon = saveData.metaData["Boons"];
            var boonParts = savedDataBoon.Split('^');
            var boons = ModContent.GetContent<BoonTemplate>().OrderBy(a => a.Id).ToArray();

            for (var i = 0; i < boons.Length; i++)
            {
                ((BoonTemplate)ModContent.GetInstance(boons[i].GetType())).cost = int.Parse(boonParts[i]);
            }

            mod.level = int.Parse(saveData.metaData["Level"]);
            mod.levelXp = int.Parse(saveData.metaData["Xp"]);
            mod.XPMax = int.Parse(saveData.metaData["MaxXp"]);
        }
    }
    public override void OnGameModelLoaded(GameModel model)
    {
        Reset();
    }
    public override void OnNewGameModel(GameModel result)
    {
        foreach (var tower in result.towerSet)
        {
            if (tower.name.Contains("AugmenterMonkey"))
            {
                tower.GetShopTowerDetails().towerCount = 1;
            }
        }

        QuestReset();
    }
    public override void OnRoundStart()
    {
        /*if (InGame.instance.currentRoundId == 50)
        {
            //ModHelper.Msg<Augmenter>("It's round 50");
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 700, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi transmissionUi = panel.AddComponent<MenuUi>();
            ModHelperText transmitText = panel.AddText(new Info("transmitText", 0, 350, 1200, 180), "Incoming Transmission", 100);
            ModHelperText speakerText = panel.AddText(new Info("speakerText", 0, 150, 1200, 180), "Unknown Transmitter:", 80);
            ModHelperText transmission = panel.AddText(new Info("transmission", 0, -50, 1300, 800), "*<Stactic>* Hello? *<Static>* Can anyone hear me? *<Static>*", 60);

            ModHelperButton close = panel.AddButton(new Info("close", 0, -400, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => transmissionUi.CloseMessage()));
            ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
        }

        if (InGame.instance.currentRoundId == 70)
        {
            //ModHelper.Msg<Augmenter>("It's round 70");
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 700, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi transmissionUi = panel.AddComponent<MenuUi>();
            ModHelperText transmitText = panel.AddText(new Info("transmitText", 0, 350, 1200, 180), "Incoming Transmission", 100);
            ModHelperText speakerText = panel.AddText(new Info("speakerText", 0, 150, 1200, 180), "Unknown Transmitter:", 80);
            ModHelperText transmission = panel.AddText(new Info("transmission", 0, -75, 1300, 800), "*<Stactic>* He... *<Static>* llo? *<Static>* Nee... *<Static>* d... *<Static>* help. *<Static>* ple... *<Static>* respon *<Static>*", 60);

            ModHelperButton close = panel.AddButton(new Info("close", 0, -400, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => transmissionUi.CloseMessage()));
            ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);

            mod.questLevel = 1;
        }*/
    }
    public override void OnRestart()
    {
        MenuUi.instance.CloseMenu();
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.towerModel.name.Contains("AugmenterMonkey"))
        {
            Reset();
        }

        foreach (var augmentStat in ModContent.GetContent<BasicStat>().OrderByDescending(c => c.mod == mod))
        {
            augmentStat.EditTower(tower);
        }
    }
    public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
    {
        foreach (var augmentStat in ModContent.GetContent<BasicStat>().OrderByDescending(c => c.mod == mod))
        {
            augmentStat.EditTower(tower);
        }

        if (tower.towerModel.name.Contains("DartMonkey"))
        {
            foreach (var dartStat in ModContent.GetContent<DartStat>().OrderByDescending(c => c.mod == mod))
            {
                dartStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("BoomerangMonkey"))
        {
            foreach (var boomerangStat in ModContent.GetContent<BoomerangStat>().OrderByDescending(c => c.mod == mod))
            {
                boomerangStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("BombShooter"))
        {
            foreach (var bombStat in ModContent.GetContent<BombStat>().OrderByDescending(c => c.mod == mod))
            {
                bombStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("TackShooter"))
        {
            foreach (var tackStat in ModContent.GetContent<TackStat>().OrderByDescending(c => c.mod == mod))
            {
                tackStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("IceMonkey"))
        {
            foreach (var iceStat in ModContent.GetContent<IceStat>().OrderByDescending(c => c.mod == mod))
            {
                iceStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("GlueGunner"))
        {
            foreach (var glueStat in ModContent.GetContent<GlueStat>().OrderByDescending(c => c.mod == mod))
            {
                glueStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("SniperMonkey"))
        {
            foreach (var sniperStat in ModContent.GetContent<SniperStat>().OrderByDescending(c => c.mod == mod))
            {
                sniperStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("MonkeySub"))
        {
            foreach (var subStat in ModContent.GetContent<SubStat>().OrderByDescending(c => c.mod == mod))
            {
                subStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("MonkeyBuccaneer"))
        {
            foreach (var buccaneerStat in ModContent.GetContent<BuccaneerStat>().OrderByDescending(c => c.mod == mod))
            {
                buccaneerStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("MonkeyAce"))
        {
            foreach (var aceStat in ModContent.GetContent<AceStat>().OrderByDescending(c => c.mod == mod))
            {
                aceStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("HeliPilot"))
        {
            foreach (var heliStat in ModContent.GetContent<HeliStat>().OrderByDescending(c => c.mod == mod))
            {
                heliStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("MortarMonkey"))
        {
            foreach (var mortarStat in ModContent.GetContent<MortarStat>().OrderByDescending(c => c.mod == mod))
            {
                mortarStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("DartlingGunner"))
        {
            foreach (var dartlingStat in ModContent.GetContent<DartlingStat>().OrderByDescending(c => c.mod == mod))
            {
                dartlingStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("WizardMonkey"))
        {
            foreach (var wizardStat in ModContent.GetContent<WizardStat>().OrderByDescending(c => c.mod == mod))
            {
                wizardStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("SuperMonkey"))
        {
            foreach (var superStat in ModContent.GetContent<SuperStat>().OrderByDescending(c => c.mod == mod))
            {
                superStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("NinjaMonkey"))
        {
            foreach (var ninjaStat in ModContent.GetContent<NinjaStat>().OrderByDescending(c => c.mod == mod))
            {
                ninjaStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("Alchemist"))
        {
            foreach (var alchemistStat in ModContent.GetContent<AlchemistStat>().OrderByDescending(c => c.mod == mod))
            {
                alchemistStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("Druid"))
        {
            foreach (var druidStat in ModContent.GetContent<DruidStat>().OrderByDescending(c => c.mod == mod))
            {
                druidStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("Mermonkey"))
        {
            foreach (var mermonkeyStat in ModContent.GetContent<MermonkeyStat>().OrderByDescending(c => c.mod == mod))
            {
                mermonkeyStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("SpikeFactory"))
        {
            foreach (var spactoryStat in ModContent.GetContent<SpactoryStat>().OrderByDescending(c => c.mod == mod))
            {
                spactoryStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("BananaFarm"))
        {
            foreach (var farmStat in ModContent.GetContent<FarmStat>().OrderByDescending(c => c.mod == mod))
            {
                farmStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("MonkeyVillage"))
        {
            foreach (var villageStat in ModContent.GetContent<VillageStat>().OrderByDescending(c => c.mod == mod))
            {
                villageStat.EditTower(tower);
            }
        }
        if (tower.towerModel.name.Contains("EngineerMonkey"))
        {
            foreach (var engineerStat in ModContent.GetContent<EngineerStat>().OrderByDescending(c => c.mod == mod))
            {
                engineerStat.EditTower(tower);
            }
        }
    }
    public override void OnTowerSelected(Tower tower)
    {
        if (tower.towerModel.name.Contains("AugmenterMonkey"))
        {
            if (SandboxMode)
            {
                mod.level = 4;
            }

            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            MenuUi.CreateUpgradeMenu(rect, tower);
            isSelected = true;
        }
    }
    public override void OnTowerDeselected(Tower tower)
    {
        if (tower.towerModel.name.Contains("AugmenterMonkey"))
        {
            isSelected = false;
            if (MenuUi.instance)
            {
                MenuUi.instance.CloseMenu();
            }

        }
    }
    [RegisterTypeInIl2Cpp(false)]
    public class MenuUi : MonoBehaviour
    {
        public static MenuUi instance;

        public ModHelperInputField input;
        public void CloseMenu()
        {
            if (gameObject)
            {
                Destroy(gameObject);
            }

        }


        // #### Basic Augments ####

        public static void NewBasicAugmentPanel(RectTransform rect, Tower tower)
        {
            mod.panelOpen = true;
            instance.CloseMenu();
            if (SandboxMode)
            {
                BasicSandBoxAugmentPanel(rect, tower);
                return;
            }
            float augmentPanelWidth = 833.33f;
            float augmentPanelX = 412.5f;
            float augmentPanelY = 900;
            float augContentX = 25 - (mod.newAugmentSlot - 1) * 425;
            float panelWidth = mod.newAugmentSlot * augmentPanelWidth;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, panelWidth, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlueNotchesShadow);

            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText selectAug = panel.AddText(new Info("selectAug", 0, 800, 2500, 180), "Select New Augment", 100);
            Il2CppSystem.Random rnd = new();
            for (int i = 0; i < mod.newAugmentSlot; i++)
            {
                var AugRarityNum = rnd.Next(1, 100);
                var AugRarity = "Basic";
                /*var RarityNumber = 1;
                var MinNum = 1;
                var MaxNum = 1;
                /*if (mod.minNewAugmentRarity == AugmentTemplate.Rarity.Basic)
                {
                    MinNum = 1;
                }
                if (mod.minNewAugmentRarity == AugmentTemplate.Rarity.Intermediate)
                {
                    MinNum = 2;
                }
                if (mod.minNewAugmentRarity == AugmentTemplate.Rarity.Advanced)
                {
                    MinNum = 3;
                }
                if (mod.minNewAugmentRarity == AugmentTemplate.Rarity.Mastery)
                {
                    MinNum = 4;
                }
                if (mod.maxNewAugmentRarity == AugmentTemplate.Rarity.Basic)
                {
                    MaxNum = 1;
                }
                if (mod.maxNewAugmentRarity == AugmentTemplate.Rarity.Intermediate)
                {
                    MaxNum = 2;
                }
                if (mod.maxNewAugmentRarity == AugmentTemplate.Rarity.Advanced)
                {
                    MaxNum = 3;
                }
                if (mod.maxNewAugmentRarity == AugmentTemplate.Rarity.Mastery)
                {
                    MaxNum = 4;
                }
                if (AugRarityNum >= mod.intermediateChance)
                {
                    RarityNumber = 2;
                }
                if (AugRarityNum >= mod.advancedChance)
                {
                    RarityNumber = 3;
                }
                if (AugRarityNum >= mod.masteryChance)
                {
                    RarityNumber = 4;
                }
                if (RarityNumber < MinNum)
                {
                    RarityNumber = MinNum;
                }
                if (RarityNumber > MaxNum)
                {
                    RarityNumber = MaxNum;
                }
                if (RarityNumber == 2)
                {
                    AugRarity = "Intermediate";
                }
                if (RarityNumber == 3)
                {
                    AugRarity = "Advanced";
                }
                if (RarityNumber == 4)
                {
                    AugRarity = "Mastery";
                }*/

                var sprite = VanillaSprites.MainBgPanelJukebox;
                var augment = "";
                var img = "";
                List<string> augmentName = new();
                List<string> augmentImage = new();

                if (mod.activeBoon == true)
                {
                    foreach (var augmentBoon in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augmentBoon.TowerType.Contains(mod.boon) && augmentBoon.SandboxIndex == 1)
                        {
                            augmentName.Add(augmentBoon.AugmentName);
                            augmentImage.Add(augmentBoon.Icon);
                        }
                    }

                    var numAug = rnd.Next(0, augmentName.Count);
                    augment = augmentName[numAug];
                    img = augmentImage[numAug];
                }
                else
                {
                    var numAug = rnd.Next(0, Basic.BasicAug.Count);
                    augment = Basic.BasicAug[numAug];
                    img = Basic.BasicImg[numAug];
                }
                /*if (AugRarity == "Intermediate")
                {
                    numAug = rnd.Next(0, Intermediate.IntermediateAug.Count);
                    sprite = VanillaSprites.MainBGPanelBronze;
                    augment = Intermediate.IntermediateAug[numAug];
                    img = Intermediate.IntermediateImg[numAug];
                }
                if (AugRarity == "Advanced")
                {
                    numAug = rnd.Next(0, Advanced.AdvancedAug.Count);
                    sprite = VanillaSprites.MainBGPanelSilver;
                    augment = Advanced.AdvancedAug[numAug];
                    img = Advanced.AdvancedImg[numAug];
                }
                /*if (AugRarity == "Mastery")
                {
                    numAug = rnd.Next(0, Mastery.MasteryAug.Count);
                    sprite = VanillaSprites.MainBgPanelHematite;
                    augment = Mastery.MasteryAug[numAug];
                    img = Mastery.MasteryImg[numAug];
                }*/
                /*if (AugRarity == "Heroic")
                {
                    numAug = rnd.Next(0, Heroic.HeroicAug.Count);
                    sprite = VanillaSprites.PortraitContainerParagon;
                    augment = Heroic.HeroicAug[numAug];
                    img = Heroic.HeroicImg[numAug];
                }*/
                ModHelperPanel augPanel = panel.AddPanel(new Info("augPanel", augmentPanelX, augmentPanelY, 650, 1450, new UnityEngine.Vector2()), sprite);
                ModHelperText rarityText = panel.AddText(new Info("rarityText", augContentX, 600, 800, 180), AugRarity, 100);
                ModHelperText augmentText = panel.AddText(new Info("augmentText", augContentX, 500, 800, 180), augment, 75);
                ModHelperImage image = panel.AddImage(new Info("image", augContentX, 150, 400, 400), img);
                ModHelperButton selectAugBtn = panel.AddButton(new Info("selectAugBtn", augContentX, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.AugmentSelected(augment, tower)));
                ModHelperText selectAugTxt = selectAugBtn.AddText(new Info("selectWpnTxt", 0, 0, 700, 160), "Select", 70);
                foreach (var augmentContent in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                {
                    if (augmentContent.AugmentName == augment)
                    {
                        ModHelperText StackIndex = panel.AddText(new Info("StackIndex", augContentX + 275, 650, 100, 100), $"{augmentContent.StackIndex}", 80);
                        ModHelperText towerType = panel.AddText(new Info("towerType", augContentX, 425, 1000, 150), augmentContent.TowerType, 40);
                        ModHelperText augmentDescription = panel.AddText(new Info("augmentDescription", augContentX, -300, 700, 400), augmentContent.AugmentDescription, 40);
                    }
                }
                augmentPanelX += augmentPanelWidth;
                augContentX += augmentPanelWidth;
            }
        }

        public void NewBasicAugment(Tower tower)
        {
            InGame game = InGame.instance;
            if (SandboxMode)
            {
                RectTransform rect = game.uiRect;
                MenuUi.NewBasicAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
                return;
            }
            if (game.GetCash() >= mod.basicCost)
            {
                if (mod.level < 4)
                {
                    mod.levelXp += 1;
                }

                if (mod.questLevel == 1 || mod.questLevel == 2 || mod.questLevel == 5 && mod.transmitLevel < mod.questLevel)
                {
                    mod.questXp += 1;
                }

                game.AddCash(-mod.basicCost);
                RectTransform rect = game.uiRect;
                tower.worth += mod.basicCost;
                MenuUi.NewBasicAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
            }
        }

        public static void BasicSandBoxAugmentPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();
            ModHelperScrollPanel scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 0, 0, 2500, 1850), RectTransform.Axis.Vertical, VanillaSprites.MainBGPanelBlue, 15, 50);
            ModHelperButton exit = panel.AddButton(new Info("exit", 1200, 900, 135), VanillaSprites.RedBtn, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false); panel.DeleteObject(); mod.selectingAugmentOpen = false; mod.panelOpen = false; /*if (mod.upgradeOpen == true) {*/ CreateUpgradeMenu(rect, tower); //}
            }));
            ModHelperText x = exit.AddText(new Info("x", 0, 0, 130), "X", 80);

            foreach (var augment in ModContent.GetContent<AugmentTemplate>())
            {
                if (augment.SandboxIndex == 1)
                {
                    scrollPanel.AddScrollContent(CreateAugment(augment, tower));
                }
            }
        }


        // #### Intermediate Augments ####

        public static void NewIntermediateAugmentPanel(RectTransform rect, Tower tower)
        {
            mod.panelOpen = true;
            instance.CloseMenu();
            if (SandboxMode)
            {
                IntermediateSandBoxAugmentPanel(rect, tower);
                return;
            }
            float augmentPanelWidth = 833.33f;
            float augmentPanelX = 412.5f;
            float augmentPanelY = 900;
            float augContentX = 25 - (mod.newAugmentSlot - 1) * 425;
            float panelWidth = mod.newAugmentSlot * augmentPanelWidth;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, panelWidth, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlueNotchesShadow);

            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText selectAug = panel.AddText(new Info("selectAug", 0, 800, 2500, 180), "Select New Augment", 100);
            Il2CppSystem.Random rnd = new();
            for (int i = 0; i < mod.newAugmentSlot; i++)
            {
                var AugRarityNum = rnd.Next(1, 100);
                var AugRarity = "Intermediate";

                var sprite = VanillaSprites.MainBGPanelBronze;
                var augment = "";
                var img = "";
                List<string> augmentName = new();
                List<string> augmentImage = new();

                if (mod.activeBoon == true)
                {
                    foreach (var augmentBoon in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augmentBoon.TowerType.Contains(mod.boon) && augmentBoon.SandboxIndex == 2)
                        {
                            augmentName.Add(augmentBoon.AugmentName);
                            augmentImage.Add(augmentBoon.Icon);
                        }
                    }

                    var numAug = rnd.Next(0, augmentName.Count);
                    augment = augmentName[numAug];
                    img = augmentImage[numAug];
                }
                else
                {
                    var numAug = rnd.Next(0, Intermediate.IntermediateAug.Count);
                    augment = Intermediate.IntermediateAug[numAug];
                    img = Intermediate.IntermediateImg[numAug];
                }

                ModHelperPanel augPanel = panel.AddPanel(new Info("augPanel", augmentPanelX, augmentPanelY, 650, 1450, new UnityEngine.Vector2()), sprite);
                ModHelperText rarityText = panel.AddText(new Info("rarityText", augContentX, 600, 800, 180), AugRarity, 100);
                ModHelperText augmentText = panel.AddText(new Info("augmentText", augContentX, 500, 800, 180), augment, 75);
                ModHelperImage image = panel.AddImage(new Info("image", augContentX, 150, 400, 400), img);
                ModHelperButton selectAugBtn = panel.AddButton(new Info("selectAugBtn", augContentX, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.AugmentSelected(augment, tower)));
                ModHelperText selectAugTxt = selectAugBtn.AddText(new Info("selectWpnTxt", 0, 0, 700, 160), "Select", 70);

                foreach (var augmentContent in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                {
                    if (augmentContent.AugmentName == augment)
                    {
                        ModHelperText StackIndex = panel.AddText(new Info("StackIndex", augContentX + 275, 650, 100, 100), $"{augmentContent.StackIndex}", 80);
                        ModHelperText towerType = panel.AddText(new Info("towerType", augContentX, 425, 1000, 150), augmentContent.TowerType, 40);
                        ModHelperText augmentDescription = panel.AddText(new Info("augmentDescription", augContentX, -300, 700, 400), augmentContent.AugmentDescription, 40);
                    }
                }
                augmentPanelX += augmentPanelWidth;
                augContentX += augmentPanelWidth;
            }
        }

        public void NewIntermediateAugment(Tower tower)
        {
            InGame game = InGame.instance;
            if (SandboxMode)
            {
                RectTransform rect = game.uiRect;
                MenuUi.NewIntermediateAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
                return;
            }
            if (game.GetCash() >= mod.intermediateCost)
            {
                if (mod.level < 4)
                {
                    mod.levelXp += 5;
                }

                if (mod.questLevel == 2 || mod.questLevel == 5 && mod.transmitLevel < mod.questLevel)
                {
                    mod.questXp += 5;
                }

                game.AddCash(-mod.intermediateCost);
                RectTransform rect = game.uiRect;
                tower.worth += mod.intermediateCost;
                MenuUi.NewIntermediateAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
            }
        }

        public static void IntermediateSandBoxAugmentPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();
            ModHelperScrollPanel scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 0, 0, 2500, 1850), RectTransform.Axis.Vertical, VanillaSprites.MainBGPanelBlue, 15, 50);
            ModHelperButton exit = panel.AddButton(new Info("exit", 1200, 900, 135), VanillaSprites.RedBtn, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false); panel.DeleteObject(); mod.selectingAugmentOpen = false; mod.panelOpen = false; CreateUpgradeMenu(rect, tower);
            }));
            ModHelperText x = exit.AddText(new Info("x", 0, 0, 130), "X", 80);

            foreach (var augment in ModContent.GetContent<AugmentTemplate>())
            {
                if (augment.SandboxIndex == 2)
                {
                    scrollPanel.AddScrollContent(CreateAugment(augment, tower));
                }
            }
        }


        // #### Advanced Augments ####

        public static void NewAdvancedAugmentPanel(RectTransform rect, Tower tower)
        {
            mod.panelOpen = true;
            instance.CloseMenu();
            if (SandboxMode)
            {
                AdvancedSandBoxAugmentPanel(rect, tower);
                return;
            }
            float augmentPanelWidth = 833.33f;
            float augmentPanelX = 412.5f;
            float augmentPanelY = 900;
            float augContentX = 25 - (mod.newAugmentSlot - 1) * 425;
            float panelWidth = mod.newAugmentSlot * augmentPanelWidth;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, panelWidth, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlueNotchesShadow);

            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText selectAug = panel.AddText(new Info("selectAug", 0, 800, 2500, 180), "Select New Augment", 100);
            Il2CppSystem.Random rnd = new();
            for (int i = 0; i < mod.newAugmentSlot; i++)
            {
                var AugRarityNum = rnd.Next(1, 100);
                var AugRarity = "Advanced";

                var sprite = VanillaSprites.MainBGPanelSilver;
                var augment = "";
                var img = "";
                List<string> augmentName = new();
                List<string> augmentImage = new();

                if (mod.activeBoon == true)
                {
                    foreach (var augmentBoon in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augmentBoon.TowerType.Contains(mod.boon) && augmentBoon.SandboxIndex == 3)
                        {
                            augmentName.Add(augmentBoon.AugmentName);
                            augmentImage.Add(augmentBoon.Icon);
                        }
                    }

                    if (augmentName.Count > 0)
                    {
                        var numAug = rnd.Next(0, augmentName.Count);
                        augment = augmentName[numAug];
                        img = augmentImage[numAug];
                    }
                    else
                    {
                        var numAug = rnd.Next(0, Advanced.AdvancedAug.Count);
                        augment = Advanced.AdvancedAug[numAug];
                        img = Advanced.AdvancedImg[numAug];
                        InGame game = InGame.instance;

                        foreach (var boon in ModContent.GetContent<BoonTemplate>().OrderByDescending(c => c.mod == mod))
                        {
                            if (boon.BoonCode == mod.boon)
                            {
                                game.AddCash((boon.cost / 1.2f) / 3);
                                tower.worth -= (boon.cost / 1.2f) / 3;
                            }
                        }
                    }
                }
                else
                {
                    var numAug = rnd.Next(0, Advanced.AdvancedAug.Count);
                    augment = Advanced.AdvancedAug[numAug];
                    img = Advanced.AdvancedImg[numAug];
                }

                ModHelperPanel augPanel = panel.AddPanel(new Info("augPanel", augmentPanelX, augmentPanelY, 650, 1450, new UnityEngine.Vector2()), sprite);
                ModHelperText rarityText = panel.AddText(new Info("rarityText", augContentX, 600, 800, 180), AugRarity, 100);
                ModHelperText augmentText = panel.AddText(new Info("augmentText", augContentX, 500, 800, 180), augment, 75);
                ModHelperImage image = panel.AddImage(new Info("image", augContentX, 150, 400, 400), img);
                ModHelperButton selectAugBtn = panel.AddButton(new Info("selectAugBtn", augContentX, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.AugmentSelected(augment, tower)));
                ModHelperText selectAugTxt = selectAugBtn.AddText(new Info("selectWpnTxt", 0, 0, 700, 160), "Select", 70);
                foreach (var augmentContent in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                {
                    if (augmentContent.AugmentName == augment)
                    {
                        ModHelperText StackIndex = panel.AddText(new Info("StackIndex", augContentX + 275, 650, 100, 100), $"{augmentContent.StackIndex}", 80);
                        ModHelperText towerType = panel.AddText(new Info("towerType", augContentX, 425, 1000, 150), augmentContent.TowerType, 40);
                        ModHelperText augmentDescription = panel.AddText(new Info("augmentDescription", augContentX, -300, 700, 400), augmentContent.AugmentDescription, 40);
                    }
                }
                augmentPanelX += augmentPanelWidth;
                augContentX += augmentPanelWidth;
            }
        }

        public void NewAdvancedAugment(Tower tower)
        {
            InGame game = InGame.instance;
            if (SandboxMode)
            {
                RectTransform rect = game.uiRect;
                MenuUi.NewAdvancedAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
                return;
            }
            if (game.GetCash() >= mod.advancedCost)
            {
                if (mod.level < 4)
                {
                    mod.levelXp += 10;
                }

                if (mod.questLevel == 2 || mod.questLevel == 5 && mod.transmitLevel < mod.questLevel)
                {
                    mod.questXp += 10;
                }
                if (mod.questLevel == 4 && mod.transmitLevel < mod.questLevel)
                {
                    mod.questXp += 1;
                }

                game.AddCash(-mod.advancedCost);
                RectTransform rect = game.uiRect;
                tower.worth += mod.advancedCost;
                MenuUi.NewAdvancedAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
            }
        }

        public static void AdvancedSandBoxAugmentPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();
            ModHelperScrollPanel scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 0, 0, 2500, 1850), RectTransform.Axis.Vertical, VanillaSprites.MainBGPanelBlue, 15, 50);
            ModHelperButton exit = panel.AddButton(new Info("exit", 1200, 900, 135), VanillaSprites.RedBtn, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false); panel.DeleteObject(); mod.selectingAugmentOpen = false; mod.panelOpen = false; CreateUpgradeMenu(rect, tower);
            }));
            ModHelperText x = exit.AddText(new Info("x", 0, 0, 130), "X", 80);

            foreach (var augment in ModContent.GetContent<AugmentTemplate>())
            {
                if (augment.SandboxIndex == 3)
                {
                    scrollPanel.AddScrollContent(CreateAugment(augment, tower));
                }
            }
        }


        // #### Mastery Augments ####

        public static void NewMasteryAugmentPanel(RectTransform rect, Tower tower)
        {
            mod.panelOpen = true;
            instance.CloseMenu();
            if (SandboxMode)
            {
                MasterySandBoxAugmentPanel(rect, tower);
                return;
            }
            float augmentPanelWidth = 833.33f;
            float augmentPanelX = 412.5f;
            float augmentPanelY = 900;
            float augContentX = 25 - (mod.newAugmentSlot - 1) * 425;
            float panelWidth = mod.newAugmentSlot * augmentPanelWidth;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, panelWidth, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlueNotchesShadow);

            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText selectAug = panel.AddText(new Info("selectAug", 0, 800, 2500, 180), "Select New Augment", 100);
            Il2CppSystem.Random rnd = new();
            for (int i = 0; i < mod.newAugmentSlot; i++)
            {
                var AugRarityNum = rnd.Next(1, 100);
                var AugRarity = "Mastery";

                var sprite = VanillaSprites.MainBgPanelHematite;
                var augment = "";
                var img = "";
                List<string> augmentName = new();
                List<string> augmentImage = new();

                if (mod.activeBoon == true)
                {
                    foreach (var augmentBoon in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augmentBoon.TowerType.Contains(mod.boon) && augmentBoon.SandboxIndex == 4)
                        {
                            augmentName.Add(augmentBoon.AugmentName);
                            augmentImage.Add(augmentBoon.Icon);
                        }
                    }

                    if (augmentName.Count > 0)
                    {
                        var numAug = rnd.Next(0, augmentName.Count);
                        augment = augmentName[numAug];
                        img = augmentImage[numAug];
                    }
                    else
                    {
                        var numAug = rnd.Next(0, Mastery.MasteryAug.Count);
                        augment = Mastery.MasteryAug[numAug];
                        img = Mastery.MasteryImg[numAug];
                        InGame game = InGame.instance;

                        foreach (var boon in ModContent.GetContent<BoonTemplate>().OrderByDescending(c => c.mod == mod))
                        {
                            if (boon.BoonCode == mod.boon)
                            {
                                game.AddCash((boon.cost / 1.2f) / 3);
                                tower.worth -= (boon.cost / 1.2f) / 3;
                            }
                        }
                    }
                }
                else
                {
                    var numAug = rnd.Next(0, Mastery.MasteryAug.Count);
                    augment = Mastery.MasteryAug[numAug];
                    img = Mastery.MasteryImg[numAug];
                }

                ModHelperPanel augPanel = panel.AddPanel(new Info("augPanel", augmentPanelX, augmentPanelY, 650, 1450, new UnityEngine.Vector2()), sprite);
                ModHelperText rarityText = panel.AddText(new Info("rarityText", augContentX, 600, 800, 180), AugRarity, 100);
                ModHelperText augmentText = panel.AddText(new Info("augmentText", augContentX, 500, 800, 180), augment, 75);
                ModHelperImage image = panel.AddImage(new Info("image", augContentX, 150, 400, 400), img);
                ModHelperButton selectAugBtn = panel.AddButton(new Info("selectAugBtn", augContentX, -600, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.AugmentSelected(augment, tower)));
                ModHelperText selectAugTxt = selectAugBtn.AddText(new Info("selectWpnTxt", 0, 0, 700, 160), "Select", 70);
                foreach (var augmentContent in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                {
                    if (augmentContent.AugmentName == augment)
                    {
                        ModHelperText StackIndex = panel.AddText(new Info("StackIndex", augContentX + 275, 650, 100, 100), $"{augmentContent.StackIndex}", 80);
                        ModHelperText towerType = panel.AddText(new Info("towerType", augContentX, 425, 1000, 150), augmentContent.TowerType, 40);
                        ModHelperText augmentDescription = panel.AddText(new Info("augmentDescription", augContentX, -300, 700, 400), augmentContent.AugmentDescription, 40);
                    }
                }
                augmentPanelX += augmentPanelWidth;
                augContentX += augmentPanelWidth;
            }
        }

        public void NewMasteryAugment(Tower tower)
        {
            InGame game = InGame.instance;
            if (SandboxMode)
            {
                RectTransform rect = game.uiRect;
                MenuUi.NewMasteryAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
                return;
            }
            if (game.GetCash() >= mod.masteryCost)
            {
                if (mod.level < 4)
                {
                    mod.levelXp += 25;
                }

                if (mod.questLevel == 2 || mod.questLevel == 5 && mod.transmitLevel < mod.questLevel)
                {
                    mod.questXp += 25;
                }

                game.AddCash(-mod.masteryCost);
                RectTransform rect = game.uiRect;
                tower.worth += mod.masteryCost;
                MenuUi.NewMasteryAugmentPanel(rect, tower);
                MenuUi.instance.CloseMenu();
            }
        }

        public static void MasterySandBoxAugmentPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 2500, 1850, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();
            ModHelperScrollPanel scrollPanel = panel.AddScrollPanel(new Info("scrollPanel", 0, 0, 2500, 1850), RectTransform.Axis.Vertical, VanillaSprites.MainBGPanelBlue, 15, 50);
            ModHelperButton exit = panel.AddButton(new Info("exit", 1200, 900, 135), VanillaSprites.RedBtn, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false); panel.DeleteObject(); mod.selectingAugmentOpen = false; mod.panelOpen = false; CreateUpgradeMenu(rect, tower);
            }));
            ModHelperText x = exit.AddText(new Info("x", 0, 0, 130), "X", 80);

            foreach (var augment in ModContent.GetContent<AugmentTemplate>())
            {
                if (augment.SandboxIndex == 4)
                {
                    scrollPanel.AddScrollContent(CreateAugment(augment, tower));
                }
            }
        }


        // #### Heroic Augments ####

        public void NewHeroic(Tower tower)
        {
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            MenuUi.NewHeroicAugmentPanel(rect, tower);
            MenuUi.instance.CloseMenu();
        }

        public static void NewHeroicAugmentPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1350, 3400, 2000, new UnityEngine.Vector2()), VanillaSprites.MainBgPanelParagon);
            panel.transform.DestroyAllChildren();
            ModHelperText text = panel.AddText(new Info("text", 0, 1000, 1200, 180), "Heroic Augments", 100);
            ModHelperScrollPanel heroicScrollPanel = panel.AddScrollPanel(new Info("heroicScrollPanel", 0, 0, 3250, 1850), RectTransform.Axis.Horizontal, VanillaSprites.MainBgPanelParagon, 50, 50);

            foreach (var augment in ModContent.GetContent<AugmentTemplate>())
            {
                if (augment.SandboxIndex == 5)
                {
                    heroicScrollPanel.AddScrollContent(CreateHeroic(augment, tower));
                }
            }

            ModHelperButton close = panel.AddButton(new Info("close", 0, -1025, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false);
                panel.DeleteObject();
                if (mod.isSelected == true)
                {
                    CreateUpgradeMenu(rect, tower);
                }
            }));
            ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
        }

        public static ModHelperPanel CreateHeroic(AugmentTemplate augment, Tower tower)
        {
            var panel = ModHelperPanel.Create(new Info("AugmentContent" + augment.AugmentName, 0, 0, 900, 1750), VanillaSprites.PortraitContainerParagon);
            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText augName = panel.AddText(new Info("augName", 0, 775, 900, 180), augment.AugmentName, 80);
            ModHelperText augType = panel.AddText(new Info("augType", 0, 675, 900, 180), augment.TowerType, 60);
            ModHelperImage image = panel.AddImage(new Info("image", 0, 300, 600), augment.Icon);

            ModHelperButton infoAugBtn = panel.AddButton(new Info("infoAugBtn", -325, -150, 150), VanillaSprites.BlueBtn, new System.Action(() => { upgradeUi.InfoPanel(augment, tower); }));
            ModHelperText infoAug = infoAugBtn.AddText(new Info("infoAug", 0, 0, 90, 90), "!", 80);
            ModHelperPanel degreePanel = panel.AddPanel(new Info("degreePanel", 275, -150, 250), VanillaSprites.PortraitContainerParagon);
            ModHelperText degreeAug = degreePanel.AddText(new Info("degreeAug", 0, 0, 250), $"{augment.StackIndex}", 100);

            ModHelperPanel degreeProgress = panel.AddPanel(new Info("degreePanel", 0, -400, 800, 120), VanillaSprites.PortraitContainerParagon);
            ModHelperText prgress = panel.AddText(new Info("progress", 0, -400, 700, 180), "Degree Level", 60);

            ModHelperPanel unlockProgress = panel.AddPanel(new Info("unlockPanel", 0, -550, 800, 120), VanillaSprites.PortraitContainerParagon);
            ModHelperText objectiveProgress = panel.AddText(new Info("objProgress", 0, -550, 750, 180), $"Gain xxx augment points from {augment.TowerType}s to unlock", 40);

            ModHelperButton selectAugBtn = panel.AddButton(new Info("selectAugBtn", 0, -750, 600, 150), VanillaSprites.PurpleBtnLong, null);
            ModHelperText selectAug = selectAugBtn.AddText(new Info("selectAug", 0, 0, 600, 150), "Upgrade", 80);
            return panel;
        }


        // #### Augment Panels ####

        public static ModHelperPanel CreateAugment(AugmentTemplate augment, Tower tower)
        {
            var sprite = VanillaSprites.MainBgPanelJukebox;
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Intermediate)
            {
                sprite = VanillaSprites.MainBGPanelBronze;
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Advanced)
            {
                sprite = VanillaSprites.MainBGPanelSilver;
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Mastery)
            {
                sprite = VanillaSprites.MainBgPanelHematite;
            }
            var panel = ModHelperPanel.Create(new Info("AugmentContent" + augment.AugmentName, 0, 0, 2250, 150), sprite);
            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText augName = panel.AddText(new Info("augName", -600, 0, 1000, 150), augment.AugmentName, 80, TextAlignmentOptions.MidlineLeft);
            ModHelperText rarity = panel.AddText(new Info("rarity", 275, 0, 600, 150), augment.AugmentRarity.ToString(), 80, TextAlignmentOptions.MidlineLeft);
            ModHelperImage image = panel.AddImage(new Info("image", -150, 0, 140, 140), augment.Icon);
            ModHelperButton infoAugBtn = panel.AddButton(new Info("infoAugBtn", 600, 0, 90, 90), VanillaSprites.BlueBtn, new System.Action(() => { upgradeUi.InfoPanel(augment, tower); }));
            ModHelperText infoAug = infoAugBtn.AddText(new Info("infoAug", 0, 0, 90, 90), "!", 50);
            ModHelperButton selectAugBtn = panel.AddButton(new Info("selectAugBtn", 900, 0, 400, 120), VanillaSprites.GreenBtnLong, new System.Action(() => { upgradeUi.AugmentSelected(augment.AugmentName, tower); }));
            ModHelperText selectAug = selectAugBtn.AddText(new Info("selectAug", 0, 0, 400, 120), "Select", 60);
            return panel;
        }

        public void AugmentSelected(string Augment, Tower tower)
        {
            mod.panelOpen = false;
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            if (!SandboxMode)
            {
                Destroy(gameObject);
            }

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
            {
                if (augment.AugmentName == Augment)
                {
                    augment.StackIndex += 1;
                    augment.EditTower();
                }
            }

            mod.boon = " ";
            mod.activeBoon = false;
            if (mod.level == 0 && mod.levelXp >= mod.XPMax)
            {
                mod.level += 1;
                mod.levelXp = 0;
                mod.XPMax = 50;
            }
            if (mod.level == 1 && mod.levelXp >= mod.XPMax)
            {
                mod.level += 1;
                mod.levelXp = 0;
                mod.XPMax = 100;
            }
            if (mod.level == 2 && mod.levelXp >= mod.XPMax)
            {
                mod.level += 1;
                mod.levelXp = 0;
                mod.XPMax = 250;
            }
            if (mod.level == 3 && mod.levelXp >= mod.XPMax)
            {
                mod.level += 1;
            }

            if (mod.questLevel == 1 && mod.questXp >= mod.questXPMax)
            {
                mod.transmitLevel = 1;
                mod.questXp = mod.questXPMax;
            }
            if (mod.questLevel == 2 && mod.questXp >= mod.questXPMax)
            {
                mod.transmitLevel = 2;
                mod.questXp = mod.questXPMax;
            }
            if (mod.questLevel == 3 && mod.questXp >= mod.questXPMax)
            {
                mod.transmitLevel = 3;
                mod.questXp = mod.questXPMax;
            }
            if (mod.questLevel == 4 && mod.questXp >= mod.questXPMax)
            {
                mod.transmitLevel = 4;
                mod.questXp = mod.questXPMax;
            }
            if (mod.questLevel == 5 && mod.questXp >= mod.questXPMax)
            {
                mod.transmitLevel = 5;
                mod.questXp = mod.questXPMax;
            }

            if (mod.isSelected == true && !SandboxMode)
            {
                CreateUpgradeMenu(rect, tower);
            }
            mod.selectingAugmentOpen = false;
        }


        // #### Boon Panel ####

        public void NewBoon(Tower tower)
        {
            if (!SandboxMode)
            {
                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                MenuUi.NewBoonPanel(rect, tower);
                MenuUi.instance.CloseMenu();
            }
        }

        public static void NewBoonPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1350, 2200, 1950, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelGold);
            panel.transform.DestroyAllChildren();

            foreach (var boon in ModContent.GetContent<BoonTemplate>().ToArray())
            {
                ModHelperButton boonButton = panel.AddButton(new Info("boonButton", boon.xPos, boon.yPos, 250), VanillaSprites.YellowBtn, new System.Action(() =>
                {
                    InGame game = InGame.instance;
                    if (mod.activeBoon == false && game.GetCash() >= boon.cost)
                    {
                        mod.activeBoon = true;
                        mod.boon = boon.BoonCode;
                        game.AddCash(-boon.cost);
                        tower.worth += (boon.cost * 0.8f);
                        boon.cost *= 1.2f;

                        if (mod.questLevel == 3 && mod.transmitLevel < mod.questLevel)
                        {
                            mod.questXp += 1;
                        }

                        tower.SetSelectionBlocked(false);
                        panel.DeleteObject();
                        mod.selectingAugmentOpen = false;
                        mod.panelOpen = false; CreateUpgradeMenu(rect, tower);
                    }
                }));
                ModHelperImage image = boonButton.AddImage(new Info("image", 0, 0, 200), boon.TowerIcon);
                ModHelperText cost = panel.AddText(new Info("cost", boon.xPos, boon.yPos - 150, 250, 120), $"${Mathf.Round(boon.cost)}", 50);
            }

            ModHelperText disabled = panel.AddText(new Info("text", -600, -750, 250, 200), "Currently Disabled", 45);
            ModHelperText text = panel.AddText(new Info("text", 0, 975, 1200, 180), "Boons", 100);
            ModHelperText primaryText = panel.AddText(new Info("text", 0, 800, 1200, 180), "Primary", 100);
            ModHelperText militaryText = panel.AddText(new Info("text", 0, 350, 1200, 180), "Military", 100);
            ModHelperText magicText = panel.AddText(new Info("text", 0, -100, 1200, 180), "Magic", 100);
            ModHelperText supportText = panel.AddText(new Info("text", 0, -550, 1200, 180), "Support", 100);
            ModHelperButton exit = panel.AddButton(new Info("exit", 1100, 975, 135, 135), VanillaSprites.RedBtn, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false);
                panel.DeleteObject();
                mod.selectingAugmentOpen = false;
                mod.panelOpen = false; CreateUpgradeMenu(rect, tower);
            }));
            ModHelperText x = exit.AddText(new Info("close", 0, 0, 135, 135), "X", 80);
        }


        // #### Stack Tracker Panel ####

        public static void AugmentStack(Tower tower)
        {
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            MenuUi.StackTrackerPanel(rect, tower);
            MenuUi.instance.CloseMenu();
        }

        public static void StackTrackerPanel(RectTransform rect, Tower tower)
        {
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1350, 3350, 2000, new UnityEngine.Vector2()), VanillaSprites.MainBGPanelBlue);
            panel.transform.DestroyAllChildren();
            ModHelperScrollPanel basicScrollPanel = panel.AddScrollPanel(new Info("basicScrollPanel", -1300, 0, 600, 1850), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanel, 15, 50);
            ModHelperScrollPanel intermediateScrollPanel = panel.AddScrollPanel(new Info("intermediateScrollPanel", -650, 0, 600, 1850), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanel, 15, 50);
            ModHelperScrollPanel advancedScrollPanel = panel.AddScrollPanel(new Info("advancedScrollPanel", 0, 0, 600, 1850), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanel, 15, 50);
            ModHelperScrollPanel masteryScrollPanel = panel.AddScrollPanel(new Info("masteryScrollPanel", 650, 0, 600, 1850), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanel, 15, 50);
            ModHelperScrollPanel heroicScrollPanel = panel.AddScrollPanel(new Info("heroicScrollPanel", 1300, 0, 600, 1850), RectTransform.Axis.Vertical, VanillaSprites.BlueInsertPanel, 15, 50);
            ModHelperText text = heroicScrollPanel.AddText(new Info("text", 0, 0, 550, 400), "Coming Soon", 100);

            ModHelperButton close = panel.AddButton(new Info("close", 0, -1025, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() =>
            {
                tower.SetSelectionBlocked(false);
                panel.DeleteObject();
                if (mod.isSelected == true)
                {
                    CreateUpgradeMenu(rect, tower);
                }
            }));
            ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);

            ModHelperText basicText = panel.AddText(new Info("basicText", -1300, 975, 1200, 180), "Basic", 100);
            ModHelperText intermediateText = panel.AddText(new Info("intermediateText", -650, 975, 1200, 180), "Intermediate", 100);
            ModHelperText advancedText = panel.AddText(new Info("advancedText", 0, 975, 1200, 180), "Advanced", 100);
            ModHelperText masteryText = panel.AddText(new Info("masteryText", 650, 975, 1200, 180), "Mastery", 100);
            ModHelperText heroicText = panel.AddText(new Info("heroicText", 1300, 975, 1200, 180), "Heroic", 100);

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
            {
                if (augment.StackIndex >= 1)
                {
                    if (augment.AugmentRarity == AugmentTemplate.Rarity.Basic)
                    {
                        basicScrollPanel.AddScrollContent(CreateAugmentStack(augment, tower));
                    }
                    else if (augment.AugmentRarity == AugmentTemplate.Rarity.Intermediate)
                    {
                        intermediateScrollPanel.AddScrollContent(CreateAugmentStack(augment, tower));
                    }
                    else if (augment.AugmentRarity == AugmentTemplate.Rarity.Advanced)
                    {
                        advancedScrollPanel.AddScrollContent(CreateAugmentStack(augment, tower));
                    }
                    else if (augment.AugmentRarity == AugmentTemplate.Rarity.Mastery)
                    {
                        masteryScrollPanel.AddScrollContent(CreateAugmentStack(augment, tower));
                    }
                    else if (augment.AugmentRarity == AugmentTemplate.Rarity.Heroic)
                    {
                        heroicScrollPanel.AddScrollContent(CreateAugmentStack(augment, tower));
                    }
                }
            }
        }

        public static ModHelperPanel CreateAugmentStack(AugmentTemplate augment, Tower tower)
        {
            var sprite = VanillaSprites.MainBgPanelJukebox;
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Intermediate)
            {
                sprite = VanillaSprites.MainBGPanelBronze;
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Advanced)
            {
                sprite = VanillaSprites.MainBGPanelSilver;
            }
            if (augment.AugmentRarity == AugmentTemplate.Rarity.Mastery)
            {
                sprite = VanillaSprites.MainBgPanelHematite;
            }
            var panel = ModHelperPanel.Create(new Info("AugmentContent" + augment.AugmentName, 0, 0, 520, 300), sprite);
            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            ModHelperText augName = panel.AddText(new Info("augName", 0, 100, 1000, 150), augment.AugmentName, 55);
            ModHelperImage image = panel.AddImage(new Info("image", 0, 0, 140, 140), augment.Icon);
            ModHelperText stack = panel.AddText(new Info("stack", 175, -75, 400, 120), augment.StackIndex.ToString(), 80);
            ModHelperButton infoAugBtn = panel.AddButton(new Info("infoAugBtn", -175, -75, 90, 90), VanillaSprites.BlueBtn, new System.Action(() => { upgradeUi.InfoPanel(augment, tower); }));
            ModHelperText infoAug = infoAugBtn.AddText(new Info("infoAug", 0, 0, 90, 90), "!", 50);
            return panel;
        }

        
        // #### Info Panel ####

        public void InfoPanel(AugmentTemplate augment, Tower tower)
        {
            if (mod.infoOpen == false)
            {
                mod.infoOpen = true;
                var sprite = VanillaSprites.MainBgPanelJukebox;
                if (augment.AugmentRarity == AugmentTemplate.Rarity.Intermediate)
                {
                    sprite = VanillaSprites.MainBGPanelBronze;
                }
                if (augment.AugmentRarity == AugmentTemplate.Rarity.Advanced)
                {
                    sprite = VanillaSprites.MainBGPanelSilver;
                }
                if (augment.AugmentRarity == AugmentTemplate.Rarity.Mastery)
                {
                    sprite = VanillaSprites.MainBgPanelHematite;
                }
                if (augment.AugmentRarity == AugmentTemplate.Rarity.Heroic)
                {
                    sprite = VanillaSprites.PortraitContainerParagon;
                }
                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 1250, new UnityEngine.Vector2()), sprite);
                MenuUi tutorialUi = panel.AddComponent<MenuUi>();
                ModHelperText infoText = panel.AddText(new Info("infoText", 0, 500, 1200, 180), augment.AugmentName, 100);
                ModHelperText description = panel.AddText(new Info("description", 0, 0, 1300, 800), augment.AugmentDescription, 75);

                ModHelperButton close = panel.AddButton(new Info("close", 0, -600, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => tutorialUi.CloseMessage()));
                ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
            }
        }
        public void CloseMessage()
        {
            Destroy(gameObject);
            mod.infoOpen = false;
            mod.transmitOpen = false;
        }


        // #### Tutorial Panel ####

        public void TutorialPanel(Tower tower)
        {
            MenuUi.instance.CloseMenu();
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1350, 3000, 2000, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi tutorialUi = panel.AddComponent<MenuUi>();
            ModHelperText tutorialText = panel.AddText(new Info("tutorialText", 0, 1000, 2500, 180), "How Augmenter Works", 100);

            ModHelperScrollPanel augmentPanel = panel.AddScrollPanel(new Info("AugPanel", -950, 0, 900, 1800), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanelDark);
            ModHelperText augmentText = augmentPanel.AddText(new Info("augmentText", 0, 800, 2500, 180), "Augments:", 100);
            ModHelperText augText1 = augmentPanel.AddText(new Info("augText1", 0, 625, 850, 180), "* Choose from 1 of 3 random augments", 50, TextAlignmentOptions.Left);
            ModHelperText augText2 = augmentPanel.AddText(new Info("augText2", 0, 525, 850, 180), "* Augments come in 5 rarites:", 50, TextAlignmentOptions.Left);
            ModHelperText augText3 = augmentPanel.AddText(new Info("augText3", 50, 425, 775, 180), "- Basic: Provides general stats to the specified tower", 50, TextAlignmentOptions.Left);
            ModHelperText augText4 = augmentPanel.AddText(new Info("augText4", 50, 275, 775, 180), "- Intermediate: Provides effects to the specified tier 1 or 2", 50, TextAlignmentOptions.Left);
            ModHelperText augText5 = augmentPanel.AddText(new Info("augText5", 50, 125, 775, 180), "- Advanced: Provides effects to the specified tier 3 or 4", 50, TextAlignmentOptions.Left);
            ModHelperText augText6 = augmentPanel.AddText(new Info("augText6", 50, 0, 775, 180), "- Mastery: Provides powerful effects to the specified tier 5", 50, TextAlignmentOptions.Left);
            ModHelperText augText7 = augmentPanel.AddText(new Info("augText7", 50, -150, 775, 180), "- Heroic: Provides powerful effects to the specified [REDACTED]", 50, TextAlignmentOptions.Left);
            ModHelperText augText8 = augmentPanel.AddText(new Info("augText8", 0, -350, 850, 230), "* Purchased augments provide effects to all towers with the specified upgrades for the rest of the game", 50, TextAlignmentOptions.Left);
            ModHelperText augText9 = augmentPanel.AddText(new Info("augText9", 0, -525, 850, 180), "* Augments stack providing stronger effects", 50, TextAlignmentOptions.Left);
            ModHelperText augText10 = augmentPanel.AddText(new Info("augText10", 0, -700, 850, 230), "* Stats shown in parentheses are the provided effect for every copy of that augment after the first one", 50, TextAlignmentOptions.Left);

            ModHelperScrollPanel trackerPanel = panel.AddScrollPanel(new Info("TrackPanel", 0, 450, 900), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanelDark);
            ModHelperText trackerText = trackerPanel.AddText(new Info("trackerText", 0, 350, 2500, 180), "Stack Tracker:", 100);
            ModHelperText trackText1 = trackerPanel.AddText(new Info("trackText1", 0, 175, 850, 180), "* Stack Tracker menu displays all augments you have purchased", 50, TextAlignmentOptions.Left);
            ModHelperText trackText2 = trackerPanel.AddText(new Info("trackText2", 0, 25, 850, 180), "* Stack Tracker seperates augments by rarity", 50, TextAlignmentOptions.Left);
            ModHelperText trackText3 = trackerPanel.AddText(new Info("trackText3", 0, -125, 850, 180), "* Numer in bottom right of augments displays the count of the augment", 50, TextAlignmentOptions.Left);
            ModHelperText trackText4 = trackerPanel.AddText(new Info("trackText4", 0, -300, 850, 230), "* clicking on the info button in bottom left of augments displays the augment's effects", 50, TextAlignmentOptions.Left);

            ModHelperScrollPanel boonsPanel = panel.AddScrollPanel(new Info("BoonPanel", 950, 450, 900), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanelDark);
            ModHelperText boonsText = boonsPanel.AddText(new Info("boonsText", 0, 350, 2500, 180), "Boons:", 100);
            ModHelperText boonText1 = boonsPanel.AddText(new Info("boonText1", 0, 175, 850, 180), "* Boons menu allows you to buy tower boons", 50, TextAlignmentOptions.Left);
            ModHelperText boonText2 = boonsPanel.AddText(new Info("boonText2", 0, 25, 850, 180), "* Boons guarantee that your next augment purchase is for that tower", 50, TextAlignmentOptions.Left);
            ModHelperText boonText3 = boonsPanel.AddText(new Info("boonText3", 0, -125, 850, 180), "* Boons increase in price as you buy them", 50, TextAlignmentOptions.Left);
            ModHelperText boonText4 = boonsPanel.AddText(new Info("boonText4", 0, -300, 850, 230), "* If a tower does not have an augment of the purchased rarity, the cost of the boon will be refunded", 50, TextAlignmentOptions.Left);

            ModHelperScrollPanel levelsPanel = panel.AddScrollPanel(new Info("LevelsPanel", 475, -475, 1850, 850), RectTransform.Axis.Vertical, VanillaSprites.BrownInsertPanelDark);
            ModHelperText levelsText = levelsPanel.AddText(new Info("levelsText", 0, 325, 2500, 180), "Level Up:", 100);
            ModHelperText levelText1 = levelsPanel.AddText(new Info("levelText1", 0, 200, 1800, 180), "* Augmenter unlocks higher rarity augments as it levels up", 50, TextAlignmentOptions.Left);
            ModHelperText levelText2 = levelsPanel.AddText(new Info("levelText2", 0, 100, 1800, 180), "* Purchasing augments provides augment points based on the rarity that contribute towards leveling up the augmeter", 50, TextAlignmentOptions.Left);
            ModHelperText levelText3 = levelsPanel.AddText(new Info("levelText3", 50, 0, 1750, 180), "- Basic augments provide 1 augment point", 50, TextAlignmentOptions.Left);
            ModHelperText levelText4 = levelsPanel.AddText(new Info("levelText4", 50, -75, 1750, 180), "- Intermediate augments provide 5 augment points", 50, TextAlignmentOptions.Left);
            ModHelperText levelText5 = levelsPanel.AddText(new Info("levelText5", 50, -150, 1750, 180), "- Advanced augments provide 10 augment points", 50, TextAlignmentOptions.Left);
            ModHelperText levelText6 = levelsPanel.AddText(new Info("levelText6", 50, -225, 1750, 180), "- Mastery augments provide 25 augment points", 50, TextAlignmentOptions.Left);
            ModHelperText levelText7 = levelsPanel.AddText(new Info("levelText7", 0, -325, 1800, 180), "* Boons become available to purchase once you unlock Intermediate augments", 50, TextAlignmentOptions.Left);

            ModHelperButton cancel = panel.AddButton(new Info("cancel", 0, -1000, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => tutorialUi.Cancel(tower)));
            ModHelperText cancelText = cancel.AddText(new Info("cancelText", 0, 0, 700, 160), "Cancel", 70);
        }

        public void Cancel(Tower tower)
        {
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            Destroy(gameObject);
            mod.panelOpen = false;
            if (mod.isSelected == true)
            {
                CreateUpgradeMenu(rect, tower);
            }
        }


        // #### Augmenter Menu ####

        public static void CreateUpgradeMenu(RectTransform rect, Tower tower)
        {
            if (mod.panelOpen == true)
            {
                return;
            }
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 200, 3300, 450, new UnityEngine.Vector2()), VanillaSprites.BlueInsertPanel);
            MenuUi upgradeUi = panel.AddComponent<MenuUi>();
            instance = upgradeUi;
            if (SandboxMode)
            {
                ModHelperText newBasicAugCost = panel.AddText(new Info("newAugCost", -1350, 140, 1800, 180), "Free", 70);
                ModHelperText newIntermediateAugCost = panel.AddText(new Info("newAugCost", -900, 140, 1800, 180), "Free", 70);
                ModHelperText newAdvancedAugCost = panel.AddText(new Info("newAugCost", -450, 140, 1800, 180), "Free", 70);
                ModHelperText newMasteryAugCost = panel.AddText(new Info("newAugCost", 0, 140, 1800, 180), "Free", 70);
                ModHelperText RandomAugCost = panel.AddText(new Info("newAugCost", 625, 140, 1800, 180), "Free", 70);
            }
            else
            {
                ModHelperText newBasicAugCost = panel.AddText(new Info("newAugCost", -1350, 140, 1800, 180), $"{mod.basicCost}", 70);
                ModHelperText newIntermediateAugCost = panel.AddText(new Info("newAugCost", -900, 140, 1800, 180), $"{mod.intermediateCost}", 70);
                ModHelperText newAdvancedAugCost = panel.AddText(new Info("newAugCost", -450, 140, 1800, 180), $"{mod.advancedCost}", 70);
                ModHelperText newMasteryAugCost = panel.AddText(new Info("newAugCost", 0, 140, 1800, 180), $"{mod.masteryCost}", 70);
                ModHelperText RandomAugCost = panel.AddText(new Info("newAugCost", 625, 140, 1800, 180), "$3000", 70);

                if (mod.level < 3)
                {
                    var percent = mod.levelXp * 100 / mod.XPMax;
                    var size = 2970 * percent / 100;
                    ModHelperPanel xppanel = panel.AddPanel(new Info("Panel", 0, 500, 3000, 180), VanillaSprites.BrownInsertPanel);
                    ModHelperPanel xpbar = panel.AddPanel(new Info("Panel", (size - size / 2) - 2970 / 2, 500, size, 150), VanillaSprites.MainBGPanelGold);
                    ModHelperText upgrade1Buy = panel.AddText(new Info("text", 0, 500, 3000, 180), mod.XPMax - mod.levelXp + " Augment Points Until Level Up", 70);
                }
            }
            if (mod.selectingAugmentOpen == false)
            {
                ModHelperButton newBasicAugBtn = panel.AddButton(new Info("newAugBtn", -1350, 20, 400, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.NewBasicAugment(tower)));
                ModHelperButton RandomAugBtn = panel.AddButton(new Info("newAugBtn", 625, 20, 500, 160), VanillaSprites.RedBtnLong, null);
                ModHelperText newBasicAugBuy = newBasicAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Buy", 70);
                ModHelperText RandomAugBuy = RandomAugBtn.AddText(new Info("newAugBuy", 0, 0, 500, 160), "Coming Soon", 70);

                if (mod.level >= 1)
                {
                    ModHelperButton newIntermediateAugBtn = panel.AddButton(new Info("newAugBtn", -900, 20, 400, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.NewIntermediateAugment(tower)));
                    ModHelperText newIntermediateAugBuy = newIntermediateAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Buy", 70);

                    if (mod.activeBoon == true)
                    {
                        ModHelperButton BoonBtn = panel.AddButton(new Info("newAugBtn", 1300, 20, 500, 160), VanillaSprites.RedBtnLong, null);
                        ModHelperText BoonMenu = BoonBtn.AddText(new Info("newAugBuy", 0, 0, 500, 160), "Active Boon", 70);
                    }
                    else
                    {
                        ModHelperButton BoonBtn = panel.AddButton(new Info("newAugBtn", 1300, 20, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.NewBoon(tower)));
                        ModHelperText BoonMenu = BoonBtn.AddText(new Info("newAugBuy", 0, 0, 500, 160), "Boons", 70);
                    }
                }
                else
                {
                    ModHelperButton newIntermediateAugBtn = panel.AddButton(new Info("newAugBtn", -900, 20, 400, 160), VanillaSprites.RedBtnLong, null);
                    ModHelperButton BoonBtn = panel.AddButton(new Info("newAugBtn", 1300, 20, 500, 160), VanillaSprites.RedBtnLong, null);
                    ModHelperText newIntermediateAugBuy = newIntermediateAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Locked", 70);
                    ModHelperText BoonMenu = BoonBtn.AddText(new Info("newAugBuy", 0, 0, 500, 160), "Locked", 70);
                }
                if (mod.level >= 2)
                {
                    ModHelperButton newAdvancedAugBtn = panel.AddButton(new Info("newAugBtn", -450, 20, 400, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.NewAdvancedAugment(tower)));
                    ModHelperText newAdvancedAugBuy = newAdvancedAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Buy", 70);
                }
                else
                {
                    ModHelperButton newAdvancedAugBtn = panel.AddButton(new Info("newAugBtn", -450, 20, 400, 160), VanillaSprites.RedBtnLong, null);
                    ModHelperText newAdvancedAugBuy = newAdvancedAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Locked", 70);
                }
                if (mod.level >= 3)
                {
                    ModHelperButton newMasteryAugBtn = panel.AddButton(new Info("newAugBtn", 0, 20, 400, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.NewMasteryAugment(tower)));
                    ModHelperText newMasteryAugBuy = newMasteryAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Buy", 70);
                }
                else
                {
                    ModHelperButton newMasteryAugBtn = panel.AddButton(new Info("newAugBtn", 0, 20, 400, 160), VanillaSprites.RedBtnLong, null);
                    ModHelperText newMasteryAugBuy = newMasteryAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Locked", 70);
                }/*
                if (mod.level >= 4)
                {
                    ModHelperButton newHeroicAugBtn = panel.AddButton(new Info("newAugBtn", -1200, 300, 500, 160), VanillaSprites.PurpleBtnLong, new System.Action(() => upgradeUi.NewHeroic(tower)));
                    ModHelperText newHeroicAugBuy = newHeroicAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Heroics", 70);
                }
                else
                {
                    ModHelperButton newHeroicAugBtn = panel.AddButton(new Info("newAugBtn", -1200, 300, 500, 160), VanillaSprites.RedBtnLong, null);
                    ModHelperText newHeroicAugBuy = newHeroicAugBtn.AddText(new Info("newAugBuy", 0, 0, 400, 160), "Locked", 70);
                }*/

                ModHelperButton questTracker = panel.AddButton(new Info("questTracker", 1200, 300, 500, 160), VanillaSprites.YellowBtnLong, new System.Action(() => { mod.questOpen = true; MenuUi.instance.CloseMenu(); MenuUi.CreateUpgradeMenu(rect, tower); }));
                ModHelperText questTrackerText = questTracker.AddText(new Info("questTrackerText", 0, 0, 600, 160), "Transmission", 60);

                if (mod.questOpen == true)
                {
                    ModHelperPanel questPanel = panel.AddPanel(new Info("questPanel", 1200, 725, 800, 1000), VanillaSprites.BrownInsertPanel);
                    ModHelperText transmitText = questPanel.AddText(new Info("transmitText", 0, 400, 700, 180), "Mastery Quest", 80);
                    ModHelperPanel xppanel = questPanel.AddPanel(new Info("Panel", 0, -200, 800, 180), VanillaSprites.BrownInsertPanel);

                    var percent = mod.questXp * 100 / mod.questXPMax;
                    var size = 780 * percent / 100;
                    ModHelperPanel xpbar = questPanel.AddPanel(new Info("Panel", (size - size / 2) - 780 / 2, -200, size, 160), VanillaSprites.MainBGPanelGold);

                    if (mod.transmitLevel == mod.questLevel)
                    {
                        ModHelperText objective = questPanel.AddText(new Info("objective", 0, -200, 700, 180), "No objective available right now", 50);
                        ModHelperText newTransmitText = questPanel.AddText(new Info("newTransmitText", 0, 225, 700, 180), "New Transmission", 70);
                        newTransmitText.Text.color = new Color(1, 0.85f, 0);
                        newTransmitText.Text.outlineColor = new Color(0.86f, 0.5f, 0f);
                        newTransmitText.Text.outlineWidth *= 1.5f;
                        ModHelperButton transmission = questPanel.AddButton(new Info("transmission", 0, 25, 500, 160), VanillaSprites.GreenBtnLong, new System.Action(() => upgradeUi.TransmitPanel(tower)));
                        ModHelperText transmissionText = transmission.AddText(new Info("transmissionText", 0, 0, 550, 160), "Transmission", 60);
                    }
                    else
                    {
                        ModHelperText newTransmitText = questPanel.AddText(new Info("newTransmitText", 0, 100, 700, 180), "No Transmission at this time.", 70);
                        newTransmitText.Text.color = new Color(1, 0, 0.5f);
                        newTransmitText.Text.outlineColor = new Color(0.55f, 0, 0.26f);
                        newTransmitText.Text.outlineWidth *= 1.5f;

                        if (mod.questLevel == 1)
                        {
                            ModHelperText objective = questPanel.AddText(new Info("objective", 0, -200, 700, 180), $"Purchase {mod.questXPMax - mod.questXp} Basic augments", 50);     
                        }
                        else if (mod.questLevel == 2)
                        {
                            ModHelperText objective = questPanel.AddText(new Info("objective", 0, -200, 700, 180), $"Get {mod.questXPMax - mod.questXp} augment points", 50);
                        }
                        else if (mod.questLevel == 3)
                        {
                            ModHelperText objective = questPanel.AddText(new Info("objective", 0, -200, 700, 180), $"Purchase {mod.questXPMax - mod.questXp} boons", 50);
                        }
                        else if (mod.questLevel == 4)
                        {
                            ModHelperText objective = questPanel.AddText(new Info("objective", 0, -200, 700, 180), $"Purchase {mod.questXPMax - mod.questXp} Advanced augments", 50);
                        }
                        else if (mod.questLevel == 5)
                        {
                            ModHelperText objective = questPanel.AddText(new Info("objective", 0, -200, 700, 180), $"Get {mod.questXPMax - mod.questXp} augment points", 50);
                        }
                    }

                    ModHelperButton cancel = questPanel.AddButton(new Info("cancel", 0, -400, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => { mod.questOpen = false; MenuUi.instance.CloseMenu(); MenuUi.CreateUpgradeMenu(rect, tower); }));
                    ModHelperText cancelText = cancel.AddText(new Info("cancelText", 0, 0, 550, 160), "Close", 70);
                }
            }
            ModHelperText newBasicAugDesc = panel.AddText(new Info("newAugDesc", -1350, -100, 860, 180), "Basic", 42);
            ModHelperText newIntermediateAugDesc = panel.AddText(new Info("newAugDesc", -900, -100, 860, 180), "Intermediate", 42);
            ModHelperText newAdvancedAugDesc = panel.AddText(new Info("newAugDesc", -450, -100, 860, 180), "Advanced", 42);
            ModHelperText newMasteryAugDesc = panel.AddText(new Info("newAugDesc", 0, -100, 860, 180), "Mastery", 42);
            ModHelperText RandomAugDesc = panel.AddText(new Info("newAugDesc", 625, -100, 860, 180), "Random Augment", 42);
            ModHelperText BoonsDesc = panel.AddText(new Info("newAugDesc", 1300, -100, 860, 180), "Opens the Boon Menu", 42);
            ModHelperText BoonCost = panel.AddText(new Info("newAugCost", 1300, 140, 1800, 180), "Buy Tower Boons", 70);

            ModHelperButton tutorial = panel.AddButton(new Info("tutorial", -300, 300, 500, 160), VanillaSprites.BlueBtnLong, new System.Action(() => upgradeUi.TutorialPanel(tower)));
            ModHelperText tutorialMenu = tutorial.AddText(new Info("tutorialMenu", 0, 0, 600, 160), "Help", 70);

            ModHelperButton stackTracker = panel.AddButton(new Info("stackTracker", 300, 300, 500, 160), VanillaSprites.BlueBtnLong, new System.Action(() => AugmentStack(tower)));
            ModHelperText StackTrackerMenu = stackTracker.AddText(new Info("stackTrackerMenu", 0, 0, 600, 160), "Stack Tracker", 60);
        }


        // #### Quest Logs ####

        public void TransmitPanel(Tower tower)
        {
            if (mod.transmitOpen == false)
            {
                mod.transmitOpen = true;
                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 1250, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
                MenuUi transmissionUi = panel.AddComponent<MenuUi>();
                ModHelperText transmitText = panel.AddText(new Info("transmitText", 0, 550, 1200, 180), "Transmission", 100);
                ModHelperText speakerText = panel.AddText(new Info("speakerText", 0, 375, 1200, 180), "Unknown Transmitter:", 80);

                if (mod.questLevel == 0)
                {
                    ModHelperText transmission = panel.AddText(new Info("transmission", 0, -50, 1300, 800), "*<Stactic>* Alright. Link is established. Connection looks stable. I can finally let you in on what's going on. " +
                        "Unfortunately, I can't summarize everything into a single transmission and for some reason the network is locking transmissions unless you provide an input activation. " +
                        "So this will take some work from you as well. Also, this isn't exactly a secure channel so each transmission will have to be deleted after you play them because 'he' may be listening. *<Static>*", 55);
                }
                else if (mod.questLevel == 1)
                {
                    ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Stactic>* Ok, so I guess I should start with the biggest question. What is the augmenter? " +
                        "The augmenter is a helper we gave you that provides augments to make you more powerful. Pretty staight forward, but why did we give you the augmenter? I should first start with telling you who we are. " +
                        "I am a lead developer at Augtech but who I am is not important. We gave you the augmenter because we needed you to be prepared for when 'he' shows up. *<Static>*", 55);
                }
                else if (mod.questLevel == 2)
                {
                    ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Stactic>* You may be asking yourself now: Who is 'he' and what is this guy talking about? " +
                        "The problem is that I can't tell you who 'he' is because you knowing puts you in danger of being corrupted by his power. What I can tell you is that 'he' is an ancient being that " +
                        "managed to harness the powers of the elements. He used his powers for good by helping people in need. Many people adored him for the help he provided. *<Static>*", 55);
                }
                else if (mod.questLevel == 3)
                {
                    ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Stactic>* The other ancient beings were jealous of the love 'he' was recieving and so they made a plan to " +
                        "get back at 'him'. They tricked him into harming innocent people which made him so distraught that he harnessed the most powerful element of them all, Chaos, to destroy the other ancient beings. " +
                        "The other ancient beings managed to escape his wrath but because of his use of the chaos element, he became corrupted by its influence. *<Static>*", 55);
                }
                else if (mod.questLevel == 4)
                {
                    ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Stactic>* The ancient beings feared that 'he' may have become even more powerful in his corrupted form " +
                        "and might even be capable of destroying the world, so they decided to capture him and imprison him for all eternity. Now, cracks are forming in the walls of 'his' prison, and pretty soon 'he' " +
                        "may be able to break free. If 'he' manages this, he may very well destroy the world and everything on it. *<Static>*", 55);
                }
                else if (mod.questLevel == 5)
                {
                    ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Stactic>* This is why we needed you to be prepared and why we gave you the augmenter in the first place. " +
                        "I had my eye on you ever since we found out about 'his' possible escape. You seemed like the best candidate for the task of fighting 'him' if he ever breaks free. We are still hard at work " +
                        "on those heroic augments because they are your best weapons against 'him'. This is all the information I can give you right now but I will contact you again once the heroic augments are complete. " +
                        "Good Luck for now. END TRANSMISSION *<Static>*", 50);
                }

                ModHelperButton close = panel.AddButton(new Info("close", 0, -600, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => transmissionUi.CloseTransmission(tower)));
                ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
            }
        }

        public void CloseTransmission(Tower tower)
        {
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            Destroy(gameObject);
            mod.infoOpen = false;
            mod.transmitOpen = false;
            mod.questLevel += 1;
            mod.questXp = 0;

            if (mod.questLevel == 2)
            {
                mod.questXPMax = 40;
            }
            if (mod.questLevel == 3)
            {
                mod.questXPMax = 10;
            }
            if (mod.questLevel == 4)
            {
                mod.questXPMax = 25;
            }
            if (mod.questLevel == 5)
            {
                mod.questXPMax = 500;
            }
            if (mod.questLevel == 6)
            {
                MenuUi.instance.CloseMenu();
                QuestComplete(tower);
            }
            else
            {
                MenuUi.instance.CloseMenu();
                MenuUi.CreateUpgradeMenu(rect, tower);
            }
        }

        public void QuestComplete(Tower tower)
        {
            mod.transmitOpen = true;
            InGame game = InGame.instance;
            RectTransform rect = game.uiRect;
            ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 750, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
            MenuUi transmissionUi = panel.AddComponent<MenuUi>();
            ModHelperText titleText = panel.AddText(new Info("titleText", 0, 300, 1200, 180), "Quest Complete", 100);
            ModHelperText rewardText = panel.AddText(new Info("rewardText", 0, 0, 1200, 180), "Reward: All augments are now 15% off", 80);
            ModHelperButton close = panel.AddButton(new Info("close", 0, -350, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => {
                mod.basicCost *= 0.85f;
                mod.intermediateCost *= 0.85f;
                mod.advancedCost *= 0.85f;
                mod.masteryCost *= 0.85f;
                transmissionUi.CloseMessage();

                if (mod.isSelected == true)
                {
                    CreateUpgradeMenu(rect, tower);
                }
            }));
            ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
        }

        /*public void Transmit1Panel(Tower tower)
        {
            if (mod.transmitOpen == false)
            {
                mod.transmitOpen = true;
                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 1250, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
                MenuUi transmissionUi = panel.AddComponent<MenuUi>();
                ModHelperText transmitText = panel.AddText(new Info("transmitText", 0, 550, 1200, 180), "Transmission 1", 100);
                ModHelperText speakerText = panel.AddText(new Info("speakerText", 0, 350, 1200, 180), "Unknown Transmitter:", 80);
                ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Stactic>* Ah, good you managed to recieve my signal. Unfortunately, " +
                    "the signal won't hold for much longer so we don't have much time to talk. If possible, see if you can boost this signal so that next time we speak, we will have a clearer connection. *<Static>*", 65);

                ModHelperButton close = panel.AddButton(new Info("close", 0, -600, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => transmissionUi.CloseMessage()));
                ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
            }
        }

        public void Transmit2Panel(Tower tower)
        {
            if (mod.transmitOpen == false)
            {
                mod.transmitOpen = true;
                InGame game = InGame.instance;
                RectTransform rect = game.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new Info("Panel_", 2200, 1500, 1500, 1250, new UnityEngine.Vector2()), VanillaSprites.BrownInsertPanel);
                MenuUi transmissionUi = panel.AddComponent<MenuUi>();
                ModHelperText transmitText = panel.AddText(new Info("transmitText", 0, 550, 1200, 180), "Transmission 2", 100);
                ModHelperText speakerText = panel.AddText(new Info("speakerText", 0, 350, 1200, 180), "Unknown Transmitter:", 80);
                ModHelperText transmission = panel.AddText(new Info("transmission", 0, -25, 1300, 800), "*<Static>* Alright, everything is set. " +
                    "This will be the last transmission for now but I will try to contact you again in a few days with more information. Oh, and about those heroic augments. They appear to radiate some kind of paragon like energy. " +
                    "We can't send them though to you right now as we are still working on them but we'll try to get them to you as soon as possible. You'll need them for what's to come. Talk to you in a bit. *<Static>* END TRANSMISSION", 55);

                ModHelperButton close = panel.AddButton(new Info("close", 0, -600, 500, 160), VanillaSprites.RedBtnLong, new System.Action(() => transmissionUi.CloseMessage()));
                ModHelperText closeText = close.AddText(new Info("closeText", 0, 0, 700, 160), "Close", 70);
            }
        }*/
    }


    // #### Harmony Patches ####

    public static class Patch
    {
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.Input.InputManager), nameof(Il2CppAssets.Scripts.Simulation.Input.InputManager.GetRangeMeshes))]
        internal static class InputManager_GetRangeMeshes
        {
            [HarmonyPostfix]
            private static void Postfix(Il2CppAssets.Scripts.Simulation.Input.InputManager __instance, TowerModel towerModel, Il2CppAssets.Scripts.Simulation.SMath.Vector3 position, ref Il2CppSystem.Collections.Generic.List<Il2CppAssets.Scripts.Simulation.Display.Mesh> __result)
            {
                if (towerModel != null && towerModel.appliedUpgrades.Contains(UpgradeType.TradeEmpire))
                {
                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "TradeNetwork")
                        {
                            if (augment.StackIndex >= 1)
                            {
                                var mesh = RangeMesh.GetMeshStatically(__instance.Sim, position, towerModel.GetAttackModel().range,towerModel.ignoreBlockers);
                                mesh.isValid = true;
                                mesh.position = position;
                                __result.Add(mesh);
                            }
                        }
                    }
                }
            }
        }
    }

    /*[HarmonyPatch(typeof(Map), nameof(Map.GetSaveData))]
    [HarmonyPostfix]
    static void OnMapSaved(Tower tower, MapSaveDataModel mapData)
    {
       


        if (tower.towerModel.GetModTower()?.GetType() == typeof(Augmenter))
        {

        }


        var json = JsonConvert.SerializeObject(UpgradeMenu.PurchasedUpgrades);
        mapData.metaData["CursorUpgrade"] = json;

        mapData.metaData["CursorPops"] = CursorPops.ToString(CultureInfo.InvariantCulture);

        foreach (var upgrade in CurrentUpgrades.OrderBy(x => x.Tier))
        {
            upgrade.OnMapSaved(mapData);
        }
    }

    /*[HarmonyPatch(typeof(Map), nameof(Map.SetSaveData))]
    [HarmonyPostfix]
    static void OnMapLoaded(MapSaveDataModel mapData)
    {
        CurrentUpgrades.Clear();
        if (mapData.metaData.TryGetValue("CursorUpgrade", out var data))
        {
            UpgradeMenu.PurchasedUpgrades = JsonConvert.DeserializeObject<Dictionary<Path, int>>(data) ?? new Dictionary<Path, int>();

            foreach (var (path, tier) in UpgradeMenu.PurchasedUpgrades)
            {
                for (var i = 0; i <= tier; i++)
                {
                    if (!CursorUpgrade.Cache[path].TryGetValue(i, out var upgrade))
                        continue;
                    CurrentUpgrades.Add(upgrade);
                }
            }
        }
        else
        {
            UpgradeMenu.PurchasedUpgrades = Paths.ToDictionary(path => path, _ => UpgradeMenu.UnPurchased);
        }

        if (mapData.metaData.TryGetValue("CursorPops", out var cursorPops))
        {
            CursorPops = float.Parse(cursorPops, CultureInfo.InvariantCulture);
        }
        else
        {
            CursorPops = 0;
        }

        foreach (var upgrade in CurrentUpgrades.OrderBy(x => x.Tier))
        {
            upgrade.OnMapLoaded(mapData);
        }

        CursorUpgrade.UpdateTower();
    }*/
}