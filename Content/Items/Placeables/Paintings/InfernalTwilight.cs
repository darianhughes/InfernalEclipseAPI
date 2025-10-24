using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using InfernalEclipseAPI.Content.Tiles.Paintings;

namespace InfernalEclipseAPI.Content.Items.Placeables.Paintings
{
    public class InfernalTwilight : ModItem
    {
        public override string Texture => "InfernalEclipseAPI/icon";
        public override void SetDefaults()
        {
            Item.width = Item.height = 80;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0); ;
            Item.rare = ItemRarityID.White;
            if (ModLoader.TryGetMod("NoxusBoss", out Mod noxus) && noxus.TryFind("NamelessDeityRarity", out ModRarity r))
                Item.rare = r.Type;
            Item.createTile = ModContent.TileType<InfernalTwilightTile>();
            Item.Calamity().donorItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                return;

            string tooltip = "";

            int namesPerLine = 8;
            for (int i = 0; i < devList.Count; i++)
            {
                tooltip += devList[i];

                if (i == devList.Count - 1)
                    break;

                if (i % namesPerLine == 0 && i != 0)
                    tooltip += "\n";
                else
                    tooltip += ", ";
            }
            tooltip += "\n";

            int teamsPerLine = 3;
            for (int i = 0; i < teamList.Count; i++)
            {
                tooltip += teamList[i];

                if (i == teamList.Count - 1)
                    break;

                if (i % teamsPerLine == 0 && i != 0) 
                    tooltip += "\n";
                else 
                    tooltip += ", ";
            }

            TooltipLine line = tooltips.FirstOrDefault(t => t.Mod == "Terraria" && t.Name == "Tooltip2");
            if (line != null)
                line.Text = tooltip;
        }

        public static IList<string> devList = new List<string>()
        {
            //Developers
            "Akira",
            "WardrobeHummus",
            "Advantaje",

            //Designers
            "Tyr",
            "Yob",
            "Soltan",

            //Management
            "Kami",
            "TheRogueX",

            //Major Contributors
            "StarlightCat",
            "Ropro0923",
            "cheesenuggets",
            "Prime",
            "Mr. Puzzles",
            "Dandel",
            "ma3allim",
            "Nobody",
            "Peter Fung",
            "Pudd1ng",

            //Other
            "Kes",
            "KaiTheExaminer",
            "konte1m",
            "LeEr206",
            "Pil",
            "Pixus",
            "Yelmut",
            "Aizen522",
            "Suu",
            "fiend",

            //Playtesters
            "Tencvin",
            "Jillyfish",

            //Donors
            "rosé",

            //Special Thanks
            "N0t_UNowen",
            "javyz",
            "Lucille Karma",
            "Nycro",
            "brome",
            "Cataclysmic Armageddon"
        };

        public static IList<string> teamList = new List<string>()
        {
            "Calamity Mod Team",
            "Fargo Team",
            "Thorium Team",
            "Secrets of the Shadows Team",
            "Consolaria Team",
            //"Heartbeataria Team",
            "Ragnarok Development Team",
            "Bereft Souls Team"
        };
    }
}
