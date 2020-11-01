Shader "Custom/ToonOutline"
{
	Properties{
		_Color("Albedo (RGB)", Color) = (0, 0, 0, 1)
		_MainTex("Base (RGB)", 2D) = "white" {}

		_ToonEffect("Toon Effect",Range(0,1)) = 0.5
		_Steps("Steps of toon",Range(0,9)) = 3

		[Outline]
		_OutlineColor("color", Color) = (0, 0, 0, 1)
		_Outline("Thick",Range(0,0.1)) = 0.02

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
				struct v2f {
					float4 pos:SV_POSITION;
				};

				v2f vert(appdata_full v) {
					v2f o;
					float3 dir = normalize(v.vertex.xyz);
					float3 vnormal = v.normal;
					float D = dot(dir,vnormal);
					dir = dir * sign(D);
					dir = dir * 0.5 + vnormal * (1 - 0.5);
					v.vertex.xyz += dir * _Outline;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				float4 frag(v2f i) :COLOR
				{
					float4 c = _OutlineColor;
					return c;
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

				float4 _LightColor0;
				float _Steps;

				float4 _Color;
				float _ToonEffect;
				sampler2D _MainTex;
				sampler2D _ToonRampTex;
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
					float3 N = normalize(i.normal);
					float3 viewDir = normalize(i.viewDir);
					float3 lightDir = normalize(i.lightDir);

					float diffuse = max(0,dot(N,i.lightDir));
					diffuse = (diffuse + 1) / 2;
					diffuse = smoothstep(0,1,diffuse);

					float toon = floor(diffuse*_Steps) / _Steps;
					diffuse = lerp(diffuse,toon,_ToonEffect);

					c = c *_LightColor0 * diffuse;
					return c;
				}
				ENDCG
			}
		}
}
