using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.core.Players
{
    public class DungeonCursePlayer : ModPlayer // maybe rename??? -arkangel 
    {
        public override void PostUpdateMiscEffects()
        {
            // Condition 1: In the Dungeon before Skeletron is defeated
            bool unclearedDungeon = Player.ZoneDungeon && !NPC.downedBoss3 && !Main.hardMode;

            // Condition 2: In the Jungle Temple before Plantera is defeated
            bool unclearedTemple = Player.ZoneLihzhardTemple && !NPC.downedPlantBoss;

            // Apply Creative Shock if in either restricted area
            if (unclearedDungeon || unclearedTemple)
            {
                Player.AddBuff(BuffID.NoBuilding, 2);
                Player.noBuilding = true;
            }
        }
    }
} //  i dont think this could be better. -Arkangel