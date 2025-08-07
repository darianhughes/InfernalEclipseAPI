using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Lore;
using Terraria.ModLoader;
using Terraria;
using YouBoss.Content.NPCs.Bosses.TerraBlade;
using CalamityMod.Items.Materials;
using CalamityMod;
using ThoriumMod.NPCs.Depths;
using System.Security.Policy;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using ThoriumMod.NPCs.BossThePrimordials;
using CalamityMod.Items.Potions;
using MonoMod.Cil;
using System.Reflection;
using Mono.Cecil.Cil;

namespace InfernalEclipseAPI.Common.GlobalNPCs.LootAdjustments
{
    [ExtendsFromMod("ThoriumMod")]
    internal class ThoriumLootAdjustments : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<GigaClam>())
            {
                npcLoot.AddIf(() => Main.hardMode, ModContent.ItemType<MolluskHusk>(), 2, 1, 1);
            }
        }
    }

    [ExtendsFromMod("ThoriumMod")]
    public class PrimordialPotionPatch : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("ThoriumRework", out _);
        }
        public override void Load()
        {
            var method = typeof(PrimordialBase).GetMethod(
                "BossLoot",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, // binder
                new Type[] { typeof(string).MakeByRefType(), typeof(int).MakeByRefType() },
                null
            );
            if (method != null)
                MonoModHooks.Modify(method, IL_BossLoot);
        }

        private void IL_BossLoot(ILContext il)
        {
            var c = new ILCursor(il);

            // Find: ldc.i4 3544
            while (c.TryGotoNext(i => i.OpCode == OpCodes.Ldc_I4 && (int)i.Operand == 3544))
            {
                // Remove the integer load
                c.Remove();
                // Replace with a call to ModContent.ItemType<OmegaHealingPotion>()
                c.Emit(OpCodes.Call, typeof(ModContent).GetMethod(nameof(ModContent.ItemType)).MakeGenericMethod(typeof(OmegaHealingPotion)));
            }
        }
    }

    [ExtendsFromMod("ThoriumMod")]
    public class DreamEaterPotionPatch : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.TryGetMod("ThoriumRework", out _);
        }
        public override void Load()
        {
            var method = typeof(DreamEater).GetMethod(
                "BossLoot",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, // binder
                new Type[] { typeof(string).MakeByRefType(), typeof(int).MakeByRefType() },
                null
            );
            if (method != null)
                MonoModHooks.Modify(method, IL_BossLoot);
        }

        private void IL_BossLoot(ILContext il)
        {
            var c = new ILCursor(il);

            // Find: ldc.i4 3544
            while (c.TryGotoNext(i => i.OpCode == OpCodes.Ldc_I4 && (int)i.Operand == 3544))
            {
                // Remove the integer load
                c.Remove();
                // Replace with a call to ModContent.ItemType<OmegaHealingPotion>()
                c.Emit(OpCodes.Call, typeof(ModContent).GetMethod(nameof(ModContent.ItemType)).MakeGenericMethod(typeof(OmegaHealingPotion)));
            }
        }
    }
}
