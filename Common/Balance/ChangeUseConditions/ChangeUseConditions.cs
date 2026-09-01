using Terraria.Localization;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Core.World;
using Terraria.Audio;
using Terraria.Chat;
using CalamityMod.NPCs.Yharon;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Core.Configs;

namespace InfernalEclipseAPI.Common.Balance.ChangeUseConditions
{
    public class ChangeUseConditions : GlobalItem
    {
        private static int CurseID;
        private static int renewID;
        private static int starBirthID;
        private static int lostOasisID;

        public delegate bool CanItemDoActionWithPlayerDelegate(Item item, Player player);

        public static event CanItemDoActionWithPlayerDelegate CanUseItemEvent;
        public override void Unload()
        {
            CanUseItemEvent = null;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (CanUseItemEvent is null)
                return true;

            bool result = true;
            foreach (Delegate d in CanUseItemEvent.GetInvocationList())
                result &= ((CanItemDoActionWithPlayerDelegate)d).Invoke(item, player);

            return result;
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            ModLoader.TryGetMod("InfernumMode", out Mod hell);
            if (hell.TryFind("DungeonsCurse", out ModItem curse))
            {
                CurseID = curse.Type;
            }

            CanUseItemEvent += ModifyDungeonCurseUseConditions;

            if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                if (thorium.TryFind("Renew", out ModItem renew))
                    renewID = renew.Type;

                CanUseItemEvent += ModifyRenewUseConditions;

                if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calBardHeal) && ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
                {
                    if (calBardHeal.TryFind("StarBirth", out ModItem starBirth))
                        starBirthID = starBirth.Type;
                    if (calBardHeal.TryFind("LostOasis", out ModItem lostOasis))
                        lostOasisID = lostOasis.Type;
                    CanUseItemEvent += ModifyStarBirthUseConditions;
                }
            }

            return base.IsLoadingEnabled(mod);
        }

        private bool ModifyDungeonCurseUseConditions(Item item, Player player)
        {
            if (item.type == CurseID)
            {
                if (Main.dayTime)
                {
                    return false;
                }

                return true;
            }
            return true;
        }

        private bool ModifyRenewUseConditions(Item item, Player player)
        {
            if (item.type == renewID)
            {
                var cdPlayer = player.GetModPlayer<HealerPlayer>();

                if (cdPlayer.renewCooldown > 0)
                {
                    return false;
                }
                else
                {
                    cdPlayer.renewCooldown = 60;
                    return true;
                }
            }
            return true;
        }

        private bool ModifyStarBirthUseConditions(Item item, Player player)
        {
            if (item.type == starBirthID || item.type == lostOasisID)
            {
                var cdPlayer = player.GetModPlayer<HealerPlayer>();

                if (cdPlayer.starBirthCooldown > 0)
                {
                    return false;
                }
                else
                {
                    cdPlayer.starBirthCooldown = 300;
                    return true;
                }
            }
            return true;
        }
    }
}
