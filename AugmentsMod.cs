using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using AugmentsMod;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Data.TrophyStore;

namespace Augmenter
{
    public class AugmenterTower : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 0;
        public override string DisplayName => "Augmenter";
        public override string Name => "AugmenterMonkey";
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Description => "The Augmenter provides random augments that enhance your towers greatly.";
        public override string Portrait => "AugmenterIcon";
        public override string Icon => "AugmenterIcon";
        public override bool IsValidCrosspath(int[] tiers) =>
            ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.range = Game.instance.model.GetTowerFromId("SniperMonkey").range;

            var attackModel = towerModel.GetAttackModel();
            towerModel.RemoveBehavior(attackModel);
        }

        public class AugmenterMainDisplay : ModTowerDisplay<AugmenterTower>
        {
            public override float Scale => 1f;
            public override string BaseDisplay => GetDisplay(TowerType.EngineerMonkey, 0, 4);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {

            }
        }
    }
}
