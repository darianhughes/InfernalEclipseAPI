using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria.Localization;
using CalamityMod.Items.SummonItems;
using InfernumMode.Content.Items.SummonItems;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using static NoxusBoss.Assets.GennedAssets.Sounds;
using CalamityMod.Items;
using InfernalEclipseAPI.Content.Items.Weapons.Swordofthe14thGlitch;
using InfernalEclipseAPI.Content.Items.Weapons.NovaBomb;
using InfernumMode.Content.Items.Weapons.Magic;
using InfernalEclipseAPI.Content.Items.Weapons.ChromaticMassInABottle;
using ThoriumMod.Contracts;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Accessories;
using CalamityMod;
using ThoriumMod.Items.Misc;
using CalamityMod.Items.Weapons.Rogue;
using InfernalEclipseAPI.Content.Items.Weapons.StellarSabre;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Magic;

namespace InfernalEclipseAPI.Common.Balance.Recipes
{
    internal sealed class RecipeTweaks : ModSystem
    {
        public static RecipeGroup EvilSkinRecipeGroup;
        public override void Unload()
        {
            EvilSkinRecipeGroup = null;
        }
        public override void AddRecipeGroups()
        {
            EvilSkinRecipeGroup = new RecipeGroup(() => $"{Lang.GetItemNameValue(ItemID.ShadowScale)} or {Lang.GetItemNameValue(ItemID.TissueSample)}", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup("LimitedResourcesRecipes:EvilSkin", EvilSkinRecipeGroup);
        }
        public override void PostAddRecipes()
        {
            base.PostAddRecipes();

            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);

            foreach (var recipe in Main.recipe)
            {

                if (!ModLoader.TryGetMod("PackBuilder", out Mod tPack))
                {
                    if (recipe.HasResult(ModContent.ItemType<Kevin>()))
                    {
                        recipe.AddIngredient(ModContent.ItemType<Rock>(), 1);
                    }

                    if (InfernalConfig.Instance.VanillaBalanceChanges)
                    {
                        if (recipe.HasResult(ItemID.Zenith))
                        {
                            recipe.RemoveIngredient(ItemID.EnchantedSword);
                            recipe.AddIngredient(ItemID.Terragrim, 1);
                        }
                    }
                }

                //Calamity
                //If any mods allow the terminus to be crafted, make it post-Primordial Wyrm.
                if (recipe.HasResult(ModContent.ItemType<Terminus>()))
                {
                    //recipe.AddIngredient(ModContent.ItemType<EvokingSearune>(), 1);
                    recipe.DisableRecipe();
                }

                if (recipe.HasResult(ModContent.ItemType<OnyxExcavatorKey>()))
                {
                    recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 3);
                }

                if (thorium != null && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    if (recipe.HasResult(ModContent.ItemType<NuclearFury>()))
                    {
                        recipe.RemoveIngredient(ItemID.RazorbladeTyphoon);
                        recipe.AddIngredient(thorium.Find<ModItem>("NuclearFury").Type);
                    }
                }

                if (InfernalConfig.Instance.CalamityRecipeTweaks)
                {
                    //Cosmilite post-DoG - Maybe add its own config?
                    if (recipe.HasResult<Cosmolight>())
                    {
                        recipe.AddIngredient<CosmiliteBar>(1);
                        recipe.RemoveTile(16);
                        recipe.AddTile(ModContent.TileType<CosmicAnvil>());
                    }

                    if (recipe.HasResult(ModContent.ItemType<OnyxExcavatorKey>()))
                        recipe.DisableRecipe();

                    if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                    {
                        if (recipe.HasResult(ModContent.ItemType<EmpyreanKnives>()))
                        {
                            recipe.RemoveIngredient(ModContent.ItemType<CosmiliteBar>());
                            recipe.RemoveIngredient(ModContent.ItemType<DarksunFragment>());
                            recipe.AddIngredient<EffulgentFeather>(8);
                            recipe.RemoveTile(ModContent.TileType<CosmicAnvil>());
                            recipe.AddTile(TileID.LunarCraftingStation);
                        }
                    }
                }

                if (InfernalConfig.Instance.ThoriumBalanceChangess && thorium != null && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                {
                    if (recipe.HasResult(ModContent.ItemType<ElementalDisk>()))
                    {
                        thorium.TryFind("TerraKnife", out ModItem terraKnife);
                        recipe.AddIngredient(terraKnife.Type);
                    }

                    if (recipe.HasResult(thorium.Find<ModItem>("NuclearFury").Type))
                    {
                        recipe.AddIngredient(thorium.Find<ModItem>("WhiteDwarfFragment").Type, 6);
                        recipe.RemoveTile(TileID.Bookcases);
                        recipe.AddTile(TileID.LunarCraftingStation);
                    }
                }

                if (ModLoader.TryGetMod("ZenithToilet", out Mod toilet))
                {
                    if (toilet.TryFind("ZenithToilet", out ModItem zToilet))
                    {
                        if (recipe.HasResult(zToilet))
                        {
                            recipe.AddIngredient(ModContent.ItemType<Rock>(), 1);
                            recipe.RemoveTile(TileID.MythrilAnvil);
                            recipe.AddIngredient<AshesofAnnihilation>(3);
                            recipe.AddIngredient<MiracleMatter>(3);
                            recipe.AddIngredient<ShadowspecBar>(3);
                            recipe.AddTile(ModContent.TileType<DraedonsForge>());
                        }
                    }

                    if (toilet.TryFind("TrueZenithToilet", out ModItem trueToilet))
                    {
                        if (recipe.HasResult(trueToilet))
                        {
                            recipe.RemoveTile(TileID.MythrilAnvil);
                            recipe.AddTile(ModContent.TileType<DraedonsForge>());
                            recipe.AddIngredient(ModContent.ItemType<StellarSabre>(), 1);
                            recipe.AddIngredient(ModContent.ItemType<Swordofthe14thGlitch>(), 1);
                            recipe.AddIngredient(ModContent.ItemType<NovaBomb>(), 1);
                            recipe.AddIngredient(ModContent.ItemType<Kevin>(), 1);
                            recipe.AddIngredient(ModContent.ItemType<ChromaticMassInABottle>(), 1);
                            recipe.AddIngredient(ModContent.ItemType<Rock>(), 1);
                        }
                    }
                }

                //Thorium
                ModItem holySycthe = null;
                if (thorium != null)
                {
                    //Materials
                    thorium.TryFind("NinjaEmblem", out ModItem ninjaEmblem);
                    thorium.TryFind("BloomWeave", out ModItem bloomWeave);
                    thorium.TryFind("MermaidCanteen", out ModItem mermaidCanteen);
                    thorium.TryFind("TerrariumHolyScythe", out holySycthe);

                    if (InfernalConfig.Instance.DisableDuplicateContent)
                    {
                        if (thorium.TryFind("CobaltThrowingSpear", out ModItem cbthrowspear))
                        {
                            if (recipe.HasResult(cbthrowspear))
                                recipe.DisableRecipe();
                        }

                        if (thorium.TryFind("PalladiumThrowingSpear", out ModItem palthrowspear))
                        {
                            if (recipe.HasResult(palthrowspear))
                                recipe.DisableRecipe();
                        }

                        if (thorium.TryFind("IronTomahawk", out ModItem ironToma))
                        {
                            if (recipe.HasResult(ironToma))
                            {
                                recipe.DisableRecipe();
                            }
                        }

                        if (thorium.TryFind("LeadTomahawk", out ModItem leadToma))
                        {
                            if (recipe.HasResult(leadToma))
                            {
                                recipe.DisableRecipe();
                            }
                        }

                        if (thorium.TryFind("LodestoneJavelin", out ModItem lodeJav))
                            if (recipe.HasResult(lodeJav))
                                recipe.DisableRecipe();

                        if (thorium.TryFind("ValadiumThrowingAxe", out ModItem valdiumAxe))
                            if (recipe.HasResult(valdiumAxe))
                                recipe.DisableRecipe();

                        if (thorium.TryFind("AromaticBulb", out ModItem bulb))
                        {
                            if (recipe.HasResult(bulb))
                                recipe.DisableRecipe();
                        }

                        string[] coatings =
                        {
                            "DeepFreezeCoatingItem",
                            "ExplosiveCoatingItem",
                            "GorgonCoatingItem",
                            "SporeCoatingItem",
                            "ToxicCoatingItem",
                        };

                        foreach (string coating in coatings)
                            if (thorium.TryFind(coating, out ModItem coatingItem))
                                if (recipe.HasResult(coatingItem))
                                    recipe.DisableRecipe();

                        if (thorium.TryFind("AdamantiteGlaive", out ModItem adamGlaive))
                            if (recipe.HasResult(adamGlaive))
                                recipe.DisableRecipe();

                        if (thorium.TryFind("TitaniumGlaive", out ModItem titanGlaive))
                            if (recipe.HasResult(titanGlaive))
                                recipe.DisableRecipe();

                        string[] disabledItems =
                        {
                            "KineticPotion",
                            "ChlorophyteTomahawk",
                            "DemonBloodBow",
                            "MyceliumGatlingGun"
                        };

                        foreach (string item in disabledItems)
                            if (thorium.TryFind(item, out ModItem tempItem))
                                if (recipe.HasResult(tempItem))
                                    recipe.DisableRecipe();
                    }

                    if (InfernalConfig.Instance.ThoriumBalanceChangess)
                    {
                        if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                        {
                            if (thorium.TryFind("ThrowingGuideVolume2", out ModItem rogue101v2))
                            {
                                if (recipe.HasResult(rogue101v2))
                                {
                                    recipe.RemoveIngredient(ninjaEmblem.Type);
                                    recipe.AddIngredient(ModContent.ItemType<RogueEmblem>());
                                    recipe.AddIngredient(bloomWeave.Type, 5);
                                    recipe.AddIngredient(mermaidCanteen.Type);
                                }
                            }

                            if (thorium.TryFind("ThrowingGuideVolume3", out ModItem rouge101v3))
                            {
                                if (recipe.HasResult(rouge101v3))
                                {
                                    recipe.RemoveIngredient(ItemID.SoulofSight);
                                    recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 3);
                                    recipe.RemoveIngredient(ItemID.SoulofMight);
                                    recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 4);
                                    recipe.RemoveIngredient(ItemID.SoulofFright);
                                }
                            }

                            if (thorium.TryFind("Mjolnir", out ModItem thorsHammer))
                            {
                                if (recipe.HasResult(thorsHammer))
                                {
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 5);
                                }
                            }

                            if (thorium.TryFind("DarkContagion", out ModItem darkCont))
                            {
                                if (recipe.HasResult(darkCont))
                                {
                                    recipe.RemoveIngredient(ItemID.Ichor);
                                    recipe.RemoveIngredient(ItemID.SpellTome);
                                    recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 5);
                                    recipe.AddIngredient(ItemID.Deathweed, 2);
                                }
                            }

                            if (thorium.TryFind("AssassinsGuard", out ModItem assass1))
                            {
                                if (recipe.HasResult(assass1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("AssassinsWalkers", out ModItem assass2))
                            {
                                if (recipe.HasResult(assass2))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("DreamWeaversHelmet", out ModItem dreamweave1))
                            {
                                if (recipe.HasResult(dreamweave1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("DreamWeaversHood", out ModItem dreamweave2))
                            {
                                if (recipe.HasResult(dreamweave2))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("DreamWeaversTabard", out ModItem dreamweave3))
                            {
                                if (recipe.HasResult(dreamweave3))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("DreamWeaversTreads", out ModItem dreamweave4))
                            {
                                if (recipe.HasResult(dreamweave4))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("InspiratorsHelmet", out ModItem inspir1))
                            {
                                if (recipe.HasResult(inspir1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("MagmaSeerMask", out ModItem magmaseer1))
                            {
                                if (recipe.HasResult(magmaseer1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("MasterArbalestHood", out ModItem masterArba1))
                            {
                                if (recipe.HasResult(masterArba1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("MasterMarksmansScouter", out ModItem masterMark1))
                            {
                                if (recipe.HasResult(masterMark1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("PyromancersCowl", out ModItem pyro1))
                            {
                                if (recipe.HasResult(pyro1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("PyromancersLeggings", out ModItem pyro2))
                            {
                                if (recipe.HasResult(pyro2))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("PyromancersTabard", out ModItem pyro3))
                            {
                                if (recipe.HasResult(pyro3))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("RhapsodistBoots", out ModItem rhap1))
                            {
                                if (recipe.HasResult(rhap1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("RhapsodistChestWoofer", out ModItem rhap2))
                            {
                                if (recipe.HasResult(rhap2))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("SoloistsHat", out ModItem solo1))
                            {
                                if (recipe.HasResult(solo1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("TideTurnerBreastplate", out ModItem tide1))
                            {
                                if (recipe.HasResult(tide1))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("TideTurnerGreaves", out ModItem tide2))
                            {
                                if (recipe.HasResult(tide2))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("TideTurnerHelmet", out ModItem tide3))
                            {
                                if (recipe.HasResult(tide3))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }
                            if (thorium.TryFind("TideTurnersGaze", out ModItem tide4))
                            {
                                if (recipe.HasResult(tide4))
                                {
                                    recipe.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                                    recipe.AddIngredient(ModContent.ItemType<AuricBar>());
                                }
                            }

                            ModItem[] preMechIngredients =
                            {
                                GetItem(thorium, "BenignBalloon"),
                                GetItem(thorium, "AbyssalChitin"),
                                GetItem(thorium, "CrystalGeode"),
                                GetItem(thorium, "UnfathomableFlesh"),
                                GetItem(thorium, "GreenDragonScale"),
                                GetItem(thorium, "HallowedCharm"),
                                GetItem(thorium, "LodeStoneIngot"),
                                GetItem(thorium, "CeruleanMorel"),
                                GetItem(thorium, "PharaohsBreath"),
                                GetItem(thorium, "SoulofPlight"),
                                GetItem(thorium, "ValadiumIngot"),
                                GetItem(thorium, "PotionChaser"),
                                GetItem(thorium, "BronzeAlloyFragments")
                            };

                            ModItem[] preMechItems = 
                            {
                                GetItem(thorium, "CrystalGeode"),
                                GetItem(thorium, "UnfathomableFlesh"),
                                GetItem(thorium, "GreenDragonScale"),
                                GetItem(thorium, "SupportSash"),
                                GetItem(thorium, "BalanceBloom"),
                                GetItem(thorium, "ChronoOcarina"),
                                GetItem(thorium, "CometCrossfire"),
                                GetItem(thorium, "CorruptlingStaff"),
                                GetItem(thorium, "CrimsonHoundStaff"),
                                GetItem(thorium, "FrostwindCymbals"),
                                GetItem(thorium, "IridescentStaff"),
                                GetItem(thorium, "LustrousBaton"),
                                GetItem(thorium, "MastersLibram"),
                                GetItem(thorium, "Omniwrench"),
                                GetItem(thorium, "StellarSystem"),
                                GetItem(thorium, "Violin"),
                                GetItem(thorium, "WindChimes")
                            };

                            if (recipe.HasIngredient(ItemID.DynastyWood) && recipe.HasTile(TileID.MythrilAnvil))
                            {
                                recipe.RemoveTile(TileID.MythrilAnvil);
                                recipe.AddTile(TileID.Anvils);
                            }

                            foreach (ModItem item in preMechIngredients)
                            {
                                if (recipe.HasIngredient(item) && recipe.HasTile(TileID.MythrilAnvil))
                                {
                                    recipe.RemoveTile(TileID.MythrilAnvil);
                                    recipe.AddTile(TileID.Anvils);
                                }
                            }

                            foreach (ModItem iitem in preMechItems)
                            {
                                if (recipe.HasResult(iitem) && recipe.HasTile(TileID.MythrilAnvil))
                                {
                                    recipe.RemoveTile(TileID.MythrilAnvil);
                                    recipe.AddTile(TileID.Anvils);
                                }
                            }
                        }
                    }
                }

                //Ragnarok
                if (ModLoader.TryGetMod("RagnarokMod", out Mod ragCal))
                {
                    ragCal.TryFind("JellySlicer", out ModItem gelSlicer);

                    if (recipe.HasResult(gelSlicer) && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                    {
                        recipe.RemoveTile(TileID.Anvils);
                        recipe.AddTile(ModContent.TileType<StaticRefiner>());
                    }

                    ragCal.TryFind("UniversalHeadset", out ModItem uniHeadset);

                    if (recipe.HasResult(uniHeadset) && InfernalConfig.Instance.ThoriumBalanceChangess)
                    {
                        recipe.RemoveIngredient(ItemID.LunarBar);
                        recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 8);
                    }

                    ragCal.TryFind("ExecutionerMark05", out ModItem exMark5);
                    ragCal.TryFind("ElementalReaper", out ModItem elementalReaper);

                    if (recipe.HasResult(exMark5) && holySycthe != null && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                    {
                        recipe.RemoveIngredient(holySycthe.Type);
                        recipe.AddIngredient(elementalReaper.Type);
                    }
                }

                //Thorium Bosses Reworked
                if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework) && InfernalConfig.Instance.ThoriumBalanceChangess)
                {
                    if (!ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                    {
                        if (thorRework.TryFind("Loudener", out ModItem loud))
                        {
                            if (recipe.HasResult(loud))
                            {
                                recipe.AddIngredient(thorium.Find<ModItem>("BronzeAlloyFragments"), 5);
                            }
                        }

                        if (thorRework.TryFind("ImpulseAmplifier", out ModItem impulse))
                        {
                            if (recipe.HasResult(impulse))
                            {
                                recipe.RemoveIngredient(ItemID.Wire);
                                recipe.AddIngredient(ModContent.ItemType<StormlionMandible>(), 1);
                            }
                        }

                        if (thorRework.TryFind("ExecutionersContract", out ModItem contract))
                        {
                            if (recipe.HasResult(contract))
                            {
                                recipe.RemoveTile(TileID.Loom);
                                recipe.AddTile(TileID.LunarCraftingStation);
                                recipe.AddIngredient(thorium.Find<ModItem>("CelestialFragment"), 5);
                            }
                        }

                        if (thorRework.TryFind("SealedContract", out ModItem sealedContract))
                        {
                            if (recipe.HasResult(sealedContract))
                            {
                                recipe.RemoveTile(TileID.MythrilAnvil);
                                recipe.AddTile(ModContent.TileType<CosmicAnvil>());
                                recipe.AddIngredient(ModContent.ItemType<RuinousSoul>());
                            }
                        }

                        if (thorRework.TryFind("FanDonations", out ModItem fanDOnations))
                        {
                            if (recipe.HasResult(fanDOnations))
                            {
                                recipe.RemoveTile(TileID.WorkBenches);
                                recipe.AddTile(TileID.LunarCraftingStation);
                            }
                        }
                    }

                    if (thorRework.TryFind("DeathsingerPotion", out ModItem deathSingerPotion) && recipe.HasResult(deathSingerPotion))
                    {
                        recipe.DisableRecipe();
                    }
                }

                //Unofficial Calamity Bard & Healer
                if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && InfernalConfig.Instance.ThoriumBalanceChangess)
                {
                    if (calBardHeal.TryFind("SongoftheAncients", out ModItem songAncinet))
                    {
                        if (recipe.HasResult(songAncinet))
                        {
                            thorium.TryFind("BrokenHeroFragment", out ModItem heroFragment);
                            recipe.ChangeIngredientStack(heroFragment.Type, 3);
                        }
                    }

                    if (ragCal != null)
                    {
                        if (calBardHeal.TryFind("OmniSpeaker", out ModItem omniSpeak))
                        {
                            if (recipe.HasResult(omniSpeak))
                            {
                                ragCal.TryFind("SigilOfACruelWorld", out ModItem sigil);
                                recipe.AddIngredient(sigil.Type);
                            }
                        }
                    }

                    if (calBardHeal.TryFind("Syzygy", out ModItem syzygy) && !ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                    {
                        if (recipe.HasResult(syzygy))
                        {
                            thorium.TryFind("TerraScythe", out ModItem terraSycthe);

                            recipe.RemoveIngredient(terraSycthe.Type);
                            recipe.AddIngredient(holySycthe.Type);
                        }
                    }

                    if (calBardHeal.TryFind("CherubimOmega", out ModItem healOmega))
                    {
                        if (recipe.HasResult(healOmega))
                            recipe.RemoveIngredient(holySycthe.Type);
                    }
                }

                if (InfernalConfig.Instance.SOTSBalanceChanges)
                {
                    //SOTS
                    if (ModLoader.TryGetMod("SOTS", out Mod sots))
                    {
                        // Frigid Pickaxe
                        if (sots.TryFind("FrigidPickaxe", out ModItem frigidPick))
                        {
                            if (recipe.HasResult(frigidPick))
                            {
                                recipe.AddRecipeGroup(EvilSkinRecipeGroup, 6);
                            }
                        }

                        // Challenger's Ring
                        if (sots.TryFind("ChallengerRing", out ModItem challRing))
                        {
                            if (recipe.HasResult(challRing))
                            {
                                if (sots.TryFind("PhaseBar", out ModItem phaseBar))
                                {
                                    recipe.AddIngredient(phaseBar, 6);
                                }
                                else
                                {
                                    recipe.AddIngredient(ItemID.Ectoplasm, 6);
                                }
                            }
                        }
                    }
                }
            }
        }

        private ModItem GetItem(Mod mod, string name)
        {
            return mod.Find<ModItem>(name);
        }
    }
}
