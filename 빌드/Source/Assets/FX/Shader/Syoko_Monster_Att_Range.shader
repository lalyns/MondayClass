Shader "Custom/Syoko_Monster_Att_Range"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert noshadow alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
		float _RimPower;
		fixed4 _Color, _RimColor;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float rim = dot(o.Normal, IN.viewDir);
            o.Emission = c.rgb;
            o.Alpha = pow(1-rim,_RimPower);
        }

		/*float4 LightingSyoko(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{
			float3 Rim;
			float NdotV = dot(s.Normal, viewDir);
			Rim = pow(1 - abs(NdotV), _RimPower) * _RimColor;

			return float4(Rim, 1);
		}*/
        ENDCG
    }
    FallBack "Diffuse"
}
