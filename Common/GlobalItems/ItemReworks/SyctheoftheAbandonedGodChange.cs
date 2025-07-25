using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    //Provided by Wardrobe Hummus
    public class SyctheoftheAbandonedGodChange : GlobalItem
    {
        private const int BaseDamage = 500;
        private const int ReducedDamage = 166;
        private const string CatalystModName = "CatalystMod";
        private const string ScytheInternalName = "ScytheoftheAbandonedGod";

        public override void UpdateInventory(Item item, Player player)
        {
            if (!TryGetScytheModItem(out ModItem scythe) || item.type != scythe.Type || !InfernalConfig.Instance.CalamityBalanceChanges
                //|| ModLoader.TryGetMod("WHummusMultiModBalancing", out _)
                )
                return;

            int ProgressionDamage;
            if (CalamityMod.DownedBossSystem.downedBoomerDuke) //thanks fabsol for that name i guess lmao
            {
                ProgressionDamage = BaseDamage;
            }
            else if (CalamityMod.DownedBossSystem.downedPolterghast)
            {
                ProgressionDamage = 450;
            }
            else if (CalamityMod.DownedBossSystem.downedProvidence)
            {
                ProgressionDamage = 325;
            }
            else if (NPC.downedMoonlord)
            {
                ProgressionDamage = 215;
            }
            else
            {
                ProgressionDamage = 110;
            }

            //item.damage = player.slotsMinions > 0 ? ReducedDamage : ProgressionDamage;
            item.damage = ProgressionDamage;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!TryGetScytheModItem(out ModItem scythe) || item.type != scythe.Type || !InfernalConfig.Instance.CalamityBalanceChanges
                || ModLoader.TryGetMod("WHummusMultiModBalancing", out _)
                )
                return;

            Color lerpedColor = Color.Lerp(Color.White, new Color(30, 144, 255), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
            Color astraWarningColor = Color.Lerp(Color.White, new Color(110, 42, 259), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));

            string progressionText = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ScytheScale");

            tooltips.Add(new TooltipLine(Mod, "ScytheoftheAbandonedGodDamageInfo", progressionText)
            {
                OverrideColor = lerpedColor
            });

            string tooltipText = Main.LocalPlayer.slotsMinions > 0
                ? Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ScytheSummonOn")
                : Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.ScytheSummon");

            tooltips.Add(new TooltipLine(Mod, "ScytheoftheAbandonedGodInfo", tooltipText)
            {
                OverrideColor = astraWarningColor
            });
        }

        private bool TryGetScytheModItem(out ModItem modItem)
        {
            modItem = null;
            if (!ModLoader.TryGetMod(CatalystModName, out Mod mod))
                return false;

            return mod.TryFind(ScytheInternalName, out modItem);
        }
    }
}
