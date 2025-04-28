using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using InfernumSaveSystem = InfernumMode.Core.GlobalInstances.Systems.WorldSaveSystem;

namespace InfernalEclipseAPI.Content.DifficultyOverrides
{
    internal class hellActive
    {
        public static bool InfernumActive
        {
            get => InfernumSaveSystem.InfernumModeEnabled;
        }
    }
}
