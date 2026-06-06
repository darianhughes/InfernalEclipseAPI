using InfernalEclipseAPI.Core.Systems;
using ThoriumRework;

namespace InfernalEclipseAPI.Content.Buffs
{
    public class HotHands : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }

    public class HotHands2 : ModBuff
    {
        public override string Texture => "InfernalEclipseAPI/Content/Buffs/HotHands";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }

    public class HotHandsPlayer : ModPlayer
    {
        public int HotHandsStacks;

        public override void ResetEffects()
        {
            if (NPC.downedGolemBoss || NPC.downedMoonlord)
            {
                if (!Player.HasBuff(ModContent.BuffType<HotHands2>()))
                    HotHandsStacks = 0;
            }
            else
            {
                if (!Player.HasBuff(ModContent.BuffType<HotHands>()))
                    HotHandsStacks = 0;
            }
        }

        public override void PostUpdate()
        {
            if (HotHandsStacks >= 10)
            {
                Player.armorEffectDrawShadow = true;
            }
        }

        public override void PostUpdateEquips()
        {
            if (HotHandsStacks <= 0)
                return;

            Player.moveSpeed += 0.02f * HotHandsStacks;

            if (NPC.downedGolemBoss || NPC.downedMoonlord)
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.01f * HotHandsStacks;
        }

        public void AddStack()
        {
            HotHandsStacks++;

            if (HotHandsStacks > 10)
                HotHandsStacks = 10;

            if (NPC.downedGolemBoss || NPC.downedMoonlord)
            {
                Player.AddBuff(ModContent.BuffType<HotHands2>(), 180);
            }
            else
            {
                Player.AddBuff(ModContent.BuffType<HotHands>(), 180);
            }
        }
    }
}
