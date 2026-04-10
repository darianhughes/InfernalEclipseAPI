using CalamityMod.UI.CalamitasEnchants;
using InfernalEclipseAPI.Content.Items.Accessories;
using InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse;
using InfernalEclipseAPI.Content.Items.SpawnItems;
using InfernalEclipseAPI.Content.Items.Weapons.Donor.Steetsign;

namespace InfernalEclipseAPI.Core.Systems;

public class ExhumedItems : ModSystem
{
    public override void OnModLoad()
    {
        var dict = EnchantmentManager.ItemUpgradeRelationship;

        dict[ModContent.ItemType<LoreProvi>()] = ModContent.ItemType<MysteriousDiary>();
        dict[ModContent.ItemType<EmptyDemonicTome>()] = ModContent.ItemType<DemonicTome>();
        dict[ItemID.Sign] = ModContent.ItemType<Streetsign>();

        if (InfernalCrossmod.Clamity.Loaded)
        {
            dict[InfernalCrossmod.Clamity.Mod.Find<ModItem>("TheSubcommunity").Type] = ModContent.ItemType<ShatteredSubcommunity>();
        }
    }
}