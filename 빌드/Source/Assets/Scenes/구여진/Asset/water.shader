Shader "Custom/water"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normal",2D) = "Bump" {}
		_Cube ("Cubemap",CUBE) = ""{}
		_SpecColor("Speular",color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 200

		GrabPass{}

        CGPROGRAM
        #pragma surface surf BlinnPhong fullforwardshadows /*vertex:vert */  //noambient noshadow 
        #pragma target 3.0

		/*void vert(inout appdata_full v) {
		v.vertex.y += sin(abs(v.texcoord.x * 2 - 1 ) * 10 + _Time.y )*0.1;
		}*/


        sampler2D _MainTex,_BumpMap,_GrabTexture;
		samplerCUBE _Cube;
		//float4 _SpecColor;
        struct Input
        {
            float2 uv_MainTex ,uv_BumpMap;
			float3 worldRefl,viewDir;
			float4 screenPos;
			INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			//Normal -----------------------------
            fixed3 No1 = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap + _Time.x * 0.1)) ;
            fixed3 No2 = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap - _Time.x * 0.1)) ;
			o.Normal = (No1 + No2)/2;
			//GrabPass-----------------------------
			float3 scpo = IN.screenPos.xyz / (IN.screenPos.w+0.000001);
			fixed4 sc = tex2D(_GrabTexture, scpo.xy + o.Normal.r * 0.05);
			//o.Emission = sc;

			//Cube map------------------------------
			float4 cube = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal));

			//Rim ----------------------------------
			float rim = saturate (dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, 2);

			o.Gloss = 1;
			o.Specular = 1;

			o.Emission = lerp(sc , cube , rim);
           // o.Albedo = c.rgb;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
