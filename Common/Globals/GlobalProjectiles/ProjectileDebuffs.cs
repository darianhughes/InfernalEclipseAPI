using CalamityMod.Projectiles.Melee.MaceFlails;
using InfernalEclipseAPI.Content.Buffs.Tag;
using ThoriumMod;

namespace InfernalEclipseAPI.Common.GlobalProjectiles
{
    //Wardrobe Hummus
    public class ProjectileDebuffs : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }

        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type == ProjectileID.HoundiusShootius)
            {
                target.AddBuff(BuffID.Electrified, 120);
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
            {
                if (projectile.type == thoriumMod.Find<ModProjectile>("ThunderTalonPro").Type)
                {
                    target.AddBuff(BuffID.Electrified, 180);
                }

                if (projectile.type == (thoriumMod.Find<ModProjectile>("VoltHatchetPro")?.Type ?? -1))
                {
                    target.AddBuff(BuffID.Electrified, 60);
                }

                if (ModLoader.TryGetMod("CalamityMod", out Mod calamity1))
                {
                    if (projectile.type == thoriumMod.Find<ModProjectile>("DrenchedPro").Type)
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 60);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("AquaPelterPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 60);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("GeyserPro2")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 180);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("AquaiteKnifePro")?.Type ?? -1) || projectile.type == (thoriumMod.Find<ModProjectile>("AquaiteKnifePro2")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 60);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("AquamarineWineGlassPro2")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 180);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("AquaiteScythePro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 180);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("IllustriousPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 120);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("IridescentPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 120);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("PearlPikePro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 120);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("ScubaCurvaPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 120);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("ScubaCurvaPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 120);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("BlobhornCoralStaffPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 180);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("SeaFoamScepterPro")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 180);
                    }

                    if (projectile.type == (thoriumMod.Find<ModProjectile>("SerpentsCryPro")?.Type ?? -1) || projectile.type == (thoriumMod.Find<ModProjectile>("SerpentsCryPro2")?.Type ?? -1))
                    {
                        target.AddBuff(calamity1.Find<ModBuff>("RiptideDebuff")?.Type ?? -1, 120);
                    }

                    if (projectile.type == ModContent.ProjectileType<UrchinMaceProj>() || projectile.type == (calamity1.Find<ModProjectile>("RedtideWhirlpool")?.Type ?? -1))
                    {
                        target.AddBuff(BuffID.Poisoned, 120);
                    }
                }
            }

            // Check if all mods are loaded before continuing.
            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarokMod) &&
                ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) &&
                ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod2))
            {

                // Helper function for modular lookups
                void TryApplyBuff(string projName, Mod modProj, string buffName, Mod modBuff, int time)
                {
                    var proj = modProj.Find<ModProjectile>(projName);
                    var buff = modBuff.Find<ModBuff>(buffName);
                    if (proj is not null && projectile.type == proj.Type && buff is not null)
                        target.AddBuff(buff.Type, time);
                }

                // Ragnarok projectiles that apply Calamity/Thorium debuffs
                TryApplyBuff("MarbleScythePro", ragnarokMod, "HolyGlare", thoriumMod2, 180);
                TryApplyBuff("CorrosiveFluxPro", ragnarokMod, "Irradiated", calamityMod, 180);
                if (projectile.type == (ragnarokMod.Find<ModProjectile>("VirusprayerPro1")?.Type ?? -1) || projectile.type == (ragnarokMod.Find<ModProjectile>("VirusprayerPro2")?.Type ?? -1))
                {
                    target.AddBuff(calamityMod.Find<ModBuff>("Irradiated")?.Type ?? -1, 180);
                }
                TryApplyBuff("AstralRipperPro", ragnarokMod, "AstralInfectionDebuff", calamityMod, 180);
                TryApplyBuff("AstralRipperStarPro", ragnarokMod, "AstralInfectionDebuff", calamityMod, 120);

                // Nightmare Freezer variants: GlacialState + Cursed Inferno
                var freezerPro2 = ragnarokMod.Find<ModProjectile>("NightmareFreezerPro2");
                var freezerPro3 = ragnarokMod.Find<ModProjectile>("NightmareFreezerPro3");
                var freezerPro = ragnarokMod.Find<ModProjectile>("NightmareFreezerPro");
                var glacialState = calamityMod.Find<ModBuff>("GlacialState");

                if ((freezerPro != null && projectile.type == freezerPro.Type) && glacialState != null)
                {
                    target.AddBuff(glacialState.Type, 60);
                    target.AddBuff(BuffID.Frostburn, 300);
                }
                else if ((freezerPro2 != null && projectile.type == freezerPro2.Type) ||
                         (freezerPro3 != null && projectile.type == freezerPro3.Type))
                {
                    if (glacialState != null)
                        target.AddBuff(glacialState.Type, 30);
                    target.AddBuff(BuffID.Frostburn, 300);
                }

                // Executioner projectiles: MiracleBlight
                var execMark5 = ragnarokMod.Find<ModProjectile>("ExecutionerMark05Pro");
                var execMark5Throw = ragnarokMod.Find<ModProjectile>("ExecutionerMark05ThrowPro");
                var miracleBlight = calamityMod.Find<ModBuff>("MiracleBlight");

                if ((execMark5 != null && projectile.type == execMark5.Type) ||
                    (execMark5Throw != null && projectile.type == execMark5Throw.Type))
                {
                    if (miracleBlight != null)
                        target.AddBuff(miracleBlight.Type, 300);
                }

                // ProfanedScythe/ElysianSong: HolyFlames
                TryApplyBuff("ProfanedScythePro", ragnarokMod, "HolyFlames", calamityMod, 300);
                TryApplyBuff("ElysianSongPro", ragnarokMod, "HolyFlames", calamityMod, 180);

                // Steampipes/ScoriaDualscythe: CrushDepth + Hydratoxin (323)
                void ApplyCrushDepth(string projName)
                {
                    var proj = ragnarokMod.Find<ModProjectile>(projName);
                    var crushDepth = calamityMod.Find<ModBuff>("CrushDepth");
                    if (proj is not null && projectile.type == proj.Type && crushDepth is not null)
                    {
                        target.AddBuff(crushDepth.Type, 180);
                        target.AddBuff(BuffID.OnFire3, 180); // Hydratoxin
                    }
                }
                ApplyCrushDepth("SteampipesPro");
                ApplyCrushDepth("ScoriaDualscythePro");

                // Fractal Orbs: Nightwither
                var fractalOrb = ragnarokMod.Find<ModProjectile>("FractalOrb");
                var fractalPro1 = ragnarokMod.Find<ModProjectile>("FractalPro1");
                var fractalPro2 = ragnarokMod.Find<ModProjectile>("FractalPro2");
                var nightwither = calamityMod.Find<ModBuff>("Nightwither");

                if ((fractalOrb != null && projectile.type == fractalOrb.Type) ||
                    (fractalPro1 != null && projectile.type == fractalPro1.Type) ||
                    (fractalPro2 != null && projectile.type == fractalPro2.Type))
                {
                    if (nightwither != null)
                        target.AddBuff(nightwither.Type, 180);
                }
            }

            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod bardhealer) && ModLoader.TryGetMod("CalamityMod", out Mod calamity2))
            {
                if (projectile.type == (bardhealer.Find<ModProjectile>("ExoSound")?.Type ?? -1))
                {
                    Player obj = Main.player[projectile.owner];

                    // Nerfed healing
                    int healAmount = damageDone / 200;
                    obj.statLife += healAmount;
                    obj.HealEffect(healAmount, true);

                    // Thorium inspiration
                    if (Utils.NextBool(Main.rand, 3))
                    {
                        HealInspiration.Init(obj);
                    }

                    // Apply Calamity debuff
                    int buffType = calamity2.Find<ModBuff>("MiracleBlight")?.Type ?? 0;
                    if (buffType > 0)
                    {
                        target.AddBuff(buffType, 240, false);
                    }
                }

                if (projectile.type == (bardhealer.Find<ModProjectile>("InfestedCastanet")?.Type ?? -1) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium3))
                {
                    // Apply Calamity debuff
                    int buffType = thorium3.Find<ModBuff>("FungalGrowth")?.Type ?? 0;
                    if (buffType > 0)
                    {
                        target.AddBuff(buffType, 180, false);
                    }
                }
            }

            if (ModLoader.TryGetMod("CatalystMod", out Mod catalysyt))
            {
                if (projectile.type == (catalysyt.Find<ModProjectile>("CoralCrusherProjectile")?.Type ?? -1))
                {
                    target.AddBuff(ModContent.BuffType<DefaultSummonTag>(), 300);
                    target.GetGlobalNPC<TaggedNPC>().summonTagDamage = 4f;
                }

                if (projectile.type == (catalysyt.Find<ModProjectile>("ResonantStrikerProjectile")?.Type ?? -1))
                {
                    target.AddBuff(ModContent.BuffType<DefaultSummonTag>(), 300);
                    target.GetGlobalNPC<TaggedNPC>().summonTagDamage = 8f;
                }

                if (projectile.type == (catalysyt.Find<ModProjectile>("BlossomsBlessingProjectile")?.Type ?? -1))
                {
                    target.AddBuff(ModContent.BuffType<DefaultSummonTag>(), 300);
                    target.GetGlobalNPC<TaggedNPC>().summonTagDamage = 12f;
                }

                if (projectile.type == (catalysyt.Find<ModProjectile>("UnrelentingTormentProjectile")?.Type ?? -1))
                {
                    target.AddBuff(ModContent.BuffType<DefaultSummonTag>(), 300);
                    target.GetGlobalNPC<TaggedNPC>().summonTagDamage = 10f;
                }

                if (projectile.type == (catalysyt.Find<ModProjectile>("CatharsisProjectile")?.Type ?? -1)
                    || projectile.type == (catalysyt.Find<ModProjectile>("HighCatharsisDown")?.Type ?? -1)
                    || projectile.type == (catalysyt.Find<ModProjectile>("HighCatharsisUp")?.Type ?? -1))
                {
                    target.AddBuff(ModContent.BuffType<DefaultSummonTag>(), 300);
                    target.GetGlobalNPC<TaggedNPC>().summonTagDamage = 12f;
                }
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public static class HealInspiration
    {
        public static void Init(Player obj)
        {
            obj.GetModPlayer<ThoriumPlayer>().HealInspiration(1);
        }
    }
}
