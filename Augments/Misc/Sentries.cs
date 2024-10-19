using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using System.Linq;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Models.GenericBehaviors;

namespace AugmentSentries;

public class FireElemental : ModTower
{
    public override string Portrait => VanillaSprites.WallOfFireUpgradeIcon;
    public override string Name => "FireElemental";
    public override TowerSet TowerSet => TowerSet.Magic;
    public override string BaseTower => TowerType.DartMonkey;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;


    public override string DisplayName => "Fire Elemental";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.radius /= 2;
        towerModel.range = Game.instance.model.GetTowerFromId("WizardMonkey").range;
        towerModel.GetAttackModel().range = Game.instance.model.GetTowerFromId("WizardMonkey").range;

        towerModel.GetAttackModel().weapons[0].rate = Game.instance.model.GetTowerFromId("DartlingGunner-020").GetAttackModel().weapons[0].rate;
        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.display;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 20f, 5, false, false));
        towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
    }

    public class FireElementalDisplay : ModTowerDisplay<FireElemental>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("HotSauceCreatureTowerV2").GetBehavior<DisplayModel>().display.guidRef;

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}

public class IcicleTower : ModTower
{
    public override string Portrait => VanillaSprites.IcicleImpaleUpgradeIcon;
    public override string Name => "IcicleTower";
    public override TowerSet TowerSet => TowerSet.Primary;
    public override string BaseTower => TowerType.DartMonkey;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;


    public override string DisplayName => "Icicle Tower";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.RemoveBehavior(towerModel.GetAttackModel());
        towerModel.AddBehavior(new SlowBloonsZoneModel("IcicleDomain_", towerModel.range, "Ice:Regular:ArcticWind", true, null, 0.75f, 0, true, 0, "", false));

        var icicle = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile;
        towerModel.AddBehavior(new CreateProjectileOnExpireModel("IcicleDomain_", icicle, new ArcEmissionModel("", 8, 0, 360, null, true, false), true));

        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 15f, 5, false, false));
        towerModel.AddBehavior(new CreditPopsToParentTowerModel("CreditPopsToParentTowerModel_"));
    }

    public class FireElementalDisplay : ModTowerDisplay<FireElemental>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("SentryCold").GetBehavior<DisplayModel>().display.guidRef;

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}