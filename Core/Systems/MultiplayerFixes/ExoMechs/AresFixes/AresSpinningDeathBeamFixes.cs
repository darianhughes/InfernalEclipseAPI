using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.Ares;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride;
using MonoMod.Utils;
using Terraria.ModLoader.IO;
using ThoriumMod.Projectiles.Healer;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ExoMechs.AresFixes
{
    //public class AresSpinningDeathBeamFixes : ModSystem
    //{
    //    private static Hook receiveExtraAIHook;
    //    private static Hook postAIHook;

    //    private static MethodInfo receiveExtraAIOrig;
    //    private static MethodInfo postAIOrig;

    //    public override void Load()
    //    {
    //        Type type = typeof(AresSpinningDeathBeam);

    //        // Hook into ReceiveExtraAI
    //        receiveExtraAIOrig = type.GetMethod("ReceiveExtraAI", BindingFlags.Instance | BindingFlags.Public);
    //        receiveExtraAIHook = new Hook(receiveExtraAIOrig, new Action<AresSpinningDeathBeam, BinaryReader>(ReceiveExtraAIHook));

    //        // Hook into PostAI
    //        postAIOrig = type.GetMethod("PostAI", BindingFlags.Instance | BindingFlags.Public);
    //        postAIHook = new Hook(postAIOrig, new Action<AresSpinningDeathBeam>(PostAIHook));
    //    }

    //    public override void Unload()
    //    {
    //        receiveExtraAIHook?.Dispose();
    //        postAIHook?.Dispose();

    //        receiveExtraAIHook = null;
    //        postAIHook = null;

    //        receiveExtraAIOrig = null;
    //        postAIOrig = null;
    //    }

    //    private static void ReceiveExtraAIHook(AresSpinningDeathBeam self, BinaryReader reader)
    //    {
    //        // Call original
    //        receiveExtraAIOrig.Invoke(self, new object[] { reader });

    //        // Injected logic
    //        self.Projectile.localAI[2] = self.Projectile.velocity.ToRotation();

    //        TwinsAttackType attackType = (TwinsAttackType)Main.npc[self.OwnerIndex].ai[0];
    //        if (attackType != TwinsAttackType.ArtemisLaserRay)
    //            self.Projectile.Kill();
    //    }

    //    private static void PostAIHook(AresSpinningDeathBeam self)
    //    {
    //        // Call original
    //        postAIOrig.Invoke(self, null);

    //        // Injected logic
    //        self.Projectile.localAI[2] = self.Projectile.velocity.ToRotation();
    //    }
    //}

    //public class AresSpinningDeathBeamFixes : GlobalProjectile
    //{
    //    public override void PostAI(Projectile projectile)
    //    {
    //        if (projectile.type == ModContent.ProjectileType<AresSpinningDeathBeam>())
    //        {
    //            base.PostAI(projectile);

    //            projectile.localAI[2] = projectile.velocity.ToRotation();
    //        }
    //    }

    //    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    //    {

    //    }

    //    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    //    {
    //        if (projectile.type == ModContent.ProjectileType<AresSpinningDeathBeam>())
    //        {
    //            base.ReceiveExtraAI(projectile, bitReader, binaryReader);

    //            if (projectile.timeLeft <= 1) return;

    //            projectile.velocity = projectile.localAI[2].ToRotationVector2();
    //            projectile.rotation = projectile.localAI[2] - MathF.PI / 2;
    //        }
    //    }
    //}
}
