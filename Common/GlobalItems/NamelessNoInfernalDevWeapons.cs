using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Content.Items.Weapons.NovaBomb;
using InfernalEclipseAPI.Content.Items.Weapons.Swordofthe14thGlitch;
using InfernalEclipseAPI.Core.World;
using InfernumMode.Content.Items.Weapons.Magic;
using NoxusBoss.Content.NPCs.Bosses.NamelessDeity;
using NoxusBoss.Core.World.WorldSaving;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InfernalEclipseAPI.Core.Players;
using InfernalEclipseAPI.Content.RogueThrower;

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
            var cdPlayer = player.GetModPlayer<InfernalPlayer>();

            if (NPC.AnyNPCs(ModContent.NPCType<NamelessDeityBoss>()))
            {
                foreach (int weapon in blockedWeapons)
                {
                    if (item.type == weapon)
                    {
                        Color test = new Color(255, 70, 61);
                        if (InfernalWorld.namelessDeveloperDiagloguePlayed == false)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("That item is now allowed during the test."), test);
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
    }
}
