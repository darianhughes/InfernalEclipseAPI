using CalamityMod;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Content.Buffs;
using Terraria.DataStructures;
using InfernalEclipseAPI.Core.World;
using Terraria.Localization;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.NPCs.SupremeCalamitas;
using InfernalEclipseAPI.Core.DamageClasses;
using Terraria.ModLoader.IO;
using InfernalEclipseAPI.Content.Items.Weapons.Legendary.Lycanroc;
using InfernalEclipseAPI.Core.Systems;
using CalamityMod.NPCs.AstrumDeus;
using System.Collections.Generic;
using CalamityMod.Events;
using Terraria.GameInput;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Melee;
using Terraria.UI;
using InfernalEclipseAPI.Content.UI;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.NPCs.AquaticScourge;

namespace InfernalEclipseAPI.Core.Players
{
    public class InfernalPlayer : ModPlayer
    {
        public bool LazyCrafterAmulet;
        public bool statShareAll;
        public bool scalingArmorPenetration;
        public bool flightArmor;
        public bool Earthdrive;

        private const int AdjRadius = 4;

        public override void PlayerConnect()
        {
            if (!InfernalWorld.craftedWorkshop && workshopHasBeenOwned)
            {
                InfernalWorld.craftedWorkshop = true;
            }

            /*
            if (InfernalConfig.Instance.InfernumModeForced && WorldSaveSystem.InfernumModeEnabled == false)
            {
                WorldSaveSystem.InfernumModeEnabled = true;
            }
            */

            if (InfernalConfig.Instance.ForceFullXerocDialogue)
            {
                DownedBossSystem.startedBossRushAtLeastOnce = false;
            }

            if (!InfernalConfig.Instance.DisplayWorldEntryMessages) return;

            Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.MPConnect"), 95, 06, 06);
        }

        public override void OnEnterWorld()
        {
            if (!InfernalWorld.craftedWorkshop && workshopHasBeenOwned)
            {
                InfernalWorld.craftedWorkshop = true;
            }

            if (ModLoader.HasMod("FargowiltasSouls"))
            {
                InGameNotificationsTracker.AddNotification(new FargosSoulsNotification());
            }
            
            if (Main.getGoodWorld)
            {
                InGameNotificationsTracker.AddNotification(new ForTheWorthyNotification());
            }

            //TODO convert these to notifications
            if (ModLoader.HasMod("CWRMod"))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.OverhaulWarning"), 255, 0, 0);
            }

            if (ModLoader.HasMod("Remnants"))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RemnantsWarning"), 255, 255, 06);
            }

            if (ModLoader.HasMod("CalamityMinus"))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.CalMinus"), 255, 255, 06);
            }

            if (ModLoader.HasMod("CalBalChange"))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.CalBalNotice"), 255, 255, 06);
            }

            if (ModLoader.HasMod("InfernumMasterPatch"))
            {
                Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.MasterPatchNotice"), 255, 255, 06);
            }

            if (InfernalWorld.RagnarokModeEnabled)
            {
                if (InfernalConfig.Instance.DisplayWorldEntryMessages)
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.InfernumActive"), 95, 06, 06);
                    SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, Player.Center);
                }
            }
            /*
            else if (InfernalConfig.Instance.InfernumModeForced)
            {
                if (InfernalConfig.Instance.DisplayWorldEntryMessages)
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.InfernumForced"), 95, 06, 06);
                    SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, Player.Center);
                }
                WorldSaveSystem.InfernumModeEnabled = true;
            }
            */

            if (InfernalConfig.Instance.ForceFullXerocDialogue)
            {
                DownedBossSystem.startedBossRushAtLeastOnce = false;
            }

            if (!InfernalConfig.Instance.DisplayWorldEntryMessages) return;

            Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.Welcome"), 95, 06, 06);

            if (ModLoader.HasMod("ThoriumMod"))
            {
                //This message should always popup upon entering a world if they are playing the mod pack.
                if (ModLoader.TryGetMod("ThoriumRework", out Mod rework))
                {
                    //Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.TBRNotice"), 255, 255, 0);
                }
                else
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.TBRWarning"), 255, 0, 0);
                }

                if (!ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RagWarning"), 255, 0, 0);
                }
                else if (ragnarok != null && !InfernalConfig.Instance.AutomatedConfigSetup)
                {
                    Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RagnarokBalance"), 255, 255, 0);

                    if (rework != null && !InfernalConfig.Instance.AutomatedConfigSetup)
                    {
                        Main.NewText(Language.GetTextValue("Mods.InfernalEclipseAPI.WelcomeMessage.RagnarokRework"), 255, 255, 0);
                    }
                }
            }
        }

        private Vector2 previousPos;
        private bool wasUsingItem;
        private int horrifiedTimer = 0;
        private int jamTimer = 0;
        private int batCoinTimer = 0;

        public int resonatorTimer = 0;
        public int incubatorTextTime = 0;
        public int namelessDialogueCooldown;
        public int voidMagePrevention;

        public int CloverCharmCooldown;
        public bool workshopHasBeenOwned;
        public bool batPoop;
        public bool tixThumbRing;
        public bool bloodstainedCoin;
        public bool putridCoin;
        public bool eyeOfChaos;
        public bool snakeEyes;
        public bool chaosBadge;
        public bool focusReticle;
        public bool exoSights;
        public int BoostPressTimer;
        public int BoostDirection;
        public int boostCooldownTime;
        public int RingofRestCooldown;

        public bool singularityCore;

        public override void Initialize()
        {
            workshopHasBeenOwned = false;
            singularityCore = false;
        }

        public override void SaveData(TagCompound tag)
        {
            if (workshopHasBeenOwned)
            {
                tag.Add("workshopHasBeenOwned", true);
            }
            var boost = new List<string>();
            boost.AddWithCondition("singularityCore", singularityCore);

            tag["IEORboost"] = boost;
        }

        public override void LoadData(TagCompound tag)
        {
            workshopHasBeenOwned = tag.Get<bool>("workshopHasBeenOwned");

            var boost = tag.GetList<string>("IEORboost");
            singularityCore = boost.Contains("singularityCore");
        }

        public override bool CanUseItem(Item item)
        {
            if (InfernalCrossmod.Thorium.Loaded)
            {
                if (Player.HasBuff(InfernalCrossmod.Thorium.Mod.Find<ModBuff>("Bubbled").Type))
                    return false;
            }

            if (ModLoader.TryGetMod("XDContentMod", out Mod heartbeat))
            {
                if (Player.mount.Active)
                {
                    if (Player.mount?.Type == heartbeat.Find<ModMount>("TapTapMinivan").Type || Player.mount?.Type == heartbeat.Find<ModMount>("LuxuryConvertible").Type || Player.mount?.Type == heartbeat.Find<ModMount>("DiDiCar").Type || Player.mount?.Type == heartbeat.Find<ModMount>("DiDiBike").Type || Player.mount?.Type == heartbeat.Find<ModMount>("KFCDeliveryScooter").Type)
                    {
                        return false;
                    }
                }
            }

            return base.CanUseItem(item);
        }

        public override void ResetEffects()
        {
            if (!Player.HasBuff(ModContent.BuffType<StarboundHorrification>()))
                horrifiedTimer = 0;

            if (!Player.HasBuff(ModContent.BuffType<WarpJammed>()))
                jamTimer = 0;

            if (namelessDialogueCooldown > 0)
                namelessDialogueCooldown--;

            if (CloverCharmCooldown > 0)
                CloverCharmCooldown--;

            if (incubatorTextTime > 0)
                incubatorTextTime--;

            if (voidMagePrevention > 0)
                voidMagePrevention--;

            if (boostCooldownTime > 0)
                boostCooldownTime--;

            if (batPoop)
            {
                batCoinTimer++;
                if (batCoinTimer == 60 * 5)
                {
                    Player.QuickSpawnItem(Player.GetSource_Misc("IEoR_PoopCoin"), ItemID.GoldCoin, Main.rand.Next(1, 6));
                    batCoinTimer = 0;
                }
            }
            else
                batCoinTimer = 0;

            if (resonatorTimer > 0)
                resonatorTimer--;

            if (resonatorTimer == 1)
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 pos = Player.Center + new Vector2(Main.rand.NextFloat(-16f, 16f), Main.rand.NextFloat(-24f, 8f));
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(-0.6f, -0.1f));

                    int d = Dust.NewDust(pos, 0, 0, DustID.Cloud, vel.X, vel.Y, 150, default, Main.rand.NextFloat(1.0f, 1.6f));
                    Main.dust[d].noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
            }
            if (namelessDialogueCooldown <= 0)
                InfernalWorld.namelessDeveloperDiagloguePlayed = false;

            soltanBullying = false;
            HarvestMoonBuff = false;
            scalingArmorPenetration = false;
            statShareAll = false;
            LazyCrafterAmulet = false;
            batPoop = false;
            bloodstainedCoin = false;
            putridCoin = false;
            eyeOfChaos = false;
            snakeEyes = false;
            chaosBadge = false;
            focusReticle = false;
            exoSights = false;
            flightArmor = false;
        }

        public override void PreUpdate()
        {
            if (BoostPressTimer > 0)
                BoostPressTimer--;

            if (Player.ZoneLihzhardTemple && !NPC.downedPlantBoss)
            {
                Player.statLife -= 1;
                if (Player.statLife == 0)
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} fell to the jungles curse..."), 0, 0);
                Player.AddBuff(BuffID.PotionSickness, 60);
            }

            if (InfernalCrossmod.Thorium.Loaded) 
            {
                if (Player.IsUnderwater() && NPC.AnyNPCs(InfernalCrossmod.Thorium.Mod.Find<ModNPC>("QueenJellyfish").Type))
                {
                    Player.AddBuff(InfernalCrossmod.Thorium.Mod.Find<ModBuff>("Bubbled").Type, 60);
                    Player.AddBuff(BuffID.Electrified, 60);
                }

                if (Player.HasBuff(InfernalCrossmod.Thorium.Mod.Find<ModBuff>("RealityBearer").Type) && BossRushEvent.BossRushActive)
                    Player.ClearBuff(InfernalCrossmod.Thorium.Mod.Find<ModBuff>("RealityBearer").Type);
            }
        }

        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<StarboundHorrification>()))
            {
                horrifiedTimer++;

                // Give a 1 second grace period after first applying the buff
                if (horrifiedTimer < 60)
                {
                    previousPos = Player.position;
                    wasUsingItem = Player.itemAnimation > 0;
                    return;
                }

                float distanceMoved = Vector2.Distance(Player.position, previousPos);
                bool usedTeleportItem = !wasUsingItem && Player.itemAnimation > 0 &&
                    (Player.HeldItem.type == ItemID.MagicMirror ||
                     Player.HeldItem.type == ItemID.IceMirror ||
                     Player.HeldItem.type == ItemID.RecallPotion ||
                     Player.HeldItem.type == ItemID.WormholePotion ||
                     Player.HeldItem.type == ItemID.PotionOfReturn ||
                     Player.HeldItem.type == ItemID.CellPhone ||
                     Player.HeldItem.type == ItemID.Shellphone ||
                     Player.HeldItem.type == ItemID.ShellphoneHell ||
                     Player.HeldItem.type == ItemID.ShellphoneOcean ||
                     Player.HeldItem.type == ItemID.ShellphoneSpawn ||
                     Player.HeldItem.type == ItemID.DemonConch ||
                     Player.HeldItem.type == ItemID.MagicConch ||
                     Player.HeldItem.type == ItemID.TeleportationPotion);

                if (distanceMoved > 1000f || usedTeleportItem)
                {
                    SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh, Player.Center);
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} tried to escape the multiversal terror."), 9999.0, 0);
                }

                previousPos = Player.position;
                wasUsingItem = Player.itemAnimation > 0;
            }

            if (Player.HasBuff(ModContent.BuffType<WarpJammed>()))
            {
                jamTimer++;

                // Give a 3 second grace period after first applying the buff
                if (jamTimer < 180)
                {
                    previousPos = Player.position;
                    wasUsingItem = Player.itemAnimation > 0;
                    return;
                }

                float distanceMoved = Vector2.Distance(Player.position, previousPos);
                bool usedTeleportItem = !wasUsingItem && Player.itemAnimation > 0 &&
                    (Player.HeldItem.type == ItemID.MagicMirror ||
                     Player.HeldItem.type == ItemID.IceMirror ||
                     Player.HeldItem.type == ItemID.RecallPotion ||
                     Player.HeldItem.type == ItemID.WormholePotion ||
                     Player.HeldItem.type == ItemID.PotionOfReturn ||
                     Player.HeldItem.type == ItemID.CellPhone ||
                     Player.HeldItem.type == ItemID.Shellphone ||
                     Player.HeldItem.type == ItemID.ShellphoneHell ||
                     Player.HeldItem.type == ItemID.ShellphoneOcean ||
                     Player.HeldItem.type == ItemID.ShellphoneSpawn ||
                     Player.HeldItem.type == ItemID.DemonConch ||
                     Player.HeldItem.type == ItemID.MagicConch ||
                     Player.HeldItem.type == ItemID.TeleportationPotion);

                if (distanceMoved > 1000f || usedTeleportItem)
                {
                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaShootSound, Player.Center);
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name} tried to escape draedon's creations."), 9999.0, 0);
                }

                previousPos = Player.position;
                wasUsingItem = Player.itemAnimation > 0;
            }

            if (!NPC.downedBoss3 && !Main.hardMode && InfernalConfig.Instance.BossKillCheckOnOres && Player.HasBuff(BuffID.Bewitched))
                Player.ClearBuff(BuffID.Bewitched);

            if (RingofRestCooldown > 0)
                RingofRestCooldown--;
        }

        public bool soltanBullying = false;
        public bool HarvestMoonBuff = false;

        public override void PostUpdateMiscEffects()
        {
            if (soltanBullying)
            {
                float emptySummonSlots = Player.maxMinions - Player.slotsMinions;
                ref StatModifier melee = ref Player.GetDamage(DamageClass.Melee);
                melee += (float)(0.02 * emptySummonSlots);
                ref StatModifier ranged = ref Player.GetDamage(DamageClass.Ranged);
                ranged += (float)(0.02 * emptySummonSlots);
                ref StatModifier magic = ref Player.GetDamage(DamageClass.Magic);
                magic += (float)(0.02 * emptySummonSlots);
                ref StatModifier throwing = ref Player.GetDamage(DamageClass.Throwing);
                throwing += (float)(0.02 * emptySummonSlots);

                ref StatModifier summon = ref Player.GetDamage(DamageClass.Summon);
                summon -= (float)(0.1 * Player.slotsMinions);
            }
        }

        public override void PostUpdateEquips()
        {
            if (exoSights || focusReticle)
            {
                Player.GetCritChance(DamageClass.Generic) += 15f;
            }
            else if (eyeOfChaos)
            {
                Player.GetCritChance(DamageClass.Generic) += 12f;
            }
            else
            {
                if (snakeEyes)
                {
                    Player.GetCritChance(DamageClass.Generic) += 3f;
                }

                if (chaosBadge)
                {
                    Player.GetCritChance(DamageClass.Generic) += 5f;
                }
            }

            if (LazyCrafterAmulet)
            {
                Player.adjTile[TileID.WorkBenches] = true;
                Player.adjTile[TileID.Furnaces] = true;
                Player.adjTile[TileID.Anvils] = true;
                Player.adjTile[TileID.Bottles] = true;
                Player.adjTile[TileID.Tables] = true;
            }

            if (statShareAll)
            {
                var meleeDamage = Player.GetDamage(DamageClass.Melee);
                float meleeAdd = (meleeDamage.Additive - 1f) * 0.1f;
                float meleeFlat = meleeDamage.Flat * 0.1f;
                float meleeMult = ((meleeDamage.Multiplicative - 1f) * 0.1f) + 1f;
                float meleeBase = meleeDamage.Base * 0.1f;

                var rangedDamage = Player.GetDamage(DamageClass.Ranged);
                float rangedAdd = (rangedDamage.Additive - 1f) * 0.1f;
                float rangedFlat = rangedDamage.Flat * 0.1f;
                float rangedMult = ((rangedDamage.Multiplicative - 1f) * 0.1f) + 1f;
                float rangedBase = rangedDamage.Base * 0.1f;

                var magicDamage = Player.GetDamage(DamageClass.Magic);
                float magicAdd = (magicDamage.Additive - 1f) * 0.1f;
                float magicFlat = magicDamage.Flat * 0.1f;
                float magicMult = ((magicDamage.Multiplicative - 1f) * 0.1f) + 1f;
                float magicBase = magicDamage.Base * 0.1f;

                var summonDamage = Player.GetDamage(DamageClass.Summon);
                float summonAdd = (summonDamage.Additive - 1f) * 0.1f;
                float summonFlat = summonDamage.Flat * 0.1f;
                float summonMult = ((summonDamage.Multiplicative - 1f) * 0.1f) + 1f;
                float summonBase = summonDamage.Base * 0.1f;

                if (meleeAdd > 0f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var melee = ref Player.GetDamage(DamageClass.Melee);
                    generic += meleeAdd;
                    melee -= meleeAdd;
                }

                if (meleeFlat > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += meleeFlat;
                    Player.GetDamage(DamageClass.Melee).Flat -= meleeFlat;
                }

                if (meleeMult > 1f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var melee = ref Player.GetDamage(DamageClass.Melee);
                    generic *= meleeMult;
                    melee /= meleeMult;
                }

                if (meleeBase > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Base += meleeBase;
                    Player.GetDamage(DamageClass.Melee).Base -= meleeBase;
                }

                if (rangedAdd > 0f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var ranged = ref Player.GetDamage(DamageClass.Ranged);
                    generic += rangedAdd;
                    ranged -= rangedAdd;
                }

                if (rangedFlat > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += rangedFlat;
                    Player.GetDamage(DamageClass.Ranged).Flat -= rangedFlat;
                }

                if (rangedMult > 1f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var ranged = ref Player.GetDamage(DamageClass.Ranged);
                    generic *= rangedMult;
                    ranged /= rangedMult;
                }

                if (rangedBase > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Base += rangedBase;
                    Player.GetDamage(DamageClass.Ranged).Base -= rangedBase;
                }

                if (magicAdd > 0f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var magic = ref Player.GetDamage(DamageClass.Magic);
                    generic += magicAdd;
                    magic -= magicAdd;
                }

                if (magicFlat > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += magicFlat;
                    Player.GetDamage(DamageClass.Magic).Flat -= magicFlat;
                }

                if (magicMult > 1f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var magic = ref Player.GetDamage(DamageClass.Magic);
                    generic *= magicMult;
                    magic /= magicMult;
                }

                if (magicBase > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Base += magicBase;
                    Player.GetDamage(DamageClass.Magic).Base -= magicBase;
                }

                if (summonAdd > 0f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var summon = ref Player.GetDamage(DamageClass.Summon);
                    generic += summonAdd;
                    summon -= summonAdd;
                }

                if (summonFlat > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Flat += summonFlat;
                    Player.GetDamage(DamageClass.Summon).Flat -= summonFlat;
                }

                if (summonMult > 1f)
                {
                    ref var generic = ref Player.GetDamage(DamageClass.Generic);
                    ref var summon = ref Player.GetDamage(DamageClass.Summon);
                    generic *= summonMult;
                    summon /= summonMult;
                }

                if (summonBase > 0f)
                {
                    Player.GetDamage(DamageClass.Generic).Base += summonBase;
                    Player.GetDamage(DamageClass.Summon).Base -= summonBase;
                }
            }

            if (Earthdrive)
            {
                float meleeSpeedBonus = Player.GetAttackSpeed(DamageClass.Melee) - 1f;
                float miningSpeedBonus = 1f - Player.pickSpeed;
                if (meleeSpeedBonus > 0.0)
                    Player.pickSpeed -= meleeSpeedBonus;
                if (miningSpeedBonus > 0.0)
                {
                    if (miningSpeedBonus > 0.1f)
                        miningSpeedBonus = 0.1f;
                    Player.GetAttackSpeed(DamageClass.Melee) += miningSpeedBonus;
                }
            }
            Earthdrive = false;
        }

        private bool oceanBufferModified = false;
        public override void PostUpdateBuffs()
        {
            if (InfernalCrossmod.SOTS.Loaded)
            {
                int idx = Player.FindBuffIndex(ModContent.BuffType<VoidSickness2>());
                if (idx == -1)
                    return;

                float time = Player.buffTime[idx];

                ref StatModifier local = ref Player.GetDamage(DamageClass.Generic);
                local -= (float)(0.25 * (time / 300f));
            }

            if (InfernalCrossmod.Thorium.Loaded && InfernalConfig.Instance.ThoriumBalanceChangess && !InfernalCrossmod.Hummus.Loaded)
            {
                if (ModContent.TryFind<ModBuff>("ThoriumMod", "OceansBufferExhaust", out var buff))
                {
                    for (int i = 0; i < Player.buffType.Length; i++)
                    {
                        if (Player.buffType[i] == buff.Type && Player.buffTime[i] > 0)
                        {
                            if (!oceanBufferModified)
                            {
                                Player.buffTime[i] = (int)(Player.buffTime[i] * 2.5f);
                                oceanBufferModified = true;
                            }
                            break; // stop looping once we found the buff
                        }
                    }
                }
                else
                {
                    oceanBufferModified = false; // reset if buff is gone
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (InfernalEclipseAPI.SubpaceBoostHotkey.JustPressed && boostCooldownTime <= 15)
            {
                BoostPressTimer = 2;
                BoostDirection =
                    Player.controlRight ? 1 :
                    Player.controlLeft ? -1 :
                    Player.direction;

                if (boostCooldownTime >= 5)
                    boostCooldownTime = 225;
                else
                    boostCooldownTime = 135;
            }
        }

        public void ConvertSummonMeleeToMelee(Player player, Item item, ref StatModifier damage)
        {
            if (item.DamageType == ModContent.GetInstance<MeleeWhip>())
                item.DamageType = DamageClass.SummonMeleeSpeed;

            if (!soltanBullying || item is null || item.IsAir)
                return;

            var summonMeleeSpeed = ModContent.GetInstance<SummonMeleeSpeedDamageClass>();
            if (!item.CountsAsClass(summonMeleeSpeed))
                return;

            // Replace the item's damage scaling with Melee scaling:
            /*
            float meleeScale = player.GetTotalDamage(DamageClass.Melee).ApplyTo(1f);
            float sourceScale = player.GetTotalDamage(summonMeleeSpeed).ApplyTo(1f);
            float ratio = meleeScale / MathF.Max(sourceScale, 1e-6f);
            */
            item.DamageType = ModContent.GetInstance<MeleeWhip>();

            //damage *= ratio;       // mimic Melee scaling
            damage *= 1.10f;       // extra 10% while SoltanBullying
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            ConvertSummonMeleeToMelee(Player, item, ref damage);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (scalingArmorPenetration)
            {
                modifiers.DefenseEffectiveness *= Main.hardMode ? 0.25f : 0.2f;
            }

            if (InfernalCrossmod.ThoriumRework.Loaded)
            {
                if (target.type == InfernalCrossmod.Thorium.Mod.Find<ModNPC>("BoreanHopper").Type)
                {
                    modifiers.FinalDamage *= 0.2f;
                }
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if ((target.type == ModContent.NPCType<AstrumDeusHead>() || target.type == ModContent.NPCType<AstrumDeusBody>() || target.type == ModContent.NPCType<AstrumDeusTail>()) && !NPC.downedAncientCultist)
            {
                modifiers.FinalDamage *= 0.2f;
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if ((proj.type == ModContent.ProjectileType<CelestusProj>() || proj.type == ModContent.ProjectileType<CelestusMiniScythe>()) &&
                (target.type == ModContent.NPCType<SepulcherHead>() || target.type == ModContent.NPCType<SepulcherBody>() || target.type == ModContent.NPCType<SepulcherTail>()) &&
                InfernalConfig.Instance.PreventBossCheese)
            {
                modifiers.FinalDamage *= 0.2f;
            }

            if ((target.type == ModContent.NPCType<AstrumDeusHead>() || target.type == ModContent.NPCType<AstrumDeusBody>() || target.type == ModContent.NPCType<AstrumDeusTail>()) && !NPC.downedAncientCultist)
            {
                modifiers.FinalDamage *= 0.8f;
            }

            if (target.type == ModContent.NPCType<Yharon>() && target.life < target.lifeMax / 4 && (proj.type == ModContent.ProjectileType<GalaxySmasherHammer>() || proj.type == ModContent.ProjectileType<GalaxySmasherBlast>() || proj.type == ModContent.ProjectileType<GalaxySmasherEcho>() || proj.type == ModContent.ProjectileType<GalaxySmasherMini>()) &&
                InfernalConfig.Instance.PreventBossCheese)
            {
                modifiers.FinalDamage /= 2;
            }

            if (target.type == NPCID.TheDestroyer || target.type == NPCID.TheDestroyerBody || target.type == NPCID.TheDestroyerTail &&  InfernalConfig.Instance.PreventBossCheese)
            {
                if (proj.type == ModContent.ProjectileType<SubmarineShockerProj>())
                    modifiers.FinalDamage *= 0.2f;
                
                if (InfernalCrossmod.ThoriumRework.Loaded)
                {
                    if (proj.type == InfernalCrossmod.ThoriumRework.Mod.Find<ModProjectile>("BeholderBlade").Type || proj.type == InfernalCrossmod.ThoriumRework.Mod.Find<ModProjectile>("Void").Type)
                        modifiers.FinalDamage /= 2;
                }
            }

            if (target.type == ModContent.NPCType<AquaticScourgeHead>() || target.type == ModContent.NPCType<AquaticScourgeBody>() || target.type == ModContent.NPCType<AquaticScourgeTail>())
            {
                if (InfernalCrossmod.ThoriumRework.Loaded)
                {
                    if (proj.type == InfernalCrossmod.ThoriumRework.Mod.Find<ModProjectile>("BeholderBlade").Type || proj.type == InfernalCrossmod.ThoriumRework.Mod.Find<ModProjectile>("Void").Type)
                        modifiers.FinalDamage /= 2;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Player.whoAmI != Main.myPlayer) return;

            if (!proj.npcProj && !proj.trap && proj.friendly)
            {
                LycanrocGlobalProjectile lgp = proj.GetGlobalProjectile<LycanrocGlobalProjectile>();
                if (lgp.appliesArmorCrunch)
                {
                    target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 180);
                }

                if (lgp.appliesCrumbling)
                {
                    target.AddBuff(ModContent.BuffType<Crumbling>(), 180);
                }
            }

            if (tixThumbRing && proj.arrow && hit.Crit)
                target.AddBuff(BuffID.ShadowFlame, 60, false);
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            TryCoinDebuff();
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Terraria.Player.HurtModifiers modifiers)
        {
            //Reverts vanilla trap damage nerf
            ref StatModifier sourceDamage = ref modifiers.SourceDamage;

            if (proj.type == ProjectileID.Explosives)
            {
                sourceDamage /= Main.expertMode ? 0.225f : 0.35f;
            }
            else if (proj.type == ProjectileID.RollingCactus || proj.type == ProjectileID.RollingCactusSpike)
            {
                sourceDamage /= Main.expertMode ? 0.3f : 0.5f;
            }

            if (!Main.expertMode)
                return;

            if (proj.type == ProjectileID.Boulder || proj.type == ProjectileID.MiniBoulder)
            {
                sourceDamage /= 0.65f;
            }
            else if (proj.type == ProjectileID.SpikyBallTrap || proj.type == ProjectileID.FlamethrowerTrap || proj.type == ProjectileID.PoisonDartTrap)
            {
                sourceDamage /= 0.625f;
            }
            else if (proj.type == ProjectileID.SpearTrap)
            {
                sourceDamage /= 0.6f;
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            TryCoinDebuff();
        }
        private void TryCoinDebuff()
        {
            if (bloodstainedCoin || putridCoin)
            {
                if (Main.rand.Next(4) != 0)
                {
                    if (putridCoin)
                        Player.AddBuff(BuffID.Poisoned, 1020, false);
                    if (bloodstainedCoin)
                        Player.AddBuff(BuffID.Bleeding, 1020, false);
                }
            }
        }
    }

    public class SoltanGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            int owner = projectile.owner;
            if (owner < 0 || owner >= Main.maxPlayers)
                return;

            Player p = Main.player[owner];
            var mp = p.GetModPlayer<InfernalPlayer>();
            if (!mp.soltanBullying)
                return;

            var summonMeleeSpeed = ModContent.GetInstance<SummonMeleeSpeedDamageClass>();
            if (projectile.DamageType == summonMeleeSpeed)
            {
                // Make the projectile actually "be" Melee so Melee-only effects can see it.
                projectile.DamageType = DamageClass.Melee;
                projectile.netUpdate = true;
            }
        }
    }
}
