using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.CalPlayer;

namespace InfernalEclipseAPI.Content.RogueThrower
{
    //Exhaustion removal code by Wardrobe Hummus
    //White Dwarf set cooldown by Akira
    public class RogueThrowerPlayer : ModPlayer
    {
        private int volume2Type = -1;
        private int volume3Type = -1;
        private int volume4Type = -1;
        private int soul1Type = -1;
        private int soul2Type = -1;
        private int soul3Type = -1;
        private int wing1Type = -1;
        private bool initialized = false;

        private int? previousHeldItemType;
        private int? previousHeldItemPrefix = null;
        private bool? previousHeldItemOriginalExhaustion;

        public int whiteDwarfCooldown;
        public int ShinobiSigilCooldown;
        public override void ResetEffects()
        {
            if (whiteDwarfCooldown > 0)
                whiteDwarfCooldown--;
            if (ShinobiSigilCooldown > 0)
                ShinobiSigilCooldown--;
        }

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            if (ModContent.TryFind("ThoriumMod/ThrowingGuideVolume2", out ModItem modItem1))
                volume2Type = modItem1.Type;

            if (ModContent.TryFind("ThoriumMod/ThrowingGuideVolume3", out ModItem modItem2))
                volume3Type = modItem2.Type;

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

            bool accessoryEquipped = HasExhaustionClearingAccessoryEquipped();

            Item heldItem = Player.HeldItem;

            bool heldItemChanged =
                previousHeldItemType == null ||
                previousHeldItemPrefix == null ||
                heldItem.type != previousHeldItemType ||
                heldItem.prefix != previousHeldItemPrefix;

            if (heldItemChanged)
            {
                RestoreExhaustion();

                previousHeldItemType = heldItem.type;
                previousHeldItemPrefix = heldItem.prefix;
                previousHeldItemOriginalExhaustion = null;
            }

            if (accessoryEquipped)
            {
                ApplyExhaustionDisabling();
            }
            else
            {
                RestoreExhaustion();
            }
        }

        private void ApplyExhaustionDisabling()
        {
            Item heldItem = Player.HeldItem;
            if (heldItem.IsAir || heldItem.ModItem == null)
                return;

            ModItem modItem = heldItem.ModItem;

            if (modItem.Mod?.Name == "ThoriumMod")
            {
                var field = modItem.GetType().GetField("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    bool currentValue = (bool)field.GetValue(modItem);

                    if (previousHeldItemOriginalExhaustion == null)
                    {
                        previousHeldItemOriginalExhaustion = currentValue;
                    }

                    if (currentValue)
                    {
                        field.SetValue(modItem, false); // Disable exhaustion
                    }
                }
            }
        }

        private void RestoreExhaustion()
        {
            if (previousHeldItemOriginalExhaustion == null)
                return;

            for (int i = 0; i < Player.inventory.Length; i++)
            {
                Item item = Player.inventory[i];
                if (!item.IsAir &&
                    item.type == previousHeldItemType &&
                    item.prefix == previousHeldItemPrefix &&
                    item.ModItem != null &&
                    item.ModItem.Mod?.Name == "ThoriumMod")
                {
                    var field = item.ModItem.GetType().GetField("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (field != null)
                    {
                        field.SetValue(item.ModItem, previousHeldItemOriginalExhaustion);
                    }

                    break;
                }
            }

            previousHeldItemOriginalExhaustion = null;
        }

        public override void PostUpdateEquips()
        {
            Mod thorium; 
            ModLoader.TryGetMod("ThoriumMod", out thorium);
            if (thorium == null) return;

            int whiteDwarfHelm = thorium.Find<ModItem>("WhiteDwarfMask").Type;
            int whiteDwarfPlate = thorium.Find<ModItem>("WhiteDwarfGuard").Type;
            int whiteDwarfGreaves = thorium.Find<ModItem>("WhiteDwarfGreaves").Type;

            if (Player.armor[0].type == whiteDwarfHelm &&
                Player.armor[1].type == whiteDwarfPlate &&
                Player.armor[2].type == whiteDwarfGreaves)
            {
                Player.setBonus += "\nIvory flares spawn on a 2 second cooldown";
                // Add effects here if needed
            }
        }

        public bool HasExhaustionClearingAccessoryEquipped()
        {
            EnsureInitialized();

            // Accessory slots start at index 3. We stop before vanity slots.
            int start = 3;
            int end = Player.armor.Length - 10; // Excludes vanity and dye slots

            for (int i = start; i < end; i++)
            {
                if (i >= Player.armor.Length)
                    continue;

                Item accessory = Player.armor[i];

                // Skip empty slots
                if (accessory.IsAir)
                    continue;

                // Match any of the exhaustion-clearing accessory types
                if (accessory.type == volume2Type || accessory.type == volume3Type || accessory.type == volume4Type ||
                    accessory.type == soul1Type || accessory.type == soul2Type || accessory.type == soul3Type ||
                    accessory.type == wing1Type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
