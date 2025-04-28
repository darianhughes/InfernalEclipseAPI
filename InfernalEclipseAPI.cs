using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content.Sources;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.GreatSandShark;
using GreatSandSharkNPC = CalamityMod.NPCs.GreatSandShark.GreatSandShark;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using CalamityMod.Sounds;
using CalamityMod.NPCs.SupremeCalamitas;
using InfernumMode.Content.BossIntroScreens.InfernumScreens;
using CalamityMod;
using InfernumActive = InfernalEclipseAPI.Content.DifficultyOverrides.hellActive;
using CalamityMod.Events;
using static CalamityMod.Events.BossRushEvent;
using static InfernumMode.Core.GlobalInstances.Systems.BossRushChangesSystem;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.Enums;
using CalamityMod.Systems;
using InfernalEclipseAPI.Core;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Typeless;

namespace InfernalEclipseAPI
{
	public class InfernalEclipseAPI : Mod
	{
        public static InfernalEclipseAPI Instance;
        public InfernalEclipseAPI() => Instance = this;

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                BossRushInjection(calamity);
            }
        }

        public static void BossRushInjection(Mod calamity)
        {
            List<(int, int, Action<int>, int, bool, float, int[], int[])> brEntries = (List<(int, int, Action<int>, int, bool, float, int[], int[])>)calamity.Call("GetBossRushEntries");

            bool sotsEnabled = false;
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                sotsEnabled = true;
                int pharaohID = sots.Find<ModNPC>("PharaohsCurse").Type;
                int pharaohBRIndex = 0;
                bool pharaohInBR = false;

                int subspaceID = sots.Find<ModNPC>("SubspaceSerpentHead").Type;
                int subspaceBRIndex = 0;

                for (int i = 0; i < brEntries.Count; i++)
                {
                    if (brEntries[i].Item1 == pharaohID)
                    {
                        pharaohBRIndex = i;
                        pharaohInBR = true;
                    }
                    else if (brEntries[i].Item1 == subspaceID)
                    {
                        subspaceBRIndex = i;
                    }
                }

                //if (pharaohInBR)
                //{
                //    Action<int> prPharaoh = delegate (int npc)
                //    {
                //        BossRushTeleports.PyramidTeleport();
                //        byte closest = Player.FindClosest(new Vector2(Main.maxTilesX * 8f, Main.maxTilesY * 8f), 0, 0);
                //        NPC.SpawnOnPlayer(closest, pharaohID);

                //        int npcIndex = NPC.FindFirstNPC(pharaohID);
                //        if (npcIndex != -1)
                //            Main.npc[npcIndex].Center = Main.player[closest].Center;
                //    };
                //    brEntries[pharaohBRIndex] = (brEntries[pharaohBRIndex].Item1, brEntries[pharaohBRIndex].Item2, prPharaoh, brEntries[pharaohBRIndex].Item4, brEntries[pharaohBRIndex].Item5, brEntries[pharaohBRIndex].Item6, brEntries[pharaohBRIndex].Item7, brEntries[pharaohBRIndex].Item8);
                //}
            }

            //Remove tier 3 completion from Skeletron spawn action to skeletron kill
            int skeletronID = NPCID.SkeletronHead;
            int skeletronBRIndex = 0;

            for (int i = 0; i < brEntries.Count; i++)
            {
                if (brEntries[i].Item1 == skeletronID)
                {
                    skeletronBRIndex = i;
                    break;
                }
            }

            Action<int> prSkeletron = delegate (int npc)
            {
                int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                Player player = Main.player[whomst];
                NPC.SpawnOnPlayer(whomst, skeletronID);
            };

            brEntries[skeletronBRIndex] = (brEntries[skeletronBRIndex].Item1, brEntries[skeletronBRIndex].Item2, prSkeletron, brEntries[skeletronBRIndex].Item4, brEntries[skeletronBRIndex].Item5, brEntries[skeletronBRIndex].Item6, brEntries[skeletronBRIndex].Item7, brEntries[skeletronBRIndex].Item8);

            BossRushEvent.BossDeathEffects[skeletronID] = npc =>
            {
                BossRushDialogueSystem.StartDialogue(BossRushDialoguePhase.TierTwoComplete);

                ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Player current = enumerator.Current;
                    if (!current.dead)
                    {
                        int num = Projectile.NewProjectile(new EntitySource_WorldEvent(), current.Center, Vector2.Zero, ModContent.ProjectileType<BossRushTierAnimation>(), 0, 0f, current.whoAmI);
                        if (Main.projectile.IndexInRange(num))
                        {
                            Main.projectile[num].ai[0] = 3;
                        }
                    }
                }
            };

            //Borean Strider (Thorium Mod) - Checks to see if Borean Strider is in Boss Rush. If not, adds it.
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) {
                int BoreanInsertID = Terraria.ID.NPCID.WallofFlesh;
                bool boreanInBossRush = false;
                int striderID = thorium.Find<ModNPC>("BoreanStrider").Type;

                for (int i = 0; i < brEntries.Count; i++)
                {
                    if (brEntries[i].Item1 == striderID)
                    {
                        boreanInBossRush = true;
                        break;
                    }
                    if (brEntries[i].Item1 == BoreanInsertID)
                    {
                        BoreanInsertID = i;
                    }
                }

                if (!boreanInBossRush)
                {
                    int[] boreanID = { thorium.Find<ModNPC>("BoreanStriderPopped").Type };
                    int[] boreanMinionIDs = { thorium.Find<ModNPC>("BoreanHopper").Type, thorium.Find<ModNPC>("BoreanMyte").Type };

                    Action<int> prBorean = delegate (int npc)
                    {
                        //SoundStyle roar;
                        int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                        Player player = Main.player[whomst];
                        //SoundEngine.PlaySound(DefaultBossRoar, player.Center);
                        NPC.SpawnOnPlayer(whomst, striderID);
                    };

                    brEntries.Insert(BoreanInsertID + 1, (striderID, -1, prBorean, 180, false, 0f, boreanMinionIDs, boreanID));
                }
            }

            //Dreadnautilus
            if (ModContent.GetInstance<InfernalConfig>().DreadnautillusInBossRush)
            {
                int[] dreadID = { Terraria.ID.NPCID.BloodNautilus };
                int[] dreadMinionsIDs = { Terraria.ID.NPCID.EyeballFlyingFish, Terraria.ID.NPCID.VampireBat };

                Action<int> prDread = delegate (int npc)
                {
                    //SoundStyle roar;
                    int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                    Player player = Main.player[whomst];
                    //SoundEngine.PlaySound(roar, player.Center);
                    NPC.SpawnOnPlayer(whomst, Terraria.ID.NPCID.BloodNautilus);
                };

                int DreadInsertID = Terraria.ID.NPCID.QueenSlimeBoss;

                for (int i = 0; i < brEntries.Count; i++)
                {
                    if (brEntries[i].Item1 == DreadInsertID)
                    {
                        DreadInsertID = i;
                        break;
                    }
                }
                brEntries.Insert(DreadInsertID - 1, (Terraria.ID.NPCID.BloodNautilus, -1, prDread, 180, false, 0f, dreadMinionsIDs, dreadID));
            }


            //Betsy
            if (ModContent.GetInstance<InfernalConfig>().BetsyInBossRush)
            {
                int[] betsyID = { Terraria.ID.NPCID.DD2Betsy };
                int[] betsyMinionsIDs = { };

                Action<int> prBetsy = delegate (int npc)
                {
                    SoundStyle roar = Terraria.ID.SoundID.DD2_BetsyScream;
                    int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                    Player player = Main.player[whomst];
                    SoundEngine.PlaySound(roar, player.Center);
                    NPC.SpawnOnPlayer(whomst, Terraria.ID.NPCID.DD2Betsy);
                };

                int BetsyInsertID = Terraria.ID.NPCID.HallowBoss;

                for (int i = 0; i < brEntries.Count; i++)
                {
                    if (brEntries[i].Item1 == BetsyInsertID)
                    {
                        BetsyInsertID = i;
                        break;
                    }
                }
                brEntries.Insert(BetsyInsertID + 1, (Terraria.ID.NPCID.DD2Betsy, 0, prBetsy, 400, true, 0f, betsyMinionsIDs, betsyID));
            }

            //Bereft Vassal (InfernumMode)
            int[] bereftID = { ModContent.NPCType<BereftVassal>() };
            int[] bereftMinionIDs = { ModContent.NPCType<GreatSandSharkNPC>() };

            Action<int> prBereft = delegate (int npc)
            {
                SoundStyle roar = InfernumMode.Assets.Sounds.InfernumSoundRegistry.VassalHornSound;
                int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                Player player = Main.player[whomst];
                SoundEngine.PlaySound(roar, player.Center);
                NPC.SpawnOnPlayer(whomst, ModContent.NPCType<BereftVassal>());
            };

            int BereftInsertID = Terraria.ID.NPCID.CultistBoss;

            for (int i = 0; i < brEntries.Count; i++)
            {
                if (brEntries[i].Item1 == BereftInsertID)
                {
                    BereftInsertID = i;
                    break;
                }
            }
            brEntries.Insert(BereftInsertID + 1, (ModContent.NPCType<BereftVassal>(), 0, prBereft, 180, true, 0f, bereftMinionIDs, bereftID));

            //Astrageldon (CatalystMod)
            if (ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
            {
                int[] AstraID = { catalyst.Find<ModNPC>("Astrageldon").Type };
                int[] AstraMinionIDs = { catalyst.Find<ModNPC>("NovaSlime").Type, catalyst.Find<ModNPC>("NovaSlimer").Type };

                Action<int> prAstra = delegate (int npc)
                {
                    //SoundStyle roar;
                    int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                    Player player = Main.player[whomst];
                    //SoundEngine.PlaySound(DefaultBossRoar, player.Center);
                    NPC.SpawnOnPlayer(whomst, catalyst.Find<ModNPC>("Astrageldon").Type);
                };

                //int AstraInsertID = Terraria.ID.NPCID.MoonLordCore;

                //for (int i = 0; i < brEntries.Count; i++)
                //{
                //    if (brEntries[i].Item1 == AstraInsertID)
                //    {
                //        AstraInsertID = i;
                //        break;
                //    }
                //}
                brEntries.Insert(BereftInsertID + 2, (catalyst.Find<ModNPC>("Astrageldon").Type, -1, prAstra, 180, false, 0f, AstraMinionIDs, AstraID));
            }


            //Terra Blade (YouBoss)

            //Used later
            int TerraInsertID = calamity.Find<ModNPC>("Polterghast").Type;

            for (int i = 0; i < brEntries.Count; i++)
            {
                if (brEntries[i].Item1 == TerraInsertID)
                {
                    TerraInsertID = i;
                    break;
                }
            }

            if (InfernalConfig.Instance.TerraBladeBossInBossRush)
            {
                int[] TerraMinionIDs = { };
                int[] TerraID = { ModContent.NPCType<TerraBladeBoss>() };

                Action<int> prTerra = delegate (int npc)
                {
                    SoundStyle roar = YouBoss.Assets.SoundsRegistry.TerraBlade.DashSound;
                    int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                    Player player = Main.player[whomst];
                    SoundEngine.PlaySound(roar, player.Center);
                    NPC.SpawnOnPlayer(whomst, ModContent.NPCType<TerraBladeBoss>());
                };

                brEntries.Insert(TerraInsertID - 1, (ModContent.NPCType<TerraBladeBoss>(), -1, prTerra, 180, true, 0f, TerraMinionIDs, TerraID));
            }

            //Old Duke (CalamityMod) - Reinserts Old Duke into Boss Rush if he has been removed by other mod (i.e. CalamityHunt)
            bool oDukeInBossRush = false;
            int OldDukeID = calamity.Find<ModNPC>("OldDuke").Type;

            for (int i = 0; (i < brEntries.Count); i++)
            {
                if (brEntries[i].Item1 == OldDukeID)
                {
                    oDukeInBossRush = true; break;
                }
            }

            if (!oDukeInBossRush)
            {
                int[] ODukeID = { calamity.Find<ModNPC>("OldDuke").Type };
                int[] ODukeMinionsIDs = { calamity.Find<ModNPC>("OldDukeToothBall").Type, calamity.Find<ModNPC>("SulphurousSharkron").Type };

                Action<int> prODuke = delegate (int npc)
                {
                    int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                    Player player = Main.player[whomst];
                    NPC.SpawnOnPlayer(whomst, calamity.Find<ModNPC>("OldDuke").Type);
                };

                brEntries.Insert(TerraInsertID + 1, (calamity.Find<ModNPC>("OldDuke").Type, 0, prODuke, 180, false, 0f, ODukeMinionsIDs, ODukeID));
            }

            //Primordial Wyrm
            int[] WyrmIDs = { calamity.Find<ModNPC>("PrimordialWyrmHead").Type, calamity.Find<ModNPC>("PrimordialWyrmBody").Type, calamity.Find<ModNPC>("PrimordialWyrmBodyAlt").Type, calamity.Find<ModNPC>("PrimordialWyrmTail").Type };
            int[] WyrmMinionIDs = { };

            Action<int> prWyrm = delegate (int npc)
            {
                SoundStyle roar = CalamityMod.Sounds.CommonCalamitySounds.WyrmScreamSound;
                int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                Player player = Main.player[whomst];
                SoundEngine.PlaySound(roar, player.Center);
                NPC.SpawnOnPlayer(whomst, calamity.Find<ModNPC>("PrimordialWyrmHead").Type);
            };

            int WyrmInsertID = calamity.Find<ModNPC>("SupremeCalamitas").Type;
            for (int i = 0; i < brEntries.Count; i++)
            {
                if (brEntries[i].Item1 == WyrmInsertID)
                {
                    WyrmInsertID = i;
                    break;
                }
            }
            brEntries.Insert(WyrmInsertID - 3, (calamity.Find<ModNPC>("PrimordialWyrmHead").Type, -1, prWyrm, 180, true, 0f, WyrmMinionIDs, WyrmIDs));

            if (ModContent.GetInstance<InfernalConfig>().WrathoftheGodsBossesInBossRush)
            {
                if (ModLoader.TryGetMod("NoxusBoss", out Mod wotg))
                {
                    //Mars
                    int[] MarsID = { wotg.Find<ModNPC>("MarsBody").Type };
                    int[] MarsMinionIDs = { wotg.Find<ModNPC>("BattleSolyn").Type, wotg.Find<ModNPC>("TrappingHolographicForcefield").Type };

                    Action<int> prMars = delegate (int npc)
                    {
                        int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                        Player player = Main.player[whomst];
                        NPC.SpawnOnPlayer(whomst, wotg.Find<ModNPC>("MarsBody").Type);
                    };

                    brEntries.Insert(WyrmInsertID - 1, (wotg.Find<ModNPC>("MarsBody").Type, -1, prMars, 180, false, 0f, MarsMinionIDs, MarsID));

                    //Avatar of Emptiness
                    int[] AvatarMinionIDs = { wotg.Find<ModNPC>("BattleSolyn").Type, wotg.Find<ModNPC>("NamelessDeityBoss").Type };
                    int[] AvatarIDs = { wotg.Find<ModNPC>("AvatarRift").Type, wotg.Find<ModNPC>("AvatarOfEmptiness").Type };

                    Action<int> prAvatar = delegate (int npc)
                    {
                        //SoundStyle roar;
                        int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                        Player player = Main.player[whomst];
                        //SoundEngine.PlaySound(roar, player.Center);
                        NPC.SpawnOnPlayer(whomst, wotg.Find<ModNPC>("AvatarRift").Type);
                    };


                    int AvatarInstertID;
                    if (ModLoader.TryGetMod("CalamityHunt", out Mod CalHunt))
                    {
                        AvatarInstertID = CalHunt.Find<ModNPC>("Goozma").Type;
                    }
                    else
                    {
                        AvatarInstertID = ModContent.NPCType<SupremeCalamitas>();
                    }

                    for (int i = 0; i < brEntries.Count; i++)
                    {
                        if (brEntries[i].Item1 == AvatarInstertID)
                        {
                            AvatarInstertID = i;
                            break;
                        }
                    }

                    brEntries.Insert(AvatarInstertID + 1, (wotg.Find<ModNPC>("AvatarRift").Type, -1, prAvatar, 180, false, 0f, AvatarMinionIDs, AvatarIDs));

                    //Nameless Deity
                    int[] NamelessMinionIDs = { };
                    int[] NamelessID = { wotg.Find<ModNPC>("NamelessDeityBoss").Type };

                    Action<int> prNameless = delegate (int npc)
                    {
                        int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                        Player player = Main.player[whomst];
                        NPC.SpawnOnPlayer(whomst, wotg.Find<ModNPC>("NamelessDeityBoss").Type);
                    };

                    int NamelessInsertID;

                    //Checks to see if Mutant is in Boss Rush, then puts nameless after him.
                    if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSouls) && ModLoader.TryGetMod("FargowiltasCrossMod", out Mod soulsDLC))
                    {
                        NamelessInsertID = fargoSouls.Find<ModNPC>("MutantBoss").Type;

                        for (int i = 0; i < brEntries.Count; i++)
                        {
                            if (brEntries[i].Item1 == NamelessInsertID)
                            {
                                NamelessInsertID = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        NamelessInsertID = AvatarInstertID + 1;
                    }

                    brEntries.Insert(NamelessInsertID + 1, (wotg.Find<ModNPC>("NamelessDeityBoss").Type, -1, prNameless, 270, false, 0f, NamelessMinionIDs, NamelessID));
                }
            }

            //Finish the call
            calamity.Call("SetBossRushEntries", brEntries);

            //Use Multiplayer friendly teleports
            //Wall of Flessh
            //BossDeathEffects.Remove(NPCID.WallofFlesh);
            //BossDeathEffects.Add(NPCID.WallofFlesh, npc => {
            //    ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        Player current = enumerator.Current;
            //        if (current.Calamity().BossRushReturnPosition.HasValue)
            //        {
            //            CalamityPlayer.ModTeleport(current, current.Calamity().BossRushReturnPosition.Value, playSound: false, 2);
            //            current.Calamity().BossRushReturnPosition = null;
            //        }

            //        current.Calamity().BossRushReturnPosition = null;
            //        SoundStyle style = TeleportSound with
            //        {
            //            Volume = 1.6f
            //        };
            //        SoundEngine.PlaySound(in style, current.Center);
            //    }
            //});
            
            //Ocram
            if (ModLoader.TryGetMod("Consolaria", out Mod consolaria))
            {
                BossDeathEffects.Add(consolaria.Find<ModNPC>("Ocram").Type, npc => { Main.bloodMoon = false; });
            }

            //Pharaoh's Curse
            //if (sotsEnabled)
            //{
            //    BossDeathEffects.Remove(sots.Find<ModNPC>("PharaohsCurse").Type);
            //    BossDeathEffects.Add(sots.Find<ModNPC>("PharaohsCurse").Type, npc => {
            //        ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
            //        while (enumerator.MoveNext())
            //        {
            //            Player current = enumerator.Current;
            //            if (current.Calamity().BossRushReturnPosition.HasValue)
            //            {
            //                CalamityPlayer.ModTeleport(current, current.Calamity().BossRushReturnPosition.Value, playSound: false, 2);
            //                current.Calamity().BossRushReturnPosition = null;
            //            }

            //            current.Calamity().BossRushReturnPosition = null;
            //            SoundStyle style = TeleportSound with
            //            {
            //                Volume = 1.6f
            //            };
            //            SoundEngine.PlaySound(in style, current.Center);
            //        }
            //    });

            //    BossDeathEffects.Remove(sots.Find<ModNPC>("SubspaceSerpentHead").Type);
            //    BossDeathEffects.Add(sots.Find<ModNPC>("SubspaceSerpentHead").Type, npc => { ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        Player current = enumerator.Current;
            //        if (current.Calamity().BossRushReturnPosition.HasValue)
            //        {
            //            CalamityPlayer.ModTeleport(current, current.Calamity().BossRushReturnPosition.Value, playSound: false, 2);
            //            current.Calamity().BossRushReturnPosition = null;
            //        }

            //        current.Calamity().BossRushReturnPosition = null;
            //        SoundStyle style = TeleportSound with
            //        {
            //            Volume = 1.6f
            //        };
            //        SoundEngine.PlaySound(in style, current.Center);
            //    } });
            //}

            BossDeathEffects.Remove(ModContent.NPCType<ProfanedGuardianCommander>());
            BossDeathEffects.Add(ModContent.NPCType<ProfanedGuardianCommander>(), npc =>
            {
                BossRushDialogueSystem.StartDialogue(BossRushDialoguePhase.TierOneComplete);
                ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Player current = enumerator.Current;
                    if (!current.dead)
                    {
                        current.Spawn(PlayerSpawnContext.RecallFromItem);
                        int num = Projectile.NewProjectile(new EntitySource_WorldEvent(), current.Center, Vector2.Zero, ModContent.ProjectileType<BossRushTierAnimation>(), 0, 0f, current.whoAmI);
                        if (Main.projectile.IndexInRange(num))
                        {
                            Main.projectile[num].ai[0] = 2;
                        }
                    }
                }
            });

            BossDeathEffects.Remove(ModContent.NPCType<ProfanedGuardianCommander>());
            BossDeathEffects.Add(ModContent.NPCType<ProfanedGuardianCommander>(), npc =>
            {
                BossRushDialogueSystem.StartDialogue(BossRushDialoguePhase.TierOneComplete);
                ActiveEntityIterator<Player>.Enumerator enumerator = Main.ActivePlayers.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Player current = enumerator.Current;
                    if (!current.dead)
                    {
                        current.Spawn(PlayerSpawnContext.RecallFromItem);
                        int num = Projectile.NewProjectile(new EntitySource_WorldEvent(), current.Center, Vector2.Zero, ModContent.ProjectileType<BossRushTierAnimation>(), 0, 0f, current.whoAmI);
                        if (Main.projectile.IndexInRange(num))
                        {
                            Main.projectile[num].ai[0] = 2;
                        }
                    }
                }
            });




            //Removes the end effect of boss rush and assigns it to the final boss.
            BossDeathEffects.Remove(ModContent.NPCType<SupremeCalamitas>());
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargos) && ModLoader.TryGetMod("FargowiltasCrossMod", out Mod fargoDLC))
            {
                BossDeathEffects.Remove(fargos.Find<ModNPC>("MutantBoss").Type);
            }
            BossDeathEffects.Remove(brEntries[^1].Item1);

            BossDeathEffects.Add(brEntries[^1].Item1, npc =>
            {
                if (InfernalConfig.Instance.ForceFullXerocDialogue)
                {
                    //Always play end dialgoue
                    BossRushDialogueSystem.StartDialogue(BossRushDialoguePhase.End);
                }
                else
                {
                    BossRushDialogueSystem.StartDialogue(DownedBossSystem.downedBossRush ? BossRushDialoguePhase.EndRepeat : BossRushDialoguePhase.End);
                }
                CalamityUtils.KillAllHostileProjectiles();
                HostileProjectileKillCounter = 3;
            });
        }
    }
}
