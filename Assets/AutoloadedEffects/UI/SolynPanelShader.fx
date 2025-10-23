sampler uImage0 : register(s0);

float uTime;
float4 uSource;
float uHoverIntensity;
float uPixel;
float uColorResolution;
float4 uColor;
float4 uSecondaryColor;
float uSpeed;

float2 normalizeWithPixelation(float2 coords, float pixelSize, float2 resolution)
{
    coords = floor(coords / pixelSize) * pixelSize;
    return coords / resolution;
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{
    float2 resolution = uSource.xy;
    float2 worldPos = uSource.zw;
    coords = (position.xy - worldPos) / resolution;
    
    float2 uv = normalizeWithPixelation(position.xy - worldPos, uPixel, resolution);
    
    // Cosmic-themed colors (purple to cyan gradient)
    float cosmicWave = sin(uTime * uSpeed * 3.14159);
    float4 cosmicGrad = lerp(uColor * (1 + uHoverIntensity * 0.3), uSecondaryColor, uv.x * 1.5 - cosmicWave * 0.15);
    
    // Add some cosmic sparkle effect
    float sparkle = sin(uv.x * 8 + uv.y * 6 - uTime * uSpeed * 2) * 0.2;
    float4 panelColor = lerp(cosmicGrad, sampleColor + 0.1 + uHoverIntensity * 0.2, pow(uv.x, 1.1 + cosmicWave * 0.2));
    
    panelColor.rgb *= 1.3 + sparkle * (1 - uv.x * 0.2);
    
    return tex2D(uImage0, coords) * panelColor;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}