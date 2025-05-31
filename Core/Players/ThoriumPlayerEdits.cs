using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;
using Terraria.GameContent.Events;
using Terraria.ID;
using InfernalEclipseAPI.Content.Buffs;
using ThoriumMod.Items;

namespace InfernalEclipseAPI.Core.Players
{
    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumPlayerEdits : ModPlayer
    {
        public int radiantDamageCooldown;
        public int currentHealBonus;

        public override void ResetEffects()
        {
            if (radiantDamageCooldown > 0)
                radiantDamageCooldown--;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.LocalPlayer;
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();
            var ThorPlayer = player.GetModPlayer<ThoriumPlayer>();

            if (radiantDamageCooldown > 0)
            {
                Item heldItem = Player.ActiveItem();

                //Don't grant penalty when having all the Gems Active
                bool gemTechAllGem = CalPlayer.GemTechSet && CalPlayer.GemTechState.IsYellowGemActive &&
                                                             CalPlayer.GemTechState.IsGreenGemActive &&
                                                             CalPlayer.GemTechState.IsPurpleGemActive &&
                                                             CalPlayer.GemTechState.IsBlueGemActive &&
                                                             CalPlayer.GemTechState.IsRedGemActive &&
                                                             CalPlayer.GemTechState.IsPinkGemActive; // wow this is ugly

                bool crossClassNerfDisabled = gemTechAllGem || DD2Event.Ongoing; //dont do it during Old Ones Army because your expected to be cross-class with summoner

                if (!crossClassNerfDisabled)
                {
                    bool heldItemIsClassedWeapon = !heldItem.CountsAsClass<HealerDamage>() && (
                        heldItem.CountsAsClass<MeleeDamageClass>() ||
                        heldItem.CountsAsClass<RangedDamageClass>() ||
                        heldItem.CountsAsClass<MagicDamageClass>() ||
                        heldItem.CountsAsClass<SummonDamageClass>() ||
                        heldItem.CountsAsClass<RogueDamageClass>() ||
                        heldItem.CountsAsClass<ThrowingDamageClass>() ||
                        heldItem.CountsAsClass<BardDamage>()
                    //|| heldItem.CountsAsClass<TrueDamage>()
                    );

                    bool heldItemIsTool = heldItem.pick > 0 || heldItem.axe > 0 || heldItem.hammer > 0;
                    bool heldItemCanBeUsed = heldItem.useStyle != ItemUseStyleID.None;
                    bool heldItemIsAccessoryOrAmmo = heldItem.accessory || heldItem.ammo != AmmoID.None;

                    if (heldItemIsClassedWeapon && heldItemCanBeUsed && !heldItemIsTool && !heldItemIsAccessoryOrAmmo && radiantDamageCooldown > 0 && !player.HasBuff(ModContent.BuffType<BrokenOath>()))
                    {
                        player.AddBuff(ModContent.BuffType<BrokenOath>(), 900);
                    }
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[proj.owner];
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();
            var ThorPlayer = player.GetModPlayer<ThoriumPlayer>();

            currentHealBonus = ThorPlayer.healBonus;

            bool isRadiant = proj.CountsAsClass<HealerDamage>();
            if (!isRadiant && radiantDamageCooldown > 0)
            {
                Item heldItem = Player.ActiveItem();

                //Don't grant penalty when having all the Gems Active
                bool gemTechAllGem = CalPlayer.GemTechSet && CalPlayer.GemTechState.IsYellowGemActive && 
                                                             CalPlayer.GemTechState.IsGreenGemActive && 
                                                             CalPlayer.GemTechState.IsPurpleGemActive && 
                                                             CalPlayer.GemTechState.IsBlueGemActive &&
                                                             CalPlayer.GemTechState.IsRedGemActive &&
                                                             CalPlayer.GemTechState.IsPinkGemActive; // wow this is ugly

                bool crossClassNerfDisabled = gemTechAllGem || DD2Event.Ongoing; //dont do it during Old Ones Army because your expected to be cross-class with summoner

                if (!isRadiant && !crossClassNerfDisabled)
                {
                    bool heldItemIsClassedWeapon = !heldItem.CountsAsClass<HealerDamage>() && (
                        heldItem.CountsAsClass<MeleeDamageClass>() ||
                        heldItem.CountsAsClass<RangedDamageClass>() ||
                        heldItem.CountsAsClass<MagicDamageClass>() ||
                        heldItem.CountsAsClass<SummonDamageClass>() ||
                        heldItem.CountsAsClass<RogueDamageClass>() ||
                        heldItem.CountsAsClass<ThrowingDamageClass>() ||
                        heldItem.CountsAsClass<BardDamage>()
                        //|| heldItem.CountsAsClass<TrueDamage>()
                    );

                    bool heldItemIsTool = heldItem.pick > 0 || heldItem.axe > 0 || heldItem.hammer > 0;
                    bool heldItemCanBeUsed = heldItem.useStyle != ItemUseStyleID.None;
                    bool heldItemIsAccessoryOrAmmo = heldItem.accessory || heldItem.ammo != AmmoID.None;

                    if (heldItemIsClassedWeapon &&  heldItemCanBeUsed && !heldItemIsTool && !heldItemIsAccessoryOrAmmo && radiantDamageCooldown > 0 && !player.HasBuff(ModContent.BuffType<BrokenOath>())) 
                    {
                        player.AddBuff(ModContent.BuffType<BrokenOath>(), 900);
                    }
                }
            }
            else if (isRadiant)
            {
                if (player.HasBuff(ModContent.BuffType<BrokenOath>()))
                {
                    modifiers.FinalDamage *= 0.5f;
                    ThorPlayer.healBonus -= 10;
                }
                radiantDamageCooldown = 900;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (!InfernalConfig.Instance.AutomaticallyReforgeThoriumRogueItems)
                return;

            Player player = Main.LocalPlayer;
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();
            bool canStealthStike = CalPlayer.StealthStrikeAvailable();

            if (ModLoader.TryGetMod("CalamityBardHealer", out _) || ModLoader.TryGetMod("RagnarokMod", out _))
            {
                // Only for non-consumable Thorium thrower weapons
                if (item.ModItem is ThoriumItem thoriumItem && thoriumItem.isThrowerNon && !item.consumable)
                {
                    if (canStealthStike)
                        damage *= 1.15f; // This number centers on 1f, so 1.15f = 1.15x damage.
                }
                else if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.consumable)
                {
                    bool canStealthStrike = CalPlayer.StealthStrikeAvailable();
                    if (canStealthStrike)
                        damage *= 1.75f;
                }
            }
        }
    }
}
