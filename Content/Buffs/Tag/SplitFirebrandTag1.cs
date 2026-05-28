namespace InfernalEclipseAPI.Content.Buffs.Tag
{
    public class SplitFirebrandTag1 : ModBuff
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }
}
