using System.Collections.Generic;
using Terraria.Localization;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Content.Items.Accessories.ExoSights
{
    [ExtendsFromMod("ThoriumMod", "SOTS")]
    public class ExoSightsGlobal : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<ExoSights>();
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            player.GetThoriumPlayer().accInfernoLordsFocus = true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
           if (ThoriumConfigClient.Instance.ShowAccessoryDamage)
           {
                int index = tooltips.FindIndex((Predicate<TooltipLine>)(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Tooltip0")));
                if (index != -1)
                    tooltips.Insert(index, new TooltipLine(Mod, "AccessoryDamage", $"15% {Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.BasicDamage")}"));
           }
        }
    }
}
