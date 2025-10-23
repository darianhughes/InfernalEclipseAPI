using System.Linq;
using InfernalEclipseAPI.Core.World;
using InfernumMode.Content.Items.Weapons.Magic;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Content.Items.Weapons.BossRush.NovaBomb;
using InfernalEclipseAPI.Content.Items.Weapons.BossRush.Swordofthe14thGlitch;
using System.Collections.Generic;
using NoxusBoss.Content.Items.Placeable;

namespace InfernalEclipseAPI.Common.GlobalItems
{
    [ExtendsFromMod("NoxusBoss")]
    public class NamelessNoInfernalDevWeapons : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            int[] blockedWeapons =
            {
                ModContent.ItemType<Swordofthe14thGlitch>(),
                ModContent.ItemType<NovaBomb>(),
                ModContent.ItemType<Kevin>(),
            };

            if (ModLoader.TryGetMod("ZenithToilet", out Mod toilet))
            {
                blockedWeapons.Append(toilet.Find<ModItem>("ZenithToilet").Type);
                blockedWeapons.Append(toilet.Find<ModItem>("TrueZenithToilet").Type);
            }

            var cdPlayer = player.GetModPlayer<InfernalPlayer>();

            if (NPC.AnyNPCs(ModContent.NPCType<NamelessDeityBoss>()) && player.name != "Infernal Tester")
            {
                foreach (int weapon in blockedWeapons)
                {
                    if (item.type == weapon)
                    {
                        Color test = new Color(255, 70, 61);
                        if (InfernalWorld.namelessDeveloperDiagloguePlayed == false)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("That item is not allowed during the test."), test);
                            InfernalWorld.namelessDeveloperDiagloguePlayed = true;
                            SoundEngine.PlaySound(new SoundStyle("NoxusBoss/Assets/Sounds/Custom/NamelessDeity/DoNotVocals1"));
                        }
                        cdPlayer.namelessDialogueCooldown = 300;
                        return false;
                    }
                }
            }

            return true;
        }

        public void AddTooltip(List<TooltipLine> tooltips, string stealthTooltip, bool afterSplash = false)
        {
            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, insert a new TooltipLine right after it with the desired color
            if (maxTooltipIndex != -1)
            {
                int insertIndex = maxTooltipIndex;
                TooltipLine customLine = new TooltipLine(Mod, "StealthTooltip", stealthTooltip);
                tooltips.Insert(insertIndex + (afterSplash ? 1 : 0), customLine);
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<StarlitForge>())
            {
                AddTooltip(tooltips, Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.StarlitForgeExtra"));
            }
        }
    }
}
