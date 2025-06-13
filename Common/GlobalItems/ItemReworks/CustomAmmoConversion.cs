using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    public class CustomAmmoConversion : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            if (ModLoader.TryGetMod("WHummusMultiModBalancing", out _))
                return false;
            return true;
        }
        private static int GetModdedProjectile(string modName, string projName)
        {
            if (ModLoader.TryGetMod(modName, out Mod mod) && mod.TryFind(projName, out ModProjectile modProjectile))
            {
                return modProjectile.Type;
            }
            return ProjectileID.WoodenArrowFriendly;
        }

        private static readonly Dictionary<(string modName, string itemName), int> ArrowConversions = new();

        // Counter to track alternating shots (per item ID)
        private static readonly Dictionary<int, int> shotCounters = new();

        static CustomAmmoConversion()
        {
            ArrowConversions.Add(("ThoriumMod", "FrostFury"), ProjectileID.FrostburnArrow);
            ArrowConversions.Add(("ThoriumMod", "StreamSting"), ProjectileID.BoneArrow);
            ArrowConversions.Add(("ThoriumMod", "CometCrossfire"), ProjectileID.JestersArrow);
            ArrowConversions.Add(("ThoriumMod", "SteelBow"), GetModdedProjectile("ThoriumMod", "SteelArrow"));
            ArrowConversions.Add(("ThoriumMod", "eSandStoneBow"), GetModdedProjectile("ThoriumMod", "TalonArrowPro"));
            ArrowConversions.Add(("ThoriumMod", "DurasteelRepeater"), GetModdedProjectile("ThoriumMod", "DurasteelArrow"));

            if (ModLoader.TryGetMod("CalamityMod", out _))
            {
                ArrowConversions.Add(("ThoriumMod", "Trigun"), GetModdedProjectile("CalamityMod", "HallowPointRoundProj"));
            }
        }

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return true; // needed for general logic + Mycelium shot tracking
        }

        //MYCELIUMGATLINGGUN REWORK
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            string modName = item.ModItem?.Mod.Name;
            string itemName = item.ModItem?.Name;

            if (modName == "ThoriumMod" && itemName == "MyceliumGattlingGun")
            {
                int itemId = item.type;

                int shroomBolt = GetModdedProjectile("ThoriumMod", "ShroomBolt");
                int fungiOrb = ModLoader.TryGetMod("CalamityMod", out _) ? GetModdedProjectile("CalamityMod", "FungiOrb") : shroomBolt;

                if (!shotCounters.ContainsKey(itemId))
                    shotCounters[itemId] = 0;

                int counter = shotCounters[itemId];

                // Use current counter to determine shot type
                bool isFungiOrbShot = counter % 4 == 3;

                type = isFungiOrbShot ? fungiOrb : shroomBolt;

                // Increment counter AFTER deciding projectile type
                shotCounters[itemId] = (counter + 1) % 4;

                return;
            }

            // General arrow/bullet conversion logic
            if (modName != null && itemName != null && ArrowConversions.TryGetValue((modName, itemName), out int newProjectile))
            {
                if (type == ProjectileID.WoodenArrowFriendly || type == ProjectileID.Bullet)
                {
                    type = newProjectile;
                }
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.ModItem?.Mod?.Name == "ThoriumMod" && item.ModItem.Name == "MyceliumGattlingGun")
            {
                int itemId = item.type;

                if (!shotCounters.TryGetValue(itemId, out int counter))
                    return true;

                // The current shot was decided BEFORE counter increment, so actual shot index is (counter - 1)
                int shotIndex = (counter + 3) % 4; // same as counter-1 mod 4 but avoiding negative

                bool isFungiOrbShot = shotIndex == 3;

                if (isFungiOrbShot)
                {
                    int shroomBolt = GetModdedProjectile("ThoriumMod", "ShroomBolt");

                    // Spawn extra shroom bolt projectile
                    Projectile.NewProjectile(source, position, velocity, shroomBolt, damage, knockback, player.whoAmI);

                    // Play fungi orb sound immediately
                    if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) &&
                        calamity.TryFind("Fungicide", out ModItem fungicideItem) &&
                        fungicideItem.Item.UseSound is SoundStyle calSound)
                    {
                        SoundEngine.PlaySound(calSound, player.position);
                    }
                }
                else
                {
                    // Play normal shot sound immediately
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                        thorium.TryFind("MyceliumGattlingGun", out ModItem thoriumItem) &&
                        thoriumItem.Item.UseSound is SoundStyle thoriumSound)
                    {
                        SoundEngine.PlaySound(thoriumSound, player.position);
                    }
                }
            }

            return true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "FrostFury")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Wooden Arrows into Frostburn Arrows")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "StreamSting")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Wooden Arrows into Bone Arrows")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "CometCrossfire")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Wooden Arrows into Jester Arrows")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "SteelBow")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Wooden Arrows into Steel Arrows")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "eSandStoneBow")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Wooden Arrows into Talon Arrows")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "DurasteelRepeater")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Wooden Arrows into Durasteel Arrows")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "Trigun" && ModLoader.TryGetMod("CalamityMod", out _))
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Converts Musket balls into Hallow-point rounds")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }

            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.ModItem.Name == "MyceliumGattlingGun")
            {
                tooltips.Add(new TooltipLine(Mod, "AmmoChange", "Fires barrages of Mycelium")
                {
                    OverrideColor = Microsoft.Xna.Framework.Color.White
                });
            }
        }
    }
}
