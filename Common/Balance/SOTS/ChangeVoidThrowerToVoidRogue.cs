using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using ThoriumMod.Items;
using CalamityMod;
using InfernalEclipseAPI.Core.DamageClasses;

namespace InfernalEclipseAPI.Common.Balance.SOTS
{
    public class ChangeVoidThrowerToVoidRogue : GlobalItem
    {
        public Mod SOTSBardHealer
        {
            get
            {
                ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsBardHeal);
                return sotsBardHeal;
            }
        }

        public Mod SOTS
        {
            get
            {
                ModLoader.TryGetMod("SOTS", out Mod sotsBardHeal);
                return sotsBardHeal;
            }
        }

        public override void SetDefaults(Item item)
        {
            if (SOTSBardHealer != null && InfernalConfig.Instance.SOTSThrowerToRogue)
            {
                if ((item.type == SOTSBardHealer.Find<ModItem>("ForbiddenMaelstrom").Type) || (item.type == SOTSBardHealer.Find<ModItem>("GoopwoodSplit").Type))
                {
                    item.DamageType = ModContent.GetInstance<VoidRogue>();
                    if (InfernalConfig.Instance.SOTSBalanceChanges)
                        item.damage = 30;
                }
                if (item.type == SOTSBardHealer.Find<ModItem>("GoopwoodSplit").Type)
                {
                    item.DamageType = ModContent.GetInstance<VoidRogue>();
                }
            }

            if (SOTS != null)
            {
                if (item.type == SOTS.Find<ModItem>("GelAxe").Type)
                {
                    item.DamageType = ModContent.GetInstance<RogueDamageClass>();
                }
            }
        }
    }
}
