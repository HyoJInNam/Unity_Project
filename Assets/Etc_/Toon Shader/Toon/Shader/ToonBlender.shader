Shader "Custom/ToonBlender"
{
	Properties{

		_Color("Albedo (RGB)", Color) = (0, 0, 0, 1)
		_MainTex("Base Texture", 2D) = "white" {}
		_ToonEffect("Toon Effect",Range(0,1)) = 0.5
	}


	SubShader{
		pass {
			Tags { "RenderType" = "Always" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha:blend
			#include "UnityCG.cginc"

			float4 _LightColor0;
			float _Steps;

			float4 _Color;
			float _ToonEffect;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct v2f {
				float4 pos:SV_POSITION;
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float2 uv:TEXCOORD3;
			};

			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 c = tex2D(_MainTex, i.uv) * _Color;
				return c;
			}
			ENDCG
		}
	}
}
