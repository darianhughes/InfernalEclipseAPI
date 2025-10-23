using CalamityMod;
using InfernumMode.Core.GlobalInstances.Systems;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Content.Buffs;
using Terraria.ID;
using Terraria.DataStructures;
using InfernalEclipseAPI.Core.World;
using Terraria.Localization;
using InfernalEclipseAPI.Content.Items.Weapons.Legendary.Lycanroc;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.NPCs.SupremeCalamitas;
using InfernalEclipseAPI.Core.DamageClasses;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Core.Players
{
    public class InfernalPlayer : ModPlayer
    {
        private const int AdjRadius = 4;

        public override void PlayerConnect()
        {
            if (!InfernalWorld.craftedWorkshop && workshopHasBeenOwned)
            {
                InfernalWorld.craftedWorkshop = true;
            }

            if (!InfernalConfig.Instance.DisplayWorldEntryMessages) return;
            Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.MPConnect"), 95, 06, 06);

            if (InfernalConfig.Instance.InfernumModeForced && WorldSaveSystem.InfernumModeEnabled == false)
            {
                WorldSaveSystem.InfernumModeEnabled = true;
            }

            if (InfernalConfig.Instance.ForceFullXerocDialogue)
            {
                DownedBossSystem.startedBossRushAtLeastOnce = false;
            }
        }

        public override void OnEnterWorld()
        {
            if (!InfernalWorld.craftedWorkshop && workshopHasBeenOwned)
            {
                InfernalWorld.craftedWorkshop = true;
            }

            if (!InfernalConfig.Instance.DisplayWorldEntryMessages) return;

            Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.Welcome"), 95, 06, 06);

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                //This message should always popup upon entering a world if they are playing the mod pack.
                if (ModLoader.TryGetMod("ThoriumRework", out Mod rework))
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.TBRNotice"), 255, 255, 0);
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.TBRWarning"), 255, 0, 0);
                }

                if (!ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RagWarning"), 255, 0, 0);
                }
                else if (ragnarok != null)
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RagnarokBalance"), 255, 255, 0);

                    if (rework != null)
                    {
                        Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RagnarokRework"), 255, 255, 0);
                    }
                }
            }

            //Alerts the player if they have Fargo's Souls enabled.
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls)) 
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.SoulsWarning"), 255, 0, 0);
            }

            if (ModLoader.TryGetMod("CalamityMinus", out Mod calMinus))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.CalMinus"), 255, 255, 06);
            }

            if (ModLoader.TryGetMod("CalBalChange", out Mod calBal))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.CalBalNotice"), 255, 255, 06);
            }

            if (InfernumActive.InfernumActive)
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.InfernumActive"), 95, 06, 06);
                SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, Player.Center);
            }
            else if (InfernalConfig.Instance.InfernumModeForced)
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.InfernumForced"), 95, 06, 06);
                SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, Player.Center);
                WorldSaveSystem.InfernumModeEnabled = true;
            }

            if (InfernalConfig.Instance.ForceFullXerocDialogue)
            {
                DownedBossSystem.startedBossRushAtLeastOnce = false;
            }

        }

        private Vector2 previousPos;
        private bool wasUsingItem;
        private int horrifiedTimer = 0;
        private int jamTimer = 0;
        public int namelessDialogueCooldown;
        public int CloverCharmCooldown;
        public bool workshopHasBeenOwned;

        public override void Initialize()
        {
            workshopHasBeenOwned = false;
        }

        public override void SaveData(TagCompound tag)
        {
            if (workshopHasBeenOwned)
            {
                tag.Add("workshopHasBeenOwned", true);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            workshopHasBeenOwned = tag.Get<bool>("workshopHasBeenOwned");
        }

        public override void ResetEffects()
        {
            if (!Player.HasBuff(ModContent.BuffType<StarboundHorrification>()))
                horrifiedTimer = 0;

            if (!Player.HasBuff(ModContent.BuffType<WarpJammed>()))
                jamTimer = 0;

            if (namelessDialogueCooldown > 0)
                namelessDialogueCooldown--;

            if (CloverCharmCooldown > 0)
                CloverCharmCooldown--;

            if (namelessDialogueCooldown <= 0)
                InfernalWorld.namelessDeveloperDiagloguePlayed = false;

            soltanBullying = false;
            HarvestMoonBuff = false;
        }

        public override void PreUpdate()
        {
            if (Player.ZoneLihzhardTemple && !NPC.downedPlantBoss)
            {
                Player.statLife -= 1;
                Player.AddBuff(BuffID.PotionSickness, 60);
            }
        }

        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<StarboundHorrification>()))
            {
                horrifiedTimer++;

                // Give a 1 second grace period after first applying the buff
                if (horrifiedTimer < 60)
                {
                    previousPos = Player.position;
                    wasUsingItem = Player.itemAnimation > 0;
                    return;
                }

                float distanceMoved = Vector2.Distance(Player.position, previousPos);
                bool usedTeleportItem = !wasUsingItem && Player.itemAnimation > 0 &&
                    (Player.HeldItem.type == ItemID.MagicMirror ||
                     Player.HeldItem.type == ItemID.IceMirror ||
                     Player.HeldItem.type == ItemID.RecallPotion ||
                     Player.HeldItem.type == ItemID.WormholePotion ||
                     Player.HeldItem.type == ItemID.PotionOfReturn ||
                     Player.HeldItem.type == ItemID.CellPhone ||
                     Player.HeldItem.type == ItemID.Shellphone ||
                     Player.HeldItem.type == ItemID.ShellphoneHell ||
                     Player.HeldItem.type == ItemID.ShellphoneOcean ||
                     Player.HeldItem.type == ItemID.ShellphoneSpawn ||
                     Player.HeldItem.type == ItemID.DemonConch ||
                     Player.HeldItem.type == ItemID.MagicConch ||
                     Player.HeldItem.type == ItemID.TeleportationPotion);

                if (distanceMoved > 1000f || usedTeleportItem)
                {
                    SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, Player.Center);
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} tried to escape the multiversal terror."), 9999.0, 0);
                }

                previousPos = Player.position;
                wasUsingItem = Player.itemAnimation > 0;
            }

            if (Player.HasBuff(ModContent.BuffType<WarpJammed>()))
            {
                jamTimer++;

                // Give a 3 second grace period after first applying the buff
                if (jamTimer < 180)
                {
                    previousPos = Player.position;
                    wasUsingItem = Player.itemAnimation > 0;
                    return;
                }

                float distanceMoved = Vector2.Distance(Player.position, previousPos);
                bool usedTeleportItem = !wasUsingItem && Player.itemAnimation > 0 &&
                    (Player.HeldItem.type == ItemID.MagicMirror ||
                     Player.HeldItem.type == ItemID.IceMirror ||
                     Player.HeldItem.type == ItemID.RecallPotion ||
                     Player.HeldItem.type == ItemID.WormholePotion ||
                     Player.HeldItem.type == ItemID.PotionOfReturn ||
                     Player.HeldItem.type == ItemID.CellPhone ||
                     Player.HeldItem.type == ItemID.Shellphone ||
                     Player.HeldItem.type == ItemID.ShellphoneHell ||
                     Player.HeldItem.type == ItemID.ShellphoneOcean ||
                     Player.HeldItem.type == ItemID.ShellphoneSpawn ||
                     Player.HeldItem.type == ItemID.DemonConch ||
                     Player.HeldItem.type == ItemID.MagicConch ||
                     Player.HeldItem.type == ItemID.TeleportationPotion);

                if (distanceMoved > 1000f || usedTeleportItem)
                {
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaShootSound, Player.Center);
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} tried to escape draedon's creations."), 9999.0, 0);
                }

                previousPos = Player.position;
                wasUsingItem = Player.itemAnimation > 0;
            }
        }

        public bool soltanBullying = false;
        public bool HarvestMoonBuff = false;

        public override void PostUpdateMiscEffects()
        {
            if (soltanBullying)
            {
                float emptySummonSlots = Player.maxMinions - Player.slotsMinions;
                ref StatModifier melee = ref Player.GetDamage(DamageClass.Melee);
                melee += (float)(0.02 * emptySummonSlots);
                ref StatModifier ranged = ref Player.GetDamage(DamageClass.Ranged);
                ranged += (float)(0.02 * emptySummonSlots);
                ref StatModifier magic = ref Player.GetDamage(DamageClass.Magic);
                magic += (float)(0.02 * emptySummonSlots);
                ref StatModifier throwing = ref Player.GetDamage(DamageClass.Throwing);
                throwing += (float)(0.02 * emptySummonSlots);

                ref StatModifier summon = ref Player.GetDamage(DamageClass.Summon);
                summon -= (float)(0.1 * Player.slotsMinions);
            }
        }

        public override void PostUpdateEquips()
        {
            /*
            //Why why why why SOTS
            bool nearRealAlchemyTable = IsNearTile(TileID.AlchemyTable, AdjRadius);

            if (!nearRealAlchemyTable)
            {
                if (Player.adjTile[TileID.AlchemyTable]) // 355
                    Player.adjTile[TileID.AlchemyTable] = false;

                if (Player.alchemyTable)
                    Player.alchemyTable = false;
            }
            */
        }

        public void ConvertSummonMeleeToMelee(Player player, Item item, ref StatModifier damage)
        {
            if (item.DamageType == ModContent.GetInstance<MeleeWhip>())
                item.DamageType = DamageClass.SummonMeleeSpeed;

            if (!soltanBullying || item is null || item.IsAir)
                return;

            var summonMeleeSpeed = ModContent.GetInstance<SummonMeleeSpeedDamageClass>();
            if (!item.CountsAsClass(summonMeleeSpeed))
                return;

            // Replace the item's damage scaling with Melee scaling:
            /*
            float meleeScale = player.GetTotalDamage(DamageClass.Melee).ApplyTo(1f);
            float sourceScale = player.GetTotalDamage(summonMeleeSpeed).ApplyTo(1f);
            float ratio = meleeScale / MathF.Max(sourceScale, 1e-6f);
            */
            item.DamageType = ModContent.GetInstance<MeleeWhip>();

            //damage *= ratio;       // mimic Melee scaling
            damage *= 1.10f;       // extra 10% while SoltanBullying
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            ConvertSummonMeleeToMelee(Player, item, ref damage);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.whoAmI != Main.myPlayer) return;

            if (!proj.npcProj && !proj.trap && proj.friendly)
            {
                LycanrocGlobalProjectile lgp = proj.GetGlobalProjectile<LycanrocGlobalProjectile>();
                if (lgp.appliesArmorCrunch)
                {
                    target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180);
                }

                if (lgp.appliesCrumbling)
                {
                    target.AddBuff(ModContent.BuffType<Crumbling>(), 180);
                }
            }

            if ((proj.type == ModContent.ProjectileType<CelestusProj>() || proj.type == ModContent.ProjectileType<CelestusMiniScythe>()) && 
                (target.type == ModContent.NPCType<SepulcherHead>() || target.type == ModContent.NPCType<SepulcherBody>() || target.type == ModContent.NPCType<SepulcherTail>()) && 
                InfernalConfig.Instance.PreventBossCheese)
            {
                hit.Damage -= (int)(hit.Damage * 0.2);
            }
        }

        private bool IsNearTile(ushort tileType, int radius)
        {
            int px = (int)(Player.Center.X / 16f);
            int py = (int)(Player.Center.Y / 16f);

            for (int x = px - radius; x <= px + radius; x++)
            {
                if (x < 0 || x >= Main.maxTilesX) continue;
                for (int y = py - radius; y <= py + radius; y++)
                {
                    if (y < 0 || y >= Main.maxTilesY) continue;
                    Tile t = Main.tile[x, y];
                    if (t != null && t.HasTile && t.TileType == tileType)
                        return true;
                }
            }
            return false;
        }
    }

    public class SoltanGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            int owner = projectile.owner;
            if (owner < 0 || owner >= Main.maxPlayers)
                return;

            Player p = Main.player[owner];
            var mp = p.GetModPlayer<InfernalPlayer>();
            if (!mp.soltanBullying)
                return;

            var summonMeleeSpeed = ModContent.GetInstance<SummonMeleeSpeedDamageClass>();
            if (projectile.DamageType == summonMeleeSpeed)
            {
                // Make the projectile actually "be" Melee so Melee-only effects can see it.
                projectile.DamageType = DamageClass.Melee;
                projectile.netUpdate = true;
            }
        }
    }
}
