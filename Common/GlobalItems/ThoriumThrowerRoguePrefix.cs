using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using ThoriumMod;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class ThoriumThrowerRoguePrefix : GlobalItem
    {
        //public static List<int> RoguePrefixes;

        //public override void Load()
        //{
        //    RoguePrefixes = new List<int>();

        //    // Try to add Calamity rogue prefixes if loaded
        //    if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
        //    {
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Radical").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Pointy").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Sharp").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Glorious").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Feathered").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Sleek").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Hefty").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Mighty").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Serrated").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Vicious").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Lethal").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Flawless").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Blunt").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Flimsy").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Unbalanced").Type);
        //        RoguePrefixes.Add(calamity.Find<ModPrefix>("Atrocious").Type);
        //    }

        //    // Add vanilla rogue-compatible prefixes
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Keen);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Superior);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Forceful);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Broken);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Damaged);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Hurtful);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Strong);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Unpleasant);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Weak);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Ruthless);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Godly);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Demonic);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Zealous);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Quick);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Deadly2);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Agile);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Nimble);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Murderous);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Slow);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Sluggish);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Lazy);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Annoying);
        //    RoguePrefixes.Add(Terraria.ID.PrefixID.Nasty);
        //}
        //public override int ChoosePrefix(Item item, UnifiedRandom rand)
        //{
        //    if ((item.DamageType == DamageClass.Throwing || item.CountsAsClass<RogueDamageClass>()) && !item.consumable && item.maxStack == 1 && item.damage > 0)
        //    {
        //        // If Calamity is loaded, use Calamity's rogue prefix logic
        //        if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
        //        {
        //            Type calamityUtils = calamity.Code.GetType("CalamityMod.CalamityUtils");
        //            if (calamityUtils != null)
        //            {
        //                MethodInfo randomRoguePrefix = calamityUtils.GetMethod("RandomRoguePrefix", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        //                MethodInfo negativeRoguePrefix = calamityUtils.GetMethod("NegativeRoguePrefix", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        //                if (randomRoguePrefix != null && negativeRoguePrefix != null)
        //                {
        //                    int prefix = (int)randomRoguePrefix.Invoke(null, null);
        //                    bool isNegative = (bool)negativeRoguePrefix.Invoke(null, new object[] { prefix });

        //                    // Annoying is always voided in Calamity
        //                    bool isAnnoying = prefix == Terraria.ID.PrefixID.Annoying;

        //                    // Negative prefixes have a 2/3 chance to be voided (skipped)
        //                    if (!isNegative || rand.Next(3) == 0)
        //                    {
        //                        if (!isAnnoying)
        //                            return prefix;
        //                    }
        //                    // Otherwise, reroll to "no prefix"
        //                    return 0;
        //                }
        //            }
        //        }

        //        // If Calamity not loaded, just select random allowed rogue prefix
        //        if (RoguePrefixes != null && RoguePrefixes.Count > 0)
        //        {
        //            int prefix = RoguePrefixes[rand.Next(RoguePrefixes.Count)];
        //            return prefix;
        //        }
        //    }
        //    return -1;
        //}
    }
}
