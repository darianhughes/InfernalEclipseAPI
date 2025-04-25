using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.Balance.Infernum
{
    public class DungeonCurseFix : GlobalItem
    {
        private static int CurseID;

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
    }
}
