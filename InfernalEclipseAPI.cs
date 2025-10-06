global using Terraria;
global using Terraria.ModLoader;
global using Terraria.ID;
global using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using InfernumMode.Content.BehaviorOverrides.BossAIs.GreatSandShark;
using GreatSandSharkNPC = CalamityMod.NPCs.GreatSandShark.GreatSandShark;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod;
using CalamityMod.Events;
using static CalamityMod.Events.BossRushEvent;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.Enums;
using CalamityMod.Systems;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.Projectiles.Typeless;
using System.IO;
using InfernalEclipseAPI.Core.Players;
using System.Reflection;
using InfernalEclipseAPI.Content.Projectiles;
using InfernalEclipseAPI.Core.World;
using CalamityMod.NPCs.Polterghast;
using InfernalEclipseAPI.Core.Players.ThoriumMulticlassNerf;

namespace InfernalEclipseAPI
{
    public enum InfernalEclipseMessageType : byte
    {
        SyncDownedBosses = 1,
        TriggerScytheCharge = 2,
        ThoriumEmpowerment = 3
    }
    public class InfernalEclipseAPI : Mod
	{
        public static bool FargosDLCEnabled
        {
            get
            {
                if (ModLoader.TryGetMod("FargowiltasCrossMod", out Mod fargoDLC))
                    return true;
                return false;
            }
        }

        public static InfernalEclipseAPI Instance;
        public InfernalEclipseAPI() => Instance = this;

        public static int WhiteFlareType = 0;
        private bool _hijackInteraction;

        public override void Load()
        {
            DifficultyManagementSystem.DisableDifficultyModes = false;

            // Cache the WhiteFlare projectile type from Thorium
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (thorium.TryFind<ModProjectile>("WhiteFlare", out var whiteFlare))
                    WhiteFlareType = whiteFlare.Type;

                if (ModLoader.TryGetMod("CellphonePylon", out _))
                {
                    On_Player.IsTileTypeInInteractionRange += On_Player_IsTileTypeInInteractionRange;
                    On_Player.InInteractionRange += On_Player_InInteractionRange;
                }
            }

            AchievementUpdateHandler = typeof(InfernumMode.Core.GlobalInstances.Players.AchievementPlayer).GetMethod("ExtraUpdateHandler", BindingFlags.Static | BindingFlags.NonPublic);

            BackgroundTextureLoader.AddBackgroundTexture(this, "InfernalEclipseAPI/Assets/Textures/Menu/MenuClouds");
            BackgroundTextureLoader.AddBackgroundTexture(this, "InfernalEclipseAPI/Assets/Textures/Menu/MenuMountains");
            BackgroundTextureLoader.AddBackgroundTexture(this, "InfernalEclipseAPI/Assets/Textures/Menu/MenuHills");
        }

        public override void Unload()
        {
            DifficultyManagementSystem.DisableDifficultyModes = true;
            WhiteFlareType = 0; // Clean up on unload
            On_Player.IsTileTypeInInteractionRange -= On_Player_IsTileTypeInInteractionRange;
            On_Player.InInteractionRange -= On_Player_InInteractionRange;
        }

        private bool On_Player_IsTileTypeInInteractionRange(
            On_Player.orig_IsTileTypeInInteractionRange orig,
            Player self,
            int targetTileType,
            TileReachCheckSettings settings)
        {
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            // Only hijack for tile type 597
            if (targetTileType != 597)
                return orig(self, targetTileType, settings);

            _hijackInteraction = true;
            return orig(self, targetTileType, settings)
                || self.HasItemInInventoryOrOpenVoidBag(thorium.Find<ModItem>("WishingGlass").Type);
        }

        private bool On_Player_InInteractionRange(
            On_Player.orig_InInteractionRange orig,
            Player self,
            int interactX,
            int interactY,
            TileReachCheckSettings settings)
        {
            // If not hijacking, proceed as normal
            if (!_hijackInteraction)
                return orig(self, interactX, interactY, settings);

            // Reset hijack flag and allow interaction
            _hijackInteraction = false;
            return true;
        }

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                BossRushInjection(calamity);
            }
        }

        MethodInfo AchievementUpdateHandler;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            InfernalEclipseMessageType msgType = (InfernalEclipseMessageType)reader.ReadByte();

            switch (msgType)
            {
                case InfernalEclipseMessageType.SyncDownedBosses:
                    bool downed = reader.ReadBoolean();
                    InfernalDownedBossSystem.downedDreadNautilus = downed;

                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)InfernalEclipseMessageType.SyncDownedBosses);
                        packet.Write(downed);
                        packet.Send(-1, whoAmI); // sync to all clients except sender
                    }
                    break;

                case InfernalEclipseMessageType.TriggerScytheCharge:
                    byte index = reader.ReadByte();
                    if (index < byte.MaxValue)
                    {
                        Main.player[index].GetModPlayer<HealerPlayer>().TriggerScytheCharge(true);
                    }
                    break;

                case InfernalEclipseMessageType.ThoriumEmpowerment:
                    {
                        if (!ModLoader.TryGetMod("ThoriumMod", out _))
                            break;

                        ThoriumEmpowermentMsg sub = (ThoriumEmpowermentMsg)reader.ReadByte();
                        switch (sub)
                        {
                            case ThoriumEmpowermentMsg.ClearEmpowerments:
                                {
                                    int plr = reader.ReadByte();
                                    if (plr >= 0 && plr < Main.maxPlayers)
                                    {
                                        Player target = Main.player[plr];
                                        ThoriumHelpers.ClearAllEmpowerments(target);

                                        // relay to others if server
                                        if (Main.netMode == NetmodeID.Server)
                                        {
                                            ModPacket p = GetPacket();
                                            p.Write((byte)InfernalEclipseMessageType.ThoriumEmpowerment);
                                            p.Write((byte)ThoriumEmpowermentMsg.ClearEmpowerments);
                                            p.Write((byte)plr);
                                            p.Send(-1, whoAmI);
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }

            //int npcIndex = reader.ReadInt32();
            //if (AchievementUpdateHandler != null && Main.netMode == NetmodeID.MultiplayerClient)
            //{
            //    AchievementUpdateHandler.Invoke(null, new object[] { Main.LocalPlayer, InfernumMode.Content.Achievements.AchievementUpdateCheck.NPCKill, npcIndex });
            //}
            //else
            //{
            //    Logger.Debug("Didnt find methodinfo for achievement update handler!");
            //}
        }

        public static void BossRushInjection(Mod calamity)
        {
            List<(int, int, Action<int>, int, bool, float, int[], int[])> brEntries = (List<(int, int, Action<int>, int, bool, float, int[], int[])>)calamity.Call("GetBossRushEntries");

            if (!FargosDLCEnabled)
            {
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

                //The Excavator
                if (sotsEnabled)
                {

                    int[] excavatorID = { sots.Find<ModNPC>("Excavator").Type };
                    int[] excatorMinionIDs = { sots.Find<ModNPC>("ExcavatorBody").Type, sots.Find<ModNPC>("ExcavatorBody2").Type, sots.Find<ModNPC>("ExcavatorTail").Type, sots.Find<ModNPC>("ExcavatorDrillTail").Type };

                    Action<int> prExcavator = delegate (int npc)
                    {
                        int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                        Player player = Main.player[whomst];
                        NPC.SpawnOnPlayer(whomst, sots.Find<ModNPC>("Excavator").Type);
                    };

                    brEntries.Insert(skeletronBRIndex - 1, (sots.Find<ModNPC>("Excavator").Type, -1, prExcavator, 180, false, 0f, excatorMinionIDs, excavatorID));
                }

                //Borean Strider (Thorium Mod) - Checks to see if Borean Strider is in Boss Rush. If not, adds it.
                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) 
                    //&& !ModLoader.TryGetMod("ThoriumRework", out _)
                    )
                {
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
                        int[] boreanMinionIDs = { thorium.Find<ModNPC>("BoreanStrider").Type, thorium.Find<ModNPC>("BoreanStriderPopped").Type, thorium.Find<ModNPC>("BoreanHopper").Type, thorium.Find<ModNPC>("BoreanMyte").Type };

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
            }

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

                int AstraInsertID = Terraria.ID.NPCID.MoonLordCore;

                for (int i = 0; i < brEntries.Count; i++)
                {
                    if (brEntries[i].Item1 == AstraInsertID)
                    {
                        AstraInsertID = i;
                        break;
                    }
                }

                if (FargosDLCEnabled)
                {
                    AstraInsertID += 1;
                }
                else
                {
                    AstraInsertID -= 1;
                }

                    brEntries.Insert(AstraInsertID, (catalyst.Find<ModNPC>("Astrageldon").Type, -1, prAstra, 180, false, 0f, AstraMinionIDs, AstraID));
            }


            //Terra Blade (YouBoss)
            int TerraInsertID = calamity.Find<ModNPC>("Polterghast").Type;

            for (int i = 0; i < brEntries.Count; i++)
            {
                if (brEntries[i].Item1 == TerraInsertID)
                {
                    TerraInsertID = i;
                    break;
                }
            }

            if (InfernalConfig.Instance.TerraBladeBossInBossRush && ModLoader.TryGetMod("YouBoss", out Mod you))
            {
                int[] TerraMinionIDs = { };
                int[] TerraID = { you.Find<ModNPC>("TerraBladeBoss").Type };

                Action<int> prTerra = delegate (int npc)
                {
                    SoundStyle roar = new SoundStyle("InfernalEclipseAPI/Assets/Sounds/Custom/TerraBlade/Dash");
                    int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                    Player player = Main.player[whomst];
                    SoundEngine.PlaySound(roar, player.Center);
                    NPC.SpawnOnPlayer(whomst, you.Find<ModNPC>("TerraBladeBoss").Type);
                };

                brEntries.Insert(TerraInsertID - 1, (you.Find<ModNPC>("TerraBladeBoss").Type, -1, prTerra, 180, true, 0f, TerraMinionIDs, TerraID));
            }

            //Old Duke (CalamityMod) - Reinserts Old Duke into Boss Rush if he has been removed by other mod (i.e. CalamityHunt)
            bool oDukeInBossRush = false;
            int OldDukeID = calamity.Find<ModNPC>("OldDuke").Type;
            int OldDukeInsertID = -1;
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargos))
            {
                OldDukeInsertID = ModContent.NPCType<Polterghast>();
            }
            else
            {
                OldDukeInsertID = NPCID.SkeletronPrime;
            }

            for (int i = 0; (i < brEntries.Count); i++)
            {
                if (brEntries[i].Item1 == OldDukeID)
                {
                    oDukeInBossRush = true; break;
                }
                if (brEntries[i].Item1 == OldDukeInsertID)
                {
                    OldDukeInsertID = i;
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

                brEntries.Insert(OldDukeInsertID + 1, (calamity.Find<ModNPC>("OldDuke").Type, 0, prODuke, 180, false, 0f, ODukeMinionsIDs, ODukeID));
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

                    brEntries.Insert(WyrmInsertID + 1, (wotg.Find<ModNPC>("MarsBody").Type, -1, prMars, 180, false, 0f, MarsMinionIDs, MarsID));

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


                    //int AvatarInstertID;
                    //if (ModLoader.TryGetMod("CalamityHunt", out Mod CalHunt))
                    //{
                    //    AvatarInstertID = CalHunt.Find<ModNPC>("Goozma").Type;
                    //}
                    //else
                    //{
                    //    AvatarInstertID = ModContent.NPCType<SupremeCalamitas>();
                    //}

                    //for (int i = 0; i < brEntries.Count; i++)
                    //{
                    //    if (brEntries[i].Item1 == AvatarInstertID)
                    //    {
                    //        AvatarInstertID = i;
                    //        break;
                    //    }
                    //}

                    brEntries.Insert(brEntries.Count, (wotg.Find<ModNPC>("AvatarRift").Type, -1, prAvatar, 180, false, 0f, AvatarMinionIDs, AvatarIDs));

                    //Nameless Deity
                    int[] NamelessMinionIDs = { };
                    int[] NamelessID = { wotg.Find<ModNPC>("NamelessDeityBoss").Type };

                    Action<int> prNameless = delegate (int npc)
                    {
                        int whomst = Player.FindClosest(new Vector2(Main.maxTilesX, Main.maxTilesY) * 16f * 0.5f, 1, 1);
                        Player player = Main.player[whomst];
                        NPC.SpawnOnPlayer(whomst, wotg.Find<ModNPC>("NamelessDeityBoss").Type);
                    };
                   
                    //last boss always
                    brEntries.Insert(brEntries.Count, (wotg.Find<ModNPC>("NamelessDeityBoss").Type, -1, prNameless, 270, false, 0f, NamelessMinionIDs, NamelessID));
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
                BossDeathEffects.Remove(consolaria.Find<ModNPC>("Ocram").Type);
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

            //Removes the end effect of boss rush and assigns it to the final boss.
            BossDeathEffects.Remove(ModContent.NPCType<SupremeCalamitas>());
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargos1) && FargosDLCEnabled)
            {
                BossDeathEffects.Remove(fargos1.Find<ModNPC>("MutantBoss").Type);
            }
            BossDeathEffects.Remove(brEntries[^1].Item1);

            //Adds the tier 6 animation to Calamitas if she isn't the last boss of Boss Rush
            if (!(brEntries[^1].Item1 == ModContent.NPCType<SupremeCalamitas>()))
            {
                BossDeathEffects.Add(ModContent.NPCType<SupremeCalamitas>(), npc =>
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        foreach (Player p in Main.ActivePlayers)
                        {
                            if (p.dead)
                                continue;

                            int animation = Projectile.NewProjectile(new EntitySource_WorldEvent(), p.Center, Vector2.Zero, ModContent.ProjectileType<BossRushTier6Animation>(), 0, 0f, p.whoAmI);
                        }
                    }
                });
            }

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
