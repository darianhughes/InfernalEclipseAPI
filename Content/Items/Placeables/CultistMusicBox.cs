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
    public class CultistMusicBox : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("InfernumModeMusic", out _);
        }

        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Items/PlaceholderInfernumMusicBox";

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[this.Type] = false;
            ItemID.Sets.ShimmerTransformToItem[this.Type] = 576;
            ModLoader.TryGetMod("InfernumModeMusic", out Mod InfernumMusic);
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(InfernumMusic, "Sounds/Music/LunaticCultist"), ModContent.ItemType<CultistMusicBox>(), ModContent.TileType<CultistMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<CultistMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LunaticCultistRelic>(1)
                .AddIngredient(ItemID.MusicBox)
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
