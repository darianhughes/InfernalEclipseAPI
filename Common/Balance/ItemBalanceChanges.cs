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
                    item.damage = 30000;
                    item.crit = 4;
                }

                if (item.type == toilet.Find<ModItem>("TrueZenithToilet").Type)
                {
                    item.damage = 100000;
                    item.crit = 0;
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

                //Thor's Hammer: Melee
                if (item.type == FindItem(thorium, "MeleeThorHammer")) 
                {
                    item.damage = 45;
                    item.useTime = 18;
                    item.useAnimation = 18;
                }

                //Champion's Swift Blade
                if (item.type == thorium.Find<ModItem>("ChampionSwiftBlade").Type)
                {
                    item.DamageType = DamageClass.Melee;
                    item.damage = 50;
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


                //Healer
                //Wooden Baton
                if (item.type == thorium.Find<ModItem>("WoodenBaton").Type) 
                {
                    item.damage = 7;
                }

                //The Digester
                if (item.type == thorium.Find<ModItem>("TheDigester").Type)
                {
                    item.mana = 10;
                }
                //Renew
                if (item.type == thorium.Find<ModItem>("Renew").Type)
                {
                    item.useTime = 92;
                    item.autoReuse = true;
                    item.useAnimation = 92;
                }
                //Life Disperser
                if (item.type == thorium.Find<ModItem>("LifeDisperser").Type)
                {
                    item.mana = 5;
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
                //The Windmill
                if (item.type == calBardHeal.Find<ModItem>("TheWindmill").Type)
                {
                    item.damage = 5;
                }

                //Fire Hazard
                if (item.type == calBardHeal.Find<ModItem>("FireHazard").Type)
                {
                    item.damage = 51;
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
        }

        public static int FindItem (Mod mod, string name)
        {
            return mod.Find<ModItem>(name).Type;
        }
    }
}
