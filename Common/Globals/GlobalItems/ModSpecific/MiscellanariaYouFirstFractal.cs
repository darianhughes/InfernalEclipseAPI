using InfernalEclipseAPI.Core.Systems;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    public class MiscellanariaYouFirstFractal : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalCrossmod.YouBoss.Loaded && ModLoader.HasMod("Miscellanaria");
        }

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.FirstFractal;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Deprecated[ItemID.FirstFractal] = true;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Zenith] = -1;
        }
    }
}
