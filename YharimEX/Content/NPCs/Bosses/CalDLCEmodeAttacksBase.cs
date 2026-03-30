
namespace InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses
{
    public abstract class CalDLCEmodeAttacksBase : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public sealed override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return lateInstantiation && entity.type == ModContent.NPCType<YharimEXBoss>();
        }

        public virtual bool ExtraRequirements() { return true; }

        public bool FirstTick = true;
        public virtual void OnFirstTick(NPC npc) { }

        public virtual bool SafePreAI(NPC npc) => base.PreAI(npc);

        public sealed override bool PreAI(NPC npc)
        {
            if (!ExtraRequirements())
            {
                return true;
            }
            if (FirstTick)
            {
                FirstTick = false;

                OnFirstTick(npc);
            }
            return SafePreAI(npc);
        }
        public virtual void SafePostAI(NPC npc) => base.PostAI(npc);
        public sealed override void PostAI(NPC npc)
        {
            if (!ExtraRequirements())
            {
                return;
            }
            SafePostAI(npc);
            return;
        }

        protected static void NetSync(NPC npc, bool onlySendFromServer = true)
        {
            if (onlySendFromServer && Main.netMode != NetmodeID.Server)
                return;

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
        }

    }
}
