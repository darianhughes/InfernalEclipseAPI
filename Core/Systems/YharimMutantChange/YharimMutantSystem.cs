using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FargowiltasSouls.Content.Bosses.AbomBoss;
using FargowiltasSouls;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using FargowiltasSouls.Content.UI;
using FargowiltasSouls.Content.Bosses.MutantBoss;

namespace InfernalEclipseAPI.Core.Systems.YharimMutantChange
{
    [ExtendsFromMod("FargowiltasSouls")]
    public class YharimMutantSystem : ModSystem
    {
        private static ILHook _modifyAprilHook;
        private static ILHook _modifyAprilTextureHook;
        private static ILHook _modifyMutantTextureHook;
        private static ILHook _removeAbomAprilSetDefaultHook;
        private static ILHook _removeAbomP2AprilMusicHook;
        private static ILHook _removeAbomAprilTextureHook;

        public override void Load()
        {
            if (!InfernalConfig.Instance.UseAprilFoolsMutant)
                return;

            // Hook AprilFools check
            _modifyAprilHook = new ILHook(
                typeof(FargoSoulsUtil).GetMethod("get_AprilFools", BindingFlags.Static | BindingFlags.Public),
                ModifyAprilFools);
            _modifyAprilHook.Apply();

            // Hook AprilFools texture check
            _modifyAprilTextureHook = new ILHook(
                typeof(FargoSoulsUtil).GetMethod("get_TryAprilFoolsTexture", BindingFlags.Static | BindingFlags.Public),
                ModifyAprilFoolsTexture);
            _modifyAprilTextureHook.Apply();

            // Mutant boss texture hook
            _modifyMutantTextureHook = new ILHook(
                typeof(FargowiltasSouls.Content.Bosses.MutantBoss.MutantBoss).GetMethod("get_Texture", BindingFlags.Instance | BindingFlags.Public),
                ModifyMutantTexture);
            _modifyMutantTextureHook.Apply();

            // Abom boss SetDefaults hook
            _removeAbomAprilSetDefaultHook = new ILHook(
                typeof(FargowiltasSouls.Content.Bosses.AbomBoss.AbomBoss).GetMethod("SetDefaults", BindingFlags.Instance | BindingFlags.Public),
                RemoveAbomAprilSetDefault);
            _removeAbomAprilSetDefaultHook.Apply();

            // Abom boss PreAI (April Music) hook
            _removeAbomP2AprilMusicHook = new ILHook(
                typeof(FargowiltasSouls.Content.Bosses.AbomBoss.AbomBoss).GetMethod("PreAI", BindingFlags.Instance | BindingFlags.Public),
                RemoveAbomP2AprilMusic);
            _removeAbomP2AprilMusicHook.Apply();

            // Abom projectile texture hook
            _removeAbomAprilTextureHook = new ILHook(
                typeof(AbomBossProjectile).GetMethod("get_Texture", BindingFlags.Instance | BindingFlags.Public),
                RemoveAbomAprilTexture);
            _removeAbomAprilTextureHook.Apply();

            Mod.AddBossHeadTexture("FargowiltasSouls/Content/Bosses/MutantBoss/MutantBoss_April_Head_Boss", ModContent.NPCType<FargowiltasSouls.Content.Bosses.MutantBoss.MutantBoss>());
        }

        public override void Unload()
        {
            _modifyAprilHook?.Dispose();
            _modifyAprilTextureHook?.Dispose();
            _modifyMutantTextureHook?.Dispose();
            _removeAbomAprilSetDefaultHook?.Dispose();
            _removeAbomP2AprilMusicHook?.Dispose();
            _removeAbomAprilTextureHook?.Dispose();
        }

        // --- IL Manipulators ---

        private static void ModifyAprilFools(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => ILPatternMatchingExt.MatchCall(i, typeof(DateTime).GetProperty("Today").GetMethod)))
            {
                c.Emit(OpCodes.Ldc_I4_1);
                c.Emit(OpCodes.Ret);
            }
        }

        private static void ModifyAprilFoolsTexture(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => ILPatternMatchingExt.MatchCall(i, typeof(FargoSoulsUtil).GetProperty("AprilFools").GetMethod)))
            {
                c.Emit(OpCodes.Ldstr, "_April");
                c.Emit(OpCodes.Ret);
            }
        }

        private static void ModifyMutantTexture(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => ILPatternMatchingExt.MatchLdstr(i, "FargowiltasSouls/Content/Bosses/MutantBoss/MutantBoss")))
            {
                c.Emit(OpCodes.Ldstr, "FargowiltasSouls/Content/Bosses/MutantBoss/MutantBoss_April");
                c.Emit(OpCodes.Ret);
            }
        }

        private static void RemoveAbomAprilSetDefault(ILContext il)
        {
            var c = new ILCursor(il);
            ILLabel targetLabel = null;
            int found = 0;

            // Looks for the sixth brfalse and replaces with brtrue
            while (c.TryGotoNext(i => ILPatternMatchingExt.MatchBrfalse(i, targetLabel)))
            {
                found++;
                if (found == 6)
                {
                    c.Remove();
                    c.Emit(OpCodes.Brtrue, targetLabel);
                    break;
                }
            }
            if (found == 8)
            {
                c.Index--;
                c.RemoveRange(7);
            }
        }

        private static void RemoveAbomP2AprilMusic(ILContext il)
        {
            var c = new ILCursor(il);
            ILLabel targetLabel = null;
            if (c.TryGotoNext(i => ILPatternMatchingExt.MatchCall(i, typeof(FargoSoulsUtil).GetProperty("AprilFools").GetMethod)))
            {
                if (c.TryGotoNext(i => ILPatternMatchingExt.MatchBrfalse(i, targetLabel)))
                {
                    c.Remove();
                    c.Emit(OpCodes.Brtrue, targetLabel);
                }
            }
        }

        private static void RemoveAbomAprilTexture(ILContext il)
        {
            var c = new ILCursor(il);
            if (c.TryGotoNext(i => ILPatternMatchingExt.MatchLdstr(i, "FargowiltasSouls/Content/Bosses/AbomBoss/AbomBoss")))
            {
                c.Emit(OpCodes.Ldstr, "FargowiltasSouls/Content/Bosses/AbomBoss/AbomBoss");
                c.Emit(OpCodes.Ret);
            }
        }
    }
}
