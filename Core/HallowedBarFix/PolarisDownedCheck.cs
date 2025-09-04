using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOTS;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Core.HallowedBarFix
{
    [ExtendsFromMod("SOTS")]
    public class PolarisDownedCheck
    {
        public static bool isPolarisDowned()
        {
            return SOTSWorld.downedAmalgamation;
        }
    }
}
