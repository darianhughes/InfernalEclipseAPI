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
    public class EyeMusicBox : ModItem
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
            ItemID.Sets.CanGetPrefixes[this.Type] = false;
            ItemID.Sets.ShimmerTransformToItem[this.Type] = 576;
            ModLoader.TryGetMod("InfernumModeMusic", out Mod InfernumMusic);
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(InfernumMusic, "Sounds/Music/EyeOfCthulhu"), ModContent.ItemType<EyeMusicBox>(), ModContent.TileType<EyeMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<EyeMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = 4;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EyeOfCthulhuRelic>(1)
                .AddIngredient(ItemID.MusicBox)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
