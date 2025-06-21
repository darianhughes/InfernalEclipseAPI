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
        public override void SetDefaults(Item item)
        {
            if (ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsBardHeal) && InfernalConfig.Instance.SOTSThrowerToRogue)
            {
                if ((item.type == sotsBardHeal.Find<ModItem>("ForbiddenMaelstrom").Type) || (item.type == sotsBardHeal.Find<ModItem>("GoopwoodSplit").Type))
                {
                    item.DamageType = ModContent.GetInstance<VoidRogue>();
                    if (InfernalConfig.Instance.SOTSBalanceChanges)
                        item.damage = 30;
                }
                if (item.type == sotsBardHeal.Find<ModItem>("GoopwoodSplit").Type)
                {
                    item.DamageType = ModContent.GetInstance<VoidRogue>();
                }
            }
        }
    }
}
