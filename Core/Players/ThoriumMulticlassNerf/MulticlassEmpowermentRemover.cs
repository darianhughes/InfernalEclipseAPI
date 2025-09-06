using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using ThoriumMod.Items;
using ThoriumMod;
using System.Reflection;
using InfernalEclipseAPI.Core.DamageClasses.LegendaryClass;
using InfernalEclipseAPI.Core.DamageClasses.MythicClass;
using Terraria.GameContent.Events;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core.Players.ThoriumMulticlassNerf
{
    [ExtendsFromMod("ThoriumMod")]
    public class MulticlassEmpowermentRemover : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            if (item == null || item.IsAir)
                return base.UseItem(item, player);

            var mp = player.GetModPlayer<ThoriumMulticlassPlayerNerfs>();

            if (ThoriumHelpers.IsBardWeapon(item))
            {
                mp.MarkBardUse();
            }
            else if (ThoriumHelpers.IsNonBardCombatWeapon(item) && mp.InWindow)
            {
                // clear locally
                ThoriumHelpers.ClearAllEmpowerments(player);

                // and tell others
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket p = Mod.GetPacket();
                    p.Write((byte)InfernalEclipseMessageType.ThoriumEmpowerment);
                    p.Write((byte)ThoriumEmpowermentMsg.ClearEmpowerments);
                    p.Write((byte)player.whoAmI);
                    p.Send();
                }
            }

            return base.UseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var mp = player.GetModPlayer<ThoriumMulticlassPlayerNerfs>();

            if (ThoriumHelpers.IsBardWeapon(item))
                mp.MarkBardUse();
            else if (ThoriumHelpers.IsNonBardCombatWeapon(item) && mp.InWindow)
            {
                ThoriumHelpers.ClearAllEmpowerments(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket p = Mod.GetPacket();
                    p.Write((byte)InfernalEclipseMessageType.ThoriumEmpowerment);
                    p.Write((byte)ThoriumEmpowermentMsg.ClearEmpowerments);
                    p.Write((byte)player.whoAmI);
                    p.Send();
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }

    public enum ThoriumEmpowermentMsg : byte
    {
        ClearEmpowerments = 1
    }

    [ExtendsFromMod("ThoriumMod")]
    internal static class ThoriumHelpers
    {
        // Use the actual player passed in (don’t use Main.LocalPlayer here)
        private static bool GemTechAllGem(Player p)
        {
            var cal = p.GetModPlayer<CalamityPlayer>();
            return cal.GemTechSet &&
                   cal.GemTechState.IsYellowGemActive &&
                   cal.GemTechState.IsGreenGemActive &&
                   cal.GemTechState.IsPurpleGemActive &&
                   cal.GemTechState.IsBlueGemActive &&
                   cal.GemTechState.IsRedGemActive &&
                   cal.GemTechState.IsPinkGemActive;
        }

        // Keep your original IsWeapon for other systems if you want,
        // but for empowerment clearing we want a *basic* non-bard check:
        public static bool IsNonBardCombatWeapon(Item item)
            => item is not null &&
               !item.IsAir &&
               item.damage > 0 &&
               item.useStyle != ItemUseStyleID.None &&
               !item.accessory &&
               item.ammo == AmmoID.None &&
               item.pick <= 0 && item.axe <= 0 && item.hammer <= 0 &&
               !IsBardWeapon(item);

        public static bool IsBardWeapon(Item item)
        {
            if (item == null || item.IsAir)
                return false;

            if (item.ModItem is BardItem)
                return true;

            try { return item.CountsAsClass(ThoriumDamageBase<BardDamage>.Instance); }
            catch { return false; }
        }

        public static void ClearAllEmpowerments(Player player)
        {
            try
            {
                var thoriumPlayer = player.GetModPlayer<ThoriumMod.ThoriumPlayer>();
                if (thoriumPlayer == null) return;

                var empField = typeof(ThoriumMod.ThoriumPlayer)
                    .GetField("Empowerments", BindingFlags.Instance | BindingFlags.NonPublic);
                var data = empField?.GetValue(thoriumPlayer);
                if (data == null) return;

                var clearMI = typeof(ThoriumMod.Empowerments.EmpowermentLoader)
                    .GetMethod("UpdateDeadEmpowerments", BindingFlags.Static | BindingFlags.NonPublic);
                if (clearMI != null)
                    clearMI.Invoke(null, new object[] { data });

                // Nudge the cache right away (optional but helps UI/state feel instant)
                var updateMI = data.GetType().GetMethod("Update", BindingFlags.Instance | BindingFlags.Public);
                updateMI?.Invoke(data, null);
            }
            catch (Exception e)
            {
                if (Main.netMode != NetmodeID.Server)
                    Main.NewText($"[ThoriumCompat] Failed to clear empowerments: {e.Message}");
            }
        }
    }
}
