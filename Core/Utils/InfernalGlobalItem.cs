using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;
using ThoriumMod;

namespace InfernalEclipseAPI.Core.Utils
{
    [ExtendsFromMod("ThoriumMod")]
    public class InfernalThoriumGlobalItem : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            storedPrefix = -1;
        }

        private static int storedPrefix = -1;

        public override void PreReforge(Item item)
        {
            storedPrefix = item.prefix;
        }

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if (!item.CountsAsClass<HealerDamage>() && !item.CountsAsClass<HealerTool>() && !item.CountsAsClass<HealerToolDamageHybrid>() && !item.CountsAsClass<BardDamage>()) return -1;
            if (!CalamityConfig.Instance.RemoveReforgeRNG || Main.gameMenu || storedPrefix == -1) return -1;

            return ThoriumItemUtils.GetReworkedReforge(item, rand, storedPrefix);
        }

        public override void PostReforge(Item item)
        {
            storedPrefix = -1;
        }
    }
}
