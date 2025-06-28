using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.ThoriumStealthStrikes
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
        CaptainsPoignard,
        SoftServeSunderer,
        TerraKnife,
        TerraKnife2,
        ShadeShuriken,
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
                    damage = (int)(damage * 2f);

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

            // ===================== SHADE SHURIKEN =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Shade Shuriken")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                        thorium.TryFind("ShadeShuriken", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            velocity * 1.0f, // Faster
                            projType,
                            (int)(damage * 1.0f), // Stronger
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) &&
                            Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ShadeShuriken);
                        }
                    }

                    return false;
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

            // ===================== SOFT SERVE SUNDERER =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Soft Serve Sunderer")
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
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.SoftServeSunderer);
                    }

                    return false;
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

                        if (Main.projectile.IndexInRange(upProjID) &&
                            Main.projectile[upProjID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile upStealthGlobal))
                        {
                            upStealthGlobal.SetupAsStealthStrike(StealthStrikeType.WhiteDwarfCutter);
                        }

                        if (Main.projectile.IndexInRange(downProjID) &&
                            Main.projectile[downProjID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile downStealthGlobal))
                        {
                            downStealthGlobal.SetupAsStealthStrike(StealthStrikeType.WhiteDwarfCutter);
                        }
                    }

                    return false;
                }
            }

            // ===================== TERRA KNIFE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Terra Knife")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                    thorium.TryFind("TerraKnifePro", out ModProjectile mainPro);
                    thorium.TryFind("TerraKnifePro2", out ModProjectile sidePro);

                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        sidePro.Type,
                        (int)(damage * 0.8),
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.TerraKnife);
                    }

                    int sideProjType = mainPro.Type;

                    Vector2 velocityUp = velocity.RotatedBy(MathHelper.ToRadians(5f));
                    Vector2 velocityDown = velocity.RotatedBy(MathHelper.ToRadians(-5f));

                    int upProjID = Projectile.NewProjectile(source, position, velocityUp, sideProjType, damage, knockback, player.whoAmI);
                    int downProjID = Projectile.NewProjectile(source, position, velocityDown, sideProjType, damage, knockback, player.whoAmI);

                    if (Main.projectile.IndexInRange(upProjID) &&
                        Main.projectile[upProjID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile upStealthGlobal))
                    {
                        upStealthGlobal.SetupAsStealthStrike(StealthStrikeType.TerraKnife2);
                    }

                    if (Main.projectile.IndexInRange(downProjID) &&
                        Main.projectile[downProjID].TryGetGlobalProjectile(out StealthStrikeGlobalProjectile downStealthGlobal))
                    {
                        downStealthGlobal.SetupAsStealthStrike(StealthStrikeType.TerraKnife2);
                    }

                    return false;
                }
            }

            return true;
        }

        public override void SetDefaults(Item item)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) return;
            //EXHAUSTION REMOVAL

            //TERRA KNIFE
            if (item.type == thorium.Find<ModItem>("TerraKnife").Type)
            {
                TrySetIsThrowerNon(item, false);
            }
        }

        private void TrySetIsThrowerNon(Item item, bool active)
        {
            try
            {
                if (item.ModItem == null)
                {
                    Main.NewText("No ModItem attached");
                    return;
                }

                Type modItemType = item.ModItem.GetType();

                // Try field first
                FieldInfo field = modItemType.GetField("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(item.ModItem, active);
                    //Main.NewText($"[Field] Set healAmount of {item.Name} to {newCost}");
                    return;
                }

                // Then try property
                PropertyInfo prop = modItemType.GetProperty("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item.ModItem, active);
                    return;
                }

                //Main.NewText("healAmount not found on ModItem.");
            }
            catch (Exception)
            {
                //Main.NewText($"Error setting healAmount: {ex.Message}");
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
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "During a stealth strike each hit creates Ivory flares that damage for 0.1% of the target's max HP")
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

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Soft Serve Sunderer")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes summon a rain of cones from above the hit enemy")
                {
                    OverrideColor = Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.Name == "Shade Shuriken")
            {
                tooltips.Add(new TooltipLine(Mod, "CustomStealthStrikes", "Stealth strikes throw a larger, faster shuriken that has increased pierce")
                {
                    OverrideColor = Color.White
                });
            }
        }
    }
}