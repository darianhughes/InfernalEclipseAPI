using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.RogueThrower
{
    //Provided by Wardrobe Hummus
    public class ThrowerNoExhaustionTooltip : GlobalItem
    {
        private static int[] supportedTypes;
        private static bool initialized;

        private void EnsureInitialized()
        {
            if (initialized)
                return;
            List<int> intList = new List<int>();
            ModItem modItem1;
            if (ModContent.TryFind("ThoriumMod/ThrowingGuideVolume2", out modItem1))
                intList.Add(modItem1.Type);
            ModItem modItem2;
            if (ModContent.TryFind("ThoriumMod/ThrowingGuideVolume3", out modItem2))
                intList.Add(modItem2.Type);
            ModItem modItem4;
            if (ModContent.TryFind("FargowiltasCrossmod/VagabondsSoul", out modItem4))
                intList.Add(modItem4.Type);
            ModItem modItem5;
            if (ModContent.TryFind("ThoriumMod/WhiteDwarfThrusters", out modItem5))
                intList.Add(modItem5.Type);
            supportedTypes = intList.ToArray();
            initialized = true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            EnsureInitialized();
            if (supportedTypes == null || !supportedTypes.Contains(item.type) || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            tooltips.Add(new TooltipLine(Mod, "NoExhaustion", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.NoExhaustion"))
            {
                OverrideColor = new Color?(Color.LawnGreen)
            });
        }
    }
}
