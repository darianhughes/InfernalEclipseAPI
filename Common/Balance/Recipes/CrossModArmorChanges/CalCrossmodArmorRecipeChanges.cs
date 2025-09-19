using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.GameContent;

namespace InfernalEclipseAPI.Common.Balance.Recipes.CrossModArmorChanges
{
    public class CalCrossmodArmorRecipeChanges : ModSystem
    {
        public override void PostAddRecipes()
        {
            ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok);
            ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHealer);

            if (ragnarok != null && InfernalConfig.Instance.DisableUnfinisedContent)
            {
                int[] nightFallen =
                {
                    GetItemID("NightfallenBreastplate", ragnarok),
                    GetItemID("NightfallenGreaves", ragnarok),
                    GetItemID("NightfallenHelmet", ragnarok)
                };

                foreach (var recipe in Main.recipe)
                {
                    foreach (int itemID in nightFallen)
                    {
                        if (recipe.HasResult(itemID))
                        {
                            recipe.DisableRecipe();
                        }
                    }
                }
            }

                if (ragnarok != null && calBardHealer != null)
            {
                int[] ragnarokArmor =
                {
                   GetItemID("AerospecBard", ragnarok),
                   GetItemID("AerospecHealer", ragnarok),
                   GetItemID("AuricTeslaFrilledHelmet", ragnarok),
                   GetItemID("AuricTeslaHealerHead", ragnarok),
                   GetItemID("BloodflareHeadBard", ragnarok),
                   GetItemID("BloodflareHeadHealer", ragnarok),
                   GetItemID("DaedalusHeadBard", ragnarok),
                   GetItemID("DaedalusHeadHealer", ragnarok),
                   GetItemID("GodSlayerHeadBard", ragnarok),
                   GetItemID("SilvaHeadHealer", ragnarok),
                   GetItemID("StatigelHeadBard", ragnarok),
                   GetItemID("StatigelHeadHealer", ragnarok),
                   GetItemID("TarragonCowl", ragnarok),
                   GetItemID("TarragonShroud", ragnarok),
                   GetItemID("VictideHeadBard",ragnarok)
                };

                int[] calBardHealerArmor =
                {
                    GetItemID("AerospecHeadphones", calBardHealer),
                    GetItemID("AerospecBiretta", calBardHealer),
                    GetItemID("AuricTeslaFeatheredHeadwear", calBardHealer),
                    GetItemID("AuricTeslaValkyrieVisage", calBardHealer),
                    GetItemID("BloodflareSirenSkull", calBardHealer),
                    GetItemID("BloodflareRitualistMask", calBardHealer),
                    GetItemID("DaedalusHat", calBardHealer),
                    GetItemID("DaedalusCowl", calBardHealer),
                    GetItemID("GodSlayerDeathsingerCowl", calBardHealer),
                    GetItemID("SilvaGuardianHelmet", calBardHealer),
                    GetItemID("StatigelEarrings", calBardHealer),
                    GetItemID("StatigelFoxMask", calBardHealer),
                    GetItemID("TarragonParagonCrown", calBardHealer),
                    GetItemID("TarragonChapeau", calBardHealer),
                    GetItemID("VictideAmmoniteHat", calBardHealer)
                };

                for (int i = 0; i < ragnarokArmor.Length; i++)
                {
                    int ragID = ragnarokArmor[i];
                    int calID = calBardHealerArmor[i];

                    if (ragID == 0 || calID == 0)
                    {
                        continue;
                    }

                    Recipe originalRecipe = null;

                    // Disable ragnarok item recipe
                    foreach (var recipe in Main.recipe)
                    {
                        if (recipe.createItem.type == ragID)
                        {
                            originalRecipe = recipe;
                            recipe.DisableRecipe();
                            break;
                        }
                    }

                    int originalTile = originalRecipe?.requiredTile.Count > 0 ? originalRecipe.requiredTile[0] : TileID.Anvils; // fallback to something valid

                    // Forward: Ragnarok -> CalamityBH
                    Recipe forward = Recipe.Create(calID);
                    forward.AddIngredient(ragID);
                    forward.AddCondition(Condition.InGraveyard);
                    if (originalTile != TileID.Anvils)
                        forward.AddTile(originalTile);
                    forward.DisableDecraft();
                    forward.Register();

                    // Reverse: CalamityBH -> Ragnarok
                    Recipe reverse = Recipe.Create(ragID);
                    reverse.AddIngredient(calID);
                    reverse.AddCondition(Condition.InGraveyard);
                    if (originalTile != TileID.Anvils)
                        reverse.AddTile(originalTile);
                    reverse.DisableDecraft();
                    reverse.Register();
                }
            }
        }

        private int GetItemID(string name, Mod mod)
        {
            
            if (mod.TryFind(name, out ModItem item))
                return item.Type;
            return 0;
        }
    }
}
