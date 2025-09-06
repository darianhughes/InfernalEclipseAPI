using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.Tooltips
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
                //if (mod1.TryFind("ScoriaDualscythe", out modItem2))
                //    scytheTypes.Add(modItem2.Type);
                //ModItem modItem3;
                //if (mod1.TryFind("ProfanedScythe", out modItem3))
                //    scytheTypes.Add(modItem3.Type);
            }
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
            {
                if (thoriumMod.TryFind("AquaiteScythe", out ModItem scythe4))
                    scytheTypes.Add(scythe4.Type);
                if (thoriumMod.TryFind("MoltenThresher", out ModItem scythe14))
                    scytheTypes.Add(scythe14.Type);
                if (thoriumMod.TryFind("BatScythe", out ModItem scythe15))
                    scytheTypes.Add(scythe15.Type);
                if (thoriumMod.TryFind("BoneReaper", out ModItem scythe5))
                    scytheTypes.Add(scythe5.Type);
                if (thoriumMod.TryFind("FallingTwilight", out ModItem scythe6))
                    scytheTypes.Add(scythe6.Type);
                if (thoriumMod.TryFind("BloodHarvest", out ModItem scythe7))
                    scytheTypes.Add(scythe7.Type);

                if (thoriumMod.TryFind("HallowedScythe", out ModItem scythe8))
                    scytheTypes.Add(scythe8.Type);
                if (thoriumMod.TryFind("TrueHallowedScythe", out ModItem scythe9))
                    scytheTypes.Add(scythe9.Type);
                if (thoriumMod.TryFind("TitanScythe", out ModItem scythe10))
                    scytheTypes.Add(scythe10.Type);
                if (thoriumMod.TryFind("MorningDew", out ModItem scythe11))
                    scytheTypes.Add(scythe11.Type);
                if (thoriumMod.TryFind("DreadTearer", out ModItem scythe12))
                    scytheTypes.Add(scythe12.Type);
                //if (thoriumMod.TryFind("TheBlackScythe", out ModItem scythe13))
                //    scytheTypes.Add(scythe13.Type);
                if (thoriumMod.TryFind("LustrousBaton", out ModItem scythe16))
                    scytheTypes.Add(scythe16.Type);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (scytheTypes == null || !scytheTypes.Contains(item.type) || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            tooltips.Add(new TooltipLine(Mod, "ExtraInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.SoulEssence"))
            {
                OverrideColor = new Color?(Color.Yellow)
            });
        }
    }
}
