using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
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

namespace AugmentsMod.Augments.Augment_Stats
{
    public class IntermediateGlueStats : GlueStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "GorillaGlue")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerGlue))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var stun = Game.instance.model.GetTowerFromId("BombShooter-400").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<SlowModel>();
                            stun.name = "GorillaGlue_";
                            stun.Lifespan = (0.35f + 0.15f * augment.StackIndex);

                            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(stun);

                            if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter))
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(stun);
                            }
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class AdvancedGlueStats : GlueStat
    {
        public override void EditTower(Tower tower)
        {
            var towerModel = tower.rootModel.Duplicate().Cast<TowerModel>();

            foreach (var augment in ModContent.GetContent<AugmentTemplate>().ToList())
            {
                if (augment.Name == "GiantGlob")
                {
                    if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueStrike))
                    {
                        if (augment.StackIndex >= 1)
                        {
                            var glue = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
                            glue.name = "GiantGlob_";
                            glue.weapons[0].rate *= 2;
                            glue.weapons[0].projectile.GetDamageModel().damage = 0;
                            glue.weapons[0].projectile.pierce = 3;
                            glue.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                            glue.weapons[0].projectile.AddBehavior(new WindModel("GiantGlob_", 10, (10 + 5 * augment.StackIndex), 1f, true, null, 0, null, 1));
                            glue.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("GlueGunner").GetAttackModel().weapons[0].projectile.display;
                            glue.weapons[0].projectile.scale *= 2f;

                            int i = 0;
                            while (i < augment.StackIndex - 1)
                            {
                                glue.weapons[0].rate /= 1.11f;
                                i++;
                            }

                            towerModel.AddBehavior(glue);
                        }
                    }
                }
            }

            tower.UpdateRootModel(towerModel);
        }
    }

    public class MasteryGlueStats : GlueStat
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
