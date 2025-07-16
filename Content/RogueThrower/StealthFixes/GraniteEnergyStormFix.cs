using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.RogueThrower.StealthFixes
{
    public class GraniteEnergyStormFix : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework) &&
                thorRework.TryFind("PocketEnergyStorm", out ModItem pes) &&
                item.type == pes.Type &&
                ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                // Delay the stealth consumption by one tick to allow stealth strike logic to complete
                int playerIndex = player.whoAmI;

                // Queue on the next update tick
                Main.QueueMainThreadAction(() =>
                {
                    if (Main.player[playerIndex] != null && Main.player[playerIndex].active)
                    {
                        calamity.Call("ConsumeStealth", Main.player[playerIndex]);
                    }
                });
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
