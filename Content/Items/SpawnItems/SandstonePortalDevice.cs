using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.SummonItems;
using InfernumMode.Content.Subworlds;
using InfernumMode.Core.GlobalInstances.Systems;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;
using Microsoft.Xna.Framework;
using InfernumMode;
using Terraria.Audio;
using CalamityMod.Tiles.Astral;
using InfernumMode.Content.Tiles.Colosseum;
using InfernumMode.Content.BehaviorOverrides.BossAIs.GreatSandShark;

namespace InfernalEclipseAPI.Content.Items.SpawnItems
{
    public class SandstonePortalDevice : ModItem
    {
        public override string Texture => "CalamityMod/Items/SummonItems/SandstormsCore"; //placeholder until real sprite is made
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 13;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<SandstormsCore>());
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = false;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return WorldSaveSystem.HasOpenedLostColosseumPortal;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(AstralBeacon.UseSound);
            SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen);
            if (SubworldSystem.IsActive<LostColosseum>())
                SubworldSystem.Exit();
            else
            {
                if (!InfernumSaveSystem.InfernumModeEnabled)
                {
                    CombatText.NewText(Main.LocalPlayer.Hitbox, Color.Orange, Language.GetTextValue("Mods.InfernumMode.Status.InfernumNeededToEnterColosseum"));
                    return true;
                }

                Main.LocalPlayer.Infernum_Biome().PositionBeforeEnteringSubworld = Main.LocalPlayer.Center;
                SubworldSystem.Enter<LostColosseum>();

                //if (Main.netMode != NetmodeID.MultiplayerClient)
                //{
                //    Main.LocalPlayer.Infernum_Biome().PositionBeforeEnteringSubworld = Main.LocalPlayer.Center;
                //    SubworldSystem.Enter<LostColosseum>();
                //}
                //else
                //{
                //    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<BereftVassal>());
                //}
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (WorldSaveSystem.HasGeneratedColosseumEntrance || SubworldSystem.IsActive<LostColosseum>())
            {
                tooltips.Add(new TooltipLine(Mod, "SandstonePortalDeviceInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SandstonePortalDeviceInfo")) { OverrideColor = Color.Lerp(Color.Orange, Color.Yellow, 0.55f) });
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "NoGatewayWarning", Utilities.GetLocalization("Items.SandstormsCore.GatewayWarning").Value) { OverrideColor = Color.Orange });
            }
            //if (Main.netMode != NetmodeID.MultiplayerClient)
            //{
            //}
            //else
            //{
            //    tooltips.Add(new TooltipLine(Mod, "SandstonePortalDeviceInfo", Language.GetTextValue("Mods.InfernalEclipseAPI.ItemTooltip.SandstonePortalDeviceInfoMP")) { OverrideColor = Color.Lerp(Color.Orange, Color.Yellow, 0.55f) });
            //}
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SandstormsCore>()
                .AddIngredient(ItemID.FragmentStardust, 5)
                .AddTile<ColosseumPortal>()
                .Register();
        }
    }
}
