using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity;
using static Templates.AugmentTemplate;

namespace Templates
{
    public abstract class AugmentTemplate : ModContent
    {
        public override void Register() { }
        public abstract int SandboxIndex { get; }
        public abstract Rarity AugmentRarity { get; }
        public abstract string AugmentName { get; }
        public abstract string AugmentDescription { get; }
        public abstract string TowerType { get; }
        public abstract string Icon { get; }
        public abstract void EditTower();
        public enum Rarity
        {
            Basic,
            Intermediate,
            Advanced,
            Mastery,
            Heroic
        }
        public int StackIndex = 0;
    }

    public abstract class BasicStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class DartStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class BoomerangStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class BombStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class TackStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class IceStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class GlueStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class SniperStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class SubStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class BuccaneerStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class AceStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class HeliStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class MortarStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class DartlingStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class WizardStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class SuperStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class NinjaStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class AlchemistStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class DruidStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class MermonkeyStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class SpactoryStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class FarmStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class VillageStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }
    public abstract class EngineerStat : ModContent
    {
        public override void Register() { }
        public abstract void EditTower(Tower tower);
    }

    public abstract class BoonTemplate : ModContent
    {
        public override void Register() { }
        public abstract string TowerName { get; }
        public abstract string TowerIcon { get; }
        public abstract string BoonCode { get; }
        public abstract Type TowerType { get; }
        public abstract int xPos { get; }
        public abstract int yPos { get; }
        public float cost = 2000;
        public enum Type
        {
            Primary,
            Military,
            Magic,
            Support
        }
    }

    /*public class  : AugmentTemplate
    {
        public override int SandboxIndex => ;
        public override Rarity AugmentRarity => Rarity.;
        public override string AugmentName => "";
        public override string Icon => VanillaSprites.;
        public override string TowerType => " Augment";
        public override string AugmentDescription => "";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.))
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
    }*/
}