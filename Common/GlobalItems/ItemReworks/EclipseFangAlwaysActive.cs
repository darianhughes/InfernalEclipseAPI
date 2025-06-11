using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class EclipseFangAlwaysActive : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod) || !mod.TryFind("EclipseFang", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess 
                //|| ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance)
                ) return base.UseItem(item, player);
            var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.itemEclipseFangCharge = 40;
            return base.UseItem(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod) || !mod.TryFind("EclipseFang", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess 
                //|| ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance)
                ) return;
            Color color = Color.Lerp(Color.White, new Color(30, 144, byte.MaxValue), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
            string str = "[IEoR]: Weapon empowerment always active.";
            tooltips.Add(new TooltipLine(Mod, "MjolnirInfo", str)
            {
                OverrideColor = new Color?(color)
            });
        }
    }
}
