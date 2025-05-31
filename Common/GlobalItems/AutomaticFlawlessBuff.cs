using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items;
using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Summon;
using CalamityMod.CalPlayer;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    [ExtendsFromMod("ThoriumMod")]
    public class AutomaticFlawlessBuff : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            if (!InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
                return;

            if (ModLoader.TryGetMod("CalamityBardHealer", out _) || ModLoader.TryGetMod("RagnarokMod", out _)) 
            {
                // Only for non-consumable Thorium thrower weapons
                if (item.ModItem is ThoriumItem thoriumItem && thoriumItem.isThrowerNon && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
                {
                    if (!item.GetGlobalItem<AutomaticFlawlessBuff>().statBonusesApplied)
                    {
                        item.damage = (int)(item.damage * 1.15f);
                        item.useTime = (int)(item.useTime * 0.9f);
                        item.useAnimation = (int)(item.useAnimation * 0.9f); // keep synced with useTime
                        item.crit += 5;
                        item.shootSpeed *= 1.1f;
                        item.GetGlobalItem<AutomaticFlawlessBuff>().statBonusesApplied = true;
                    }
                }
            }
        }

        private bool statBonusesApplied = false;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Color InfernalRed = Color.Lerp(
                Color.White,
                new Color(255, 80, 0), // Infernal red/orange
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );
            Color InfernalRedStat = Color.Lerp(
                Color.Green,
                new Color(255, 80, 0), // Infernal red/orange
                (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)
            );
            if (ModLoader.TryGetMod("CalamityBardHealer", out _) || ModLoader.TryGetMod("RagnarokMod", out _))
            {
                if (item.ModItem is ThoriumItem thoriumItem && thoriumItem.isThrowerNon && !item.consumable)
                {
                    string info = "[IEoR]: Automatic Flawless Buff:";
                    string damagemult = "+15% damage";
                    string speedmult = "+9% speed";
                    string critmult = "+5 critical strike chance";
                    string shootspeedMult = "+10% velocity";
                    string stealthDamageMutl = "+15% steath strike damage";

                    tooltips.Add(new TooltipLine(Mod, "FlawlessInfo", info)
                    {
                        OverrideColor = new Color?(InfernalRed)
                    });
                    tooltips.Add(new TooltipLine(Mod, "FlawlessInfoDamage", damagemult)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                    tooltips.Add(new TooltipLine(Mod, "FlawlessInfoSpeed", speedmult)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                    tooltips.Add(new TooltipLine(Mod, "FlawlessInfoCrit", critmult)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                    tooltips.Add(new TooltipLine(Mod, "FlawlessInfoVelocity", shootspeedMult)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                    tooltips.Add(new TooltipLine(Mod, "FlawlessInfoStealth", stealthDamageMutl)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                }
                else if (item.ModItem != null && item.ModItem.Mod?.Name == "ThoriumMod" && item.consumable)
                {
                    string stealthDamageMultCons = "+75% stealth strike damage";
                    string stealthsppedMullt = "+75% stealth strike velocity";

                    tooltips.Add(new TooltipLine(Mod, "stealthBuffDamage", stealthDamageMultCons)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                    tooltips.Add(new TooltipLine(Mod, "stealthBuffSpeed", stealthsppedMullt)
                    {
                        OverrideColor = new Color?(InfernalRedStat)
                    });
                }
            }
        }

        //Provided by Wardrobe Hummus
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (ModLoader.TryGetMod("WHummusMultiModBalancing", out _)) return;

            if (item.ModItem != null && item.ModItem.Mod?.Name == "ThoriumMod" && item.consumable)
            {
                var CalPlayer = player.GetModPlayer<CalamityPlayer>();
                if (CalPlayer.StealthStrikeAvailable())
                {
                    velocity *= 1.75f;
                }
            }
        }
    }

}
