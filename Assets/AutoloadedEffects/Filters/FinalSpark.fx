sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float opacity;
float2 focusPosition;
float3 mainColor;

float4 FinalSpark(float2 coords : TEXCOORD0) : COLOR0
{
	float4 colour = tex2D(uImage0, coords);

	//contrast
    colour = float4(colour.r + (colour.r - 0.5) * opacity, colour.g + (colour.g - 0.5) * opacity, colour.b + (colour.b - 0.5) * opacity, colour.a);

	//hue shift
	//float lerp = 1 - uOpacity;
	//colour = float4(colour.r * lerp + colour.b * uOpacity, colour.g * lerp + colour.r * uOpacity, colour.b * lerp + colour.g * uOpacity, colour.a);
	
	return colour;
}

technique Technique1
{
    pass AutoloadPass
    {
		PixelShader = compile ps_2_0 FinalSpark();
	}
}