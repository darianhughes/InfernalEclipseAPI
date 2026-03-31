using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfernalEclipseAPI.YharimEX.Core.Globals;
using InfernalEclipseAPI.YharimEX.Core.Players;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace InfernalEclipseAPI.YharimEX.Core.Systems
{
    public static class YharimEXUtils
    {
        private static readonly FieldInfo _damageFieldHitInfo = typeof(NPC.HitInfo).GetField("_damage", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo _damageFieldHurtInfo = typeof(Player.HurtInfo).GetField("_damage", BindingFlags.Instance | BindingFlags.NonPublic);

        public static TooltipLine ArticlePrefixAdjustment(this TooltipLine itemName, string[] localizationArticles)
        {
            List<string> list = itemName.Text.Split(' ').ToList();
            for (int i = 0; i < localizationArticles.Length; i++)
            {
                if (list.Remove(localizationArticles[i]))
                {
                    list.Insert(0, localizationArticles[i]);
                    break;
                }
            }

            itemName.Text = string.Join(" ", list);
            return itemName;
        }

        public static string ArticlePrefixAdjustmentString(this string itemName, string[] localizationArticles)
        {
            List<string> list = itemName.Split(' ').ToList();
            for (int i = 0; i < localizationArticles.Length; i++)
            {
                if (list.Remove(localizationArticles[i]))
                {
                    list.Insert(0, localizationArticles[i]);
                    break;
                }
            }

            itemName = string.Join(" ", list);
            return itemName;
        }

        public static bool TryFindTooltipLine(this List<TooltipLine> tooltips, string tooltipName, out TooltipLine tooltipLine)
        {
            tooltips.TryFindTooltipLine(tooltipName, "Terraria", out tooltipLine);
            return tooltipLine != null;
        }

        public static bool TryFindTooltipLine(this List<TooltipLine> tooltips, string tooltipName, string tooltipMod, out TooltipLine tooltipLine)
        {
            tooltipLine = tooltips.First((TooltipLine line) => line.Name == tooltipName && line.Mod == tooltipMod);
            return tooltipLine != null;
        }

        public static void Null(this ref NPC.HitInfo hitInfo)
        {
            object obj = hitInfo;
            _damageFieldHitInfo.SetValue(obj, 0);
            hitInfo = (NPC.HitInfo)obj;
            hitInfo.Knockback = 0f;
            hitInfo.Crit = false;
            hitInfo.InstantKill = false;
        }

        public static void Null(this ref NPC.HitModifiers hitModifiers)
        {
            hitModifiers.ModifyHitInfo += delegate (ref NPC.HitInfo hitInfo)
            {
                hitInfo.Null();
            };
        }

        public static void Null(this ref Player.HurtInfo hurtInfo)
        {
            object obj = hurtInfo;
            _damageFieldHurtInfo.SetValue(obj, 0);
            hurtInfo = (Player.HurtInfo)obj;
            hurtInfo.Knockback = 0f;
        }

        public static void Null(this ref Player.HurtModifiers hurtModifiers)
        {
            hurtModifiers.ModifyHurtInfo += delegate (ref Player.HurtInfo hurtInfo)
            {
                hurtInfo.Null();
            };
        }

        public static void AddDebuffImmunities(this NPC npc, List<int> debuffs)
        {
            foreach (int debuff in debuffs)
            {
                NPCID.Sets.SpecificDebuffImmunity[npc.type][debuff] = true;
            }
        }

        public static YharimEXGlobalNPC YharimEX(this NPC npc)
        {
            return npc.GetGlobalNPC<YharimEXGlobalNPC>();
        }

        public static YharimEXPlayer YharimPlayer(this Player player)
        {
            return player.GetModPlayer<YharimEXPlayer>();
        }

        public static bool Alive(this Player player)
        {
            if (player != null && player.active && !player.dead)
            {
                return !player.ghost;
            }

            return false;
        }

        public static bool Alive(this Projectile projectile)
        {
            return projectile?.active ?? false;
        }

        public static bool Alive(this NPC npc)
        {
            return npc?.active ?? false;
        }

        public static bool TypeAlive(this Projectile projectile, int type)
        {
            if (projectile.Alive())
            {
                return projectile.type == type;
            }

            return false;
        }

        public static bool TypeAlive<T>(this Projectile projectile) where T : ModProjectile
        {
            if (projectile.Alive())
            {
                return projectile.type == ModContent.ProjectileType<T>();
            }

            return false;
        }

        public static bool TypeAlive(this NPC npc, int type)
        {
            if (npc.Alive())
            {
                return npc.type == type;
            }

            return false;
        }

        public static bool TypeAlive<T>(this NPC npc) where T : ModNPC
        {
            if (npc.Alive())
            {
                return npc.type == ModContent.NPCType<T>();
            }

            return false;
        }
        public static float ActualClassDamage(this Player player, DamageClass damageClass)
        {
            return player.GetTotalDamage(damageClass).Additive * player.GetTotalDamage(damageClass).Multiplicative;
        }

        public static bool IsWeapon(this Item item)
        {
            if (item.damage <= 0 || item.pick != 0 || item.axe != 0 || item.hammer != 0)
            {
                return item.type == ItemID.CoinGun;
            }

            return true;
        }

        public static bool IsWeaponWithDamageClass(this Item item)
        {
            if (item.damage <= 0 || item.DamageType == DamageClass.Default || item.pick != 0 || item.axe != 0 || item.hammer != 0)
            {
                return item.type == ItemID.CoinGun;
            }

            return true;
        }

        public static bool IsWithinBounds(this int index, int cap)
        {
            if (index >= 0)
            {
                return index < cap;
            }

            return false;
        }

        public static bool IsWithinBounds(this int index, int lowerBound, int higherBound)
        {
            if (index >= lowerBound)
            {
                return index < higherBound;
            }

            return false;
        }

        public static Vector2 SetMagnitude(this Vector2 vector, float magnitude)
        {
            return vector.SafeNormalize(Vector2.UnitY) * magnitude;
        }

        public static float ActualClassCrit(this Player player, DamageClass damageClass)
        {
            {
                return player.GetTotalCritChance(damageClass);
            }
        }

        public static bool FeralGloveReuse(this Player player, Item item)
        {
            if (player.autoReuseGlove)
            {
                if (!item.CountsAsClass(DamageClass.Melee))
                {
                    return item.CountsAsClass(DamageClass.SummonMeleeSpeed);
                }

                return true;
            }

            return false;
        }
        public static bool CountsAsClass(this DamageClass damageClass, DamageClass intendedClass)
        {
            if (damageClass != intendedClass)
            {
                return damageClass.GetEffectInheritance(intendedClass);
            }

            return true;
        }

        public static DamageClass ProcessDamageTypeFromHeldItem(this Player player)
        {
            if (player.HeldItem.damage <= 0 || player.HeldItem.pick > 0 || player.HeldItem.axe > 0 || player.HeldItem.hammer > 0)
            {
                return DamageClass.Summon;
            }

            if (player.HeldItem.DamageType.CountsAsClass(DamageClass.Melee))
            {
                return DamageClass.Melee;
            }

            if (player.HeldItem.DamageType.CountsAsClass(DamageClass.Ranged))
            {
                return DamageClass.Ranged;
            }

            if (player.HeldItem.DamageType.CountsAsClass(DamageClass.Magic))
            {
                return DamageClass.Magic;
            }

            if (player.HeldItem.DamageType.CountsAsClass(DamageClass.Summon))
            {
                return DamageClass.Summon;
            }

            if (player.HeldItem.DamageType != DamageClass.Generic && player.HeldItem.DamageType != DamageClass.Default)
            {
                return player.HeldItem.DamageType;
            }

            return DamageClass.Summon;
        }

        public static void Animate(this Projectile proj, int ticksPerFrame, int startFrame = 0, int? frames = null)
        {
            int valueOrDefault = frames.GetValueOrDefault();
            if (!frames.HasValue)
            {
                valueOrDefault = Main.projFrames[proj.type];
                frames = valueOrDefault;
            }

            if (++proj.frameCounter >= ticksPerFrame)
            {
                if (++proj.frame >= startFrame + frames)
                {
                    proj.frame = startFrame;
                }

                proj.frameCounter = 0;
            }
        }

        public static Rectangle ToWorldCoords(this Rectangle rectangle)
        {
            return new Rectangle(rectangle.X * 16, rectangle.Y * 16, rectangle.Width * 16, rectangle.Height * 16);
        }

        public static Rectangle ToTileCoords(this Rectangle rectangle)
        {
            return new Rectangle(rectangle.X / 16, rectangle.Y / 16, rectangle.Width / 16, rectangle.Height / 16);
        }

        public static void SetTexture1(this Texture2D texture) => Main.instance.GraphicsDevice.Textures[1] = texture;

        public static bool WorldIsExpertOrHarder() => Main.expertMode || (Main.GameModeInfo.IsJourneyMode && CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>().StrengthMultiplierToGiveNPCs >= 2);
        public static bool HostCheck => Main.netMode != NetmodeID.MultiplayerClient;
        public static void AddDebuffFixedDuration(Player player, int buffID, int intendedTime, bool quiet = true)
        {
            if (WorldIsExpertOrHarder() && BuffID.Sets.LongerExpertDebuff[buffID])
            {
                float debuffTimeMultiplier = Main.GameModeInfo.DebuffTimeMultiplier;
                if (Main.GameModeInfo.IsJourneyMode)
                {
                    if (Main.masterMode)
                        debuffTimeMultiplier = Main.RegisteredGameModes[2].DebuffTimeMultiplier;
                    else if (Main.expertMode)
                        debuffTimeMultiplier = Main.RegisteredGameModes[1].DebuffTimeMultiplier;
                }
                player.AddBuff(buffID, (int)Math.Round(intendedTime / debuffTimeMultiplier, MidpointRounding.ToEven), quiet);
            }
            else
            {
                player.AddBuff(buffID, intendedTime, quiet);
            }
        }

        public static float ProjWorldDamage => Main.GameModeInfo.IsJourneyMode
            ? CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>().StrengthMultiplierToGiveNPCs
            : Main.GameModeInfo.EnemyDamageMultiplier;

        public static int ScaledProjectileDamage(int npcDamage, float modifier = 1, int npcDamageCalculationsOffset = 2)
        {
            const float inherentHostileProjMultiplier = 2;
            float worldDamage = ProjWorldDamage;
            return (int)(modifier * npcDamage / inherentHostileProjMultiplier / Math.Max(npcDamageCalculationsOffset, worldDamage));
        }
        public static bool IsSummonDamage(Projectile projectile, bool includeMinionShot = true, bool includeWhips = true)
        {
            if (!includeWhips && ProjectileID.Sets.IsAWhip[projectile.type])
                return false;

            if (!includeMinionShot && (ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type]))
                return false;

            return projectile.CountsAsClass(DamageClass.Summon) || projectile.minion || projectile.sentry || projectile.minionSlots > 0 || ProjectileID.Sets.MinionSacrificable[projectile.type]
                || (includeMinionShot && (ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type]))
                || (includeWhips && ProjectileID.Sets.IsAWhip[projectile.type]);
        }

        public static bool CanDeleteProjectile(Projectile projectile, int deletionRank = 0, bool clearSummonProjs = false)
        {
            if (!projectile.active)
                return false;
            if (projectile.damage <= 0)
                return false;
            //        if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            //        {
            //            if (projectile.FargoSouls().DeletionImmuneRank > deletionRank)
            //                return false;
            //        }
            if (projectile.friendly)
            {
                if (projectile.whoAmI == Main.player[projectile.owner].heldProj)
                    return false;
                if (IsSummonDamage(projectile, false) && !clearSummonProjs)
                    return false;
            }
            return true;
        }

        public static Projectile ProjectileExists(int whoAmI, params int[] types)
        {
            return whoAmI > -1 && whoAmI < Main.maxProjectiles && Main.projectile[whoAmI].active && (types.Length == 0 || types.Contains(Main.projectile[whoAmI].type)) ? Main.projectile[whoAmI] : null;
        }

        public static Projectile ProjectileExists(float whoAmI, params int[] types)
        {
            return ProjectileExists((int)whoAmI, types);
        }

        public static bool OtherBossAlive(int npcID)
        {
            if (npcID > -1 && npcID < Main.maxNPCs)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].boss && i != npcID)
                        return true;
                }
            }
            return false;
        }
        public static NPC NPCExists(int whoAmI, params int[] types)
        {
            return whoAmI > -1 && whoAmI < Main.maxNPCs && Main.npc[whoAmI].active && (types.Length == 0 || types.Contains(Main.npc[whoAmI].type)) ? Main.npc[whoAmI] : null;
        }

        public static NPC NPCExists(float whoAmI, params int[] types)
        {
            return NPCExists((int)whoAmI, types);
        }
        public static int GetProjectileByIdentity(int player, float projectileIdentity, params int[] projectileType)
        {
            return GetProjectileByIdentity(player, (int)projectileIdentity, projectileType);
        }

        public static int GetProjectileByIdentity(int player, int projectileIdentity, params int[] projectileType)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].identity == projectileIdentity && Main.projectile[i].owner == player
                    && (projectileType.Length == 0 || projectileType.Contains(Main.projectile[i].type)))
                {
                    return i;
                }
            }
            return -1;
        }
        public static Player PlayerExists(int whoAmI)
        {
            return whoAmI > -1 && whoAmI < Main.maxPlayers && Main.player[whoAmI].active && !Main.player[whoAmI].dead && !Main.player[whoAmI].ghost ? Main.player[whoAmI] : null;
        }

        public static Player PlayerExists(float whoAmI)
        {
            return PlayerExists((int)whoAmI);
        }

        public static void ClearFriendlyProjectiles(int deletionRank = 0, int bossNpc = -1, bool clearSummonProjs = false)
        {
            ClearProjectiles(false, true, deletionRank, bossNpc, clearSummonProjs);
        }

        public static void ClearHostileProjectiles(int deletionRank = 0, int bossNpc = -1)
        {
            ClearProjectiles(true, false, deletionRank, bossNpc);
        }

        public static void ClearAllProjectiles(int deletionRank = 0, int bossNpc = -1, bool clearSummonProjs = false)
        {
            ClearProjectiles(true, true, deletionRank, bossNpc, clearSummonProjs);
        }

        private static void ClearProjectiles(bool clearHostile, bool clearFriendly, int deletionRank = 0, int bossNpc = -1, bool clearSummonProjs = false)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (OtherBossAlive(bossNpc))
                clearHostile = false;

            for (int j = 0; j < 2; j++) //do twice to wipe out projectiles spawned by projectiles
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.active && ((projectile.hostile && clearHostile) || (projectile.friendly && clearFriendly)) && CanDeleteProjectile(projectile, deletionRank, clearSummonProjs))
                    {
                        projectile.Kill();
                    }
                }
            }
        }

        public static void PrintText(string text)
        {
            PrintText(text, Color.White);
        }

        public static void PrintLocalization(string localizationKey, Color color)
        {
            PrintText(Language.GetTextValue(localizationKey), color);
        }

        public static void PrintLocalization(string localizationKey, int r, int g, int b) => PrintLocalization(localizationKey, new Color(r, g, b));

        public static void PrintLocalization(string localizationKey, Color color, params object[] args) => PrintText(Language.GetTextValue(localizationKey, args), color);

        public static void PrintText(string text, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
        }

        public static void PrintText(string text, int r, int g, int b) => PrintText(text, new Color(r, g, b));

        public static Vector2 ClosestPointInHitbox(Rectangle hitboxOfTarget, Vector2 desiredLocation)
        {
            Vector2 offset = desiredLocation - hitboxOfTarget.Center.ToVector2();
            offset.X = Math.Min(Math.Abs(offset.X), hitboxOfTarget.Width / 2) * Math.Sign(offset.X);
            offset.Y = Math.Min(Math.Abs(offset.Y), hitboxOfTarget.Height / 2) * Math.Sign(offset.Y);
            return hitboxOfTarget.Center.ToVector2() + offset;
        }

        public static Vector2 ClosestPointInHitbox(Entity entity, Vector2 desiredLocation)
        {
            return ClosestPointInHitbox(entity.Hitbox, desiredLocation);
        }
        public static int NewNPCEasy(IEntitySource source, Vector2 spawnPos, int type, int start = 0, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int target = 255, Vector2 velocity = default)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return Main.maxNPCs;

            int n = NPC.NewNPC(source, (int)spawnPos.X, (int)spawnPos.Y, type, start, ai0, ai1, ai2, ai3, target);
            if (n != Main.maxNPCs)
            {
                if (velocity != default)
                {
                    Main.npc[n].velocity = velocity;
                }

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
            }

            return n;
        }

        public static void SpawnBossNetcoded(Player player, int bossType, bool obeyLocalPlayerCheck = true)
        {
            if (player.whoAmI == Main.myPlayer || !obeyLocalPlayerCheck)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                if (HostCheck)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, bossType);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: bossType);
                }
            }
        }

        public static bool IsProjSourceItemUseReal(Projectile proj, IEntitySource source)
        {
            return source is EntitySource_ItemUse parent && parent.Item.type == Main.player[proj.owner].HeldItem.type;
        }

        public static bool AprilFools => DateTime.Today.Month == 4 && DateTime.Today.Day <= 7;

        public static void ScreenshakeRumble(float strength)
        {
            if (ScreenShakeSystem.OverallShakeIntensity < strength)
            {
                ScreenShakeSystem.SetUniversalRumble(strength, MathF.Tau, null, 0.2f);
            }
        }
        public static Color BuffEffects(Entity codable, Color lightColor, float shadow = 0f, bool effects = true, bool poisoned = false, bool onFire = false, bool onFire2 = false, bool hunter = false, bool noItems = false, bool blind = false, bool bleed = false, bool venom = false, bool midas = false, bool ichor = false, bool onFrostBurn = false, bool burned = false, bool honey = false, bool dripping = false, bool drippingSlime = false, bool loveStruck = false, bool stinky = false)
        {
            float cr = 1f; float cg = 1f; float cb = 1f; float ca = 1f;
            if (effects && honey && Main.rand.NextBool(30))
            {
                int dustID = Dust.NewDust(codable.position, codable.width, codable.height, DustID.Honey, 0f, 0f, 150, default, 1f);
                Main.dust[dustID].velocity.Y = 0.3f;
                Main.dust[dustID].velocity.X *= 0.1f;
                Main.dust[dustID].scale += Main.rand.Next(3, 4) * 0.1f;
                Main.dust[dustID].alpha = 100;
                Main.dust[dustID].noGravity = true;
                Main.dust[dustID].velocity += codable.velocity * 0.1f;
                //if (codable is Player) Main.playerDrawDust.Add(dustID);
            }
            if (poisoned)
            {
                if (effects && Main.rand.NextBool(30))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, DustID.Poisoned, 0f, 0f, 120, default, 0.2f);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].fadeIn = 1.9f;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
                cr *= 0.65f;
                cb *= 0.75f;
            }
            if (venom)
            {
                if (effects && Main.rand.NextBool(10))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, DustID.Venom, 0f, 0f, 100, default, 0.5f);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].fadeIn = 1.5f;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
                cg *= 0.45f;
                cr *= 0.75f;
            }
            if (midas)
            {
                cb *= 0.3f;
                cr *= 0.85f;
            }
            if (ichor)
            {
                if (codable is NPC) { lightColor = new Color(255, 255, 0, 255); } else { cb = 0f; }
            }
            if (burned)
            {
                if (effects)
                {
                    int dustID = Dust.NewDust(new Vector2(codable.position.X - 2f, codable.position.Y - 2f), codable.width + 4, codable.height + 4, DustID.Torch, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].velocity *= 1.8f;
                    Main.dust[dustID].velocity.Y -= 0.75f;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
                if (codable is Player)
                {
                    cr = 1f;
                    cb *= 0.6f;
                    cg *= 0.7f;
                }
            }
            if (onFrostBurn)
            {
                if (effects)
                {
                    if (Main.rand.Next(4) < 3)
                    {
                        int dustID = Dust.NewDust(new Vector2(codable.position.X - 2f, codable.position.Y - 2f), codable.width + 4, codable.height + 4, DustID.IceTorch, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 3.5f);
                        Main.dust[dustID].noGravity = true;
                        Main.dust[dustID].velocity *= 1.8f;
                        Main.dust[dustID].velocity.Y -= 0.5f;
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dustID].noGravity = false;
                            Main.dust[dustID].scale *= 0.5f;
                        }
                        //if (codable is Player) Main.playerDrawDust.Add(dustID);
                    }
                    Lighting.AddLight((int)(codable.position.X / 16f), (int)(codable.position.Y / 16f + 1f), 0.1f, 0.6f, 1f);
                }
                if (codable is Player)
                {
                    cr *= 0.5f;
                    cg *= 0.7f;
                }
            }
            if (onFire)
            {
                if (effects)
                {
                    if (!Main.rand.NextBool(4))
                    {
                        int dustID = Dust.NewDust(codable.position - new Vector2(2f, 2f), codable.width + 4, codable.height + 4, DustID.Torch, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 3.5f);
                        Main.dust[dustID].noGravity = true;
                        Main.dust[dustID].velocity *= 1.8f;
                        Main.dust[dustID].velocity.Y -= 0.5f;
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dustID].noGravity = false;
                            Main.dust[dustID].scale *= 0.5f;
                        }
                        //if (codable is Player) Main.playerDrawDust.Add(dustID);
                    }
                    Lighting.AddLight((int)(codable.position.X / 16f), (int)(codable.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
                }
                if (codable is Player)
                {
                    cb *= 0.6f;
                    cg *= 0.7f;
                }
            }
            if (dripping && shadow == 0f && !Main.rand.NextBool(4))
            {
                Vector2 position = codable.position;
                position.X -= 2f; position.Y -= 2f;
                if (Main.rand.NextBool())
                {
                    int dustID = Dust.NewDust(position, codable.width + 4, codable.height + 2, DustID.Wet, 0f, 0f, 50, default, 0.8f);
                    if (Main.rand.NextBool()) Main.dust[dustID].alpha += 25;
                    if (Main.rand.NextBool()) Main.dust[dustID].alpha += 25;
                    Main.dust[dustID].noLight = true;
                    Main.dust[dustID].velocity *= 0.2f;
                    Main.dust[dustID].velocity.Y += 0.2f;
                    Main.dust[dustID].velocity += codable.velocity;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
                else
                {
                    int dustID = Dust.NewDust(position, codable.width + 8, codable.height + 8, DustID.Wet, 0f, 0f, 50, default, 1.1f);
                    if (Main.rand.NextBool()) Main.dust[dustID].alpha += 25;
                    if (Main.rand.NextBool()) Main.dust[dustID].alpha += 25;
                    Main.dust[dustID].noLight = true;
                    Main.dust[dustID].noGravity = true;
                    Main.dust[dustID].velocity *= 0.2f;
                    Main.dust[dustID].velocity.Y += 1f;
                    Main.dust[dustID].velocity += codable.velocity;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
            }
            if (drippingSlime && shadow == 0f)
            {
                int alpha = 175;
                Color newColor = new(0, 80, 255, 100);
                if (!Main.rand.NextBool(4))
                {
                    if (Main.rand.NextBool())
                    {
                        Vector2 position2 = codable.position;
                        position2.X -= 2f; position2.Y -= 2f;
                        int dustID = Dust.NewDust(position2, codable.width + 4, codable.height + 2, DustID.TintableDust, 0f, 0f, alpha, newColor, 1.4f);
                        if (Main.rand.NextBool()) Main.dust[dustID].alpha += 25;
                        if (Main.rand.NextBool()) Main.dust[dustID].alpha += 25;
                        Main.dust[dustID].noLight = true;
                        Main.dust[dustID].velocity *= 0.2f;
                        Main.dust[dustID].velocity.Y += 0.2f;
                        Main.dust[dustID].velocity += codable.velocity;
                        //if (codable is Player) Main.playerDrawDust.Add(dustID);
                    }
                }
                cr *= 0.8f;
                cg *= 0.8f;
            }
            if (onFire2)
            {
                if (effects)
                {
                    if (!Main.rand.NextBool(4))
                    {
                        int dustID = Dust.NewDust(codable.position - new Vector2(2f, 2f), codable.width + 4, codable.height + 4, DustID.CursedTorch, codable.velocity.X * 0.4f, codable.velocity.Y * 0.4f, 100, default, 3.5f);
                        Main.dust[dustID].noGravity = true;
                        Main.dust[dustID].velocity *= 1.8f;
                        Main.dust[dustID].velocity.Y -= 0.5f;
                        if (Main.rand.NextBool(4))
                        {
                            Main.dust[dustID].noGravity = false;
                            Main.dust[dustID].scale *= 0.5f;
                        }
                        //if (codable is Player) Main.playerDrawDust.Add(dustID);
                    }
                    Lighting.AddLight((int)(codable.position.X / 16f), (int)(codable.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
                }
                if (codable is Player)
                {
                    cb *= 0.6f;
                    cg *= 0.7f;
                }
            }
            if (noItems)
            {
                cr *= 0.65f;
                cg *= 0.8f;
            }
            if (blind)
            {
                cr *= 0.7f;
                cg *= 0.65f;
            }
            if (bleed)
            {
                bool dead = codable is Player player ? player.dead : codable is NPC nPC && nPC.life <= 0;
                if (effects && !dead && Main.rand.NextBool(30))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, DustID.Blood, 0f, 0f, 0, default, 1f);
                    Main.dust[dustID].velocity.Y += 0.5f;
                    Main.dust[dustID].velocity *= 0.25f;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
                cg *= 0.9f;
                cb *= 0.9f;
            }
            if (loveStruck && effects && shadow == 0f && Main.instance.IsActive && !Main.gamePaused && Main.rand.NextBool(5))
            {
                Vector2 value = new(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                value.Normalize();
                value.X *= 0.66f;
                int goreID = Gore.NewGore(codable.GetSource_FromThis(), codable.position + new Vector2(Main.rand.Next(codable.width + 1), Main.rand.Next(codable.height + 1)), value * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                Main.gore[goreID].sticky = false;
                Main.gore[goreID].velocity *= 0.4f;
                Main.gore[goreID].velocity.Y -= 0.6f;
                //if (codable is Player) Main.playerDrawGore.Add(goreID);
            }
            if (stinky && shadow == 0f)
            {
                cr *= 0.7f;
                cb *= 0.55f;
                if (effects && Main.rand.NextBool(5) && Main.instance.IsActive && !Main.gamePaused)
                {
                    Vector2 value2 = new(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                    value2.Normalize(); value2.X *= 0.66f; value2.Y = Math.Abs(value2.Y);
                    Vector2 vector = value2 * Main.rand.Next(3, 5) * 0.25f;
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, DustID.FartInAJar, vector.X, vector.Y * 0.5f, 100, default, 1.5f);
                    Main.dust[dustID].velocity *= 0.1f;
                    Main.dust[dustID].velocity.Y -= 0.5f;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
            }
            lightColor.R = (byte)(lightColor.R * cr);
            lightColor.G = (byte)(lightColor.G * cg);
            lightColor.B = (byte)(lightColor.B * cb);
            lightColor.A = (byte)(lightColor.A * ca);

            if (hunter && (codable is not NPC || ((NPC)codable).lifeMax > 1))
            {
                if (effects && !Main.gamePaused && Main.instance.IsActive && Main.rand.NextBool(50))
                {
                    int dustID = Dust.NewDust(codable.position, codable.width, codable.height, DustID.MagicMirror, 0f, 0f, 150, default, 0.8f);
                    Main.dust[dustID].velocity *= 0.1f;
                    Main.dust[dustID].noLight = true;
                    //if (codable is Player) Main.playerDrawDust.Add(dustID);
                }
                byte colorR = 50, colorG = 255, colorB = 50;
                if (codable is NPC nPC && !(nPC.friendly || nPC.catchItem > 0 || (nPC.damage == 0 && nPC.lifeMax == 5)))
                {
                    colorR = 255; colorG = 50;
                }
                if (codable is not NPC && lightColor.R < 150) { lightColor.A = Main.mouseTextColor; }
                if (lightColor.R < colorR) { lightColor.R = colorR; }
                if (lightColor.G < colorG) { lightColor.G = colorG; }
                if (lightColor.B < colorB) { lightColor.B = colorB; }
            }
            return lightColor;
        }

        #region Easings
        public static float SineInOut(float value) => (0f - (MathF.Cos((value * MathF.PI)) - 1f)) / 2f;
        #endregion

        public static void YharimEXIncapacitate(this Player player, bool preventDashing = true)
        {
            player.controlLeft = false;
            player.controlRight = false;
            player.controlJump = false;
            player.controlDown = false;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlHook = false;
            player.releaseHook = true;
            if (player.grapCount > 0)
                player.RemoveAllGrapplingHooks();
            if (player.mount.Active)
                player.mount.Dismount(player);
            player.GetModPlayer<YharimEXPlayer>().YharimEXNoUsingItems = 2;
            if (preventDashing)
            {
                for (int i = 0; i < 4; i++)
                {
                    player.doubleTapCardinalTimer[i] = 0;
                    player.holdDownCardinalTimer[i] = 0;
                }
            }
            if (player.dashDelay < 10 && preventDashing)
                player.dashDelay = 10;
        }
        public static Vector2 SmartAccel(Vector2 position, Vector2 destination, Vector2 velocity, float accel, float decel)
        {
            Vector2 dif = destination - position;

            if (dif == Vector2.Zero)
                return Vector2.Zero;
            if (velocity == Vector2.Zero)
                velocity = Vector2.UnitX * 0.1f;
            Vector2 a = velocity;
            Vector2 b = dif.SafeNormalize(Vector2.Zero);
            float scalarProj = Vector2.Dot(a, b);
            Vector2 vProj = b * scalarProj;
            Vector2 vOrth = velocity - vProj;
            Vector2 vProjN = vProj.SafeNormalize(Vector2.Zero);
            if (scalarProj > 0)
                velocity += vProjN * (float)SmartAccel1D(dif.Length(), vProj.Length(), accel, decel);
            else
                velocity -= vProjN * decel;
            velocity -= Math.Min(decel, vOrth.Length()) * vOrth.SafeNormalize(Vector2.Zero);

            return velocity;
        }

        public static double SmartAccel1D(double s, double v, double a, double d)
        {
            s = Math.Abs(s);
            a = Math.Abs(a);
            d = -Math.Abs(d);
            double root = Math.Abs(v * v / (d * d)) + 2 * s / d;
            if (root >= 0)
                return d;
            double root2 = -2 * s / d;
            if (root2 <= 0)
                return a;
            double accelFraction = (Math.Sqrt(root2) * -d - v) / a;
            if (accelFraction > 0 && accelFraction < 1)
                return accelFraction * a;
            return a;

        }
        public static bool BossIsAlive(ref int bossID, int bossType)
        {
            if (bossID != -1)
            {
                if (Main.npc[bossID].active && Main.npc[bossID].type == bossType)
                {
                    return true;
                }
                else
                {
                    bossID = -1;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static int FindClosestHostileNPC(Vector2 location, float detectionRange, bool lineCheck = false, bool prioritizeBoss = false)
        {
            NPC closestNpc = null;

            void FindClosest(IEnumerable<NPC> npcs)
            {
                float range = detectionRange;
                foreach (NPC n in npcs)
                {
                    if (n.CanBeChasedBy() && n.Distance(location) < range
                        && (!lineCheck || Collision.CanHitLine(location, 0, 0, n.Center, 0, 0)))
                    {
                        range = n.Distance(location);
                        closestNpc = n;
                    }
                }
            }

            if (prioritizeBoss)
                FindClosest(Main.npc.Where(n => n.boss));
            if (closestNpc == null)
                FindClosest(Main.npc);

            return closestNpc == null ? -1 : closestNpc.whoAmI;
        }
        public static int FindClosestHostileNPCPrioritizingMinionFocus(Projectile projectile, float detectionRange, bool lineCheck = false, Vector2 center = default, bool prioritizeBoss = false)
        {
            if (center == default)
                center = projectile.Center;

            NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
            if (minionAttackTargetNpc != null && minionAttackTargetNpc.CanBeChasedBy() && minionAttackTargetNpc.Distance(center) < detectionRange
                && (!lineCheck || Collision.CanHitLine(center, 0, 0, minionAttackTargetNpc.Center, 0, 0)))
            {
                return minionAttackTargetNpc.whoAmI;
            }
            return FindClosestHostileNPC(center, detectionRange, lineCheck, prioritizeBoss);
        }
    }
}
