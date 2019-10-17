Shader "Custom/Dissolve_test2" {
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_DissolveTexture("디졸브 텍스처" , 2D) = "white" {}
		_Amount("세기?" , Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex,_DissolveTexture;
		float _Amount;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float _Dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
			clip(_Dissolve_value - _Amount);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Emission = _Color * step(_Dissolve_value - _Amount, 0.05);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
