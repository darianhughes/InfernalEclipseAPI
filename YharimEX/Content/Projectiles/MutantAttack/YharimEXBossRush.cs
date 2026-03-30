using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using YharimEX.Core.Globals;
using YharimEX.Core.Systems;
using YharimEX.Content.Projectiles.FargoProjectile;
using InfernalEclipseAPI.YharimEX.Content.NPCs.Bosses;

namespace YharimEX.Content.Projectiles
{
    public class YharimEXBossRush : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_454";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mutant Seal");
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            if (YharimEXCrossmodSystem.FargowiltasSouls.Loaded)
            {
                SetupFargoProjectile SetupFargoProjectile = Projectile.GetGlobalProjectile<SetupFargoProjectile>();
                SetupFargoProjectile.TimeFreezeImmune = true;
            }
        }

        public override void AI()
        {
            NPC npc = YharimEXGlobalUtilities.NPCExists(Projectile.ai[0], ModContent.NPCType<YharimEXBoss>());
            if (npc == null)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = npc.Center;
            Projectile.timeLeft = 2;

            if (--Projectile.ai[1] < 0)
            {
                Projectile.ai[1] = 180;
                Projectile.netUpdate = true;
                switch ((int)Projectile.localAI[0]++)
                {
                    case 0:
                        NPC.SpawnOnPlayer(npc.target, NPCID.EyeofCthulhu);
                        if (Main.dayTime)
                        {
                            Main.dayTime = false;
                            Main.time = 0;
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.WorldData); //sync world
                        }
                        break;

                    case 1:
                        NPC.SpawnOnPlayer(npc.target, NPCID.EaterofWorldsHead);
                        NPC.SpawnOnPlayer(npc.target, NPCID.BrainofCthulhu);
                        break;

                    case 2:
                        NPC.SpawnOnPlayer(npc.target, NPCID.QueenBee);
                        break;

                    case 3:
                        ManualSpawn(npc, NPCID.SkeletronHead);
                        if (Main.dayTime)
                        {
                            Main.dayTime = false;
                            Main.time = 0;
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.WorldData); //sync world
                        }
                        break;

                    case 4:
                        NPC.SpawnOnPlayer(npc.target, NPCID.Retinazer);
                        NPC.SpawnOnPlayer(npc.target, NPCID.Spazmatism);
                        if (Main.dayTime)
                        {
                            Main.dayTime = false;
                            Main.time = 0;
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.WorldData); //sync world
                        }
                        break;

                    case 5:
                        ManualSpawn(npc, NPCID.SkeletronPrime);
                        if (Main.dayTime)
                        {
                            Main.dayTime = false;
                            Main.time = 0;
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.WorldData); //sync world
                        }
                        break;

                    case 6:
                        NPC.SpawnOnPlayer(npc.target, NPCID.Plantera);
                        break;

                    case 7:
                        ManualSpawn(npc, NPCID.Golem);
                        break;

                    case 8:
                        ManualSpawn(npc, NPCID.DD2Betsy);
                        break;

                    case 9:
                        ManualSpawn(npc, NPCID.DukeFishron);
                        break;

                    case 10:
                        ManualSpawn(npc, NPCID.MoonLordCore);
                        break;

                    default:
                        if (!Main.dayTime)
                        {
                            Main.dayTime = true;
                            Main.time = 27000;
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.WorldData); //sync world
                        }
                        Projectile.Kill();
                        break;
                }
            }
        }

        private void ManualSpawn(NPC npc, int type)
        {
            if (YharimEXGlobalUtilities.HostCheck)
            {
                int n = YharimEXGlobalUtilities.NewNPCEasy(Terraria.Entity.InheritSource(Projectile), npc.Center, type);
                if (n != Main.maxNPCs)
                    YharimEXGlobalUtilities.PrintLocalization("Announcement.HasAwoken", new Color(175, 75, 255), Language.GetTextValue($"Mods.{Mod.Name}.NPCs.YharimEXBoss.DisplayName"));
            }
        }
    }
}