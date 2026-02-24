using MonoMod.RuntimeDetour;
using System.Reflection;
using SOTS;
using SOTS.Void;
using InfernalEclipseAPI.Core.Players;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges
{
    [JITWhenModsEnabled(InfernalCrossmod.SOTS.Name)]
    [ExtendsFromMod(InfernalCrossmod.SOTS.Name)]
    public class ElementalAmuletNerfs : ModSystem
    {
        private Hook getBonusesHook;

        public delegate void VisionAmuletEffect(Player player, int gem, int frame);

        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.HasMod("SecretsOfTheSouls");
        }

        public override void Load()
        {
            var type = InfernalCrossmod.SOTS.Mod.Code.GetType("SOTS.Items.AbandonedVillage.VisionAmulet");
            var mi = type?.GetMethod("GetBonuses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (mi == null) return;

            getBonusesHook = new Hook(mi, new GetBonuses_Replacement(GetBonuses_Hook));
        }

        public override void Unload()
        {
            getBonusesHook?.Dispose();
            getBonusesHook = null;
        }

        private delegate void GetBonuses_Replacement(object self, Player player, int gem, int frame);

        private static void GetBonuses_Hook(object self, Player player, int gem, int frame)
        {
            SOTSPlayer sotsPlayer = SOTSPlayer.ModPlayer(player);
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            InfernalPlayer infernalPlayer = player.GetModPlayer<InfernalPlayer>();

            switch (gem)
            {
                case 0:
                    player.endurance += 0.1f;
                    break;
                case 1:
                    ++player.maxMinions;
                    ++player.maxTurrets;
                    break;
                case 2:
                    sotsPlayer.attackSpeedMod += 0.12f;
                    break;
                case 3:
                    player.GetCritChance(DamageClass.Generic) += 10f;
                    break;
                case 4:
                    sotsPlayer.CritBonusMultiplier += Main.hardMode ? 0.1f : 0.05f;
                    break;
                case 5:
                    sotsPlayer.additionalHeal += 40;
                    player.lifeRegen += 2;
                    break;
                case 6:
                    player.statLifeMax2 += 20;
                    player.GetDamage(DamageClass.Generic) += Main.hardMode ? 0.1f : 0.05f;
                    break;
                case 7:
                    voidPlayer.voidRegenSpeed += 0.2f;
                    break;
            }

            switch (frame)
            {
                case 0:
                    player.discountAvailable = true;
                    break;
                case 1:
                    player.manaCost -= 0.2f;
                    break;
                case 2:
                    player.jumpSpeedBoost += 2f;
                    player.moveSpeed += 0.1f;
                    player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
                    break;
                case 3:
                    if (Main.hardMode)
                        sotsPlayer.LazyCrafterAmulet = true;
                    else
                        infernalPlayer.LazyCrafterAmulet = true;
                    sotsPlayer.additionalPotionMana += 40;
                    player.statManaMax2 += 40;
                    break;
                case 4:
                    infernalPlayer.statShareAll = true;
                    break;
                case 5:
                    infernalPlayer.scalingArmorPenetration = true;
                    break;
                case 6:
                    voidPlayer.voidGainMultiplier += 0.2f;
                    player.GetDamage<VoidGeneric>() += 0.1f;
                    break;
            }
        }
    }
}
