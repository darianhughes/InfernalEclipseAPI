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

namespace InfernalEclipseAPI.Core.Players
{
    public class InfernalPlayer : ModPlayer
    {
        public override void PlayerConnect()
        {
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
                    Main.NewText("NOTICE: For the Balancing of weapons to be as intended, set the Ragnarok mod config of 'Generic Weapon Tweaks' to OFF, as otherwise the changes will stack and weapons will be far stronger than intended.", 255, 255, 0);

                    if (rework != null)
                    {
                        Main.NewText("NOTICE: It is detected that you have both the Ragnarok mod and Thorium Bosses Reworked enabled. For the intended experience, turn all the boss toggles in the Ragnarok Boss Config to Thorium Bosses Reworked.", 255, 255, 0);
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

        public override void PreUpdate()
        {
            if (Player.ZoneLihzhardTemple && !NPC.downedPlantBoss)
            {
                Player.statLife -= 1;
                Player.AddBuff(BuffID.PotionSickness, 60);
            }
        }
    }
}
