using MonoMod.RuntimeDetour;
using ThoriumMod.Items;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace InfernalEclipseAPI.Core.Systems.ILItemChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.Thorium.Name)]
    [ExtendsFromMod(InfernalCrossmod.Thorium.Name)]
    public class TerrariansLastKnifeHealingCooldown : ModSystem
    {
        private static Hook lastKnifeOnHit = null;

        public override void OnModLoad()
        {
            if (!InfernalConfig.Instance.ThoriumBalanceChangess)
                return;

            Type terrariansLastKnife = InfernalCrossmod.Thorium.Mod.Code.GetType("ThoriumMod.Items.Donate.TerrariansLastKnife");
            MethodInfo orig = terrariansLastKnife.GetMethod("OnHitNPC", BindingFlags.Public | BindingFlags.Instance);
            lastKnifeOnHit = new Hook(orig, OnHitDetour);
        }

        public override void OnModUnload()
        {
            lastKnifeOnHit?.Dispose();
            lastKnifeOnHit = null;
        }

        private static void OnHitDetour(Action<ThoriumItem, Player, NPC, NPC.HitInfo, int> orig, ThoriumItem self, Player player, NPC target, NPC.HitInfo hitInfo, int damageDone)
        {
            int heal = (int)Math.Round(hitInfo.Damage * 0.04);
            if (heal > 100)
                heal = 100;

            if (player.altFunctionUse == 2 || !IsHostile(target) || heal <= 0 || player.moonLeech || player.lifeSteal <= 0f || target.lifeMax <= 5)
                return;

            player.lifeSteal -= heal;
            player.statLife += heal;
            player.HealEffect(heal);
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;

            for (int index1 = 0; index1 < 15; ++index1)
            {
                int index2 = Dust.NewDust(player.position, player.width, player.height, DustID.LifeDrain, 0.0f, 0.0f, 0, new Color(), 1f);
                Main.dust[index2].noGravity = true;
                Dust dust = Main.dust[index2];
                dust.velocity *= 0.75f;
                int num1 = Main.rand.Next(-55, 56);
                int num2 = Main.rand.Next(-55, 56);
                Main.dust[index2].position.X += (float)num1;
                Main.dust[index2].position.Y += (float)num2;
                Main.dust[index2].velocity.X = (float)(-(double)num1 * 0.075000002980232239);
                Main.dust[index2].velocity.Y = (float)(-(double)num2 * 0.075000002980232239);
            }
        }

        private static bool IsHostile(NPC npc, object attacker = null, bool ignoreDontTakeDamage = false)
        {
            return !npc.friendly && npc.lifeMax > 5 && npc.chaseable && !npc.dontTakeDamage | ignoreDontTakeDamage && !npc.immortal;
        }
    }
}
