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
        //private static int ShockerID;
        private static int DischargeID;
        //private static int SmasherID;
        private static int lsStaffID;
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

            /*
            if (InfernalConfig.Instance.PreventBossCheese)
            {
                ModLoader.TryGetMod("CalamityMod", out Mod cal);

                if (cal.TryFind("SubmarineShocker", out ModItem shocker))
                    ShockerID = shocker.Type;

                CanUseItemEvent += ModifySubmarineShockerUseConditions;


                if (cal.TryFind("CosmicDischarge", out ModItem discharge))
                    DischargeID = discharge.Type;

                CanUseItemEvent += ModifyCosmicDischargeUseConditions;


                if (cal.TryFind("GalaxySmasher", out ModItem smahser))
                    SmasherID = smahser.Type;

                CanUseItemEvent += ModifyGalaxySmasherUseConditions;


                if (ModLoader.TryGetMod("ThoriumMod", out Mod thor))
                {
                    if (thor.TryFind("LodeStoneStaff", out ModItem lsStaff))
                        lsStaffID = lsStaff.Type;

                    CanUseItemEvent += ModifyLodeStoneStaffUseConditions;
                }
            }
            */

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

        /*
        private bool ModifySubmarineShockerUseConditions(Item item, Player player)
        {

            if (item.type == ShockerID)
            {
                if (NPC.AnyNPCs(NPCID.TheDestroyer))
                {
                    Color draedon = new Color(155, 255, 255);
                    if (InfernalWorld.dreadonDestroyerDialoguePlayed == false)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Language.GetTextValue("Mods.InfernalEclipseAPI.BossCheesePrevention.TheDestroyer")), draedon);
                        InfernalWorld.dreadonDestroyerDialoguePlayed = true;
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaShootSound);
                    }
                    return false;
                }
                if (NPC.AnyNPCs(NPCID.Plantera))
                {
                    Color jungle = new Color(255, 240, 20);
                    if (InfernalWorld.jungleSubshockPlanteraDialoguePlayed == false)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Language.GetTextValue("Mods.InfernalEclipseAPI.BossCheesePrevention.Plantera")), jungle);
                        InfernalWorld.jungleSubshockPlanteraDialoguePlayed = true;
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaShootSound);
                    }
                    return false;
                }

                return true;
            }
            return true;
        }

        private bool ModifyLodeStoneStaffUseConditions(Item item, Player player)
        {
            if (item.type == lsStaffID)
            {
                if (NPC.AnyNPCs(NPCID.TheDestroyer))
                {
                    Color draedon = new Color(155, 255, 255);
                    if (InfernalWorld.dreadonDestroyer2DialoguePlayed == false)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Language.GetTextValue("Mods.InfernalEclipseAPI.BossCheesePrevention.TheDestroyer")), draedon);
                        InfernalWorld.dreadonDestroyer2DialoguePlayed = true;
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaShootSound);
                    }
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool ModifyCosmicDischargeUseConditions(Item item, Player player)
        {
            if (item.type == DischargeID)
            {
                if (NPC.AnyNPCs(ModContent.NPCType<Yharon>()))
                {
                    Color jungle = new Color(255, 240, 20);
                    if (InfernalWorld.yharonDischarge == false)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Language.GetTextValue("Mods.InfernalEclipseAPI.BossCheesePrevention.Yharon")), jungle);
                        InfernalWorld.yharonDischarge = true;
                        SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh);
                    }
                    return false;
                }
                return true;
            }
            return true;
        }

        private bool ModifyGalaxySmasherUseConditions(Item item, Player player)
        {
            if (item.type == SmasherID)
            {
                if (NPC.AnyNPCs(ModContent.NPCType<Yharon>()))
                {
                    Color jungle = new Color(255, 240, 20);
                    if (InfernalWorld.yharonSmasher == false)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(Language.GetTextValue("Mods.InfernalEclipseAPI.BossCheesePrevention.Yharon")), jungle);
                        InfernalWorld.yharonSmasher = true;
                        SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh);
                    }
                    return false;
                }
                return true;
            }
            return true;
        }
        */

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
