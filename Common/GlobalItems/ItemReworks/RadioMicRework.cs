using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using InfernalEclipseAPI.Content.Projectiles;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    //Wardrobe Hummus
    [ExtendsFromMod("ThoriumMod")]
    public class RadioMicRework : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }
        public override bool InstancePerEntity => true;

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            try
            {
                if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarokMod))
                {
                    if (ragnarokMod.TryFind("RadioMic", out ModItem radioMic))
                    {
                        if (item.type == radioMic.Type)
                        {
                            int numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(12);
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1f)));
                                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<RadioMicPro>(), damage, knockback, player.whoAmI);
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
