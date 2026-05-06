using InfernalEclipseAPI.Content.Projectiles;
using InfernalEclipseAPI.Core.Players;
using SOTS;
using SOTS.Buffs;
using SOTS.Buffs.Mount;
using SOTS.Void;

namespace InfernalEclipseAPI.Common.Globals.GlobalBuffs
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class SOTSGlobalBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == ModContent.BuffType<GoodVibes>())
            {
                if (!(Math.Abs(player.velocity.X) >= 0.10000000149011612 || (double)Math.Abs(player.velocity.Y) >= 0.10000000149011612))
                {
                    player.SOTSPlayer().attackSpeedMod -= 0.10f;
                    player.lifeRegen -= 4;
                }
            }

            if (type == ModContent.BuffType<Nightmare>())
            {
                player.SOTSPlayer().CritNightmare = false;
                player.GetModPlayer<InfernalPlayer>().CritNightmare = true;
            }

            if (type == ModContent.BuffType<SpiritSurfer>())
            {
                VoidPlayer.ModPlayer(player).flatVoidRegen -= 30f;
            }
        }
    }
}
