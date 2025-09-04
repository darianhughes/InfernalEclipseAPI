using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;
using Terraria.GameContent.Events;
using Terraria.ID;
using InfernalEclipseAPI.Content.Buffs;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Buffs.Healer;

namespace InfernalEclipseAPI.Core.Players
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumPlayerEdits : ModPlayer
    {
        public const int PenaltyDuration = 60 * 10; // 10s
        public int switchToHealerPenaltyTimer;

        public override void ResetEffects()
        {
            if (switchToHealerPenaltyTimer > 0)
                switchToHealerPenaltyTimer--;
        }

        // === Helpers ===
        private static bool IsHealerDamage(Item item) => item.CountsAsClass<HealerDamage>();
        private static bool IsHealerDamage(Projectile proj) => proj.CountsAsClass<HealerDamage>();

        // Treat these custom tools as "Healer usage"
        private static bool IsHealerToolOrHybrid(Item item)
        {
            var mi = item?.ModItem;
            if (mi is null) return false;

            // If you can reference the types directly, prefer:
            // return mi is HealerTool || mi is HealerToolDamageHybrid;

            // Fallback by name to avoid hard refs if needed:
            string typeName = mi.GetType().Name;
            return typeName == "HealerTool" || typeName == "HealerToolDamageHybrid";
        }

        private static bool IsHealerWeaponOrTool(Item item) =>
            IsHealerDamage(item) || IsHealerToolOrHybrid(item);

        private static bool IsCombatWeapon(Item item)
            => item is not null
               && item.type != ItemID.None
               && item.useStyle != ItemUseStyleID.None
               && !item.accessory
               && item.ammo == AmmoID.None
               && item.pick <= 0 && item.axe <= 0 && item.hammer <= 0;

        // === Item hits (melee/direct) ===
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!InfernalConfig.Instance.NerfThoriumMulticlass) return;

            if (Player.HasBuff<BrokenOath>())
            {
                modifiers.FinalDamage *= 0.5f;
            }

            if (IsCombatWeapon(item) && !IsHealerWeaponOrTool(item))
            {
                // Any non-healer weapon hit starts/refreshes the "recently non-healer" window
                switchToHealerPenaltyTimer = PenaltyDuration;
                return;
            }

            if (IsHealerWeaponOrTool(item) && switchToHealerPenaltyTimer > 0)
            {
                Player.AddBuff(ModContent.BuffType<BrokenOath>(), switchToHealerPenaltyTimer);
            }
        }

        // === Projectile hits ===
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!InfernalConfig.Instance.NerfThoriumMulticlass) return;

            if (Player.HasBuff<BrokenOath>())
            {
                modifiers.FinalDamage *= 0.5f;
            }

            if (proj.owner != Player.whoAmI)
                return;

            bool healerAttack = IsHealerDamage(proj);

            // If projectile isn't flagged healer, but we're actively using a HealerTool/Hybrid, treat it as healer usage too.
            if (!healerAttack)
            {
                var held = Player.HeldItem;
                if (IsHealerWeaponOrTool(held))
                    healerAttack = true;
            }

            if (!healerAttack)
            {
                // Non-healer projectile => refresh window
                switchToHealerPenaltyTimer = PenaltyDuration;
                return;
            }

            // Healer projectile while window active => penalty
            if (switchToHealerPenaltyTimer > 0)
            {
                Player.AddBuff(ModContent.BuffType<BrokenOath>(), switchToHealerPenaltyTimer);
            }
        }
    }
}
