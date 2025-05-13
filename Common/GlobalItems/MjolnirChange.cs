using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    //Provided by Wardrobe Hummus
    public class MjolnirChange : GlobalItem
    {
        private const int MjolnirBaseDamage = 600;

        public override void UpdateInventory(Item item, Player player)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod) || !mod.TryFind("Mjolnir", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            item.damage = player.slotsMinions > 0.0 ? 200 : 600;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod) || !mod.TryFind("Mjolnir", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            Color color = Color.Lerp(Color.White, new Color(30, 144, byte.MaxValue), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
            string str = Main.LocalPlayer.slotsMinions > 0.0 ? "Damage reduced while summons are active" : "Has reduced damage if any summons are active";
            tooltips.Add(new TooltipLine(Mod, "MjolnirInfo", str)
            {
                OverrideColor = new Color?(color)
            });
        }
    }
}
