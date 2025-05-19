using System.Collections.Generic;
using InfernalEclipseAPI.Core.Players;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    public class SoulAnchorChange : GlobalItem
    {
        public override bool InstancePerEntity => false;
        public static Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                return thorium;
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (!IsSoulAnchor(item)) return base.AltFunctionUse(item, player);
            return player.GetModPlayer<SoulAnchorPlayer>().anchorLocation != Vector2.Zero;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (!IsSoulAnchor(item)) return base.CanUseItem(item, player);
            return !player.HasBuff(thorium.Find<ModBuff>("RevivalExhaustion").Type);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!IsSoulAnchor(item)) return base.Shoot(item, player, source, position, velocity, type, damage, knockback);

            var modPlayer = player.GetModPlayer<SoulAnchorPlayer>();

            if (player.altFunctionUse == 2)
            {
                modPlayer.TryTeleport();
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item46, player.Center);
                modPlayer.SetAnchor();
            }

            return false; // cancel projectile
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) || !InfernalConfig.Instance.ThoriumBalanceChangess 
                //|| ModLoader.TryGetMod("InfernalEclipseAPI", out _) //Don't add tooltip twice if both mods are enabled.
                )
                return;

            if (!thoriumMod.TryFind("SoulAnchor", out ModItem soulAnchor))
                return;

            if (item.type == soulAnchor.Type)
            {
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Text.Contains("Anchors your soul and health to your current position"))
                    {
                        tooltip.Text += " for 20 seconds";
                    }

                    if (tooltip.Text.Contains("Right click to return to your soul, reverting health to its original value"))
                    {
                        tooltip.Text = "Within that period, right click to return to your soul, reverting health to half its original value";
                    }

                    if (tooltip.Text.Contains("Can only be used once every 5 minutes and saps your soul upon returning"))
                    {
                        tooltip.Text = "Saps your soul upon returning and grants Potion Sickness\nCan only be used once every 5 minutes";
                    }
                }
            }
        }

        private bool IsSoulAnchor(Item item)
        {
            return ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                   thorium.TryFind("SoulAnchor", out ModItem soulAnchor) &&
                   item.type == soulAnchor.Type 
                   && InfernalConfig.Instance.ThoriumBalanceChangess; //remove this line to always change
        }
    }
}
