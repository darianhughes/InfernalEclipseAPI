using Terraria.Audio;
using Terraria.DataStructures;
using ThoriumMod;
using ThoriumMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ThoriumMod.Empowerments;
using InfernalEclipseAPI.Content.Items.Weapons.Nameless.Genesis.RandomSong;
using NoxusBoss.Content.Rarities;
using NoxusBoss.Assets;

namespace InfernalEclipseAPI.Content.Items.Weapons.Nameless.Genesis
{
    [ExtendsFromMod("NoxusBoss", "ThoriumMod")]
    [JITWhenModsEnabled("NoxusBoss", "ThoriumMod")]
    public class Genesis : BardItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        // Original sprite native size
        private const int TexW = 1308;
        private const int TexH = 1582;

        // Targets for each context
        private const float HAND_TARGET_W = 64f; // when held/used
        private const float HAND_TARGET_H = 30f;

        private const float INV_TARGET_W = 40f; // in the 52×52 slot
        private const float INV_TARGET_H = 40f;

        private const float WORLD_TARGET_W = 48f; // dropped in the world
        private const float WORLD_TARGET_H = 32f;

        private static float Fit(float w, float h, float maxW, float maxH)
            => System.Math.Min(maxW / w, maxH / h);
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<Damage>(4);
            Empowerments.AddInfo<CriticalStrikeChance>(4);
            Empowerments.AddInfo<LifeRegeneration>(4);
            Empowerments.AddInfo<MovementSpeed>(4);
            Empowerments.AddInfo<Defense>(4);
            Empowerments.AddInfo<ResourceRegen>(4);
            //DisplayName.SetDefault("Moment of Creation");
            //Tooltip.SetDefault("Channels a cosmic melody that culminates in a falling galaxy barrage");
        }

        public override void SetBardDefaults()
        {
            Item.damage = 10000; // tune to taste
            Item.knockBack = 3f;
            Item.DamageType = ThoriumDamageBase<BardDamage>.Instance;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.channel = false;

            Item.shoot = ModContent.ProjectileType<GalaxyRainController>();
            Item.shootSpeed = 0f;
            //Item.UseSound = SoundID.Item84; // intro “whoosh”
            Item.rare = ModContent.RarityType<NamelessDeityRarity>();
            Item.value = Item.buyPrice(gold: 10);

            // 1) Fix SIZE WHEN HELD: set Item.scale to fit ~64×30 in-hand
            float handScale = Fit(TexW, TexH, HAND_TARGET_W, HAND_TARGET_H); // ~0.01896
            Item.scale = handScale;

            // Hitbox roughly matches the drawn size while held
            Item.width = (int)(TexW * handScale); // ~25
            Item.height = (int)(TexH * handScale); // ~30

            InspirationCost = 8;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (Main.myPlayer == player.whoAmI)
                player.GetModPlayer<RandomSongPlayer>().EnsureRandomSongPlaying();
        }

        public override bool CanPlayInstrument(Player player)
        {
            // Only one controller at a time per player.
            if (player.ownedProjectileCounts[ModContent.ProjectileType<GalaxyRainController>()] == 0)
            {
                SoundEngine.PlaySound(GennedAssets.Sounds.NamelessDeity.MomentOfCreation with { Volume = 2f });
                return true;
            }
            return false;
        }

        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Spawn/refresh controller once; it manages everything while held
            Projectile.NewProjectile(source, player.Center, Vector2.Zero,
                ModContent.ProjectileType<GalaxyRainController>(), damage, knockback, player.whoAmI);
            SoundEngine.PlaySound(SoundID.Item84 with { Volume = 0.6f }, player.Center);
            return false;
        }

        // 2) INVENTORY DRAW: ignore Item.scale; draw to ~40×40 so it’s visible.
        public override bool PreDrawInInventory(
            SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor,
            Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = TextureAssets.Item[Item.type].Value;
            Rectangle src = tex.Frame();                 // always use full texture
            Vector2 sz = new Vector2(src.Width, src.Height);
            Vector2 org = sz * 0.5f;                  // draw from center of the slot

            float invScale = Fit(sz.X, sz.Y, INV_TARGET_W, INV_TARGET_H); // ~fills slot
            float final = invScale * Main.inventoryScale;                 // respect UI zoom

            sb.Draw(tex, position, src, drawColor, 0f, org, final, SpriteEffects.None, 0f);
            return false; // we handled the draw
        }

        // 3) WORLD DRAW: ignore Item.scale; draw ~32 px tall so it’s readable on ground.
        public override bool PreDrawInWorld(
            SpriteBatch sb, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = TextureAssets.Item[Item.type].Value;
            Rectangle src = tex.Frame();
            Vector2 sz = new Vector2(src.Width, src.Height);
            Vector2 org = sz * 0.5f;
            Vector2 pos = Item.Center - Main.screenPosition;

            float worldScale = Fit(sz.X, sz.Y, WORLD_TARGET_W, WORLD_TARGET_H);

            sb.Draw(tex, pos, src, alphaColor, rotation, org, worldScale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
