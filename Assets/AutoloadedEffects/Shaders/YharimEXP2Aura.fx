sampler diagonalNoise : register(s1);

float colorMult;
float time;
float maxOpacity;
float radius;

float2 screenPosition;
float2 screenSize;
float2 anchorPoint;
float2 playerPosition;

float4 darkColor;
float4 midColor;
float4 lightColor;

float InverseLerp(float a, float b, float t)
{
    return saturate((t - a) / (b - a));
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    //float textureScale = radius / 1400; // 1400 is abom's ritual size
    
    float2 worldUV = screenPosition + screenSize * uv;
    float2 provUV = anchorPoint / screenSize;
    float worldDistance = distance(worldUV, anchorPoint);
    float adjustedTime = time * 0.1;
    
    // Pixelate the uvs
    float2 pixelatedUV = worldUV / screenSize;
    pixelatedUV.x -= worldUV.x % (1 / screenSize.x);
    pixelatedUV.y -= worldUV.y % (1 / (screenSize.y / 2) * 2);
    
    float2 noiseUV = pixelatedUV - 0.5 * (anchorPoint / screenSize);
    float2 vec1 = float2(0.56, 1.2);
    float2 vec2 = float2(-0.3, -0.9);
    float2 vec3 = float2(0.8, 0.3);
    
    // Textures
    float noiseMesh1 = tex2D(diagonalNoise, frac(noiseUV * 1.46  + vec1 * adjustedTime)).g;
    float noiseMesh2 = tex2D(diagonalNoise, frac(noiseUV * 1.57 + vec2 * adjustedTime)).g;
    float noiseMesh3 = tex2D(diagonalNoise, frac(noiseUV * 1.57 + vec3 * adjustedTime)).g;
    float textureMesh = noiseMesh1 * 0.3 + noiseMesh2 * 0.3 + noiseMesh3 * 0.3;
    
    float opacity = 0.35;
    float distToPlayer = distance(playerPosition, worldUV);
    opacity += InverseLerp(1000, 500, distToPlayer) * 0.4;
    
    
    // Define the border and fade
    bool border = worldDistance < radius && opacity > 0;
    float colorMult = 1;
    if (border) 
        colorMult = InverseLerp(radius * 0.9, radius, worldDistance);
    else
    {
        colorMult = InverseLerp(radius * 5, radius, worldDistance);
    }
        
    opacity = clamp(opacity, 0, maxOpacity);
    
    if (colorMult == 1 && (opacity == 0 || worldDistance < radius))
        return sampleColor;
    
    
    float colorLerp = pow(colorMult, 4);
    float4 color;
    float split = 0.6;
    color = lerp(darkColor, midColor, colorLerp);
    opacity /= pow(abs(textureMesh), 0.5);
        
    return color * colorMult * opacity;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
