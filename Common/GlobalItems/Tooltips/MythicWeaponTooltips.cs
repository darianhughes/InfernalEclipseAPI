using System.Collections.Generic;
using InfernumMode.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using Terraria.Localization;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee.Spears;
using InfernumMode.Content.Projectiles.Melee;
using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Items.Weapons.Summon;
using InfernumMode.Core.GlobalInstances.Systems;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Melee;
using Terraria.DataStructures;

namespace InfernalEclipseAPI.Common.GlobalItems.Tooltips
{
    public class MythicWeaponProjectiles : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (entity.type == ModContent.ProjectileType<AmidiasTridentProj>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (entity.type == ModContent.ProjectileType<AmidiasWhirlpool>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (entity.type == ModContent.ProjectileType<AtlantisSpear>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMagic>();
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (entity.type == thorium.Find<ModProjectile>("AncientLightPro").Type)
                {
                    entity.DamageType = ModContent.GetInstance<MythicMagic>();
                }

                if (entity.type == thorium.Find<ModProjectile>("AncientLightPro2").Type)
                {
                    entity.DamageType = ModContent.GetInstance<MythicMagic>();
                }
            }

            if (entity.type == ModContent.ProjectileType<MyrindaelBonkProjectile>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (entity.type == ModContent.ProjectileType<MyrindaelLightning>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (entity.type == ModContent.ProjectileType<MyrindaelSpark>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (entity.type == ModContent.ProjectileType<MyrindaelSpinProjectile>())
            {
                entity.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (ModLoader.TryGetMod("YouBoss", out Mod youBoss))
            {
                if (entity.type == youBoss.Find<ModProjectile>("FirstFractalHoldout").Type)
                {
                    entity.DamageType = ModContent.GetInstance<MythicMelee>();
                }

                if (entity.type == youBoss.Find<ModProjectile>("HomingTerraBeam").Type)
                {
                    entity.DamageType = ModContent.GetInstance<MythicMelee>();
                }

                if (entity.type == youBoss.Find<ModProjectile>("PlayerShadowClone").Type)
                {
                    entity.DamageType = ModContent.GetInstance<MythicMelee>();
                }

                if (entity.type == youBoss.Find<ModProjectile>("TerraSlash").Type)
                {
                    entity.DamageType = ModContent.GetInstance<MythicMelee>();
                }
            }

            if (entity.type == ModContent.ProjectileType<PulseRifleShot>())
            {
                entity.DamageType = ModContent.GetInstance<MythicRanged>();
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.type == ModContent.ProjectileType<Calamitamini>() && WorldSaveSystem.InfernumModeEnabled)
            {
                projectile.DamageType = ModContent.GetInstance<MythicSummon>();
            }

            if (projectile.type == ModContent.ProjectileType<BrimstoneLaserSummon>() && WorldSaveSystem.InfernumModeEnabled)
            {
                projectile.DamageType = ModContent.GetInstance<MythicSummon>();
            }

            if (projectile.type == ModContent.ProjectileType<Catastromini>() && WorldSaveSystem.InfernumModeEnabled)
            {
                projectile.DamageType = ModContent.GetInstance<MythicSummon>();
            }

            if (projectile.type == ModContent.ProjectileType<Cataclymini>() && WorldSaveSystem.InfernumModeEnabled)
            {
                projectile.DamageType = ModContent.GetInstance<MythicSummon>();
            }

            if (projectile.type == ModContent.ProjectileType<BrimstoneFireSummon>() && WorldSaveSystem.InfernumModeEnabled)
            {
                projectile.DamageType = ModContent.GetInstance<MythicSummon>();
            }
        }
    }

    public class MythicWeaponTooltips : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<AmidiasTrident>())
            {
                item.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (item.type == ModContent.ItemType<Atlantis>())
            {
                item.DamageType = ModContent.GetInstance<MythicMagic>();
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (item.type == thorium.Find<ModItem>("AncientLight").Type)
                {
                    item.DamageType = ModContent.GetInstance<MythicMagic>();
                }
            }

            if (item.type == ModContent.ItemType<Myrindael>())
            {
                item.DamageType = ModContent.GetInstance<MythicMelee>();
            }

            if (item.type == ModContent.ItemType<PulseRifle>())
            {
                item.DamageType = ModContent.GetInstance<MythicRanged>();
            }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.type == ModContent.ItemType<EntropysVigil>())
            {
                if (WorldSaveSystem.InfernumModeEnabled) item.DamageType = ModContent.GetInstance<MythicSummon>();
                else item.DamageType = DamageClass.Summon;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
            if (index != -1)
            {
                bool canAddTooltip = false;
                string importantName = "";

                if (item.type == ModContent.ItemType<AmidiasTrident>())
                {
                    canAddTooltip = true;
                    importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Amidias");
                }

                if (item.type == ModContent.ItemType<EntropysVigil>() && WorldSaveSystem.InfernumModeEnabled)
                {
                    canAddTooltip = true;
                    importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.CalClone");
                }

                if (item.type == ModContent.ItemType<Atlantis>())
                {
                    canAddTooltip = true;
                    importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Anahita");
                }

                if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                {
                    if (item.type == thorium.Find<ModItem>("AncientLight").Type)
                    {
                        canAddTooltip = true;
                        importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Cultist");
                    }
                }

                if (item.type == ModContent.ItemType<Myrindael>())
                {
                    canAddTooltip = true;
                    importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Argus");
                }

                if (ModLoader.TryGetMod("YouBoss", out Mod you))
                {
                    if (item.type == you.Find<ModItem>("FirstFractal").Type)
                    {
                        canAddTooltip = true;
                        importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Your");
                    }
                }

                if (item.type == ModContent.ItemType<PulseRifle>())
                {
                    canAddTooltip = true;
                    importantName = Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Draedon");
                }

                if (canAddTooltip)
                {
                    tooltips.Insert(index + 1, new TooltipLine(Mod, "SignatureWeapon", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MythicTooltips.Base", importantName))
                    {
                        OverrideColor = Color.Cyan
                    });
                }
            }
        }
    }
}
