using System.Collections.Generic;
using System.Reflection;
using SOTS.Items.Chaos;
using SOTS.Items.Earth;
using SOTS.Items.Permafrost;
using SOTS;
using Terraria.Utilities;
using MonoMod.RuntimeDetour;
using CalamityMod;
using CalamityMod.Tiles.SunkenSea;
using CalamityMod.Tiles.Ores;
using SOTS.Items.Furniture.Functional;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class SOTSMineralariumHooks : ModSystem
    {
        private static Hook durationHook;
        private static Hook getRandomHook;
        private static Hook countsHook;
        private static Hook killTileHook;

        public override void OnModLoad()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            // ---------- existing OreType hooks (from previous message) ----------
            Type oreType = sots.Code?.GetType("SOTS.Items.Furniture.Functional.MineralariumTE+OreType");
            if (oreType != null)
            {
                MethodInfo srcDuration = oreType.GetMethod("DurationBasedOnType", flags);
                MethodInfo srcGetRandom = oreType.GetMethod("GetRandomType", flags);
                MethodInfo srcCounts = oreType.GetMethod("CountsAsOre", flags);

                if (srcDuration != null && srcGetRandom != null && srcCounts != null)
                {
                    durationHook = new Hook(
                        srcDuration,
                        typeof(SOTSMineralariumHooks).GetMethod(nameof(DurationBasedOnType_Hook), BindingFlags.NonPublic | BindingFlags.Static));

                    getRandomHook = new Hook(
                        srcGetRandom,
                        typeof(SOTSMineralariumHooks).GetMethod(nameof(GetRandomType_Hook), BindingFlags.NonPublic | BindingFlags.Static));

                    countsHook = new Hook(
                        srcCounts,
                        typeof(SOTSMineralariumHooks).GetMethod(nameof(CountsAsOre_Hook), BindingFlags.NonPublic | BindingFlags.Static));
                }
            }

            // ---------- hook MineralariumTile.KillTile to stop ore breaking ----------
            MethodInfo killTileMI = typeof(MineralariumTile).GetMethod("KillTile", BindingFlags.Public | BindingFlags.Instance);

            if (killTileMI != null)
            {
                killTileHook = new Hook(
                    killTileMI,
                    typeof(SOTSMineralariumHooks).GetMethod(
                        nameof(MineralariumKillTile_Hook),
                        BindingFlags.NonPublic | BindingFlags.Static
                    )
                );
            }
        }

        public override void OnModUnload()
        {
            durationHook?.Dispose();
            getRandomHook?.Dispose();
            countsHook?.Dispose();
            killTileHook?.Dispose();

            durationHook = null;
            getRandomHook = null;
            countsHook = null;
            killTileHook = null;
        }

        public override void PostSetupContent()
        {
            ParseNewOre(ModContent.TileType<SeaPrism>(), 1500, 0.4, () => DownedBossSystem.downedDesertScourge);
            ParseNewOre(ModContent.TileType<AerialiteOre>(), 2700, 0.5, () => DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind);
            ParseNewOre(ModContent.TileType<InfernalSuevite>(), 3050, 0.65, () => NPC.downedMechBossAny);
            ParseNewOre(ModContent.TileType<CryonicOre>(), 3950, 0.7, () => DownedBossSystem.downedCryogen && (NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss1 && NPC.downedMechBoss3 || NPC.downedMechBoss2 && NPC.downedMechBoss3));
            ParseNewOre(ModContent.TileType<HallowedOre>(), 2100, 1.25, () => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
            ParseNewOre(ModContent.TileType<PerennialOre>(), 4100, 1.25, () => NPC.downedPlantBoss);
            ParseNewOre(ModContent.TileType<ScoriaOre>(), 4300, 1.25, () => NPC.downedGolemBoss);
            ParseNewOre(ModContent.TileType<AstralOre>(), 10000, 1.25, () => DownedBossSystem.downedAstrumDeus);
            ParseNewOre(ModContent.TileType<ExodiumOre>(), 11100, 1.25, () => NPC.downedMoonlord);
            ParseNewOre(ModContent.TileType<UelibloomOre>(), 11250, 1.3, () => DownedBossSystem.downedProvidence);
            ParseNewOre(ModContent.TileType<AuricOre>(), 11300, 1.35, () => DownedBossSystem.downedYharon);
        }

        // =========================================================
        //  KillTile hook: prevent ore destruction
        // =========================================================

        private delegate void Orig_MineralariumKillTile(
            MineralariumTile self,
            int i,
            int j,
            ref bool fail,
            ref bool effectOnly,
            ref bool noItem
        );

        private static void MineralariumKillTile_Hook(
            Orig_MineralariumKillTile orig,
            MineralariumTile self,
            int i,
            int j,
            ref bool fail,
            ref bool effectOnly,
            ref bool noItem)
        {
            if (fail || effectOnly)
                return;
        }


        // =========================================================
        //  New "OreType" implementation (dictionary-based)
        // =========================================================

        private delegate bool SpawnCondition();

        private static readonly Dictionary<int, int> SpawnDuration = new()
        {
            { TileID.Copper,       1200 },
            { TileID.Tin,          1230 },
            { TileID.Iron,         1230 },
            { TileID.Lead,         1260 },
            { TileID.Silver,       1260 },
            { TileID.Tungsten,     1300 },
            { TileID.Gold,         1400 },
            { TileID.Platinum,     1450 },
            { TileID.Meteorite,    1900 },
            { TileID.Demonite,     2300 },
            { TileID.Crimtane,     2360 },
            { TileID.Obsidian,     1200 },
            { TileID.Hellstone,    2700 },
            { TileID.Cobalt,       2700 },
            { TileID.Palladium,    2800 },
            { TileID.Mythril,      3000 },
            { TileID.Orichalcum,   3100 },
            { TileID.Adamantite,   3900 },
            { TileID.Titanium,     4000 },
            { TileID.Chlorophyte,  2100 },
            { TileID.LunarOre,     11100 },

            { ModContent.TileType<FrigidIceTile>(),      1600 },
            { ModContent.TileType<FrigidIceTileSafe>(),  1600 },
            { ModContent.TileType<VibrantOreTile>(),     1400 },
            { ModContent.TileType<PhaseOreTile>(),       5100 },
        };

        private static readonly Dictionary<int, double> OreWeights = new()
        {
            { TileID.Copper,    0.2 },
            { TileID.Tin,       0.2 },
            { TileID.Iron,      0.25 },
            { TileID.Lead,      0.25 },
            { TileID.Silver,    0.3 },
            { TileID.Tungsten,  0.3 },
            { TileID.Gold,      0.35 },
            { TileID.Platinum,  0.35 },

            { TileID.Demonite,  0.5 },
            { TileID.Crimtane,  0.5 },
            { ModContent.TileType<VibrantOreTile>(), 0.6 },

            { TileID.Obsidian,  0.2 },
            { TileID.Meteorite, 0.5 },
            { ModContent.TileType<FrigidIceTileSafe>(), 0.75 },
            { TileID.Hellstone, 1 },

            { TileID.Cobalt,    0.6 },
            { TileID.Palladium, 0.6 },
            { TileID.Mythril,   0.65 },
            { TileID.Orichalcum,0.65 },
            { TileID.Adamantite,0.7 },
            { TileID.Titanium,  0.7 },

            { TileID.Chlorophyte,           1.25 },
            { ModContent.TileType<PhaseOreTile>(), 1.25 },
            { TileID.LunarOre,              1.25 },
        };

        private static readonly Dictionary<int, SpawnCondition> OreSpawnConditions = new()
        {
            { TileID.Demonite, () => NPC.downedBoss1 },
            { TileID.Crimtane, () => NPC.downedBoss1 },
            { ModContent.TileType<VibrantOreTile>(), () => NPC.downedBoss1 },

            { TileID.Obsidian, () => NPC.downedBoss2 },
            { TileID.Meteorite, () => NPC.downedBoss2 },
            { ModContent.TileType<FrigidIceTileSafe>(), () => NPC.downedBoss2 },

            { TileID.Hellstone, () => NPC.downedBoss3 || SOTSWorld.downedAdvisor },

            { TileID.Cobalt,    () => Main.hardMode },
            { TileID.Palladium, () => Main.hardMode },

            { TileID.Mythril,   () => CalamityServerConfig.Instance.EarlyHardmodeProgressionRework ? NPC.downedMechBossAny : Main.hardMode },
            { TileID.Orichalcum,() => CalamityServerConfig.Instance.EarlyHardmodeProgressionRework ? NPC.downedMechBossAny : Main.hardMode },

            { TileID.Adamantite,() => CalamityServerConfig.Instance.EarlyHardmodeProgressionRework ? NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss1 && NPC.downedMechBoss3 || NPC.downedMechBoss2 && NPC.downedMechBoss3 : NPC.downedMechBossAny },
            { TileID.Titanium,  () => CalamityServerConfig.Instance.EarlyHardmodeProgressionRework ? NPC.downedMechBoss1 && NPC.downedMechBoss2 || NPC.downedMechBoss1 && NPC.downedMechBoss3 || NPC.downedMechBoss2 && NPC.downedMechBoss3 : NPC.downedMechBossAny },

            { TileID.Chlorophyte, () => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 },
            { ModContent.TileType<PhaseOreTile>(), () => SOTSWorld.downedLux },
            { TileID.LunarOre,  () => NPC.downedMoonlord },
        };

        private static readonly HashSet<int> OreTypes = new()
        {
            TileID.Copper, TileID.Tin,
            TileID.Iron, TileID.Lead,
            TileID.Silver, TileID.Tungsten,
            TileID.Gold, TileID.Platinum,
            TileID.Meteorite,
            TileID.Demonite, TileID.Crimtane,
            TileID.Obsidian, TileID.Hellstone,
            TileID.Cobalt, TileID.Palladium,
            TileID.Mythril, TileID.Orichalcum,
            TileID.Adamantite, TileID.Titanium,
            TileID.Chlorophyte, TileID.LunarOre,

            ModContent.TileType<FrigidIceTile>(),
            ModContent.TileType<FrigidIceTileSafe>(),
            ModContent.TileType<PhaseOreTile>(),
            ModContent.TileType<VibrantOreTile>(),
        };

        // =========================================================
        //  Hook targets (signatures must match)
        // =========================================================
        private static int DurationBasedOnType_Hook(Func<int, int> orig, int oreID)
        {
            if (SpawnDuration.TryGetValue(oreID, out int dur))
                return dur;

            return orig(oreID);
        }

        private static int GetRandomType_Hook(Func<int> orig)
        {
            WeightedRandom<int> types = new();

            foreach (var kv in OreWeights)
            {
                int oreType = kv.Key;
                double weight = kv.Value;

                if (OreSpawnConditions.TryGetValue(oreType, out SpawnCondition cond))
                {
                    if (cond != null && cond())
                        types.Add(oreType, weight);
                }
                else
                {
                    types.Add(oreType, weight);
                }
            }

            if (types.elements.Count == 0)
                return orig();

            return types.Get();
        }

        private static bool CountsAsOre_Hook(Func<int, bool> orig, int t)
        {
            if (OreTypes.Contains(t))
                return true;

            return orig(t);
        }

        // =========================================================
        //  Public API – add new ores from any mod
        // =========================================================

        /// <summary>
        /// Registers a new ore so Mineralarium can spawn it.
        /// </summary>
        /// <param name="oreType">Tile ID of the ore.</param>
        /// <param name="duration">Generation duration (ticks).</param>
        /// <param name="weight">Spawn weight in the weighted random.</param>
        /// <param name="condition">Optional: condition for when this ore can spawn.</param>
        public static bool ParseNewOre(int oreType, int duration, double weight, Func<bool> condition = null)
        {
            if (duration <= 0)
                throw new ArgumentException("duration must be > 0", nameof(duration));
            if (weight <= 0)
                throw new ArgumentException("weight must be > 0", nameof(weight));

            OreTypes.Add(oreType);
            SpawnDuration[oreType] = duration;
            OreWeights[oreType] = weight;

            if (condition != null)
                OreSpawnConditions[oreType] = () => condition();

            return true;
        }
    }
}
