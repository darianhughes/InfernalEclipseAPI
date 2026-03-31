using CalamityMod;
using CalamityMod.Cooldowns;
using InfernalEclipseAPI.YharimEX.Core.Systems;

namespace InfernalEclipseAPI.YharimEX.Content.Buffs
{
    public class TyrantDesperationBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.YharimPlayer().YharimDesperation = true;

            player.AddBuff(BuffID.ChaosState, 2);

            player.AddCooldown(PermafrostConcoction.ID, CalamityUtils.SecondsToFrames(180));
            player.AddCooldown(GlobalDodge.ID, CalamityUtils.SecondsToFrames(180));
            player.AddCooldown(CalamityMod.Cooldowns.ChaosState.ID, CalamityUtils.SecondsToFrames(180));
        }
    }
}
