using CalamityMod;
using CalamityMod.Cooldowns;
using SOTS.Buffs.Debuffs;
using ThoriumMod;
using ThoriumMod.Buffs;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Content.Buffs
{
    public class BrimstoneDesperation : ModBuff
    {
        public override string Texture => "CalamityMod/Buffs/DamageOverTime/VulnerabilityHex";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.AddBuff(BuffID.ChaosState, 2);

            player.AddCooldown(PermafrostConcoction.ID, CalamityUtils.SecondsToFrames(180));
            player.AddCooldown(GlobalDodge.ID, CalamityUtils.SecondsToFrames(180));
            player.AddCooldown(CalamityMod.Cooldowns.ChaosState.ID, CalamityUtils.SecondsToFrames(180));
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public static class ThoriumEffectHandler
    {
        public static void DisableThoriumEffects(Player player)
        {
            ThoriumPlayer mp = player.GetThoriumPlayer();

            mp.debuffRevivalExhaustion = true;

            mp.accAbyssalShell = false;
            mp.accFlawlessChrysalis = false;

            player.ClearBuff(ModContent.BuffType<PhylacteryBuff>());
        }
    }

    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public static class SOTSEffectHandler
    {
        public static void DisableSOTSEffects(Player player)
        {
            player.AddBuff(ModContent.BuffType<ChaosState2>(), 2);
        }
    }
}
