using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace InfernalEclipseAPI.Core.Players
{
    //Provided by Wardrobe Hummus
    public class RogueThrowerPlayer : ModPlayer
    {
        private int volume2Type = -1;
        private int volume3Type = -1;
        private int volume4Type = -1;
        private int soul1Type = -1;
        private int soul2Type = -1;
        private int soul3Type = -1;
        private int wing1Type = -1;

        private bool initialized;
        private Item previousHeldItem;
        private bool? previousHeldItemOriginalExhaustion;

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            if (ModContent.TryFind("ThoriumMod/ThrowingGuideVolume2", out ModItem modItem1))
                volume2Type = modItem1.Type;

            if (ModContent.TryFind("ThoriumMod/ThrowingGuideVolume3", out ModItem modItem2))
                volume3Type = modItem2.Type;

            if (ModContent.TryFind("ssm/GtTETFinal", out ModItem modItem3))
                volume4Type = modItem3.Type;

            if (ModContent.TryFind("FargowiltasCrossmod/VagabondsSoul", out ModItem modItem4))
                soul1Type = modItem4.Type;

            if (ModContent.TryFind("FargowiltasSouls/UniverseSoul", out ModItem modItem5))
                soul2Type = modItem5.Type;

            if (ModContent.TryFind("FargowiltasSouls/EternitySoul", out ModItem modItem6))
                soul3Type = modItem6.Type;

            if (ModContent.TryFind("ThoriumMod/WhiteDwarfThrusters", out ModItem modItem7))
                wing1Type = modItem7.Type;

            initialized = true;
        }

        public override void UpdateEquips()
        {
            EnsureInitialized();
            bool shouldBypass = false;

            for (int i = 3; i < Player.armor.Length; i++)
            {
                Item item = Player.armor[i];
                if (!item.IsAir &&
                    (item.type == volume2Type || item.type == volume3Type || item.type == volume4Type ||
                     item.type == soul1Type || item.type == soul2Type || item.type == soul3Type || item.type == wing1Type) &&
                    !Player.hideVisibleAccessory[i])
                {
                    shouldBypass = true;
                    break;
                }
            }

            if (Player.HeldItem != previousHeldItem)
            {
                RestoreExhaustion(previousHeldItem);
                previousHeldItem = Player.HeldItem;
                previousHeldItemOriginalExhaustion = null;
            }

            if (shouldBypass)
            {
                ApplyExhaustionDisabling();
            }
        }

        private void ApplyExhaustionDisabling()
        {
            Item heldItem = Player.HeldItem;
            if (heldItem.IsAir || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;

            ModItem modItem = heldItem.ModItem;
            if (modItem?.Mod?.Name != "ThoriumMod")
                return;

            FieldInfo field = modItem.GetType().GetField("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null || !(field.GetValue(modItem) is bool currentValue) || !currentValue)
                return;

            previousHeldItemOriginalExhaustion = true;
            field.SetValue(modItem, false);
        }

        private void RestoreExhaustion(Item item)
        {
            if (item == null || item.IsAir || previousHeldItemOriginalExhaustion != true || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;

            ModItem modItem = item.ModItem;
            if (modItem?.Mod?.Name != "ThoriumMod")
                return;

            FieldInfo field = modItem.GetType().GetField("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(modItem, true);
            }
        }
    }
}
