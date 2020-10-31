Shader "Custom/RimHolo"
{
	Properties
	{
		_Color("base color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		_MaskTex("Mask Texture", 2D) = "white" {}

		// Rim
		_RPower("Rim power", Range(1, 10)) = 2
		[HDR]
		_RColor("Rim color", Color) = (1, 1, 1, 1)
		_FlashSpeed("flashing speed", float) = 1

		// Hologram
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
			float4 mainTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float4 maskTex = tex2D(_MaskTex, (IN.uv_MainTex.x, IN.uv_MainTex.y - _Time.y * 0.5));

			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, _RPower);
			o.Emission = _RColor.rgb;

			float holo = pow(frac(IN.worldPos.y * _HLineCnt - _Time.y * _HSpeed), 5);
			float flashSpeed = (_FlashSpeed > 0) ? abs(sin(_Time.y * _FlashSpeed)) : 1;
			o.Alpha = (holo + maskTex * rim) * flashSpeed;
		}

		ENDCG
	}
		FallBack "Diffuse"
}
