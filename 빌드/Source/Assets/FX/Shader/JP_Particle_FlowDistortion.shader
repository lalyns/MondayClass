// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "jpshader/Particle/JP_FlowDistortion" {
	Properties {
	
		[HDR]_Color("MainColor",color) = (1,1,1,1)
		_MainTex ("첫번째 텍스쳐", 2D) = "white" {}
		_tex1U("첫번째 텍스쳐 가로 스크롤 속도",float) = 0 
		_tex1V("첫번째 텍스쳐 세로 스크롤 속도",float) = 0
		_rot1("첫번째 텍스쳐 회전 속도",float) = 0
		
		[Gamma]_MainTex2("구기는 텍스쳐(Linear).", 2D) = "black" {}
		_tex2pow("텍스쳐의 구김영향력", Range(0,1)) = 0
		_tex2U("구기는 텍스쳐 가로 스크롤 속도",float) = 0 
		_tex2V("구기는 텍스쳐 세로 스크롤 속도",float) = 0
		_rot2("구기는 텍스쳐 회전 속도",float) = 0

		[Toggle]_CustomVertexSt("파티클 커스텀 데이터를 사용한다" , float) = 0
		
		[Space(50)]
		[Header(Add_ SrcAlpha_One)]
		[Header(Add_ One_One)]
		[Header(AlphaBlend_ SrcAlpha_OneMinusSrcAlpha)]
		[Header(Multiply_ DstColor_Zero)]
		
		
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend mode", Float) = 1
		 [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend mode", Float) = 1
		 [HideInInspector][Toggle] _Zwrite("Zwrite", float) = 0	

		
	}
	
	
	Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend [_SrcBlend] [_DstBlend]

	ZWrite [_Zwrite]

	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off 
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			fixed4 _TintColor;
			sampler2D _MainTex;
			sampler2D _MainTex2;

			float _tex1U;
			float _tex1V;

			float _rot1;
			float _rot2;

			float _tex2U;
			float _tex2V;
			float4 _Color;
			float _tex2pow;
			
			float4 _MainTex_ST;
			float4 _MainTex2_ST;

			float _CustomVertexSt;
			
			float2 rotateUVs(float2 Texcoords, float2 center, float theta) { 
			 // compute sin and cos for this angle 
			 float2 sc; 
			 sincos( (theta/180.0f*3.14159f), sc.x, sc.y ); 
			// pi to dgree
			//sincos(x,s,c) : sin(x)와 cos(x)를 동시에 s, c로 리턴한다. 여기서 s, c는 x와 동일한 차원의 타입이어야 한다.
			// move the rotation center to the origin : 중점이동 (center는 기초값을 0.5로 하면 중심이 되것지)
			 float2 uv = Texcoords - center;  
			  
			 // rotate the uv : 기본 UV 좌표와의 dot연산 
			 float2 rotateduv; 
			 rotateduv.x = dot( uv, float2( sc.y, -sc.x ) ); 
			 rotateduv.y = dot( uv, sc.xy );  
			  
			 // move the uv's back to the correct place 
			 rotateduv += center; 
			  
			 return rotateduv; 
			} 
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				float4 texcoord2 : TEXCOORD1;
				float4 texcoord3 : TEXCOORD2;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD1;
				float4 customdata1 : TEXCOORD2;
				float4 customdata2 : TEXCOORD3;
	
	
			};
			
			

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
				o.texcoord2 = TRANSFORM_TEX(v.texcoord.xy,_MainTex2);
				o.customdata1 = v.texcoord2;
				o.customdata2 = v.texcoord3;

				return o;
			}


			
			fixed4 frag (v2f i) : SV_Target
			{

				float4 customdata1;
				float4 customdata2;
					
				if (_CustomVertexSt == 0){
					customdata1 = 0;
					customdata2 = 0;
				}
				else{
					customdata1 = i.customdata1;
					customdata2 = i.customdata2;
				}






				//distortion texture
				//rotate UV 
				float2 SDUVRotate = rotateUVs(i.texcoord2,0.5,(_Time.y * _rot2 + customdata2.w) * 10);
				//rotate + Scroll UV
				float2 RotateScrollUV2 = float2(SDUVRotate.x-_Time.y* _tex2U, SDUVRotate.y-_Time.y* _tex2V);


				half4 e = tex2D (_MainTex2, RotateScrollUV2 + float2(customdata2.y,customdata2.z));
				e = e *2-1;
				e *= _tex2pow + customdata2.x;




				//rotate UV 
				float2 firstUVRotate = rotateUVs(i.texcoord,0.5,(_Time.y * _rot1+ customdata1.w)*10);

				//rotate + Scroll UV
				float2 RotateScrollUV = float2(firstUVRotate.x+e.r-_Time.y* _tex1U, firstUVRotate.y+e.r-_Time.y* _tex1V); 
				//RotateScrollUV = i.texcoord+e.r; 
		
				half4 c = tex2D (_MainTex, RotateScrollUV + float2(customdata1.y,customdata1.z)) * ( _Color  + customdata1.x);
				return c * i.color;
			}
			ENDCG 
		}
	}	
}
}
