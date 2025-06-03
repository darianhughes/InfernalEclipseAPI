using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer;
using InfernalEclipseAPI.Common.GlobalProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class ThoriumStealthStrikes : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity,
            ref int type, ref int damage, ref float knockback)
        {
            var calPlayer = player.GetModPlayer<CalamityPlayer>();

            // ===================== CACTUS NEEDLE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Cactus Needle")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    float spread = MathHelper.ToRadians(5f);

                    for (int i = 0; i < 2; i++)
                    {
                        float rotation = spread * (i * 2 - 1);
                        Vector2 newVelocity = velocity.RotatedBy(rotation) * 1.75f;
                        Projectile.NewProjectile(
                            player.GetSource_ItemUse(item),
                            position,
                            newVelocity,
                            type,
                            damage,
                            knockback,
                            player.whoAmI
                        );
                    }
                }
            }

            // ===================== ZEPHYR'S RUIN =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumRework" && item.Name == "Zephyr's Ruin")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    damage = (int)(damage * 3.5);
                    velocity *= 1.75f;

                    // pinging global projectile
                    ThoriumStealthStrikeProjectiles.StealthStrikeFromPlayer[player.whoAmI] = true;
                }
            }
        }

        //TOOLTIPS
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Cactus Needle")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes throw out needles in a tight fan of 3")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumRework" && item.Name == "Zephyr's Ruin")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes throw a stronger spear that always crits")
                {
                    OverrideColor = Color.White
                });
            }
        }
    }
}
