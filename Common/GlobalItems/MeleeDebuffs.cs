using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    //Wardrobe Hummus
    public class MeleeDebuffs : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }

        public override bool InstancePerEntity => true;

        public override void OnHitNPC(
            Item item,
            Player player,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                var thunderTalon = thoriumMod.Find<ModItem>("ThunderTalon");
                if (thunderTalon != null && item.type == thunderTalon.Type)
                {
                    target.AddBuff(144, 180); // Electrified
                }

                if (item.type == ModLoader.GetMod("ThoriumMod")?.Find<ModItem>("LifeQuartzClaymore")?.Type)
                {
                    HealPlayer(player, 2);
                    return;
                }

                // Thorium HereticBreaker (additive: does NOT remove original effects)
                if (item.type == ModLoader.GetMod("ThoriumMod")?.Find<ModItem>("HereticBreaker")?.Type)
                {
                    HealPlayer(player, 3);
                }
            }

            if (item.type == ItemID.LucyTheAxe && InfernalConfig.Instance.VanillaBalanceChanges)
            {
                target.AddBuff(ModContent.BuffType<Crumbling>(), 180);
            }
        }

        private void HealPlayer(Player player, int healAmount)
        {
            player.statLife += healAmount;
            player.HealEffect(healAmount, true); // true = play heal effect number popup
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModLoader.GetMod("ThoriumMod")?.Find<ModItem>("LifeQuartzClaymore")?.Type)
            {
                // Remove the existing tooltip
                tooltips.RemoveAll(t => t.Text.Contains("Steals 1 life"));

                // Add your custom tooltip
                tooltips.Add(new TooltipLine(Mod, "CustomTooltip", "Steals 3 life"));
            }
        }
    }
}
