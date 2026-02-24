using System.Collections.Generic;
using ThoriumMod;
using ThoriumMod.Utilities;
using Microsoft.Xna.Framework;
using Terraria.Localization;


namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.OmniSpeakerCraftingTree
{
    [ExtendsFromMod("ThoriumMod")]
    public class OmniSpeakerAccessoryChanges : GlobalItem
    {
        private Mod Ragnarok
        {
            get
            {
                ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok);
                return ragnarok;
            }
        }
        private Mod CalBardHealer
        {
            get
            {
                ModLoader.TryGetMod("CalamityBardHealer", out Mod calbh);
                return calbh;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "RagnarokMod" &&
                item.ModItem.Name == "SigilOfACruelWorld" &&
                CalBardHealer != null)
            {
                ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
                ref StatModifier local = ref player.GetDamage(ThoriumDamageBase<BardDamage>.Instance);
                local -= 0.03f;
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "RagnarokMod" &&
                item.ModItem.Name == "UniversalHeadset" &&
                CalBardHealer != null)
            {
                ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
                ref StatModifier local = ref player.GetDamage(ThoriumDamageBase<BardDamage>.Instance);
                local -= 0.06f;
                player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) -= 2f;
                player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) -= 0.02f;
                thoriumPlayer.inspirationRegenBonus -= 0.02f;
            }

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "CalamityBardHealer" &&
                item.ModItem.Name == "OmniSpeaker" &&
                Ragnarok != null)
            {
                ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
                ref StatModifier local = ref player.GetDamage(ThoriumDamageBase<BardDamage>.Instance);
                local -= 0.07f;
                player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) -= 7f;
                player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) -= 0.07f;
                thoriumPlayer.inspirationRegenBonus += 0.08f;
                thoriumPlayer.bardResourceMax2 += 5;
                thoriumPlayer.accHeadset = true;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Color InfernalRed = Color.Lerp(
                Color.White,
                new Color(255, 80, 0), // Infernal red/orange
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            if (Ragnarok != null & CalBardHealer != null)
            {
                if (item.type == Ragnarok.Find<ModItem>("SigilOfACruelWorld").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains("15%"))
                        {
                            tooltip.Text = tooltip.Text.Replace("15%", "12%");
                        }
                    }
                }

                if (item.type == Ragnarok.Find<ModItem>("UniversalHeadset").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains("20%"))
                        {
                            tooltip.Text = tooltip.Text.Replace("20%", "14%");
                        }
                        if (tooltip.Text.Contains("7%"))
                        {
                            tooltip.Text = tooltip.Text.Replace("7%", "5%");
                        }
                        if (tooltip.Text.Contains("10%"))
                        {
                            tooltip.Text =  tooltip.Text.Replace("10%","8%");
                        }
                    }
                }

                if (item.type == CalBardHealer.Find<ModItem>("OmniSpeaker").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains(Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Omni.OrigTooltip")))
                        {
                            tooltip.Text = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.Omni.Replace");
                            tooltip.OverrideColor = new Color?(InfernalRed);
                        }
                    }

                    tooltips.Add(new TooltipLine(Mod, "MaxInsp", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.OmniIns"))
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });

                    tooltips.Add(new TooltipLine(Mod, "HeadsetInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.OmniEmp"))
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
        }
    }
}
