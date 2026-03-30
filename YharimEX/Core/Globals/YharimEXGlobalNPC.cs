namespace InfernalEclipseAPI.YharimEX.Core.Globals
{
    public class YharimEXGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public static int yharimEXBoss = -1;
        public static int boss = -1;

        public override bool PreAI(NPC npc)
        {
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead)
                boss = npc.whoAmI;
            if (!LumUtils.AnyBosses())
                boss = -1;

            bool retval = base.PreAI(npc);
            return retval;
        }
    }
}
