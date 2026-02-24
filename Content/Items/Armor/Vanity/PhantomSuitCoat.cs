using System.Collections.Generic;
using CalamityMod;
using InfernumMode;
using InfernumMode.Content.Rarities.InfernumRarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class PhantomSuitCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.value = 10000;
            Item.rare = ModContent.RarityType<InfernumRedSparkRarity>();
            if (ModLoader.TryGetMod("NoxusBoss", out Mod noxus))
            {
                ModRarity r;
                noxus.TryFind("NamelessDeityRarity", out r);
                Item.rare = r.Type;
            }
            Item.vanity = true;

            Item.Infernum_Tooltips().DeveloperItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color color = CalamityUtils.ColorSwap(Color.OrangeRed, Color.DarkRed, 2f);

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.Dedicated.Akira"))}");
                line5.OverrideColor = color;
                tooltips.Add(line5);
            }
        }
    }
}
