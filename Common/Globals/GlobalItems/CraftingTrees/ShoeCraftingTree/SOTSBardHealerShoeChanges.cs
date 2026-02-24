using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace InfernalEclipseAPI.Common.GlobalItems.CraftingTrees.ShoeCraftingTree
{
    [ExtendsFromMod("SOTSBardHealer")]
    public class SOTSBardHealerShoeChanges : ModSystem
    {
        private ILHook hook;

        public override void Load()
        {
            var type = typeof(SOTSBardHealer.BootProgressionFix);
            var method = type.GetMethod("UpdateAccessory", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            hook = new ILHook(method, InjectEarlyReturn);
        }

        public override void Unload()
        {
            hook?.Dispose();
            hook = null;
        }

        private void InjectEarlyReturn(ILContext il)
        {
            var c = new ILCursor(il);

            // Import ModType as a TypeReference for Cecil
            var modTypeTypeRef = il.Method.Module.ImportReference(typeof(Terraria.ModLoader.ModType));
            il.Body.Variables.Add(new VariableDefinition(modTypeTypeRef));
            int modTypeLocalIndex = il.Body.Variables.Count - 1;

            var continueLabel = c.DefineLabel();

            c.Emit(OpCodes.Ldarg_1); // item
            c.Emit(OpCodes.Callvirt, typeof(Terraria.Item).GetProperty("ModItem").GetGetMethod());
            c.Emit(OpCodes.Isinst, typeof(Terraria.ModLoader.ModType));
            c.Emit(OpCodes.Stloc, modTypeLocalIndex); // store as local

            c.Emit(OpCodes.Ldloc, modTypeLocalIndex);
            c.Emit(OpCodes.Brfalse_S, continueLabel);

            c.Emit(OpCodes.Ldloc, modTypeLocalIndex);
            c.Emit(OpCodes.Callvirt, typeof(Terraria.ModLoader.ModType).GetProperty("Name").GetGetMethod());
            c.Emit(OpCodes.Ldstr, "TerrariumParticleSprinters");
            c.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality", new[] { typeof(string), typeof(string) }));
            c.Emit(OpCodes.Brfalse_S, continueLabel);

            c.Emit(OpCodes.Ret);

            c.MarkLabel(continueLabel);
        }
    }
}
