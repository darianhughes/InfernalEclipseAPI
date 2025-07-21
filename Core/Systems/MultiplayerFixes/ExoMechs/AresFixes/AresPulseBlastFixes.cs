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
using Terraria.ModLoader.IO;
using static InfernumMode.Content.BehaviorOverrides.BossAIs.Draedon.ArtemisAndApollo.ApolloBehaviorOverride;

namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.ExoMechs.AresFixes
{
    //public class AresPulseBlastFixes : ModSystem
    //{
    //    private static Hook hook;
    //    private delegate void ReceiveExtraAIDelegate(AresSpinningDeathBeam self, BinaryReader reader);
    //    private static ReceiveExtraAIDelegate orig;

    //    public override void Load()
    //    {
    //        MethodInfo target = typeof(AresSpinningDeathBeam).GetMethod("ReceiveExtraAI", BindingFlags.Instance | BindingFlags.Public);

    //        // Manually bind original using CreateDelegate on unhooked method
    //        orig = (ReceiveExtraAIDelegate)Delegate.CreateDelegate(typeof(ReceiveExtraAIDelegate), target);

    //        // Hook it
    //        hook = new Hook(target, new ReceiveExtraAIDelegate(Hooked_ReceiveExtraAI));
    //    }

    //    public override void Unload()
    //    {
    //        hook?.Dispose();
    //        hook = null;
    //        orig = null;
    //    }

    //    private static void Hooked_ReceiveExtraAI(AresSpinningDeathBeam self, BinaryReader reader)
    //    {
    //        // Call original behavior
    //        orig(self, reader);

    //        // Injected logic
    //        self.Projectile.localAI[2] = self.Projectile.velocity.ToRotation();

    //        TwinsAttackType attackType = (TwinsAttackType)Main.npc[self.OwnerIndex].ai[0];
    //        if (attackType != TwinsAttackType.ArtemisLaserRay)
    //            self.Projectile.Kill();
    //    }
    //}

    //public class AresPulseBlastFixes : GlobalProjectile
    //{
    //    public override void AI(Projectile projectile)
    //    {
    //        if (projectile.type == ModContent.ProjectileType<AresPulseBlast>())
    //        {
    //            if (projectile.owner != Main.myPlayer)
    //            {
    //                Main.projectile[projectile.whoAmI].active = false;
    //                return;
    //            }

    //            base.AI(projectile);
    //        }
    //    }
    //}
}
