// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Cubemap" {
Properties {
    _Tint ("Tint Color", Color) = (.5, .5, .5, .5)
    [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
    _Rotation ("Rotation", Range(0, 360)) = 0
    [NoScaleOffset] _Tex ("Cubemap   (HDR)", Cube) = "grey" {}
	
	_OffsetHorizon("Horizon Offset",  Range(-1, 1)) = 0


	[Header(Cubemap Settings)]
	_DaycubeMap("³· Å¥ºê¸Ê", CUBE) = ""{}
	_NightcubeMap("¹ã Å¥ºê¸Ê", CUBE) = ""{}

	[Header(Sun Settings)]
	_SunColor("Sun Color", Color) = (1,1,1,1)
	_SunRadius("Sun Radius",  Range(0, 2)) = 0.1

	[Header(Moon Settings)]
	_MoonColor("Moon Color", Color) = (1,1,1,1)
	_MoonRadius("Moon Radius",  Range(0, 2)) = 0.15
	_MoonOffset("Moon Crescent",  Range(-1, 1)) = -0.1

	[Header(Main Cloud Settings)]
	_BaseNoise("Base Noise", 2D) = "black" {}
	_CloudTint("Cloud color", Color) = (1,1,1,1)
	_BaseNoiseScale("Base Noise Scale",  Range(0, 1)) = 0.2
	_BaseNoisePower("Base Noise Power",  Range(0, 100)) = 0.2
	_Speed("Movement Speed",  Range(0, 10)) = 1.4
	_Brightness("Cloud Brightness",  Range(0, 10)) = 0.1
}

SubShader{
	Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
	

	Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0

		#include "UnityCG.cginc"

		sampler2D _BaseNoise;
        samplerCUBE _DaycubeMap, _NightcubeMap;
        half4 _Tint;
        half _Exposure;
        float _Rotation, _Brightness, _OffsetHorizon;
		float _SunRadius, _MoonRadius, _MoonOffset;
		float4 _SunColor, _MoonColor, _CloudTint;
		float _BaseNoiseScale, _BaseNoisePower, _Speed;


        struct appdata {
            float4 vertex : POSITION;
			float3 uv : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f {
			float3 uv : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
            float4 vertex : SV_POSITION;

            
        };

        v2f vert (appdata v)
        {
            v2f o;
           	o.uv = v.uv;
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
            o.vertex = UnityObjectToClipPos(v.vertex);
            
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
			//float horizon = abs((i.uv.y ) - _OffsetHorizon);

			// uv for the sky
			float2 skyUV = i.worldPos.xz / i.worldPos.y ;

			// moving clouds
			float baseNoise = tex2D(_BaseNoise, (skyUV - _Time.x  ) * _BaseNoiseScale).x * _CloudTint ;
			baseNoise = (1 - baseNoise ) * _Brightness ;

			float cloudsNegative = 1 - baseNoise;

			// sun
			float sun = distance(i.uv.xyz, _WorldSpaceLightPos0);
			float sunDisc = 1 - (sun / _SunRadius);
			sunDisc = saturate(sunDisc * 50);

			//moon
			float moon = distance(i.uv, -_WorldSpaceLightPos0);
			float crescentMoon = distance(float3(i.uv.x + _MoonOffset, i.uv.yz), -_WorldSpaceLightPos0);
			float crescentMoonDisc = 1 - (crescentMoon / _MoonRadius);
			crescentMoonDisc = saturate(crescentMoonDisc * 50);
			float moonDisc = 1 - (moon / _MoonRadius);
			moonDisc = saturate(moonDisc * 50);
			moonDisc = saturate(moonDisc - crescentMoonDisc);

			float3 sunAndMoon = (sunDisc * _SunColor) + (moonDisc * _MoonColor) * cloudsNegative;

			//Cube texture
			float4 Day = texCUBE(_DaycubeMap, i.uv) * baseNoise;
			float4 Night = texCUBE(_NightcubeMap, i.uv) * baseNoise;
			float3 skyGradients = lerp(Night, Day, saturate(_WorldSpaceLightPos0.y)) ;

            //half4 tex = texCUBE (_Tex, i.texcoord);
            //half3 c = DecodeHDR (tex, _Tex_HDR);
            //c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
            //c *= _Exposure;
				float3 final;
				final=  skyGradients * baseNoise + sunAndMoon;
				
            return float4(final, 1);
        }
        ENDCG
    }
}


Fallback Off

}
