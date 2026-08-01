using System.Collections.Generic;
using System.IO;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.NPCs;
using InfernalEclipseAPI.Core.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Weapons.Donor.BlixerCore
{
    public class BlixerCore : ModItem
    {
        private bool laserMode = false;

        public override void SetDefaults()
        {
            Item.width = Item.height = 28;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;

            Item.useTime = Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.autoReuse = true;

            Item.damage = 100;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 2f;
            Item.mana = 12;

            Item.shoot = ModContent.ProjectileType<BlixerHand>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 || player.altFunctionUse == 2;

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool[] tentaclesPresent = new bool[4];
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.type == type && projectile.owner == Main.myPlayer && projectile.ai[1] >= 0f && projectile.ai[1] < 4f)
                    tentaclesPresent[(int)projectile.ai[1]] = true;
            }

            for (int i = 0; i < 4; i++)
            {
                if (!tentaclesPresent[i])
                {
                    Vector2 vel = new Vector2(Main.rand.Next(-13, 14), Main.rand.Next(-13, 14)) * 0.25f;
                    int index = Projectile.NewProjectile(source, player.Center, vel, type, damage, knockback, player.whoAmI, Main.rand.Next(120), i);
                    if (Main.projectile.IndexInRange(index))
                        Main.projectile[index].originalDamage = Item.damage;
                }
            }

            if (player.altFunctionUse == 2)
            {
                player.GetModPlayer<InfernalPlayer>().blixerLaserMode = !player.GetModPlayer<InfernalPlayer>().blixerLaserMode;
            }

            laserMode = player.GetModPlayer<InfernalPlayer>().blixerLaserMode;

            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new(Mod, "BlixerModer", laserMode ? Language.GetTextValue("Mods.InfernalEclipseAPI.Items.BlixerCore.LaserMode") : Language.GetTextValue("Mods.InfernalEclipseAPI.Items.BlixerCore.DefaultMode")) { OverrideColor = laserMode ? Color.DeepPink : Color.CornflowerBlue });

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line6 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.Minesky"))}");
                line6.OverrideColor = new(196, 35, 44);
                tooltips.Add(line6);

                TooltipLine line5 = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Donor"));
                line5.OverrideColor = new Color(196, 35, 44);
                tooltips.Add(line5);
            }
            else
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Donor"));
                line5.OverrideColor = new Color(196, 35, 44);
                tooltips.Add(line5);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
        }
    }

    [PierceResistException]
    public class BlixerHand : ModProjectile
    {
        public bool initSegments = false;
        public Vector2[] segment = new Vector2[6];

        private int shootingDelay;

        private Vector2 laserDirection = Vector2.UnitX;
        private bool laserActive;

        private const float LaserLength = 2000f;
        private const float LaserThickness = 20f;
        private int laserTimer = -40;
        private float laserVisualStrength;

        private const int LaserChargeTime = 40;
        private const int LaserFireTime = 200;

        private Player Owner => Main.player[Projectile.owner];
        private SlotId laserSoundSlot;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.netImportant = true;

            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 8;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.rotation);
            writer.Write(Projectile.spriteDirection);

            writer.Write(laserDirection.X);
            writer.Write(laserDirection.Y);
            writer.Write(laserActive);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.rotation = reader.ReadSingle();
            Projectile.spriteDirection = reader.ReadInt32();

            laserDirection.X = reader.ReadSingle();
            laserDirection.Y = reader.ReadSingle();
            laserActive = reader.ReadBoolean();
        }

        public override bool? CanDamage()
        {
            if (Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode)
                return laserActive ? null : false;

            if (Projectile.ai[1] % 3 == 0)
                return false;

            return null;
        }

        public override bool PreAI()
        {
            if (!initSegments)
            {
                Owner.AddBuff(ModContent.BuffType<BlixerBuff>(), 2);
                laserTimer = (int)(-LaserChargeTime - (Projectile.ai[1] * 10));
                initSegments = true;
                for (int i = 0; i < 6; i++)
                {
                    segment[i] = Projectile.Center;
                }
            }
            return true;
        }

        public override void AI()
        {
            if (Owner.active && Owner.GetModPlayer<InfernalPlayer>().blixerCoreSummon)
                Projectile.timeLeft = 8;

            Vector2 playerVel = Owner.position - Owner.oldPosition;
            Projectile.position += playerVel;
            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 0f)
            {
                Vector2 home = Owner.Center + new Vector2(50, 0).RotatedBy(ToRadians(60) * Projectile.ai[1]);
                Vector2 distance = home - Projectile.Center;
                float range = distance.Length();
                distance.Normalize();
                if (Projectile.ai[0] == 0f)
                {
                    if (range > 13f)
                    {
                        Projectile.ai[0] = -1f; // If in fast mode, stay fast until back in range
                        if (range > 1350f)
                        {
                            Projectile.Kill();
                            return;
                        }
                    }
                    else
                    {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 3f + Main.rand.NextFloat(3f);
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    distance /= 8f;
                }

                if (range > 150f) //switch to fast return mode
                {
                    Projectile.ai[0] = -1f;
                    Projectile.netUpdate = true;
                }
                Projectile.velocity += distance;
                if (range > 60f)
                    Projectile.velocity *= 0.96f;

                if (Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode)
                {
                    bool holdingAttack = Owner.controlUseItem;

                    // Only the owning client knows its mouse position.
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Vector2 newLaserDirection =
                            Projectile.DirectionTo(Main.MouseWorld);

                        if (newLaserDirection == Vector2.Zero)
                            newLaserDirection = Vector2.UnitX;

                        laserDirection = newLaserDirection;
                        Projectile.rotation = laserDirection.ToRotation();

                        if (laserDirection.X >= 0f)
                        {
                            Projectile.spriteDirection = -1;
                        }
                        else
                        {
                            Projectile.rotation += MathHelper.Pi;
                            Projectile.spriteDirection = 1;
                        }

                        Projectile.netUpdate = true;
                    }

                    if (!holdingAttack)
                    {
                        // Releasing the button cancels the current cycle.
                        laserTimer = (int)(-LaserChargeTime - (Projectile.ai[1] * 10));
                        laserActive = false;
                        laserVisualStrength = MathHelper.Lerp(
                            laserVisualStrength,
                            0f,
                            0.2f
                        );

                        StopLaserSound();
                    }
                    else
                    {
                        laserTimer++;

                        if (laserTimer < 0)
                        {
                            // Charging phase.
                            laserActive = false;

                            float chargeProgress = Utils.GetLerpValue(
                                -LaserChargeTime,
                                0f,
                                laserTimer,
                                true
                            );

                            laserVisualStrength = MathHelper.Lerp(
                                laserVisualStrength,
                                chargeProgress * 0.35f,
                                0.15f
                            );

                            Lighting.AddLight(
                                Projectile.Center,
                                Color.DeepPink.ToVector3() *
                                chargeProgress *
                                0.35f
                            );

                            StopLaserSound();
                        }
                        else if (laserTimer < LaserFireTime - Projectile.ai[1] * 10)
                        {
                            // Active firing phase.
                            laserActive = true;

                            laserVisualStrength = MathHelper.Lerp(
                                laserVisualStrength,
                                1f,
                                0.25f
                            );

                            Lighting.AddLight(
                                Projectile.Center,
                                Color.DeepPink.ToVector3() * 0.75f
                            );

                            UpdateLaserSound();
                        }
                        else
                        {
                            // Firing period finished; begin charging again.
                            laserTimer = (int)(-LaserChargeTime - (Projectile.ai[1] * 10));
                            laserActive = false;

                            StopLaserSound();
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else if (Projectile.ai[0] > 60f && Projectile.ai[1] % 3 != 0) //attack nearby enemy
                {
                    laserTimer = -LaserChargeTime;
                    laserActive = false;

                    laserVisualStrength = MathHelper.Lerp(
                        laserVisualStrength,
                        0f,
                        0.2f
                    );

                    StopLaserSound();

                    Projectile.ai[0] = 10 + Main.rand.Next(10);
                    float maxDistance = 800f;
                    int target = -1;
                    foreach (NPC npc in Main.ActiveNPCs)
                    {
                        if (npc.CanBeChasedBy(Projectile))
                        {
                            float npcDistance = Projectile.Distance(npc.Center);
                            if (npcDistance < maxDistance)
                            {
                                maxDistance = npcDistance;
                                target = npc.whoAmI;
                            }
                        }
                    }
                    if (target != -1)
                    {
                        Projectile.velocity = Vector2.Normalize(Main.npc[target].Center - Projectile.Center) * 13f + (Main.npc[target].velocity / 2f) - (playerVel / 2f);
                        Projectile.ai[0] *= -1f;
                    }
                    Projectile.netUpdate = true;
                }
                else if (Projectile.ai[1] % 3 == 0)
                {
                    laserTimer = -LaserChargeTime;
                    laserActive = false;

                    laserVisualStrength = MathHelper.Lerp(
                        laserVisualStrength,
                        0f,
                        0.2f
                    );

                    StopLaserSound();

                    // Aim at the mouse.
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Vector2 aimPosition = Projectile.Center + Vector2.UnitY * 4f;
                        Vector2 mouseWorld = Main.MouseWorld;

                        Projectile.rotation = (aimPosition - mouseWorld).ToRotation();

                        if (mouseWorld.X > Projectile.Center.X)
                        {
                            Projectile.rotation -= Pi;
                            Projectile.spriteDirection = -1;
                        }
                        else
                        {
                            Projectile.spriteDirection = 1;
                        }

                        Projectile.netUpdate = true;
                    }

                    // Fire periodically.
                    if (Projectile.ai[0] >= 6f && Owner.controlUseItem)
                    {
                        shootingDelay--;

                        if (shootingDelay <= 0)
                        {
                            shootingDelay = 60;

                            FireBullet();
                        }
                    }
                }
            }

            //tentacle segment updates
            segment[0] = Owner.Center;
            for (int i = 1; i < 5; i++)
            {
                MoveSegment(segment[i - 1], ref segment[i], segment[i + 1]);
            }
            MoveSegment(segment[4], ref segment[5], Projectile.Center + Projectile.velocity);
        }

        private static void MoveSegment(Vector2 previous, ref Vector2 current, Vector2 next)
        {
            current = previous + next;
            current /= 2;
        }

        private void UpdateLaserSound()
        {
            if (SoundEngine.TryGetActiveSound(laserSoundSlot, out ActiveSound activeSound) && activeSound.IsPlaying)
            {
                activeSound.Position = Projectile.Center;
                activeSound.Volume = 0.4f;
                return;
            }

            string soundPath = ((int)Projectile.ai[1] % 4) switch
            {
                0 => "CalamityMod/Sounds/Item/MittWelding/Weld1",
                1 => "CalamityMod/Sounds/Item/MittWelding/Weld2",
                2 => "CalamityMod/Sounds/Item/MittWelding/Weld3",
                _ => "CalamityMod/Sounds/Item/MittWelding/Weld4"
            };

            SoundStyle laserSound = new(soundPath)
            {
                Volume = 0.4f,
                IsLooped = true
            };

            laserSoundSlot = SoundEngine.PlaySound(
                laserSound,
                Projectile.Center
            );
        }

        private void StopLaserSound()
        {
            if (SoundEngine.TryGetActiveSound(
                laserSoundSlot,
                out ActiveSound activeSound
            ))
            {
                activeSound.Stop();
            }
        }

        public void FireBullet()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 spawnPosition = new(Projectile.Center.X,Projectile.Center.Y + 5f);
                Vector2 velocity = new Vector2(6f, 0f).RotatedBy((Main.MouseWorld - spawnPosition).ToRotation());
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, velocity, ModContent.ProjectileType<BlixerCannonPro>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode || !laserActive)
            {
                return null;
            }

            Vector2 beamStart = Projectile.Center + laserDirection * 8f;
            Vector2 beamEnd = beamStart + laserDirection * LaserLength;

            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), beamStart, beamEnd, LaserThickness * Projectile.scale, ref collisionPoint);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (laserActive)
            {
                modifiers.SourceDamage /= 4f;
            }
            else
            {
                modifiers.SourceDamage *= 2.5f; // only effects the hand bashing
            }
        }

        public override void OnKill(int timeLeft)
        {
            StopLaserSound();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            GameShaders.Armor.ApplySecondary(Owner.cBody, Owner, new DrawData?());
            Texture2D texture2D13 = Projectile.ai[1] % 3 != 0 ? Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode ? ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Donor/BlixerCore/BlixerCannon").Value : Terraria.GameContent.TextureAssets.Projectile[Type].Value : ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Donor/BlixerCore/BlixerCannon").Value;
            Texture2D segmentSprite = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Donor/BlixerCore/BlixerArm").Value;
            for (int i = 0; i < 5; i++)
            {
                Vector2 direction = Projectile.Center - segment[i];
                float rotation = direction.ToRotation();

                SpriteEffects effects = SpriteEffects.None;
                if (direction.X < 0f)
                    effects = SpriteEffects.FlipHorizontally;

                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                    case 2:
                        segmentSprite = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Donor/BlixerCore/BlixerArm").Value;
                        break;
                    case 3:
                    case 4:
                        segmentSprite = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Donor/BlixerCore/BlixerArmLarge").Value;
                        break;
                    default:
                        break;

                }
                Main.spriteBatch.Draw(segmentSprite, segment[i] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), segmentSprite.Bounds, Projectile.GetAlpha(lightColor), rotation, segmentSprite.Bounds.Size() / 2f, Projectile.scale, effects, 0);
            }

            Vector2 handDirection = Projectile.Center - segment[5];

            if (Projectile.ai[1] % 3 != 0 && !Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode)
                Projectile.rotation = handDirection.ToRotation();

            SpriteEffects handEffects;

            if (Projectile.ai[1] % 3 == 0 || Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode)
            {
                handEffects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
            else
            {
                handEffects = handDirection.X < 0f  ? SpriteEffects.FlipVertically : SpriteEffects.None;
            }

            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), texture2D13.Bounds, Projectile.GetAlpha(lightColor), Projectile.rotation, texture2D13.Bounds.Size() / 2f, Projectile.scale, handEffects, 0);

            if (Owner.GetModPlayer<InfernalPlayer>().blixerLaserMode && laserActive)
            {
                Texture2D laserTexture = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLineFade").Value;

                Texture2D coreTexture = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomLineThick").Value;

                Vector2 beamStart = Projectile.Center + laserDirection * 8f;
                Vector2 beamCenter = beamStart + laserDirection * (LaserLength * 0.5f);

                float beamRotation = laserDirection.ToRotation() + PiOver2;

                Color outerColor = new Color(255, 20, 147, 0);
                Color coreColor = new Color(255, 190, 230, 0);

                float pulse = 0.9f + Main.rand.NextFloat(0.2f);

                Vector2 outerScale = new(LaserThickness * pulse / laserTexture.Width, LaserLength / laserTexture.Height);

                Vector2 coreScale = new(LaserThickness * 0.25f / coreTexture.Width, LaserLength / coreTexture.Height);

                Main.spriteBatch.Draw(laserTexture, beamCenter - Main.screenPosition, null, outerColor, beamRotation, laserTexture.Size() * 0.5f, outerScale, SpriteEffects.FlipVertically, 0f);

                Main.spriteBatch.Draw(coreTexture, beamCenter - Main.screenPosition, null, coreColor, beamRotation, coreTexture.Size() * 0.5f, coreScale, SpriteEffects.FlipVertically, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }

    public class BlixerBuff : ModBuff
    {
        public override string Texture => "CalamityMod/Buffs/DamageOverTime/VulnerabilityHex";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            InfernalPlayer modPlayer = player.GetModPlayer<InfernalPlayer>();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BlixerHand>()] > 0)
                modPlayer.blixerCoreSummon = true;
            if (!modPlayer.blixerCoreSummon)
            {
                player.DelBuff(buffIndex);
                --buffIndex;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
    }
}
