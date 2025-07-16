using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using System.Reflection;
using InfernumMode.Core.Balancing;
using System.Security.Policy;
using CalamityMod.Items.Tools;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    public class ItemBalanceChanges : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            #region Vanilla
            if (item.type == ItemID.ReaverShark)
            {
                item.pick = 59;
                item.useTime = 11;
            }

            if (ModContent.GetInstance<InfernalConfig>().VanillaBalanceChanges)
            {
                if (item.type == ItemID.FieryGreatsword)
                {
                    item.damage = 50;
                    item.useTime = 36;
                    item.useAnimation = 36;
                }

                if (item.type == ItemID.Muramasa)
                {
                    item.useTime = 13;
                    item.useAnimation = 13;
                    item.damage = 22;
                }

                if (item.type == ItemID.NightsEdge)
                {
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.damage = 48;
                }

                if (item.type == ItemID.BoneWhip)
                {
                    item.useTime = 22;
                    item.useAnimation = 22;
                    item.damage = 30;
                }

                if (item.type == ItemID.ThornWhip)
                {
                    item.useTime = 30;
                    item.useAnimation = 30;
                }

                if (item.type == ItemID.Zenith)
                {
                    item.damage = 310;
                }

                if (item.type == ItemID.TrueNightsEdge)
                {
                    item.damage = 165;
                }

                if (item.type == ItemID.LucyTheAxe)
                {
                    item.damage = 37;
                }

                if (item.type == ItemID.PewMaticHorn)
                {
                    item.damage = 35;
                }

                if (item.type == ItemID.WeatherPain)
                {
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.damage = 30;
                }

                if (item.type == ItemID.HoundiusShootius)
                {
                    item.damage = 60;
                }
            }

            if (InfernalConfig.Instance.ChanageWeaponClasses)
            {
                if (item.type == ItemID.Shuriken)
                {
                    item.DamageType = ModContent.GetInstance<RogueDamageClass>();
                }
            }
            #endregion

            #region Zenith Toilet
            if (ModLoader.TryGetMod("ZenithToilet", out Mod toilet))
            {
                if (item.type == toilet.Find<ModItem>("ZenithToilet").Type)
                {
                    item.damage = 50000;
                    item.crit = 4;
                }

                if (item.type == toilet.Find<ModItem>("TrueZenithToilet").Type)
                {
                    item.damage = 100000;
                    item.crit = 0;
                }
            }
            #endregion

            #region You
            if (ModLoader.TryGetMod("YouBoss", out Mod youBoss)) 
            {
                //First Fractal
                if (GetItem(youBoss, "FirstFractal", item))
                {
                    item.damage = 315;
                }
            }
            #endregion

            #region Shields of Cthulhu
            if (ModLoader.TryGetMod("ShieldsOfCthulhu", out Mod SoC))
            {
                if (GetItem(SoC, "CobaltShieldOfCthulhu", item))
                {
                    item.defense = 3;
                }

                if (GetItem(SoC, "ObsidianShieldOfCthulhu", item))
                {
                    item.defense = 4;
                }

                if (GetItem(SoC, "AnkhShieldOfCthulhu", item))
                {
                    item.defense = 6;
                }
            }
            #endregion

            #region Calamity
            if (InfernalConfig.Instance.CalamityBalanceChanges)
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);

                #region Melee
                if (GetItem(cal, "SubmarineShocker", item))
                {
                    item.damage = 55;
                }

                if (item.type == ModContent.ItemType<InfernaCutter>())
                {
                    item.DamageType = DamageClass.Melee;
                }

                if (GetItem(cal, "EmpyreanKnives", item))
                {
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.damage = 90;
                }
                #endregion

                #region Ranged
                if (GetItem(cal, "CrackshotColt", item))
                {
                    item.useTime = 28;
                    item.useAnimation = 28;
                    item.damage = 30;
                }

                if (GetItem(cal, "MidasPrime", item))
                {
                    item.useTime = 26;
                    item.useAnimation = 26;
                }

                if (GetItem(cal, "Barinade", item))
                {
                    item.useTime = 15;
                    item.useAnimation = 15;
                }

                if (GetItem(cal, "Galeforce", item))
                {
                    item.damage = 25;
                }

                if (GetItem(cal, "FlurrystormCannon", item))
                {
                    item.damage = 14;
                }

                if (GetItem(cal, "Archerfish", item))
                {
                    item.damage = 25;
                    item.useTime = 10;
                    item.useAnimation = 10;
                }

                if (GetItem(cal, "Goobow", item))
                {
                    item.damage = 47;
                    item.useTime = 24;
                    item.useAnimation = 24;
                }

                if (GetItem(cal, "OverloadedBlaster", item))
                {
                    item.useTime = 24;
                    item.useAnimation = 24;
                }

                if (GetItem(cal, "LunarianBow", item))
                {
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.damage = 34;
                }

                if (GetItem(cal, "ThermoclineBlaster", item))
                {
                    item.damage = 62;
                }

                if (GetItem(cal, "VernalBolter", item))
                {
                    item.useTime = 19;
                    item.useAnimation = 19;
                }

                if (GetItem(cal, "Arietes41", item))
                {
                    item.damage = 70;
                }

                if (GetItem(cal, "ElementalEruption", item))
                {
                    item.damage = 116;
                }
                #endregion

                #region Summoner
                //Deathstare Rod
                if (item.type == cal.Find<ModItem>("DeathstareRod").Type)
                {
                    item.damage = 30;
                }

                //Herring Staff
                if (item.type == cal.Find<ModItem>("HerringStaff").Type)
                {
                    item.damage = 20;
                }

                //Mounted Scanner
                if (GetItem(cal, "MountedScanner", item))
                {
                    item.damage = 33;
                }

                //Plantation Staff 
                if (GetItem(cal, "PlantationStaff", item))
                {
                    item.damage = 82;
                }
                #endregion
            }
            #endregion

            bool hasCatalyst = false;
            #region Catalyst
            if (InfernalConfig.Instance.CalamityBalanceChanges && ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
            {
                hasCatalyst = true;
                #region Ranger
                //Desert Scorcher
                if (item.type == catalyst.Find<ModItem>("DesertScorcher").Type)
                {
                    item.shootSpeed = 10;
                    item.damage = 8;
                    item.useTime = 40;
                    item.useAnimation = 40;
                }

                //Ichorthrower
                if (item.type == catalyst.Find<ModItem>("Ichorthrower").Type)
                {
                    item.shootSpeed = (int)3.5;
                    item.damage = 21;
                }

                if (GetItem(catalyst, "BreathofGlacies", item))
                {
                    item.damage = 70;
                }

                if (GetItem(catalyst, "StaticInferno", item))
                {
                    item.useTime = 26;
                    item.useAnimation = 26;
                }
                #endregion

                #region Summoner
                //Coral Crusher
                if (item.type == catalyst.Find<ModItem>("CoralCrusher").Type)
                {
                    item.useTime = 32;
                    item.useAnimation = 32;
                    item.damage = 21;
                }

                //Congealed Duo-Whip
                if (item.type == catalyst.Find<ModItem>("CongeledDuoWhip").Type)
                {
                    item.useTime = 36;
                    item.useAnimation = 36;
                }

                if (GetItem(catalyst, "BlossomsBlessing", item))
                {
                    item.useTime = 28;
                    item.useAnimation = 28;
                }

                if (GetItem(catalyst, "AstralpodStaff", item))
                {
                    item.damage = 80;
                }

                if (GetItem(catalyst, "Catharsis", item))
                {
                    item.damage = 850;
                }
                #endregion

                #region Rogue
                if (GetItem(catalyst, "InterstellarVolution", item))
                {
                    item.damage = 330;
                }
                #endregion

                #region Armor
                if (GetItem(catalyst, "IntergelacticBreastplate", item))
                {
                    item.defense = 20;
                }

                if (GetItem(catalyst, "IntergelacticGreaves", item))
                {
                    item.defense = 12;
                }

                if (GetItem(catalyst, "IntergelacticHeadMagic", item))
                {
                    item.defense = 14;
                }

                if (GetItem(catalyst, "IntergelacticHeadMelee", item))
                {
                    item.defense = 44;
                }

                if (GetItem(catalyst, "IntergelacticHeadRanged", item))
                {
                    item.defense = 14;
                }

                if (GetItem(catalyst, "IntergelacticHeadRogue", item))
                {
                    item.defense = 34;
                }

                if (GetItem(catalyst, "IntergelacticHeadSummon", item))
                {
                    item.defense = 6;
                }
                #endregion
            }
            #endregion

            bool hasCalHunt = false;
            #region Calamity: Hunt of the Old God
            if (ModLoader.TryGetMod("CalamityHunt", out Mod calHunt) && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                hasCalHunt = true;
                #region Melee
                if (GetItem(calHunt, "Parasanguine", item))
                {
                    item.useTime = 22;
                    item.useAnimation = 22;
                }

                if (GetItem(calHunt, "ScytheOfTheOldGod", item))
                {
                    item.damage = 2700;
                }
                #endregion

                #region Ranged
                if (GetItem(calHunt, "SludgeShaker", item))
                {
                    item.damage = 2800;
                }

                if (GetItem(calHunt, "Trailblazer", item))
                {
                    item.damage = 1050;
                }
                #endregion

                #region Mage
                if (GetItem(calHunt, "CrystalGauntlets", item))
                {
                    item.damage = 900;
                }
                #endregion

                #region Summoner
                if (GetItem(calHunt, "SlimeCane", item))
                {
                    item.damage = 650;
                }
                #endregion

                #region Rogue
                if (GetItem(calHunt, "FissionFlyer", item))
                {
                    item.damage = 1000;
                }
                #endregion
            }
            #endregion

            #region Draedon's Expansion
            if (ModLoader.TryGetMod("DraedonExpansion", out Mod draedonExpansion) && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                if (UnsafeGetItem(draedonExpansion, "StunGun", item))
                {
                    item.damage = 19;
                }

                if (UnsafeGetItem(draedonExpansion, "EnergyPistol", item))
                {
                    item.useTime = 24;
                    item.useAnimation = 24;
                    item.damage = 16;
                }
            }
            #endregion

            #region Calamity Ranger Expansion
            if (ModLoader.TryGetMod("CalamityAmmo", out Mod calAmmo) && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                if (UnsafeGetItem(calAmmo, "SandWorm", item))
                {
                    item.damage = 16;
                    item.useTime = 22;
                    item.useAnimation = 22;
                }

                if (UnsafeGetItem(calAmmo, "SpectreRifle", item))
                {
                    item.damage = 117;
                }

                if (UnsafeGetItem(calAmmo, "PlaguenadeLauncher", item))
                {
                    item.damage = 30;
                }
            }
            #endregion

            #region Clamity
            if (ModLoader.TryGetMod("Clamity", out Mod clam) && InfernalConfig.Instance.CalamityBalanceChanges)
            {
                if (UnsafeGetItem(clam, "MoonstoneKnives", item))
                {
                    item.damage = 8;
                }

                if (UnsafeGetItem(clam, "Obsidigun", item))
                {
                    item.useTime = 18;
                    item.useAnimation = 18;
                }

                if (UnsafeGetItem(clam, "TheGenerator", item))
                {
                    item.damage = 77;
                }

                if (UnsafeGetItem(clam, "MoltenPiercer", item))
                {
                    item.damage = 144;
                    item.useTime = 16;
                    item.useAnimation = 16;
                }

                if (UnsafeGetItem(clam, "ClamitasCrusher", item))
                {
                    item.damage = 130;
                }

                if (UnsafeGetItem(clam, "DepthsEchoRifle", item))
                {
                    item.damage = 40;
                }

                if (UnsafeGetItem(clam, "HellstoneShellfishStaff", item))
                {
                    item.damage = 200;
                }

                if (UnsafeGetItem(clam, "Disease", item))
                {
                    item.damage = 194;
                }

                if (UnsafeGetItem(clam, "PlanterrorStaff", item))
                {
                    item.damage = 100;
                }

                if (UnsafeGetItem(clam, "RoseBow", item))
                {
                    item.damage = 400;
                }

                if (UnsafeGetItem(clam, "AuricKunai", item))
                {
                    item.damage = 140;
                }

                if (UnsafeGetItem(clam, "Omega", item))
                {
                    item.damage = 80;
                }

                if (UnsafeGetItem(clam, "Everest", item))
                {
                    item.damage = 1300;
                }

                if (UnsafeGetItem(clam, "FrozenVolcano", item))
                {
                    item.damage = 2500;
                    item.useTime = 12;
                    item.useAnimation = 12;
                }

                if (UnsafeGetItem(clam, "FrozenStarShuriken", item))
                {
                    item.useAnimation = 8;
                    item.useTime = 8;
                    item.damage = 900;
                }

                if (UnsafeGetItem(clam, "WitheredBoneBow", item))
                {
                    item.damage = 70;
                }
            }
            #endregion

            #region Thorium
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                if (GetItem(thorium, "ValadiumPickaxe", item) || GetItem(thorium, "LodeStonePickaxe", item))
                {
                    item.pick = 190;
                }

                if (GetItem(thorium, "DreadDrill", item) || GetItem(thorium, "DemonBloodDrill", item))
                {
                    item.pick = 200;
                }

                if (InfernalConfig.Instance.ThoriumBalanceChangess)
                {
                    #region Melee
                    #region Pre-Hardmode
                    //The Snowball
                    if (item.type == thorium.Find<ModItem>("TheSnowball").Type)
                    {
                        item.knockBack = 6;
                        item.damage = 20;
                    }

                    //Ice Breaker
                    if (item.type == thorium.Find<ModItem>("IceBreaker").Type)
                    {
                        item.damage = 11;
                    }

                    //Cold Front
                    if (item.type == thorium.Find<ModItem>("ColdFront").Type)
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.damage = 18;
                    }

                    //Bent Zombie Arm
                    if (item.type == thorium.Find<ModItem>("BentZombieArm").Type)
                    {
                        //projectile speed = 15
                        item.damage = 21;
                    }

                    //Coral Slasher
                    if (item.type == thorium.Find<ModItem>("CoralSlasher").Type)
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;

                        item.scale *= 1.5f;
                    }

                    //Coral Polearm
                    if (item.type == FindItem(thorium, "CoralPolearm"))
                    {
                        item.useTime = 21;
                        item.useAnimation = 21;
                        //projectile speed = 5
                    }

                    //Heartstriker
                    if (item.type == FindItem(thorium, "Heartstriker"))
                    {
                        item.useTime = 30;
                        item.damage = 24;
                        item.useAnimation = 30;
                    }

                    //Thorium Blade
                    if (item.type == FindItem(thorium, "ThoriumBlade"))
                    {
                        item.damage = 26;
                        item.useTime = 17;
                        item.useAnimation = 17;
                    }

                    //Thorium Boomerang
                    if (item.type == FindItem(thorium, "ThoriumBoomerang"))
                    {
                        item.damage *= 2;
                        item.useTime = 22;
                        item.useAnimation = 22;
                    }

                    //Thorium Spear
                    if (item.type == FindItem(thorium, "ThoriumSpear"))
                    {
                        item.damage = 22;
                        item.shootSpeed = 8;
                        item.useTime = 18;
                        item.useAnimation = 18;
                    }

                    //Sandstorm Scimitar
                    if (item.type == FindItem(thorium, "dSandStoneScimtar"))
                    {
                        item.damage = 20;
                    }

                    //Sandstone Spear
                    if (item.type == FindItem(thorium, "fSandStoneSpear"))
                    {
                        item.damage = 21;
                        item.shootSpeed = 6;
                        item.useTime = 18;
                        item.useAnimation = 18;
                    }

                    //Thunder Talon
                    if (item.type == FindItem(thorium, "ThunderTalon"))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    //Spoon
                    if (item.type == FindItem(thorium, "Spoon"))
                    {
                        item.damage = 48;

                        item.scale *= 2;
                    }

                    //Kitchen Knife
                    if (item.type == FindItem(thorium, "Knife"))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    //Fork
                    if (item.type == FindItem(thorium, "Fork"))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.damage = 23;
                        item.shootSpeed = 8;
                    }

                    //Whip
                    if (item.type == FindItem(thorium, "Whip"))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.knockBack = (float)1.5;
                    }

                    //Steel Blade
                    if (item.type == FindItem(thorium, "SteelBlade"))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.damage = 30;
                    }

                    //Harpy Talon
                    if (item.type == FindItem(thorium, "HarpyTalon"))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.shootSpeed = 12;
                    }

                    //Bellerose
                    if (item.type == FindItem(thorium, "Bellerose"))
                    {
                        item.useTime = 13;
                        item.useAnimation = 13;
                    }

                    //Blooming BLade
                    if (item.type == FindItem(thorium, "BloomingBlade"))
                    {
                        item.damage = 45;
                        item.useTime = 26;
                        item.useAnimation = 26;

                        item.scale *= 1.5f;
                    }

                    //Pearl Pike
                    if (item.type == FindItem(thorium, "PearlPike"))
                    {
                        item.useTime = 22;
                        item.useAnimation = 22;
                        item.shootSpeed = 8;
                    }

                    //Drenched Dirk
                    if (item.type == FindItem(thorium, "DrenchedDirk"))
                    {
                        item.useTime = 6;
                        item.useAnimation = 6;
                        item.damage = 14;
                        item.shootSpeed = 18;
                    }

                    //Illustrious
                    if (item.type == FindItem(thorium, "Illustrious"))
                    {
                        item.damage = 30;
                    }

                    //Thor's Hammer: Melee
                    if (item.type == FindItem(thorium, "MeleeThorHammer"))
                    {
                        item.damage = 45;
                        item.useTime = 18;
                        item.useAnimation = 18;
                    }

                    //Giant Glowstick
                    if (item.type == FindItem(thorium, "GiantGlowstick"))
                    {
                        item.shootSpeed = 14;
                    }

                    //Sparking Jelly Ball
                    if (item.type == FindItem(thorium, "SparkingJellyBall"))
                    {
                        item.damage = 60;
                    }

                    //Pollen Pike
                    if (item.type == FindItem(thorium, "PollenPike"))
                    {
                        item.useTime = 6;
                        item.useAnimation = 6;
                        item.shootSpeed = 22;
                    }

                    //Whirlpool Saber
                    if (item.type == FindItem(thorium, "WhirlpoolSaber"))
                    {
                        item.shootSpeed = 10;
                    }

                    //Moonlight
                    if (item.type == FindItem(thorium, "Moonlight"))
                    {
                        item.useTime = 20;
                        item.useAnimation = 20;
                        item.shootSpeed = (float)7.5;
                        item.damage = 47;
                    }

                    //Darksteel Broadsword
                    if (item.type == FindItem(thorium, "eDarksteelBroadSword"))
                    {
                        item.useTime = 17;
                        item.useAnimation = 17;
                        item.damage = 40;
                    }

                    //Champion's Swift Blade
                    if (item.type == thorium.Find<ModItem>("ChampionSwiftBlade").Type)
                    {
                        item.DamageType = DamageClass.Melee;
                        item.damage = 50;
                    }

                    //Gorgon's Eye
                    if (item.type == thorium.Find<ModItem>("GorgonsEye").Type)
                    {
                        item.damage = 35;
                    }

                    //Granite Reflector
                    if (item.type == FindItem(thorium, "GraniteReflector"))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.shootSpeed = 14;
                    }

                    //Star Trail
                    if (item.type == thorium.Find<ModItem>("StarTrail").Type)
                    {
                        item.damage = 35;
                    }
                    #endregion

                    #region Hardmode
                    //Durasteel Blade
                    if (item.type == FindItem(thorium, "DurasteelBlade"))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    if (GetItem(thorium, "GrimFlayer", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    if (GetItem(thorium, "BloodDrinker", item))
                    {
                        item.useTime = 13;
                        item.useAnimation = 13;

                        item.scale *= 1.25f;
                    }

                    if (GetItem(thorium, "Saba", item))
                    {
                        item.useAnimation = 15;
                        item.useTime = 30;

                    }

                    if (GetItem(thorium, "DragonTooth", item))
                    {
                        item.crit = 0;

                    }

                    if (GetItem(thorium, "DoomFireAxe", item))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                    }

                    if (GetItem(thorium, "Scalper", item))
                    {
                        item.shootSpeed = 16;
                    }

                    if (GetItem(thorium, "Rapier", item))
                    {
                        item.useTime = 8;
                        item.useAnimation = 8;
                        item.shootSpeed = 8;
                    }

                    if (GetItem(thorium, "BackStabber", item))
                    {
                        item.useTime = 8;
                        item.useAnimation = 8;
                    }

                    if (GetItem(thorium, "RifleSpear", item))
                    {
                        item.useTime = 10;
                        item.useAnimation = 10;
                    }

                    if (GetItem(thorium, "PoseidonCharge", item))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.shootSpeed = 12;
                    }

                    if (GetItem(thorium, "DragonTalon", item))
                    {
                        item.crit = 0;
                        item.damage = 54;
                    }

                    if (GetItem(thorium, "Schmelze", item))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.damage = 90;
                    }

                    if (GetItem(thorium, "Blitzzard", item))
                    {
                        item.useTime = 10;
                        item.useAnimation = 10;
                        item.damage = 60;
                    }

                    if (GetItem(thorium, "Glacier", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                        item.damage = 80;

                        item.scale *= 1.5f;
                    }

                    if (GetItem(thorium, "Executioner", item))
                    {
                        item.scale *= 1.25f;
                    }
                    if (GetItem(thorium, "Executioner", item))
                    {
                        item.scale *= 1.25f;
                    }

                    if (GetItem(thorium, "ShipsHelm", item))
                    {
                        item.useAnimation = 8;
                        item.useTime = 8;
                        item.damage = 80;
                    }

                    if (GetItem(thorium, "TheJuggernaut", item))
                    {
                        item.crit = 26;
                    }

                    if (GetItem(thorium, "MidasGavel", item))
                    {
                        item.damage = 200;
                    }

                    if (GetItem(thorium, "LodeStoneClaymore", item))
                    {
                        item.damage = 75;
                    }

                    if (GetItem(thorium, "ValadiumSlicer", item))
                    {
                        item.damage = 131;
                    }

                    if (GetItem(thorium, "TheSeaMine", item))
                    {
                        item.damage = 80;
                    }

                    if (GetItem(thorium, "BloodyHighClaws", item))
                    {
                        item.crit = 16;
                        item.damage = 73;
                    }

                    if (GetItem(thorium, "PrimesFury", item))
                    {
                        item.damage = 200;
                    }

                    if (GetItem(thorium, "EyeoftheDestroyer", item))
                    {
                        item.damage = 110;
                    }

                    if (GetItem(thorium, "ClimbersIceAxe", item))
                    {
                        item.damage = 140;
                    }

                    if (GetItem(thorium, "TitanBoomerang", item))
                    {
                        item.damage = 115;
                    }

                    if (GetItem(thorium, "TitanSword", item))
                    {
                        item.damage = 88;
                    }

                    if (GetItem(thorium, "GoldenLocks", item))
                    {
                        item.useTime = 4;
                        item.useAnimation = 4;
                        item.damage = 39;
                    }

                    if (GetItem(thorium, "SoulRender", item))
                    {
                        item.useTime = 20;
                        item.useAnimation = 20;
                        item.damage = 200;
                    }

                    if (GetItem(thorium, "DreadRazor", item))
                    {
                        item.useTime = 20;
                        item.useAnimation = 20;
                        item.damage = 177;
                    }

                    if (GetItem(thorium, "DemonBloodSword", item))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.crit = 0;
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "IllumiteBlade", item))
                    {
                        item.crit = 0;
                    }

                    if (GetItem(thorium, "SolScorchedSlab", item))
                    {
                        item.damage = 158;

                        item.scale *= 1.5f;
                    }

                    if (GetItem(thorium, "TheBlackBlade", item))
                    {
                        item.shootSpeed = 20;
                    }

                    if (GetItem(thorium, "DreadFork", item))
                    {
                        item.crit = 0;
                        item.damage = 143;
                    }

                    if (GetItem(thorium, "DemonBloodSpear", item))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.crit = 0;
                        item.damage = 100;
                    }

                    if (GetItem(thorium, "IllumiteSpear", item))
                    {
                        item.damage = 76;
                    }

                    if (GetItem(thorium, "Spearmint", item))
                    {
                        item.damage = 108;
                    }

                    if (GetItem(thorium, "SoulReaver", item))
                    {
                        item.useTime = 10;
                        item.useAnimation = 10;
                        item.damage = 98;
                    }

                    if (GetItem(thorium, "LingeringWill", item))
                    {
                        item.useTime = 24;
                        item.useAnimation = 24;
                        item.damage = 265;
                        item.shootSpeed = 20;
                    }

                    if (GetItem(thorium, "WyvernSlayer", item))
                    {
                        item.damage = 254;
                    }

                    if (GetItem(thorium, "GolemsGaze", item))
                    {
                        item.damage = 192;
                    }

                    if (GetItem(thorium, "LivewireCrasher", item))
                    {
                        item.damage = 87;
                    }

                    if (GetItem(thorium, "TheWhirlpool", item))
                    {
                        item.damage = 184;
                    }

                    if (GetItem(thorium, "MantisShrimpPunch", item))
                    {
                        item.damage = 264;
                    }

                    if (GetItem(thorium, "EclipseFang", item))
                    {
                        item.damage = 250;

                        item.scale *= 1.5f;
                    }

                    if (GetItem(thorium, "TerrariumSaber", item))
                    {
                        item.damage = 230;
                        item.crit = 0;
                    }

                    if (GetItem(thorium, "Skadoosh", item))
                    {
                        item.damage = 53;
                    }

                    if (GetItem(thorium, "TerrariumHyperDisc", item))
                    {
                        item.shootSpeed = 14;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "BloodGlory", item))
                    {
                        item.damage = 400;
                    }

                    if (GetItem(thorium, "QuakeGauntlet", item))
                    {
                        item.useTime = 3;
                        item.useAnimation = 3;
                        item.shootSpeed = 20;
                        item.damage = 147;
                    }

                    if (GetItem(thorium, "SevenSeasDevastator", item))
                    {
                        item.damage = 300;
                    }

                    if (GetItem(thorium, "OceansJudgement", item))
                    {
                        item.damage = 600;
                        item.shootSpeed = 25;
                    }

                    if (GetItem(thorium, "TerrariansLastKnife", item))
                    {
                        item.damage = 900;
                    }

                    #endregion
                    #endregion

                    #region Ranged
                    #region Pre-Hardmode
                    //Frost Fury
                    if (item.type == thorium.Find<ModItem>("FrostFury").Type)
                    {
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }

                    //Frost Pelter
                    if (UnsafeGetItem(thorium, "FrostPelter", item))
                    {
                        item.shootSpeed = 20;
                    }

                    //Coral Crossbow
                    if (item.type == thorium.Find<ModItem>("CoralCrossbow").Type)
                    {
                        item.useTime = 26;
                        item.useAnimation = 26;
                        item.damage = 10;
                    }

                    //Thorium Revolver
                    if (item.type == thorium.Find<ModItem>("ThoriumRevolver").Type)
                    {
                        item.damage = 17;
                    }

                    //Talon Burst
                    if (item.type == thorium.Find<ModItem>("TalonBurst").Type)
                    {
                        item.damage = 9;
                    }

                    //Feather Foe
                    if (item.type == thorium.Find<ModItem>("FeatherFoe").Type)
                    {
                        item.damage = 22;
                    }

                    //Blooming Bow
                    if (item.type == thorium.Find<ModItem>("BloomingBow").Type)
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    //Harpy Pelter
                    if (item.type == thorium.Find<ModItem>("HarpyPelter").Type)
                    {
                        item.damage = 15;
                    }

                    //Spud Bomber
                    if (item.type == thorium.Find<ModItem>("SpudBomber").Type)
                    {
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }

                    //Golbin Warpipe
                    if (item.type == thorium.Find<ModItem>("GoblinWarpipe").Type)
                    {
                        item.useAnimation = 20;
                        item.useTime = 20;
                    }

                    //Aqua Pelter
                    if (item.type == thorium.Find<ModItem>("AquaPelter").Type)
                    {
                        item.useTime = 6;
                        item.useAnimation = 6;
                        item.damage = 12;
                    }

                    //Thor's Hammer: Ranged
                    if (item.type == thorium.Find<ModItem>("RangedThorHammer").Type)
                    {
                        item.damage = 40;
                    }

                    //Guano Gunner
                    if (item.type == thorium.Find<ModItem>("GuanoGunner").Type)
                    {
                        item.damage = 24;
                    }

                    //Stream Sting
                    if (item.type == thorium.Find<ModItem>("StreamSting").Type)
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.damage = 16;
                    }

                    //Darksteel Crossbow
                    if (item.type == thorium.Find<ModItem>("gDarkSteelCrossBow").Type)
                    {
                        item.damage = 56;
                    }

                    //Elephant Gun
                    if (item.type == thorium.Find<ModItem>("ElephantGun").Type)
                    {
                        item.damage = 45;
                    }

                    //Champion's Trifecta-Shot
                    if (item.type == thorium.Find<ModItem>("ChampionsTrifectaShot").Type)
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    //Granite Crossbow
                    if (item.type == thorium.Find<ModItem>("GraniteCrossbow").Type)
                    {
                        item.useAnimation = 10;
                        item.useTime = 10;
                    }

                    //Energy Storm Bolter
                    if (item.type == thorium.Find<ModItem>("EnergyStormBolter").Type)
                    {
                        item.damage = 26;
                    }

                    //Hit Scanner
                    if (item.type == thorium.Find<ModItem>("HitScanner").Type)
                    {
                        item.damage = 30;
                        item.useAnimation = 16;
                        item.useTime = 16;
                    }
                    #endregion

                    #region Hardmode
                    if (GetItem(thorium, "DurasteelRepeater", item))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.damage = 45;
                    }

                    if (GetItem(thorium, "CometCrossfire", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                        item.crit = 16;
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "CupidString", item))
                    {
                        item.crit = 26;
                    }

                    if (GetItem(thorium, "NagaRecurve", item))
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                    }

                    if (GetItem(thorium, "FleshBow", item))
                    {
                        item.useTime = 18;
                        item.useAnimation = 18;
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "CinderString", item))
                    {
                        item.damage = 50;
                    }

                    if (GetItem(thorium, "SpineBuster", item))
                    {
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }

                    if (GetItem(thorium, "DragonsGaze", item))
                    {
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "BloodBelcher", item))
                    {
                        item.shootSpeed = 20;
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "Funggat", item))
                    {
                        item.damage = 36;
                    }

                    if (GetItem(thorium, "VegaPhaser", item))
                    {
                        item.useTime = 20;
                        item.useAnimation = 20;
                    }

                    if (GetItem(thorium, "BulletStorm", item))
                    {
                        item.useTime = 7;
                        item.useAnimation = 7;
                    }

                    if (GetItem(thorium, "ArmorBane", item))
                    {
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "ShusWrath", item))
                    {
                        item.damage = 47;
                    }

                    if (GetItem(thorium, "TommyGun", item))
                    {
                        item.damage = 26;
                    }

                    if (GetItem(thorium, "FreezeRay", item))
                    {
                        item.damage = 65;
                    }

                    if (GetItem(thorium, "GlacialSting", item))
                    {
                        item.useTime = 18;
                        item.useAnimation = 18;
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "HandCannon", item))
                    {
                        item.damage = 80;
                    }

                    if (GetItem(thorium, "Obliterator", item))
                    {
                        item.crit = 0;
                        item.damage = 30;
                    }

                    if (GetItem(thorium, "LodeStoneBow", item))
                    {
                        item.damage = 30;
                    }

                    if (GetItem(thorium, "LodeStoneQuickDraw", item))
                    {
                        item.damage = 15;
                    }

                    if (GetItem(thorium, "ValadiumBow", item))
                    {
                        item.damage = 35;
                    }

                    if (GetItem(thorium, "ValadiumFoeBlaster", item))
                    {
                        item.damage = 26;
                    }

                    if (GetItem(thorium, "OrichalcumPelter", item))
                    {
                        item.useTime = 17;
                        item.useAnimation = 17;
                    }

                    if (GetItem(thorium, "MythrilPelter", item))
                    {
                        item.damage = 50;
                    }

                    if (GetItem(thorium, "ChargedSplasher", item))
                    {
                        item.damage = 36;
                    }

                    if (GetItem(thorium, "AdamantiteCarbine", item))
                    {
                        item.damage = 114;
                    }

                    if (GetItem(thorium, "Trigun", item))
                    {
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "Shockbuster", item))
                    {
                        item.damage = 44;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "DestroyersRage", item))
                    {
                        item.useAnimation = 9;
                        item.useTime = 9;
                        item.damage = 50;
                    }

                    if (GetItem(thorium, "TitanBow", item))
                    {
                        item.damage = 90;
                    }

                    if (GetItem(thorium, "LittleRed", item))
                    {
                        item.damage = 148;
                        item.useTime = 37;
                        item.useAnimation = 37;
                    }

                    if (GetItem(thorium, "DecayingSorrow", item))
                    {
                        item.damage = 80;
                    }

                    if (GetItem(thorium, "DemonBloodCrossBow", item))
                    {
                        item.useTime = 3;
                        item.useAnimation = 6;
                    }

                    if (GetItem(thorium, "IllumiteShotbow", item))
                    {
                        item.crit = 0;
                        item.damage = 36;
                    }

                    if (GetItem(thorium, "IllumiteBlaster", item))
                    {
                        item.crit = 0;
                        item.damage = 36;
                    }

                    if (GetItem(thorium, "ShadowFlareBow", item))
                    {
                        item.damage = 146;
                    }

                    if (GetItem(thorium, "TheBlackBow", item))
                    {
                        item.useTime = 4;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "UmbraBlaster", item))
                    {
                        item.damage = 35;
                    }

                    if (GetItem(thorium, "BeetleBlaster", item))
                    {
                        item.damage = 26;
                    }

                    if (GetItem(thorium, "IllumiteBarrage", item))
                    {
                        item.damage = 31;
                        item.crit = 0;
                        item.useTime = 12;
                        item.useAnimation = 36;
                    }

                    if (GetItem(thorium, "DreadLauncher", item))
                    {
                        item.damage = 93;
                    }

                    if (GetItem(thorium, "TheMassacre", item))
                    {
                        item.damage = 260;
                    }

                    if (GetItem(thorium, "SupersonicBomber", item))
                    {
                        item.damage = 88;
                    }

                    if (GetItem(thorium, "SpiritBreaker", item))
                    {
                        item.damage = 47;
                        item.crit = 4;
                    }

                    if (GetItem(thorium, "TrenchSpitter", item))
                    {
                        item.damage = 57;
                    }

                    if (GetItem(thorium, "TerrariumLongbow", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    if (GetItem(thorium, "TerrariumPulseRifle", item))
                    {
                        item.damage = 104;
                    }

                    if (GetItem(thorium, "WyrmDecimator", item))
                    {
                        item.damage = 600;
                    }

                    if (GetItem(thorium, "OmniBow", item))
                    {
                        item.damage = 220;
                    }

                    if (GetItem(thorium, "OmniCannon", item))
                    {
                        item.damage = 590;
                    }

                    if (GetItem(thorium, "TheJavelin", item))
                    {
                        item.damage = 1350;
                    }

                    if (GetItem(thorium, "QuasarsFlare", item))
                    {
                        item.useTime = 11;
                        item.useAnimation = 11;
                        item.damage = 1000;
                    }
                    #endregion
                    #endregion

                    #region Magic
                    #region Pre-Hardmode
                    //Ice Cube
                    if (item.type == thorium.Find<ModItem>("IceCube").Type)
                    {
                        item.mana = 2;
                        item.damage = 10;
                    }

                    //Flurry
                    if (item.type == thorium.Find<ModItem>("Flurry").Type)
                    {
                        item.mana = 2;
                        item.useTime = 8;
                        item.useAnimation = 8;
                    }

                    //Opal Staff
                    if (item.type == thorium.Find<ModItem>("OpalStaff").Type)
                    {
                        item.mana = 4;
                        item.damage = 27;
                        item.useTime = 36;
                        item.useAnimation = 36;
                    }

                    //Aquamarine Staff
                    if (item.type == thorium.Find<ModItem>("AquamarineStaff").Type)
                    {
                        item.mana = 4;
                        item.damage = 20;
                        item.useTime = 33;
                        item.useAnimation = 33;
                    }

                    //Vessel Buster
                    if (item.type == thorium.Find<ModItem>("VesselBuster").Type)
                    {
                        item.useTime = 10;
                        item.useAmmo = 10;
                    }

                    //Magic Conch
                    if (item.type == thorium.Find<ModItem>("MagicConch").Type)
                    {
                        item.mana = 10;
                        item.useTime = 2;
                        item.useAnimation = 10;
                        item.damage = 16;
                    }

                    //Detached Blaster
                    if (item.type == thorium.Find<ModItem>("DetachedBlaster").Type)
                    {
                        item.damage = 34;
                    }

                    //Shadoowflame Staff
                    if (item.type == thorium.Find<ModItem>("ShadowflameStaff").Type)
                    {
                        item.damage *= 2;
                    }

                    //Blooming Staff
                    if (item.type == thorium.Find<ModItem>("BloomingStaff").Type)
                    {
                        item.damage = 16;
                        item.useTime = 2;
                        item.useAnimation = 6;
                    }

                    //Thor's Hammer: Magic
                    if (item.type == thorium.Find<ModItem>("MagicThorHammer").Type)
                    {
                        item.damage = 44;
                    }

                    if (UnsafeGetItem(thorium, "InfernoStaff", item))
                    {
                        item.damage = 45;
                    }

                    //Dark Tome
                    if (item.type == thorium.Find<ModItem>("DarkTome").Type)
                    {
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }

                    //Jelly Pond Wand
                    if (item.type == thorium.Find<ModItem>("JellyPondWand").Type)
                    {
                        item.damage = 34;
                        item.shootSpeed = 10;
                    }

                    //Vampire Scepter
                    if (item.type == thorium.Find<ModItem>("VampireScepter").Type)
                    {
                        item.shootSpeed = 16;
                    }

                    //Phantom Camera
                    if (item.type == thorium.Find<ModItem>("PhantomCamera").Type)
                    {
                        item.damage = 85;
                    }

                    //Eel-rod
                    if (item.type == thorium.Find<ModItem>("Eelrod").Type)
                    {
                        item.damage = 24;
                    }

                    //Bloody Wand
                    if (item.type == thorium.Find<ModItem>("BloodyWand").Type)
                    {
                        item.damage = 18;
                    }

                    //Energy Projector
                    if (item.type == thorium.Find<ModItem>("EnergyProjector").Type)
                    {
                        item.damage = 30;
                    }

                    //Champion's Bomber Staff
                    if (item.type == thorium.Find<ModItem>("ChampionBomberStaff").Type)
                    {
                        item.damage = 7;
                    }

                    //Particle Whip
                    if (item.type == thorium.Find<ModItem>("ParticleWhip").Type)
                    {
                        item.damage = 38;
                    }
                    #endregion

                    #region Hardmode
                    if (GetItem(thorium, "NightStaff", item))
                    {
                        item.useTime = 17;
                        item.useAnimation = 17;
                    }

                    if (GetItem(thorium, "CobaltStaff", item))
                    {
                        item.shootSpeed = 13;
                        item.useTime = 8;
                        item.useAnimation = 8;
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "PalladiumStaff", item))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.damage = 48;
                    }

                    if (GetItem(thorium, "FrostPlagueStaff", item))
                    {
                        item.shootSpeed = 16;
                        item.damage = 28;
                    }

                    if (GetItem(thorium, "SeaFoamScepter", item))
                    {
                        item.shootSpeed = 12;
                        item.damage = 44;
                    }

                    if (GetItem(thorium, "PrismStaff", item))
                    {
                        item.damage = 55;
                    }

                    if (GetItem(thorium, "BloodClotStaff", item))
                    {
                        item.damage = 45;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "Stalagmite", item))
                    {
                        item.damage = 58;
                    }

                    if (GetItem(thorium, "DragonsBreath", item))
                    {
                        item.shootSpeed = 18;
                        item.damage = 25;
                    }

                    if (GetItem(thorium, "Cyclone", item))
                    {
                        item.damage = 100;
                    }

                    if (GetItem(thorium, "DevilDagger", item))
                    {
                        item.shootSpeed = 20;
                        item.damage = 45;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "GeomancersBrush", item))
                    {
                        item.damage = 90;
                    }

                    if (GetItem(thorium, "DynastyWarFan", item))
                    {
                        item.shootSpeed = 15;
                        item.useTime = 18;
                        item.useAnimation = 18;
                    }

                    if (GetItem(thorium, "KineticKnife", item))
                    {
                        item.damage = 41;
                    }

                    if (GetItem(thorium, "HeavensGate", item))
                    {
                        item.damage = 38;
                    }

                    if (GetItem(thorium, "DutchmansAvarice", item))
                    {
                        item.damage = 60;
                    }

                    //Lodestone Staff
                    if (GetItem(thorium, "LodeStoneStaff", item))
                    {
                        item.damage = 38;
                    }

                    if (GetItem(thorium, "ValadiumStaff", item))
                    {
                        item.useTime = 30;
                        item.useAnimation = 30;
                        item.damage = 44;
                    }

                    if (GetItem(thorium, "MythrilStaff", item))
                    {
                        item.damage = 56;
                    }

                    if (GetItem(thorium, "OrichalcumStaff", item))
                    {
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "TitaniumStaff", item))
                    {
                        item.damage = 60;
                    }

                    if (GetItem(thorium, "AdamantiteStaff", item))
                    {
                        item.damage = 50;
                        item.useAnimation = 22;
                        item.useTime = 22;
                    }

                    if (GetItem(thorium, "HallowedStaff", item))
                    {
                    }

                    if (GetItem(thorium, "StaticProd", item))
                    {
                        item.useTime = 4;
                    }

                    if (GetItem(thorium, "WondrousWand", item))
                    {
                        item.damage = 86;
                    }

                    if (GetItem(thorium, "ChlorophyteStaff", item))
                    {
                        item.damage = 51;
                    }

                    if (GetItem(thorium, "SnowWhite", item))
                    {
                        item.damage = 56;
                    }

                    if (GetItem(thorium, "BassBooster", item))
                    {
                        item.damage = 60;
                    }

                    if (GetItem(thorium, "DarkFlame", item))
                    {
                        item.damage = 120;
                    }

                    if (GetItem(thorium, "Razorlash", item))
                    {
                        item.damage = 68;
                    }

                    if (GetItem(thorium, "DemonBloodStaff", item))
                    {
                        item.damage = 43;
                    }

                    if (GetItem(thorium, "DarkGrip", item))
                    {
                        item.damage = 100;
                    }

                    if (GetItem(thorium, "Legacy", item))
                    {
                        item.damage = 120;
                    }

                    if (GetItem(thorium, "LightningStaff", item))
                    {
                        item.damage = 145;
                        item.useTime = 18;
                        item.useAnimation = 18;
                    }

                    if (GetItem(thorium, "EruptingFlare", item))
                    {
                        item.damage = 104;
                    }

                    if (GetItem(thorium, "GodKiller", item))
                    {
                        item.damage = 88;
                    }

                    if (GetItem(thorium, "DreadBlaster", item))
                    {
                        item.damage = 64;
                    }

                    if (GetItem(thorium, "PLG", item))
                    {
                        item.damage = 55;
                    }

                    if (GetItem(thorium, "Spores", item))
                    {
                        item.useTime = 9;
                        item.useAnimation = 9;
                    }

                    if (GetItem(thorium, "CharonsBeacon", item))
                    {
                        item.damage = 110;
                    }

                    if (GetItem(thorium, "SuperPlasmaCannon", item))
                    {
                        item.damage = 75;
                    }

                    if (GetItem(thorium, "OldGodsVision", item))
                    {
                        item.damage = 45;
                    }

                    if (GetItem(thorium, "AncientLight", item))
                    {
                        item.damage = 110;
                    }

                    if (GetItem(thorium, "AncientSpark", item))
                    {
                        item.damage = 120;
                    }

                    if (UnsafeGetItem(thorium, "ChromaticFury", item))
                    {
                        item.damage = 30;
                    }

                    if (GetItem(thorium, "TerrariumSageStaff", item))
                    {
                        item.damage = 120;
                    }

                    if (GetItem(thorium, "CatsEyeGreatStaff", item))
                    {
                        item.damage = 210;
                    }

                    if (GetItem(thorium, "TeleologicImposition", item))
                    {
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "AlmanacofAgony", item))
                    {
                        item.damage = 850;
                    }

                    if (GetItem(thorium, "DevilsClaw", item))
                    {
                        item.damage = 600;
                    }

                    if (GetItem(thorium, "NorthernLight", item))
                    {
                        item.damage = 300;
                    }
                    #endregion
                    #endregion

                    #region Summoner
                    #region Pre-Hardmode
                    //Storm Hatchling Staff
                    if (item.type == thorium.Find<ModItem>("StormHatchlingStaff").Type)
                    {
                        item.damage = 26;
                    }

                    //Meteor Head Staff
                    if (item.type == thorium.Find<ModItem>("MeteorHeadStaff").Type)
                    {
                        item.damage = 21;
                    }

                    //Bleeding Heart Staff
                    if (item.type == thorium.Find<ModItem>("BleedingHeartStaff").Type)
                    {
                        item.damage = 35;
                    }

                    //Taboo Wand
                    if (item.type == thorium.Find<ModItem>("TabooWand").Type)
                    {
                        item.damage = 30;
                    }

                    //Weed Eater
                    if (item.type == thorium.Find<ModItem>("WeedEater").Type)
                    {
                        item.damage = 26;
                    }

                    //Arsenal Staff
                    if (item.type == thorium.Find<ModItem>("ArsenalStaff").Type)
                    {
                        item.damage = 30;
                    }

                    //Yarn Ball
                    if (item.type == thorium.Find<ModItem>("YarnBall").Type)
                    {
                        item.damage = 21;
                    }

                    //Strongest Link
                    if (item.type == thorium.Find<ModItem>("StrongestLink").Type)
                    {
                        item.damage = 65;
                    }

                    //Strange Skull
                    if (item.type == thorium.Find<ModItem>("StrangeSkull").Type)
                    {
                        item.damage = 45;
                    }

                    //Boulder Probe Staff
                    if (item.type == thorium.Find<ModItem>("BoulderProbeStaff").Type)
                    {
                        item.damage = 35;
                    }
                    #endregion

                    #region Hardmode
                    if (UnsafeGetItem(thorium, "Thrombosis", item))
                    {
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "DraconicMagmaStaff", item))
                    {
                        item.damage = 35;
                    }

                    if (GetItem(thorium, "CrimsonHoundStaff", item))
                    {
                        item.damage = 37;
                    }

                    if (GetItem(thorium, "LadyLight", item))
                    {
                        item.damage = 34;
                    }

                    if (GetItem(thorium, "MortarStaff", item))
                    {
                        item.damage = 122;
                    }

                    if (GetItem(thorium, "BeholderStaff", item))
                    {
                        item.damage = 33;
                    }

                    if (GetItem(thorium, "AeonStaff", item))
                    {
                        item.damage = 46;
                    }

                    if (GetItem(thorium, "SteamgunnerController", item))
                    {
                        item.damage = 24;
                    }

                    if (GetItem(thorium, "ValkyrieBlade", item))
                    {
                        item.damage = 53;
                    }

                    if (GetItem(thorium, "CorrodlingStaff", item))
                    {
                        item.damage = 72;
                    }

                    if (GetItem(thorium, "EyeofOdin", item))
                    {
                        item.damage = 75;
                    }

                    if (GetItem(thorium, "VoltModule", item))
                    {
                        item.damage = 170;
                    }

                    if (GetItem(thorium, "TheIncubator", item))
                    {
                        item.damage = 88;
                    }

                    if (GetItem(thorium, "NebulaReflection", item))
                    {
                        item.damage = 133;
                    }

                    if (GetItem(thorium, "TerrariumEnigmaStaff", item))
                    {
                        item.damage = 55;
                    }

                    if (GetItem(thorium, "EmberStaff", item))
                    {
                        item.damage = 260;
                    }

                    if (GetItem(thorium, "PrometheanStaff", item))
                    {
                        item.damage = 900;
                    }
                    #endregion
                    #endregion

                    #region Rogue & Thrower
                    #region Pre-Hardmode
                    //Stone Throwing Spear
                    if (item.type == thorium.Find<ModItem>("StoneThrowingSpear").Type)
                    {
                        item.damage = 16;
                    }

                    //Cactus Needle
                    if (item.type == thorium.Find<ModItem>("CactusNeedle").Type)
                    {
                        item.shootSpeed = 15;
                        item.useTime = 10;
                        item.useAnimation = 10;
                        item.damage = 13;
                    }

                    //Iron Tomahawk
                    if (item.type == thorium.Find<ModItem>("IronTomahawk").Type)
                    {
                        item.useTime = 22;
                        item.useAnimation = 22;
                        item.damage = 11;
                    }

                    //Lead Tomahawk
                    if (item.type == thorium.Find<ModItem>("LeadTomahawk").Type)
                    {
                        item.shootSpeed = 14;
                        item.damage = 15;
                    }

                    //Demonite Tomahawk
                    if (item.type == thorium.Find<ModItem>("DemoniteTomahawk").Type)
                    {
                        item.useTime = 20;
                        item.useAnimation = 20;
                    }

                    //Crimtane Tomahawk
                    if (item.type == thorium.Find<ModItem>("CrimtaneTomahawk").Type)
                    {
                        item.shootSpeed = 16;
                    }

                    //Lasting Pliers
                    if (item.type == thorium.Find<ModItem>("LastingPliers").Type)
                    {
                        item.damage = 26;
                        item.useAnimation = 26;
                        item.useTime = 26;
                    }

                    //Crude Bat
                    if (item.type == thorium.Find<ModItem>("BaseballBat").Type)
                    {
                        item.useTime = 6;
                        item.useAnimation = 6;
                        item.damage = 14;
                    }

                    //Coral Caltrop
                    if (item.type == thorium.Find<ModItem>("CoralCaltrop").Type)
                    {
                        item.damage = 13;
                    }

                    //Severed Hand
                    if (item.type == thorium.Find<ModItem>("SeveredHand").Type)
                    {
                        item.damage = 20;
                        item.shootSpeed = 12;
                    }

                    //Thorium Dagger
                    if (item.type == thorium.Find<ModItem>("ThoriumDagger").Type)
                    {
                        item.shootSpeed = 18;
                        item.damage = 30;
                    }

                    //Bolas
                    if (item.type == thorium.Find<ModItem>("Bolas").Type)
                    {
                        item.damage = 35;
                    }

                    //Obsidian Striker
                    if (item.type == thorium.Find<ModItem>("ObsidianStriker").Type)
                    {
                        item.damage = 21;
                    }

                    //Sandstone Throwing Knife
                    if (item.type == thorium.Find<ModItem>("gSandStoneThrowingKnife").Type)
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                        item.damage = 20;
                    }

                    //Shinobi Slicer
                    if (item.type == thorium.Find<ModItem>("ShinobiSlicer").Type)
                    {
                        item.shootSpeed = 8;
                        item.damage = 20;
                    }

                    //Gel Glove
                    if (item.type == thorium.Find<ModItem>("GelGlove").Type)
                    {
                        item.damage = 30;
                        item.shootSpeed = 15;
                    }

                    //Steel Throwing Axe
                    if (item.type == thorium.Find<ModItem>("SteelThrowingAxe").Type)
                    {
                        item.shootSpeed = 10;
                        item.damage = 30;
                    }

                    //Blooming Shuriken
                    if (item.type == thorium.Find<ModItem>("BloomingShuriken").Type)
                    {
                        item.useTime = 30;
                        item.useAnimation = 30;
                        item.damage = 15;
                    }

                    //Harpy’s Barrage
                    if (item.type == thorium.Find<ModItem>("HarpiesBarrage").Type)
                    {
                        item.damage = 36;
                    }

                    //Spike Bomb
                    if (item.type == thorium.Find<ModItem>("SpikeBomb").Type)
                    {
                        item.shootSpeed = 5;
                    }

                    //Goblin War Spear
                    if (item.type == thorium.Find<ModItem>("GoblinWarSpear").Type)
                    {
                        item.shootSpeed = 14;
                        item.damage = 32;
                    }

                    //Meteorite Cluster Bomb
                    if (item.type == thorium.Find<ModItem>("MeteoriteClusterBomb").Type)
                    {
                        item.damage = 20;
                    }

                    //Aquaite Knife
                    if (item.type == thorium.Find<ModItem>("AquaiteKnife").Type)
                    {
                        item.useAnimation = 15;
                        item.useTime = 15;
                        item.damage = 21;
                    }

                    //Molten Knife
                    if (item.type == thorium.Find<ModItem>("MoltenKnife").Type)
                    {
                        item.damage = 40;
                        item.useAnimation = 24;
                        item.useTime = 24;
                    }

                    //Arcane Anelace
                    if (item.type == thorium.Find<ModItem>("ArcaneAnelace").Type)
                    {
                        item.damage = 30;
                    }

                    //Naiad's Shiv
                    if (item.type == thorium.Find<ModItem>("NaiadShiv").Type)
                    {
                        item.useTime = 8;
                        item.useAnimation = 8;
                        item.damage = 27;
                    }

                    //Spiky Caltrop
                    if (item.type == thorium.Find<ModItem>("SpikyCaltrop").Type)
                    {
                        item.damage = 24;
                    }

                    //Phase Chopper
                    if (item.type == thorium.Find<ModItem>("PhaseChopper").Type)
                    {
                        item.damage = 22;
                    }

                    //Wack Wrench
                    if (item.type == thorium.Find<ModItem>("WackWrench").Type)
                    {
                        item.damage = 28;
                    }

                    //Champion's God Hand
                    if (item.type == thorium.Find<ModItem>("ChampionsGodHand").Type)
                    {
                        item.damage = 55;
                        item.shootSpeed = 20;
                    }

                    //Bronze Throwing Axe
                    if (item.type == thorium.Find<ModItem>("BronzeThrowingAxe").Type)
                    {
                        item.shootSpeed = 10;
                        item.damage = 39;
                    }

                    //Bronze Throwing Axe
                    if (item.type == thorium.Find<ModItem>("GraniteThrowingAxe").Type)
                    {
                        item.shootSpeed = 10;
                        item.damage = 37;
                    }

                    //Light's Anquish
                    if (item.type == thorium.Find<ModItem>("LightAnguish").Type)
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.shootSpeed = 16;
                        item.damage = 42;
                    }

                    //Embowellment
                    if (item.type == thorium.Find<ModItem>("Embowelment").Type)
                    {
                        item.useTime = 18;
                        item.useAnimation = 18;
                        item.shootSpeed = 16;
                        item.damage = 56;
                    }

                    //Gauss Flinger
                    if (item.type == thorium.Find<ModItem>("GaussFlinger").Type)
                    {
                        item.useTime = 10;
                        item.useAnimation = 10;
                    }
                    #endregion

                    #region Hardmode
                    //Durasteel Throwing Spear
                    if (item.type == thorium.Find<ModItem>("DurasteelThrowingSpear").Type)
                    {
                        item.damage = 70;
                    }

                    //Eviscerating Claw
                    if (item.type == thorium.Find<ModItem>("EvisceratingClaw").Type)
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.damage = 39;
                    }

                    //Kunai
                    if (item.type == thorium.Find<ModItem>("Kunai").Type)
                    {
                        item.damage = 60;
                    }

                    //Venom Kunai
                    if (item.type == thorium.Find<ModItem>("VenomKunai").Type)
                    {
                        item.damage = 66;
                    }

                    //Corrupter's Balloon, Crystal Balloon, Festering Balloon
                    if (item.type == thorium.Find<ModItem>("CorrupterBalloon").Type || item.type == thorium.Find<ModItem>("CrystalBalloon").Type || item.type == thorium.Find<ModItem>("FesteringBalloon").Type)
                    {
                        item.useTime = 10;
                        item.useAnimation = 10;
                        item.shootSpeed = 13;
                        item.damage = 50;
                    }

                    if (GetItem(thorium, "FungalPopper", item))
                    {
                        item.damage = 41;
                    }

                    if (GetItem(thorium, "MorelGrenade", item))
                    {
                        item.damage = 48;
                    }

                    if (GetItem(thorium, "ShadowTippedJavelin", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    if (GetItem(thorium, "HotPot", item))
                    {
                        item.damage = 54;
                    }

                    if (GetItem(thorium, "RocketFist", item))
                    {
                        item.useTime = 15;
                        item.useAnimation = 15;
                    }

                    if (GetItem(thorium, "VoltHatchet", item))
                    {
                        item.damage = 96;
                    }

                    if (GetItem(thorium, "AxeBlade", item))
                    {
                        item.damage = 67;
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    if (GetItem(thorium, "LegionOrnament", item))
                    {
                        item.damage = 51;
                    }

                    if (GetItem(thorium, "TheCryoFang", item))
                    {
                        item.shootSpeed = 22;
                        item.damage = 82;
                        item.useTime = 22;
                        item.useAnimation = 22;
                    }

                    if (GetItem(thorium, "CaptainsPoniard", item))
                    {
                        item.useTime = 15;
                        item.useAnimation = 15;
                        item.damage = 41;
                    }

                    if (GetItem(thorium, "HellRoller", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                        item.damage = 66;
                    }

                    if (GetItem(thorium, "ClockWorkBomb", item))
                    {
                        item.damage = 50;
                        item.shootSpeed = 14;
                    }

                    if (GetItem(thorium, "MagicCard", item))
                    {
                        item.crit = 21;
                        item.shootSpeed = 14;
                        item.damage = 60;
                        item.useTime = 8;
                        item.useAnimation = 8;
                    }

                    if (GetItem(thorium, "SoulBomb", item))
                    {
                        item.damage = 140;
                    }

                    if (GetItem(thorium, "SparkTaser", item))
                    {
                        item.damage = 65;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "HadronCollider", item))
                    {
                        item.useTime = 20;
                        item.useAnimation = 20;
                    }

                    if (GetItem(thorium, "Carnwennan", item))
                    {
                        item.useTime = 13;
                        item.useAnimation = 13;
                    }

                    if (GetItem(thorium, "TrueCarnwennan", item))
                    {
                        item.useTime = 13;
                        item.useAnimation = 13;
                    }

                    if (GetItem(thorium, "RiftTearer", item))
                    {
                        item.damage = 150;
                    }

                    if (GetItem(thorium, "StalkersSnippers", item))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.damage = 124;
                    }

                    if (GetItem(thorium, "TrueLightAnguish", item))
                    {
                        item.shootSpeed = 26;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "TrueEmbowelment", item))
                    {
                        item.shootSpeed = 26;
                        item.useTime = 30;
                        item.useAnimation = 30;
                        item.damage = 90;
                    }

                    if (GetItem(thorium, "BudBomb", item))
                    {
                        item.damage = 38;
                    }

                    if (GetItem(thorium, "LihzahrdKukri", item))
                    {
                        item.damage = 99;
                    }

                    if (GetItem(thorium, "ProximityMine", item))
                    {
                        item.damage = 111;
                    }

                    if (GetItem(thorium, "Soulslasher", item))
                    {
                        item.damage = 109;
                    }

                    if (GetItem(thorium, "BugenkaiShuriken", item))
                    {
                        item.damage = 85;
                    }

                    if (GetItem(thorium, "ShadeKunai", item))
                    {
                        item.useTime = 8;
                        item.useAnimation = 8;
                    }

                    if (GetItem(thorium, "TerraKnife", item))
                    {
                        item.damage = 40;
                    }

                    if (GetItem(thorium, "FireAxe", item))
                    {
                        item.damage = 350;
                    }

                    if (GetItem(thorium, "ShadeKusarigama", item))
                    {
                        item.shootSpeed = 22;
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.damage = 275;
                    }

                    if (GetItem(thorium, "PharaohsSlab", item))
                    {
                        item.damage = 160;
                    }

                    if (GetItem(thorium, "TheBlackDagger", item))
                    {
                        item.damage = 55;
                        item.useTime = 6;
                        item.useAnimation = 6;
                    }

                    if (GetItem(thorium, "SoftServeSunderer", item))
                    {
                        item.damage = 120;
                    }

                    if (GetItem(thorium, "Witchblade", item))
                    {
                        item.useTime = 12;
                        item.useAnimation = 12;
                        item.damage = 180;
                    }

                    if (GetItem(thorium, "CosmicDagger", item))
                    {
                        item.damage = 224;
                    }

                    if (GetItem(thorium, "ElectroRebounder", item))
                    {
                        item.damage = 177;
                        item.shootSpeed = 20;
                    }

                    if (GetItem(thorium, "Brinefang", item))
                    {
                        item.damage = 150;
                    }

                    if (GetItem(thorium, "DragonFang", item))
                    {
                        item.damage = 125;
                    }

                    if (GetItem(thorium, "TerrariumRippleKnife", item))
                    {
                        item.damage = 66;
                    }

                    if (GetItem(thorium, "StarEater", item))
                    {
                        item.damage = 80;
                    }

                    if (GetItem(thorium, "AngelsEnd", item))
                    {
                        item.damage = 525;
                    }

                    if (GetItem(thorium, "DeitysTrefork", item))
                    {
                        item.damage = 500;
                    }

                    if (GetItem(thorium, "TidalWave", item))
                    {
                        item.damage = 160;
                    }
                    #endregion
                    #endregion

                    #region Healer
                    #region Pre-Hardmode
                    //Wooden Baton
                    if (item.type == thorium.Find<ModItem>("WoodenBaton").Type)
                    {
                        item.damage = 7;
                    }

                    //Pill
                    if (item.type == thorium.Find<ModItem>("Pill").Type)
                    {
                        item.damage = 11;
                        item.shootSpeed = 9;
                    }

                    //Bonesaw 
                    if (item.type == thorium.Find<ModItem>("Bonesaw").Type)
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                    }

                    //Life Quartz Claymore
                    if (item.type == thorium.Find<ModItem>("LifeQuartzClaymore").Type)
                    {
                        item.damage = 17;
                        item.useAnimation = 26;
                        item.shootSpeed = 26;
                        item.healLife = 1;

                        item.scale *= 1.5f;
                    }

                    //Palm Cross
                    if (item.type == thorium.Find<ModItem>("PalmCross").Type)
                    {
                        item.damage = 15;
                    }

                    //Ice Shaver
                    if (item.type == thorium.Find<ModItem>("IceShaver").Type)
                    {
                        item.damage = 10;
                    }

                    //Poison Prickler
                    if (item.type == thorium.Find<ModItem>("PoisonPrickler").Type)
                    {
                        item.shootSpeed = 10;
                        item.useTime = 10;
                        item.useAnimation = 10;
                        item.damage = 15;
                    }

                    //The Digester
                    if (item.type == thorium.Find<ModItem>("TheDigester").Type)
                    {
                        item.mana = 10;
                    }

                    //Rotten Cod
                    if (item.type == thorium.Find<ModItem>("RottenCod").Type)
                    {
                        item.damage = 36;
                    }

                    //Deep Staff
                    if (item.type == thorium.Find<ModItem>("DeepStaff").Type)
                    {
                        item.damage = 25;
                    }

                    //Heretic Breaker
                    if (item.type == thorium.Find<ModItem>("HereticBreaker").Type)
                    {
                        item.useTime = 16;
                        item.useAnimation = 16;
                        item.healLife = 1;
                        item.damage = 20;

                        item.scale *= 1.25f;
                    }

                    //Renew
                    if (item.type == thorium.Find<ModItem>("Renew").Type)
                    {
                        //item.useTime = 92;
                        item.autoReuse = true;
                        //item.useAnimation = 92;
                    }

                    //Leech Bolt
                    if (item.type == thorium.Find<ModItem>("LeechBolt").Type)
                    {
                        item.damage = 18;
                    }

                    //Cleric's Cross
                    if (item.type == thorium.Find<ModItem>("ClericsCross").Type)
                    {
                        item.damage = 19;
                    }

                    //Purified Water
                    if (item.type == thorium.Find<ModItem>("PurifiedWater").Type)
                    {
                        item.damage = 22;
                    }

                    //Shadow Wand
                    if (item.type == thorium.Find<ModItem>("ShadowWand").Type)
                    {
                        item.damage = 30;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    //Aquaite Scythe
                    if (item.type == thorium.Find<ModItem>("AquaiteScythe").Type)
                    {
                        item.useTime = 23;
                        item.useAnimation = 23;
                        item.damage = 9;
                    }

                    //Samsara Lotus
                    if (item.type == thorium.Find<ModItem>("SamsaraLotus").Type)
                    {
                        item.damage = 28;
                    }

                    //The Stalker
                    if (item.type == thorium.Find<ModItem>("TheStalker").Type)
                    {
                        item.damage = 53;
                    }

                    //Molten Thresher
                    if (item.type == thorium.Find<ModItem>("MoltenThresher").Type)
                    {
                        item.damage = 20;
                    }

                    //Omen
                    if (item.type == thorium.Find<ModItem>("Omen").Type)
                    {
                        item.damage = 30;
                    }

                    //Bat Scythe
                    if (item.type == thorium.Find<ModItem>("BatScythe").Type)
                    {
                        item.damage = 20;
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }

                    //Life Disperser
                    if (item.type == thorium.Find<ModItem>("LifeDisperser").Type)
                    {
                        item.mana = 5;
                    }

                    //Bone Reaper
                    if (item.type == thorium.Find<ModItem>("BoneReaper").Type)
                    {
                        item.crit = 16;
                        item.damage = 11;
                    }

                    //Spirit Blast Wand
                    if (item.type == thorium.Find<ModItem>("SpiritBlastWand").Type)
                    {
                        item.damage = 28;
                    }

                    //Templar's Judgement
                    if (item.type == thorium.Find<ModItem>("TemplarJudgment").Type)
                    {
                        item.damage = 80;
                    }

                    //Light's Lament
                    if (item.type == thorium.Find<ModItem>("LightsLament").Type)
                    {
                        item.mana = 20;
                        item.useTime = 15;
                        item.useAnimation = 15;
                        item.damage = 40;
                        item.shootSpeed = 20;
                    }

                    //Dark Contagion
                    if (item.type == thorium.Find<ModItem>("DarkContagion").Type)
                    {
                        item.shootSpeed = 40;
                        item.useTime = 28;
                        item.useAnimation = 28;
                        item.damage = 60;
                    }

                    //Falling Twilight
                    if (item.type == thorium.Find<ModItem>("FallingTwilight").Type)
                    {
                        item.damage = 30;
                    }

                    //Blood Harvest
                    if (item.type == thorium.Find<ModItem>("BloodHarvest").Type)
                    {
                        item.damage = 27;
                    }
                    #endregion

                    #region Hardmode
                    //Bone Baton
                    if (GetItem(thorium, "BoneBaton", item))
                    {
                        item.damage = 45;
                    }

                    //Sacred Bludgeon
                    if (GetItem(thorium, "SacredBludgeon", item))
                    {
                        item.shootSpeed = 22;
                        item.damage = 95;
                    }

                    //Tranquil Lyre
                    if (GetItem(thorium, "TranquilLyre", item))
                    {
                        item.shootSpeed = 16;
                        item.damage = 55;
                    }

                    //Blood Transfusion
                    if (GetItem(thorium, "BloodTransfusion", item))
                    {
                        item.shootSpeed = 14;
                    }

                    //Wild Umbra
                    if (GetItem(thorium, "WildUmbra", item))
                    {
                        item.damage = 36;
                    }

                    //Iridescent Staff
                    if (GetItem(thorium, "IridescentStaff", item))
                    {
                        item.useTime = 19;
                        item.useAnimation = 19;
                    }

                    //Twilight Staff
                    if (GetItem(thorium, "TwilightStaff", item))
                    {
                        item.damage = 40;
                    }

                    //Hallowed Scythe
                    if (GetItem(thorium, "HallowedScythe", item))
                    {
                        item.damage = 44;
                    }

                    //True Hallowed Scythe
                    if (GetItem(thorium, "TrueHallowedScythe", item))
                    {
                        item.damage = 54;
                    }

                    if (GetItem(thorium, "TitanScythe", item))
                    {
                        item.crit = 16;
                    }

                    if (GetItem(thorium, "MorningDew", item))
                    {
                        item.damage = 60;
                    }

                    if (GetItem(thorium, "TrueFallingTwilight", item))
                    {
                        item.damage = 51;
                    }

                    if (GetItem(thorium, "TrueBloodHarvest", item))
                    {
                        item.damage = 33;
                    }

                    if (GetItem(thorium, "TheEffuser", item))
                    {
                        item.damage = 152;
                        item.shootSpeed = 22;
                    }

                    if (GetItem(thorium, "HallowedBlessing", item))
                    {
                        item.damage = 90;
                    }

                    if (GetItem(thorium, "SmitingHammer", item))
                    {
                        item.damage = 139;
                    }
                    
                    if (GetItem(thorium, "MindMelter", item))
                    {
                        item.damage = 125;
                        item.useTime = 10;
                        item.useAnimation = 10;
                    }

                    if (GetItem(thorium, "DreadTearer", item))
                    {
                        item.damage = 92;
                    }

                    if (GetItem(thorium, "IllumiteScythe", item))
                    {
                        item.damage = 71;
                    }

                    if (GetItem(thorium, "LethalInjection", item))
                    {
                        item.damage = 78;
                    }

                    if (GetItem(thorium, "LightBringerWarhammer", item))
                    {
                        item.useTime = 26;
                        item.useAnimation = 26;
                        item.damage = 104;
                    }

                    if (GetItem(thorium, "HolyHammer", item))
                    {
                        item.damage = 144;
                    }

                    if (GetItem(thorium, "PillPopper", item))
                    {
                        item.damage = 104;
                    }

                    if (GetItem(thorium, "TheBlackScythe", item))
                    {
                        item.damage = 90;
                    }

                    if (GetItem(thorium, "TerraScythe", item))
                    {
                        item.damage = 63;
                    }

                    if (GetItem(thorium, "ChristmasCheer", item))
                    {
                        item.damage = 240;
                    }

                    if (GetItem(thorium, "Kinetoscythe", item))
                    {
                        item.damage = 160;
                    }

                    if (GetItem(thorium, "PaganGrasp", item))
                    {
                        item.damage = 276;
                    }

                    if (GetItem(thorium, "CosmicFluxStaff", item))
                    {
                        item.damage = 140;
                    }

                    if (GetItem(thorium, "TerrariumHolyScythe", item))
                    {
                        item.damage = 25;
                    }

                    //Lucidty
                    if (GetItem(thorium, "Lucidity", item))
                    {
                        item.damage = 135;
                    }

                    //Reality Slasher
                    if (GetItem(thorium, "RealitySlasher", item))
                    {
                        item.damage = 75;
                    }
                    #endregion
                    #endregion

                    #region Bard
                    #region Pre-Hardmode
                    //Wooden Whistle
                    if (item.type == thorium.Find<ModItem>("WoodenWhistle").Type)
                    {
                        item.damage = 9;
                    }

                    //Drum Mallet
                    if (item.type == thorium.Find<ModItem>("DrumMallet").Type)
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                    }

                    //Ukulele
                    if (item.type == thorium.Find<ModItem>("Ukulele").Type)
                    {
                        item.damage = 12;
                    }

                    //Tambourine
                    if (item.type == thorium.Find<ModItem>("Tambourine").Type)
                    {
                        item.damage = 19;
                        item.useAnimation = 10;
                        item.useTime = 10;
                        item.shootSpeed = 12;
                    }

                    //Antlion Maraca
                    if (item.type == thorium.Find<ModItem>("AntlionMaraca").Type)
                    {
                        item.shootSpeed = 16;
                    }

                    //Lightning Claves
                    if (item.type == thorium.Find<ModItem>("LightningClaves").Type)
                    {
                        item.shootSpeed = 8;
                    }

                    //Gold Bugle Horn
                    if (item.type == thorium.Find<ModItem>("GoldBugleHorn").Type)
                    {
                        item.damage = 15;
                        item.useTime = 30;
                        item.useAnimation = 30;
                    }

                    //Ebonwood Tambourine
                    if (item.type == thorium.Find<ModItem>("EbonWoodTambourine").Type)
                    {
                        item.damage = 18;
                        item.useTime = 6;
                        item.useAnimation = 6;
                    }

                    //Shadewood Tambourine
                    if (item.type == thorium.Find<ModItem>("ShadeWoodTambourine").Type)
                    {
                        item.damage = 26;
                        item.useTime = 22;
                        item.useAnimation = 22;
                    }

                    //Vicious Mockery
                    if (item.type == thorium.Find<ModItem>("ViciousMockery").Type)
                    {
                        item.damage = 19;
                        item.shootSpeed = 12;
                        item.useTime = 26;
                        item.useAnimation = 26;
                    }

                    //Didgeridoo
                    if (item.type == thorium.Find<ModItem>("Didgeridoo").Type)
                    {
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }

                    //Sinister Honk
                    if (item.type == thorium.Find<ModItem>("SinisterHonk").Type)
                    {
                        item.shootSpeed = 13;
                        item.damage = 25;
                    }

                    //Yew Wood Lute
                    if (item.type == thorium.Find<ModItem>("YewWoodLute").Type)
                    {
                        item.damage = 26;
                    }

                    //Dynasty Guzheng
                    if (item.type == thorium.Find<ModItem>("DynastyGuzheng").Type)
                    {
                        item.damage = 28;
                    }

                    //Marine Wine Glasss
                    if (item.type == thorium.Find<ModItem>("MarineWineGlass").Type)
                    {
                        item.damage = 30;
                        item.useTime = 10;
                        item.useAnimation = 10;
                    }

                    //Bongos
                    if (item.type == thorium.Find<ModItem>("Bongos").Type)
                    {
                        item.damage = 38;
                    }

                    //Jar O' Mayo
                    if (item.type == thorium.Find<ModItem>("JarOMayo").Type)
                    {
                        item.damage = 30;
                    }

                    if (UnsafeGetItem(thorium, "MeteoriteOboe", item))
                    {
                        TrySetInspirationCost(item, 2);
                    }

                    //Sonar Cannon
                    if (item.type == thorium.Find<ModItem>("SonarCannon").Type)
                    {
                        item.damage = 25;
                    }

                    //Bone Trumpet
                    if (item.type == thorium.Find<ModItem>("BoneTrumpet").Type)
                    {
                        item.damage = 50;
                    }

                    //Calaveras
                    if (item.type == thorium.Find<ModItem>("Calaveras").Type)
                    {
                        item.damage = 37;
                    }

                    //Microphone
                    if (item.type == thorium.Find<ModItem>("Microphone").Type)
                    {
                        item.damage = 34;
                    }

                    //Nocturne
                    if (item.type == thorium.Find<ModItem>("Nocturne").Type)
                    {
                        item.damage = 72;
                    }

                    //Roboboe
                    if (item.type == thorium.Find<ModItem>("Roboboe").Type)
                    {
                        item.damage = 45;
                    }
                    #endregion

                    #region Hardmode
                    //Dragon's Wail
                    if (GetItem(thorium, "DragonsWail", item))
                    {
                        item.shootSpeed = 14;
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "FleshHorn", item))
                    {
                        item.shootSpeed = 20;
                        item.damage = 110;
                    }

                    if (GetItem(thorium, "Trombone", item))
                    {
                        item.damage = 44;
                    }

                    if (GetItem(thorium, "CobaltGong", item))
                    {
                        item.damage = 140;
                    }

                    if (GetItem(thorium, "ResonatorsArm", item))
                    {
                        item.shootSpeed = 12;
                    }

                    if (GetItem(thorium, "Xylophone", item))
                    {
                        item.damage = 94;
                    }

                    if (GetItem(thorium, "AcousticGuitar", item))
                    {
                        item.damage = 60;
                    }

                    if (GetItem(thorium, "ScholarsHarp", item))
                    {
                        item.damage = 36;
                    }

                    if (GetItem(thorium, "TheLullaby", item))
                    {
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "RiffWeaver", item))
                    {
                        item.damage = 70;
                    }

                    if (GetItem(thorium, "GeodeSaxophone", item))
                    {
                        item.damage = 50;
                    }

                    if (GetItem(thorium, "WindChimes", item))
                    {
                        item.shootSpeed = 15;
                        item.damage = 54;
                    }

                    if (GetItem(thorium, "GreedfulGurdy", item))
                    {
                        item.damage = 80;
                    }

                    if (GetItem(thorium, "Cello", item))
                    {
                        item.damage = 85;
                        item.useTime = 18;
                        item.useAnimation = 18;
                    }

                    if (GetItem(thorium, "LodestoneRadio", item))
                    {
                        item.damage = 17;
                    }

                    if (GetItem(thorium, "Kazoo", item))
                    {
                        item.damage = 44;
                    }

                    if (GetItem(thorium, "MythrilMelodica", item))
                    {
                        item.damage = 82;
                    }

                    if (GetItem(thorium, "OrichalcumSlideWhistle", item))
                    {
                        item.damage = 55;
                    }

                    if (GetItem(thorium, "GuiltyPleasure", item))
                    {
                        item.damage = 42;
                    }

                    if (GetItem(thorium, "SteamFlute", item))
                    {
                        item.damage = 64;
                    }

                    if (GetItem(thorium, "BaritoneSaxophone", item))
                    {
                        item.damage = 64;
                    }

                    if (GetItem(thorium, "AdamantiteKlaxon", item))
                    {
                        item.damage = 164;
                        TrySetInspirationCost(item, 1);
                    }

                    if (GetItem(thorium, "TitaniumCimbasso", item))
                    {
                        item.damage = 54;
                    }

                    if (GetItem(thorium, "Zunpet", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                        item.damage = 50;
                    }

                    if (GetItem(thorium, "PrimeRoar", item))
                    {
                        item.damage = 200;
                        item.useTime = 34;
                        item.useAnimation = 34;
                    }

                    if (GetItem(thorium, "PortableWintergatan", item))
                    {
                        item.damage = 97;
                    }

                    if (GetItem(thorium, "TheGreenTambourine", item))
                    {
                        item.damage = 108;
                        item.shootSpeed = 18;
                    }

                    if (GetItem(thorium, "HallowedMegaphone", item))
                    {
                        TrySetInspirationCost(item, 1);
                    }

                    if (GetItem(thorium, "MidnightBassBooster", item))
                    {
                        item.damage = 65;
                        item.useTime = 6;
                        item.useAnimation = 6;
                    }

                    if (GetItem(thorium, "CadaverCornet", item))
                    {
                        item.damage = 102;
                        TrySetInspirationCost(item, 1);
                    }

                    if (GetItem(thorium, "VuvuzelaBlue", item) || GetItem(thorium, "VuvuzelaYellow", item) || GetItem(thorium, "VuvuzelaRed", item) || GetItem(thorium, "VuvuzelaGreen", item))
                    {
                        item.damage = 100;
                    }

                    if (GetItem(thorium, "TheMaw", item))
                    {
                        item.damage = 85;
                        item.shootSpeed = 30;
                    }

                    if (GetItem(thorium, "Buisine", item))
                    {
                        TrySetInspirationCost(item, 1);
                        item.damage = 100;
                    }

                    if (GetItem(thorium, "Clarinet", item))
                    {
                        item.damage = 172;
                    }

                    if (GetItem(thorium, "Bassoon", item))
                    {
                        TrySetInspirationCost(item, 1);
                        item.damage = 188;
                    }

                    if (GetItem(thorium, "TheBopper", item))
                    {
                        item.shootSpeed = 17.5f;
                    }

                    if (GetItem(thorium, "IdolsMicrophone", item))
                    {
                        item.damage = 155;
                    }

                    if (GetItem(thorium, "GhastlyFrenchHorn", item))
                    {
                        item.damage = 124;
                    }

                    if (GetItem(thorium, "StrawberryHeart", item))
                    {
                        item.damage = 158;
                    }

                    if (GetItem(thorium, "Fishbone", item))
                    {
                        item.damage = 177;
                    }

                    if (GetItem(thorium, "Pungi", item))
                    {
                        item.damage = 85;
                        item.useTime = 12;
                        item.useAnimation = 12;
                    }

                    if (GetItem(thorium, "HauntingBassDrum", item))
                    {
                        item.damage = 404;
                    }

                    if (GetItem(thorium, "JingleBells", item))
                    {
                        item.damage = 165;
                        item.shootSpeed = 15;
                    }

                    if (GetItem(thorium, "Turntable", item))
                    {
                        item.damage = 190;
                    }

                    if (GetItem(thorium, "SirensLyre", item))
                    {
                        item.damage = 154;
                        item.crit = 21;
                    }
 
                    if (GetItem(thorium, "BetsysBellow", item))
                    {
                        item.damage = 94;
                    }

                    if (GetItem(thorium, "TerrariumAutoharp", item))
                    {
                        item.damage = 62;
                    }

                    if (GetItem(thorium, "SonicAmplifier", item))
                    {
                        item.useTime = 3;
                    }

                    //Edge of Imagination
                    if (GetItem(thorium, "EdgeofImagination", item))
                    {
                        item.damage = 400;
                    }

                    //Holophonor
                    if (GetItem(thorium, "Holophonor", item))
                    {
                        item.damage = 340;
                    }

                    //The Set
                    if (GetItem(thorium, "TheSet", item))
                    {
                        item.useTime = 34;
                        item.useAnimation = 34;
                        item.damage = 1000;
                    }

                    //Sousaphone
                    if (GetItem(thorium, "Sousaphone", item))
                    {
                        item.damage = 950;
                    }

                    if (GetItem(thorium, "BlackMIDI", item))
                    {
                        item.useTime = 14;
                        item.useAnimation = 14;
                        item.damage = 260;
                    }
                    #endregion
                    //Provided by Wardrobe Hummus
                    #region Insipration Changes
                    if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod")
                    {
                        string itemName = item.ModItem.Name;

                        // Change this to the internal name of the item you want to modify
                        if (itemName == "Nocturne")
                        {
                            TrySetInspirationCost(item, 1);
                        }
                        if (itemName == "SonarCannon")
                        {
                            TrySetInspirationCost(item, 1);
                        }
                        if (itemName == "CobaltGong")
                        {
                            TrySetInspirationCost(item, 2);
                        }
                        if (itemName == "HellBell")
                        {
                            TrySetInspirationCost(item, 1);
                        }
                    }
                    #endregion
                    #endregion

                    #region True
                    if (GetItem(thorium, "Mjolnir", item))
                    {
                        item.damage = 600;
                    }
                    #endregion

                    #region Armor
                    if (GetItem(thorium, "DurasteelChestplate", item))
                    {
                        item.defense = 12;
                    }

                    if (GetItem(thorium, "DurasteelGreaves", item))
                    {
                        item.defense = 11;
                    }

                    if (GetItem(thorium, "DurasteelHelmet", item))
                    {
                        item.defense = 12;
                    }

                    if (GetItem(thorium, "SteelChestplate", item))
                    {
                        item.defense = 7;
                    }

                    if (GetItem(thorium, "SteelGreaves", item))
                    {
                        item.defense = 7;
                    }

                    if (GetItem(thorium, "SteelHelmet", item))
                    {
                        item.defense = 6;
                    }
                    #endregion

                    #region Accessories
                    //Champion's Wings
                    if (item.type == thorium.Find<ModItem>("ChampionWing").Type)
                    {
                        item.defense = 7;
                    }

                    //Dragon Wings
                    if (GetItem(thorium, "DragonWings", item))
                    {
                        item.defense = 10;
                    }

                    //Subspace Wings
                    if (GetItem(thorium, "SubspaceWings", item))
                    {
                        item.defense = 10;
                        item.lifeRegen = 2;
                    }

                    //Terrarium Wings
                    if (GetItem(thorium, "TerrariumWings", item))
                    {
                        item.defense = 10;
                        item.lifeRegen = 4;
                    }

                    //Shinobi Sigil
                    if (UnsafeGetItem(thorium, "ShinobiSigil", item))
                    {
                        TrySetAccessoryDamage(item, "25% basic damage");
                    }
                    #endregion
                }
            }
            #endregion

            #region Unofficial Calamity Bard & Healler
            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && (ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess || ModContent.GetInstance<InfernalConfig>().CalamityBalanceChanges))
            {
                #region Healer
                #region Pre-Hardmode
                //Wulfrum Weed Wacker
                if (item.type == calBardHeal.Find<ModItem>("WulfrumWeedWacker").Type)
                {
                    item.useTime = 36;
                    item.useAnimation = 36;
                }

                //Dry Mouth
                if (item.type == calBardHeal.Find<ModItem>("DryMouth").Type)
                {
                    item.useTime = 40;
                    item.useAnimation = 40;
                    item.damage = 18;
                }

                //The Windmill
                if (item.type == calBardHeal.Find<ModItem>("TheWindmill").Type)
                {
                    item.damage = 5;
                }

                //Duality
                if (item.type == calBardHeal.Find<ModItem>("Duality").Type)
                {
                    item.damage = 45;
                }

                //Gelatin Therapy
                if (item.type == calBardHeal.Find<ModItem>("GelatinTherapy").Type)
                {
                    item.useTime = 256;
                    item.useAnimation = 256;
                }

                //Birthplace of Stars
                if (calBardHeal.TryFind("StarBirth", out ModItem starBirth))
                {
                    if (item.type == starBirth.Type && hasCatalyst)
                    {
                        TrySetHealAmmount(item, 2);
                        item.autoReuse = true;
                    }
                }
                #endregion

                #region Hardmode
                if (GetItem(calBardHeal, "CottonMouth", item))
                {
                    item.damage = 20;
                }

                if (GetItem(calBardHeal, "ElectricQuarterstaff", item))
                {
                    item.damage = 32;
                }

                //Fire Hazard
                if (item.type == calBardHeal.Find<ModItem>("FireHazard").Type)
                {
                    item.damage = 30;
                    //projectile?
                }

                if (GetItem(calBardHeal, "TidalForce", item))
                {
                    item.damage = 231;
                }

                if (GetItem(calBardHeal, "SARS", item))
                {
                    item.damage = 100;
                    item.crit = 0;
                }

                if (GetItem(calBardHeal, "OmnicidesLaw", item))
                {
                    item.damage = 90;
                }

                //Syzygy
                if (GetItem(calBardHeal, "Syzygy", item))
                {
                    item.damage = 80;
                }

                //Singularity
                if (hasCatalyst)
                {
                    if (GetItem(calBardHeal, "Singularity", item))
                    {
                        item.damage = 110;
                    }
                }

                //Blooming Saintess' Devotion
                if (GetItem(calBardHeal, "BloomingSaintessDevotion", item))
                {
                    item.damage = 69;
                }

                //Death Adder
                if (GetItem(calBardHeal, "DeathAdder", item))
                {
                    item.damage = 200;
                }

                //Purgatorium Pandemonium
                if (GetItem(calBardHeal, "PurgatoriumPandemonium", item))
                {
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.damage = 175;
                }

                //Exorectionist
                if (GetItem(calBardHeal, "Exorectionist", item))
                {
                    item.Calamity().ChargePerUse = (float)0.5;
                }

                //Milky Way
                if (GetItem(calBardHeal, "MilkyWay", item))
                {
                    item.damage = 165;
                    item.crit = 0;
                }

                //Saving Grace
                if (GetItem(calBardHeal, "SavingGrace", item))
                {
                    item.useTime = 80;
                    item.useAnimation = 80;
                    item.autoReuse = true;
                }

                //Will of the Ragnarok
                if (GetItem(calBardHeal, "WilloftheRagnarok", item))
                {
                    item.damage = 77;
                }

                //Cherubim Omega
                if (GetItem(calBardHeal, "CherubimOmega", item))
                {
                    item.damage = 600;
                }

                //Disaster
                if (GetItem(calBardHeal, "Disaster", item))
                {
                    item.damage = 550;
                }

                //Times Old Roman
                if (hasCalHunt)
                {
                    if (GetItem(calBardHeal, "TimesOldRoman", item))
                    {
                        item.crit = 0;
                        item.damage = 400;
                        item.useTime = 24;
                        item.useAnimation = 24;
                    }
                }

                #endregion
                #endregion

                #region Bard
                #region Pre-Hardmode
                //Wullfrum Megaphone
                if (UnsafeGetItem(calBardHeal, "WulfrumMegaphone", item))
                {
                    item.damage = 13;
                }

                //Crystal Hydraulophone
                if (UnsafeGetItem(calBardHeal, "CrystalHydraulophone", item))
                {
                    item.damage = 30;
                }

                //Violince
                if (GetItem(calBardHeal, "Violince", item))
                {
                    item.damage = 9;
                }

                if (UnsafeGetItem(calBardHeal, "FilthyFlute", item))
                {
                    item.damage = 32;
                }

                if (UnsafeGetItem(calBardHeal, "ScrapGuitar", item))
                {
                    item.damage = 43;
                }

                //Return to Sludge
                if (item.type == calBardHeal.Find<ModItem>("ReturntoSludge").Type)
                {
                    item.damage = 33;
                    item.shootSpeed = 8;
                }
            #endregion

                #region Hardmode
                //Song of the Elements
                if (GetItem(calBardHeal, "ArcticReinforcement", item))
                {
                    item.damage = 49;
                }

                if (GetItem(calBardHeal, "SongoftheAncients", item))
                {
                    item.damage = 25;
                }

                if (GetItem(calBardHeal, "AnahitasArpeggio", item))
                {
                    item.damage = 144;
                }

                if (GetItem(calBardHeal, "TectonicPlates", item))
                {
                    item.damage = 80;
                }

                if (GetItem(calBardHeal, "SongoftheElements", item))
                {
                    item.damage = 77;
                }

                //Supercluster
                if (hasCatalyst)
                {
                    if (GetItem(calBardHeal, "Supercluster", item))
                    {
                        item.damage = 60;
                    }
                }

                if (GetItem(calBardHeal, "TreeWhisperersHarp", item))
                {
                    item.damage = 115;
                }

                if (GetItem(calBardHeal, "FeralKeytar", item))
                {
                    item.damage = 150;
                }

                if (GetItem(calBardHeal, "FaceMelter", item))
                {
                    item.damage = 65;
                }

                if (GetItem(calBardHeal, "ChristmasCarol", item))
                {
                    item.damage = 150;
                }

                if (GetItem(calBardHeal, "SpookyMonth", item))
                {
                    item.damage = 165;
                }

                if (GetItem(calBardHeal, "DoomsdayCatharsis", item))
                {
                    item.damage = 90;
                }

                if (GetItem(calBardHeal, "SongoftheCosmos", item))
                {
                    item.damage = 190;
                }

                if (GetItem(calBardHeal, "SymphonicFabrications", item))
                {
                    item.damage = 140;
                }

                if (GetItem(calBardHeal, "Gashadokuro", item))
                {
                    item.damage = 444;
                }

                if (hasCalHunt)
                {
                    if (GetItem(calBardHeal, "HarmonyoftheOldGod", item))
                    {
                        item.crit = 16;
                    }
                }
                #endregion
                #endregion

                #region Armor
                if (hasCatalyst)
                {
                    if (GetItem(calBardHeal, "IntergelacticCloche", item))
                    {
                        item.defense = 44;
                    }

                    if (GetItem(calBardHeal, "IntergelacticProtectorHelm", item))
                    {
                        item.defense = 48;
                    }
                }
                #endregion
            }
            #endregion

            #region Thorium Bosses Reworked
            if (ModLoader.TryGetMod("ThoriumRework", out Mod rethorium) && ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess)
            {
                #region Melee
                //Will of Coznix
                if (GetItem(rethorium, "BeholderBlade", item))
                {
                    item.crit = 0;
                }
                #endregion

                #region Ranged

                #endregion

                #region Summoner
                if (GetItem(rethorium, "LichWhip", item))
                {
                    item.damage = 21;
                }
                #endregion

                #region Thrower
                //Pocket Energy Storm
                if (GetItem(rethorium, "PocketEnergyStorm", item))
                {
                    //item.damage = 13;
                }
                #endregion

                #region Healer
                if (GetItem(rethorium, "RedCresent", item))
                {
                    item.useTime = 12;
                    item.useAnimation = 12;
                }
                #endregion

                #region Bard
                if (GetItem(rethorium, "JellyfishJam", item))
                {
                    item.damage = 36;
                }

                if (GetItem(rethorium, "StellarTune", item))
                {
                    item.damage = 140;
                }
                #endregion

                #region True
                if (GetItem(rethorium, "Oneirophobia", item))
                {
                    item.damage = 1000;
                }
                #endregion
            }
            #endregion

            #region Ragnarok
            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) && ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess)
            {
                #region Healer
                //Prisma
                if (item.type == ragnarok.Find<ModItem>("Prisma").Type)
                {
                    item.damage = 8;
                }

                //Equivalent Exchange
                if (item.type == ragnarok.Find<ModItem>("EquivalentExchange").Type)
                {
                    item.shootSpeed = 13;
                    item.damage = 42;
                }

                //Wind Reaver
                if (item.type == ragnarok.Find<ModItem>("WindReaver").Type)
                {
                    item.damage = 26;
                    item.useTime = 8;
                    item.useAnimation = 8;
                }

                //Marble Scythe
                if (item.type == ragnarok.Find<ModItem>("MarbleScythe").Type)
                {
                    item.damage = 21;
                    item.healLife = 3;
                }

                //Jelly Slicer
                if (item.type == ragnarok.Find<ModItem>("JellySlicer").Type)
                {
                    item.damage = 20;
                }

                //Scoria Dualscythe
                if (GetItem(ragnarok, "ScoriaDualscythe", item))
                {
                    item.damage = 65;
                }

                //Astral Ripper
                if (GetItem(ragnarok, "AstralRipper", item))
                {
                    item.useTime = 8;
                    item.useAnimation = 8;
                    item.damage = 200;
                }

                //Profaned Scythe
                if (GetItem(ragnarok, "ProfanedScythe", item))
                {
                    item.useTime = 12;
                    item.useAnimation = 12;
                    item.damage = 120;
                }

                //Verdurant Bloom
                if (GetItem(ragnarok, "VerdurantBloom", item))
                {
                    item.damage = 150;
                }

                //Splattercannon
                if (GetItem(ragnarok, "Splattercannon", item))
                {
                    item.useTime = 6;
                    item.useAnimation = 6;
                    item.damage = 200;
                }

                //Fractal
                if (GetItem(ragnarok, "Fractal", item))
                {
                    item.shootSpeed = 2;
                }

                //Nightmare Freezer
                if (GetItem(ragnarok, "NightmareFreezer", item))
                {
                    item.useTime = 12;
                    item.useAnimation = 12;
                    item.damage = 210;
                }

                //Cosmic Injector
                if (GetItem(ragnarok, "CosmicInjector", item))
                {
                    item.damage = 150;
                }

                #endregion

                #region Bard
                if (GetItem(ragnarok, "CalamityBell", item))
                {
                    item.shootSpeed = 20;
                    TrySetInspirationCost(item, 2);
                    item.useTime = 40;
                    item.useAnimation = 40;
                }

                if (GetItem(ragnarok, "DrumStick", item))
                {
                    item.useTime = 12;
                    item.useAnimation = 12;
                    item.damage = 220;
                }

                if (GetItem(ragnarok, "Steampipes", item))
                {
                    item.shootSpeed = 20;
                }

                //Drum Stick
                if (GetItem(ragnarok, "DrumStick", item))
                {
                    item.useTime = 12;
                    item.useAnimation = 12;
                    item.damage = 220;
                }

                //Profaned Bell
                if (GetItem(ragnarok, "ProfanedBell", item))
                {
                    item.damage = 600;
                    item.useTime = 21;
                    item.useAnimation = 21;
                    TrySetInspirationCost(item, 2);
                }

                //Elysian Song
                if (GetItem(ragnarok, "ElysianSong", item))
                {
                    item.damage = 340;
                    item.useAnimation = 12;
                    item.useTime = 12;
                }

                if (GetItem(ragnarok, "RadioMic", item))
                {
                    item.damage = 435;
                    TrySetInspirationCost(item, 2);
                    item.useTime = 24;
                    item.useAnimation = 24;
                }

                if (GetItem(ragnarok, "UnbreakableCombatUkulele", item))
                {
                    item.damage = 1400;
                }

                if (GetItem(ragnarok, "Arpeggiator", item))
                {
                    item.damage = 450;
                    item.useAnimation = 5;
                }

                if (GetItem(ragnarok, "Korobeiniki", item))
                {
                    item.damage = 250;
                }
                #endregion
            }
            #endregion

            #region Consolaria
            if (ModLoader.TryGetMod("Consolaria", out Mod console) && ModContent.GetInstance<InfernalConfig>().ConsolariaBalanceChanges)
            {
                if (UnsafeGetItem(console, "AlbinoMandible", item))
                {
                    item.damage = 20;
                }

                if (UnsafeGetItem(console, "EggCannon", item))
                {
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.damage = 24;
                }

                if (UnsafeGetItem(console, "GreatDrumstick", item))
                {
                    item.scale *= 1.5f;
                    item.useTime = 26;
                    item.useAnimation = 26;
                    item.damage = 62;
                }

                if (UnsafeGetItem(console, "FeatherStorm", item))
                {
                    item.damage = 35;
                }

                if (UnsafeGetItem(console, "DragonBreath", item))
                {
                    item.damage = 44;
                }

                if (UnsafeGetItem(console, "VolcanicRepeater", item))
                {
                    item.damage = 30;
                }

                if (UnsafeGetItem(console, "Tizona", item))
                {
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.damage = 93;
                }

                if (UnsafeGetItem(console, "Tonbogiri", item))
                {
                    item.useTime = 28;
                    item.useAnimation = 28;
                    item.damage = 112;
                }
            }
            #endregion
        }

        public static int FindItem(Mod mod, string name)
        {
            return mod.Find<ModItem>(name).Type;
        }

        public static bool GetItem(Mod mod, string name, Item item)
        {
            ModItem foundItem;
            ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHealer);
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            if (mod == calBardHealer || mod == thorium)
            {
                if (!mod.TryFind(name, out foundItem))
                    return false;
            }
            else
            {
                foundItem = mod.Find<ModItem>(name);
            }

            if (item.type == foundItem.Type)
            {
                return true;
            }
            return false;
        }

        //used for testing
        public static bool UnsafeGetItem(Mod mod, string name, Item item)
        {
            ModItem foundItem;
            foundItem = mod.Find<ModItem>(name);

            if (item.type == foundItem.Type)
            {
                return true;
            }
            return false;
        }

        //method by Wardrobe Hummus
        private void TrySetInspirationCost(Item item, int newCost)
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
                FieldInfo field = modItemType.GetField("InspirationCost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(item.ModItem, newCost);
                    Main.NewText($"[Field] Set InspirationCost of {item.Name} to {newCost}");
                    return;
                }

                // Then try property
                PropertyInfo prop = modItemType.GetProperty("InspirationCost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item.ModItem, newCost);
                    return;
                }

                Main.NewText("InspirationCost not found on ModItem.");
            }
            catch (Exception ex)
            {
                Main.NewText($"Error setting InspirationCost: {ex.Message}");
            }
        }
        //Refactored by Akira
        private void TrySetHealAmmount(Item item, int newCost)
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
                FieldInfo field = modItemType.GetField("healAmount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(item.ModItem, newCost);
                    //Main.NewText($"[Field] Set healAmount of {item.Name} to {newCost}");
                    return;
                }

                // Then try property
                PropertyInfo prop = modItemType.GetProperty("healAmount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item.ModItem, newCost);
                    return;
                }

                //Main.NewText("healAmount not found on ModItem.");
            }
            catch (Exception)
            {
                //Main.NewText($"Error setting healAmount: {ex.Message}");
            }
        }

        private void TrySetAccessoryDamage(Item item, string newDamage)
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
                FieldInfo field = modItemType.GetField("accDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(item.ModItem, newDamage);
                    //Main.NewText($"[Field] Set healAmount of {item.Name} to {newCost}");
                    return;
                }

                // Then try property
                PropertyInfo prop = modItemType.GetProperty("accDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item.ModItem, newDamage);
                    return;
                }

                //Main.NewText("healAmount not found on ModItem.");
            }
            catch (Exception)
            {
                //Main.NewText($"Error setting healAmount: {ex.Message}");
            }
        }
    }
}
