using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace InfernalEclipseAPI.Assets.Effects
{
    public class ShaderManager : ModSystem
    {
        private static Dictionary<string, ManagedShader> shaders;

        public static bool HasFinishedLoading
        {
            get;
            private set;
        }

        public override void OnModLoad()
        {
            // Don't attempt to load shaders on servers.
            if (Main.netMode == NetmodeID.Server)
                return;

            shaders = [];
            foreach (var path in Mod.GetFileNames().Where(f => f.Contains("Assets/Effects/")))
            {
                // Ignore paths inside of the compiler directory.
                if (path.Contains("Compiler/"))
                    continue;

                string shaderName = Path.GetFileNameWithoutExtension(path);
                string clearedPath = Path.Combine(Path.GetDirectoryName(path), shaderName).Replace(@"\", @"/");
                Ref<Effect> shader = new(Mod.Assets.Request<Effect>(clearedPath, AssetRequestMode.ImmediateLoad).Value);
                SetShader(shaderName, shader);
            }

            HasFinishedLoading = true;
        }

        public static ManagedShader GetShader(string name) => shaders[name];

        public static void SetShader(string name, Ref<Effect> newShaderData) => shaders[name] = new(name, newShaderData);
    }
}
