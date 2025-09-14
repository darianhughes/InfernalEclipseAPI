using ThoriumMod;
using InfernalEclipseAPI.Content.Buffs;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using CalamityMod.CalPlayer;
using Terraria.GameContent.Events;

namespace InfernalEclipseAPI.Core.Players.ThoriumMulticlassNerf
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumMulticlassPlayerNerfs : ModPlayer
    {
        //HEALER MULTICLASS NERF
        public const int PenaltyDuration = 60 * 10; // 10s
        public int switchToHealerPenaltyTimer;

        //BARD MULTICLASS NERF
        public const int WindowTicks = 60 * 5; // 5 seconds @ 60 FPS
        internal int lastBardUseTick = int.MinValue / 2;

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

            string typeName = mi.GetType().Name;
            return typeName == "HealerTool" || typeName == "HealerToolDamageHybrid";
        }
         
        private static bool IsHealerWeaponOrTool(Item item) =>
            IsHealerDamage(item) || IsHealerToolOrHybrid(item);

        private bool HasAllGemTech(Player p)
        {
            var cal = p.GetModPlayer<CalamityPlayer>();
            return cal.GemTechSet &&
                   cal.GemTechState.IsYellowGemActive &&
                   cal.GemTechState.IsGreenGemActive &&
                   cal.GemTechState.IsPurpleGemActive &&
                   cal.GemTechState.IsBlueGemActive &&
                   cal.GemTechState.IsRedGemActive &&
                   cal.GemTechState.IsPinkGemActive;
        }

        private static bool IsExcluded(Item item) =>
            item.CountsAsClass<LegendaryMelee>() || item.CountsAsClass<LegendaryRanged>() ||
            item.CountsAsClass<MythicMelee>() || item.CountsAsClass<MythicMagic>();

        private static bool IsExcluded(Projectile proj) =>
            proj.CountsAsClass<LegendaryMelee>() || proj.CountsAsClass<LegendaryRanged>() ||
            proj.CountsAsClass<MythicMelee>() || proj.CountsAsClass<MythicMagic>();

        private bool ShouldIgnoreContext() => HasAllGemTech(Player) || DD2Event.Ongoing;

        private bool IsCombatWeapon(Item item) =>
            item is not null &&
            item.type != ItemID.None &&
            item.useStyle != ItemUseStyleID.None &&
            !item.accessory &&
            item.ammo == AmmoID.None &&
            item.pick <= 0 && item.axe <= 0 && item.hammer <= 0 &&
            !IsExcluded(item) &&
            !ShouldIgnoreContext();
        public bool InWindow
        {
            get
            {
                int now = (int)Main.GameUpdateCount;
                return now - lastBardUseTick <= WindowTicks;
            }
        }

        public void MarkBardUse() => lastBardUseTick = (int)Main.GameUpdateCount;

        public override void OnEnterWorld() => lastBardUseTick = int.MinValue / 2;

        // === Item hits (melee/direct) ===
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!InfernalConfig.Instance.NerfThoriumMulticlass) return;

            if (IsExcluded(item) || ShouldIgnoreContext())
            {
                switchToHealerPenaltyTimer = 0;
                return;
            }

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
            if (proj.owner != Player.whoAmI) return;

            // Do NOT let Legendary/Mythic/GemTech/OOA interact with the penalty window at all.
            if (IsExcluded(proj) || ShouldIgnoreContext())
            {
                switchToHealerPenaltyTimer = 0;
                return;
            }

            if (Player.HasBuff<BrokenOath>())
                modifiers.FinalDamage *= 0.5f;

            bool healerAttack = IsHealerDamage(proj);

            // If the projectile itself isn't tagged healer, but the held item is a healer tool/hybrid,
            // treat it as healer usage (common for tools spawning neutral projectiles).
            if (!healerAttack && IsHealerWeaponOrTool(Player.HeldItem))
                healerAttack = true;

            if (!healerAttack)
            {
                // Non-healer projectile -> start/refresh window (since not excluded)
                switchToHealerPenaltyTimer = PenaltyDuration;
                return;
            }

            // Healer projectile during active window -> apply debuff
            if (switchToHealerPenaltyTimer > 0)
                Player.AddBuff(ModContent.BuffType<BrokenOath>(), switchToHealerPenaltyTimer);
        }

        public override void PostUpdateMiscEffects()
        {
            InfernalPlayer infernalPlayer = Player.GetModPlayer<InfernalPlayer>();
            if (infernalPlayer.soltanBullying)
            {
                float emptySummonSlots = Player.maxMinions - Player.slotsMinions;
                ref StatModifier melee = ref Player.GetDamage(ThoriumDamageBase<BardDamage>.Instance);
                melee += (float)(0.02 * emptySummonSlots);
                ref StatModifier ranged = ref Player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance);
                ranged += (float)(0.02 * emptySummonSlots);
            }
        }
    }
}
