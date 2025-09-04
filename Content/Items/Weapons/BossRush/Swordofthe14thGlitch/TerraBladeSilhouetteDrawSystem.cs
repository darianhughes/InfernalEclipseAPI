using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.ID;
using Luminance.Core.Graphics;

namespace InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch
{
    public class SilhouetteTargetContent : ARenderTargetContentByRequest
    {
        /// <summary>
        /// The host of this render target to draw.
        /// </summary>
        public Entity Host
        {
            get;
            internal set;
        }

        protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            // Initialize the underlying render target if necessary.
            Vector2 size = new(Main.screenWidth, Main.screenHeight);
            PrepareARenderTarget_WithoutListeningToEvents(ref _target, Main.instance.GraphicsDevice, (int)size.X, (int)size.Y, RenderTargetUsage.PreserveContents);

            device.SetRenderTarget(_target);
            device.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            DrawPlayer();
            DrawTerraBlade();
            Main.spriteBatch.End();

            device.SetRenderTarget(null);

            // Mark preparations as completed.
            _wasPrepared = true;
        }

        private static void DrawPlayer()
        {
            int owner = Main.myPlayer;
            Player other = Main.player[owner];
            Player player = Main.playerVisualClone[owner] ??= new();

            player.CopyVisuals(other);
            player.isFirstFractalAfterImage = true;
            player.firstFractalAfterImageOpacity = 1f;
            player.ResetVisibleAccessories();
            player.UpdateDyes();
            player.DisplayDollUpdate();
            player.UpdateSocialShadow();
            player.itemRotation = 0f;
            player.heldProj = other.heldProj;
            player.Center = other.Center;
            player.wingFrame = other.wingFrame;
            player.velocity.Y = other.velocity.Y;
            player.PlayerFrame();
            player.socialIgnoreLight = true;
            Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin, 0f);
        }

        private void DrawTerraBlade()
        {
            if (!Host.active)
                return;

            if (Host is NPC n)
                Main.instance.DrawNPC(n.whoAmI, false);
            if (Host is Projectile p)
                Main.instance.DrawProj(p.whoAmI);
        }
    }

    public class TerraBladeSilhouetteDrawSystem : ModSystem
    {
        /// <summary>
        /// The silhouette target responsible.
        /// </summary>
        public static SilhouetteTargetContent SilhouetteDrawContents
        {
            get;
            private set;
        }

        /// <summary>
        /// The opacity of the silhouette effect.
        /// </summary>
        public static float SilhouetteOpacity
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the silhouette effect should be inverted in terms of contrast choices or not.
        /// </summary>
        public static bool Inverted
        {
            get;
            set;
        }

        /// <summary>
        /// The entity that should be included in the silhouette.
        /// </summary>
        public static Entity Subject
        {
            get;
            set;
        }

        public override void OnModLoad()
        {
            SilhouetteDrawContents = new();
            Main.ContentThatNeedsRenderTargets.Add(SilhouetteDrawContents);
            Main.OnPostDraw += DrawSilhouette;
        }

        private void DrawSilhouette(GameTime obj)
        {
            if (Main.gameMenu)
            {
                SilhouetteOpacity = 0f;
                Subject = null;
            }

            // Don't waste resources if the silhouette is not in use.
            if (SilhouetteOpacity <= 0f || Subject is null || !Subject.active)
                return;

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.Identity);

            // Initialize the silhouette drawer, with the terra blade as its current host.
            SilhouetteDrawContents.Host = Subject;
            SilhouetteDrawContents.Request();

            // If the drawer isn't ready, wait until it is.
            if (!SilhouetteDrawContents.IsReady)
            {
                Main.spriteBatch.End();
                return;
            }

            // Draw the black background.
            Main.spriteBatch.Draw(LoadDeferred("InfernalEclipseAPI/Assets/Textures/Pixel"), Vector2.Zero, null, (Inverted ? Color.White : Color.Black) * SilhouetteOpacity, 0f, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight), 0, 0f);

            // Draw the silhouette as pure white.
            ManagedShader silhouetteShader = ShaderManager.GetShader("SilhouetteShader");
            silhouetteShader.TrySetParameter("inverted", Inverted);
            silhouetteShader.Apply();
            Main.spriteBatch.Draw(SilhouetteDrawContents.GetTarget(), Vector2.Zero, null, Color.White * SilhouetteOpacity, 0f, Vector2.Zero, 1f, 0, 0f);

            // Draw the eye gleam over everything, resetting the silhouette shader.
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.End();
        }

        private static Texture2D LoadDeferred(string path)
        {
            // Don't attempt to load anything server-side.
            if (Main.netMode == NetmodeID.Server)
                return default;

            return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
        }
    }
}
