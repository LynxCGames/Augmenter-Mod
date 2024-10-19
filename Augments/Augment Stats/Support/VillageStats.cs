using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Templates;

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateVillageStats : VillageStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "StasisField")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.GrowBlocker))
                    {
                        if (augment.StackIndex >= 1 && augment.StackIndex <= 13)
                        {
                            towerModel.AddBehavior(new SlowBloonsZoneModel("StasisField_", towerModel.range, "Ice:Regular:ArcticWind", true, new Il2CppReferenceArray<FilterModel>(new FilterModel[] { new FilterInvisibleModel("Camo", false, false) }), (0.9f - 0.05f * augment.StackIndex), 0, true, 0, "", false));
                        }
                        else if (augment.StackIndex > 13)
                        {
                            towerModel.AddBehavior(new SlowBloonsZoneModel("StasisField_", towerModel.range, "Ice:Regular:ArcticWind", true, new Il2CppReferenceArray<FilterModel>(new FilterModel[] { new FilterInvisibleModel("Camo", false, false) }), 0.25f, 0, true, 0, "", false));
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedVillageStats : VillageStat
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

    public class MasteryVillageStats : VillageStat
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
