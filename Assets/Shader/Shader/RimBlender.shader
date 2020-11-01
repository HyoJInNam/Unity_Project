Shader "Custom/RimBlender"
{
	Properties
	{
		_Color("base color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

		// Rim
		_RPower("Rim power", Range(1, 10)) = 5
		[HDR]
		_RimColor("Rim color", Color) = (1, 1, 1, 1)

		_FlashSpeed("flashing speed", float) = 1
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

		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
		};

		float _RPower;
		float4 _RimColor;

		float _FlashSpeed;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 mainTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, _RPower);

			o.Emission = _RimColor.rgb;
			//o.Alpha = saturate(rim * sin(_Time.y * _FlashSpeed));
			//o.Alpha = rim * (sin(_Time.y * _FlashSpeed) * 0.5 + 0,5);
			o.Alpha = rim * abs(sin(_Time.y * _FlashSpeed));
		}

		ENDCG
	}
		FallBack "Diffuse"
}
