using System.Reflection;
using InfernalEclipseAPI.Core.Systems.Hooks.ILTileChanges;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using ThoriumMod;
using ThoriumMod.Tiles;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges
{
    [ExtendsFromMod("ThoriumMod")]
    public class CapeoftheSurvivorNerfSystem : ModSystem
    {
        private ILHook _consumableDodgeIL;
        private ILHook _postUpdateEquipsIL;

        public override void Load()
        {
            var tp = typeof(ThoriumMod.ThoriumPlayer);

            // 1) Disable the “<= 1 damage” Cape dodge.
            var miConsumableDodge = tp.GetMethod("ConsumableDodge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _consumableDodgeIL = new ILHook(miConsumableDodge, PatchConsumableDodge);

            // 2) Reduce Cape DR gain so it caps at 0.15 (600 * 0.00025f).
            var miPostUpdateEquips = tp.GetMethod("PostUpdateEquips", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _postUpdateEquipsIL = new ILHook(miPostUpdateEquips, PatchPostUpdateEquips);
        }

        public override void Unload()
        {
            _consumableDodgeIL?.Dispose();
            _postUpdateEquipsIL?.Dispose();
        }

        public override void PostSetupContent()
        {
            if (InfernalCrossmod.SOTS.Loaded)
            {
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<SmoothCoal>(), 1240, 0.275);
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<ThoriumOre>(), 1500, 0.4);
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<LifeQuartz>(), 1550, 0.45);
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<Aquaite>(), 2000, 0.5, () => NPC.downedBoss2);
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<LodeStone>(), 2750, 0.55, () => ThoriumWorld.downedFallenBeholder);
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<ValadiumChunk>(), 2750, 0.55, () => ThoriumWorld.downedFallenBeholder);
                SOTSMineralariumHooks.ParseNewOre(ModContent.TileType<IllumiteChunk>(), 4100, 1.25, () => NPC.downedPlantBoss);
            }
        }

        private static void PatchConsumableDodge(ILContext il)
        {
            var c = new ILCursor(il);

            // Find the call to CapeoftheSurvivorDodge and back up to the constant "1" in the compare (Damage <= 1).
            if (c.TryGotoNext(MoveType.Before, i => i.MatchCallvirt(typeof(ThoriumMod.ThoriumPlayer).GetMethod("CapeoftheSurvivorDodge",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))))
            {
                if (c.TryGotoPrev(i => i.MatchLdcI4(1)))
                {
                    // Change `<= 1` to `<= 0` (effectively never triggers on normal hits)
                    c.Next.Operand = 0;
                }
            }
        }

        private static void PatchPostUpdateEquips(ILContext il)
        {
            var c = new ILCursor(il);

            // Replace the Cape DR increment coefficient: 0.000334f -> 0.00025f (so 600 ticks => 0.15 DR).
            if (c.TryGotoNext(i => i.MatchLdcR4(0.000334f)))
            {
                c.Next.Operand = 0.00025f;
            }
        }
    }
}
