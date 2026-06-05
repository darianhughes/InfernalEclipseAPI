namespace InfernalEclipseAPI.Content.Buffs.Tag
{ 
    public class PrismBreakTag : ModBuff
    {
        public override string Texture => "InfernalEclipseAPI/Assets/Textures/Empty";

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }
}
