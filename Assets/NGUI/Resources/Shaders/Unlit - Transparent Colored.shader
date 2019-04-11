// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Colored"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : SV_Target
			{
				//return tex2D(_MainTex, IN.texcoord) * IN.color;
				half4 col = tex2D(_MainTex, IN.texcoord) * IN.color;

				// atlas : Editor GUI and Legacy GUI (=> wrong alpha)
				//col.rgb = pow(col.rgb + 0.055, 2.4) / 1.13711896582;
				//col.a = pow(col.a, 2.2);

				// atlas : Editor GUI and Legacy GUI (=> good alpha)
				//col.rgb = pow(col.rgb + 0.055, 2.4) / 1.13711896582;
				//col.a = pow(col.a - 0.02, 0.6) * 1.07;

				// atlas : Sprite (2D and UI) (=> good alpha & good color)
				//col.a = pow(col.a - 0.02, 0.6) * 1.07;
				/*
				if (col.a > 0)
				{
					//col.a = (pow(col.a + 0.07, 0.7) - 0.02) * 1.00;
					//col.a = (pow(col.a + 0.085, 0.8) * 1.092 - 0.02) * 0.935;
					//col.a = (pow(col.a - 0.02, 0.6) - 0.06)* 1.2;
					col.a = (pow(col.a - 0.02, 0.6) - 0.06)* 1.25;
					if (col.a > 0.96)
					{
						//if (col.a > 1.01)
						//	col.a = col.a - 0.0942;
						//else
						//	col.a = col.a - 0.038;
					}
				}
				*/

				if (col.a > 0)
				{
					col.a = pow(col.a - 0.02, 0.6) * 1.07;
				}

				return col;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
