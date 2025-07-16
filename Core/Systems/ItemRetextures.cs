using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.Systems
{
    public class ItemRetextures : ModSystem
    {
        public override void PostSetupContent()
        {
            // Try to get the mod and item
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium)
                || !thorium.TryFind("NinjaEmblem", out ModItem ninjaEmblem))
                return;

            int type = ninjaEmblem.Type;
            string replacementPath = "InfernalEclipseAPI/Assets/Textures/Items/HeroEmblem";

            // Replace the texture in TextureAssets.Item
            TextureAssets.Item[type] = ModContent.Request<Texture2D>(replacementPath, AssetRequestMode.ImmediateLoad);
        }

        public override void Unload()
        {
            // Restore or unload the replaced texture
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium)
                && thorium.TryFind("NinjaEmblem", out ModItem ninjaEmblem))
            {
                TextureAssets.Item[ninjaEmblem.Type] = null;
            }
        }
    }
}
