Shader "Unlit/Sky box"
{
    Properties
    {
        _cubeMap ("Sky box", CUBE) = "" {}
        _star ("Star Texture", 2D) = "" {}
		_Color ("Color", Color) = (1,1,1,1)
		[HDR]_starColor ("Star Color", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_Color3 ("Color3", Color) = (1,1,1,1)
		_StarsCutoff (" Star cutoff", Range(0, 1)) = 0.08
		_offsetCol2 ("Color2 Offset", Range(0, 10)) = 0.08
		_offsetCol3 ("Color3 Offset", Range(-10, 10)) = 0.08
		
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
				
            };

	samplerCUBE _cubeMap;
	sampler2D _star;
	float _StarsCutoff, _offsetCol2, _offsetCol3;
	float4 _Color, _Color2, _Color3, _starColor;

            struct v2f
            {
                float3 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
            };

            
           

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				
                return o;
            }

			float Random(float2 uv)
			{
				float random;
				random = sin(dot(uv, float2(1, 81123.348348)) );

				return random;

			}

            fixed4 frag (v2f i) : SV_Target
            {
				//float2 skyUV = i.worldPos.zx * 2 / i.worldPos.y - 12347.1241124;
				float2 skyUV = i.worldPos.zx * 2 / i.worldPos.y - 12347.12411241232;

				float4 Stars = tex2D(_star, skyUV) ;
				Stars = step(_StarsCutoff, Stars) * _starColor;

                float4 sky = texCUBE(_cubeMap, i.uv) + Stars;

				float4 col = _Color;
				float4 col2 = lerp(_Color2, col, saturate(i.uv.y + _offsetCol2) );
				float4 col3 = lerp(_Color3, col2, saturate(i.uv.y - _offsetCol3) );

				
				float4 final;
				final = sky + col3 + Stars;

                return final;
            }
            ENDCG
        }
    }
}
