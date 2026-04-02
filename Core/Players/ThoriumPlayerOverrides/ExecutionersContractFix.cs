using System.Reflection;
using MonoMod.RuntimeDetour;
using ThoriumMod;

namespace InfernalEclipseAPI.Core.Players.ThoriumPlayerOverrides
{
    [JITWhenModsEnabled("ThoriumRework")]
    [ExtendsFromMod("ThoriumRework")]
    public class ExecutionersContractFix : ModPlayer
    {
        private static bool initialized = false;
        private static Type trPlayerType;
        private static FieldInfo aggregateField;
        private static FieldInfo contractField;

        private static void Init(Mod thoriumRework)
        {
            if (initialized) return;

            trPlayerType = thoriumRework.Code.GetType("ThoriumRework.ThoriumPlayer");

            if (trPlayerType != null)
            {
                aggregateField = trPlayerType.GetField("aggregate");
                contractField = trPlayerType.GetField("contract");
            }

            initialized = true;
        }

        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!projectile.friendly)
                return;

            if (!ModLoader.TryGetMod("ThoriumRework", out Mod thoriumRework))
                return;

            Init(thoriumRework);

            if (trPlayerType == null || aggregateField == null || contractField == null)
                return;

            // --- Get ThoriumRework player (reflection) ---
            ModPlayer trPlayer = GetThoriumReworkPlayer(Player);
            if (trPlayer == null)
                return;

            // --- Get Thorium player (clean way, like you already do) ---
            var thor = Player.GetModPlayer<ThoriumMod.ThoriumPlayer>();
            if (thor == null)
                return;

            try
            {
                bool contract = (bool)contractField.GetValue(trPlayer);
                if (!contract) return;

                int aggregate = (int)aggregateField.GetValue(trPlayer);
                int soul = thor.soulEssence; // CORRECT SOURCE

                // --- SAME CONDITIONS AS ORIGINAL ---
                if (aggregate <= 0)
                    return;

                if (!projectile.CountsAsClass(ThoriumDamageBase<HealerDamage>.Instance))
                    return;

                if (Player.heldProj == projectile.whoAmI)
                    return;

                // --- FIX ---
                // Runs AFTER Thorium logic
                if (soul > 4)
                {
                    int newAggregate = Math.Max(aggregate - 1, 0);

                    if (newAggregate < aggregate)
                    {
                        aggregateField.SetValue(trPlayer, newAggregate);
                    }
                }
            }
            catch
            {
                // silent fail like Thorium
            }
        }

        private ModPlayer GetThoriumReworkPlayer(Player player)
        {
            var modPlayersField = typeof(Player).GetField("modPlayers",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (modPlayersField == null)
                return null;

            var modPlayers = modPlayersField.GetValue(player) as ModPlayer[];

            if (modPlayers == null)
                return null;

            foreach (var mp in modPlayers)
            {
                if (mp.GetType() == trPlayerType)
                    return mp;
            }

            return null;
        }
    }

    [JITWhenModsEnabled("ThoriumRework")]
    [ExtendsFromMod("ThoriumRework")]
    public class ThoriumAggregateBlocker : ModSystem
    {
        private static Hook OnHitNPCWithProjHook;
        private static FieldInfo aggregateField;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumRework", out var thoriumRework)) return;

            var thoriumPlayerType = thoriumRework.Code.GetType("ThoriumRework.ThoriumPlayer");
            if (thoriumPlayerType == null) return;

            aggregateField = thoriumPlayerType.GetField("aggregate", BindingFlags.Public | BindingFlags.Instance);
            if (aggregateField == null) return;

            var method = thoriumPlayerType.GetMethod(
                "OnHitNPCWithProj",
                BindingFlags.Public | BindingFlags.Instance
            );
            if (method == null) return;

            // Detour Thorium's OnHitNPCWithProj
            OnHitNPCWithProjHook = new Hook(
                method,
                typeof(ThoriumAggregateBlocker).GetMethod(nameof(OnHitNPCWithProjDetour),
                    BindingFlags.Static | BindingFlags.NonPublic)
            );
        }

        private static void OnHitNPCWithProjDetour(Action<ModPlayer, Projectile, NPC, NPC.HitInfo, int> orig, ModPlayer self, Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (aggregateField == null)
            {
                orig(self, projectile, target, hit, damageDone);
                return;
            }

            // Read the original aggregate value
            int aggregateValue = (int)aggregateField.GetValue(self);

            // Call the original method
            orig(self, projectile, target, hit, damageDone);

            // Write back the aggregate **only if it decreased by 1** (skip that one decrement)
            int newValue = (int)aggregateField.GetValue(self);
            if (newValue == aggregateValue - 1)
            {
                aggregateField.SetValue(self, aggregateValue);
            }
        }

        public override void Unload()
        {
            OnHitNPCWithProjHook?.Dispose();
            OnHitNPCWithProjHook = null;
            aggregateField = null;
        }
    }
}
