using System.Collections.Generic;
using System.Linq;
using InfernalEclipseAPI.Content.Items.Armor.Vanity;

namespace InfernalEclipseAPI.Core.Systems
{
    public class DevSetSystem : ModSystem
    {
        private static List<DevSetCollection> DevSets { get; set; }

        public static List<DevSetCollection> GetDevSets() => DevSets;

        public override void SetStaticDefaults()
        {
            DevSets = new List<DevSetCollection>()
            {
                new DevSetCollection(new List<int>()
                {
                    ModContent.ItemType<PhantomMask>(),
                    ModContent.ItemType<PhantomSuitCoat>(),
                    ModContent.ItemType<PhantomSuitPants>()
                })
            };
        }
    }

    public sealed class DevSetCollection
    {
        private readonly List<Condition> _conditions;

        public IReadOnlyList<int> Items { get; }
        public float? SeparateChance { get; }
        public bool Hardmode { get; }
        public IReadOnlyList<Condition> Conditions => _conditions;

        public DevSetCollection(
            IEnumerable<int> items,
            IEnumerable<Condition> conditions = null,
            bool hardmode = false,
            float? separateChance = null)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            Items = items as IReadOnlyList<int> ?? items.ToList();
            _conditions = conditions?.Where(c => c != null).ToList() ?? new List<Condition>();

            Hardmode = hardmode;
            SeparateChance = separateChance;
        }

        public DevSetCollection(
            IEnumerable<int> items,
            Condition condition,
            bool hardmode = false,
            float? separateChance = null)
            : this(items, condition is null ? null : new[] { condition }, hardmode, separateChance)
        {
        }

        public bool CanDrop(int bagType)
        {
            // Hardmode-only sets should not drop from pre-hardmode-like bags.
            if (Hardmode && ItemID.Sets.PreHardmodeLikeBossBag[bagType])
                return false;

            // Optional independent roll gate.
            if (SeparateChance is float chance && Main.rand.NextFloat() >= chance)
                return false;

            // All conditions must be met.
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (!_conditions[i].IsMet())
                    return false;
            }

            return true;
        }
    }
}
