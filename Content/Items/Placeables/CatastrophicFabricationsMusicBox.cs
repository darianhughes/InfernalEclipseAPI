using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using InfernalEclipseAPI.Content.Tiles;
using InfernumMode.Content.Items.Relics;

namespace InfernalEclipseAPI.Content.Items.Placeables
{
    public class CatastrophicFabricationsMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            this.Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[this.Type] = false;
            ItemID.Sets.ShimmerTransformToItem[this.Type] = 576;
            MusicLoader.AddMusicBox(((ModType)this).Mod, MusicLoader.GetMusicSlot(((ModType)this).Mod, "Assets/Music/CatastrophicFabrications"), ModContent.ItemType<CatastrophicFabricationsMusicBox>(), ModContent.TileType<CatastrophicFabricationsMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            this.Item.useStyle = 1;
            this.Item.useTurn = true;
            this.Item.useAnimation = 15;
            this.Item.useTime = 10;
            this.Item.autoReuse = true;
            this.Item.consumable = true;
            this.Item.createTile = ModContent.TileType<CatastrophicFabricationsMusicBoxTile>();
            ((Entity)this.Item).width = 30;
            ((Entity)this.Item).height = 20;
            this.Item.rare = 4;
            this.Item.value = 100000;
            this.Item.accessory = true;
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
