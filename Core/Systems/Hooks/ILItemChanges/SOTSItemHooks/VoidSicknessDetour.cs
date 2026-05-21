using System.Reflection;
using InfernalEclipseAPI.Content.Buffs;
using InfernalEclipseAPI.Core.Players;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using SOTS.Items.Void;
using SOTS.Void;
using Terraria.Localization;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.SOTSItemHooks
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class VoidSicknessDetour : ModSystem
    {
        private static Hook onConsumeHook;
        private Hook sealedUpdateInventoryHook;

        public override void Load()
        {
            if (!InfernalCrossmod.SOTS.Loaded)
                return;

            MethodInfo m = typeof(VoidConsumable).GetMethod("OnConsumeItem", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (m is null)
                throw new MissingMethodException("SOTS.Items.Void.VoidConsumable.OnConsumeItem(Player) not found.");

            onConsumeHook = new Hook(m, OnConsumeItem_Detour);

            MethodInfo sealedUpdateInventoryMethod = typeof(VoidConsumable).GetMethod(
               nameof(VoidConsumable.SealedUpdateInventory),
               BindingFlags.Instance | BindingFlags.Public);

            if (sealedUpdateInventoryMethod is null)
                throw new Exception("Failed to find VoidConsumable.SealedUpdateInventory for detour.");

            sealedUpdateInventoryHook = new Hook(sealedUpdateInventoryMethod, HookSealedUpdateInventory);
        }

        public override void Unload()
        {
            onConsumeHook?.Dispose();
            onConsumeHook = null;

            sealedUpdateInventoryHook?.Dispose();
            sealedUpdateInventoryHook = null;

        }

        private static void OnConsumeItem_Detour(Action<VoidConsumable, Player> orig, VoidConsumable self, Player player)
        {
            orig(self, player);

            if (player?.active == true)
                player.AddBuff(ModContent.BuffType<VoidSickness2>(), 300);
        }

        private delegate void Orig_SealedUpdateInventory(VoidConsumable self, Player player);
        private static void HookSealedUpdateInventory(Orig_SealedUpdateInventory orig, VoidConsumable self, Player player)
        {
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            int num1 = 0;
            int num2 = voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls - voidPlayer.VoidMinionConsumption - self.GetVoidAmt();

            if (player.HasBuff(ModContent.BuffType<VoidSickness2>()))
            {
                int buffIndex = player.FindBuffIndex(ModContent.BuffType<VoidSickness2>());

                if (buffIndex != -1 && player.buffTime[buffIndex] >= 10 * 60)
                {
                    if (player.GetModPlayer<InfernalPlayer>().voidSicknessTextCooldown <= 0)
                    {
                        CombatText.NewText(player.Hitbox, Color.Lerp(Color.Red, Color.Magenta, 0.5f), Language.GetTextValue("Mods.InfernalEclipseAPI.UI.NoVoidConsumable"), true);
                        player.GetModPlayer<InfernalPlayer>().voidSicknessTextCooldown = 60 * 5;
                    }
                    return;
                }
            }

            while (player?.active == true && voidPlayer.voidMeter <= num1 && num2 >= 0 && self.Item.stack > 0 && self.CanUseItem(player))
            {
                self.Activate(player);
                player.AddBuff(ModContent.BuffType<VoidSickness2>(), 300);
            }
        }
    }
}
