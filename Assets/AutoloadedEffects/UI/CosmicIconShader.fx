sampler uImage0 : register(s0);

float uTime;
float uSpeed;
float uPixel;
float uColorResolution;
float4 uSource;
float3 uInColor;

float3 cosmicRay(float2 ray, float2 uv, float3 col, float delay, float size, float len)
{
    float3 cosmicCol = float3(0.5, 0.3, 1.0); // Purple-blue cosmic color
    float ang = dot(normalize(uv), normalize(ray));
    ang = 1.0 - ang;
    ang = ang / size;
    ang = clamp(ang, 0.0, 1.0);

    float v = smoothstep(0.0, 1.0, (1.0 - ang) * (1.0 - ang));
    v *= sin(uTime / 2.0 + delay) / 2.0 + 0.5;

    float l = length(uv) * len;
    l = clamp(l, 0.0, 1.0);

    float3 o = lerp(col, lerp(cosmicCol, col, l), smoothstep(1.0, 4.0, uTime) * v);
    return o;
}

float hash11(float p)
{
    p = frac(p * .1031);
    p *= p + 34.32;
    p *= p + p;
    return frac(p);
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{
    float2 uv = (coords - 0.5) * 2.0;
    
    float t = uTime * uSpeed;
    float2x2 rotTime = float2x2(cos(t), sin(t), -sin(t), cos(t));
    float2 ray = mul(float2(1.0, 0.0), rotTime);
    
    float3 col = float3(0, 0, 0);
    float3 cosmicCol = uInColor;
    
    // Add multiple cosmic rays
    for (int i = 0; i < 8; i++)
    {
        float angle = 3.14159 * 2.0 * float(i) / 8.0;
        float2 rayDir = float2(cos(angle + t), sin(angle + t));
        col = cosmicRay(rayDir, uv, col, float(i), 0.05, 0.6);
    }
    
    // Add pulsing cosmic energy
    float pulse = sin(uTime * 2.0) * 0.5 + 0.5;
    col += cosmicCol * pulse * 0.3;
    
    return tex2D(uImage0, coords) * float4(col + sampleColor.rgb, sampleColor.a);
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}