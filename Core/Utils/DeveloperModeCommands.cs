
namespace InfernalEclipseAPI.Core.Utils
{
    internal class PrintProjectilesCommand : ModCommand
    {
        public override bool IsLoadingEnabled(Mod mod) => InfernalConfig.Instance.DeveloperMode;
        public override CommandType Type => CommandType.Chat;
        public override string Command => "printprojs";
        public override string Description => "Prints the internal names of active projectiles.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            foreach (var proj in Main.projectile)
            {
                if (proj.active && proj.ModProjectile != null)
                {
                    Main.NewText($"ID: {proj.whoAmI}, Name: {proj.ModProjectile.Mod.Name}/{proj.ModProjectile.Name}");
                }
            }
        }
    }
}
