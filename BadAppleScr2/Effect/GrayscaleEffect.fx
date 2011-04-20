#define LUMA1 (16.0/255.0)
#define LUMA2 (255.0/219.0)

/// <summary>Chrominance</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float chroma : register(c0);

sampler2D implicitInput : register(s0);

float4 fixLuma(float4 color)
{
	return (color - LUMA1) * LUMA2;
}

float4 setChroma(float4 color)
{
	// Calculate Luma according to Rec. 601
	// @see: http://en.wikipedia.org/wiki/Luma_(video)
	float luma = dot(color.rgb, float3(0.3, 0.59, 0.11));

	return float4(mul(color.rgb, chroma), log10((1 - luma) * 10));
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 color = fixLuma(tex2D(implicitInput, uv));
	return setChroma(color);
}