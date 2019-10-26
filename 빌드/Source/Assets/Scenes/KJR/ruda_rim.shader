Shader "Custom/ruda_rim"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent""Queue" = "Transparent" }
        LOD 200

		zwrite on
		ColorMask 0

        CGPROGRAM
        #pragma surface surf Lambert 
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			o.Alpha = 0.5;
        }

        ENDCG

		zwrite off

		CGPROGRAM
		#pragma surface surf Lambert	alpha:blend
		#pragma target 3.0

			sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = 0.5;
		}

		ENDCG
	
    }
    FallBack "Diffuse"
}
