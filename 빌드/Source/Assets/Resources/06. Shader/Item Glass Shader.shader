Shader "Custom/Item Glass Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		
		_alphaIntensity ("Alpha intensity", Range(0,1)) = 1
		_specIntensity ("Specular intensity", Range(0,1)) = 1

   [HDR]_specColor("Specular Color", Color) = (0,0,0,0)
		_rimPow("Rim Pow", float) = 0
		_specPow("Spec Pow", float) = 0

		_smoothMin ("smoothMin" , Range(0,1)) = 0
		_smoothMax ("smoothMax" , Range(0,1)) = 1
		
		_specSmoothMin("Specular smoothMin" , Range(0,1)) = 0
		_specSmoothMax("Specular smoothMax" , Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
  
        #pragma surface surf Glass alpha:fade noambient



        sampler2D _MainTex;
		float4 _Color, _specColor;
		float _rimPow, _specPow, _alphaIntensity, _specIntensity, _smoothMin, _smoothMax;
		float _specSmoothMin, _specSmoothMax;

        struct Input
        {
            float2 uv_MainTex;
        };

      
       


        void surf (Input IN, inout SurfaceOutput o)
        {
           
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = 0;
			o.Specular = c.r;
        }

		float4 LightingGlass(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {

			float ndotL = dot(s.Normal, lightDir) * 0.5 + 0.5;
			float ndotV = dot(s.Normal, viewDir) ;

		/*	float3 H = normalize(lightDir + viewDir);
			float spec = saturate(dot(s.Normal, H));*/

			float spec;
			spec = ndotV * 0.5 + 0.5;

			spec = pow(spec, _specPow);
			ndotV = pow(ndotV, _rimPow);

			float3 rim;
			//rim = saturate(pow(1 - ndotV, _rimPow) * _rimColor);

			rim = smoothstep(_smoothMin, _smoothMax, ndotV) ;
			
		

			spec = smoothstep(_specSmoothMin, _specSmoothMax, spec) * _specIntensity;

			s.Alpha = _alphaIntensity * rim;

			

			float4 final;

			final.rgb = _specColor ;
			final.a = s.Alpha + spec;

			return final; 
		}


        ENDCG
    }
    FallBack "Diffuse"
}
