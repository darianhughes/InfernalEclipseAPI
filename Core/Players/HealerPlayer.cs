using InfernalEclipseAPI.Common.GlobalProjectiles;
using InfernalEclipseAPI.Content.Buffs;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ThoriumMod;

namespace InfernalEclipseAPI.Core.Players
{
    //Original code provided by Wardrobe Hummus
    public class HealerPlayer : ModPlayer
    {
        private int scytheChargeCooldown;
        private bool initialized;
        public HashSet<int> fifthScytheTypes = new();

        public bool accessoryEquipped = false;

        private int contractCooldownTimer = 0;
        private bool restoreContractAfterCooldown = false;

        private int executionersContract = -1;
        private int sealedContract = -1;
        private bool ContractInitialized = false;

        public int renewCooldown;
        public int starBirthCooldown;

        public bool buffBubbleBulwarkWandCooldown;

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (buffBubbleBulwarkWandCooldown)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<BubbleShock>(), 5400);
            }
            return base.ConsumableDodge(info);
        }


        public override void ResetEffects()
        {
            if (renewCooldown > 0)
                renewCooldown--;
            if (starBirthCooldown > 0)
                starBirthCooldown--;
            buffBubbleBulwarkWandCooldown = false;
        }

        // Dynamic callsite storage
        private static class DynamicSetters
        {
            public static CallSite<Func<CallSite, object, bool, object>> SetCanGiveScytheCharge;
        }

        public override void Initialize()
        {
            scytheChargeCooldown = 0;
            initialized = false;
        }

        private void EnsureInitialized()
        {
            if (ContractInitialized) return;

            if (ModContent.TryFind<ModItem>("ThoriumRework/ExecutionersContract", out var modItem1))
            {
                executionersContract = modItem1.Type;
            }

            if (ModContent.TryFind<ModItem>("ThoriumRework/SealedContract", out var modItem2))
            {
                sealedContract = modItem2.Type;
            }

            ContractInitialized = true;
        }


        public override void PostUpdate()
        {
            EnsureInitialized();
            if (!initialized)
            {
                initialized = true;
                LoadProjectileTypes();
            }

            if (scytheChargeCooldown > 0)
                scytheChargeCooldown--;

            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && fifthScytheTypes.Contains(projectile.type) && projectile.ModProjectile != null && !ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                {
                    object modProjectile = projectile.ModProjectile;

                    if (DynamicSetters.SetCanGiveScytheCharge == null)
                    {
                        DynamicSetters.SetCanGiveScytheCharge = CallSite<Func<CallSite, object, bool, object>>.Create(
                            Microsoft.CSharp.RuntimeBinder.Binder.SetMember(
                                CSharpBinderFlags.None,
                                "CanGiveScytheCharge",
                                typeof(HealerPlayer),
                                new[]
                                {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                                }
                            )
                        );
                    }

                    DynamicSetters.SetCanGiveScytheCharge.Target(
                        DynamicSetters.SetCanGiveScytheCharge,
                        modProjectile,
                        scytheChargeCooldown <= 0
                    );
                }
            }

            if (accessoryEquipped)
            {
                if (contractCooldownTimer > 0)
                {
                    contractCooldownTimer--;
                    if (contractCooldownTimer == 0 && restoreContractAfterCooldown)
                    {
                        SetContract(true);
                        restoreContractAfterCooldown = false;
                    }
                }
            }
            else
            {
                SetContract(false);
            }
        }

        public void OnProjectileHit()
        {
            if (accessoryEquipped)
            {
                bool contractNow = GetContract();

                if (contractNow && contractCooldownTimer == 0)
                {
                    SetContract(true);
                    contractCooldownTimer = 90; //EASY CHANGE COOLDOWN NUMBER
                    restoreContractAfterCooldown = true;
                }
                else if (contractCooldownTimer > 0)
                {
                    SetContract(false);
                }
            }
        }
        public override void UpdateEquips()
        {
            EnsureInitialized();

            accessoryEquipped = false;

            // We only want to check up to the smaller of these two lengths
            int maxSlot = Math.Min(Player.armor.Length, Player.hideVisibleAccessory.Length);

            for (int i = 3; i < maxSlot; i++) // starts at 3 to skip armor slots
            {
                Item accessory = Player.armor[i];

                if (accessory.IsAir)
                    continue;

                if (accessory.type == executionersContract || accessory.type == sealedContract)
                {
                    // We're ignoring visibility here, so just set true and break
                    accessoryEquipped = true;
                    break;
                }
            }
        }

        private bool GetContract()
        {
            object thoriumPlayer = GetThoriumPlayer();
            if (thoriumPlayer == null)
                return false;

            var contractField = thoriumPlayer.GetType().GetField("contract", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (contractField == null)
                return false;

            return (bool)contractField.GetValue(thoriumPlayer);
        }

        private void SetContract(bool value)
        {
            object thoriumPlayer = GetThoriumPlayer();
            if (thoriumPlayer == null)
                return;

            var contractField = thoriumPlayer.GetType().GetField("contract", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            contractField?.SetValue(thoriumPlayer, value);
        }

        private object GetThoriumPlayer()
        {
            if (!ModLoader.TryGetMod("ThoriumRework", out var thoriumRework))
                return null;

            var thoriumPlayerType = thoriumRework.Code?.GetType("ThoriumRework.ThoriumPlayer");
            if (thoriumPlayerType == null)
                return null;

            var getModPlayerMethod = typeof(Player).GetMethod("GetModPlayer", 1, Type.EmptyTypes);
            var genericMethod = getModPlayerMethod?.MakeGenericMethod(thoriumPlayerType);
            return genericMethod?.Invoke(Player, null);
        }

        private void LoadProjectileTypes()
        {
            void TryAdd(Mod mod, string name)
            {
                if (mod != null && mod.TryFind(name, out ModProjectile proj))
                    fifthScytheTypes.Add(proj.Type);
            }

            ModLoader.TryGetMod("RagnarokMod", out Mod ragnarokMod);
            ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod);

            // Ragnarok
            TryAdd(ragnarokMod, "ScoriaDualscythePro");
            TryAdd(ragnarokMod, "ProfanedScythePro");
            TryAdd(ragnarokMod, "MarbleScythePro");

            // Thorium
            string[] thoriumProjs = {
                "AquaiteScythePro", "MoltenThresherPro", "BatScythePro", "BoneReaperPro", "BloodHarvestPro", "FallingTwilightPro",
                "HallowedScythePro", "TrueHallowedScythePro", "TitanScythePro", "MorningDewPro",
                "DreadTearerPro", "TheBlackScythePro", "LustrousBatonPro"
            };

            foreach (string name in thoriumProjs)
                TryAdd(thoriumMod, name);
        }

        public bool CanTriggerChargeEffect() => scytheChargeCooldown <= 0;

        public void TriggerScytheCharge(bool fromServer = false)
        {
            scytheChargeCooldown = 2;

            if (Main.netMode == NetmodeID.MultiplayerClient && !fromServer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)0);
                packet.Write((byte)Player.whoAmI);
                packet.Send();
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["scytheChargeCooldown"] = scytheChargeCooldown;
        }

        public override void LoadData(TagCompound tag)
        {
            scytheChargeCooldown = tag.GetInt("scytheChargeCooldown");
        }
    }
}
