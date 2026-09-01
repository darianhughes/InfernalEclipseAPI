using System.Linq;
using System.Reflection;

namespace InfernalEclipseAPI.Common.Globals.GlobalItems.ItemReworks.Weapons.Healer.Scythes
{
    [ExtendsFromMod("ThoriumMod")]
    public class SoulEssenceGlobalItem : GlobalItem
    {
        private static int woodenBatonType = -1;
        private static int iceShaverType = -1;
        private static int timesOldRomanType = -1;

        // Reflection helpers for Thorium's ScytheItem
        private static FieldInfo scytheSoulChargeFieldOnScytheBase;
        private static PropertyInfo scytheSoulChargePropOnScytheBase;
        private static Type thoriumScytheBaseType;

        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            // Thorium items
            woodenBatonType = thorium.Find<ModItem>("WoodenBaton")?.Type ?? -1;
            iceShaverType = thorium.Find<ModItem>("IceShaver")?.Type ?? -1;

            // CalamityBardHealer Cal Hunt item
            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod calamityBardHealer) && ModLoader.TryGetMod("CalamityHunt", out Mod calHunt))
            {
                timesOldRomanType = calamityBardHealer.Find<ModItem>("TimesOldRoman")?.Type ?? -1;
            }

            // Find Thorium's ScytheItem / base type containing scytheSoulCharge
            try
            {
                var asm = thorium.Code;

                thoriumScytheBaseType = asm.GetTypes()
                    .FirstOrDefault(t =>
                        typeof(ModItem).IsAssignableFrom(t) &&
                        (
                            t.GetField(
                                "scytheSoulCharge",
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.NonPublic
                            ) != null
                            ||
                            t.GetProperty(
                                "scytheSoulCharge",
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.NonPublic
                            ) != null
                        )
                    );

                if (thoriumScytheBaseType != null)
                {
                    scytheSoulChargeFieldOnScytheBase =
                        thoriumScytheBaseType.GetField(
                            "scytheSoulCharge",
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic
                        );

                    scytheSoulChargePropOnScytheBase =
                        thoriumScytheBaseType.GetProperty(
                            "scytheSoulCharge",
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic
                        );

                    Mod.Logger.Info(
                        $"[SoulEssenceGlobalItem] Found Thorium scythe base type: " +
                        $"{thoriumScytheBaseType.FullName}. " +
                        $"Field: {scytheSoulChargeFieldOnScytheBase?.Name ?? "null"}, " +
                        $"Prop: {scytheSoulChargePropOnScytheBase?.Name ?? "null"}"
                    );
                }
                else
                {
                    Mod.Logger.Info(
                        "[SoulEssenceGlobalItem] Could not find a Thorium ModItem-derived " +
                        "type declaring 'scytheSoulCharge'. Will fallback to instance reflection."
                    );
                }
            }
            catch (Exception ex)
            {
                Mod.Logger.Warn(
                    $"[SoulEssenceGlobalItem] Reflection search failed: {ex}"
                );

                thoriumScytheBaseType = null;
                scytheSoulChargeFieldOnScytheBase = null;
                scytheSoulChargePropOnScytheBase = null;
            }
        }

        public override void SetDefaults(Item item)
        {
            if (item == null)
                return;

            if (item.type != woodenBatonType &&
                item.type != iceShaverType &&
                item.type != timesOldRomanType)
            {
                return;
            }

            if (item.ModItem == null)
                return;

            // CalamityBardHealer
            if (item.type == timesOldRomanType)
            {
                SetSoulEssence(item.ModItem, 1);
                return;
            }

            // Thorium

            if (item.ModItem.Mod?.Name != "ThoriumMod")
            {
                Mod.Logger.Info(
                    $"[SoulEssenceGlobalItem] Item {item.Name} is not a Thorium ModItem; skipping."
                );

                return;
            }

            SetSoulEssence(item.ModItem, 1);
        }

        private void SetSoulEssence(ModItem modItem, int amount)
        {
            if (modItem == null)
                return;

            // 1. Cached base-class field

            if (scytheSoulChargeFieldOnScytheBase != null)
            {
                try
                {
                    scytheSoulChargeFieldOnScytheBase.SetValue(modItem, amount);
                    return;
                }
                catch (Exception ex)
                {
                    Mod.Logger.Warn(
                        $"[SoulEssenceGlobalItem] Failed to set field on base type: {ex}"
                    );
                }
            }

            // 2. Cached base-class property

            if (scytheSoulChargePropOnScytheBase != null &&
                scytheSoulChargePropOnScytheBase.CanWrite)
            {
                try
                {
                    scytheSoulChargePropOnScytheBase.SetValue(modItem, amount);
                    return;
                }
                catch (Exception ex)
                {
                    Mod.Logger.Warn(
                        $"[SoulEssenceGlobalItem] Failed to set property on base type: {ex}"
                    );
                }
            }

            // 3. Fallback: concrete item type

            var concreteType = modItem.GetType();

            var field = concreteType.GetField(
                "scytheSoulCharge",
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            if (field != null)
            {
                try
                {
                    field.SetValue(modItem, amount);
                    return;
                }
                catch (Exception ex)
                {
                    Mod.Logger.Warn(
                        $"[SoulEssenceGlobalItem] Failed to set concrete field: {ex}"
                    );
                }
            }

            var prop = concreteType.GetProperty(
                "scytheSoulCharge",
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            if (prop != null && prop.CanWrite)
            {
                try
                {
                    prop.SetValue(modItem, amount);
                    return;
                }
                catch (Exception ex)
                {
                    Mod.Logger.Warn(
                        $"[SoulEssenceGlobalItem] Failed to set concrete property: {ex}"
                    );
                }
            }

            Mod.Logger.Warn(
                $"[SoulEssenceGlobalItem] Couldn't find 'scytheSoulCharge' " +
                $"on {concreteType.FullName}."
            );
        }
    }
}
