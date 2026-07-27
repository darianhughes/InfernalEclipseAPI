using InfernumMode.Content.BossIntroScreens.InfernumScreens;  

namespace InfernalEclipseAPI.Common.InfernumScreens
{
    [JITWhenModsEnabled("HypnosMod")]
    [ExtendsFromMod("HypnosMod")]
    public class HypnosDraedonScreen : DraedonIntroScreen
    {
        public override bool ShouldBeActive()
        {
            if (NPC.AnyNPCs(ModLoader.GetMod("HypnosMod").Find<ModNPC>("Draedon").Type))
                return InfernumMode.InfernumMode.CanUseCustomAIs;

            return false;
        }
    }
}
