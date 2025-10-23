using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks.ThrowableScythes
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
                //if (entity.type == thorium.Find<ModItem>("TrueFallingTwilight").Type) return true;
                //if (entity.type == thorium.Find<ModItem>("TrueBloodHarvest").Type) return true;
                if (entity.type == thorium.Find<ModItem>("MorningDew").Type) return true;
                //if (entity.type == thorium.Find<ModItem>("TerraScythe").Type) return true;
                if (entity.type == thorium.Find<ModItem>("ChristmasCheer").Type) return true;
                if (entity.type == thorium.Find<ModItem>("DreadTearer").Type) return true;
                if (entity.type == thorium.Find<ModItem>("TheBlackScythe").Type) return true;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                if (entity.type == ragnarok.Find<ModItem>("ProfanedScythe").Type) return true;
                if (entity.type == ragnarok.Find<ModItem>("ScoriaDualscythe").Type) return true;
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
                if (item.type == thorium.Find<ModItem>("BatScythe").Type) ThrowDistance = 125f;
                //if (item.type == thorium.Find<ModItem>("TitanScythe").Type) ThrowDistance = 150f;
                if (item.type == thorium.Find<ModItem>("IceShaver").Type) ThrowDistance = 45f;
                if (item.type == thorium.Find<ModItem>("DarkScythe").Type) ThrowDistance = 80f;
                if (item.type == thorium.Find<ModItem>("CrimsonScythe").Type) ThrowDistance = 65f;
                if (item.type == thorium.Find<ModItem>("FallingTwilight").Type) ThrowDistance = 80f;
                if (item.type == thorium.Find<ModItem>("BloodHarvest").Type) ThrowDistance = 80f;

                if (item.type == thorium.Find<ModItem>("BoneReaper").Type) ThrowDistance = 100f;
                if (item.type == thorium.Find<ModItem>("LustrousBaton").Type) ThrowDistance = 115f;
                //if (item.type == thorium.Find<ModItem>("TrueFallingTwilight").Type) ThrowDistance = 110f;
                //if (item.type == thorium.Find<ModItem>("TrueBloodHarvest").Type) ThrowDistance = 110f;
                if (item.type == thorium.Find<ModItem>("MorningDew").Type) ThrowDistance = 135f;
                //if (item.type == thorium.Find<ModItem>("TerraScythe").Type) ThrowDistance = 120f;
                if (item.type == thorium.Find<ModItem>("ChristmasCheer").Type) ThrowDistance = 150f;
                if (item.type == thorium.Find<ModItem>("DreadTearer").Type) ThrowDistance = 100f;
                if (item.type == thorium.Find<ModItem>("TheBlackScythe").Type) ThrowDistance = 150f;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                if (item.type == ragnarok.Find<ModItem>("ProfanedScythe").Type) ThrowDistance = 250f;
                if (item.type == ragnarok.Find<ModItem>("ScoriaDualscythe").Type) ThrowDistance = 75f;
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
                        damage - damage / 10,
                        knockback,
                        player.whoAmI,
                        (Main.rand.Next(2, 5) + 1) * 0.1f, // ai[0]
                        player.itemTime // ai[1]
                    );

                    if (projIndex >= 0)
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projIndex);

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
                //if (item.type == thorium.Find<ModItem>("TrueFallingTwilight").Type) isThrowableScythe = true;
                //if (item.type == thorium.Find<ModItem>("TrueBloodHarvest").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("MorningDew").Type) isThrowableScythe = true;
                //if (item.type == thorium.Find<ModItem>("TerraScythe").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("ChristmasCheer").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("DreadTearer").Type) isThrowableScythe = true;
                if (item.type == thorium.Find<ModItem>("TheBlackScythe").Type) isThrowableScythe = true;
            }

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                if (item.type == ragnarok.Find<ModItem>("ProfanedScythe").Type) isThrowableScythe = true;
                if (item.type == ragnarok.Find<ModItem>("ScoriaDualscythe").Type) isThrowableScythe = true;
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calbardhealer))
            {
                if (item.type == calbardhealer.Find<ModItem>("HyphaeBaton").Type) isThrowableScythe = true;
            }

            if (isThrowableScythe)
            {
                tooltips.Add(new TooltipLine(Mod, "ScytheThrow",
                    Terraria.Localization.Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.ScytheThrow")));
            }
        }
    }
}
