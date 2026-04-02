sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float3 uColor;
float3 uSecondaryColor;

float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float uDirection;

float4 uSourceRect;

float3 uLightSource;

float2 uImageSize0;
float2 uImageSize1;

float2 uWorldPosition;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    if (!any(color))
    {
        return color;
    }
    
    float brightness = dot(color.rgb, float3(0.299, 0.587, 0.114));
    
    brightness = pow(brightness, 0.6);
    brightness = floor(brightness * 8) / 8;
    
    float3 white = float3(1.0, 1.0, 1.0);
    float3 base = white * brightness;
    
    base = lerp(base, white, 0.5);

    float2 pixel = float2(1.0 / uImageSize0.x, 1.0 / uImageSize0.y);

    float a1 = tex2D(uImage0, coords + float2(pixel.x, 0)).a;
    float a2 = tex2D(uImage0, coords + float2(-pixel.x, 0)).a;
    float a3 = tex2D(uImage0, coords + float2(0, pixel.y)).a;
    float a4 = tex2D(uImage0, coords + float2(0, -pixel.y)).a;
    
    float a5 = tex2D(uImage0, coords + float2(pixel.x * 2, 0)).a;
    float a6 = tex2D(uImage0, coords + float2(-pixel.x * 2, 0)).a;
    float a7 = tex2D(uImage0, coords + float2(0, pixel.y * 2)).a;
    float a8 = tex2D(uImage0, coords + float2(0, -pixel.y * 2)).a;

    float neighbor = min(min(a1, a2), min(a3, a4));
    
    neighbor = min(neighbor, min(min(a5, a6), min(a7, a8)));
    
    float edge = step(neighbor, 0.1);
    
    float3 gold = lerp(float3(0.631, 0.573, 0.290), float3(0.741, 0.667, 0.302), frac(uTime * 0.5 + coords.y * 2.0));
    float3 result = lerp(base, gold, edge);

    return float4(result, color.a);
}

technique ArkangelDyeTechnique
{
    pass ArkangelDyePass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
