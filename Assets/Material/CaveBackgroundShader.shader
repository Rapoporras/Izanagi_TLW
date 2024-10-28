Shader "Unlit/CaveBackgroundShader"
{
    Properties
    {
        _DarkColor ("Dark Color", Color) = (0, 0, 0, 1)
        _RockColor ("Rock Color", Color) = (0.2, 0.2, 0.2, 1)
        _Scale ("Scale", Float) = 10.0
        _TimeSpeed ("Time Speed", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _DarkColor;
            float4 _RockColor;
            float _Scale;
            float _TimeSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float Voronoi(float2 uv)
            {
                float2 p = uv * _Scale;
                float2 i = floor(p);
                float2 f = frac(p);
                
                float minDist = 1.0;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        float2 neighbor = float2(x, y);
                        float2 point = float2(sin(dot(i + neighbor, float2(12.9898, 78.233))) * 43758.5453,
                                              cos(dot(i + neighbor, float2(12.9898, 78.233))) * 43758.5453);
                        float dist = length(point - f);
                        minDist = min(minDist, dist);
                    }
                }
                return minDist;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float noiseValue = Voronoi(i.uv + _Time.y * _TimeSpeed);
                float shadowFactor = smoothstep(0.1, 0.0, noiseValue); // Ajusta estos valores para controlar las sombras

                fixed4 color = lerp(_DarkColor, _RockColor, shadowFactor);
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}