using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;

namespace InfernalEclipseAPI.Content.NPCs.LittleCat
{
    // This boss shouldn't be seen yet, it's not done. Honestly, I could just commit all of this out, but ¯\_(ツ)_/¯
    // StarlightCat boss?!?!?? :P
    public class LittleCat : ModNPC
    {
        // NPC.AI[0] is the phase
        // NPC.AI[1] is the current attack
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override string HeadTexture => "CalamityMod/Projectiles/InvisibleProj";
        Player Target => Main.player[NPC.target];
        // I'm not good enough for this :D
        // private int AttackBuffer;
        private enum Attacks
        {
            // Note for making AIs: ALWAYS. I MEAN ALWAYS, use enumerations. It allows you to change the AI 100x easier, and it's way easier. The attacks obviously won't be called this, but it's a start for when I want to actually do this boss after I finish Infernity or just want a break for a bit. Although, I prob wont on account of this boss will be out of place for a while and also if I had to choose between boss AI related projects, I would probably choose Infernity. I yap a lot, honestly, just delete my yaps if you don't care.

            // Phase 1
            P1Attack1 = 1,
            P1Attack2,
            P1Attack3,
            P1Attack4,
            P1Attack5,
            // Phase 2
            P2Attack1,
            P2Attack2,
            P2Attack3,
            P2Attack4,
            P2Attack5,
            // Phase 3
            P3Attack1,
            P3Attack2,
            P3Attack3,
            P3Attack4,
            P3Attack5,
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement($"Mods.InfernalEclipseAPI.NPCs.LittleCat.Bestiary")
            ]);
        }
        public override void SetDefaults()
        {
            NPC.width = 120;
            NPC.height = 120;
            NPC.damage = 1;
            NPC.defense = 1;
            NPC.lifeMax = 1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 6f;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.netAlways = true;
            NPC.BossBar = ModContent.GetInstance<LittleCatBossBar>();
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LittleCatTheme");
            SceneEffectPriority = SceneEffectPriority.BossHigh;
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC.active = false;
            Main.NewText("You aren't supposed to be seeing this. If you are, tell Ropro0923", Color.MediumPurple);
            SpawnRune();
        }
        public override bool PreAI()
        {
            NPC.ai[0] = (NPC.life / NPC.lifeMax) > 0.6 ? 1 : (NPC.life / NPC.lifeMax) > 0.3 ? 2 : 3;
            return true;
        }
        public override void AI()
        {
            // Debug
            Main.NewText("[ " + NPC.ai[0] + ", " + NPC.ai[1] + " ]");

            switch ((Attacks)(int)NPC.ai[1])
            {
                case Attacks.P1Attack1:
                    P1Attack1();
                    break;
                case Attacks.P1Attack2:
                    P1Attack2();
                    break;
                case Attacks.P1Attack3:
                    P1Attack3();
                    break;
                case Attacks.P1Attack4:
                    P1Attack4();
                    break;
                case Attacks.P1Attack5:
                    P1Attack5();
                    break;
                case Attacks.P2Attack1:
                    P2Attack1();
                    break;
                case Attacks.P2Attack2:
                    P2Attack2();
                    break;
                case Attacks.P2Attack3:
                    P2Attack3();
                    break;
                case Attacks.P2Attack4:
                    P2Attack4();
                    break;
                case Attacks.P2Attack5:
                    P2Attack5();
                    break;
                case Attacks.P3Attack1:
                    P3Attack1();
                    break;
                case Attacks.P3Attack2:
                    P3Attack2();
                    break;
                case Attacks.P3Attack3:
                    P3Attack3();
                    break;
                case Attacks.P3Attack4:
                    P3Attack4();
                    break;
                case Attacks.P3Attack5:
                    P3Attack5();
                    break;
            }
        }
        #region Phase 1 Attacks
        void P1Attack1()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P1Attack2()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P1Attack3()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P1Attack4()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P1Attack5()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        #endregion
        #region Phase 2 Attacks
        void P2Attack1()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P2Attack2()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P2Attack3()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P2Attack4()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P2Attack5()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        #endregion
        #region Phase 3 Attacks
        void P3Attack1()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P3Attack2()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P3Attack3()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P3Attack4()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P3Attack5()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        #endregion
        #region Helper Methods
        void ChooseNextAttack(int PreviousAttack)
        {
            if (NPC.ai[0] == 1)
            {
                do NPC.ai[1] = Main.rand.Next((int)Attacks.P1Attack1, (int)Attacks.P2Attack1);
                while (NPC.ai[1] == PreviousAttack);
            }
            else if (NPC.ai[0] == 2)
            {
                do NPC.ai[1] = Main.rand.Next((int)Attacks.P2Attack1, (int)Attacks.P3Attack1);
                while (NPC.ai[1] == PreviousAttack);
            }
            else if (NPC.ai[0] == 3)
            {
                do NPC.ai[1] = Main.rand.Next((int)Attacks.P3Attack1, (int)Attacks.P3Attack5) + 1;
                while (NPC.ai[1] == PreviousAttack);
            }
            NPC.netUpdate = true;
        }
        void SpawnRune()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            Projectile projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DemonicRune>(), 0, 0, NPC.whoAmI);
            projectile.ai[0] = NPC.whoAmI;
        }
        #endregion
    }
}