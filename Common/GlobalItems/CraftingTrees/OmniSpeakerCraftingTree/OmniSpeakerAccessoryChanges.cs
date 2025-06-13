using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using Microsoft.Xna.Framework;
using System.Security.Policy;


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
                item.ModItem.Name == "UniversalHeadset" &&
                CalBardHealer != null)
            {
                ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
                ref StatModifier local = ref player.GetDamage(ThoriumDamageBase<BardDamage>.Instance);
                local -= 0.05f;
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
                player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) -= 0.7f;
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
                if (item.type == Ragnarok.Find<ModItem>("UniversalHeadset").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains("Increases symphonic damage by 20%"))
                        {
                            tooltip.Text = "Increases symphonic damage by 15%";
                        }
                        if (tooltip.Text.Contains("Increases symphonic playing speed by 10%"))
                        {
                            tooltip.Text = "Increases symphonic playing speed by 8%";
                        }
                        if (tooltip.Text.Contains("Increases inspiration regeneration rate by 10%"))
                        {
                            tooltip.Text = "Increases inspiration regeneration rate by 8%";
                        }
                    }
                }

                if (item.type == CalBardHealer.Find<ModItem>("OmniSpeaker").Type)
                {
                    foreach (TooltipLine tooltip in tooltips)
                    {
                        if (tooltip.Text.Contains("15% increased symphonic damage, playing speed, and critical strike chance"))
                        {
                            tooltip.Text = "15% increased symphonic damage\n8% increased playing speed, critical strike chance, and inspiration regeneration rate";
                            tooltip.OverrideColor = new Color?(InfernalRed);
                        }
                    }

                    tooltips.Add(new TooltipLine(Mod, "MaxInsp", "Increases maximum inspiration by 5")
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });

                    tooltips.Add(new TooltipLine(Mod, "HeadsetInfo", "Each unique empowerment you have increases movement speed by 2% and playing speed by 1%")
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                }
            }
        }
    }
}
