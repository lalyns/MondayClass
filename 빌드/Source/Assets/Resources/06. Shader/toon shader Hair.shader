Shader "Custom/toon shader Hair"
{
	Properties
	{

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_ShadowMask("Mask", 2D) = "" {}
		_ShadowColor("ShadowColor", Color) = (1,1,1,1)
		_SpecCol("SpecColor", Color) = (1,1,1,1)
		_LineWidth("Line Width", Range(0,1)) = 0
	[Toggle]_LineEmission("Line Emission" ,  float) = 0
	[HDR]_LineColor("Line Color",Color) = (1,1,1,1)
		_ShadowWidth("shadow", Range(0,1)) = 0.5
		_AmbientWidth("Ambient", Range(0,1)) = 0.5

	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		cull front

		//1st pass 

		CGPROGRAM
		#pragma surface surf Blackline vertex:vert noshadow noambient
		

		float _LineWidth, _LineEmission;
		float4 _LineColor;
		void vert(inout appdata_full v) {

		v.vertex.xyz = v.vertex.xyz + v.normal.xyz * _LineWidth / v.color.g;
	}


	struct Input
	{
		float4 color:COLOR;

	};

	void surf(Input IN, inout SurfaceOutput o) {
	}


	float4 LightingBlackline(SurfaceOutput s, float3 lightDir, float atten) {

		float4 LineCol = _LineColor * _LineEmission;


		float4 final;
		final.rgb = LineCol;
		final.a = s.Alpha;
		;

		return final;
		;
	}

	ENDCG

		cull back
		cull off
		//2nd pass

		CGPROGRAM
		#pragma surface surf toon 
		#pragma target 3.0


	sampler2D _MainTex, _ShadowMask;
	float _ShadowWidth, _AmbientWidth;
	float4 _ShadowColor, _SpecCol;
	
	float2 uv_ShadowMask;

	struct Input
	{
		float2 uv_MainTex, uv_ShadowMask;
		float4 color:COLOR;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		float4 albedoMap = tex2D(_MainTex, IN.uv_MainTex);
		float4 ShadowMask = tex2D(_ShadowMask, IN.uv_ShadowMask);
		

		o.Albedo = albedoMap.rgb;
		o.Gloss = ShadowMask.r;
		o.Specular = ShadowMask.g;
	}

	float4 Lightingtoon(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {


		
		float ndotL = dot(s.Normal, lightDir) * _ShadowWidth + _AmbientWidth;
		

		ndotL = ndotL * atten;
		
		float4 Color = _ShadowColor;
		float4 SColor = _SpecCol;

		

		if (ndotL < s.Gloss)
		{
			s.Albedo *= Color;
		}

		if (ndotL > s.Specular)
		{
			s.Albedo += SColor  ;
		}

		
	
		float4 final;

		final.rgb = s.Albedo  * _LightColor0.rgb;
		final.a = s.Alpha;

		return final;

	}
	ENDCG
	}
		FallBack "Diffuse"


}
