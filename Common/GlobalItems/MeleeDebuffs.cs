using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    //Wardrobe Hummus
    public class MeleeDebuffs : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("WHummusMultiModBalancing", out _);
        }

        public override bool InstancePerEntity => true;

        public override void OnHitNPC(
            Item item,
            Player player,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) && InfernalConfig.Instance.ThoriumBalanceChangess)
            {
                var thunderTalon = thoriumMod.Find<ModItem>("ThunderTalon");
                if (thunderTalon != null && item.type == thunderTalon.Type)
                {
                    target.AddBuff(144, 180); // Electrified
                }
            }

            if (item.type == ItemID.LucyTheAxe && InfernalConfig.Instance.VanillaBalanceChanges)
            {
                target.AddBuff(ModContent.BuffType<Crumbling>(), 180);
            }
        }
    }
}
