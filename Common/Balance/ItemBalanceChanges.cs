using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content.Sources;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using InfernumMode;
using InfernumMode.Content.BehaviorOverrides.BossAIs.GreatSandShark;
using GreatSandSharkNPC = CalamityMod.NPCs.GreatSandShark.GreatSandShark;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using CalamityMod.Sounds;
using CalamityMod.NPCs.SupremeCalamitas;
using System.Xml.XPath;
using CalamityMod.Tiles.PlayerTurrets;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Thorium;
using CalamityMod.Items.Weapons.Summon;

namespace InfernalEclipseAPI.Common.Balance
{
    public class ItemBalanceChanges : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            //Vanilla
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
            }

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

            if (InfernalConfig.Instance.CalamityBalanceChanges)
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);

                //Summoner
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
            }

            //Thorium
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess)
            {
                //Melee
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

                //HARDMODE
                //Durasteel Blade
                if (item.type == FindItem(thorium, "DurasteelBlade"))
                {
                    item.useTime = 14;
                    item.useAnimation = 14;
                }

                //Ranged
                //Frost Fury
                if (item.type == thorium.Find<ModItem>("FrostFury").Type)
                {
                    item.useTime = 24;
                    item.useAnimation = 24;
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
                    item.damage = 34;
                }

                //Hit Scanner
                if (item.type == thorium.Find<ModItem>("HitScanner").Type)
                {
                    item.damage = 30;
                    item.useAnimation = 16;
                    item.useTime = 16;
                }

                //Magic
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

                //Magick Staff
                if (item.type == thorium.Find<ModItem>("MagickStaff").Type)
                {
                    //Add debufs
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
                    item.knockBack = 10;
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

                //Summoner
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

                //Healer
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
                }

                //Palm Cross
                if (item.type == thorium.Find<ModItem>("PalmCross").Type)
                {
                    item.damage = 11;
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
                    item.shootSpeed = 14;
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
                }

                //Renew
                if (item.type == thorium.Find<ModItem>("Renew").Type)
                {
                    item.useTime = 92;
                    item.autoReuse = true;
                    item.useAnimation = 92;
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
                    item.healMana = 10;
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
                    item.shootSpeed = 16;
                }

                //Molten Thresher
                if (item.type == thorium.Find<ModItem>("MoltenThresher").Type)
                {
                    item.healLife = 1;
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
                    item.useTime = 12;
                    item.useAnimation = 12;
                }

                //Life Disperser
                if (item.type == thorium.Find<ModItem>("LifeDisperser").Type)
                {
                    item.mana = 5;
                }

                //Bone Reaper
                if (item.type == thorium.Find<ModItem>("BoneReaper").Type)
                {
                    item.useTime = 7;
                    item.useAnimation = 7;
                    item.crit = 16;
                    item.healMana = 5;
                    item.damage = 21;
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
                    item.shootSpeed = 18;
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
                    item.healMana = 20;
                    item.healLife = 2;
                    item.damage = 55;
                }

                //Blood Harvest
                if (item.type == thorium.Find<ModItem>("BloodHarvest").Type)
                {
                    item.healLife = 4;
                    item.healMana = 10;
                    item.damage = 50;
                }

                //Bard
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
                    item.useTime = 26;
                    item.useAnimation = 26;
                }

                //Sinister Honk
                if (item.type == thorium.Find<ModItem>("SinisterHonk").Type)
                {
                    item.shootSpeed = 13;
                }

                //Yew Wood Lute
                if (item.type == thorium.Find<ModItem>("YewWoodLute").Type)
                {
                    item.damage = 22;
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

                //Sonar Cannon
                if (item.type == thorium.Find<ModItem>("SonarCannon").Type)
                {
                    item.damage = 25;
                }

                //Bone Trumpet
                if (item.type == thorium.Find<ModItem>("BoneTrumpet").Type)
                {
                    item.damage = 40;
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
            }
            
            //Unofficial Calamity Bard & Healler
            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && (ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess || ModContent.GetInstance<InfernalConfig>().CalamityBalanceChanges)) 
            {
                //Healer Adjustments
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

                //Fire Hazard
                if (item.type == calBardHeal.Find<ModItem>("FireHazard").Type)
                {
                    item.damage = 36;
                    //projectile?
                }

                //Bard Adjustments
                //Return to Sludge
                if (item.type == calBardHeal.Find<ModItem>("ReturntoSludge").Type)
                {
                    item.damage = 22;
                    item.shootSpeed = 8;
                }

                //Song of the Ancients
            }

            //Thorium Bosses Reworked
            if (ModLoader.TryGetMod("ThoriumRework", out Mod rethorium) && ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess)
            {
                //Thrower Item Adjustments
                //Pocket Energy Storm
                if (item.type == rethorium.Find<ModItem>("PocketEnergyStorm").Type)
                {
                    item.damage = 13;
                }
            }

            //Ragnarook
            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok) && ModContent.GetInstance<InfernalConfig>().ThoriumBalanceChangess)
            {
                //Healer
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
                    item.damage = 20;
                    item.useTime = 5;
                    item.useAnimation = 5;
                }

                //Marble Scythe
                if (item.type == ragnarok.Find<ModItem>("MarbleScythe").Type)
                {
                    item.damage = 28;
                    item.healLife = 3;
                }

                //Jelly Slicer
                if (item.type == ragnarok.Find<ModItem>("JellySlicer").Type)
                {
                    item.damage = 20;
                }
            }
        }

        public static int FindItem (Mod mod, string name)
        {
            return mod.Find<ModItem>(name).Type;
        }
    }
}
