sampler uImage0 : register(s0);
sampler noiseTexture : register(s1);

float globalTime;
float4 secondaryColor;
float4 lensFlareColor;
matrix uWorldViewProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, uWorldViewProjection);
    output.Position = pos;

    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float2 coords = input.TextureCoordinates;
    float4 color = input.Color;

    // Texture distortion correction
    coords.y = (coords.y - 0.5) / input.TextureCoordinates.z + 0.5;

    // Animated noise
    float2 noiseCoords = frac(coords * 2 + float2(0, coords.x * 5) - globalTime * 3);
    float noise = tex2D(noiseTexture, noiseCoords).r;

    float opacity = smoothstep(0.3, 0.45, noise);

    float horizontalDistanceFromCenter = distance(coords.y, 0.5);
    float innerGlow = saturate(lerp(0.01, 0.98, noise) / horizontalDistanceFromCenter) * smoothstep(0.25, 0.12, horizontalDistanceFromCenter);
    float4 startingGlow = lensFlareColor * smoothstep(0.11, 0.01, coords.x);

    color = lerp(color, secondaryColor, smoothstep(0.3, 0.18, horizontalDistanceFromCenter) + noise * 0.1);

    return saturate(color * opacity + innerGlow + startingGlow) * smoothstep(0.5, 0.4, horizontalDistanceFromCenter);
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}