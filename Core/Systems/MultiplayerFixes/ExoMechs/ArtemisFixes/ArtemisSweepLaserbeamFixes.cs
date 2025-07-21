using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ExoMechs.ArtemisFixes
{
    //public class ArtemisSweepLaserbeamFixes : ModSystem
    //{
    //    private static Hook postAIHook;
    //    private static MethodInfo orig_PostAI;

    //    public override void Load()
    //    {
    //        Type type = typeof(ArtemisSweepLaserbeam);
    //        orig_PostAI = type.GetMethod("PostAI", BindingFlags.Instance | BindingFlags.Public);
    //        postAIHook = new Hook(orig_PostAI, new Action<object>(PostAIHook));
    //    }

    //    public override void Unload()
    //    {
    //        postAIHook?.Dispose();
    //        postAIHook = null;
    //        orig_PostAI = null;
    //    }

    //    private static void PostAIHook(object self)
    //    {
    //        // Call original method
    //        orig_PostAI.Invoke(self, null);

    //        // Cast and inject logic
    //        ArtemisSweepLaserbeam laser = (ArtemisSweepLaserbeam)self;

    //        TwinsAttackType attackType = (TwinsAttackType)Main.npc[laser.OwnerIndex].ai[0];
    //        if (attackType != TwinsAttackType.SlowLaserRayAndPlasmaBlasts)
    //        {
    //            laser.Projectile.Kill();
    //        }
    //    }
    //}

    //public class ArtemisSweepLaserbeamFixes : GlobalProjectile
    //{
    //    public override void PostAI(Projectile projectile)
    //    {
    //        if (projectile.type == ModContent.ProjectileType<ArtemisSweepLaserbeam>())
    //        {
    //            base.PostAI(projectile);

    //            TwinsAttackType attackType = (TwinsAttackType)Main.npc[projectile.owner].ai[0];
    //            if (attackType != TwinsAttackType.SlowLaserRayAndPlasmaBlasts)
    //            {
    //                projectile.Kill();
    //            }
    //        }
    //    }
    //}
}
