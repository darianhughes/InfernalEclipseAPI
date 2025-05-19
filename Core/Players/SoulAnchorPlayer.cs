using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;
using InfernalEclipseAPI.Content.Buffs;

namespace InfernalEclipseAPI.Core.Players
{
    public class SoulAnchorPlayer : ModPlayer
    {
        public static Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                return thorium;
            }
        }
        public Vector2 anchorLocation = Vector2.Zero;
        public int storedHP = 0;
        public ulong anchorSetTime = 0;

        private bool anchorExpiredMessageShown = false;

        public override void Initialize()
        {
            anchorLocation = Vector2.Zero;
            storedHP = 0;
            anchorSetTime = 0;
            anchorExpiredMessageShown = false;
        }
        public override void PostUpdate()
        {
            if (anchorLocation != Vector2.Zero)
            {
                if (Main.GameUpdateCount - anchorSetTime > 60 * 20 && !anchorExpiredMessageShown)
                {
                    SendChatToPlayer("Your Soul Anchor has faded.", Color.Gray);
                    anchorExpiredMessageShown = true;

                    // Clear anchor automatically if desired:
                    anchorLocation = Vector2.Zero;
                    storedHP = 0;
                    anchorSetTime = 0;
                }
            }
        }

        public void SetAnchor()
        {
            anchorLocation = Player.Center;
            storedHP = Player.statLife;
            anchorSetTime = Main.GameUpdateCount;
            anchorExpiredMessageShown = false;

            SendChatToPlayer("Soul Anchor set. You have 20 seconds to return.", Color.Cyan);
            Player.AddBuff(ModContent.BuffType<AnchoredSoul>(), 1200);
        }

        public void TryTeleport()
        {
            if (Main.GameUpdateCount - anchorSetTime <= 60 * 20)
            {
                Player.Teleport(anchorLocation, 1);
                int heal = storedHP / 2;
                Player.statLife = Math.Min(heal, Player.statLifeMax2);
                Player.HealEffect(heal);
                Player.AddBuff(BuffID.PotionSickness, 3600);
                Player.AddBuff(thorium.Find<ModBuff>("Mortality").Type, 600);
                Player.AddBuff(thorium.Find<ModBuff>("RevivalExhaustion").Type, 18000); // 5 minute cooldown
                Player.ClearBuff(ModContent.BuffType<AnchoredSoul>());
            }
            else if (!anchorExpiredMessageShown)
            {
                SendChatToPlayer("Your Soul Anchor has faded.", Color.Gray);
                anchorExpiredMessageShown = true;
            }

            // Clear anchor
            anchorLocation = Vector2.Zero;
            storedHP = 0;
            anchorSetTime = 0;
        }

        private void SendChatToPlayer(string message, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, color);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(message), color, Player.whoAmI);
            }
        }
    }
}
