using System.Collections.Generic;
using InfernalEclipseAPI.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using ThoriumMod.Projectiles.Scythe;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Healer.Scythes.ThrowableScythes
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThrowableScythes : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            List<ModItem> throwableScythes = new();

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (entity.type == thorium.Find<ModItem>("BatScythe").Type) return true;
                //if (entity.type == thorium.Find<ModItem>("TitanScythe").Type) return true;
                if (entity.type == thorium.Find<ModItem>("IceShaver").Type) return true;
                if (entity.type == thorium.Find<ModItem>("DarkScythe").Type) return true;
                if (entity.type == thorium.Find<ModItem>("CrimsonScythe").Type) return true;
                if (entity.type == thorium.Find<ModItem>("FallingTwilight").Type) return true;
                if (entity.type == thorium.Find<ModItem>("BloodHarvest").Type) return true;

                if (entity.type == thorium.Find<ModItem>("BoneReaper").Type) return true;
                if (entity.type == thorium.Find<ModItem>("LustrousBaton").Type) return true;
                if (entity.type == thorium.Find<ModItem>("TrueFallingTwilight").Type) return true;
                if (entity.type == thorium.Find<ModItem>("TrueBloodHarvest").Type) return true;
                if (entity.type == thorium.Find<ModItem>("MorningDew").Type) return true;
                if (entity.type == thorium.Find<ModItem>("TerraScythe").Type) return true;
                if (entity.type == thorium.Find<ModItem>("ChristmasCheer").Type) return true;
                if (entity.type == thorium.Find<ModItem>("DreadTearer").Type) return true;
                if (entity.type == thorium.Find<ModItem>("TheBlackScythe").Type) return true;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                //if (entity.type == ragnarok.Find<ModItem>("ProfanedScythe").Type) return true;
                //if (entity.type == ragnarok.Find<ModItem>("ScoriaDualscythe").Type) return true;
            }

            if (ModLoader.TryGetMod("Consolaria", out Mod consolaria))
            {
                if (entity.type == consolaria.Find<ModItem>("ScytheFantasma").Type) return true;
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calbardhealer))
            {
                if (entity.type == calbardhealer.Find<ModItem>("HyphaeBaton").Type) return true;
            }

            foreach (var item in throwableScythes)
            {
                if (item != null && entity.type == item.Type)
                    return true;
            }
            return false;
        }

        public override bool InstancePerEntity => true;

        public float ThrowDistance = 180f; //base throw distance

        private void SetCustomThrowDistance(Item item)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (item.type == thorium.Find<ModItem>("BatScythe").Type) ThrowDistance = 165f;
                //if (item.type == thorium.Find<ModItem>("TitanScythe").Type) ThrowDistance = 150f;
                if (item.type == thorium.Find<ModItem>("IceShaver").Type) ThrowDistance = 45f;
                if (item.type == thorium.Find<ModItem>("DarkScythe").Type) ThrowDistance = 80f;
                if (item.type == thorium.Find<ModItem>("CrimsonScythe").Type) ThrowDistance = 65f;
                if (item.type == thorium.Find<ModItem>("FallingTwilight").Type) ThrowDistance = 100f;
                if (item.type == thorium.Find<ModItem>("BloodHarvest").Type) ThrowDistance = 100f;

                if (item.type == thorium.Find<ModItem>("BoneReaper").Type) ThrowDistance = 100f;
                if (item.type == thorium.Find<ModItem>("LustrousBaton").Type) ThrowDistance = 115f;
                if (item.type == thorium.Find<ModItem>("TrueFallingTwilight").Type) ThrowDistance = 125f;
                if (item.type == thorium.Find<ModItem>("TrueBloodHarvest").Type) ThrowDistance = 125f;
                if (item.type == thorium.Find<ModItem>("MorningDew").Type) ThrowDistance = 150f;
                if (item.type == thorium.Find<ModItem>("TerraScythe").Type) ThrowDistance = 140f;
                if (item.type == thorium.Find<ModItem>("ChristmasCheer").Type) ThrowDistance = 150f;
                if (item.type == thorium.Find<ModItem>("DreadTearer").Type) ThrowDistance = 100f;
                if (item.type == thorium.Find<ModItem>("TheBlackScythe").Type) ThrowDistance = 150f;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                //if (item.type == ragnarok.Find<ModItem>("ProfanedScythe").Type) ThrowDistance = 250f;
                //if (item.type == ragnarok.Find<ModItem>("ScoriaDualscythe").Type) ThrowDistance = 75f;
            }

            if (ModLoader.TryGetMod("Consolaria", out Mod consolaria))
            {
                if (item.type == consolaria.Find<ModItem>("ScytheFantasma").Type) ThrowDistance = 120f;
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calbardhealer))
            {
                if (item.type == calbardhealer.Find<ModItem>("HyphaeBaton").Type) ThrowDistance = 70f;
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            return true;
        }

        public override void SetDefaults(Item entity)
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[entity.type] = true;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SetCustomThrowDistance(item);

            if (Main.myPlayer == player.whoAmI)
            {
                int projIndex = -1;

                if (player.altFunctionUse == 2)
                {
                    Vector2 throwVel = Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * -ThrowDistance;

                    projIndex = Projectile.NewProjectile(
                        source,
                        position,
                        throwVel,
                        type,
                        damage + damage / 5,
                        knockback,
                        player.whoAmI,
                        (Main.rand.Next(2, 5) + 1) * 0.1f, // ai[0]
                        player.itemTime // ai[1]
                    );

                    if (projIndex >= 0)
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projIndex);

                    //TRUE FALLING TWILIGHT AND BLOOD HARVEST
                    Mod thorium = ModLoader.GetMod("ThoriumMod");

                    if (item.type == thorium.Find<ModItem>("TrueFallingTwilight").Type)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Projectile.NewProjectile((IEntitySource)(object)source, ((Entity)player).Center.X, ((Entity)player).Center.Y, 0f, 0f, ModContent.ProjectileType<TrueFallingTwilightBurn>(), (int)((float)damage * 0.75f), knockback, ((Entity)player).whoAmI, (float)k, 0f, 0f);
                        }
                    }
                    if (item.type == thorium.Find<ModItem>("TrueBloodHarvest").Type)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Projectile.NewProjectile((IEntitySource)(object)source, ((Entity)player).Center.X, ((Entity)player).Center.Y, 0f, 0f, ModContent.ProjectileType<TrueBloodHarvestOrb>(), (int)((float)damage * 0.75f), knockback, ((Entity)player).whoAmI, (float)k, 0f, 0f);
                        }
                    }

                    return false;
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            return player.altFunctionUse == 2 ? 2f : 1f;
        }

        public override float UseAnimationMultiplier(Item item, Player player)
        {
            return player.altFunctionUse == 2 ? 2f : 1f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            bool isThrowableScythe = false;

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (item.type == thorium.Find<ModItem>("BatScythe").Type) isThrowableScythe = true;
                //if (item.type == thorium.Find<ModItem>("TitanScythe").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("IceShaver").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("DarkScythe").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("CrimsonScythe").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("FallingTwilight").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("BloodHarvest").Type) isThrowableScythe = true;

                if (item.type == thorium.Find<ModItem>("BoneReaper").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("LustrousBaton").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("TrueFallingTwilight").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("TrueBloodHarvest").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("MorningDew").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("TerraScythe").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("ChristmasCheer").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("DreadTearer").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("TheBlackScythe").Type) isThrowableScythe = true;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                //if (item.type == ragnarok.Find<ModItem>("ProfanedScythe").Type) isThrowableScythe = true;
                //if (item.type == ragnarok.Find<ModItem>("ScoriaDualscythe").Type) isThrowableScythe = true;
            }

            if (ModLoader.TryGetMod("Consolaria", out Mod consolaria))
            {
                if (item.type == consolaria.Find<ModItem>("ScytheFantasma").Type) isThrowableScythe = true;
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calbardhealer))
            {
                if (item.type == calbardhealer.Find<ModItem>("HyphaeBaton").Type) isThrowableScythe = true;
            }

            if (isThrowableScythe)
            {
                InfernalUtilities.AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.ScytheThrow"), Color.Lerp(Color.White, new Color(255, 80, 0), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)));
            }
        }
    }
}
