Shader "Custom/DepthRender" {
	Properties{
	 _MainTex("Albedo (RGB)", 2D) = "white" {}
	 _Amount("DepthPower",Range(1,5)) = 1.0

	}
		SubShader{
		pass
		{
		 CGPROGRAM
		 #pragma vertex vert_img
		 #pragma fragment frag
		 #pragma fragmentoption ARB_precision_hint_fastest
		 #include "UnityCG.cginc"

		 uniform sampler2D _MainTex;
		 fixed _Amount;
		 sampler2D _CameraDepthTexture;  //카메라 뎁스 텍스쳐를 위해 추가 하였습니다.

		 fixed4 frag(v2f_img i) :COLOR
		 {
		  float d = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv.xy)); //카메라 뎁스 텍스쳐를 받아 와서 적용 합니다. 
		  d = pow(Linear01Depth(d),_Amount);  //값이 0~1범위 내에 있는지 확인. 0~1범위에 있는 중간 값의 위치를 제어할 수 있다.

		  return d;
		 }
		 ENDCG
		}

	 }
}
