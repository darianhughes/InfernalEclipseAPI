sampler uImage0 : register(s0);

float uTime;
float uHoverIntensity;

float4 PixelShaderFunction(float2 coords : SV_POSITION, float2 tex_coords : TEXCOORD0, float4 baseColor : COLOR0) : COLOR0
{
    float2 uv = tex_coords;
    float4 color1 = float4(139.0/255.0, 0.0/255.0, 0.0/255.0, 1.0);
    float4 color2 = float4(255.0/255.0, 165.0/255.0, 0.0/255.0, 1.0);
    
    float flow1 = sin(uTime * 1.8 + uv.x * 6.0 + uv.y * 2.0) * 0.25;
    float flow2 = sin(uTime * 3.1 + uv.x * 9.0 - uv.y * 1.5) * 0.15;
    float flow3 = sin(uTime * 0.6 + uv.x * 3.5 + uv.y * 4.0) * 0.18;
    float flow = flow1 + flow2 + flow3;
    float4 gradient = lerp(color1, color2, uv.y + flow);
    gradient.rgb = lerp(gradient.rgb, gradient.rgb * 1.3, uHoverIntensity * 0.3);
    
    return tex2D(uImage0, tex_coords) * gradient;
}

technique Technique1
{
    pass PanelGradient
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}