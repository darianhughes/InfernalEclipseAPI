using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems.Tooltips
{
    //Provided by Wardrobe Hummus
    public class WingTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;

            if (!thoriumMod.TryFind("SubspaceWings", out ModItem subspaceWings) ||
                !thoriumMod.TryFind("TerrariumWings", out ModItem terrariumWings))
                return;

            if (item.type == subspaceWings.Type)
            {
                tooltips.Add(new TooltipLine(Mod, "WingInfo", "Also increases life regen by 5")
                {
                    OverrideColor = Color.White
                });
            }
            else if (item.type == terrariumWings.Type)
            {
                tooltips.Add(new TooltipLine(Mod, "WingInfo", "Also increases life regen by 10")
                {
                    OverrideColor = Color.White
                });
            }
        }
    }
}
