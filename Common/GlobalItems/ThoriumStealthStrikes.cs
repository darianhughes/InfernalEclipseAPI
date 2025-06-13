using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer;
using InfernalEclipseAPI.Common.GlobalProjectiles;
using InfernalEclipseAPI.Core.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class ThoriumStealthStrikes : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var calPlayer = player.GetModPlayer<CalamityPlayer>();

            // ===================== ICY TOMAHAWK =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Icy Tomahawk")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    Vector2[] velocities = {
                        velocity * 0.33f,
                        velocity * 0.66f,
                        velocity,
                        velocity * 1.33f,
                    };

                    foreach (Vector2 v in velocities)
                    {
                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            v,
                            type,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.IcyTomahawk);
                        }
                    }

                    return false;
                }
            }

            // ===================== CACTUS NEEDLE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Cactus Needle")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    float spread = MathHelper.ToRadians(5f);

                    for (int i = -1; i <= 1; i++) // 3 projectiles: left, center, right
                    {
                        float rotation = spread * i;
                        Vector2 newVelocity = velocity.RotatedBy(rotation) * 1.75f;

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            newVelocity,
                            type,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.CactusNeedle);
                        }
                    }

                    return false;
                }
            }

            // ===================== ZEPHYR'S RUIN =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumRework" && item.Name == "Zephyr's Ruin")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    velocity *= 1.75f;
                    damage = (int)(damage * 3.5f);

                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ZephyrsRuin);
                    }

                    return false;
                }
            }

            return true;
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

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Icy Tomahawk")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes throw out 4 tomahawks of different speeds that last longer and pierce more")
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
