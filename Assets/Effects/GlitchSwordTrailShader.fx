sampler baseTexture : register(s1);

bool flatOpacity;
float globalTime;
float blackAppearanceInterpolant;
float trailAnimationSpeed;
float3 colorA;
float3 colorB;
matrix uWorldViewProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float BlendMode(float a, float b)
{
    return a + b * (a + b);
}

float3 BlendMode(float3 a, float3 b)
{
    return float3(BlendMode(a.r, b.r), BlendMode(a.g, b.g), BlendMode(a.b, b.b));
}

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, uWorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float2 coords = input.TextureCoordinates;
    float edgeStreakDissipation = smoothstep(0, 1, 1 - coords.x);
    float opacity = smoothstep(0, 0.05, coords.x) * smoothstep(2, 1.45, coords.x * 2 + coords.y * 1.6) * edgeStreakDissipation;
    float blendInterpolant = tex2D(baseTexture, coords * float2(1, 4) + float2(globalTime * -trailAnimationSpeed * 4, 0));
    float blackInterpolant = tex2D(baseTexture, coords * float2(1.1, 1.5) + float2(globalTime * -trailAnimationSpeed * 3.3, blendInterpolant * 0.15));
    float4 color = float4(BlendMode(colorA, colorB * blendInterpolant * 2) * 0.9, 1);
    
    // Interpolate towards black, creating a harsh contrast between the cyans and yellows.
    color = lerp(color, float4(0.06, 0.06, 0.06, 1), smoothstep(0.3, 0, blackInterpolant - blackAppearanceInterpolant));
    
    return color * input.Color * (flatOpacity ? 1 : opacity);
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
