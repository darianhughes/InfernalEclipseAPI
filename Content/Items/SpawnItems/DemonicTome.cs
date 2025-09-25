using System.Linq;
using CalamityMod.Events;
using InfernalEclipseAPI.Content.NPCs.LittleCat;
using Terraria.Audio;
using CalamityMod.UI.CalamitasEnchants;
using Terraria.ID;

namespace InfernalEclipseAPI.Content.Items.SpawnItems
{
    public class DemonicTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 13;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.rare = ItemRarityID.Purple;
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            NPC TownCat = Main.npc.FirstOrDefault(n => n.active && n.netID == NPCID.TownCat);
            if (TownCat == null)
            {
                return false;
            }
            bool allowMoreThanOneBoss = ModLoader.TryGetMod("Fargowiltas", out _) || !NPC.AnyNPCs(ModContent.NPCType<LittleCat>());
            return allowMoreThanOneBoss && !BossRushEvent.BossRushActive;
        }

        public override bool? UseItem(Player player)
        {
            NPC TownCat = Main.npc.FirstOrDefault(n => n.active && n.netID == NPCID.TownCat);
            if (TownCat == null)
            {
                return false;
            }

            SoundEngine.PlaySound(SoundID.Roar, player.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<LittleCat>());
            else
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<LittleCat>());

            return true;
        }
    }
}