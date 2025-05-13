using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    //Provided by Wardrobe Hummus
    public class SoulEssenceBuffTooltip : GlobalItem
    {
        private static List<int> scytheTypes;

        public override void Load()
        {
            scytheTypes = new List<int>();
            Mod mod1;
            if (ModLoader.TryGetMod("RagnarokMod", out mod1))
            {
                ModItem modItem1;
                if (mod1.TryFind("MarbleScythe", out modItem1))
                    scytheTypes.Add(modItem1.Type);
                ModItem modItem2;
                if (mod1.TryFind("ScoriaDualscythe", out modItem2))
                    scytheTypes.Add(modItem2.Type);
                ModItem modItem3;
                if (mod1.TryFind("ProfanedScythe", out modItem3))
                    scytheTypes.Add(modItem3.Type);
            }
            Mod mod2;
            if (!ModLoader.TryGetMod("ThoriumMod", out mod2))
                return;
            ModItem modItem4;
            if (mod2.TryFind("AquaiteScythe", out modItem4))
                scytheTypes.Add(modItem4.Type);
            ModItem modItem5;
            if (mod2.TryFind("BoneReaper", out modItem5))
                scytheTypes.Add(modItem5.Type);
            ModItem modItem6;
            if (mod2.TryFind("FallingTwilight", out modItem6))
                scytheTypes.Add(modItem6.Type);
            ModItem modItem7;
            if (mod2.TryFind("BloodHarvest", out modItem7))
                scytheTypes.Add(modItem7.Type);
            ModItem modItem8;
            if (mod2.TryFind("HallowedScythe", out modItem8))
                scytheTypes.Add(modItem8.Type);
            ModItem modItem9;
            if (mod2.TryFind("TrueHallowedScythe", out modItem9))
                scytheTypes.Add(modItem9.Type);
            ModItem modItem10;
            if (mod2.TryFind("TitanScythe", out modItem10))
                scytheTypes.Add(modItem10.Type);
            ModItem modItem11;
            if (mod2.TryFind("MorningDew", out modItem11))
                scytheTypes.Add(modItem11.Type);
            ModItem modItem12;
            if (mod2.TryFind("DreadTearer", out modItem12))
                scytheTypes.Add(modItem12.Type);
            ModItem modItem13;
            if (!mod2.TryFind("TheBlackScythe", out modItem13))
                return;
            scytheTypes.Add(modItem13.Type);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (scytheTypes == null || !scytheTypes.Contains(item.type) || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            tooltips.Add(new TooltipLine(Mod, "ExtraInfo", "Gains soul essence rapidly")
            {
                OverrideColor = new Color?(Color.Yellow)
            });
        }
    }
}
