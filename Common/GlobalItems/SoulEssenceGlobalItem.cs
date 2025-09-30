using System.Linq;
using System.Reflection;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    [ExtendsFromMod("ThoriumMod")]
    public class SoulEssenceGlobalItem : GlobalItem
    {
        private static int woodenBatonType = -1;
        private static int iceShaverType = -1;

        // Reflection helpers
        private static FieldInfo scytheSoulChargeFieldOnScytheBase;
        private static PropertyInfo scytheSoulChargePropOnScytheBase;
        private static System.Type thoriumScytheBaseType;

        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            // use the exact Thorium item names you wanted
            woodenBatonType = thorium.Find<ModItem>("WoodenBaton")?.Type ?? -1;
            iceShaverType = thorium.Find<ModItem>("IceShaver")?.Type ?? -1;

            // Try to find a ModItem-derived type in Thorium that defines scytheSoulCharge
            var asm = thorium.Code;
            try
            {
                thoriumScytheBaseType = asm.GetTypes()
                    .FirstOrDefault(t =>
                        typeof(ModItem).IsAssignableFrom(t) &&
                        (t.GetField("scytheSoulCharge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null
                         || t.GetProperty("scytheSoulCharge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
                    );

                if (thoriumScytheBaseType != null)
                {
                    scytheSoulChargeFieldOnScytheBase = thoriumScytheBaseType.GetField("scytheSoulCharge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    scytheSoulChargePropOnScytheBase = thoriumScytheBaseType.GetProperty("scytheSoulCharge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    Mod.Logger.Info($"[SoulEssenceGlobalItem] Found Thorium scythe base type: {thoriumScytheBaseType.FullName}. Field: {scytheSoulChargeFieldOnScytheBase?.Name ?? "null"}, Prop: {scytheSoulChargePropOnScytheBase?.Name ?? "null"}");
                }
                else
                {
                    Mod.Logger.Info("[SoulEssenceGlobalItem] Could not find a Thorium ModItem-derived type declaring 'scytheSoulCharge'. Will fallback to instance reflection.");
                }
            }
            catch (System.Exception ex)
            {
                Mod.Logger.Warn($"[SoulEssenceGlobalItem] Reflection search failed: {ex}");
                thoriumScytheBaseType = null;
                scytheSoulChargeFieldOnScytheBase = null;
                scytheSoulChargePropOnScytheBase = null;
            }
        }

        public override void SetDefaults(Item item)
        {
            if (item == null)
                return;

            // Only operate for the two items you asked about
            if (item.type != woodenBatonType && item.type != iceShaverType)
                return;

            // Ensure it's actually a Thorium item instance
            if (item.ModItem == null || item.ModItem.Mod?.Name != "ThoriumMod")
            {
                Mod.Logger.Info($"[SoulEssenceGlobalItem] Item {item.Name} is not a Thorium ModItem; skipping.");
                return;
            }

            // 1) If we found a base type that declares the field/property, try to set via the cached FieldInfo/PropertyInfo
            if (scytheSoulChargeFieldOnScytheBase != null)
            {
                try
                {
                    scytheSoulChargeFieldOnScytheBase.SetValue(item.ModItem, 1);
                    return;
                }
                catch (System.Exception ex)
                {
                    Mod.Logger.Warn($"[SoulEssenceGlobalItem] Failed to set field on base type: {ex}");
                }
            }
            if (scytheSoulChargePropOnScytheBase != null && scytheSoulChargePropOnScytheBase.CanWrite)
            {
                try
                {
                    scytheSoulChargePropOnScytheBase.SetValue(item.ModItem, 1);
                    return;
                }
                catch (System.Exception ex)
                {
                    Mod.Logger.Warn($"[SoulEssenceGlobalItem] Failed to set property on base type: {ex}");
                }
            }

            // 2) Fallback: look directly on the concrete ModItem instance's type (covers odd layouts)
            var concreteType = item.ModItem.GetType();
            var field = concreteType.GetField("scytheSoulCharge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(item.ModItem, 1);
                return;
            }

            var prop = concreteType.GetProperty("scytheSoulCharge", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(item.ModItem, 1);
                return;
            }

            // If we reach here nothing was found/set — log for debugging
            Mod.Logger.Warn($"[SoulEssenceGlobalItem] Couldn't find 'scytheSoulCharge' on Thorium item instance {item.Name} (concrete type {concreteType.FullName}).");
        }
    }
}
