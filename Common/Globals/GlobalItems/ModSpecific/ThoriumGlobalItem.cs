using System.Collections.Generic;
using System.Reflection;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using InfernalEclipseAPI.Core.Systems;
using InfernalEclipseAPI.Core.Systems.Hooks;
using InfernalEclipseAPI.Core.Utils;
using InfernumMode.Content.Items.Accessories;
using InfernumMode.Core.GlobalInstances.Players;
using Terraria;
using Terraria.Localization;
using ThoriumMod;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Slag;
using ThoriumMod.Items.Bronze;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.Coral;
using ThoriumMod.Items.Cultist;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Dread;
using ThoriumMod.Items.Flesh;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Icy;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.Sandstone;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Valadium;
using ThoriumMod.Utilities;
using static Terraria.ModLoader.ModContent;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    [JITWhenModsEnabled(InfernalCrossmod.Thorium.Name)]
    [ExtendsFromMod(InfernalCrossmod.Thorium.Name)]
    public class ThoriumGlobalItem : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            InfernumPlayer.AccessoryUpdateEvent += (InfernumPlayer player) =>
            {
                if (player.GetValue<bool>(Purity.FieldName))
                {
                    Player p = player.Player;
                    float bonus = 1.4f;

                    p.GetDamage<BardDamage>() *= bonus;
                    p.GetDamage<HealerDamage>() *= bonus;
                    p.GetDamage<HealerTool>() *= bonus;
                    p.GetDamage<HealerToolDamageHybrid>() *= bonus;
                    p.GetDamage<TrueDamage>() *= bonus;
                }
            };
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemType<IceLance>() ||
                item.type == ItemType<fSandStoneSpear>() ||
                item.type == ItemType<Fork>() ||
                item.type == ItemType<CoralPolearm>() ||
                item.type == ItemType<HarpyTalon>() ||
                item.type == ItemType<PearlPike>() ||
                item.type == ItemType<Moonlight>() ||
                item.type == ItemType<EnergyStormPartisan>() ||
                item.type == ItemType<FleshSkewer>() ||
                item.type == ItemType<HellishHalberd>() ||
                item.type == ItemType<ValadiumSpear>() ||
                item.type == ItemType<BloodGlory>()
    )
            {
                item.DamageType = GetInstance<TrueMeleeDamageClass>();
            }

            if (InfernalConfig.Instance.ChanageWeaponClasses) 
            {
                if (item.type == ItemID.TheAxe)
                {
                    item.DamageType = ThoriumDamageBase<BardDamage>.Instance;
                }

                if (item.type == ItemType<AncientFlame>())
                {
                    item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    item.damage = 32;
                }

                if (item.type == ItemType<TheBurningSky>())
                {
                    item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    item.mana = 35;
                }
            }

            if (InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (item.type == ItemType<ShadeMasterMask>())
                {
                    item.defense = 10;
                }
                if (item.type == ItemType<ShadeMasterTreads>())
                {
                    item.defense = 12;
                }
                if (item.type == ItemType<ShadeMasterGarb>())
                {
                    item.defense = 16;
                }

                if (item.type == ItemType<CoralHelmet>())
                {
                    item.defense = 4;
                }
                if (item.type == ItemType<CoralChestGuard>())
                {
                    item.defense = 5;
                }
                if (item.type == ItemType<CoralGreaves>())
                {
                    item.defense = 4;
                }

                if (item.type == ItemType<IridescentHelmet>())
                {
                    item.defense = 5;
                }
                if (item.type == ItemType<IridescentMail>())
                {
                    item.defense = 6;
                }

                if (item.type == ItemType<FallenPaladinFaceguard>())
                {
                    item.defense = 20;
                }
                if (item.type == ItemType<FallenPaladinCuirass>())
                {
                    item.defense = 28;
                }
                if (item.type == ItemType<FallenPaladinGreaves>())
                {
                    item.defense = 22;
                }

                if (item.type == ItemType<WhisperingHood>())
                {
                    item.defense = 10;
                }
                if (item.type == ItemType<WhisperingTabard>())
                {
                    item.defense = 20;
                }
                if (item.type == ItemType<WhisperingLeggings>())
                {
                    item.defense = 14;
                }

                if (item.type == ItemID.BreakerBlade)
                {
                    SheathCompatibilitySystem.SetIncompatible(item.type);
                }
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EyeoftheGolem)
                player.Calamity().critDamage -= 0.15f;

            if (item.type == ItemType<MaskoftheCrystalEye>())
            {
                player.GetThoriumPlayer().bonusCritDamage -= 0.12f;
                if (!InfernalCrossmod.SOTS.Loaded)
                    player.Calamity().critDamage += 0.15f;
            }

            if (item.type == ItemType<TravelersBoots>())
            {
                player.runAcceleration -= 0.06f;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (InfernalConfig.Instance.ThoriumBalanceChangess && !InfernalCrossmod.Hummus.Loaded)
            {
                if (item.type == ItemType<CrystalHoney>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.05f;
                }

                if (item.type == ItemType<DemonTongue>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.02f;
                }

                if (item.type == ItemType<DarkGlaze>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.05f;
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 5f;
                }

                if (item.type == ItemType<ArchDemonCurse>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.08f;
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 7f;
                }

                if (item.type == ItemType<MagmaSeersMask>() && InfernalCrossmod.ThoriumRework.Loaded)
                {
                    player.GetDamage(DamageClass.Summon).Flat -= 30f;
                    player.maxMinions -= 4;
                    player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) -= 0.2f;
                }

                if (item.type == ItemType<PyromancerTabard>() && InfernalCrossmod.ThoriumRework.Loaded)
                {
                    player.whipRangeMultiplier -= 0.50f;
                    player.maxMinions -= 1;
                }

                if (item.type == ItemType<FlightMask>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.03f;
                    player.GetCritChance(DamageClass.Throwing) -= 1f;
                }

                if (item.type == ItemType<FlightMail>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.07f;
                }

                if (item.type == ItemType<BronzeHelmet>())
                {
                    player.GetCritChance(DamageClass.Throwing) -= 4f;
                }
                if (item.type == ItemType<BronzeBreastplate>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.1f;
                }

                if (item.type == ItemType<PlagueDoctorsMask>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.07f;
                }
                if (item.type == ItemType<PlagueDoctorsGarb>())
                {
                    player.GetDamage(DamageClass.Throwing) += 0.01f;
                }
                if (item.type == ItemType<PlagueDoctorsLeggings>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.04f;
                }

                if (item.type == ItemType<FungusHat>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.03f;
                }
                if (item.type == ItemType<FungusGuard>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.02f;
                }

                if (item.type == ItemType<ShadeMasterMask>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.05f;
                }
                if (item.type == ItemType<ShadeMasterTreads>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.025f;
                }

                if (item.type == ItemType<WhiteDwarfMask>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.1f;
                }
                if (item.type == ItemType<WhiteDwarfGreaves>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.05f;
                }

                if (item.type == ItemType<TideTurnerGreaves>() && InfernalCrossmod.ThoriumRework.Loaded)
                {
                    player.GetAttackSpeed(DamageClass.Melee) -= 0.2f;
                    player.GetAttackSpeed(DamageClass.Throwing) -= 0.2f;
                }

                if (item.type == ItemType<EbonHood>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.1f;
                }

                if (item.type == ItemType<EbonCloak>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.1f;
                }

                if (item.type == ItemType<EbonLeggings>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.07f;
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance).Flat += 1f;
                }

                if (item.type == ItemType<CoralHelmet>())
                {
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 2f;
                }
                if (item.type == ItemType<CoralChestGuard>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.03f;
                }
                if (item.type == ItemType<CoralGreaves>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.02f;
                }

                if (item.type == ItemType<IridescentHelmet>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.11f;
                }
                if (item.type == ItemType<IridescentMail>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.02f;
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 6f;
                }
                if (item.type == ItemType<IridescentGreaves>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.05f;
                }

                if (item.type == ItemType<BloomingTabard>())
                {
                    InfernalCrossmod.Thorium.Mod.Call(new object[]
                    {
                        "BonusHealerHealBonus",
                        player,
                        +1
                    });
                }

                if (item.type == ItemType<WarlockHood>())
                {
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 5f;
                }
                if (item.type == ItemType<WarlockGarb>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.2f;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                }
                if (item.type == ItemType<WarlockLeggings>())
                {
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 5f;
                }

                if (item.type == ItemType<BioTechGarment>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.1f;
                }

                if (item.type == ItemType<FallenPaladinFaceguard>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.2f;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                }
                if (item.type == ItemType<FallenPaladinCuirass>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.2f;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                }
                if (item.type == ItemType<FallenPaladinGreaves>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.2f;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                }

                if (item.type == ItemType<WhisperingHood>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.1f;
                    player.GetDamage(DamageClass.Generic) += 0.05f;
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 2f;
                }
                if (item.type == ItemType<WhisperingTabard>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.2f;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 2f;
                }
                if (item.type == ItemType<WhisperingLeggings>())
                {
                    player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.2f;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 6f;
                }

                if (item.type == ItemType<DreadGreaves>())
                {
                    player.moveSpeed -= 0.11f;
                }

                if (InfernalCrossmod.RagnarokMod.Loaded)
                {
                    if (item.type == ItemType<NinjaEmblem>())
                    {
                        player.GetDamage(DamageClass.Generic) -= 0.03f;
                        player.GetCritChance(DamageClass.Generic) -= 3f;
                    }
                }

                if (InfernalCrossmod.CalBardHealer.Loaded)
                {
                    Mod cBH = InfernalCrossmod.CalBardHealer.Mod;
                    int FindItem(string name) => cBH.Find<ModItem>(name).Type;

                    if (item.type == FindItem("TarragonParagonCrown"))
                    {
                        InfernalCrossmod.Thorium.Mod.Call(new object[]
                        {
                        "BonusHealerHealBonus",
                        player,
                        -2
                        });
                    }

                    if (item.type == FindItem("BloodflareRitualistMask"))
                    {
                        InfernalCrossmod.Thorium.Mod.Call(new object[]
                        {
                        "BonusHealerHealBonus",
                        player,
                        -4
                        });
                    }

                    if (item.type == FindItem("SilvaGuardianHelmet"))
                    {
                        InfernalCrossmod.Thorium.Mod.Call(new object[]
                        {
                        "BonusHealerHealBonus",
                        player,
                        -5
                        });
                    }

                    if (item.type == FindItem("AuricTeslaValkyrieVisage"))
                    {
                        InfernalCrossmod.Thorium.Mod.Call(new object[]
                        {
                    "BonusHealerHealBonus",
                    player,
                    -8
                        });
                    }

                    if (InfernalCrossmod.Catalyst.Loaded)
                    {
                        if (item.type == FindItem("AugmentedAuricTeslaValkyrieVisage"))
                        {
                            InfernalCrossmod.Thorium.Mod.Call(new object[]
                            {
                                "BonusHealerHealBonus",
                                player,
                                -8
                            });
                        }
                    }

                    if (item.type == FindItem("ElementalBloom"))
                    {
                        player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) -= 0.05f;
                        player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) -= 7f;
                    }
                }
            }
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemType<DreadSkull>() && body.type == ItemType<DreadChestPlate>() && legs.type == ItemType<DreadGreaves>())
                return "Dread";

            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "Dread")
            {
                player.maxRunSpeed -= 1.5f;
                player.runAcceleration -= 0.04f;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.EyeoftheGolem) {
                foreach (var line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = Language.GetTextValue("ItemTooltip.EyeoftheGolem");
                        break;
                    }
                }
            }

            if (item.type == ItemType<MaskoftheCrystalEye>()) 
            {
                if (InfernalCrossmod.SOTS.Loaded)
                    InfernalUtilities.FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.MaskoftheCrystalEye.SOTS"));
                else
                    InfernalUtilities.FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.MergedCraftingTreeTooltip.MaskoftheCrystalEye.Thorium"));
            }

            if (item.type == ItemType<BloodPotion>())
                InfernalUtilities.FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.BloodPotion"));

            if (item.type == ItemType<FrenzyPotion>())
            {
                if (InfernalCrossmod.SOTS.Loaded)
                {
                    InfernalUtilities.AddDisabledItemTag(tooltips);
                }
                InfernalUtilities.FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.FrenzyPotion"));
            }

            if (item.type == ItemType<KineticPotion>())
            {
                InfernalUtilities.AddDisabledItemTag(tooltips);
            }

            if (item.type == ItemType<TravelersBoots>())
            {
                InfernalUtilities.FullTooltipOveride(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.TravelBoot"));
            }

            if (item.type == ItemType<DreadGreaves>())
            {
                InfernalUtilities.ReplaceTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DreadGreaves.Orig"), Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DreadGreaves.Nerf"));
            }

            if (InfernalConfig.Instance.DisableDuplicateContent)
            {
                string[] coatings =
                {
                    "DeepFreezeCoatingItem",
                    "ExplosiveCoatingItem",
                    "GorgonCoatingItem",
                    "SporeCoatingItem",
                    "ToxicCoatingItem",
                };

                foreach (string coating in coatings)
                    if (item.type == InfernalCrossmod.Thorium.Mod.Find<ModItem>(coating).Type)
                        InfernalUtilities.AddDisabledItemTag(tooltips);

                string[] disabledItems =
                {
                    //"ChlorophyteTomahawk",
                    "DemonBloodBow",
                    //"MyceliumGatlingGun",
                    "TimeWarp",
                    "MoltenKnife",
                };

                foreach (string disabledItem in disabledItems)
                    if (item.type == InfernalCrossmod.Thorium.Mod.Find<ModItem>(disabledItem).Type)
                        InfernalUtilities.AddDisabledItemTag(tooltips);
            }

            if (InfernalCrossmod.ThoriumRework.Loaded)
            {
                if (item.type == InfernalCrossmod.ThoriumRework.Mod.Find<ModItem>("DeathsingerPotion").Type)
                    InfernalUtilities.AddDisabledItemTag(tooltips);
            }
        }
    }

    public class InfernoLordsFocusAccDamageEdit : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.ModItem?.Mod.Name == "ThoriumMod"
                && item.ModItem.Name == "InfernoLordsFocus";
        }

        public override void SetDefaults(Item entity)
        {
            if (entity.ModItem == null)
                return;

            object modItem = entity.ModItem;

            // Look for field named "accDamage"
            FieldInfo field = modItem.GetType().GetField(
                "accDamage",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (field != null)
            {
                field.SetValue(modItem, "5% basic damage");
            }
        }
    }
}
