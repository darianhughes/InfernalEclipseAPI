using System.Collections.Generic;
using InfernalEclipseAPI.Core.Configs;
using InfernalEclipseAPI.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Multi
{
    //Provided by Wardrobe Hummus
    public class MjolnirChange : GlobalItem
    {
        private const int MjolnirBaseDamage = 1666;

        public override void UpdateInventory(Item item, Player player)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod) || !mod.TryFind("Mjolnir", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            item.damage = player.slotsMinions > 0 ? MjolnirBaseDamage / 5 : MjolnirBaseDamage;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod) || !mod.TryFind("Mjolnir", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            Color color = Color.Lerp(Color.White, new Color(30, 144, byte.MaxValue), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
            InfernalUtilities.AddTooltip(tooltips, Main.LocalPlayer.slotsMinions > 0.0 ? Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ScytheSummonOn") : Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ScytheSummon"), color);
        }
    }
}
