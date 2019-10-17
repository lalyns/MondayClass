Shader "Unlit/mdp frg 1"
{
	Properties
	{
		_StepValue("Step Value" , Range(0,1)) = 0.5
		_Tilling("Tileling",Range(0,10)) = 1
		_RotValue("Rot Valule",float) = 1
		_RGBSplit("RGB Split", Range(0,1))= 1
		 _TraslateUV("Translate UV" , vector) = (0,0,0,0)
		_ScaleValue("ScaleValue", float) = 1
		_RotateValue("RotateValue",float) = 0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work

			#include "UnityCG.cginc"

			float _StepValue , _Tilling , _RotValue , _RGBSplit;
			float4 _TraslateUV;
			float _ScaleValue, _RotateValue;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv ;
                return o;
            }

			float2 TranslateUV(float2 uv, float2 translate)
			{
				float3x3 translateMatrix = float3x3 (1, 0, translate.x,
																			0, 1, translate.y,
																			0, 0, 1);
				uv = mul(translateMatrix, float3 (uv, 1));
				//uv +=  translate 이거랑 똑같음;
				return uv;
			}

			float2 ScaleUV(float2 uv, float scale) // , float2 offset)
			{
				float2x2 scaleMatrix = float2x2(scale, 0, 0, scale);
				uv -= (0.5);// + offset);
				uv = mul(scaleMatrix, uv);
				uv += (0.5);// + offset);
				return uv;
			}

			float2 RotateUV(float2 uv, float degrees)
			{
				const float Deg2Rad = UNITY_PI * 2 / 360;
				float rotationRadians = degrees * Deg2Rad;
				float c = cos(rotationRadians);
				float s = sin(rotationRadians);
				float2x2 rotateMatrix = float2x2(c, s, -s, c);
				uv -= 0.5;
				uv = mul(uv, rotateMatrix);
				uv += 0.5;
				return uv;
			}


            fixed4 frag (v2f i) : SV_Target
			{
				float4 col = float4(1,1,1,1);
				float2 rotateUV = RotateUV(i.uv, _RotValue);
				
				i.uv = TranslateUV(i.uv, float2(frac(_TraslateUV.x), frac(_TraslateUV.y)));
				i.uv = RotateUV(i.uv, _RotateValue);
				i.uv = ScaleUV(i.uv, _ScaleValue);


			  col.r =
					min( min(step (frac(rotateUV.x * _Tilling),1 - _StepValue) ,
									step(_StepValue, frac(rotateUV.x * _Tilling)))
						,min(		step (frac(rotateUV.y * _Tilling),1 - _StepValue) ,
									step(_StepValue, frac(rotateUV.y * _Tilling))) );

			  //=====================================================
		
			  col.g =
				  min(min(step(frac(rotateUV.x * _Tilling*(1-_RGBSplit) ), 1 - _StepValue),
					  step(_StepValue, frac(rotateUV.x * _Tilling*(1 - _RGBSplit))))
					  , min(step(frac(rotateUV.y * _Tilling*(1 - _RGBSplit)), 1 - _StepValue),
						  step(_StepValue, frac(rotateUV.y * _Tilling*(1 - _RGBSplit)))));
	
			  //=====================================================

			  col.b =
				  min(min(step(frac(rotateUV.x * _Tilling*(1 + _RGBSplit)), 1 - _StepValue),
					  step(_StepValue, frac(rotateUV.x * _Tilling*(1 + _RGBSplit))))
					  , min(step(frac(rotateUV.y * _Tilling*(1 + _RGBSplit)), 1 - _StepValue),
						  step(_StepValue, frac(rotateUV.y * _Tilling*(1 + _RGBSplit)))));

			

			
			  return col;
            }
            ENDCG
        }
    }
}
