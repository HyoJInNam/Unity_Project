Shader "Custom/Rim"
{
    Properties
    {
		_Color("base color", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

		// Rim
		_Pow("Rim strong", Range(1, 10)) = 5
		[HDR]
		_RimColor("Rim color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

		float4 _Color;
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
        };

		float _Pow;
		float4 _RimColor;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 mainTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(1 - rim, _Pow);

			o.Emission = rim * _RimColor.rgb;
			o.Alpha = mainTex.a;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
