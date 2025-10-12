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
                if ((item.type == SOTSBardHealer.Find<ModItem>("ForbiddenMaelstrom").Type) || (item.type == SOTSBardHealer.Find<ModItem>("GoopwoodSplit").Type) || (item.type == SOTSBardHealer.Find<ModItem>("Serpentbite").Type))
                {
                    item.DamageType = ModContent.GetInstance<VoidRogue>();
                }
            }
        }
    }
}
