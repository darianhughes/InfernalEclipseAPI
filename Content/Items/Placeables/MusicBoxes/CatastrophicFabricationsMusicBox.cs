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
    public class CatastrophicFabricationsMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/CatastrophicFabrications"), ModContent.ItemType<CatastrophicFabricationsMusicBox>(), ModContent.TileType<CatastrophicFabricationsMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<CatastrophicFabricationsMusicBoxTile>();
            Item.width = 30;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<DraedonRelic>(1)
                .AddIngredient(ItemID.MusicBox)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
