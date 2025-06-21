using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Terraria.ID;
using InfernalEclipseAPI.Core.DamageClasses.MergedRogueClass;

namespace InfernalEclipseAPI.Content.ThoriumStealthStrikes
{
    [ExtendsFromMod("ThoriumMod")]
    public class ConsumableThrowerBuff : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            if (ModLoader.TryGetMod("CalamityBardHealer", out _) || ModLoader.TryGetMod("RagnarokMod", out _))
            {
                return true;
            }
            return false;
        }

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);

            string[] ignoreStealthBonusItemNames =
            {
                "ClockWorkBomb",
                "SoulBomb",
                "CaptainsPoniard",
                "SoftServeSunderer"
            };
            
            foreach (string itemName in ignoreStealthBonusItemNames)
            {
                if (entity.type == thorium.Find<ModItem>(itemName).Type) return false;
            }

            return base.AppliesToEntity(entity, lateInstantiation);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
            {
                damage *= 1.15f;
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
            {
                return 0.9f;
            }
            return base.UseTimeMultiplier(item, player);
        }

        public override float UseAnimationMultiplier(Item item, Player player)
        {
            if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
            {
                return 0.9f;
            }
            return base.UseAnimationMultiplier(item, player);
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
            {
                crit += 5;
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
            {
                return base.Shoot(item, player, source, position, velocity * 1.1f, type, damage, knockback);
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

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
                if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable)
                {
                    string info = "[IEoR]: Automatic Flawless Buff:";
                    string damagemult = "+15% damage";
                    string speedmult = "+9% speed";
                    string critmult = "+5% critical strike chance";
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
                else if (item.ModItem != null && item.ModItem.Mod?.Name == "ThoriumMod" && item.consumable && (item.DamageType == ModContent.GetInstance<RogueDamageClass>() || item.DamageType == ModContent.GetInstance<MergedThrowerRogue>()))
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
            if (item.ModItem is ThoriumItem thoriumItem && item.DamageType == ModContent.GetInstance<RogueDamageClass>() && !item.consumable && InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
            {
                velocity *= 1.1f;
            }

            if (ModLoader.TryGetMod("WHummusMultiModBalancing", out _)) return;

            if (item.ModItem != null && item.ModItem.Mod?.Name == "ThoriumMod" && item.consumable && (item.DamageType == ModContent.GetInstance<RogueDamageClass>() || item.DamageType == ModContent.GetInstance<MergedThrowerRogue>()))
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
