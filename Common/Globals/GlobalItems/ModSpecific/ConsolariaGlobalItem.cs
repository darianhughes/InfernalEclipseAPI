using CalamityMod;
using InfernalEclipseAPI.Core.Systems;
using Consolaria.Content.Crossmod.Thorium.Armor;
using static Terraria.ModLoader.ModContent;
using Consolaria.Content.Items.Misc;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Core.Utils;
using Terraria.Localization;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    [JITWhenModsEnabled(InfernalCrossmod.Consolaria.Name)]
    [ExtendsFromMod(InfernalCrossmod.Consolaria.Name)]
    public class ConsolariaGlobalItem : GlobalItem
    {
        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            if (item.type == ItemType<HornoPlenty>() && !ModLoader.HasMod("HornScale"))
            {
                item.healLife = !DownedBossSystem.downedDoG ? (!DownedBossSystem.downedProvidence ? (!NPC.downedMoonlord ? (!Main.hardMode ? 100 : 150) : 200) : 250) : 300;
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ItemType<HornoPlenty>() && !ModLoader.HasMod("HornScale"))
            {
                item.healLife = !DownedBossSystem.downedDoG ? (!DownedBossSystem.downedProvidence ? (!NPC.downedMoonlord ? (!Main.hardMode ? 100 : 150) : 200) : 250) : 300;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (InfernalConfig.Instance.ConsolariaBalanceChanges && InfernalCrossmod.Thorium.Loaded)
            {
                if (item.type == ItemType<ViperHelmet>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                }
                if (item.type == ItemType<ViperChestplate>())
                {
                    player.GetAttackSpeed(DamageClass.Throwing) -= 0.15f;
                }
                if (item.type == ItemType<ViperLegs>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                    player.GetCritChance(DamageClass.Throwing) -= 10;
                }

                if (item.type == ItemType<OldViperHelmet>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                }
                if (item.type == ItemType<OldViperChestplate>())
                {
                    player.GetAttackSpeed(DamageClass.Throwing) -= 0.15f;
                }
                if (item.type == ItemType<OldViperLegs>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                    player.GetCritChance(DamageClass.Throwing) -= 10;
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Color InfernalRed = Color.Lerp(
                Color.White,
                new Color(255, 80, 0), // Infernal red/orange
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );

            if (item.type == ItemType<HornoPlenty>())
            {
                InfernalUtilities.AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.HornScale"), InfernalRed);
            }
        }
    }
}
