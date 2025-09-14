using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using InfernalEclipseAPI.Content.Items.Lore.InfernalEclipse;
using InfernalEclipseAPI.Core.Players;
using Terraria.Localization;

namespace InfernalEclipseAPI.Content.Items.Accessories.RingofTix
{
    public class RingofTix : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.accessory = true;
            Item.defense = 12;
            Item.lifeRegen = 1;
            Item.Calamity().devItem = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.darkSunRing = true;

            player.GetDamage(DamageClass.Generic).Flat += 4;
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            ref StatModifier local = ref player.GetDamage(DamageClass.Generic);
            local += 0.06f;
            player.GetCritChance(DamageClass.Generic) += 4;

            player.lifeRegen += 3;
            player.luck += 0.25f;

            InfernalPlayer infernal = player.GetModPlayer<InfernalPlayer>();
            infernal.HarvestMoonBuff = true;

            //Challenger Ring handled in GlobalItem
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Always show the normal stats tooltip.
            tooltips.Add(new TooltipLine(Mod, "NormalTip",
                Language.GetTextValue("Mods.InfernalEclipseAPI.Items.RingofTix.NormalTooltip")));

            // Show SOTS-specific usage text only if SOTS is enabled.
            if (ModLoader.TryGetMod("SOTS", out _))
                tooltips.Add(new TooltipLine(Mod, "SOTSTip",
                    Language.GetTextValue("Mods.InfernalEclipseAPI.Items.RingofTix.SOTSTooltip")));
        }

        public override void AddRecipes()
        {
            Recipe tixRing = Recipe.Create(ModContent.ItemType<RingofTix>());
            tixRing.AddIngredient<DarkSunRing>();
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                tixRing.AddIngredient(thorium.Find<ModItem>("TheRing"));
            if (ModLoader.TryGetMod("BlueMoon", out Mod moons))
                tixRing.AddIngredient(moons.Find<ModItem>("MoonsRing"));
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
                tixRing.AddIngredient(sots.Find<ModItem>("ChallengerRing"));
            tixRing.AddIngredient<ShadowspecBar>(5);
            tixRing.AddIngredient<LoreDylan>();
            tixRing.AddTile<DraedonsForge>();
            tixRing.Register();
        }
    }

    public class TixGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];
            InfernalPlayer mp = player.GetModPlayer<InfernalPlayer>();
            if (mp.HarvestMoonBuff) npc.value += 2f;
        }
    }
}
