using CalamityMod.DataStructures;
using CalamityMod.Systems.Collections;
using InfernalEclipseAPI.Content.Buffs.Tag;
using InfernalEclipseAPI.Content.Items.Weapons.Legendary.SplitFirebrand;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace InfernalEclipseAPI.Core.Systems
{
    public class InfernalEclipseAPISummonTagSystem : ModSystem
    {
        private struct SummonTagEntry
        {
            public Func<int> ItemType;

            public Func<int> BuffType;

            public Action<SummonTag> Setup;
        }

        public override void PostSetupContent()
        {
            SummonTagEntry[] array = new SummonTagEntry[1]
            {
                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<SplitFirebrand>(),
                    BuffType = () => ModContent.BuffType<SplitFirebrandTag1>(),
                    Setup = delegate (SummonTag summonTag)
                    {
                        summonTag.FlatTagDamage = 3;
                        summonTag.AutoDrawTooltip = false;
                        summonTag.TagTexture = ModContent.Request<Texture2D>("InfernalEclipseAPI/Content/Items/Weapons/Legendary/SplitFirebrand/SplitFirebrand", (AssetRequestMode)1);
                    }
                }/*,
                new SummonTagEntry
                {
                    BuffType = () => ModContent.BuffType<DefaultSummonTag>(),
                    Setup = delegate (SummonTag summonTag)
                    {
                        summonTag.AutoDrawTooltip = false;
                        summonTag.TagTexture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj", (AssetRequestMode)1);
                    }
                },*/
            };

            for (int num = 0; num < array.Length; num++)
            {
                SummonTagEntry summonTagEntry = array[num];
                SummonTag tag = new SummonTag(summonTagEntry.ItemType());
                summonTagEntry.Setup?.Invoke(tag);
                int debuff = summonTagEntry.BuffType();
                if (!CalamityBuffSets.SummonTagDebuff.ContainsKey(debuff))
                {
                    CalamityBuffSets.SummonTagDebuff.Add(debuff, tag);
                }
            }
        }


    }
}
