using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CatalystMod;
using CatalystMod.Items;
using CatalystMod.Items.SummonItems;
using CatalystMod.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod("CatalystMod")]
    public class AstralCommunitcatorChanges : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<AstralCommunicator>();
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (!InfernumSaveSystem.InfernumModeEnabled) return base.CanUseItem(item, player);

            if (!WorldDefeats.downedAstrageldon && player.altFunctionUse == 2)
            {
                return false;
            }
            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (!InfernumSaveSystem.InfernumModeEnabled) return base.UseItem(item, player);

            if (!WorldDefeats.downedAstrageldonPhase1 || WorldDefeats.downedAstrageldon) return base.UseItem(item, player);

            if (player.whoAmI == Main.myPlayer && player.HeldItem.ModItem is AstralCommunicator)
            {
                Vector2 vector2 = player.Center + new Vector2(18 * player.direction, -26f);
                SoundStyle soundStyle = CatalystMod.CatalystMod.GetSound("Item/AstrageldonSpawner").WithVolume(1f);
                SoundEngine.PlaySound(soundStyle, new Vector2?(player.Center), null);
                Projectile.NewProjectile(player.GetSource_ItemUse(item, "CatalystMod:AstralCommunicator"), vector2, new Vector2(0.0f, -2f), ModContent.ProjectileType<AstrageldonSpawner>(), 0, -20f, Main.myPlayer, 0.0f, false ? -1f : 0.0f, 0.0f);
            }
            return new bool?(true);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernumSaveSystem.InfernumModeEnabled) return;

            if (WorldDefeats.downedAstrageldon) return;

            for (int i = 0; i < tooltips.Count; ++i)
            {
                if (tooltips[i].Mod == "Terraria")
                {
                    if (tooltips[i].Name == "Tooltip0" && WorldDefeats.downedAstrageldonPhase1)
                    {
                        TooltipLine tooltip = tooltips[i];
                        tooltip.Text = $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AstralCommunicatorInfernum1")}\n{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AstralCommunicatorInfernum2")}";
                    }
                }
            }
        }

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Text == Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.AstralCommunicatorInfernum2"))
            {
                SuperbossRarity.Draw(item, line);
                return false;
            }
            return base.PreDrawTooltipLine(item, line, ref yOffset);
        }
    }
}
