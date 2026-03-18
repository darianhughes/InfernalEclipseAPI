using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.ThoriumItemHooks
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class GreatFleshNerfSystem : ModSystem
    {
        private Hook onHitProjHook;
        private Hook onHitItemHook;

        internal static int GreatFleshType;

        private bool bossWasAliveLastTick;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            GreatFleshType = ModContent.ItemType<ThoriumMod.Items.Flesh.GreatFlesh>();

            Type thoriumPlayerType = thorium.Code.GetType("ThoriumMod.ThoriumPlayer");
            if (thoriumPlayerType is null)
                return;

            MethodInfo onHitProj = thoriumPlayerType.GetMethod(
                "OnHitNPCWithProj",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo onHitItem = thoriumPlayerType.GetMethod(
                "OnHitNPCWithItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (onHitProj is not null)
                onHitProjHook = new Hook(onHitProj, OnHitNPCWithProj_Hook);

            if (onHitItem is not null)
                onHitItemHook = new Hook(onHitItem, OnHitNPCWithItem_Hook);

            bossWasAliveLastTick = false;
        }

        public override void Unload()
        {
            onHitProjHook?.Dispose();
            onHitItemHook?.Dispose();

            onHitProjHook = null;
            onHitItemHook = null;
        }

        public override void PostUpdateWorld()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            bool bossAliveNow = AnyBossAlive();

            // Delete flesh only on the transition from "no boss" -> "boss exists".
            if (bossAliveNow && !bossWasAliveLastTick)
            {
                for (int i = 0; i < Main.maxItems; i++)
                {
                    Item item = Main.item[i];
                    if (item != null && item.active && item.type == GreatFleshType)
                        DeleteItem(i);
                }
            }

            bossWasAliveLastTick = bossAliveNow;
        }

        private delegate void OnHitNPCWithProjDelegate(object self, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone);
        private delegate void OnHitNPCWithItemDelegate(object self, Item item, NPC target, NPC.HitInfo hit, int damageDone);

        private static void OnHitNPCWithProj_Hook(OnHitNPCWithProjDelegate orig, object self, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = GetThoriumPlayerOwner(self);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                orig(self, proj, target, hit, damageDone);
                return;
            }

            HandleGreatFleshSpawnControl(player, () => orig(self, proj, target, hit, damageDone));
        }

        private static void OnHitNPCWithItem_Hook(OnHitNPCWithItemDelegate orig, object self, Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = GetThoriumPlayerOwner(self);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                orig(self, item, target, hit, damageDone);
                return;
            }

            HandleGreatFleshSpawnControl(player, () => orig(self, item, target, hit, damageDone));
        }

        private static void HandleGreatFleshSpawnControl(Player player, Action runOrig)
        {
            if (player == null || !player.active)
            {
                runOrig();
                return;
            }

            GreatFleshControlPlayer modPlayer = player.GetModPlayer<GreatFleshControlPlayer>();

            HashSet<int> before = GetActiveGreatFleshIndices();
            int countBefore = before.Count;
            int maxAllowed = GetGreatFleshCap();

            runOrig();

            HashSet<int> after = GetActiveGreatFleshIndices();

            List<int> newIndices = new();
            foreach (int index in after)
            {
                if (!before.Contains(index))
                    newIndices.Add(index);
            }

            if (newIndices.Count == 0)
                return;

            bool onCooldown = modPlayer.GreatFleshSpawnCooldown > 0;
            bool overCap = countBefore >= maxAllowed;

            if (onCooldown || overCap)
            {
                foreach (int index in newIndices)
                    DeleteItem(index);

                return;
            }

            int allowed = maxAllowed - countBefore;

            for (int i = allowed; i < newIndices.Count; i++)
                DeleteItem(newIndices[i]);

            if (allowed > 0)
                modPlayer.GreatFleshSpawnCooldown = 60;
        }

        private static int GetGreatFleshCap()
        {
            int activePlayers = 0;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player != null && player.active)
                    activePlayers++;
            }

            return Math.Max(5, activePlayers * 5);
        }

        private static Player GetThoriumPlayerOwner(object thoriumPlayerInstance)
        {
            if (thoriumPlayerInstance is null)
                return null;

            Type t = thoriumPlayerInstance.GetType();

            FieldInfo playerField = t.GetField("Player", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (playerField?.GetValue(thoriumPlayerInstance) is Player p1)
                return p1;

            PropertyInfo playerProp = t.GetProperty("Player", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (playerProp?.GetValue(thoriumPlayerInstance) is Player p2)
                return p2;

            return null;
        }

        private static HashSet<int> GetActiveGreatFleshIndices()
        {
            HashSet<int> indices = new();

            for (int i = 0; i < Main.maxItems; i++)
            {
                Item item = Main.item[i];
                if (item != null && item.active && item.type == GreatFleshType)
                    indices.Add(i);
            }

            return indices;
        }

        private static void DeleteItem(int index)
        {
            if (index < 0 || index >= Main.maxItems)
                return;

            Item item = Main.item[index];
            if (item == null || !item.active)
                return;

            item.TurnToAir();
            item.active = false;

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index);
        }

        private static bool AnyBossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc != null && npc.active && npc.boss)
                    return true;
            }

            return false;
        }
    }

    public sealed class GreatFleshControlPlayer : ModPlayer
    {
        public int GreatFleshSpawnCooldown;

        public override void PreUpdate()
        {
            if (GreatFleshSpawnCooldown > 0)
                GreatFleshSpawnCooldown--;
        }
    }
}
