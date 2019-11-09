Shader "Custom/Standard_OBJ"
{
    Properties
    {
	   [HDR]_HitColor("HitColor", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic",2D) = "white" {}
		_AOTex("ambiant" , 2D) = "white" {}
		_BumpMap("NormalMap" , 2D) = "Bump" {}
		_EmissionMap("EmissionMap",2D) = "white"
		[Toggle]_Hittrigger("Hittrigger" , float) = 1
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Metallic;
		sampler2D _AOTex;
		sampler2D _BumpMap;
		sampler2D _EmissionMap;
		float _Hittrigger;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_AOTex;
            float2 uv_Metallic;
            float2 uv_BumpMap;
            float2 uv_EmissionMap;
        };

        half _Glossiness;
        //half _Metallic;
        float4 _HitColor;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			float ao = tex2D(_AOTex, IN.uv_AOTex);
			float MT = tex2D(_Metallic, IN.uv_Metallic).r;
			float3 EM = tex2D(_EmissionMap, IN.uv_EmissionMap);
			float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            float3 ALBE = c.rgb;
            o.Albedo = lerp(ALBE, _HitColor, _Hittrigger);
            o.Metallic = MT;
            o.Smoothness = _Glossiness;
			o.Occlusion = ao;
            o.Alpha = c.a;
			o.Emission = lerp(EM, _HitColor, _Hittrigger);
			o.Normal = n;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
