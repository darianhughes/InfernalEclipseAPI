sampler baseTexture : register(s0);
sampler noiseTexture : register(s1);

float time;
float chromaticAberrationOffset;
float2 textureSize;

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 pixelationFactor = 1 / textureSize;
    coords = floor(coords / pixelationFactor) * pixelationFactor;
    
    // Calculate values for glitch noise.
    // This will be used to offset the texture in bar-like formations.
    float glitchPixelationFactor = 72;
    float timeStepFactor = 11;
    float glitchCoords = floor(coords.y * glitchPixelationFactor) / glitchPixelationFactor + floor(time * timeStepFactor) / timeStepFactor;    
    float glitchNoise = tex2D(noiseTexture, glitchCoords);
    
    glitchCoords = floor(coords.y * glitchPixelationFactor * 0.5) / glitchPixelationFactor + floor(time * timeStepFactor * 1.5) / timeStepFactor;
    glitchNoise = (glitchNoise + tex2D(noiseTexture, glitchCoords)) * 0.67;
    
    glitchCoords = floor(coords.y * glitchPixelationFactor * 0.4) / glitchPixelationFactor + floor(time * timeStepFactor * 1.3) / timeStepFactor;
    glitchNoise = (glitchNoise + tex2D(noiseTexture, glitchCoords)) * 0.62;
    
    // Perform the glitch offset and re-pixelate.
    coords.x += smoothstep(0.8, 1, glitchNoise) * 0.05;
    coords = round(coords / pixelationFactor) * pixelationFactor;
    
    float r = tex2D(baseTexture, coords + float2(-0.707, 0) / textureSize * chromaticAberrationOffset).r;
    float g = tex2D(baseTexture, coords + float2(0.707, 0) / textureSize * chromaticAberrationOffset).g;
    float b = tex2D(baseTexture, coords + float2(0, 0.707) / textureSize * chromaticAberrationOffset).b;
    float4 color = float4(r, g, b, tex2D(baseTexture, coords).a);
    
    return color * sampleColor;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}