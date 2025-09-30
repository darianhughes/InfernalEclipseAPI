using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;

namespace InfernalEclipseAPI.Content.NPCs.LittleCat
{
    // This boss shouldn't be seen yet, it's not done. Honestly, I could just commit all of this out, but ¯\_(ツ)_/¯
    // StarlightCat boss?!?!?? :P
    public class LittleCat : ModNPC
    {
        // NPC.AI[0] is the phase
        // NPC.AI[1] is the current attack
        // NPC.AI[2] is a timer
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override string Texture => "Terraria/Images/TownNPCs/Cat_Silver";
        public override string HeadTexture => "Terraria/Images/TownNPCs/NPC_Head_80";
        Player Target => Main.player[NPC.target];
        private string TownCatName;
        private int TownCatVariant;
        private int RitualScale;
        private int DeathTimer;
        private int StartTimer;
        readonly private Queue<int> AttackQueue = new(2);
        private enum Attacks
        {
            // Note for making AIs: ALWAYS. I MEAN ALWAYS, use enumerations. It allows you to change the AI 100x easier, and it's way easier. The attacks obviously won't be called this, but it's a start for when I want to actually do this boss after I finish Infernity or just want a break for a bit. Although, I prob wont on account of this boss will be out of place for a while and also if I had to choose between boss AI related projects, I would probably choose Infernity. I yap a lot, honestly, just delete my yaps if you don't care.

            // Phase 1
            P1Attack1 = 1,
            // Scratch
            P1Attack2,
            // Litter Box
            P1Attack3,
            // 
            P1Attack4,
            P1Attack5,
            P1Attack6,
            P1Attack7,
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
            Main.npcFrameCount[NPC.type] = 28;
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
            NPC.width = 36;
            NPC.height = 30;
            NPC.damage = 1;
            NPC.defense = 0;
            NPC.scale = 2.6f;
            NPC.lifeMax = 50000;
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
        public override bool CheckDead()
        {
            NPC.life = 1;
            NPC.ai[0] = 0;
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC TownCat = Main.npc.FirstOrDefault(n => n.active && n.netID == NPCID.TownCat);
            if (TownCat == null)
            {
                NPC.active = false;
                return;
            }
            TownCatName = TownCat.GivenName;
            TownCatVariant = TownCat.townNpcVariationIndex;
            NPC.Center = TownCat.Center + new Vector2(0, -5);
            NPC.spriteDirection = TownCat.spriteDirection;
            NPC.direction = TownCat.direction != 0 ? TownCat.direction : 1;
            TownCat.active = false;
            AttackQueue.Enqueue(0);
            base.OnSpawn(source);
        }
        public override bool PreAI()
        {
            float LifeRatio = (float)NPC.life / NPC.lifeMax;
            NPC.ai[0] = LifeRatio > 0.6f ? 1 : LifeRatio > 0.3f ? 2 : 3;
            NPC.dontTakeDamage = false;

            if (NPC.life == 1 && DeathTimer++ < 180)
            {
                Vector2 PreviousCenter = NPC.Center;
                NPC.width = (int)(36 * NPC.scale);
                NPC.height = (int)(30 * NPC.scale);
                NPC.ai[0] = 4;
                NPC.scale = 2.6f - DeathTimer / 180f * 1.6f;
                NPC.Center = PreviousCenter;
                NPC.dontTakeDamage = true;
                if (RitualScale > 0)
                {
                    RitualScale--;
                }
            }
            else if (NPC.life == 1 && DeathTimer >= 180)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int TownCatNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.TownCat);
                    if (TownCatNPC >= 0 && TownCatNPC < Main.npc.Length)
                    {
                        Main.npc[TownCatNPC].direction = NPC.direction;
                        Main.npc[TownCatNPC].GivenName = TownCatName;
                        Main.npc[TownCatNPC].townNpcVariationIndex = TownCatVariant;
                    }
                }
                NPC.active = false;
            }
            else if (NPC.life == NPC.lifeMax && StartTimer < 180)
            {
                StartTimer++;
                Vector2 PreviousCenter = NPC.Center;
                NPC.ai[0] = 0;
                NPC.dontTakeDamage = true;
                NPC.scale = 1f + StartTimer / 180f * 1.6f;
                NPC.width = (int)(36 * NPC.scale);
                NPC.height = (int)(30 * NPC.scale);
                NPC.Center = PreviousCenter;
                NPC.position.Y -= StartTimer / 180f * 1.75f;
                if (RitualScale < 180)
                {
                    RitualScale++;
                }
            }
            return true;
        }
        public override void AI()
        {
            Main.NewText("[ Phase= " + NPC.ai[0] + ", Attack= " + NPC.ai[1] + ", Timer= " + NPC.ai[2] + " ]");

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
                case Attacks.P1Attack6:
                    P1Attack6();
                    break;
                case Attacks.P1Attack7:
                    P1Attack7();
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
                default: NPC.ai[1] = (float)Attacks.P1Attack1; break;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawRitual(Main.GameUpdateCount * 0.025f, RitualScale * 0.7f / 180f);
            DrawRitual(Main.GameUpdateCount * -0.0375f, RitualScale * 0.4f / 180f);

            void DrawRitual(float rotation, float scale)
            {
                Main.spriteBatch.Draw(
                    TextureAssets.Projectile[490].Value,
                    NPC.Center - Main.screenPosition,
                    null,
                    new Color(160, 32, 240),
                    rotation,
                    TextureAssets.Projectile[490].Value.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0f
                );

                Main.spriteBatch.Draw(
                    TextureAssets.Extra[34].Value,
                    NPC.Center - Main.screenPosition,
                    null,
                    new Color(160, 32, 240),
                    rotation,
                    TextureAssets.Extra[34].Value.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
            return true;
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
        void P1Attack6()
        {
            ChooseNextAttack((int)NPC.ai[1]);
        }
        void P1Attack7()
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
            AttackQueue.Enqueue(PreviousAttack);
            int PreviousAttack1 = AttackQueue.Dequeue();
            int PreviousAttack2 = AttackQueue.Peek();

            if (NPC.ai[0] == 1)
            {
                do NPC.ai[1] = Main.rand.Next((int)Attacks.P1Attack1, (int)Attacks.P2Attack1);
                while (NPC.ai[1] == PreviousAttack1 || NPC.ai[1] == PreviousAttack2);
            }
            else if (NPC.ai[0] == 2)
            {
                do NPC.ai[1] = Main.rand.Next((int)Attacks.P2Attack1, (int)Attacks.P3Attack1);
                while (NPC.ai[1] == PreviousAttack1 || NPC.ai[1] == PreviousAttack2);
            }
            else if (NPC.ai[0] == 3)
            {
                do NPC.ai[1] = Main.rand.Next((int)Attacks.P3Attack1, (int)(Attacks.P3Attack5 + 1));
                while (NPC.ai[1] == PreviousAttack1 || NPC.ai[1] == PreviousAttack2);
            }
            NPC.netUpdate = true;
        }
        public override void SendExtraAI(BinaryWriter BinaryWriter)
        {
            BinaryWriter.Write(TownCatName);
            BinaryWriter.Write(RitualScale);
            BinaryWriter.Write(DeathTimer);
            BinaryWriter.Write(StartTimer);
            BinaryWriter.Write(TownCatVariant);
            foreach (int attack in AttackQueue)
            {
                BinaryWriter.Write(attack);
            }
        }

        public override void ReceiveExtraAI(BinaryReader BinaryReader)
        {
            TownCatName = BinaryReader.ReadString();
            RitualScale = BinaryReader.ReadInt32();
            DeathTimer = BinaryReader.ReadInt32();
            StartTimer = BinaryReader.ReadInt32();
            TownCatVariant = BinaryReader.ReadInt32();
            AttackQueue.Clear();
            for (int i = 0; i < 2; i++)
            {
                AttackQueue.Enqueue(BinaryReader.ReadInt32());
            }
        }
        #endregion
    }
}