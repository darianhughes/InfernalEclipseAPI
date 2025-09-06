using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.ModSceneEffects.CameraEffects
{
    public class DreamEaterCameraEffects : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        private bool _finaleFocusStarted;
        private int introDuration;

        // Resolve Dream Eater's type without compile-time Thorium reference
        private static int DreamEaterType
        {
            get
            {
                var thorium = ModLoader.GetMod("ThoriumMod");
                return thorium?.Find<ModNPC>("DreamEater")?.Type ?? -1;
            }
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("ThoriumMod", out _);
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
            => entity.type == DreamEaterType;

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (Main.netMode == NetmodeID.Server) return;
            const int IntroDuration = 100;   // Thorium: Intro_Duration = 300
            //introDuration = IntroDuration;
            DreamEaterCameraSystem.StartFocus(npc.whoAmI, IntroDuration);
        }

        public override void AI(NPC npc)
        {
            if (Main.netMode == NetmodeID.Server) return;

            // Thorium sets finale timer on ai[3] (> 0 == FinaleOngoing)
            if (!_finaleFocusStarted && npc.ai[3] > 0f)
            {
                _finaleFocusStarted = true;
                const int FinaleDuration = 600; // Thorium: Finale_Duration = 600
                DreamEaterCameraSystem.StartFocus(npc.whoAmI, FinaleDuration, smoothStart: true, lerpOverride: 0.18f);
            }
        }
    }
}
