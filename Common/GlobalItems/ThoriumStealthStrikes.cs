using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer;
using InfernalEclipseAPI.Common.GlobalProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public enum StealthStrikeType
    {
        None,
        CactusNeedle,
        IcyTomahawk,
        ZephyrsRuin,
        ClockworkBomb,
        SoulBomb,
        PlayingCard,
        WhiteDwarfCutter,
        Soulslasher,
        CaptainsPoignard
    }
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
                        Vector2 newVelocity = velocity.RotatedBy(rotation);

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

            // ===================== CAPTAIN'S POIGNARD =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Captain's Poignard")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.CaptainsPoignard);
                    }

                    return false; // prevent original spawn, we spawned manually
                }
            }




            // ===================== CLOCKWORK BOMB =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Clockwork Bomb")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
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
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ClockworkBomb);
                    }

                    return false;
                }
            }

            // ===================== SOUL BOMB =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Soul Bomb")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
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
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.SoulBomb);
                    }

                    return false;
                }
            }

            // ===================== PLAYING CARD =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Playing Card")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int count = 5;
                    float spread = MathHelper.ToRadians(10f);
                    float baseAngle = velocity.ToRotation();
                    float startAngle = baseAngle - spread / 2f;

                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) &&
                        thoriumMod.TryFind("MagicCardPro", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        for (int i = 0; i < count; i++)
                        {
                            float rotation = MathHelper.ToRadians(15f) * (i - 1.5f);
                            Vector2 rotatedVelocity = velocity.RotatedBy(rotation);

                            int adjustedDamage = (int)Math.Round(damage * 1.15);

                            int projID = Projectile.NewProjectile(
                                source,
                                position,
                                rotatedVelocity,
                                projType,
                                adjustedDamage,
                                knockback,
                                player.whoAmI,
                                4,
                                1f // force explosive version
                            );

                            if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                            {
                                stealthGlobal.SetupAsStealthStrike(StealthStrikeType.PlayingCard);
                            }
                        }

                        return false;
                    }
                }
            }



            // ===================== SOULSLASHER =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Soulslasher")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) &&
                        thoriumMod.TryFind("SoulslasherPro", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            velocity,
                            projType,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) &&
                            Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.Soulslasher);
                        }

                        return false; // Prevent default projectile
                    }
                }
            }


            // ===================== WHITE DWARF CUTTER =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "White Dwarf Cutter")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    // Fire the main kunai
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.WhiteDwarfCutter);
                    }

                    // Spawn the two angled kunai
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                        thorium.TryFind("WhiteDwarfKunaiPro2", out ModProjectile sideProjMod))
                    {
                        int sideProjType = sideProjMod.Type;

                        Vector2 velocityUp = velocity.RotatedBy(MathHelper.ToRadians(5f));
                        Vector2 velocityDown = velocity.RotatedBy(MathHelper.ToRadians(-5f));

                        int upProjID = Projectile.NewProjectile(source, position, velocityUp, sideProjType, damage, knockback, player.whoAmI);
                        int downProjID = Projectile.NewProjectile(source, position, velocityDown, sideProjType, damage, knockback, player.whoAmI);
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

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Clockwork Bomb")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes create a larger field that lasts longer")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Soul Bomb")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes release a burst of homing souls on detonation")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Playing Card")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes throw 5 homing cards that are always explosive")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Soulslasher")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes will also aggressively home after hitting an enemy")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "White Dwarf Cutter")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "During a stealth strike each hit from the main knife creates Ivory flares that damage for 0.1% of the target's max HP")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Captain's Poignard")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes throw a burst of 6 daggers and gives the player increased attack speed for 10 seconds")
                {
                    OverrideColor = Color.White
                });
            }
        }
    }
}