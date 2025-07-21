using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride;
using Terraria.ModLoader.IO;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ExoMechs.ArtemisFixes
{
    //public class ArtemisSpinLaserFixes : ModSystem
    //{
    //    private static Hook receiveExtraAIHook;
    //    private static Hook postAIHook;

    //    private static MethodInfo orig_ReceiveExtraAI;
    //    private static MethodInfo orig_PostAI;

    //    public override void Load()
    //    {
    //        Type type = typeof(ArtemisSpinLaser);

    //        // Hook ReceiveExtraAI
    //        orig_ReceiveExtraAI = type.GetMethod("ReceiveExtraAI", BindingFlags.Instance | BindingFlags.Public);
    //        receiveExtraAIHook = new Hook(orig_ReceiveExtraAI, new Action<object, BinaryReader>(ReceiveExtraAIHook));

    //        // Hook PostAI
    //        orig_PostAI = type.GetMethod("PostAI", BindingFlags.Instance | BindingFlags.Public);
    //        postAIHook = new Hook(orig_PostAI, new Action<object>(PostAIHook));
    //    }

    //    public override void Unload()
    //    {
    //        receiveExtraAIHook?.Dispose();
    //        postAIHook?.Dispose();

    //        receiveExtraAIHook = null;
    //        postAIHook = null;
    //        orig_ReceiveExtraAI = null;
    //        orig_PostAI = null;
    //    }

    //    private static void ReceiveExtraAIHook(object self, BinaryReader reader)
    //    {
    //        orig_ReceiveExtraAI.Invoke(self, new object[] { reader });

    //        ArtemisSpinLaser laser = (ArtemisSpinLaser)self;

    //        if (laser.Time <= 1)
    //            return;

    //        laser.Projectile.velocity = laser.Projectile.localAI[2].ToRotationVector2();
    //        laser.Projectile.rotation = laser.Projectile.localAI[2] - MathF.PI / 2f;
    //    }

    //    private static void PostAIHook(object self)
    //    {
    //        orig_PostAI.Invoke(self, null);

    //        ArtemisSpinLaser laser = (ArtemisSpinLaser)self;
    //        laser.Projectile.localAI[2] = laser.Projectile.velocity.ToRotation();

    //        TwinsAttackType attackType = (TwinsAttackType)Main.npc[laser.OwnerIndex].ai[0];
    //        if (attackType != TwinsAttackType.ArtemisLaserRay)
    //            laser.Projectile.Kill();
    //    }
    //}

    //public class ArtemisSpinLaserFixes : GlobalProjectile
    //{
    //    public override void PostAI(Projectile projectile)
    //    {
    //        if (projectile.type == ModContent.ProjectileType<ArtemisSpinLaser>())
    //        {
    //            base.PostAI(projectile);

    //            projectile.localAI[2] = projectile.velocity.ToRotation();

    //            TwinsAttackType attackType = (TwinsAttackType)Main.npc[projectile.owner].ai[0];
    //            if (attackType != TwinsAttackType.ArtemisLaserRay)
    //                projectile.Kill();
    //        }
    //    }

    //    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    //    {
    //    }

    //    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    //    {
    //        if (projectile.type == ModContent.ProjectileType<ArtemisSpinLaser>())
    //        {
    //            base.ReceiveExtraAI(projectile, bitReader, binaryReader);

    //            if (projectile.timeLeft <= 1)
    //                return;

    //            projectile.velocity = projectile.localAI[2].ToRotationVector2();
    //            projectile.rotation = projectile.localAI[2] - MathF.PI / 2f;
    //        }
    //    }
    //}
}
