sampler noiseImage : register(s1);
float globalTime;
float3 mainColor;

matrix uWorldViewProjection;

struct VertexShaderInput
{
    float3 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float3 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, uWorldViewProjection);
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
};

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 color = input.Color;
    float2 coords = input.TextureCoordinates;
	coords.y = (coords.y - 0.5) / input.TextureCoordinates.z + 0.5;

    float bloomOpacity = pow(cos(coords.x * 4.8 - 0.8), 55 + pow(coords.y, 4) * 700);
    
    float opacity = 1;
    if (coords.y > 0.9)
        bloomOpacity *= pow(1 - (coords.y - 0.9) / 0.1, 1);
    else if (coords.y < 0.1)
        bloomOpacity *= pow(coords.y / 0.1, 1);

    return lerp(color, float4(mainColor, color.a), coords.y) * bloomOpacity * 0.6;
    
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_3_0 MainPS();

    }
}