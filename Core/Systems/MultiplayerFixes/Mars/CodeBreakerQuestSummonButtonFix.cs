using CalamityMod.TileEntities;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System.Reflection;
using static CalamityMod.UI.DraedonSummoning.CodebreakerUI;
namespace InfernalEclipseAPI.Core.Systems.MultiplayerFixes.Mars
{
    public class CodeBreakerQuestSummonButtonFix : ModSystem
    {
        private static Hook marsButtonHook;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.HasMod("CalAddonMultiplayerWorkaround");
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod("NoxusBoss", out Mod _))
                return;

            MethodInfo target = typeof(NoxusBoss.Core.Graphics.UI.Codebreaker.CodebreakerUIRewriter)
                .GetMethod("HandleDraedonSummonButton_Mars", BindingFlags.Public | BindingFlags.Static); // hooks to the function that is used to summon Mars.

            if (target != null)
            {
                marsButtonHook = new Hook(target, typeof(CodeBreakerQuestSummonButtonFix).GetMethod(nameof(MarsButtonFix), BindingFlags.Public | BindingFlags.Static));
            }
        }

        public override void Unload()
        {
            marsButtonHook?.Dispose();
        }

        public static void MarsButtonFix(Action<TECodebreaker, Vector2> orig, TECodebreaker codebreakerTileEntity, Vector2 drawPosition)
        {
            orig(codebreakerTileEntity, drawPosition);

            if (MouseScreenArea.Intersects(Terraria.Utils.CenteredRectangle(drawPosition, Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>("CalamityMod/UI/DraedonSummoning/ContactIcon").Value.Size() * 1f))
                && Main.mouseLeft && Main.mouseLeftRelease
                && Main.netMode != Terraria.ID.NetmodeID.SinglePlayer)
            {
                ModPacket packet = ModContent.GetInstance<InfernalEclipseAPI>().GetPacket();
                packet.Write((byte)InfernalEclipseMessageType.MarsMultiplayerSync);
                packet.Send();
            }
        }
    }
}
