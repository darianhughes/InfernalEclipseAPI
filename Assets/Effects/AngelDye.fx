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
    float3 result = white * brightness;
    
    result = lerp(result, white, 0.5);

    return float4(result, color.a);
}

technique AngelDyeTechnique
{
    pass AngelDyePass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
