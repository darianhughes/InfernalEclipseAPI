using InfernalEclipseAPI.YharimEX.Core.Players;

namespace InfernalEclipseAPI.YharimEX.Core.Globals
{
    public class YharimEXGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            YharimEXPlayer YharimEXPlayer = player.GetModPlayer<YharimEXPlayer>();

            if (YharimEXPlayer.YharimEXNoUsingItems > 0)
                return false;
            return true;
        }
    }
}
