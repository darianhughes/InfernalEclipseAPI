using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.DataStructures;
using ThoriumMod.Buffs.Thrower;
using ThoriumMod.Projectiles.Thrower;

namespace InfernalEclipseAPI.Content.RogueThrower
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class AddedTechniqueBuffs : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // only care about Thorium techniques
            int decoyType = ModContent.ProjectileType<ThrowerDecoyLog>();
            int smokeType = ModContent.ProjectileType<SmokeBomb>();

            Player owner = Main.player[projectile.owner];

            if (!owner.active)
                return;

            if (projectile.type == decoyType)
            {
                owner.AddBuff(ModContent.BuffType<ThrowingSpeed>(), 60);
            }

            if (projectile.type == smokeType)
            {
                owner.AddBuff(ModContent.BuffType<ThrowingSpeed>(), 120);
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class AbsoluteZeroCostAdjustment : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");
            var getCostMethod = helperType.GetMethod("GetCost", new Type[] { enumType });
            var getDamageMethod = helperType.GetMethod("GetDamage", new Type[] { enumType });

            if (getCostMethod == null)
                return;

            hook = new ILHook(getCostMethod, PatchAbsoluteZeroCost);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchAbsoluteZeroCost(ILContext il)
        {
            var c = new ILCursor(il);
            int costTwoCount = 0;

            while (c.Next != null)
            {
                if (c.Next is Instruction instruction)
                {
                    if (instruction.OpCode == OpCodes.Ldc_I4_2)
                    {
                        if (costTwoCount == 6) // AbsoluteZero
                        {
                            instruction.OpCode = OpCodes.Ldc_I4_1;
                            instruction.Operand = null;
                            return;
                        }
                        costTwoCount++;
                    }
                }
                c.Next = c.Next.Next;
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class BloodLotusSystem : ModPlayer
    {
        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(ModContent.BuffType<BloodLotus>()))
            {
                Player.statDefense += 100;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            RemoveBloodLotus();
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            RemoveBloodLotus();
        }

        private void RemoveBloodLotus()
        {
            int buffType = ModContent.BuffType<BloodLotus>();

            int index = Player.FindBuffIndex(buffType);
            if (index != -1)
            {
                Player.DelBuff(index);
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class CobraBiteCostAdjustment : ModSystem
    {
        private ILHook hook;
        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");

            var getCostMethod = helperType.GetMethod("GetCost", new Type[] { enumType });

            if (getCostMethod == null)
                return;

            hook = new ILHook(getCostMethod, PatchCobraBiteCost);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchCobraBiteCost(ILContext il)
        {
            var c = new ILCursor(il);
            int twoCount = 0;

            while (c.Next != null)
            {
                if (c.Next is Instruction ins)
                {
                    if (ins.MatchLdcI4(2))
                    {
                        if (twoCount == 4)
                        {
                            ins.OpCode = OpCodes.Ldc_I4_1;
                            ins.Operand = null;
                            return;
                        }

                        twoCount++;
                    }
                }

                c.Next = c.Next.Next;
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class HiddenBladeDamageAdjustment : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");

            var getDamageMethod = helperType.GetMethod("GetDamage", new Type[] { enumType });

            if (getDamageMethod == null)
                return;

            hook = new ILHook(getDamageMethod, PatchHiddenBladeDamage);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchHiddenBladeDamage(ILContext il)
        {
            var c = new ILCursor(il);

            while (c.Next != null)
            {
                if (c.Next is Instruction instruction)
                {
                    if (instruction.MatchLdcI4(16)) // Hidden Blade
                    {
                        instruction.OpCode = OpCodes.Ldc_I4;
                        instruction.Operand = 62;
                        return;
                    }
                }
                c.Next = c.Next.Next;
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ShadowCloneCostChange : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");

            var getCostMethod = helperType.GetMethod("GetCost", new Type[] { enumType });

            if (getCostMethod == null)
                return;

            hook = new ILHook(getCostMethod, PatchShadowCloneCost);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchShadowCloneCost(ILContext il)
        {
            var c = new ILCursor(il);
            int foundThree = 0;

            while (c.Next != null)
            {
                if (c.Next is Instruction ins)
                {
                    // ShadowClone cost is 3 in the switch
                    if (ins.MatchLdcI4(3))
                    {
                        if (foundThree == 3) // second "3" in the switch = ShadowClone
                        {
                            ins.OpCode = OpCodes.Ldc_I4_2;
                            ins.Operand = null;
                            return;
                        }

                        foundThree++;
                    }
                }

                c.Next = c.Next.Next;
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ShadowDanceChanges : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");
            var getCostMethod = helperType.GetMethod("GetCost", new Type[] { enumType });

            if (getCostMethod == null)
                return;

            hook = new ILHook(getCostMethod, PatchShadowDanceCost);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchShadowDanceCost(ILContext il)
        {
            var c = new ILCursor(il);
            int costTwoCount = 0;

            while (c.Next != null)
            {
                if (c.Next is Instruction instruction)
                {
                    if (instruction.OpCode == OpCodes.Ldc_I4_2)
                    {
                        if (costTwoCount == 4) // 0-based index; 6th occurrence is ShadowDance
                        {
                            instruction.OpCode = OpCodes.Ldc_I4_5; // Replace with 'ldc.i4.4'
                            instruction.Operand = null; // Always null for short-form
                            return;
                        }
                        costTwoCount++;
                    }
                }
                c.Next = c.Next.Next;
            }
        }
    }

    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class StickyExplosiveDamageAdjustment : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            var helperType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueTypeHelper");
            var enumType = thorium.Code.GetType("ThoriumMod.ThrowerTechniqueType");

            var getDamageMethod = helperType.GetMethod("GetDamage", new Type[] { enumType });

            if (getDamageMethod == null)
                return;

            hook = new ILHook(getDamageMethod, PatchStickyExplosiveDamage);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void PatchStickyExplosiveDamage(ILContext il)
        {
            var c = new ILCursor(il);

            while (c.Next != null)
            {
                if (c.Next is Instruction instruction)
                {
                    if (instruction.MatchLdcI4(28)) // Sticky Explosive
                    {
                        instruction.OpCode = OpCodes.Ldc_I4;
                        instruction.Operand = 42;
                        return;
                    }
                }
                c.Next = c.Next.Next;
            }
        }
    }
}
