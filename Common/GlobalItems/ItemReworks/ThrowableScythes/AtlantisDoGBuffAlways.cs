using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks.ThrowableScythes
{
    public class AtlantisDoGBuffAlways : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<Atlantis>();
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            // Apply the GFB multiplier even outside Zenith. Avoid double-applying in Zenith.
            if (!Main.zenithWorld && DownedBossSystem.downedDoG)
            {
                damage *= 348f / 86f; // same ratio used by Atlantis in GFB
            }
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            // Make the 2.5x use speed active outside Zenith too. (Don’t stack in Zenith.)
            if (!Main.zenithWorld && DownedBossSystem.downedDoG)
                return 2.5f;

            return 1f;
        }

        public void FullTooltipOveride(List<TooltipLine> tooltips, string stealthTooltip)
        {
            for (int index = 0; index < tooltips.Count; ++index)
            {
                if (tooltips[index].Mod == "Terraria")
                {
                    if (tooltips[index].Name == "Tooltip0")
                    {
                        TooltipLine tooltip = tooltips[index];
                        tooltip.Text = $"{stealthTooltip}";
                    }
                    else if (tooltips[index].Name.Contains("Tooltip"))
                    {
                        tooltips[index].Hide();
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.zenithWorld)
                return; // Let Calamity handle Zenith worlds to avoid duplication

            string line = DownedBossSystem.downedDoG
                ? Language.GetTextValue("Mods.CalamityMod.Items.Weapons.Magic.Atlantis.TooltipGFBDoG")
                : Language.GetTextValue("Mods.CalamityMod.Items.Weapons.Magic.Atlantis.TooltipGFB");

            for (int i = 0; i < tooltips.Count; i++)
            {
                FullTooltipOveride(tooltips, line);
            }
        }
    }
}
