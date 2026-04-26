using System.Collections.Generic;
using System.Reflection;
using CalamityMod;
using CalamityMod.Enums;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Summon;
using InfernalEclipseAPI.Content.Items.Materials;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Core.Systems;
using InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.ThoriumItemHooks;
using InfernalEclipseAPI.Core.Utils;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using ThoriumMod;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossLich;
using ThoriumMod.Items.BossThePrimordials;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Dream;
using ThoriumMod.Items.BossThePrimordials.Omni;
using ThoriumMod.Items.BossThePrimordials.Slag;
using ThoriumMod.Items.Bronze;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.Coral;
using ThoriumMod.Items.Cultist;
using ThoriumMod.Items.DemonBlood;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Dread;
using ThoriumMod.Items.Flesh;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Icy;
using ThoriumMod.Items.Misc;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.Sandstone;
using ThoriumMod.Items.SummonItems;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.Thorium;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Valadium;
using ThoriumMod.Utilities;
using Consolaria.Content.Crossmod.Thorium.Armor;
using static Terraria.ModLoader.ModContent;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ModSpecific
{
    [JITWhenModsEnabled(InfernalCrossmod.Consolaria.Name)]
    [ExtendsFromMod(InfernalCrossmod.Thorium.Name)]
    public class ConsolariaGlobalItem : GlobalItem
    {
        public override void UpdateEquip(Item item, Player player)
        {
            if (InfernalConfig.Instance.ConsolariaBalanceChanges)
            {
                if (item.type == ItemType<ViperHelmet>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                }
                if (item.type == ItemType<ViperChestplate>())
                {
                    player.GetAttackSpeed(DamageClass.Throwing) -= 0.15f;
                }
                if (item.type == ItemType<ViperLegs>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                    player.GetCritChance(DamageClass.Throwing) -= 10;
                }

                if (item.type == ItemType<OldViperHelmet>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                }
                if (item.type == ItemType<OldViperChestplate>())
                {
                    player.GetAttackSpeed(DamageClass.Throwing) -= 0.15f;
                }
                if (item.type == ItemType<OldViperLegs>())
                {
                    player.GetDamage(DamageClass.Throwing) -= 0.10f;
                    player.GetCritChance(DamageClass.Throwing) -= 10;
                }
            }
        }
    }
}
