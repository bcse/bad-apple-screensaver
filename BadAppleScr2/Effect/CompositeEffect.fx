sampler2D implicitInput : register(s0);
sampler2D tex1 : register(s1);
sampler2D tex2 : register(s2);


float GetLuma(float4 color)
{
	// Calculate Luma according to Rec. 601
	// @see: http://en.wikipedia.org/wiki/Luma_(video)
	return dot(color.rgb, float3(0.3, 0.59, 0.11));
}

float4 main(float2 location : TEXCOORD) : COLOR
{
	float maskLuma = GetLuma(tex2D(implicitInput, location.xy));
	float4 tex1Color = tex2D(tex1, location.xy);
	float4 tex2Color = tex2D(tex2, location.xy);
	
	return float4(
		tex1Color.r * maskLuma + tex2Color.r * (1 - maskLuma),
		tex1Color.g * maskLuma + tex2Color.g * (1 - maskLuma),
		tex1Color.b * maskLuma + tex2Color.b * (1 - maskLuma),
		1);
}