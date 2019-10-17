Shader "Custom/clip_test1" {
	Properties {
		_intensity ("조절",Range(0,1)) = 1
		[HDR]_Color ("Color",Color) = (1,1,1,0)
		_MainTex ("Albedo 1", 2D) = "white" {}
		_MainTex2("Albedo 2", 2D) = "white" {}

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex, _MainTex2;
		float _intensity;
		float4 _Color;

		struct Input {
			float2 uv_MainTex, uv_MainTex2;
		};
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 d = tex2D(_MainTex2, IN.uv_MainTex2);

			

			float CCRC = c.r - _intensity;

			clip(CCRC);

			o.Albedo = d.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
