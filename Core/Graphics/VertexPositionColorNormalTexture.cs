using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace InfernalEclipseAPI.Core.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct VertexPositionColorNormalTexture(Vector3 position, Color color, Vector2 textureCoordinates, Vector3 normal) : IVertexType
    {
        public readonly Vector3 Position = position;
        public readonly Color Color = color;
        public readonly Vector2 TextureCoords = textureCoordinates;
        public readonly Vector3 Normal = normal;
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[4]
        {
            new VertexElement(0, (VertexElementFormat) 2, (VertexElementUsage) 0, 0),
            new VertexElement(12, (VertexElementFormat) 4, (VertexElementUsage) 1, 0),
            new VertexElement(16 /*0x10*/, (VertexElementFormat) 1, (VertexElementUsage) 2, 0),
            new VertexElement(24, (VertexElementFormat) 2, (VertexElementUsage) 3, 0)
        });

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get => VertexDeclaration;
        }
    }
}
