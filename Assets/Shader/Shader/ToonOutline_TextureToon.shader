Shader "Custom/ToonOutline_TextureToon"
{
	Properties{
		[Header(Outline)]
		_OutlineColor("color", Color) = (0, 0, 0, 1)
		_Outline("Line",Range(0,0.1)) = 0.02
		_OutThick("Thick",Range(0,1)) = 0.5

		[Header(Toon)]
		_Color("Albedo (RGB)", Color) = (0, 0, 0, 1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Glossiness("Glossiness",Range(0,128)) = 20

		[Header(Ramp)]
		_RampTex("Texture (RGB)", 2D) = "white" {}
		_RampOverallPower("Overall Brightness",Range(0,1)) = 0.5
		_RampBrightPartPower("Bright Part Brightness",Range(0,1)) = 0.5

		[Header(Light)]
		_DirX("direction X",Range(-1,1)) = 0.5
		_DirY("direction Y",Range(-1,1)) = 0.5
		_DirZ("direction Z",Range(-1,1)) = 0.5

	}

	SubShader{
		pass {
			Tags{ "LightMode" = "Always" }
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _OutlineColor;
			float _Outline;
			float _OutThick;

			struct v2f {
				float4 pos:SV_POSITION;
			};

			v2f vert(appdata_full v) {
				v2f o;
				float3 dir = normalize(v.vertex.xyz);
				float3 vnormal = v.normal;
				float D = dot(dir,vnormal);
				dir = dir * sign(D);
				dir = dir * 0.5 + vnormal * (1 - _OutThick);
				v.vertex.xyz += dir * _Outline;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}

		pass {
			Tags{ "LightMode" = "ForwardBase" }
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _LightColor0;

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

			float4 _Color;
			float _DirX;
			float _DirY;
			float _DirZ;

			sampler2D _RampTex;
			float _RampOverallPower;
			float _RampBrightPartPower;
			
			float4 _SpecularColor;
			float _Glossiness;

			float4 frag(v2f i) :COLOR
			{
				float4 c = tex2D(_MainTex, i.uv) * _Color;
				float3 lightVector = float3(_DirX, _DirY, _DirZ);

				float uv = dot(i.normal, i.lightDir * lightVector) * 0.5 + 0.5;
				float4 ramp = tex2D(_RampTex, float2(uv, uv)) * _RampBrightPartPower + _RampOverallPower;

				c = c * _LightColor0 * ramp;
				return c;
			}
			ENDCG
		}


		Pass
		{
			Tags { "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos:SV_POSITION;
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
			};

			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}

			ENDCG
		}
	}
}
