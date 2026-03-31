using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.YharimEX.Common.Graphics.Particles
{
    public class ExpandingBloomParticle : BaseExpandingParticle
    {
        public ExpandingBloomParticle(Vector2 position, Vector2 velocity, Color drawColor, Vector2 startScale, Vector2 endScale, int lifetime, bool useExtraBloom = false, Color? extraBloomColor = null)
            : base(position, velocity, drawColor, startScale, endScale, lifetime, useExtraBloom, extraBloomColor)
        {
        }
    }
}
