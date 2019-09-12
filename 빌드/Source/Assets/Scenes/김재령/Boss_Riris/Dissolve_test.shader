Shader "Custom/Dissolve_test"
{
	Properties
	{
	  _MainTex("Texture", 2D) = "white" {}
	  _BumpMap("Normal Map", 2D) = "bump" {}
	  _BumpPower("Normal Power", float) = 1
	  _DissolveTex("Dissolve Map", 2D) = "white" {}
	  [HDR]_DissolveEdgeColor("Dissolve Edge Color", Color) = (1,1,1,0)
	  _DissolveIntensity("Dissolve Intensity", Range(0.0, 1.0)) = 0
	  _DissolveEdgeRange("Dissolve Edge Range", Range(0.0, 1.0)) = 0
	  _DissolveEdgeMultiplier("Dissolve Edge Multiplier", Float) = 1
	}

		SubShader
	  {
		Tags { "RenderType" = "Opaque" }
		//Cull Off

		CGPROGRAM
		#pragma surface surf Lambert addshadow

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_DissolveTex;
			float3 viewDir;
			float3 worldNormal;
		};

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _DissolveTex;

		//uniform으로 cup연산 후 gpu 처리
		uniform float _BumpPower;
		uniform float4 _DissolveEdgeColor;
		uniform float _DissolveEdgeRange;
		uniform float _DissolveIntensity;
		uniform float _DissolveEdgeMultiplier;

		void surf(Input IN, inout SurfaceOutput o)
		{
			//디졸브 텍스쳐
		  float4 dissolveColor = tex2D(_DissolveTex, IN.uv_DissolveTex);
		  //잘리기 위한 조합, 디졸브텍스처를 뺀다. 무엇을?  디졸브 강약 조절 프로퍼티를
		  float dissolveClip = dissolveColor.r - _DissolveIntensity;
		  //디졸브의 끝선의 길이, 짤리는 부분의 엣지의 길이. max 함수로 a와 b 값중 큰 값을 반환한다 (일정부분만 선처럼 만든다)
		  float edgeRamp = max(0, _DissolveEdgeRange - dissolveClip);

		  //잘리는 여부 O X 0보다 작으면 현재 픽셀을 버린다. (수치값이 오르면 오를 수록 그픽셀을 버린다)
		  clip(dissolveClip);

		  //디퓨즈 칼라 넣는곳
		  float4 texColor = tex2D(_MainTex, IN.uv_MainTex);

		  //최종 조합, 러프 함수를 사용한것은 일정 구간만 디졸브 엣지 칼라가 들어가야함, 마찬가지로 min 함수로 a와 b중 작은값을 반환
		  o.Albedo = lerp(texColor, _DissolveEdgeColor, min(1, edgeRamp * _DissolveEdgeMultiplier));

		  float3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		  normal.b /= _BumpPower;

		  o.Normal = normalize(normal);

		  
		}
		ENDCG
	  }
		  Fallback "Diffuse"
}