namespace InfernalEclipseAPI.Content.Buffs
{
    [ExtendsFromMod("ThoriumMod")]
    public class BrokenOath : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // -10 bonus healing while debuffed
            var thor = player.GetModPlayer<ThoriumMod.ThoriumPlayer>();
            if (thor != null)
            {
                thor.healBonus -= 10;

                if (thor.healBonus < 0)
                    thor.healBonus = 0;
            }
        }
    }
}
