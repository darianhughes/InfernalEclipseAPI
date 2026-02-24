namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    public class CalamityAmmoGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return InfernalConfig.Instance.CalamityBalanceChanges;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            if (!ModLoader.TryGetMod("CalamityAmmo", out Mod calamityAmmo))
                return;

            if (calamityAmmo.TryFind("WulfrumCoil", out ModItem coil))
            {
                player.GetDamage(DamageClass.Ranged) -= 0.02f;
            }
        }
    }
}
