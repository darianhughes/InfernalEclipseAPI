using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Thorium;
using CalamityMod.Items.Potions;
using Terraria.ID;
using ThoriumMod.Projectiles;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    [ExtendsFromMod("ThoriumMod")]
    public class ElectrifiedThorsHammer : GlobalItem
    {
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!InfernalConfig.Instance.ThoriumBalanceChangess) return;
            if (item.type == ModContent.ItemType<MeleeThorHammer>())
                target.AddBuff(BuffID.Electrified, 180);
            base.OnHitNPC(item, player, target, hit, damageDone);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        { 
            if (!(item.type == ModContent.ItemType<MeleeThorHammer>() || item.type == ModContent.ItemType<RangedThorHammer>() || item.type == ModContent.ItemType<MagicThorHammer>()) || !InfernalConfig.Instance.ThoriumBalanceChangess) 
                return;
            Color color = Color.Lerp(Color.White, new Color(30, 144, byte.MaxValue), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
            string str = "Inflcts electrified for 3 seconds on hits.";
            tooltips.Add(new TooltipLine(Mod, "MjolnirInfo", str)
            {
                OverrideColor = new Color?(color)
            });
        }
    }
    [ExtendsFromMod("ThoriumMod")]
    public class ElectfifiedThorsHammerPro : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!InfernalConfig.Instance.ThoriumBalanceChangess) return;
            if (projectile.type == ModContent.ProjectileType<MagicThorHammerPro>() || projectile.type == ModContent.ProjectileType<RangedThorHammerPro>())
                target.AddBuff(BuffID.Electrified, 180);
            base.OnHitNPC(projectile, target, hit, damageDone);
        }
    }
}
