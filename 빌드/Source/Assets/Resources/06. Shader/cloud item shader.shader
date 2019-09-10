Shader "Custom/cloud item shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
   [HDR]_EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _shadowColor ("Shadow Color", Color) = (1,1,1,1)
   [HDR]_specColor ("Specular Color", Color) = (0,0,0,0)
        _rimColor ("Rim Color", Color) = (0,0,0,0)
        _rimPow ("Rim Pow", float) = 0
        _specPow ("Specular Pow", float) = 0.1


		_MainTex ("Albedo", 2D ) = "white" {}

		_smoothMin ("smoothMin" , Range(0,1)) = 0
		_smoothMax ("smoothMax" , Range(0,1)) = 1
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
       
        #pragma surface surf Item noambient noshadow

        sampler2D _MainTex;
		float4 _shadowColor, _Color, _rimColor, _EmissionColor, _specColor;
		float _smoothMin, _smoothMax, _rimPow, _specPow, _specOn;

        struct Input
        {
            float2 uv_MainTex;
        };

  
    
       
        void surf (Input IN, inout SurfaceOutput o)
        {
         
			float4 albedoMap = tex2D(_MainTex, IN.uv_MainTex);
			

			o.Albedo = albedoMap.rgb;
			o.Emission = albedoMap * _EmissionColor;
          
        }



		float4 LightingItem(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {

			float ndotL = dot(s.Normal, lightDir) * 0.8 + 0.2;
			float ndotV = dot(s.Normal, viewDir);

			float3 H = normalize(lightDir + viewDir);
			float shade = saturate(dot(s.Normal, H));
			shade = pow(shade, 4);

			float3 rim;
			rim = saturate(pow(1 - ndotV, _rimPow) * _rimColor);


			float3 shadowCol;
			shadowCol = _shadowColor;

			float3 shadow;
			shadow = smoothstep(_smoothMin, _smoothMax, 1 - shade) ;
			
			float3 spec;
			spec = saturate(pow(ndotV, _specPow)) * _specColor;
			
			
			
			float3 finalCol;
			finalCol = lerp(_Color, shadowCol, shadow) * s.Albedo;
			
			

			float4 final;

			final.rgb = (finalCol * _LightColor0.rgb  * atten  + spec) + rim ;
			final.a = s.Alpha;

			return final;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
