using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Templates;
using BTD_Mod_Helper.Api.Enums;

namespace AugmentsMod.Augments.Misc
{
    public class DartBoon : BoonTemplate
    {
        public override string TowerName => "Dart Monkey";
        public override string TowerIcon => VanillaSprites.DartMonkeyIcon;
        public override string BoonCode => "Dart Monkey";
        public override int xPos => -900;
        public override int yPos => 600;
        
        public override Type TowerType => Type.Primary;
    }
    public class BoomerangBoon : BoonTemplate
    {
        public override string TowerName => "Boomerang Monkey";
        public override string TowerIcon => VanillaSprites.BoomerangMonkeyIcon;
        public override string BoonCode => "Boomerang Monkey";
        public override int xPos => -600;
        public override int yPos => 600;
        public override Type TowerType => Type.Primary;
    }
    public class BombBoon : BoonTemplate
    {
        public override string TowerName => "Bomb Shooter";
        public override string TowerIcon => VanillaSprites.BombShooterIcon;
        public override string BoonCode => "Bomb Shooter";
        public override int xPos => -300;
        public override int yPos => 600;
        public override Type TowerType => Type.Primary;
    }
    public class TackBoon : BoonTemplate
    {
        public override string TowerName => "Tack Shooter";
        public override string TowerIcon => VanillaSprites.TackShooterIcon;
        public override string BoonCode => "Tack Shooter";
        public override int xPos => 0;
        public override int yPos => 600;
        public override Type TowerType => Type.Primary;
    }
    public class IceBoon : BoonTemplate
    {
        public override string TowerName => "Ice Monkey";
        public override string TowerIcon => VanillaSprites.IceMonkeyIcon;
        public override string BoonCode => "Ice Monkey";
        public override int xPos => 300;
        public override int yPos => 600;
        public override Type TowerType => Type.Primary;
    }
    public class GlueBoon : BoonTemplate
    {
        public override string TowerName => "Glue Gunner";
        public override string TowerIcon => VanillaSprites.GlueGunnerIcon;
        public override string BoonCode => "Glue Gunner";
        public override int xPos => 600;
        public override int yPos => 600;
        public override Type TowerType => Type.Primary;
    }
    public class SniperBoon : BoonTemplate
    {
        public override string TowerName => "Sniper Monkey";
        public override string TowerIcon => VanillaSprites.SniperMonkeyIcon;
        public override string BoonCode => "Sniper Monkey";
        public override int xPos => -900;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class SubBoon : BoonTemplate
    {
        public override string TowerName => "Monkey Sub";
        public override string TowerIcon => VanillaSprites.MonkeySubIcon;
        public override string BoonCode => "Monkey Sub";
        public override int xPos => -600;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class BuccaneerBoon : BoonTemplate
    {
        public override string TowerName => "Monkey Buccaneer";
        public override string TowerIcon => VanillaSprites.MonkeyBuccaneerIcon;
        public override string BoonCode => "Monkey Buccaneer";
        public override int xPos => -300;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class AceBoon : BoonTemplate
    {
        public override string TowerName => "Monkey Ace";
        public override string TowerIcon => VanillaSprites.MonkeyAceIcon;
        public override string BoonCode => "Monkey Ace";
        public override int xPos => 0;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class HeliBoon : BoonTemplate
    {
        public override string TowerName => "Heli Pilot";
        public override string TowerIcon => VanillaSprites.HeliPilotIcon;
        public override string BoonCode => "Heli Pilot";
        public override int xPos => 300;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class MortarBoon : BoonTemplate
    {
        public override string TowerName => "Mortar Monkey";
        public override string TowerIcon => VanillaSprites.MortarMonkeyIcon;
        public override string BoonCode => "Mortar Monkey";
        public override int xPos => 600;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class DartlingBoon : BoonTemplate
    {
        public override string TowerName => "Dartling Gunner";
        public override string TowerIcon => VanillaSprites.DartlingGunnerIcon;
        public override string BoonCode => "Dartling Gunner";
        public override int xPos => 900;
        public override int yPos => 150;
        public override Type TowerType => Type.Military;
    }
    public class WizardBoon : BoonTemplate
    {
        public override string TowerName => "Wizard Monkey";
        public override string TowerIcon => VanillaSprites.WizardIcon;
        public override string BoonCode => "Wizard Monkey";
        public override int xPos => -900;
        public override int yPos => -300;
        public override Type TowerType => Type.Magic;
    }
    public class SuperBoon : BoonTemplate
    {
        public override string TowerName => "Super Monkey";
        public override string TowerIcon => VanillaSprites.SuperMonkeyIcon;
        public override string BoonCode => "Super Monkey";
        public override int xPos => -600;
        public override int yPos => -300;
        public override Type TowerType => Type.Magic;
    }
    public class NinjaBoon : BoonTemplate
    {
        public override string TowerName => "Ninja Monkey";
        public override string TowerIcon => VanillaSprites.NInjaMonkeyIcon;
        public override string BoonCode => "Ninja Monkey";
        public override int xPos => -300;
        public override int yPos => -300;
        public override Type TowerType => Type.Magic;
    }
    public class AlchemistBoon : BoonTemplate
    {
        public override string TowerName => "Alchemist";
        public override string TowerIcon => VanillaSprites.AlchemistIcon;
        public override string BoonCode => "Alchemist";
        public override int xPos => 0;
        public override int yPos => -300;
        public override Type TowerType => Type.Magic;
    }
    public class DruidBoon : BoonTemplate
    {
        public override string TowerName => "Druid";
        public override string TowerIcon => VanillaSprites.DruidIcon;
        public override string BoonCode => "Druid";
        public override int xPos => 300;
        public override int yPos => -300;
        public override Type TowerType => Type.Magic;
    }
    public class MermonkeyBoon : BoonTemplate
    {
        public override string TowerName => "Mermonkey";
        public override string TowerIcon => VanillaSprites.MermonkeyIcon;
        public override string BoonCode => "Mermonkey";
        public override int xPos => 600;
        public override int yPos => -300;
        public override Type TowerType => Type.Magic;
    }
    public class SpactoryBoon : BoonTemplate
    {
        public override string TowerName => "Spike Factory";
        public override string TowerIcon => VanillaSprites.SpikeFactoryIcon;
        public override string BoonCode => "Spike Factory";
        public override int xPos => -900;
        public override int yPos => -750;
        public override Type TowerType => Type.Support;
    }
    /*public class FarmBoon : BoonTemplate
    {
        public override string TowerName => "Banana Farm";
        public override string TowerIcon => VanillaSprites.BananaFarmIcon2;
        public override string BoonCode => "Banana Farm";
        public override int xPos => -600;
        public override int yPos => -750;
        public override Type TowerType => Type.Support;
    }*/
    public class VillageBoon : BoonTemplate
    {
        public override string TowerName => "Monkey Village";
        public override string TowerIcon => VanillaSprites.MonkeyVillageIcon;
        public override string BoonCode => "Monkey Village";
        public override int xPos => -300;
        public override int yPos => -750;
        public override Type TowerType => Type.Support;
    }
    public class EngineerBoon : BoonTemplate
    {
        public override string TowerName => "Engineer Monkey";
        public override string TowerIcon => VanillaSprites.EngineerMonkeyicon;
        public override string BoonCode => "Engineer Monkey";
        public override int xPos => 0;
        public override int yPos => -750;
        public override Type TowerType => Type.Support;
    }
}
