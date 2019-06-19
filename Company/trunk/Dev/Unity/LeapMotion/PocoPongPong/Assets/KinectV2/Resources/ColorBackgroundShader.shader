Shader "Kinect/ColorBackgroundShader" {
    Properties {
//        _BodyTex ("Body (RGB)", 2D) = "white" {}
        _ColorTex ("Color (RGB)", 2D) = "white" {}
    }
    
	SubShader {
		Pass {
			ZWrite On Cull Off
			Fog { Mode off }
		
			CGPROGRAM
			#pragma target 5.0
			#pragma enable_d3d11_debug_symbols

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

//			uniform sampler2D _BodyTex;
			uniform sampler2D _ColorTex;
			
			uniform float _ColorResX;
			uniform float _ColorResY;
			uniform float _DepthResX;
			uniform float _DepthResY;

			StructuredBuffer<float2> _DepthCoords;
			StructuredBuffer<float> _DepthBuffer;

			struct v2f {
				float4 pos : SV_POSITION;
			    float2 uv : TEXCOORD0;
			    float4 projPos : TEXCOORD1;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				o.projPos = ComputeScreenPos(o.pos);
				
				return o;
			}

			void frag (v2f i, out half4 c:COLOR, out float d:DEPTH)
			{
				int cx = (int)(i.uv.x * _ColorResX);
				int cy = (int)(i.uv.y * _ColorResY);
				int ci = (int)(cx + cy * _ColorResX);
				
				if (!isinf(_DepthCoords[ci].x) && !isinf(_DepthCoords[ci].y))
				{
					float di_index, di_length;
					di_index = _DepthCoords[ci].x + (_DepthCoords[ci].y * _DepthResX);
					di_length = _DepthResX * _DepthResY;
				
					if(di_index >= 0 && di_index < di_length)
					{
//						float2 di_uv;
//						di_uv.x = _DepthCoords[ci].x / _DepthResX;
//						di_uv.y = _DepthCoords[ci].y / _DepthResY;
//
//						float player = tex2D(_BodyTex, di_uv).r;
//						if (player != 0)
						{
							float depth = _DepthBuffer[di_index] / 1000;
							
							c = tex2D(_ColorTex, i.uv);
							d = 0; // depth / i.projPos.w;  // i.projPos.z
							
							return;
						}
					}
				}
				
				c = tex2D(_ColorTex, i.uv);
				d = 1;
			}

			ENDCG
		}
	}

	Fallback Off
}