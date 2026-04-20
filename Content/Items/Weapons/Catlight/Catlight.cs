using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InfernalEclipseAPI.Content.Items.Weapons.Catlight
{
    public class Catlight : ModItem
    {
        internal const string Path = "InfernalEclipseAPI/Content/Items/Weapons/Catlight/";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.channel = true;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = CatlightDamage.Instance;
            Item.damage = 1;
            Item.damage = 740;
            Item.crit = 10;
            Item.shoot = ModContent.ProjectileType<CatlightDeathray>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            static int GetProgression()
            {
                if (NPC.downedMoonlord) return 10;
                if (NPC.downedAncientCultist) return 9;
                if (NPC.downedGolemBoss) return 8;
                if (NPC.downedPlantBoss) return 7;
                if (NPC.downedMechBoss3) return 6;
                if (NPC.downedMechBoss2) return 5;
                if (NPC.downedMechBoss1) return 4;
                if (Main.hardMode) return 3;
                if (NPC.downedBoss3) return 2; // Skele
                if (NPC.downedBoss2) return 1; // Evil
                return 0;
            }

            switch (GetProgression())
            {
                case 0:
                    damage.Flat = 15;
                    Item.damage = 15;
                    break;
                case 1:
                    damage.Flat = 20;
                    Item.damage = 20;
                    break;
                case 2:
                    damage.Flat = 30;
                    Item.damage = 30;
                    break;
                case 3:
                    damage.Flat = 45;
                    Item.damage = 45;
                    break;
                case 4:
                    damage.Flat = 50;
                    Item.damage = 50;
                    break;
                case 5:
                    damage.Flat = 55;
                    Item.damage = 55;
                    break;
                case 6:
                    damage.Flat = 60;
                    Item.damage = 60;
                    break;
                case 7:
                    damage.Flat = 75;
                    Item.damage = 75;
                    break;
                case 8:
                    damage.Flat = 90;
                    Item.damage = 90;
                    break;
                case 9:
                    damage.Flat = 120;
                    Item.damage = 120;
                    break;
                case 10:
                    damage.Flat = 200;
                    Item.damage = 200;
                    break;
                case 11:
                    damage.Flat = 7400;
                    Item.damage = 7400;
                    break;
                case 12:
                    damage.Flat = 74000;
                    Item.damage = 74000;
                    break;

            }

            damage.Flat *= 0.75f;
            Item.damage = (int)(Item.damage * 0.75f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int typeProj = ModContent.ProjectileType<CatlightDeathrayJRR>();

            if (NPC.downedMoonlord)
            {
                typeProj = ModContent.ProjectileType<CatlightDeathray>();
            }
            else if (Main.hardMode && !NPC.downedMoonlord)
            {
                typeProj = ModContent.ProjectileType<CatlightDeathrayJR>();
            }

            Projectile.NewProjectile(source, position, velocity, typeProj, damage, knockback, player.whoAmI);

            SoundEngine.PlaySound(new(Path + "CatlightExplosion"), player.Center);

            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(ItemID.Catfish);
            recipe.AddIngredient(ItemID.FallenStar, 3996);
            recipe.Register();
        }
    }
    public class CatlightDamage : DamageClass
    {
        internal static CatlightDamage? Instance;
        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) => StatInheritanceData.None;
        public override bool GetEffectInheritance(DamageClass damageClass) => true;
    }
    public struct VertexInfo : IVertexType
    {
        private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0), new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0), new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0));

        public Vector2 Position;

        public Color Color;

        public Vector3 TexCoord;

        public VertexDeclaration VertexDeclaration => VertexInfo._vertexDeclaration;

        public VertexInfo(Vector2 position, Vector3 texCoord, Color color)
        {
            Position = position;
            TexCoord = texCoord;
            Color = color;
        }
    }
    public class CatlightDeathray : ModProjectile
    {
        private Vector2[] lasersTop = new Vector2[140];

        private Vector2[] lasersBot = new Vector2[140];

        private BlendState blendStatef = new BlendState
        {
            AlphaBlendFunction = BlendState.AlphaBlend.AlphaBlendFunction,
            AlphaDestinationBlend = BlendState.AlphaBlend.AlphaDestinationBlend,
            AlphaSourceBlend = BlendState.AlphaBlend.AlphaSourceBlend,
            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorSourceBlend = BlendState.Additive.ColorSourceBlend,
            ColorWriteChannels = ColorWriteChannels.All,
            ColorWriteChannels1 = ColorWriteChannels.All,
            ColorWriteChannels2 = ColorWriteChannels.All,
            ColorWriteChannels3 = ColorWriteChannels.All,
            BlendFactor = Color.White,
            MultiSampleMask = -1
        };
        //placeholder bc ray use another texture
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Backgrounds/BlankPixel";

        public override void SetStaticDefaults()
        {
            //long ray
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.timeLeft = 550;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            if (Projectile.localAI[0] <= 4.8f || Projectile.timeLeft < 15)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2400f, 200f, ref point);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            //target.SimpleStrikeNPC((int)Projectile.owner.ToPlayer().GetDamage<GenericDamageClass>().ApplyTo(745), 0);
            //Projectile.owner.ToPlayer().addDPS((int)Projectile.owner.ToPlayer().GetDamage<GenericDamageClass>().ApplyTo(745));
        }
        public override void AI()
        {
            //Main.LocalPlayer.CSE().Screenshake = 1;
            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 6f;
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 140; i++)
            {
                float x = (float)i * 15f;
                float y = 29f * (0.08f * Projectile.localAI[0]) * (float)Math.Pow(0.1f * x, 0.45);
                lasersTop[i] = new Vector2(x, y);
                lasersBot[i] = new Vector2(x, 0f - y);
            }
            float x2 = 4500f;
            float y2 = 19f * (0.08f * Projectile.localAI[0]) * (float)Math.Pow(210.0, 0.45);
            if (Projectile.localAI[0] <= 8f && player.channel)
            {
                Projectile.localAI[0] += 0.5f;
            }
            if (!player.channel)
            {
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                player.itemTime = (player.itemAnimation = 2);
                Projectile.localAI[0] -= 1f;
                if (Projectile.localAI[0] < 0f)
                {
                    //fade sound?
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[2] % 5f == 0f && !player.CheckMana(player.HeldItem, 8, pay: true))
            {
                Projectile.localAI[2] = 1f;
            }
            if (Projectile.localAI[2] == 1f)
            {
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                player.itemTime = (player.itemAnimation = 2);
                Projectile.localAI[0] -= 1f;
                if (Projectile.localAI[0] < 0f)
                {
                    //fade sound?
                    Projectile.Kill();
                }
            }
            if (player.channel)
            {
                player.itemTime = (player.itemAnimation = 2);
                Projectile.timeLeft = 55;
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                Projectile.ai[2] += 1f;
                if (Projectile.ai[2] % 50f == 1f)
                {
                    //SoundEngine.PlaySound(new("ssm/Assets/Sounds/CatlightRay"), Projectile.Center);
                }
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), 0.06f);
                Projectile.velocity = Projectile.rotation.ToRotationVector2();
                if (player.direction == 1)
                {
                    player.itemRotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    player.itemRotation = Projectile.velocity.ToRotation() + 3.1415925f;
                }
                player.heldProj = Projectile.whoAmI;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            List<VertexInfo> vertices = new List<VertexInfo>();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blendStatef, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //    Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("ssm/Assets/ExtraTextures/FadedGlowStreak").Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(Catlight.Path + "FadedGlowStreak").Value;
            vertices.Clear();
            float MoveFactor = Main.GlobalTimeWrappedHourly / 0.7f;
            for (int i = 0; i < 140; i++)
            {
                if (lasersTop[i] + Main.LocalPlayer.Center != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo(lasersTop[i].RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(0f, 1f - (float)i / 70f + MoveFactor, 1f - (float)i / 70f), Color.DarkRed));
                    vertices.Add(new VertexInfo(lasersBot[i].RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f, 1f - (float)i / 70f + MoveFactor, 1f - (float)i / 70f), Color.White));
                }
            }
            if (vertices.Count >= 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            // Not 2.0    Main.graphics.GraphicsDevice.Textures[0] = FargosTextureRegistry.DevInnerStreak.Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(Catlight.Path + "DevInnerStreak").Value;
            for (int j = 0; j < 3; j += 2)
            {
                _ = j * (float)Math.PI / 1f;
                vertices.Clear();
                for (int i = 0; i < 140; i += 2)
                {
                    float fac = 0.13f * i;
                    float sinfactor = 0.545f * (float)Math.Sin((1.1f - 0.03f * fac) * fac - Main.GlobalTimeWrappedHourly * 9.15f) * (float)(j - 1);
                    Vector2 top = new Vector2(lasersTop[i].X, lasersTop[i].Y * sinfactor + 84f);
                    Vector2 bot = new Vector2(lasersTop[i].X, lasersTop[i].Y * sinfactor - 84f);
                    vertices.Add(new VertexInfo(top.RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f - (float)i / 70f + MoveFactor, 0f, 1f - (float)i / 70f), new(Main.DiscoR, 0, 0)));
                    vertices.Add(new VertexInfo(bot.RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f - (float)i / 70f + MoveFactor, 1f, 1f - (float)i / 70f), new(255 - Main.DiscoR, 0, 0)));
                }
                if (vertices.Count >= 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }

    //pre ml
    public class CatlightDeathrayJR : ModProjectile
    {
        private Vector2[] lasersTop = new Vector2[140];

        private Vector2[] lasersBot = new Vector2[140];

        //placeholder bc ray use another texture
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Backgrounds/BlankPixel";

        public override void SetStaticDefaults()
        {
            //long ray
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = (Projectile.height = 10);
            Projectile.timeLeft = 550;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            if (Projectile.localAI[0] <= 4.8f || Projectile.timeLeft < 15)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2400f, 200f, ref point);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
            //target.SimpleStrikeNPC((int)Projectile.owner.ToPlayer().GetDamage<GenericDamageClass>().ApplyTo(745), 0);
            //Projectile.owner.ToPlayer().addDPS((int)Projectile.owner.ToPlayer().GetDamage<GenericDamageClass>().ApplyTo(745));
        }
        public override void AI()
        {
            //Main.LocalPlayer.CSE().Screenshake = 1;
            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 6f;
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 140; i++)
            {
                float x = (float)i * 15f;
                float y = 29f * (0.08f * Projectile.localAI[0]) * (float)Math.Pow(0.1f * x, 0.45);
                lasersTop[i] = new Vector2(x, y);
                lasersBot[i] = new Vector2(x, 0f - y);
            }
            float x2 = 4500f;
            float y2 = 19f * (0.08f * Projectile.localAI[0]) * (float)Math.Pow(210.0, 0.45);
            if (Projectile.localAI[0] <= 8f && player.channel)
            {
                Projectile.localAI[0] += 0.5f;
            }
            if (!player.channel)
            {
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                player.itemTime = (player.itemAnimation = 2);
                Projectile.localAI[0] -= 1f;
                if (Projectile.localAI[0] < 0f)
                {
                    //fade sound?
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[2] % 5f == 0f && !player.CheckMana(player.HeldItem, 8, pay: true))
            {
                Projectile.localAI[2] = 1f;
            }
            if (Projectile.localAI[2] == 1f)
            {
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                player.itemTime = (player.itemAnimation = 2);
                Projectile.localAI[0] -= 1f;
                if (Projectile.localAI[0] < 0f)
                {
                    //fade sound?
                    Projectile.Kill();
                }
            }
            if (player.channel)
            {
                player.itemTime = (player.itemAnimation = 2);
                Projectile.timeLeft = 55;
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                Projectile.ai[2] += 1f;
                if (Projectile.ai[2] % 50f == 1f)
                {
                    //SoundEngine.PlaySound(new("ssm/Assets/Sounds/CatlightRay"), Projectile.Center);
                }
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), 0.06f);
                Projectile.velocity = Projectile.rotation.ToRotationVector2();
                if (player.direction == 1)
                {
                    player.itemRotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    player.itemRotation = Projectile.velocity.ToRotation() + 3.1415925f;
                }
                player.heldProj = Projectile.whoAmI;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            List<VertexInfo> vertices = new List<VertexInfo>();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        //    Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("ssm/Assets/ExtraTextures/FadedGlowStreak").Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(Catlight.Path + "FadedGlowStreak").Value;
            vertices.Clear();
            float MoveFactor = Main.GlobalTimeWrappedHourly / 0.7f;
            for (int i = 0; i < 140; i++)
            {
                if (lasersTop[i] + Main.LocalPlayer.Center != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo(lasersTop[i].RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(0f, 1f - (float)i / 70f + MoveFactor, 1f - (float)i / 70f), Color.Red));
                    vertices.Add(new VertexInfo(lasersBot[i].RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f, 1f - (float)i / 70f + MoveFactor, 1f - (float)i / 70f), Color.DarkRed));
                }
            }
            if (vertices.Count >= 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            // Not 2.0    Main.graphics.GraphicsDevice.Textures[0] = FargosTextureRegistry.DevInnerStreak.Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(Catlight.Path + "DevInnerStreak").Value;
            for (int j = 0; j < 3; j += 2)
            {
                _ = j * (float)Math.PI / 1f;
                vertices.Clear();
                for (int i = 0; i < 140; i += 2)
                {
                    float fac = 0.13f * i;
                    float sinfactor = 0.545f * (float)Math.Sin((1.1f - 0.03f * fac) * fac - Main.GlobalTimeWrappedHourly * 9.15f) * (float)(j - 1);
                    Vector2 top = new Vector2(lasersTop[i].X, lasersTop[i].Y * sinfactor + 84f);
                    Vector2 bot = new Vector2(lasersTop[i].X, lasersTop[i].Y * sinfactor - 84f);
                    vertices.Add(new VertexInfo(top.RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f - (float)i / 70f + MoveFactor, 0f, 1f - (float)i / 70f), Color.MediumVioletRed /*new(Main.DiscoR, 0, 0)*/));
                    vertices.Add(new VertexInfo(bot.RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f - (float)i / 70f + MoveFactor, 1f, 1f - (float)i / 70f), Color.OrangeRed /*new(255 - Main.DiscoR, 0, 0)*/));
                }
                if (vertices.Count >= 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }


    //pre hm
    public class CatlightDeathrayJRR : ModProjectile
    {
        private Vector2[] lasersTop = new Vector2[140];

        private Vector2[] lasersBot = new Vector2[140];

        //placeholder bc ray use another texture
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Backgrounds/BlankPixel";

        public override void SetStaticDefaults()
        {
            //long ray
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = (Projectile.height = 10);
            Projectile.timeLeft = 550;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            if (Projectile.localAI[0] <= 4.8f || Projectile.timeLeft < 15)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2400f, 200f, ref point);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 10;
            //target.SimpleStrikeNPC((int)Projectile.owner.ToPlayer().GetDamage<GenericDamageClass>().ApplyTo(745), 0);
            //Projectile.owner.ToPlayer().addDPS((int)Projectile.owner.ToPlayer().GetDamage<GenericDamageClass>().ApplyTo(745));
        }
        public override void AI()
        {
            //Main.LocalPlayer.CSE().Screenshake = 1;
            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 6f;
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 140; i++)
            {
                float x = (float)i * 15f;
                float y = 29f * (0.08f * Projectile.localAI[0]) * (float)Math.Pow(0.1f * x, 0.45);
                lasersTop[i] = new Vector2(x, y);
                lasersBot[i] = new Vector2(x, 0f - y);
            }
            float x2 = 4500f;
            float y2 = 19f * (0.08f * Projectile.localAI[0]) * (float)Math.Pow(210.0, 0.45);
            if (Projectile.localAI[0] <= 8f && player.channel)
            {
                Projectile.localAI[0] += 0.5f;
            }
            if (!player.channel)
            {
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                player.itemTime = (player.itemAnimation = 2);
                Projectile.localAI[0] -= 1f;
                if (Projectile.localAI[0] < 0f)
                {
                    //fade sound?
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[2] % 5f == 0f && !player.CheckMana(player.HeldItem, 8, pay: true))
            {
                Projectile.localAI[2] = 1f;
            }
            if (Projectile.localAI[2] == 1f)
            {
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                player.itemTime = (player.itemAnimation = 2);
                Projectile.localAI[0] -= 1f;
                if (Projectile.localAI[0] < 0f)
                {
                    //fade sound?
                    Projectile.Kill();
                }
            }
            if (player.channel)
            {
                player.itemTime = (player.itemAnimation = 2);
                Projectile.timeLeft = 55;
                Projectile.Center = player.Center + Projectile.rotation.ToRotationVector2() * 40f;
                Projectile.ai[2] += 1f;
                if (Projectile.ai[2] % 50f == 1f)
                {
                    //SoundEngine.PlaySound(new("ssm/Assets/Sounds/CatlightRay"), Projectile.Center);
                }
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), 0.06f);
                Projectile.velocity = Projectile.rotation.ToRotationVector2();
                if (player.direction == 1)
                {
                    player.itemRotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    player.itemRotation = Projectile.velocity.ToRotation() + 3.1415925f;
                }
                player.heldProj = Projectile.whoAmI;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            List<VertexInfo> vertices = new List<VertexInfo>();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        //    Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("ssm/Assets/ExtraTextures/FadedGlowStreak").Value;
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(Catlight.Path + "FadedGlowStreak").Value;
            vertices.Clear();
            float MoveFactor = Main.GlobalTimeWrappedHourly / 0.7f;
            for (int i = 0; i < 140; i++)
            {
                if (lasersTop[i] + Main.LocalPlayer.Center != Vector2.Zero)
                {
                    vertices.Add(new VertexInfo(lasersTop[i].RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(0f, 1f - (float)i / 70f + MoveFactor, 1f - (float)i / 70f), Color.Red));
                    vertices.Add(new VertexInfo(lasersBot[i].RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f, 1f - (float)i / 70f + MoveFactor, 1f - (float)i / 70f), Color.DarkRed));
                }
            }
            if (vertices.Count >= 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //Main.graphics.GraphicsDevice.Textures[0] = FargosTextureRegistry.DevInnerStreak.Value;
            //for (int j = 0; j < 3; j += 2)
            //{
            //    _ = j * (float)Math.PI / 1f;
            //    vertices.Clear();
            //    for (int i = 0; i < 140; i += 2)
            //    {
            //        float fac = 0.13f * i;
            //        float sinfactor = 0.545f * (float)Math.Sin((1.1f - 0.03f * fac) * fac - Main.GlobalTimeWrappedHourly * 9.15f) * (float)(j - 1);
            //        Vector2 top = new Vector2(lasersTop[i].X, lasersTop[i].Y * sinfactor + 84f);
            //        Vector2 bot = new Vector2(lasersTop[i].X, lasersTop[i].Y * sinfactor - 84f);
            //        vertices.Add(new VertexInfo(top.RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f - (float)i / 70f + MoveFactor, 0f, 1f - (float)i / 70f), new(Main.DiscoR, 0, 0)));
            //        vertices.Add(new VertexInfo(bot.RotatedBy(Projectile.rotation) + Projectile.Center - Main.screenPosition, new Vector3(1f - (float)i / 70f + MoveFactor, 1f, 1f - (float)i / 70f), new(255 - Main.DiscoR, 0, 0)));
            //    }
            //    if (vertices.Count >= 3)
            //    {
            //        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            //    }
            //}
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }

}
