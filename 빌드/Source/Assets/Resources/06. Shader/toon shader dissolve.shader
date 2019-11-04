Shader "Custom/toon shader dissolve"
{
    Properties
	{

	_MainTex("Albedo (RGB)", 2D) = "white" {}

	_HitTex("Albedo(RGB)",2D) = "white"{}

	[HDR]_EmissionCol("EmissionColor", Color) = (1,1,1,1)
	_EmissionMask("Emission Mask", 2D) = "black" {}
	_ShadowMask("Mask", 2D) = "" {}
	_ShadowColor("ShadowColor", Color) = (1,1,1,1)
	_LineWidth("Line Width",  Range(0,0.1)) = 0.002
	[Toggle]_LineEmission("Line Emission" ,  float) = 0
	[HDR]_LineColor("Line Color",Color) = (1,1,1,1)
	_ShadowWidth("shadow", Range(0,1)) = 0.5
	_AmbientWidth("Ambient", Range(0,1)) = 0.5

	[Space(50)][Header(Dissolve)]_DissolveTex("Dissolve Map", 2D) = "white" {}
	[HDR]_DissolveEdgeColor("Dissolve Edge Color", Color) = (1,1,1,0)
	_DissolveIntensity("Dissolve Intensity", Range(0.0, 1.0)) = 0
	_DissolveEdgeRange("Dissolve Edge Range", Range(0.0, 1.0)) = 0.1
	[Toggle]_DissolveEdgeMultiplier("Dissolve Edge Multiplier", float) = 8
	//_DissolveEdgeMultiplier("Dissolve Edge Multiplier", Float) = 8
		  
	[HDR]_HitColor("HitColor", Color) = (1,1,1,1)
	[Toggle]_Hittrigger("Hittrigger" , float ) = 1

		

	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			cull front

			//1st pass 

			CGPROGRAM
			#pragma surface surf Blackline vertex:vert noshadow noambient


			sampler2D _MainTex,_DissolveTex;
			float _LineWidth, _LineEmission;
			float4 _LineColor;
			uniform float4 _DissolveEdgeColor;
			uniform float _DissolveEdgeRange;
			uniform float _DissolveIntensity;
			uniform float _DissolveEdgeMultiplier;
			float edgeRamp ,_Hittrigger;
			float4 _HitColor;

			void vert(inout appdata_full v) {

			v.vertex.xyz = v.vertex.xyz + v.normal.xyz * _LineWidth / v.color.g;
		
		}


		struct Input
		{
			float4 color:COLOR;
			float2 uv_MainTex;
			float2 uv_DissolveTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {

			float4 dissolveColor = tex2D(_DissolveTex, IN.uv_DissolveTex);
			float dissolveClip = dissolveColor.r - _DissolveIntensity;
			edgeRamp = max(0, _DissolveEdgeRange - dissolveClip);
			clip(dissolveClip);

		}


		 float4 LightingBlackline(SurfaceOutput s, float3 lightDir, float atten) {


			 


			 

			
			
			 float4 LineCol = _LineColor * _LineEmission;


			 float4 final;
			 final.rgb = LineCol;
			 final.a = s.Alpha;
;			 float4 finallerp = lerp(s.Alpha, _DissolveEdgeColor, min(1, edgeRamp * _DissolveEdgeMultiplier*8));


				return final;
		}

		ENDCG

		cull back
		cull off

			//2nd pass

			CGPROGRAM
			#pragma surface surf toon addshadow
			#pragma target 3.0


			sampler2D _MainTex, _ShadowMask, _EmissionMask, _DissolveTex, _HitTex;
			float _ShadowWidth, _AmbientWidth;
			float4 _ShadowColor, _EmissionCol;
			float2 uv_ShadowMask;
			uniform float4 _DissolveEdgeColor;
			uniform float _DissolveEdgeRange;
			uniform float _DissolveIntensity;
			uniform float _DissolveEdgeMultiplier;
			float edgeRamp , _Hittrigger;
			float4 _HitColor;

			struct Input
			{
				float2 uv_MainTex, uv_ShadowMask, uv_EmissionMask;
				float4 color:COLOR;
				float2 uv_DissolveTex;
			};
			void surf(Input IN, inout SurfaceOutput o)
			{
				float4 albedoMap = tex2D(_MainTex, IN.uv_MainTex) ;
				float4 EmissionMask = tex2D(_EmissionMask, IN.uv_EmissionMask) ;
				float4 ShadowMask = tex2D(_ShadowMask, IN.uv_ShadowMask);
				//------------------------Dissolve-----------------------------------------
				float4 dissolveColor = tex2D(_DissolveTex, IN.uv_DissolveTex);
				float dissolveClip = dissolveColor.r - _DissolveIntensity;
				edgeRamp = max(0, _DissolveEdgeRange - dissolveClip);
				clip(dissolveClip);


				


				//o.Albedo = albedoMap.rgb ;
				o.Albedo = lerp(albedoMap, _HitColor, _Hittrigger);
				o.Emission = (albedoMap.rgb * EmissionMask ) * _EmissionCol;
				o.Specular = ShadowMask.r;
			}

			float4 Lightingtoon(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {
				
			
			float ndotL = saturate(dot(s.Normal, lightDir) * _ShadowWidth + _AmbientWidth) * atten;

			//float Shadow = step(0.1, 0.9, ndotL);
			float Shadow = step(0.5, ndotL);
		
					
			//Shadow = Shadow * atten;
			ndotL = ndotL * atten ;

			float3 Color = _ShadowColor ;

			if (ndotL < s.Specular)
			{
				s.Albedo *= Color ;
			}


			float4 final;
			final.rgb = s.Albedo * _LightColor0.rgb * (atten * 1);
			final.a = s.Alpha ;


			float4 finallerp = lerp(final, _DissolveEdgeColor, min(1, edgeRamp * _DissolveEdgeMultiplier*8));
			//float4 realfinal = lerp(finallerp, _HitColor , _Hittrigger);
			

			return finallerp;

				}
		ENDCG
	}
    FallBack "Diffuse"

}
