using CalamityMod.Buffs.DamageOverTime;
using Luminance.Assets;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Content.Items.Weapons.Legendary.CelestialIllumination
{
    public class CelestialIlluminationPlayer : ModPlayer
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        static int Beam => ModContent.ProjectileType<CelestialIlluminationBeam>();
        static int Star => ModContent.ProjectileType<CelestialIlluminationStar>();
        static int CI => ModContent.ItemType<CelestialIllumination>();
        public int StarCount = 0, StarTimer;
        public const int MaxStars = 13;
        static int Tier => CelestialIllumination.Tier();
        public override void ResetEffects()
        {
            if (Player.HeldItem.type != CI)
            {
                StarCount = 0;
                StarTimer = 0;
            }

            if (Player.HeldItem.type == CI)
            {
                if (StarCount > 0)
                    if (StarTimer++ > 360)
                        StarTimer = 0;
            }
        }
        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.HeldItem.type != CI)
                return;

            if (projectile.type == Beam || projectile.type == Star)
            {
                if (Tier >= CelestialIllumination.Providence)
                {
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 150);
                }
            }

            if (projectile.type == Star && StarCount < MaxStars && projectile.ai[2] == 0)
            {
                StarCount++;
                projectile.ai[2] = 1;
            }
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Player.HeldItem.type != CI)
                return;

            Vector2 center = Player.Center - Main.screenPosition;

            for (int i = 0; i < StarCount; i++)
            {
                Vector2 offset = center + new Vector2(0, 40).RotatedBy(MathHelper.ToRadians(360f / StarCount * i + StarTimer));

                float starScale = 0.2f * 0.2f;

                Texture2D bloomSmall = MiscTexturesRegistry.BloomCircleSmall.Value;

                Texture2D shineFlare = MiscTexturesRegistry.ShineFlareTexture.Value;

                Color color = Color.White with { A = 0 };

                Main.spriteBatch.Draw(
                        shineFlare,
                        offset,
                        null,
                        color,
                        0f,
                        shineFlare.Size() * 0.5f,
                        starScale,
                        SpriteEffects.None,
                        0f
                    );

                Main.spriteBatch.Draw(
                        bloomSmall,
                        offset,
                        null,
                        color,
                        0f,
                        bloomSmall.Size() * 0.5f,
                        starScale,
                        SpriteEffects.None,
                        0f
                    );
            }
        }
    }
    public class CelestialIlluminationStarReset : GlobalNPC
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.boss;
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            foreach (Player player in Main.ActivePlayers)
            {
                player.GetModPlayer<CelestialIlluminationPlayer>().StarCount = 0;
            }
        }
    }
}