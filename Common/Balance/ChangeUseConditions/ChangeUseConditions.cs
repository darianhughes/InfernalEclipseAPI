using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.NPCs.Abyss;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using InfernalEclipseAPI.Core.World;
using Terraria.Audio;
using Terraria.Chat;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.Yharon;
using ThoriumMod.Items.Lodestone;
using YouBoss.Content.Items.ItemReworks;

namespace InfernalEclipseAPI.Common.Balance.ChangeUseConditions
{
    public class ChangeUseConditions : GlobalItem
    {
        //public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        //{
        //    if (!ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) || !(InfernalConfig.Instance.CalamityBalanceChanges))
        //        return;

        //    int slagsplitterType = calamityMod.Find<ModItem>("SlagsplitterPauldron").Type;

        //    if (item.type == slagsplitterType && InfernalPlayer.BlockSlagsplitterEffects)
        //    {
        //        // Prevent effects: do nothing
        //        return;
        //    }
        //}

        private static int CurseID;
        private static int ShockerID;
        private static int DischargeID;
        private static int SmasherID;
        private static int lsStaffID;
        private static int fractalID;

        public delegate bool CanItemDoActionWithPlayerDelegate(Item item, Player player);
        public static event CanItemDoActionWithPlayerDelegate? CanUseItemEvent;
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

            ModLoader.TryGetMod("YouBoss", out Mod you);
            if (you.TryFind("FirstFractal", out ModItem firstFractal))
            {
                fractalID = firstFractal.Type;
            }

            CanUseItemEvent += ModifyFirstFractalUseConditions;

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

        private bool ModifySubmarineShockerUseConditions(Item item, Player player)
        {
            if (item.type == ShockerID)
            {
                if (NPC.AnyNPCs(NPCID.TheDestroyer))
                {
                    Color draedon = new Color(155, 255, 255);
                    if (InfernalWorld.dreadonDestroyerDialoguePlayed == false)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("I shall not let you destroy my machine so easily."), draedon);
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
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Ancient forces prevent you from using this item right now..."), jungle);
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
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("I shall not let you destroy my machine so easily."), draedon);
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
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Ancient forces prevent you from using this item right now..."), jungle);
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
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Ancient forces prevent you from using this item right now..."), jungle);
                        InfernalWorld.yharonSmasher = true;
                        SoundEngine.PlaySound(InfernumMode.Assets.Sounds.InfernumSoundRegistry.ModeToggleLaugh);
                    }
                    return false;
                }
                return true;
            }
            return true;
        }

        private bool ModifyFirstFractalUseConditions(Item item, Player player)
        {
            if (item.type == fractalID)
            {
                if (player.mount.Active)
                {
                    return false;
                }
                return true;
            }
            return true;
        }
    }
}
