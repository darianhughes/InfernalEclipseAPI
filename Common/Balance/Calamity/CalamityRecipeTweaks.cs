using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Common.Balance.Calamity
{
    public class CalamityRecipeTweaks : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (recipe.Mod == null)
                    continue;

                bool hasThor = false;
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                {
                    hasThor = true;
                    if (recipe.HasResult(ItemID.LivingMahoganyLeafWand) && !recipe.HasIngredient(thorium.Find<ModItem>("LivingLeaf")))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LeafWand) && !recipe.HasIngredient(thorium.Find<ModItem>("LivingLeaf")))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LivingWoodWand) && !recipe.HasIngredient(thorium.Find<ModItem>("LivingLeaf")))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LivingMahoganyWand) && !recipe.HasIngredient(thorium.Find<ModItem>("LivingLeaf")))
                        recipe.DisableRecipe();
                }

                if (ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
                {
                    if (recipe.HasIngredient(catalyst.Find<ModItem>("MetanovaBar")) && recipe.HasIngredient(ModContent.ItemType<MysteriousCircuitry>()))
                        recipe.DisableRecipe();
                    if (recipe.HasIngredient(catalyst.Find<ModItem>("MetanovaBar")) && recipe.HasResult(ModContent.ItemType<PlasmaRifle>()))
                        recipe.DisableRecipe();
                    if (recipe.HasIngredient(catalyst.Find<ModItem>("MetanovaBar")) && recipe.HasResult(ModContent.ItemType<Auralis>()))
                        recipe.DisableRecipe();
                    if (recipe.HasIngredient(catalyst.Find<ModItem>("MetanovaBar")) && recipe.HasResult(ModContent.ItemType<FreedomStar>()))
                        recipe.DisableRecipe();
                }

                if (!recipe.Mod.Name.Contains("Fargowiltas") && InfernalConfig.Instance.CalamityRecipeTweaks) //prevent us from disabling mutant mod recipes
                {
                    if (recipe.HasResult(ItemID.Starfury))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.EnchantedSword))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Muramasa))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.IceBoomerang))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.CloudinaBottle))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.PortableStool))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.HermesBoots))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BlizzardinaBottle))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SandstorminaBottle))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FrogLeg))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FlyingCarpet))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Aglet))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.AnkletoftheWind))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.WaterWalkingBoots))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ShinyRedBalloon))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LuckyHorseshoe))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.IceSkates))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LavaCharm))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ObsidianRose))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FeralClaws))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Radar))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.MetalDetector))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.DPSMeter))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BandofRegeneration))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FlareGun))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.CobaltShield))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ArmorPolish))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.AdhesiveBandage))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Bezoar))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Nazar))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Vitamins))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Blindfold))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.TrifoldMap))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FastClock))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Megaphone))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.PocketMirror))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.MagicQuiver))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FrozenTurtleShell))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Umbrella))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BugNet))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.StaffofRegrowth))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ShadowKey))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SkyMill))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.IceMachine))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.CatBast))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.TempleKey))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ShoeSpikes))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ClimbingClaws))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SlimeHook))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SlimySaddle))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.NinjaHood))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.NinjaPants))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.NinjaShirt))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BeeKeeper))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BeesKnees))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BeeGun))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.HoneyComb))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.HoneyedGoggles))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SkeletronHand))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BookofSkulls))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BreakerBlade))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ClockworkAssaultRifle))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LaserRifle))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FireWhip))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.WarriorEmblem))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SummonerEmblem))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SorcererEmblem))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.RangerEmblem))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.GrenadeLauncher))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.VenusMagnum))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.NettleBurst))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LeafBlower))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Seedler))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FlowerPow))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.WaspGun))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.PygmyStaff))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.TheAxe))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.ThornHook))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Picksaw))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Stynger))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.PossessedHatchet))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SunStone))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.EyeoftheGolem))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.HeatRay))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.StaffofEarth))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.GolemFist))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Flairon))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Tsunami))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.RazorbladeTyphoon))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.BubbleGun))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.TempestStaff))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.FishronWings))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Meowmere))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Terrarian))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.StarWrath))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.SDMG))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LastPrism))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LunarFlareBook))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.Celeb2))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.MoonlordTurretStaff))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.RainbowCrystalStaff))
                        recipe.DisableRecipe();
                    if (recipe.HasResult(ItemID.LifeCrystal))
                    {
                        if (hasThor)
                        {
                            if (!recipe.HasIngredient(thorium.Find<ModItem>("LifeQuartz")))
                            {
                                recipe.DisableRecipe();
                            }
                        }
                        else
                        {
                            recipe.DisableRecipe();
                        }
                    }
                    if (recipe.HasResult(ItemID.LifeFruit))
                        recipe.DisableRecipe();
                }
            }
        }
    }
}
