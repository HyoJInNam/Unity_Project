Shader "Custom/GrassBlend"
{
    Properties
    { 
		_Color("Color", color) = (1,1,1,0)
        _MainTex("Texture", 2D) = "white" {}
		_Speed("Speed", Range(1, 10)) = 3.0
		_Power("Power", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard alpha:blend
		#pragma vertex vert
        #pragma target 3.0

        sampler2D _MainTex;

        fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		float _Speed;
		float _Power;

		void vert(inout appdata_full v)
		{
			v.vertex.x = v.vertex.x + sin(_Time.y * _Speed) * ((v.vertex.y < 0.5f) ? 0 : (1 - v.vertex.y) * _Power);
		}



		struct Input
		{
			float2 uv_MainTex;
		};

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Emission = c.rgb;
			o.Alpha = (1 - IN.uv_MainTex.y) * c.a;
        }
        ENDCG

    }
    FallBack "Diffuse"
}
