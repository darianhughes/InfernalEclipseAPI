using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using InfernalEclipseAPI.YharimEX.Core.Systems;

namespace InfernalEclipseAPI.YharimEX.Common.Graphics.Particles
{
    public abstract class BaseExpandingParticle : Particle
    {
        public readonly Vector2 StartScale;

        public readonly Vector2 EndScale;

        public Color BloomColor;

        public readonly bool UseBloom;

        public override string AtlasTextureName => ""; //"YharimEX.Bloom"

        public virtual Vector2 DrawScale => Scale * 0.3f;

        public BaseExpandingParticle(Vector2 position, Vector2 velocity, Color drawColor, Vector2 startScale, Vector2 endScale, int lifetime, bool useExtraBloom = false, Color? extraBloomColor = null)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = drawColor;
            Scale = StartScale = startScale;
            EndScale = endScale;
            Lifetime = lifetime;
            UseBloom = useExtraBloom;
            Color valueOrDefault = extraBloomColor.GetValueOrDefault();
            if (!extraBloomColor.HasValue)
            {
                valueOrDefault = Color.White;
                extraBloomColor = valueOrDefault;
            }

            BloomColor = extraBloomColor.Value;
        }

        public sealed override void Update()
        {
            Opacity = MathHelper.Lerp(1f, 0f, YharimEXUtils.SineInOut(LifetimeRatio));
            Scale = Vector2.Lerp(StartScale, EndScale, YharimEXUtils.SineInOut(LifetimeRatio));
        }

        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            var tex2D = ModContent.Request<Texture2D>("InfernalEclipseAPI/YharimEX/Assets/ExtraTextures/AdditiveTextures/Bloom").Value;
            var origin = tex2D.Size() * 0.5f;
            var pos = Position - Main.screenPosition;

            var color = DrawColor; color.A = 0;
            spriteBatch.Draw(tex2D, pos, null, color * Opacity, Rotation, origin, DrawScale, SpriteEffects.None, 0);

            if (UseBloom)
            {
                var bloom = BloomColor; bloom.A = 0;
                spriteBatch.Draw(tex2D, pos, null, bloom * 0.4f * Opacity, Rotation, origin, DrawScale * 0.66f, SpriteEffects.None, 0);
            }
        }
    }
}
