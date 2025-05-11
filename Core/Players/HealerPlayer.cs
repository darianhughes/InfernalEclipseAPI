using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Core.Players
{
    public class HealerPlayer : ModPlayer
    {
        private int scytheChargeCooldown;
        private bool initialized;
        public HashSet<int> fifthScytheTypes = new();

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

        public override void PostUpdate()
        {
            if (!initialized)
            {
                initialized = true;
                LoadProjectileTypes();
            }

            if (scytheChargeCooldown > 0)
                scytheChargeCooldown--;

            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && fifthScytheTypes.Contains(projectile.type) && projectile.ModProjectile != null)
                {
                    object modProjectile = projectile.ModProjectile;

                    if (DynamicSetters.SetCanGiveScytheCharge == null)
                    {
                        DynamicSetters.SetCanGiveScytheCharge = CallSite<Func<CallSite, object, bool, object>>.Create(
                            Binder.SetMember(
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
        }

        private void LoadProjectileTypes()
        {
            if (ModLoader.TryGetMod("RagnarokMod", out Mod ragnarok))
            {
                TryAdd(ragnarok, "ScoriaDualscythePro");
                TryAdd(ragnarok, "ProfanedScythePro");
                TryAdd(ragnarok, "MarbleScythePro");
            }

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            {
                string[] names = new[]
                {
                "AquaiteScythePro", "BoneReaperPro", "BloodHarvestPro", "FallingTwilightPro",
                "HallowedScythePro", "TrueHallowedScythePro", "TitanScythePro", "MorningDewPro",
                "DreadTearerPro", "TheBlackScythePro"
            };

                foreach (var name in names)
                    TryAdd(thorium, name);
            }

            void TryAdd(Mod mod, string name)
            {
                if (mod != null && mod.TryFind(name, out ModProjectile proj))
                {
                    fifthScytheTypes.Add(proj.Type);
                }
            }
        }

        public bool CanTriggerChargeEffect() => scytheChargeCooldown <= 0;

        public void TriggerScytheCharge(bool fromServer = false)
        {
            scytheChargeCooldown = 6;

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
