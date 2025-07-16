using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using InfernumMode.Content.Items.Relics;
using InfernalEclipseAPI.Content.Tiles.MusicBoxes;

namespace InfernalEclipseAPI.Content.Items.Placeables.MusicBoxes
{
    public class ProphecyMusicBox : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("InfernumModeMusic", out _);
        }

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheRealityoftheProphey"), ModContent.ItemType<ProphecyMusicBox>(), ModContent.TileType<ProphecyMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<ProphecyMusicBoxTile>();
            Item.width = 32;
            Item.height = 48;
            Item.rare = ItemRarityID.Master;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod souls) && InfernalConfig.Instance.UseAprilFoolsMutant)
            {
                CreateRecipe()
                    .AddIngredient(souls.Find<ModItem>("MutantRelic"))
                    .AddIngredient(ItemID.MusicBox)
                    .AddTile(TileID.HeavyWorkBench)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient<DraedonRelic>()
                    .AddIngredient<SupremeCalamitasRelic>()
                    .AddIngredient(ItemID.Zenith)
                    .AddIngredient(ItemID.MusicBox)
                    .AddTile(TileID.HeavyWorkBench)
                    .Register();
            }
        }
    }
}
