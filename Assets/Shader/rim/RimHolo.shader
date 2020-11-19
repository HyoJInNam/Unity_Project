// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RimHolo"
{
	Properties
	{
		[Header(Dissolve)]
		_Color("base color", Color) = (1, 1, 1, 1)
		_Alpha("_Alpha", Range(0.0, 1.0)) = 1
		_MainTex("Main Texture", 2D) = "white" {}

		_SliceAmount("Slice Amount", Range(0.0, 1.0)) = 0
		_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
		_EmissionAmount("Emission amount", float) = 2.0

		[Header(Rim)]
		[HDR]
		_RColor("Rim color", Color) = (1, 1, 1, 1)
		_MaskTex("Mask Texture", 2D) = "white" {}
		_RPower("Rim power", Range(0, 10)) = 2
		_FlashSpeed("flashing speed", float) = 1

		[Header(Hologram)]
		_HSpeed("Holo speed", float) = 3
		_HLineCnt("Holo Line Count", float) = 30
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "transparent"
		}
		LOD 200
		CGPROGRAM
		#pragma surface surf Lambert alpha:blend
		#pragma target 3.0

		float4 _Color;
		sampler2D _MainTex;
		float _Alpha;

		float _SliceAmount;
		float _BurnSize;
		float _EmissionAmount;

		sampler2D _MaskTex;
		float _RPower;
		float4 _RColor;
		float _FlashSpeed;

		float _HSpeed;
		float _HLineCnt;


		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
		};

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutput o)
		{
			// Dissolve
			float4 mainTex = tex2D(_MainTex, IN.uv_MainTex - normalize(IN.worldPos - _SinTime.x));
			half test = mainTex.rgb - _SliceAmount;
			clip(test);

			if (test < _BurnSize && _SliceAmount > 0) {
				mainTex = tex2D(_MainTex, float2(test * (1 / _BurnSize), 0)) * _Color * _EmissionAmount;
			}
			float a = (mainTex.r + mainTex.g + mainTex.b) / 3;

			// rim
			float4 maskTex = tex2D(_MaskTex, (IN.uv_MainTex.x, IN.uv_MainTex.y - _Time.y * 0.5));
			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, _RPower);
			o.Emission = _RColor.rgb;

			// hologram
			float holo = pow(frac(IN.worldPos.y * _HLineCnt - _Time.y * _HSpeed), 5);
			float flashSpeed = (_FlashSpeed > 0) ? abs(sin(_Time.y * _FlashSpeed)) : 1;
			o.Alpha = pow(a * (holo + maskTex * rim) * flashSpeed, (1 -_Alpha)) ;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
