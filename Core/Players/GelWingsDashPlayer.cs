namespace InfernalEclipseAPI.Core.Players
{
    public class GelWingsDashPlayer : ModPlayer
    {
        public bool Active;
        public int DashTime;

        private int _inertiaTimer;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void Load()
        {
            On_Player.DashMovement += Hook_DashMovement;
        }

        public override void Unload()
        {
            On_Player.DashMovement -= Hook_DashMovement;
        }

        public override void ResetEffects()
        {
            Active = false;
        }

        public override void PostUpdateRunSpeeds()
        {
            if (_inertiaTimer > 0)
            {
                _inertiaTimer--;
                Player.runSlowdown *= 0.33f;
            }

            if (DashTime > 0)
                DashTime--;

            if (Active)
            {
                if (Player.dashDelay < 0)
                {
                    _inertiaTimer = Math.Max(_inertiaTimer, 1);

                    if (Player.controlJump && Player.releaseJump)
                    {
                        Player.dashDelay = 0;
                        _inertiaTimer = 60;
                    }

                    DashTime = 10;
                }
                else if (Player.dashDelay > 0)
                {
                    Player.dashDelay--;
                }
            }
            else
            {
                _inertiaTimer = 0;
                DashTime = 0;
            }
        }

        private static void Hook_DashMovement(On_Player.orig_DashMovement orig, Player self)
        {
            var mp = self.GetModPlayer<GelWingsDashPlayer>();

            if (mp != null && mp.Active)
            {
                self.dashType = 1;
            }

            orig(self);

            if (mp != null && mp.Active && self.dashDelay < 0)
                mp.DashTime = 10;
        }
    }
}
