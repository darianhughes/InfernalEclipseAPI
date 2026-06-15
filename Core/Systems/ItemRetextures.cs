using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace InfernalEclipseAPI.Core.Systems
{
    public class ItemRetextures : ModSystem
    {
        public override void PostSetupContent()
        {
            // Try to get the mod and item
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium)
                && thorium.TryFind("NinjaEmblem", out ModItem ninjaEmblem))
            {
                int type = ninjaEmblem.Type;
                string replacementPath = "InfernalEclipseAPI/Assets/Textures/Items/HeroEmblem";

                // Replace the texture in TextureAssets.Item
                TextureAssets.Item[type] = ModContent.Request<Texture2D>(replacementPath, AssetRequestMode.ImmediateLoad);
            }

            if (ModLoader.TryGetMod("ClamityMusic", out Mod clam))
            {
                if (clam.TryFind("ClamityTitleMusicBox", out ModItem clamTitleMusicBox))
                {
                    int type = clamTitleMusicBox.Type;
                    string replacementPath = "InfernalEclipseAPI/Assets/Textures/Items/ClamityTitleScreen";

                    TextureAssets.Item[type] = ModContent.Request<Texture2D>(replacementPath, AssetRequestMode.ImmediateLoad);
                }

                if (clam.TryFind("ClamityTitleMusicBoxTile", out ModTile clamTitleMusicBoxTile))
                {
                    int type = clamTitleMusicBoxTile.Type;
                    string replacementPath = "InfernalEclipseAPI/Assets/Textures/Tiles/ClamityTitleScreenTile";

                    TextureAssets.Tile[type] = ModContent.Request<Texture2D>(replacementPath, AssetRequestMode.ImmediateLoad);
                }
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                if (ModLoader.TryGetMod("InfernalEclipseWeaponsDLC", out _))
                {
                    int type = ragnarok.Find<ModItem>("Virusprayer").Type;
                    string replacementPath = "InfernalEclipseAPI/Assets/Textures/Items/Virusprayer";

                    TextureAssets.Item[type] = ModContent.Request<Texture2D>(replacementPath, AssetRequestMode.ImmediateLoad);
                }
            }
        }
    }
}
