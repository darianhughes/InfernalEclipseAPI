using InfernumMode.Content.BossIntroScreens.InfernumScreens;  

namespace InfernalEclipseAPI.Common.InfernumScreens
{
    public class HypnosDraedonScreen : DraedonIntroScreen
    {
        public override bool ShouldBeActive() => NPC.AnyNPCs(ModLoader.GetMod("HypnosMod").Find<ModNPC>("Draedon").Type) && InfernumMode.InfernumMode.CanUseCustomAIs;
    }
}
