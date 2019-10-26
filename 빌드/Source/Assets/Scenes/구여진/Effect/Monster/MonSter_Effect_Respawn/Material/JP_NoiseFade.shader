Shader "jpshader/JP_NoiseFade" {
	Properties {
		[HDR]_Color("기본색" , color) = (1,1,1,1)
		_MainTex ("기본 텍스쳐(RGB)", 2D) = "white" {}
		_EmitTex("셀프 일루미네이션 텍스쳐" , 2D ) =  "black" {} 
		_BumpTex("범프맵" , 2D ) = "blue" {} 
		_NoiseTex ("알파 텍스쳐 (R)", 2D) = "white"{}
		[Toggle]_Metallic("Metallic" ,float) = 0
		_Smoothness("Smoothness", Range(0,1) ) = 0
		[HideInInspector]_Cutoff ("알파테스트", float)= 0.5
		_Hide ("노이즈 한계치(1 ~ -1)_Hide" , float) = 1 
		[HDR]_AlphaColor ("알파 경계선 칼라_AlphaColor" , color ) = (1,1,1,1)
	}
	SubShader {
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "ForceNoShadowCasting" ="true" "RenderType"="TransparentCutout"}
//		LOD 200
		cull off
		
		CGPROGRAM
		#pragma surface surf Standard alphatest:_Cutoff  keepalpha
		#pragma target 3.0

		float4 _Color;
		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _EmitTex;
		sampler2D _BumpTex;
		float _Metallic;
		float _Hide;
		float _Smoothness;
		float4 _AlphaColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmitTex;
			float2 uv_NoiseTex;
			float2 uv_BumpTex;
			float4 color: COLOR;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 d = tex2D (_NoiseTex, IN.uv_NoiseTex);
			half4 emittex = tex2D (_EmitTex, IN.uv_EmitTex);
			o.Normal  = UnpackNormal(tex2D (_BumpTex, IN.uv_BumpTex));
			o.Albedo = c.rgb * _Color.rgb * IN.color.rgb;
			float alphalevel = d.r + _Hide;
			o.Alpha = alphalevel * IN.color.a;
			o.Smoothness = _Smoothness;
			o.Metallic = _Metallic;
			o.Emission = emittex.rgb + (step (0.48, 1-alphalevel)*_AlphaColor.rgb*3) ;
//			o.Emission = step (0.48, 1-alphalevel);
		}
		ENDCG
	} 
	FallBack "Legacy Shaders/Transparent/Cutout/Diffuse"
}
