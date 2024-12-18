using AlternatePaths.Displays.Projectiles;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Collections.Generic;
using System.Linq;
using Templates;

namespace AugmentsMod.Augments
{
    public class Juggerbomb : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Juggerbomb";
        public override string Icon => VanillaSprites.SpikeopultUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Spike-o-Pult projectile explodes when it expires dealing 2 (+2 per stack) damage and firing 8 spikes in all directions that deal 1 (+1 per stack) damage.";
        public override void EditTower()
        {
            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().Duplicate();
            var bomb = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
            var spike = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            bomb.name = "Juggerbomb_";
            bomb.projectile.GetDamageModel().damage = 2;
            bomb.projectile.pierce = 16;

            spike.GetBehavior<TravelStraitModel>().lifespan /= 1.75f;
            spike.GetDamageModel().damage = 1;
            spike.pierce = 3;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SpikeOPult))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.EnhancedEyesight))
                    {
                        bomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                        spike.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Juggerbomb")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel
                                    ("", spike, new ArcEmissionModel("", 8, 0, 360, null, true, false), bomb.fraction, bomb.durationfraction, true, false, true) { name = "JuggerbombSpikes_" });
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(effect);
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(sound);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("Juggerbomb_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 2;
                                    }
                                    else if (behavior.name.Contains("JuggerbombSpikes_"))
                                    {
                                        behavior.projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class SuperCrits : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Super Crits";
        public override string Icon => VanillaSprites.SharpShooterUpgradeIcon;
        public override string TowerType => "Dart Monkey Augment";
        public override string AugmentDescription => "Sharpshooter fires super crit bolts that deal 20 (+10 per stack) additional damage.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SharpShooter))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "SuperCrits")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetBehavior<CritMultiplierModel>().damage += 20;
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetBehavior<CritMultiplierModel>().damage += 10;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class MegaGlaives : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Mega Glaives";
        public override string Icon => VanillaSprites.MoarGlaivesUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "MOAR Glaives periodically throws out a ricochetting mega glaive that deals 1 damage and 14 (+7 per stack) additional damage to MOABs.";
        public override void EditTower()
        {
            var glaives = Game.instance.model.GetTowerFromId("BoomerangMonkey-300").GetAttackModel().Duplicate();
            glaives.name = "MegaGlaives_";
            glaives.weapons[0].projectile.GetDamageModel().damage = 1;
            glaives.weapons[0].projectile.hasDamageModifiers = true;
            glaives.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("MegaGlaives_", "Moab", 1, 14, false, false));
            glaives.weapons[0].projectile.ApplyDisplay<MegaGlaive>();
            glaives.weapons[0].projectile.scale *= 3;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MOARGlaives))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    glaives.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate * 4;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "MegaGlaives")
                        {
                            if (augment.StackIndex == 1)
                            {
                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.RedHotRangs))
                                {
                                    glaives.weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
                                }

                                towerModel.AddBehavior(glaives);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("MegaGlaives_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 7;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Multirang : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Multi-Rangs";
        public override string Icon => VanillaSprites.BionicBoomerangUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "Bionic Boomer fires 2 (+1 per stack) boomerangs at a time.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BioncBoomerang))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Multirang")
                        {
                            if (augment.StackIndex >= 1)
                            {
                                towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("aaa", (1 + augment.StackIndex), 0, (5 + 10 * augment.StackIndex), null, false, false);
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Splitrang : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Split-Rang";
        public override string Icon => VanillaSprites.MoabPressUpgradeIcon;
        public override string TowerType => "Boomerang Monkey Augment";
        public override string AugmentDescription => "MOAB Press causes boomerangs to split into 4 additional boomerangs on contact that deal 2 (+4 per stack) additional damage to MOABs.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MOABPress))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Splitrang")
                        {
                            if (augment.StackIndex == 1)
                            {
                                var rang = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();
                                towerModel.GetAttackModel().weapons[0].projectile.pierce = 1;
                                towerModel.GetAttackModel().weapons[0].projectile.maxPierce = 1;

                                rang.AddBehavior(new DamageModifierForTagModel("Splitrang_", "Moabs", 1, 2, false, false));
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("Splitrang_", rang, new ArcEmissionModel("", 4, 0, 360, null, false, false), false, false, true));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                {
                                    if (behavior.name.Contains("Splitrang_"))
                                    {
                                        behavior.projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 4;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Ripper : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Ripper";
        public override string Icon => VanillaSprites.BladeMaelstromUpgradeIcon;
        public override string TowerType => "Tack Shooter Augment";
        public override string AugmentDescription => "Blade Maelstrom allows fired blades to home in on Bloons and deal 1 (+1 per stack) additional damage.";
        public override void EditTower()
        {
            var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
            seeking.distance = 999;
            seeking.constantlyAquireNewTarget = true;
            seeking.turnRate *= 2;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BladeMaelstrom))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Ripper")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(seeking);
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3f;
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*public class ViralFrost : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Viral Frost";
        public override string Icon => VanillaSprites.SnowstormUpgradeIcon;
        public override string TowerType => "Ice Monkey Augment";
        public override string AugmentDescription => "Snowstorm allows frozen Bloons to freeze other Bloons that touch them and cause them to receive 2 (+1 per stack) additional damage from all sources while frozen.";
        public override void EditTower()
        {
            var viralFrost = Game.instance.model.GetTowerFromId("IceMonkey-004").GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
            //viralFrost.name = "ViralFrost_";
            //viralFrost.mutationId = "Ice:Icicles";
            //viralFrost.lifespan = 3;
            //viralFrost.GetBehavior<CarryProjectileModel>().projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetWeapon().projectile.display;
            //viralFrost.GetBehavior<CarryProjectileModel>().projectile.GetDamageModel().damage = 1;
            //viralFrost.GetBehavior<CarryProjectileModel>().projectile.AddBehavior(new FreezeModel("ViralFrost_", 0.5f, 3f, "ViralFreeze", 999, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, true, false));
            viralFrost.GetBehavior<CarryProjectileModel>().projectile.collisionPasses = new int[] { 0, -1 };
            //viralFrost.GetBehavior<CarryProjectileModel>().projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("ViralFrost_", "Acid_Bonus_Damage", 3f, 2, 999, true, false, false, "bleed"));
            //viralFrost.GetBehavior<CarryProjectileModel>().projectile.maxPierce = 99;
            //viralFrost.GetBehavior<CarryProjectileModel>().projectile.pierce = 99;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Snowstorm))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "ViralFrost")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(viralFrost);
                                towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>().ToArray())
                                {
                                    if (behavior.name.Contains("ViralFrost_"))
                                    {
                                        //behavior.GetBehavior<CarryProjectileModel>().projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class FrozenArtillery : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Frozen Artillery";
        public override string Icon => VanillaSprites.CryoCannonUpgradeIcon;
        public override string TowerType => "Ice Monkey Augment";
        public override string AugmentDescription => "Cryo Cannon periodically fires a cryo shell that explodes on landing freezing all Bloons hit and dealing 3 (+2 per stack) damage in a wide area.";
        public override void EditTower()
        {
            var mortar = Game.instance.model.GetTowerFromId("MortarMonkey-001").GetAttackModel().Duplicate();
            mortar.name = "FrozenArtillery_";
            mortar.RemoveBehavior<RotateToPointerModel>();
            mortar.AddBehavior(new RotateToTargetModel("", true, false, true, 0, true, true));
            mortar.weapons[0].projectile.scale -= 5f;
            mortar.weapons[0].projectile.AddBehavior(new RotateModel("targetspin", -300.0f));
            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = 3;
            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new FreezeModel("FrozenArtillery_", 0f, 2f, "ArtilleryFreeze", 999, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, true, false));
            mortar.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.collisionPasses = new int[] { 0, -1 };

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.CryoCannon))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap))
                    {
                        mortar.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "FrozenArtillery")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(mortar);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("FrozenArtillery_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 2;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class GiantGlob : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Giant Glob";
        public override string Icon => VanillaSprites.GlueStrikeUpgradeIcon;
        public override string TowerType => "Glue Gunner Augment";
        public override string AugmentDescription => "Glue Strike occasionally fires out a giant glob of glue that pushes any Bloons hit including MOABs back up to 15 (+5 per stack) units. (Gains +10% attack peed per stack)";
        public override void EditTower()
        {
            var glue = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
            glue.name = "GiantGlob_";
            glue.weapons[0].rate *= 2;
            glue.weapons[0].projectile.GetDamageModel().damage = 0;
            glue.weapons[0].projectile.pierce = 3;
            glue.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
            glue.weapons[0].projectile.AddBehavior(new WindModel("GiantGlob_", 10, 15, 1f, true, null, 0, null, 1));
            glue.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("GlueGunner").GetAttackModel().weapons[0].projectile.display;
            glue.weapons[0].projectile.scale *= 2f;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.GlueStrike))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "GiantGlob")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(glue);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<AttackModel>().ToArray())
                                {
                                    if (behavior.name.Contains("GiantGlob_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<WindModel>().distanceMax += 5;
                                        behavior.weapons[0].rate /= 1.1f;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Bloonzooka : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Bloonzooka";
        public override string Icon => VanillaSprites.MaimMoabUpgradeIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Maim MOAB periodically fires out Bloonzooka missiles that home in on Bloons and deal 6 (+2 per stack) damage and launch a spread of nails in all directions that deal 2 (+1 per stack) damage each.";
        public override void EditTower()
        {
            var homing = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();
            homing.distance = 999;
            homing.constantlyAquireNewTarget = false;

            var rocket = Game.instance.model.GetTowerFromId("BombShooter-120").GetAttackModel().weapons[0].Duplicate();
            rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().name = "Bloonzooka_";
            rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 6;
            rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            rocket.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
            rocket.projectile.GetBehavior<TravelStraitModel>().Speed /= 1.45f;
            rocket.projectile.AddBehavior(homing);
            rocket.projectile.display = Game.instance.model.GetTowerFromId("BombShooter-020").GetAttackModel().weapons[0].projectile.display;

            var nail = Game.instance.model.GetTowerFromId("EngineerMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            nail.GetDamageModel().damage = 2;
            nail.pierce = 3;
            rocket.projectile.AddBehavior(new CreateProjectileOnContactModel("Nail_", nail, new ArcEmissionModel("", 12, 0, 360, null, true, false), false, false, false));

            var bloonzooka = Game.instance.model.GetTowerFromId("SniperMonkey").GetAttackModel().Duplicate();
            bloonzooka.weapons[0] = rocket;
            bloonzooka.name = "Bloonzooka_";

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MaimMOAB))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    bloonzooka.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate * 3;

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.NightVisionGoggles))
                    {
                        bloonzooka.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                        bloonzooka.weapons[0].projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Bloonzooka")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(bloonzooka);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels().ToArray())
                                {
                                    if (attack.name.Contains("Bloonzooka_"))
                                    {
                                        foreach (var behavior in attack.weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
                                        {
                                            if (behavior.name.Contains("Bloonzooka_"))
                                            {
                                                behavior.projectile.GetDamageModel().damage += 2;
                                            }
                                            else if (behavior.name.Contains("Nails_"))
                                            {
                                                behavior.projectile.GetDamageModel().damage += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class BouncierBullets : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Bouncier Bullets";
        public override string Icon => VanillaSprites.BouncingBulletUpgradeIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Bouncing Bullet can hit 2 (+1 per stack) additional Bloons.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BouncingBullet))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "BouncierBullets")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.maxPierce += 2;
                                towerModel.GetAttackModel().weapons[0].projectile.pierce += 2;
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.maxPierce += 1;
                                towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class CryoBullets : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Cryo Bullets";
        public override string Icon => VanillaSprites.SemiAutomaticUpgradeIcon;
        public override string TowerType => "Sniper Monkey Augment";
        public override string AugmentDescription => "Semi-Automatic Rifle fires cryo bullets that can pop frozen Bloons and have a 25% (+15% per stack) chance to freeze Bloons hit for 0.5 (+0.1 per stack) seconds. " +
            "Gains the ability to freeze MOABs at 6+ stacks.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SemiAutomatic))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "CryoBullets")
                        {
                            if (augment.StackIndex == 1)
                            {
                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket)) { }
                                else
                                {
                                    towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Lead;
                                }

                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new FreezeModel("CryoBullets_", 0, 0.5f, "CryoBullet", 3, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0.25f, true, false, false));
                                towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FreezeModel>().lifespan += 0.1f;
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FreezeModel>().percentChanceToFreeze += 0.15f;
                            }
                            if (augment.StackIndex >= 6)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FreezeModel>().canFreezeMoabs = true;
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class BetterArmorPierce : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "True Armor Piercing";
        public override string Icon => VanillaSprites.ArmorPiercingDartsUpgradeIcon;
        public override string TowerType => "Monkey Sub Augment";
        public override string AugmentDescription => "Armor Piercing Darts can pop all Bloon types and deal 2 (+2 per stack) bonus damage to Fortified Bloons.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ArmorPiercingDarts))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "BetterArmorPierce")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("BetterArmorPierce_", "Fortified", 1, 2, false, false));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
                                {
                                    if (behavior.name.Contains("BetterArmorPierce_"))
                                    {
                                        behavior.damageAddative += 2;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class DepthCharges : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Depth Charges";
        public override string Icon => VanillaSprites.DestroyerUpgradeIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Destroyer creates depth charges around itself that will target Bloons that get near dealing 2 (+1 per stack) damage in a moderate area. (Gains +10% attack speed per stack.)";
        public override void EditTower()
        {
            var mines = Game.instance.model.GetTowerFromId("AdmiralBrickell").GetAttackModel(1).Duplicate();
            mines.weapons[0].name = "DepthCharges_";
            mines.name = "DepthCharges_";
            mines.weapons[0].rate /= 1.25f;
            mines.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 2;
            mines.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage = 2;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Destroyer))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    mines.range = towerModel.range;

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.CrowsNest))
                    {
                        mines.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "DepthCharges")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(mines);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("DepthCharges_"))
                                    {
                                        behavior.weapons[0].rate /= 1.1f;
                                        var mine = behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile;
                                        mine.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
                                        mine.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class SurplusCargo : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Surplus Cargo";
        public override string Icon => VanillaSprites.FavoredTradesUpgradeIcon;
        public override string TowerType => "Monkey Buccaneer Augment";
        public override string AugmentDescription => "Favored Trades produce 3 (+1 per stack) banana crates per round that are worth $50 (+$20 per stack) each.";
        public override void EditTower()
        {
            var crates = Game.instance.model.GetTowerFromId("BananaFarm-400").GetAttackModel().Duplicate();
            crates.name = "SurplusCargo_";
            crates.weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count = 3;
            crates.weapons[0].projectile.GetBehavior<CashModel>().maximum = 50f;
            crates.weapons[0].projectile.GetBehavior<CashModel>().minimum = 50f;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.FavoredTrades))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    crates.range = towerModel.range;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "SurplusCargo")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(crates);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("SurplusCargo_"))
                                    {
                                        behavior.weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 1;
                                        behavior.weapons[0].projectile.GetBehavior<CashModel>().maximum += 20f;
                                        behavior.weapons[0].projectile.GetBehavior<CashModel>().minimum += 20f;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class DeployFlares : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Deploy Flares";
        public override string Icon => VanillaSprites.FighterPlaneUpgradeIcon;
        public override string TowerType => "Monkey Ace Augment";
        public override string AugmentDescription => "Fighter Plane periodically shoots out 5 flares around it that explode stunning Bloons hit for 0.5 (+0.25 per stack) seconds and have a 30% (+5% per stack) chance to knock back small Bloons.";
        public override void EditTower()
        {
            var bomb = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.Duplicate();
            var flareExplosion = bomb.GetBehavior<CreateProjectileOnContactModel>().projectile;
            flareExplosion.GetDamageModel().damage = 0;
            flareExplosion.collisionPasses = new int[] { 0, -1 };
            flareExplosion.AddBehavior(new SlowModel("DeployFlares_", 0f, 0.5f, "Flare:stun", 3, "Stun", true, false, null, false, false, false));
            flareExplosion.AddBehavior(new WindModel("DeployFlares_", 15, 35, 0.3f, false, null, 0, null, 1));

            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().Duplicate();
            var flareEffect = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
            effect.effectModel = flareEffect;

            var flares = Game.instance.model.GetTowerFromId("MonkeyAce").GetAttackModel().Duplicate();
            flares.name = "DeployFlares_";
            flares.weapons[0].GetDescendant<ArcEmissionModel>().count = 5;
            flares.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
            flares.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan /= 4;
            flares.weapons[0].projectile.GetDamageModel().damage = 0;
            flares.weapons[0].projectile.pierce = 999;
            flares.weapons[0].projectile.AddBehavior(effect);
            flares.weapons[0].projectile.AddBehavior(new CreateProjectileOnExpireModel("DeployFlares_", flareExplosion, new SingleEmissionModel("", null), false));

            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
            flares.weapons[0].projectile.AddBehavior(sound);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.FighterPlane))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
                    {
                        flareExplosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                    }

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "DeployFlares")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(flares);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("DeployFlares_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<SlowModel>().Lifespan += 0.25f;
                                        if (behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<WindModel>().chance < 100)
                                        {
                                            behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetBehavior<WindModel>().chance += 0.05f;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class ClusterFire : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Cluster Fire";
        public override string Icon => VanillaSprites.ComancheDefenseUpgradeIcon;
        public override string TowerType => "Heli Pilot Augment";
        public override string AugmentDescription => "Comanche Defense replaces the Heli Pilot’s main attack with a cluster of buckshot rounds that fires slower but deals 1 (+1 per stack) additional damage per shot.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.ComancheDefense))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "ClusterFire")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().weapons[0].projectile.display;
                                towerModel.GetAttackModel().weapons[0].emission = new RandomEmissionModel("", 5, 36, 0, null, true, 0.9f, 1.1f, 5, false);
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                                towerModel.GetAttackModel().weapons[0].rate *= 1.5f;

                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                                {
                                    towerModel.GetAttackModel().weapons[1].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().weapons[0].projectile.display;
                                    towerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().damage += 1;
                                    towerModel.GetAttackModel().weapons[1].rate *= 1.5f;
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;

                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
                                {
                                    towerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().damage += 1;
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Aftershock : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Aftershock";
        public override string Icon => VanillaSprites.ShellShockUpgradeIcon;
        public override string TowerType => "Mortar Monkey Augment";
        public override string AugmentDescription => "Shellshock shells create 4 smaller explosions around where it lands that deal 2 (+2 per stack) damage each.";
        public override void EditTower()
        {
            var bomb = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            var mortar = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().Duplicate();
            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
            var explosion = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
            var bombEffect = Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();

            bombEffect.scale /= 2;
            effect.effectModel = bombEffect;
            explosion.radius /= 2;
            explosion.pierce = 16;
            explosion.GetDamageModel().damage = 2;

            bomb.pierce = 9999;
            bomb.GetDamageModel().damage = 0;
            bomb.GetBehavior<TravelStraitModel>().Speed /= 1.75f;
            bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            bomb.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
            bomb.AddBehavior(new CreateProjectileOnExpireModel("ExpireExplosion", explosion, new ArcEmissionModel("", 1, 0, 0, null, true, false), false));
            bomb.AddBehavior(effect);
            bomb.AddBehavior(sound);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Shockwave))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Aftershock")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("Aftershock_", bomb, new ArcEmissionModel("", 4, 0, 360, null, true, false), mortar.fraction, mortar.durationfraction, true, false, true));
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
                                {
                                    if (behavior.name.Contains("Aftershock_"))
                                    {
                                        behavior.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage += 2;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class RocketBarrage : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Rocket Barrage";
        public override string Icon => VanillaSprites.RocketStormUpgradeIcon;
        public override string TowerType => "Dartling Gunner Augment";
        public override string AugmentDescription => "Rocket Storm fires 2 (+1 per stack) rockets at a time. MAD fires additional mini rockets instead.";
        public override void EditTower()
        {
            var rockets = Game.instance.model.GetTowerFromId("DartlingGunner-040").GetAttackModel().Duplicate();
            rockets.name = "RocketBarrage_";

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.RocketStorm))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "RocketBarrage")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount += 1;

                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MAD))
                                {
                                    towerModel.GetAttackModel().weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount = 1;
                                    rockets.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate;
                                    towerModel.AddBehavior(rockets);
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                towerModel.GetAttackModel().weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount += 1;

                                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.MAD))
                                {
                                    towerModel.GetAttackModel().weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount = 1;
                                    foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                    {
                                        if (behavior.name.Contains("RocketBarrage_"))
                                        {
                                            behavior.weapons[0].GetDescendant<EmissionWithOffsetsModel>().projectileCount += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*public class InfernalGrasp : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Infernal Grasp";
        public override string Icon => VanillaSprites.SummonPhoenixUpgradeIcon;
        public override string TowerType => "Wizard Monkey Augment";
        public override string AugmentDescription => "Summon Pheonix allows the wizard to create a burning vortex around itself that pulls in Bloons and small Moabs dealing 2 (+1 per stack) damage continuously. Vortex Grab pulls in Bloons every 12 (-0.5 per stack. minimum 7) seconds";
        public override void EditTower()
        {
            var damageZone = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().Duplicate();
            damageZone.name = "InfernalGrasp_";
            damageZone.weapons[0].projectile.GetDamageModel().damage = 2;
            damageZone.weapons[0].rate = 0.5f;

            var trance = Game.instance.model.GetTowerFromId("Mermonkey-004").GetAttackModel("AttackModel_AttackTrance_").Duplicate();
            //trance.name = "InfernalTrance_";
            //trance.weapons[0].rate = 12;
            //trance.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetWeapon().projectile.display;
            //trance.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SummonPhoenix))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    damageZone.range = towerModel.range;
                    //trance.range = towerModel.range;
                    damageZone.weapons[0].projectile.radius = towerModel.range;
                    //trance.weapons[0].projectile.radius = towerModel.range;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "InfernalGrasp")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var attack in towerModel.GetAttackModels())
                                {
                                    towerModel.RemoveBehavior(attack);
                                }

                                //towerModel.AddBehavior(damageZone);

                                foreach (var attacks in Game.instance.model.GetTowerFromId("Mermonkey-004").GetAttackModels().Duplicate())
                                {
                                    //towerModel.AddBehavior(attacks);
                                }

                                towerModel.AddBehavior(trance);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels())
                                {
                                    if (behavior.name.Contains("InfernalGrasp_"))
                                    {
                                        behavior.weapons[0].projectile.GetDamageModel().damage += 1;
                                    }
                                    if (behavior.name.Contains("InfernalTrance_"))
                                    {
                                        if (behavior.weapons[0].rate > 7)
                                        {
                                            behavior.weapons[0].rate -= 0.5f;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class CheaperAvatars : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Cheaper Avatars";
        public override string Icon => VanillaSprites.SunAvatarUpgradeIcon;
        public override string TowerType => "Super Monkey Augment";
        public override string AugmentDescription => "Sun Avatars cost 10% (+10% per stack) less.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.name.Contains("AugmenterMonkey"))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "CheaperAvatars")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(new DiscountZoneModel("CheaperAvatars_", 0.1f, 1, "CheaperAvatars", "CheaperAvatars", false, 0, "MonkeyBusinessBuff", "BuffIconVillagexx1", "", "Sun Avatar", true));
                                towerModel.range = 999;
                            }
                            if (augment.StackIndex > 1 && augment.StackIndex <= 10)
                            {
                                foreach (var behavior in towerModel.GetBehaviors<DiscountZoneModel>().ToArray())
                                {
                                    if (behavior.name.Contains("CheaperAvatars_"))
                                    {
                                        behavior.discountMultiplier += 0.1f;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class MidasTouch : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Midas Touch";
        public override string Icon => VanillaSprites.RubbertoGoldUpgradeIcon;
        public override string TowerType => "Alchemist Augment";
        public override string AugmentDescription => "Rubber to Gold converts up to 5 (+1 per stack) Bloons in range to gold every 8 (-0.25 per stack. Minimum 1) seconds.";
        public override void EditTower()
        {
            var goldField = Game.instance.model.GetTowerFromId("BoomerangMonkey-500").GetAttackModel(1).Duplicate();
            goldField.name = "MidasTouch_";
            goldField.weapons[0].projectile.GetDamageModel().damage = 0;
            goldField.weapons[0].rate = 8;
            goldField.weapons[0].projectile.pierce = 5;
            goldField.weapons[0].projectile.collisionPasses = new[] { -1, 0 };
            goldField.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            goldField.fireWithoutTarget = true;

            var goldSound = Game.instance.model.GetTowerFromId("Alchemist-004").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
            var goldWorth = Game.instance.model.GetTowerFromId("Alchemist-004").GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<IncreaseBloonWorthModel>().Duplicate();
            goldField.weapons[0].projectile.AddBehavior(goldSound);
            goldField.weapons[0].projectile.AddBehavior(goldWorth);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.RubberToGold))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    goldField.weapons[0].projectile.radius = towerModel.range;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "MidasTouch")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(goldField);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("MidasTouch_"))
                                    {
                                        behavior.weapons[0].projectile.pierce += 1;
                                        if (behavior.weapons[0].rate > 1)
                                        {
                                            behavior.weapons[0].rate -= 0.25f;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class HarvestVine : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Harvest Vine";
        public override string Icon => VanillaSprites.JunglesBountyUpgradeIcon;
        public override string TowerType => "Druid Augment";
        public override string AugmentDescription => "Jungle’s Bounty vines produce bananas after grabbing a Bloon that are worth $10 (+$5 per stack).";
        public override void EditTower()
        {
            var bananas = Game.instance.model.GetTowerFromId("BananaFarm").GetAttackModel().weapons[0].projectile.Duplicate();
            bananas.GetBehavior<CashModel>().maximum = 10f;
            bananas.GetBehavior<CashModel>().minimum = 10f;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.JunglesBounty))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "HarvestVine")
                        {
                            if (augment.StackIndex == 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("JungleVine"))
                                    {
                                        behavior.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("HarvestVine_", bananas, new SingleEmissionModel("", null), false, false, false));
                                    }
                                }
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("JungleVine"))
                                    {
                                        foreach (var create in behavior.weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
                                        {
                                            if (create.name.Contains("HarvestVine_"))
                                            {
                                                create.projectile.GetBehavior<CashModel>().maximum += 5f;
                                                create.projectile.GetBehavior<CashModel>().minimum += 5f;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class Minefield : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Minefield";
        public override string Icon => VanillaSprites.SpikedMinesUpgradeIcon;
        public override string TowerType => "Spike Factory Augment";
        public override string AugmentDescription => "Spiked Mines create 3 additional explosions when they are depleted that deal 3 (+2 per stack) damage in a moder ate area.";
        public override void EditTower()
        {
            var bombs = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.Duplicate();
            bombs.display = Game.instance.model.GetTowerFromId("DartlingGunner-500").GetAttackModel().weapons[0].projectile.display;
            bombs.pierce = 999;
            bombs.GetBehavior<TravelStraitModel>().Lifespan = 0.1f;
            bombs.GetDamageModel().damage = 0;
            bombs.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            bombs.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            var effect = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().Duplicate();
            var sound = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
            var explosions = Game.instance.model.GetTowerFromId("BombShooter-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
            effect.effectModel = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
            explosions.GetDamageModel().damage = 3;
            bombs.AddBehavior(new CreateProjectileOnExpireModel("Minefield_", explosions, new SingleEmissionModel("", null), false));
            bombs.AddBehavior(effect);
            bombs.AddBehavior(sound);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.SpikedMines))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    var createExplosion = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                    createExplosion.name = "Minefield_";
                    createExplosion.projectile = bombs;
                    createExplosion.emission = new ArcEmissionModel("", 3, 0, 360, null, true, false);

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "Minefield")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(createExplosion);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
                                {
                                    if (behavior.name.Contains("Minefield_"))
                                    {
                                        behavior.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage += 2;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class BananaRepublic : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Banana Republic";
        public override string Icon => VanillaSprites.BananaPlantationUpgradeIcon;
        public override string TowerType => "Banana Farm Augment";
        public override string AugmentDescription => "Banana Plantation bananas are worth 20% (+10% per stack) more.";
        public override void EditTower()
        {
            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BananaPlantation))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "BananaRepublic")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= (1 + 0.2f * augment.StackIndex);
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= (1 + 0.2f * augment.StackIndex);
                            }
                            if (augment.StackIndex == 2)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum /= (1 + 0.2f * (augment.StackIndex - 1));
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum /= (1 + 0.2f * (augment.StackIndex - 1));

                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= (1.1f + 0.1f * augment.StackIndex);
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= (1.1f + 0.1f * augment.StackIndex);
                            }
                            if (augment.StackIndex > 2)
                            {
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum /= (1.1f + 0.1f * (augment.StackIndex - 1));
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum /= (1.1f + 0.1f * (augment.StackIndex - 1));

                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= (1.1f + 0.1f * augment.StackIndex);
                                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= (1.1f + 0.1f * augment.StackIndex);
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    /*public class EnergyBeacon : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Energy Beacon";
        public override string Icon => VanillaSprites.CallToArmsUpgradeIcon;
        public override string TowerType => "Monkey Village Augment";
        public override string AugmentDescription => "Call to Arms gains an energy beacon that zaps up to 6 (+2 per stack) nearby Bloons dealing 3 (+1 per stack) damage.";
        public override void EditTower()
        {
            var beacon = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().Duplicate();
            beacon.name = "EnergyBeacon_";
            //beacon.weapons[0].projectile.pierce = 100;
            beacon.weapons[0].projectile.GetDamageModel().damage = 1;
            beacon.weapons[0].rate /= 2.5f;
            beacon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            beacon.RemoveBehavior<RotateToTargetModel>();
            beacon.RemoveWeapon(beacon.weapons[0]);
            beacon.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.CallToArms))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    beacon.weapons[0].projectile.GetBehavior<LightningModel>().splitRange = 999;
                    beacon.weapons[0].projectile.GetBehavior<LightningModel>().splits = 1;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "EnergyBeacon")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(beacon);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("EnergyBeacon_"))
                                    {
                                        behavior.weapons[0].projectile.pierce += 2;
                                        //behavior.weapons[0].projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }*/
    public class LaserCutter : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Laser Cutter";
        public override string Icon => VanillaSprites.OverclockUpgradeIcon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Overclock gains a high-powered laser cutter that rapidly shreds Bloons that get near. Laser cutter deals 1 (+1 per stack) damage continuosly.";
        public override void EditTower()
        {
            var cutter = Game.instance.model.GetTowerFromId("BallOfLightTower").GetAttackModel().Duplicate();
            cutter.weapons[0].projectile.GetDamageModel().damage = 1;
            cutter.weapons[0].projectile.pierce = 1;
            cutter.weapons[0].projectile.maxPierce = 1;
            cutter.name = "LaserCutter_";
            cutter.weapons[0].projectile.RemoveBehavior<DamageModifierForTagModel>();
            cutter.weapons[0].rate *= 2f;
            cutter.weapons[0].projectile.GetBehavior<DisplayModel>().positionOffset = new Il2CppAssets.Scripts.Simulation.SMath.Vector3(0, 0, 500);
            cutter.weapons[0].animateOnMainAttack = false;
            cutter.weapons[0].animation = 0;
            cutter.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.Overclock))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    cutter.range = towerModel.GetAttackModel().range;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "LaserCutter")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(cutter);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("LaserCutter_"))
                                    {
                                        behavior.weapons[0].projectile.GetDamageModel().damage += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }
    public class DemoCharges : AugmentTemplate
    {
        public override int SandboxIndex => 3;
        public override Rarity AugmentRarity => Rarity.Advanced;
        public override string AugmentName => "Demo Charges";
        public override string Icon => VanillaSprites.BloonTrapUpgradeIcon;
        public override string TowerType => "Engineer Monkey Augment";
        public override string AugmentDescription => "Bloon Trap periodically throws out a demo charge that deals 3 (+1 per stack) damage and 4 (+2 per stack) bonus damage to MOABs in a wide area.";
        public override void EditTower()
        {
            var charge = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().Duplicate();
            charge.name = "DemoCharges_";
            charge.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.hasDamageModifiers = true;
            charge.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 3;
            charge.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("DemoCharges_", "Moab", 1, 4, false, false));

            foreach (var towers in InGame.instance.GetTowers().ToArray())
            {
                if (towers.towerModel.appliedUpgrades.Contains(UpgradeType.BloonTrap))
                {
                    var towerModel = towers.rootModel.Duplicate().Cast<TowerModel>();
                    charge.weapons[0].rate = towerModel.GetAttackModel().range = towerModel.range;
                    charge.weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate * 2.4f;

                    foreach (var augment in ModContent.GetContent<AugmentTemplate>().OrderByDescending(c => c.mod == mod))
                    {
                        if (augment.Name == "DemoCharges")
                        {
                            if (augment.StackIndex == 1)
                            {
                                towerModel.AddBehavior(charge);
                            }
                            if (augment.StackIndex > 1)
                            {
                                foreach (var behavior in towerModel.GetAttackModels().ToArray())
                                {
                                    if (behavior.name.Contains("DemoCharges_"))
                                    {
                                        behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
                                        behavior.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 1;
                                    }
                                }
                            }
                        }
                    }

                    towers.UpdateRootModel(towerModel);
                }
            }
        }
    }

    public class Advanced
    {
        public static List<string> AdvancedAug = [];
        public static List<string> AdvancedImg = [];
    }
}