using CalamityMod.DataStructures;
using CalamityMod.Systems.Collections;
using InfernalEclipseAPI.Content.Buffs.Tag;
using InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.GameContent;
using ThoriumMod.Buffs.Summon;
using ThoriumMod.Items.SummonItems;
using WHummusMultiModBalancing;
using InfernalEclipseAPI.Common.GlobalItems;

namespace InfernalEclipseAPI.Core.Systems
{
    public class InfernalEclipseAPISummonTagSystem : ModSystem
    {
        public bool WHummusEnabled = ModLoader.TryGetMod("WHummusMultiModBalancing", out _);

        private struct SummonTagEntry
        {
            public Func<int> ItemType;

            public Func<int> BuffType;

            public Action<SummonTag> Setup;
        }

        public override void PostSetupContent()
        {
            List<SummonTagEntry> entries = new();

            // API ENTRY

            entries.Add(new SummonTagEntry
            {
                ItemType = () => ModContent.ItemType<SplitFirebrand>(),
                BuffType = () => ModContent.BuffType<SplitFirebrandTag>(),
                Setup = delegate (SummonTag summonTag)
                {
                    summonTag.AutoDrawTooltip = false;
                    summonTag.TagTexture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrand", (AssetRequestMode)1);
                }
            });

            //Checking for my mod so this can go to live on my end straight away
            //Also you can't register multiple tag entrys to the same item so we'll roll with mine for now
            if (!WHummusEnabled)
            {
                // THORIUM ENTRY

                entries.Add(new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Thrombosis>(),
                    BuffType = () => ModContent.BuffType<ThrombosisDebuff>(),
                    Setup = delegate (SummonTag summonTag)
                    {
                        summonTag.AutoDrawTooltip = false;
                        summonTag.TagTexture = ModContent.Request<Texture2D>("ThoriumMod/Items/SummonItems/Thrombosis", AssetRequestMode.ImmediateLoad);
                    }
                });

                // OPTIONAL MOD SUPPORT

                if (ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
                {
                    if (catalyst.TryFind("CoralCrusher", out ModItem item))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item.Type,
                            BuffType = () => ModContent.BuffType<CoralCrusherTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/CoralCrusher", AssetRequestMode.ImmediateLoad);
                                summonTag.MultiplicativeTagDamage = 0.04f;
                            }
                        });
                    }
                    if (catalyst.TryFind("PrismBreak", out ModItem item2))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item2.Type,
                            BuffType = () => ModContent.BuffType<PrismBreakTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/PrismBreak", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                    if (catalyst.TryFind("CongeledDuoWhip", out ModItem item3))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item3.Type,
                            BuffType = () => ModContent.BuffType<CongeledDuoWhipTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/CongeledDuoWhip", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                    if (catalyst.TryFind("UnderBite", out ModItem item4))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item4.Type,
                            BuffType = () => ModContent.BuffType<UnderBiteTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/UnderBite", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                    if (catalyst.TryFind("SandstoneReigns", out ModItem item5))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item5.Type,
                            BuffType = () => ModContent.BuffType<SandstoneReignsTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/SandstoneReigns", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                    if (catalyst.TryFind("ResonantStriker", out ModItem item6))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item6.Type,
                            BuffType = () => ModContent.BuffType<ResonantStrikerTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/ResonantStriker", AssetRequestMode.ImmediateLoad);
                                summonTag.MultiplicativeTagDamage = 0.08f;
                            }
                        });
                    }
                    if (catalyst.TryFind("BlossomsBlessing", out ModItem item7))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item7.Type,
                            BuffType = () => ModContent.BuffType<BlossomsBlessingTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/BlossomsBlessing", AssetRequestMode.ImmediateLoad);
                                summonTag.MultiplicativeTagDamage = 0.12f;
                            }
                        });
                    }
                    if (catalyst.TryFind("UnrelentingTorment", out ModItem item8))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item8.Type,
                            BuffType = () => ModContent.BuffType<UnrelentingTormentTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/UnrelentingTorment", AssetRequestMode.ImmediateLoad);
                                summonTag.MultiplicativeTagDamage = 0.10f;
                            }
                        });
                    }
                    if (catalyst.TryFind("Catharsis", out ModItem item9))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item9.Type,
                            BuffType = () => ModContent.BuffType<CatharsisTag>(),
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CatalystMod/Items/Weapons/Summon/Whips/Catharsis", AssetRequestMode.ImmediateLoad);
                                summonTag.MultiplicativeTagDamage = 0.12f;
                            }
                        });
                    }
                }

                if (ModLoader.TryGetMod("CalamityHunt", out Mod hunt))
                {
                    if (hunt.TryFind("Gobflogger", out ModItem item) && hunt.TryFind("Gobbed", out ModBuff buff))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item.Type,
                            BuffType = () => buff.Type,
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("CalamityHunt/Content/Items/Weapons/Summoner/Gobflogger", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                }

                if (ModLoader.TryGetMod("SOTS", out Mod sots))
                {
                    if (sots.TryFind("BrassWhip", out ModItem item) && sots.TryFind("BrassWhipDebuff", out ModBuff buff))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item.Type,
                            BuffType = () => buff.Type,
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("SOTS/Items/Whips/BrassWhip", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                    if (sots.TryFind("GlowWhip", out ModItem item2) && sots.TryFind("GlowWhipDebuff", out ModBuff buff2))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item2.Type,
                            BuffType = () => buff2.Type,
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("SOTS/Items/Whips/GlowWhip", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                    if (sots.TryFind("KelpWhip", out ModItem item3) && sots.TryFind("KelpWhipBuff", out ModBuff buff3))
                    {
                        entries.Add(new SummonTagEntry
                        {
                            ItemType = () => item3.Type,
                            BuffType = () => buff3.Type,
                            Setup = delegate (SummonTag summonTag)
                            {
                                summonTag.AutoDrawTooltip = false;
                                summonTag.TagTexture = ModContent.Request<Texture2D>("SOTS/Items/Whips/KelpWhip", AssetRequestMode.ImmediateLoad);
                            }
                        });
                    }
                }
            }

            foreach (SummonTagEntry summonTagEntry in entries)
            {
                SummonTag tag =
                    new SummonTag(summonTagEntry.ItemType());

                summonTagEntry.Setup?.Invoke(tag);

                int debuff = summonTagEntry.BuffType();

                if (!CalamityBuffSets.SummonTagDebuff.ContainsKey(debuff))
                {
                    CalamityBuffSets.SummonTagDebuff.Add(debuff, tag);
                }
            }
        }
    }
}
