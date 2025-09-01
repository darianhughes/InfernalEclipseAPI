using System;
using System.Collections.Generic;
using System.Reflection;
using CatalystMod.Items.SummonItems;
using CatalystMod.NPCs;
using CatalystMod.UI;
using CatalystMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    [ExtendsFromMod("CatalystMod")]
    public class AstralCommunicatorAnytime : ModSystem
    {
        // Target methods (cached via reflection)
        private static readonly MethodBase _communicatorCanUseItem =
            typeof(AstralCommunicator).GetMethod(nameof(AstralCommunicator.CanUseItem));

        private static readonly MethodBase _communicatorTooltip =
            typeof(AstralCommunicator).GetMethod(nameof(AstralCommunicator.ModifyTooltips));

        private static readonly MethodBase _communicatorPreDrawInventory =
            typeof(AstralCommunicator).GetMethod(nameof(AstralCommunicator.PreDrawInInventory));

        private static readonly MethodBase _communicatorPreDrawWorld =
            typeof(AstralCommunicator).GetMethod(nameof(AstralCommunicator.PreDrawInWorld));

        private static readonly MethodBase _playerUpdateEquips =
            typeof(CatalystPlayer).GetMethod(nameof(CatalystPlayer.PostUpdateEquips));

        private static readonly MethodBase _playerLoadData =
            typeof(CatalystPlayer).GetMethod(nameof(CatalystPlayer.LoadData));

        private static readonly MethodBase _npcPreKill =
            typeof(CatalystNPC).GetMethod(nameof(CatalystNPC.PreKill));

        private static readonly PropertyInfo _summonAstrageldonProp =
            typeof(CatalystPlayer).GetProperty("SummonAstrageldon",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly MethodInfo _summonAstrageldonSetter =
            _summonAstrageldonProp?.GetSetMethod(true);

        public override void Load()
        {
            // Hooks (MonoMod.RuntimeDetour.HookGen style)
            MonoModHooks.Add(
                _communicatorCanUseItem,
                (Func<orig_CommunicatorCanUseItem, AstralCommunicator, Player, bool>)CanUseCheck
            );

            MonoModHooks.Add(
                _communicatorTooltip,
                (Action<orig_CommunicatorTooltip, AstralCommunicator, List<TooltipLine>>)TooltipCheck
            );

            MonoModHooks.Add(
                _communicatorPreDrawInventory,
                (Func<orig_CommunicatorPreDrawInventory, AstralCommunicator, SpriteBatch, Vector2, Rectangle, Color, Color, Vector2, float, bool>)InventoryDraw
            );

            MonoModHooks.Add(
                _communicatorPreDrawWorld,
                (hook_CommunicatorPreDrawWorld)WorldDraw
            );

            MonoModHooks.Add(
                _playerUpdateEquips,
                (Action<orig_PlayerEquips, CatalystPlayer>)EquipsCheck
            );

            MonoModHooks.Add(
                _playerLoadData,
                (Action<orig_PlayerLoadData, CatalystPlayer, TagCompound>)ForceData
            );

            MonoModHooks.Add(
                _npcPreKill,
                (Func<orig_NPCPreKill, CatalystNPC, NPC, bool>)PreKillCheck
            );
        }

        // --- Hook implementations ---

        // Allow use even if Moon Lord isn't downed (temporarily spoof the flag to false).
        private static bool CanUseCheck(orig_CommunicatorCanUseItem orig, AstralCommunicator self, Player player)
        {
            bool prev = NPC.downedMoonlord;
            NPC.downedMoonlord = false;
            try
            {
                return orig(self, player);
            }
            finally
            {
                NPC.downedMoonlord = prev;
            }
        }

        // Force tooltip path that assumes Astrageldon is downed (temporarily spoof to true).
        private static void TooltipCheck(orig_CommunicatorTooltip orig, AstralCommunicator self, List<TooltipLine> tooltips)
        {
            bool prev = WorldDefeats.downedAstrageldon;
            WorldDefeats.downedAstrageldon = true;
            try
            {
                orig(self, tooltips);
            }
            finally
            {
                WorldDefeats.downedAstrageldon = prev;
            }
        }

        // Inventory draw should behave as if Moon Lord is not downed.
        private static bool InventoryDraw(
            orig_CommunicatorPreDrawInventory orig,
            AstralCommunicator self,
            SpriteBatch spriteBatch,
            Vector2 position,
            Rectangle frame,
            Color drawColor,
            Color itemColor,
            Vector2 origin,
            float scale)
        {
            bool prev = NPC.downedMoonlord;
            NPC.downedMoonlord = false;
            try
            {
                return orig(self, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            }
            finally
            {
                NPC.downedMoonlord = prev;
            }
        }

        // World draw should behave as if Moon Lord is not downed.
        private static bool WorldDraw(
            orig_CommunicatorPreDrawWorld orig,
            AstralCommunicator self,
            SpriteBatch spriteBatch,
            Color lightColor,
            Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI)
        {
            bool prev = NPC.downedMoonlord;
            NPC.downedMoonlord = false;
            try
            {
                return orig(self, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            }
            finally
            {
                NPC.downedMoonlord = prev;
            }
        }

        // Player equips update should ignore Moon Lord downed gate.
        private static void EquipsCheck(orig_PlayerEquips orig, CatalystPlayer self)
        {
            bool prev = NPC.downedMoonlord;
            NPC.downedMoonlord = false;
            try
            {
                orig(self);
            }
            finally
            {
                NPC.downedMoonlord = prev;
            }
        }

        // When loading data, force SummonAstrageldon = true via the (possibly non-public) setter.
        private static void ForceData(orig_PlayerLoadData orig, CatalystPlayer self, TagCompound tag)
        {
            orig(self, tag);
            _summonAstrageldonSetter?.Invoke(self, new object[] { true });
        }

        // PreKill should behave as if Moon Lord IS downed (to allow drops/logic gated behind it).
        private static bool PreKillCheck(orig_NPCPreKill orig, CatalystNPC self, NPC npc)
        {
            bool prev = NPC.downedMoonlord;
            NPC.downedMoonlord = true;
            try
            {
                return orig(self, npc);
            }
            finally
            {
                NPC.downedMoonlord = prev;
            }
        }

        // --- Orig delegate signatures expected by HookGen ---

        private delegate bool orig_CommunicatorCanUseItem(AstralCommunicator self, Player player);

        private delegate void orig_CommunicatorTooltip(AstralCommunicator self, List<TooltipLine> tooltips);

        private delegate bool orig_CommunicatorPreDrawInventory(
            AstralCommunicator self,
            SpriteBatch spriteBatch,
            Vector2 position,
            Rectangle frame,
            Color drawColor,
            Color itemColor,
            Vector2 origin,
            float scale);

        private delegate bool orig_CommunicatorPreDrawWorld(
            AstralCommunicator self,
            SpriteBatch spriteBatch,
            Color lightColor,
            Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI);

        private delegate bool hook_CommunicatorPreDrawWorld(
            orig_CommunicatorPreDrawWorld orig,
            AstralCommunicator self,
            SpriteBatch spriteBatch,
            Color lightColor,
            Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI);

        private delegate void orig_PlayerEquips(CatalystPlayer self);

        private delegate void orig_PlayerLoadData(CatalystPlayer self, TagCompound tag);

        private delegate bool orig_NPCPreKill(CatalystNPC self, NPC npc);
    }
}