using Terraria.Utilities;
using ThoriumMod;

namespace InfernalEclipseAPI.Core.Utils
{
    [ExtendsFromMod("ThoriumMod")]
    public static partial class ThoriumItemUtils
    {
        internal static int GetReworkedReforge(Item item, UnifiedRandom rand, int currentPrefix)
        {
            Mod CalamityMod = ModLoader.GetMod("CalamityMod");
            int GetCalPrefix(string name)
            {
                bool found = CalamityMod.TryFind(name, out ModPrefix ret);
                return found ? ret.Type : 0;
            }
            Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");
            int GetThorPrefix(string name)
            {
                bool found = ThoriumMod.TryFind(name, out ModPrefix ret);
                return found ? ret.Type : 0;
            }

            int prefix = -1;

            //HEALER
            if (item.CountsAsClass(ThoriumDamageBase<HealerDamage>.Instance) || item.CountsAsClass(ThoriumDamageBase<HealerToolDamageHybrid>.Instance))
            {
                //Heavy Scythes from BardHealer mods
                if (item.ModItem.MeleePrefix() && item.ModItem.Mod.Name == "CalamityBardHealer" || item.ModItem.Mod.Name == "SOTSBardHealer" || item.ModItem.Mod.Name == "SpookyBardHealer")
                {
                   int[][] meleeReforgeTiers = new int[][]
                   {
                        /* 0 */ new int[] { PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Light, PrefixID.Heavy, PrefixID.Light, PrefixID.Forceful, PrefixID.Strong },
                        /* 1 */ new int[] { PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Pointy, PrefixID.Bulky },
                        /* 2 */ new int[] { PrefixID.Murderous, PrefixID.Agile, PrefixID.Large, PrefixID.Dangerous, PrefixID.Sharp },
                        /* 3 */ new int[] { PrefixID.Massive, PrefixID.Unpleasant, PrefixID.Savage, PrefixID.Superior },
                        /* 4 */ new int[] { PrefixID.Demonic, PrefixID.Deadly2, PrefixID.Godly },
                        /* 5 */ new int[] { PrefixID.Legendary }
                   };
                    prefix = IteratePrefix(rand, meleeReforgeTiers, currentPrefix);
                }
                else //everything else
                {
                    int[][] magicReforgeTiers = new int[][]
                    {
                        /* 0 */ new int[] { PrefixID.Keen, PrefixID.Nimble, PrefixID.Nasty, PrefixID.Furious, PrefixID.Forceful, PrefixID.Strong },
                        /* 1 */ new int[] { PrefixID.Hurtful, PrefixID.Ruthless, PrefixID.Zealous, PrefixID.Quick, PrefixID.Taboo, PrefixID.Manic },
                        /* 2 */ new int[] { PrefixID.Murderous, PrefixID.Agile, PrefixID.Adept, PrefixID.Celestial, PrefixID.Unpleasant },
                        /* 3 */ new int[] { PrefixID.Superior, PrefixID.Demonic, PrefixID.Mystic },
                        /* 4 */ new int[] { PrefixID.Godly, PrefixID.Masterful, PrefixID.Deadly2 },
                        /* 5 */ new int[] { PrefixID.Mythical }
                    };
                    prefix = IteratePrefix(rand, magicReforgeTiers, currentPrefix);
                }
            }

            //BARD
            else if (item.CountsAsClass<BardDamage>())
            {
                int[][] bardReforgeTiers = new int[][]
                {
                    /* 0 */ new int[] { GetThorPrefix("Muted"), GetThorPrefix("OffKey"), GetThorPrefix("Rambling") },
                    /* 1 */ new int[] { GetThorPrefix("Buzzing"), GetThorPrefix("Refined"), GetThorPrefix("Loud") },
                    /* 2 */ new int[] { GetThorPrefix("Supersonic"), GetThorPrefix("Vibrant"), GetThorPrefix("Euphonic"), GetThorPrefix("Inspiring") },
                    /* 3 */ new int[] { GetThorPrefix("Melodic") },
                    /* 4 */ new int[] { GetThorPrefix("Fabled") }
                };
                prefix = IteratePrefix(rand, bardReforgeTiers, currentPrefix);
            }

            return prefix;
        }

        private static int GetPrefixTier(int[][] tiers, int currentPrefix)
        {
            for (int checkingTier = 0; checkingTier < tiers.Length; ++checkingTier)
            {
                int[] tierList = tiers[checkingTier];
                for (int i = 0; i < tierList.Length; ++i)
                    if (tierList[i] == currentPrefix)
                        return checkingTier;
            }

            // If an invalid or modded prefix is detected, return -1.
            // This will give a random tier 0 prefix (the "next tier"), starting fresh with a low-tier vanilla or Calamity prefix.
            return -1;
        }

        private static int IteratePrefix(UnifiedRandom rand, int[][] reforgeTiers, int currentPrefix)
        {
            int currentTier = GetPrefixTier(reforgeTiers, currentPrefix);

            // If max tier: give max tier reforges forever
            // Otherwise: go up by 1 tier with every reforge, guaranteed
            int newTier = currentTier == reforgeTiers.Length - 1 ? currentTier : currentTier + 1;
            return rand.Next(reforgeTiers[newTier]);
        }
    }
}
