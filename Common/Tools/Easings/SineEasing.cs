using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminance.Common.Easings;
using static Microsoft.Xna.Framework.MathHelper;

namespace InfernalEclipseAPI.Common.Tools.Easings
{
    public class SineEasing : EasingCurve
    {
        public static readonly SineEasing Default = new();

        public SineEasing()
        {
            InCurve = new(interpolant =>
            {
                return 1f - MathF.Cos(interpolant * PiOver2);
            });
            OutCurve = new(interpolant =>
            {
                return MathF.Sin(interpolant * PiOver2);
            });
            InOutCurve = new(interpolant =>
            {
                return (MathF.Cos(Pi * interpolant) - 1f) * -0.5f;
            });
        }
    }
}
