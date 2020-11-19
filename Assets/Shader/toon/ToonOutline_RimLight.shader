Shader "Custom/ToonOutline_RimLight"
{
	Properties{

		[Header(Outline)]
		_Outline("Thick of Outline",range(0,0.1)) = 0.02

		[Header(Toon)]
		_Color("Albedo (RGB)", color) = (1, 1, 1, 1)
		_MainTex("base texture", 2D) = "white" {}
		_ToonSteps("Steps of toon",range(1,9)) = 3
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold",range(0, 1)) = 0.1
		_SpecPower("Specular Power",range(0, 1)) = 0.5
		_FakeSpecPower("Fake Specular Power", range(0, 10)) = 0.5
	}

	SubShader{
		// first pass: outline
		pass {
			Tags{ "LightMode" = "Always" }
			Cull Front
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Outline;
			struct v2f {
				float4 pos:SV_POSITION;
			};

			// vertex shader for outline
			v2f vert(appdata_full v) {
				v2f o;

				float3 dir = normalize(v.vertex.xyz);
				float3 vnormal = v.normal;
				dir = dir * sign(dot(dir, vnormal));
				dir = dir * 0.5 + vnormal * (1 - 0.5);
				v.vertex.xyz += dir * _Outline;
				o.pos = UnityObjectToClipPos(v.vertex); // This is the equivalent of mul(UNITY_MATRIX_MVP, float4(pos, 1.0))
				return o;
			}

			float4 frag(v2f i) :COLOR
			{
				float4 c = 0;
				return c;
			}
			ENDCG
		}

		// second pass: diffuse,  toon, rim, specular, fake specular
		pass {
			Tags{ "LightMode" = "ForwardBase" }
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _LightColor0;

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _ToonSteps;
			float _RimAmount;
			float _RimThreshold;
			float _SpecPower;
			float _FakeSpecPower;

			struct v2f {
				float4 pos:SV_POSITION;
				float3 lightDir:TEXCOORD0;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float2 uv:TEXCOORD3;
			};

			// initialize variables like position, normal, light direction and view direction based on vertex
			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.lightDir = ObjSpaceLightDir(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); // macro, scales and offsets texture coordinates.

				return o;
			}
			
			//diffuse and toon, rim, specular, fake specular
			float4 frag(v2f i) :COLOR
			{
				float4 ambient = tex2D(_MainTex, i.uv);
				float3 viewDir = normalize(i.viewDir);
				float3 lightDir = normalize(i.lightDir);
				float3 n = normalize(i.normal);

				float atten = 1 / length(i.lightDir);
				float diffuse = smoothstep(0, 1, dot(n, i.lightDir)); // smooth it to range 0 -1
				float toon = floor(diffuse * atten * _ToonSteps) / _ToonSteps;

				// specular light
				float nDotL = dot(lightDir, n);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float nDotH = dot(n, halfVector);
				float specular = pow(smoothstep(0, 1, nDotL) * nDotH, _SpecPower * 100);
				specular = smoothstep(0.005, 0.01, specular);

				// fake specular light
				float viewL = saturate(dot(viewDir, n));
				float fakeSpec = pow(viewL, _FakeSpecPower * 100);
				fakeSpec = smoothstep(0.005, 0.01, fakeSpec);
				
				// rim light
				float rDot = 1 - dot(viewDir, n);
				float rim = rDot * pow(nDotL, _RimThreshold);
				rim = smoothstep(_RimAmount -0.01, _RimAmount + 0.01, rim);

				float light = _LightColor0 * (toon + specular + fakeSpec);
				return ambient * _Color * (ambient + light + rim);
			}
				ENDCG
		}

		// shadow
		pass{
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag  
			#pragma multi_compile_shadowcaster  
			#include "UnityCG.cginc" 

			sampler2D _Shadow;

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
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
