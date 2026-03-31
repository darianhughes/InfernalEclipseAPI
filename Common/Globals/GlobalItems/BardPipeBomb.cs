using ThoriumMod;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class BardPipeBomb : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            if (entity.DamageType == ThoriumDamageBase<BardDamage>.Instance)
            {
                entity.UseSound = new Terraria.Audio.SoundStyle("InfernalEclipseAPI/Assets/Sounds/MetalPipe") { Volume = 10f };
            }
        }
    }
}
