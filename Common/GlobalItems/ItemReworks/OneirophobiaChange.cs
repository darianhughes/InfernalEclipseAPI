using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace InfernalEclipseAPI.Common.GlobalItems.ItemReworks
{
    //Provided by Wardrobe Hummus
    public class OneirophobiaChange : GlobalItem
    {
        private const int OneirophobiaBaseDamage = 3000;

        public override void UpdateInventory(Item item, Player player)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumRework", out mod) || !mod.TryFind("Oneirophobia", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            item.damage = player.slotsMinions > 0.0 ? 1000 : 3000;

            if (item.ModItem == null || item.ModItem.Mod?.Name != "ThoriumRework" || item.ModItem.Name != "Oneirophobia")
                return;
            Mod mod1;
            Mod mod2 = ModLoader.TryGetMod("ThoriumRework", out mod1) ? mod1 : null;
            if (mod2 == null)
                return;
            Type type = mod2.Code?.GetType("ThoriumRework.ThoriumPlayer");
            if (type == null)
            {
                Main.NewText("Failed to get ThoriumPlayer type", byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            else
            {
                FieldInfo field1 = typeof(Player).GetField("modPlayers", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field1 == null)
                    Main.NewText("Failed to get modPlayers field", byte.MaxValue, byte.MaxValue, byte.MaxValue);
                else if (!(field1.GetValue(player) is IList<ModPlayer> modPlayerList))
                {
                    Main.NewText("Failed to get modPlayers list", byte.MaxValue, byte.MaxValue, byte.MaxValue);
                }
                else
                {
                    object obj = null;
                    foreach (ModPlayer modPlayer in (IEnumerable<ModPlayer>)modPlayerList)
                    {
                        if (modPlayer.GetType().FullName == "ThoriumRework.ThoriumPlayer")
                        {
                            obj = modPlayer;
                            break;
                        }
                    }
                    if (obj == null)
                    {
                        Main.NewText("Failed to locate ThoriumPlayer instance in modPlayers", byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    }
                    else
                    {
                        bool flag1 = player.HeldItem.type == item.type;
                        bool flag2 = Main.mouseRight && !Main.mouseLeft;
                        FieldInfo field2 = type.GetField("oneirophobic", BindingFlags.Instance | BindingFlags.Public);
                        if (!(field2 != null))
                            return;
                        if (flag1 & flag2)
                            field2.SetValue(obj, true);
                        else
                            field2.SetValue(obj, false);
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Mod mod;
            ModItem modItem;
            if (!ModLoader.TryGetMod("ThoriumRework", out mod) || !mod.TryFind("Oneirophobia", out modItem) || item.type != modItem.Type || !InfernalConfig.Instance.ThoriumBalanceChangess || ModLoader.TryGetMod("WHummusMultiModBalancing", out Mod WHBalance))
                return;
            string str = Main.LocalPlayer.slotsMinions > 0.0 ? "Damage reduced while summons are active" : "Has reduced damage if any summons are active";
            Color color = Color.Lerp(Color.White, new Color(30, 144 /*0x90*/, byte.MaxValue), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));
            tooltips.Add(new TooltipLine(Mod, "OneirophobiaInfo1", "Hold right click to summon the Ludic Instrument to fight for you")
            {
                OverrideColor = new Color?(color)
            });
            tooltips.Add(new TooltipLine(Mod, "OneirophobiaInfo2", str)
            {
                OverrideColor = new Color?(color)
            });
        }
    }
}
