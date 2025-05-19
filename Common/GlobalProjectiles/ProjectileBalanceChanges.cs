using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace InfernalEclipseAPI.Common.Projectiles
{
    public class ProjectileBalanceChanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (ModLoader.TryGetMod("Clamity", out Mod clam))
            {
                if (entity.type == clam.Find<ModProjectile>("FireBarrage").Type)
                {
                    entity.damage = 135;
                }
                if (entity.type == clam.Find<ModProjectile>("FireBarrageHoming").Type)
                {
                    entity.damage = 130;
                }
                if (entity.type == clam.Find<ModProjectile>("Fireblast").Type)
                {
                    entity.damage = 140;
                }
                if (entity.type == clam.Find<ModProjectile>("FireBombExplosion").Type)
                {
                    entity.damage = 135;
                }
                if (entity.type == clam.Find<ModProjectile>("Firethrower").Type)
                {
                    entity.damage = 150;
                }
            }

            if (ModLoader.TryGetMod("Thorium", out Mod thorium) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (entity.type == thorium.Find<ModProjectile>("SeashellCastanettessPro1").Type)
                {
                    entity.penetrate = 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("Cube").Type)
                {
                    entity.penetrate = 3;
                }

                if (entity.type == thorium.Find<ModProjectile>("GeyserPro2").Type)
                {
                    entity.scale *= 5;
                }

                if (entity.type == thorium.Find<ModProjectile>("GraniteBarrier").Type)
                {
                    entity.penetrate = 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("PalmCrossPro").Type)
                {
                    entity.scale *= 5;
                }

                if (entity.type == thorium.Find<ModProjectile>("TorpedoPro2").Type)
                {
                    entity.scale *= 2;
                    entity.penetrate = 10;
                }

                if (entity.type == thorium.Find<ModProjectile>("StoneThrowingSpearPro").Type)
                {
                    entity.penetrate = 2;
                }

                if (entity.type == thorium.Find<ModProjectile>("IcyTomahawkPro").Type)
                {
                    entity.penetrate = 5;
                }

                if (entity.type == thorium.Find<ModProjectile>("AquaiteScythePro").Type || entity.type == thorium.Find<ModProjectile>("BloodHarvestPro").Type || entity.type == thorium.Find<ModProjectile>("FallingTwilightPro").Type || entity.type == thorium.Find<ModProjectile>("TrueHallowedScythePro").Type || entity.type == thorium.Find<ModProjectile>("HallowedScythePro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 6;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 6;
                    }
                }

                if (entity.type == thorium.Find<ModProjectile>("BoneReaperPro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 3;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 3;
                    }
                }

                if (entity.type == thorium.Find<ModProjectile>("BatScythePro2").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 1;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 1;
                    }
                }
            } 

            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (entity.type == ragnarok.Find<ModProjectile>("GelScythePro2").Type)
                {
                    entity.penetrate = 5;
                    entity.scale = 2;
                }

                //if (entity.type == ragnarok.Find<ModProjectile>("WindSlashPro").Type)
                //{
                //    entity.scale *= 2;
                //}

                if (entity.type == ragnarok.Find<ModProjectile>("ProfanedBellBlast").Type)
                {
                    entity.penetrate = 3;
                    entity.scale = (float)0.75;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("ElysianSongPro").Type)
                {
                    entity.penetrate = 50;
                    entity.scale = (float)1.5;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("TendrilStrike").Type)
                {
                    entity.scale = (float)1.5;
                }

                if (entity.type == ragnarok.Find<ModProjectile>("MarbleScythePro").Type || entity.type == ragnarok.Find<ModProjectile>("ProfanedScythePro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 6;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 6;
                    }
                }

                if (entity.type == ragnarok.Find<ModProjectile>("ScoriaDualscythePro").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 3;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 3;
                    }
                }
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (entity.type == calBardHeal.Find<ModProjectile>("ExoSound").Type)
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = -1;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 1;
                    }
                }

                if (entity.type == calBardHeal.Find<ModProjectile>("StarBirth").Type)
                {
                    //entity.scale *= 0.3f;
                }
            }

            if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (GetProj(entity, thorRework, "DemonBloodSword") ||
                    GetProj(entity, thorRework, "DragonTooth") ||
                    GetProj(entity, thorRework, "DreadRazor") || 
                    GetProj(entity, thorRework, "IllumiteBlade") ||
                    GetProj(entity, thorRework, "LodeStoneClaymore") ||
                    GetProj(entity, thorRework, "SoulRender") || 
                    GetProj(entity, thorRework, "TerrariumSaber") ||
                    GetProj(entity, thorRework, "TitanSword") ||
                    GetProj(entity, thorRework, "ToothOfTheConsumer") ||
                    GetProj(entity, thorRework, "BeholderBlade"))
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = 60;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = 60;
                    }
                }

                if (GetProj(entity, thorRework, "GrandThunder"))
                {
                    if (entity.usesLocalNPCImmunity)
                    {
                        entity.localNPCHitCooldown = -1;
                    }

                    if (entity.usesIDStaticNPCImmunity)
                    {
                        entity.idStaticNPCHitCooldown = -1;
                    }
                }
            }
        }

        private bool GetProj(Projectile entity, Mod mod, string item)
        {
            if (entity.type == mod.Find<ModProjectile>(item).Type)
            {
                return true;
            }
            return false;
        }
    }
}
