using System.Collections.Generic;
using System.Reflection;
using CalamityMod;
using CalamityMod.Events;
using InfernalEclipseAPI.Core.Utils;
using InfernumMode.Content.Items.Misc;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using NoxusBoss.Core.World.WorldSaving;
using Terraria.Localization;

namespace InfernalEclipseAPI.Core.Systems.Hooks.ILItemChanges.CalamityItemHooks
{
    internal sealed class HyperplaneMatrixCanBeUsedOverride : ModSystem
    {
        private static Hook canBeUsedHook;

        public override void OnModLoad()
        {
            var getter = typeof(HyperplaneMatrix).GetProperty("CanBeUsed", BindingFlags.Public | BindingFlags.Static)?.GetGetMethod();

            if (getter != null)
            {
                canBeUsedHook = new Hook(getter, CanBeUsedDetour);
            }
        }

        private static bool CanBeUsedDetour(Func<bool> orig)
        {
            bool goozmaDowned = true;
            if (ModLoader.HasMod("CalamityHunt"))
                goozmaDowned = StormMaidenConditionOverride.DownedGoozma();

            return (DownedBossSystem.downedCalamitas && DownedBossSystem.downedExoMechs && goozmaDowned && !BossRushEvent.BossRushActive) || Main.LocalPlayer.name == "Dominic" || Main.LocalPlayer.name == "Lucille";
        }
    }


    public class HyperplaneMatrixGlobal : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ModContent.ItemType<HyperplaneMatrix>();

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!(Main.LocalPlayer.name == "Dominic" || Main.LocalPlayer.name == "Lucille"))
                InfernalUtilities.AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.DisabledBossRush"), Color.Lerp(Color.White, new Color(255, 80, 0), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5)));
        }
    }

    [JITWhenModsEnabled("NoxusBoss")]
    [ExtendsFromMod("NoxusBoss")]
    public static class NoxusBossDowned
    {
        public static bool DeityDowned() => BossDownedSaveSystem.HasDefeated<NamelessDeityBoss>();
    }
}
