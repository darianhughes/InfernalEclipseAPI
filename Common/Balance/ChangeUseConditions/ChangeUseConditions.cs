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

namespace InfernalEclipseAPI.Common.Balance.ChangeUseConditions
{
    public class ChangeUseConditions : GlobalItem
    {
        private static int CurseID;
        private static int ShockerID;

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
            ModLoader.TryGetMod("CalamityMod", out Mod cal);
            if (cal.TryFind("SubmarineShocker", out ModItem shocker))
                ShockerID = shocker.Type;

            CanUseItemEvent += ModifyDungeonCurseUseConditions;
            CanUseItemEvent += ModifySubmarineShockerUseConditions;

            return base.IsLoadingEnabled(mod);
        }

        private bool ModifyDungeonCurseUseConditions(Item item, Player player)
        {
            if (item.type == CurseID)
            {
                if (Main.dayTime && !NPC.downedBoss3)
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
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("I shall not let you destroy my machine so easily."), draedon);
                        InfernalWorld.dreadonDestroyerDialoguePlayed = true;
                        SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoPlasmaShootSound);
                    }
                    return false;
                }

                return true;
            }
            return true;
        }
    }
}
