using InfernalEclipseAPI.Core.Systems;

namespace InfernalEclipseAPI.Common.Globals.GlobalBuffs
{
    public class InfernalGlobalBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (InfernalCrossmod.SOTS.Loaded)
            {
                Mod sots = InfernalCrossmod.SOTS.Mod;
            }

            if (InfernalCrossmod.NoxusBoss.Loaded)
            {
                if (type == InfernalCrossmod.NoxusBoss.Mod.Find<ModBuff>("StarstrikinglySatiated").Type)
                {
                    player.GetAttackSpeed<MeleeDamageClass>() -= 0.125f;
                    player.moveSpeed -= 0.25f;
                    player.pickSpeed -= 0.05f;
                }
            }

            if (InfernalCrossmod.Thorium.Loaded)
            {
                if (type == InfernalCrossmod.Thorium.Mod.Find<ModBuff>("Bubbled").Type)
                {
                    player.AddBuff(BuffID.Suffocation, 1);
                }
            }
        }
    }
}
