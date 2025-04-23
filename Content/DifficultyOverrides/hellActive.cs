using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    internal class hellActive
    {
        public static bool InfernumActive
        {
            get
            {
                if (ModLoader.TryGetMod("InfernumMod", out Mod hell))
                {
                    return (bool)hell.Call("GetInfernumActive");
                }

                return false;
            }
        }
    }
}
