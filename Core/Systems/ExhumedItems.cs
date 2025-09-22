
using InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse;
using InfernalEclipseAPI.Content.Items.SpawnItems;

namespace InfernalEclipseAPI.Core.Systems;

public class ExhumedItems : ModSystem
{
    public override void AddRecipes()
    {
        if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
        {
            calamity.Call(["MakeItemExhumable", ModContent.ItemType<LoreProvi>(), ModContent.ItemType<MysteriousDiary>()]);
            calamity.Call(["MakeItemExhumable", ModContent.ItemType<EmptyDemonicTome>(), ModContent.ItemType<DemonicTome>()]);
        }
    }
}