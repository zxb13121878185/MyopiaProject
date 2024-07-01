Shader "Unlit/RampSky"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HorizonColor ("地平线颜色", Color) = (1, 1, 1, 1)
        _ZenithColor ("天顶颜色", Color) = (1, 1, 1, 1)
        _FallOff ("渐变", Range(0.1, 2)) = 1
        _HorizonOffset ("地平线偏移", Range(-1, 1)) = 0    
    }
    SubShader
    {
        Tags { "RenderType"="Background" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
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

            fixed4 _HorizonColor;
            fixed4 _ZenithColor;
            float _FallOff;

            fixed _HorizonOffset;
            fixed _HorizonOffset2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // fixed fade = saturate(pow(1 - i.uv.y, _FallOff));
                fixed fade = smoothstep(_HorizonOffset, _HorizonOffset + _FallOff, i.uv.y);
                fixed4 col = lerp(_ZenithColor, _HorizonColor, 1 - fade);
                
                return col;
            }
            ENDCG
        }
    }
}
