Shader "Custom/PhysicDamage"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FlashSpeed ("Flash Speed", float) = 2.0 // Velocidad de cambio de color
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FlashSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Obtenemos el color original del sprite, incluyendo el alfa
                float4 spriteColor = tex2D(_MainTex, i.uv);

                // Usamos _Time directamente, no es necesario declararlo
                float timeMod = fmod(_Time.y * _FlashSpeed, 1.0);

                // Interpolamos entre los colores basado en el valor de 'timeMod'
                float4 flashColor;
                if (timeMod < 0.33)
                {
                    flashColor = lerp(float4(1, 1, 1, 1), float4(0, 1, 0, 1), timeMod / 0.33); // Blanco a Verde
                }
                else if (timeMod < 0.66)
                {
                    flashColor = lerp(float4(0, 1, 0, 1), float4(0, 0, 0, 1), (timeMod - 0.33) / 0.33); // Verde a Negro
                }
                else
                {
                    flashColor = lerp(float4(0, 0, 0, 1), float4(1, 1, 1, 1), (timeMod - 0.66) / 0.33); // Negro a Blanco
                }

                // Combina el color del sprite con el color de flash, pero mantiene el canal alfa original
                float4 finalColor = spriteColor * flashColor;
                finalColor.a = spriteColor.a; // Mantén la transparencia original del sprite

                return finalColor;
            }
            ENDCG
        }
    }
}