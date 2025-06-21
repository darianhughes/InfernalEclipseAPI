using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace InfernalEclipseAPI.Content.RogueThrower
{
    [ExtendsFromMod("ThoriumMod")]
    public class WhiteDwarfCooldown : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            var cdPlayer = player.GetModPlayer<RogueThrowerPlayer>();

            if (thoriumPlayer.setWhiteDwarf && hit.Crit)
            {
                if (cdPlayer.whiteDwarfCooldown > 0)
                {
                    thoriumPlayer.setWhiteDwarf = false;
                }
                else
                {
                    int cooldown = 120; // Default cooldown

                    // Check if White Dwarf Thrusters accessory is equipped
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                        thorium.TryFind<ModItem>("WhiteDwarfThrusters", out var thrusterItem))
                    {
                        int whiteDwarfThrustersType = thrusterItem.Type;

                        for (int i = 3; i < player.armor.Length; i++) // accessories start at index 3
                        {
                            Item accessory = player.armor[i];

                            if (!accessory.IsAir && accessory.type == whiteDwarfThrustersType)
                            {
                                cooldown = 60; // Reduced cooldown
                                break;
                            }
                        }
                    }

                    cdPlayer.whiteDwarfCooldown = cooldown;
                }
            }
        }
    }
}
