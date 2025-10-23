using CalamityMod.Items.Materials;
using CalamityMod;
using ThoriumMod.NPCs.Depths;
using ThoriumMod.NPCs.BossThePrimordials;
using CalamityMod.Items.Potions;
using MonoMod.Cil;
using System.Reflection;
using Mono.Cecil.Cil;
using ThoriumMod.Items.Depths;
using ThoriumMod.NPCs.BossFallenBeholder;
using ThoriumMod.Items.Misc;
using ThoriumMod.NPCs;
using ThoriumMod.NPCs.BossStarScouter;
using Terraria.GameContent.ItemDropRules;
using ThoriumMod.Items.BossStarScouter;

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

            //Biome Key Drops
            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.Leviathan.Leviathan>())
            {
                npcLoot.Add(ModContent.ItemType<AquaticDepthsBiomeKey>());
            }

            if (npc.type == ModContent.NPCType<FallenBeholder2>() || npc.type == ModContent.NPCType<FallenBeholder2>())
            {
                npcLoot.Add(ModContent.ItemType<UnderworldBiomeKey>());
            }

            if (npc.type == NPCID.SandElemental)
            {
                npcLoot.Add(ModContent.ItemType<DesertBiomeKey>());
            }

            int[] meteoriteEnemies =
            {
                ModContent.NPCType<UFO>(),
                ModContent.NPCType<MartianScout>(),
                ModContent.NPCType<MartianSentry>(),
            };

            if (ModLoader.HasMod("SOTS"))
            {
                foreach (int npcID in meteoriteEnemies)
                {
                    if (npc.type == npcID)
                    {
                        npcLoot.Add(ModLoader.GetMod("SOTS").Find<ModItem>("TwilightShard").Type, 3);
                    }
                }

                if (npc.type == ModContent.NPCType<StarScouter>())
                {
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModLoader.GetMod("SOTS").Find<ModItem>("TwilightShard").Type, 1, 5, 10));
                }
            }

        }
    }

    [ExtendsFromMod("ThoriumMod")]
    public class ThoriumLootBagAdjustments : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ModContent.ItemType<StarScouterTreasureBag>() && ModLoader.HasMod("SOTS"))
            {
                itemLoot.Add(ModLoader.GetMod("SOTS").Find<ModItem>("TwilightShard").Type, 1, 7, 14);
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
