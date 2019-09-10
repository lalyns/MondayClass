Shader "Custom/cloud item Rainbow shader"
{
    Properties
    {
        _R ("Red", Color) = (1,1,1,1)
        _O ("Orange", Color) = (1,1,1,1)
        _Y ("Yellow", Color) = (1,1,1,1)
        _G ("Green", Color) = (1,1,1,1)
        _B ("Blue", Color) = (1,1,1,1)
        _I ("Indigo Blue", Color) = (1,1,1,1)
        _P ("Purple", Color) = (1,1,1,1)
   
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		cull off
        CGPROGRAM
       
        #pragma surface surf cloud noambient  noshadow

        sampler2D _MainTex;
		float4 _R, _O, _G, _Y, _B, _I, _P;
		

        struct Input
        {
            float2 uv_MainTex;
        };

  
    
       
        void surf (Input IN, inout SurfaceOutput o)
        {
         
			float3 Red;
			Red = step(0.84, IN.uv_MainTex.y) * _R;
			
			float3 Orange;
			Orange = step(0.69, IN.uv_MainTex.y) * step(IN.uv_MainTex.y, 0.84) * _O;
			
			float3 Green;
			Green = step(0.53, IN.uv_MainTex.y) * step(IN.uv_MainTex.y, 0.69) * _G;
			
			float3 Blue;
			Blue = step(0.36, IN.uv_MainTex.y) * step(IN.uv_MainTex.y, 0.53) * _B;
			
			float3 IndigoBlue;
			IndigoBlue = step(0.19, IN.uv_MainTex.y) * step(IN.uv_MainTex.y, 0.36) * _I;
			
			float3 Purple;
			Purple = step(IN.uv_MainTex.y, 0.19) * _P;
			
			float3 rainbowStep;
			rainbowStep = Red + Orange + Green + Blue + IndigoBlue + Purple;
           
		o.Emission = rainbowStep;
        }

		//* step(IN.uv_MainTex.y, 0.7) + step(0.7, Orange)

		float4 Lightingcloud(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {

	

			float4 final;

			final.rgb = s.Emission * 0.3;
			final.a = s.Alpha;

			return final;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
