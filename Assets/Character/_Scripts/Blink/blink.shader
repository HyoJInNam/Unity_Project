// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/blink"
{
    Properties
    {
	}
	SubShader
	{
		Tags {"Queue" = "Background"  "IgnoreProjector" = "True"}
		LOD 100
		
		ZWrite On
		
		Pass {
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 color;
			float amoothness;
			float curvature;
		
			struct v2f {
				  float4 pos : SV_POSITION;
				  fixed4 color : COLOR;
			};
		
			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = step(_Scale, v.texcoord.y) * lerp(0, color, v.texcoord.y);
				o.color = lerp(color, 0, v.texcoord.y);
				o.color = half4(v.vertex.y, 0, 0, 1);

				return o;
			}


			float4 frag(v2f i) : COLOR{
				float4 c = i.color;
				c.a = 1;
				return c;
			}
			ENDCG
		}
	}
    FallBack "Diffuse"
}
