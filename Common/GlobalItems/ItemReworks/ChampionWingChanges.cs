namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    //WHummus
    public class ChampionWingChanges : GlobalItem
    {
        private Mod thorium
        {
            get
            {
                ModLoader.TryGetMod("ThoriumMod", out Mod thor);
                return thor;
            }
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.HasMod("WHummusMultiModBalancing");
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (thorium == null) return;

            if (item.ModItem != null &&
                item.ModItem.Mod.Name == "ThoriumMod" &&
                item.ModItem.Name == "ChampionWing")
            {
                player.GetDamage(DamageClass.Generic) -= 0.10f;
            }
        }
    }
}
