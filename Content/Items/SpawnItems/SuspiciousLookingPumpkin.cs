using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.NPCs.BossLich;
using ThoriumMod.Tiles;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using ThoriumMod.Utilities;
using CalamityMod.Items.Materials;
using ThoriumMod.Items.Misc;

namespace InfernalEclipseAPI.Content.Items.SpawnItems
{
    [ExtendsFromMod("ThoriumMod")]
    public class SuspiciousLookingPumpkin : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 4988; //Queen Slime Summon
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 40;
            Item.rare = ItemRarityID.LightPurple;
            Item.consumable = false;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Pumpkin, 30)
                .AddIngredient<EssenceofHavoc>(5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient<SoulofPlight>(5)
                .AddTile<SoulForge>()
                .Register();
        }
    }

    [ExtendsFromMod("ThoriumMod")]
    public class AncientPhylacteryAdjustments : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            if (type == ModContent.TileType<AncientPhylactery>())
            {
                if (!AncientPhylactery.DownedAllMechBosses)
                {
                    base.RightClick(i, j, type);
                }
                else
                {
                    Player localPlayer = Main.LocalPlayer;
                    if (Main.IsItDay())
                    {
                        base.RightClick(i, j, type);
                    }
                    else
                    {
                        if ((NPC.AnyNPCs(ModContent.NPCType<Lich>()) ? 1 : (NPC.AnyNPCs(ModContent.NPCType<LichHeadless>()) ? 1 : 0)) != 0)
                        {
                            base.RightClick(i, j, type);
                        }
                        else
                        {
                            Dictionary<int, int> hasSusPumpkin = localPlayer.CountInventoryItemIdxWithStack(ModContent.ItemType<SuspiciousLookingPumpkin>(), 1);
                            if (hasSusPumpkin.Count > 0)
                            {
                                SoundEngine.PlaySound(SoundID.NPCDeath7, new Vector2?(localPlayer.Center), null);
                                int num1 = i;
                                Tile tile1 = Main.tile[i, j];
                                int num2 = tile1.TileFrameX / 18 % 3;
                                int num3 = num1 - num2;
                                int num4 = j;
                                Tile tile2 = Main.tile[i, j];
                                int num5 = tile2.TileFrameY / 18 % 3;
                                int num6 = num4 - num5;
                                Vector2 center = new Vector2((num3 + 1 + 0.5f) * 16f, (num6 + 1f - 0.2f) * 16f);
                                float dustCount = 50f;
                                for (int a = 0; a < dustCount; a++)
                                {
                                    // Angle around the circle
                                    double angle = i * (MathHelper.TwoPi / dustCount);

                                    // Get rotated offset
                                    Vector2 offset = Utils.RotatedBy(Vector2.UnitY * -1f, angle) * new Vector2(30f, 30f);

                                    // Spawn the dust
                                    int dustIndex = Dust.NewDust(center, 0, 0, DustID.GemAmethyst, 0f, 0f, 100, default, 1.25f);
                                    Dust dust = Main.dust[dustIndex];
                                    dust.noGravity = true;
                                    dust.position = center + offset;
                                    dust.velocity = Vector2.Normalize(offset) * 6f;
                                }
                                SoundEngine.PlaySound(SoundID.Roar, new Vector2?(localPlayer.Center), null);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    NPC.SpawnOnPlayer(localPlayer.whoAmI, ModContent.NPCType<Lich>());
                                else
                                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, localPlayer.whoAmI, type, 0.0f, 0.0f, 0, 0, 0);
                            }
                        }
                    }
                }
            }
            base.RightClick(i, j, type);
        }
    }
}
