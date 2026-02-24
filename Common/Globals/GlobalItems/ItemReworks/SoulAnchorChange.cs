using System.Collections.Generic;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Core.Players.ThoriumPlayerOverrides;

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

            return false;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) || !InfernalConfig.Instance.ThoriumBalanceChangess)
                return;

            if (!thoriumMod.TryFind("SoulAnchor", out ModItem soulAnchor))
                return;

            if (item.type == soulAnchor.Type)
            {
                foreach (TooltipLine tooltip in tooltips)
                {
                    if (tooltip.Text.Contains(Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SoulAnchor.OrigTooltip1")))
                    {
                        tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SoulAnchor.Replace1");
                    }

                    if (tooltip.Text.Contains(Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SoulAnchor.OrigTooltip2")))
                    {
                        tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SoulAnchor.Replace2");
                    }

                    if (tooltip.Text.Contains(Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SoulAnchor.OrigTooltip3")))
                    {
                        tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SoulAnchor.Replace3");
                    }
                }
            }
        }

        private bool IsSoulAnchor(Item item)
        {
            return ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                   thorium.TryFind("SoulAnchor", out ModItem soulAnchor) &&
                   item.type == soulAnchor.Type 
                   && InfernalConfig.Instance.ThoriumBalanceChangess;
        }
    }
}
