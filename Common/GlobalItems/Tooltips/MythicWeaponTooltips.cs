using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using YouBoss.Content.Items.ItemReworks;
using CalamityMod.Items.Weapons.DraedonsArsenal;

namespace InfernalEclipseAPI.Common.GlobalItems.Tooltips
{
    public class MythicWeaponTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
            if (index != -1)
            {
                bool canAddTooltip = false;
                string importantName = "";
                if (item.type == ModContent.ItemType<Myrindael>())
                {
                    canAddTooltip = true;
                    importantName = "Argus'";
                }

                if (ModLoader.TryGetMod("YouBoss", out Mod you))
                {
                    if (item.type == you.Find<ModItem>("FirstFractal").Type)
                    {
                        canAddTooltip = true;
                        importantName = "Your";
                    }
                }

                if (item.type == ModContent.ItemType<PulseRifle>())
                {
                    canAddTooltip = true;
                    importantName = "Draedon's";
                }

                if (canAddTooltip)
                {
                    tooltips.Insert(index + 1, new TooltipLine(Mod, "SignatureWeapon", $"-{importantName} Signature Weapon-")
                    {
                        OverrideColor = Color.Cyan
                    });
                }
            }
        }
    }
}
