using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    [ExtendsFromMod("RagnarokMod")]
    public class DisableRagnarokSigilNerf : ModSystem
    {
        private Hook _postUpdateHook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("RagnarokMod", out var rag) || rag?.Code == null)
                return;

            var t = rag.Code.GetType("RagnarokMod.Utils.RagnarokModPlayer");
            var mi = t?.GetMethod("PostUpdateMiscEffects",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (mi == null) return;

            _postUpdateHook = new Hook(mi, (PostUpdate_Rep)PostUpdate_Hook);
        }

        public override void Unload()
        {
            _postUpdateHook?.Dispose();
            _postUpdateHook = null;
        }

        private delegate void PostUpdate_Orig(object self);
        private delegate void PostUpdate_Rep(PostUpdate_Orig orig, object self);

        private static void PostUpdate_Hook(PostUpdate_Orig orig, object self)
        {
            // run the original first
            orig(self);

            // then undo just the Shinobi Sigil block Ragnarök flips:
            var mp = self as ModPlayer;
            if (mp == null) return;

            var fixField = self.GetType().GetField("accShinobiSigilFix",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // If Ragnarok set the "fix" flag, it also set Thorium's accShinobiSigil=false.
            // We flip it back on and clear the flag so downstream code doesn't trigger.
            if (fixField != null && fixField.GetValue(self) is bool wasSet && wasSet)
            {
                var tPlayer = mp.Player.GetThoriumPlayer();
                tPlayer.accShinobiSigil = true;      // restore the accessory toggle
                fixField.SetValue(self, false);      // pretend Ragnarok never set its fix
            }
        }
    }

    [ExtendsFromMod("RagnarokMod")]
    public sealed class NopShinobiSigilTooltipEdits : ModSystem
    {
        private ILHook _hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("RagnarokMod", out var rag) || rag?.Code == null)
                return;

            var t = rag.Code.GetType("RagnarokMod.Common.GlobalItems.ItemBalancer");
            var mi = t?.GetMethod("ModifyTooltips",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: new[] { typeof(Item), typeof(List<TooltipLine>) },
                modifiers: null);
            if (mi == null) return;

            _hook = new ILHook(mi, InjectEarlyReturnForSigil);
        }

        public override void Unload()
        {
            _hook?.Dispose();
            _hook = null;
        }

        private static void InjectEarlyReturnForSigil(ILContext il)
        {
            var c = new ILCursor(il);
            var continueLbl = il.DefineLabel();

            // if (IsShinobiSigil(item)) return;
            c.Emit(OpCodes.Ldarg_1); // Item item
            c.EmitDelegate<Func<Item, bool>>(IsShinobiSigil);
            c.Emit(OpCodes.Brfalse_S, continueLbl);
            c.Emit(OpCodes.Ret);
            c.MarkLabel(continueLbl);
        }

        private static bool IsShinobiSigil(Item item)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out var thorium) || thorium is null)
                return false;

            try
            {
                int sigilType = thorium.Find<ModItem>("ShinobiSigil").Type;
                return item.type == sigilType;
            }
            catch
            {
                return false;
            }
        }
    }
}
