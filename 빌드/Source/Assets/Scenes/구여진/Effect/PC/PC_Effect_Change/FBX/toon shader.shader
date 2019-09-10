Shader "Custom/toon shader"
{
    Properties
    {
     
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
	[HDR]_EmissionCol ("EmissionColor", Color) = (1,1,1,1)
		_EmissionMask ("Emission Mask", 2D) = "black" {}
        _ShadowMask ("Mask", 2D) = "" {}
        _ShadowColor ("ShadowColor", Color) = (1,1,1,1)
		_LineWidth("Line Width", Range(0,1)) = 0
		_ShadowWidth("shadow", Range(0,1)) = 0.5
		_AmbientWidth("Ambient", Range(0,1)) = 0.5

	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" } 

			cull front

			//1st pass 

			CGPROGRAM
			#pragma surface surf Blackline vertex:vert noshadow noambient

			float _LineWidth;

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
				return float4(0, 0, 0, 1);
		}

		ENDCG

		cull back
		cull off
			//2nd pass

			CGPROGRAM
			#pragma surface surf toon 
			#pragma target 3.0


			sampler2D _MainTex, _ShadowMask, _EmissionMask;
			float _ShadowWidth, _AmbientWidth;
			float4 _ShadowColor, _EmissionCol;
			float2 uv_ShadowMask;

			struct Input
			{
				float2 uv_MainTex, uv_ShadowMask, uv_EmissionMask;
				float4 color:COLOR;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				float4 albedoMap = tex2D(_MainTex, IN.uv_MainTex) ;
				float4 EmissionMask = tex2D(_EmissionMask, IN.uv_EmissionMask) ;
				float4 ShadowMask = tex2D(_ShadowMask, IN.uv_ShadowMask);



				o.Albedo = albedoMap.rgb ;
				o.Emission = (albedoMap.rgb * EmissionMask ) * _EmissionCol;
				o.Specular = ShadowMask.r;
			}

				float4 Lightingtoon(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {

				float ndotL = dot(s.Normal, lightDir) * _ShadowWidth + _AmbientWidth;

				float3 H = normalize(lightDir + viewDir);
				float spec = saturate(dot(s.Normal, H));
				spec = pow(spec, 200);
					
				ndotL = ndotL * atten;

				float3 Color = _ShadowColor ;
					
				if (ndotL < s.Specular)
				{
					s.Albedo *= Color ;
				}

				
					float4 final;
					final.rgb = s.Albedo * _LightColor0.rgb ;
					final.a = s.Alpha ;

					return final;

					}
			ENDCG
	}
    FallBack "Diffuse"

}
