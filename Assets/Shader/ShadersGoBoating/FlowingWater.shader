Shader "Unlit/FlowingWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _NoiseMap ("噪声图", 2D) = "white" {}
        _FlowSpeed ("水流速度", Range(0, 2)) = 0.16
        _FoamColor ("泡沫颜色", Color) = (1, 1, 1, 1)
        _SurfaceNoiseCutoff ("透明度阈值", Range(0, 1)) = 0.88

        _SurfaceDistortion ("扰动图", 2D) = "white" {}
        _SurfaceDistortionAmount ("扰动程度", Range(0, 0.1)) = 0.08
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define SMOOTHSTEP_AA 0.01

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float2 distortUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;

            sampler2D _NoiseMap;
            float4 _NoiseMap_ST;

            float _FlowSpeed;
            fixed4 _FoamColor;
            float _SurfaceNoiseCutoff;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            float _SurfaceDistortionAmount;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _NoiseMap);
                o.uv.w -= frac(_Time.x * _FlowSpeed);
                // o.uv.w += frac(_FlowSpeed);

                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv.xy) * _Color.rgba;

                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;
                // 用噪声图扰动
                float2 noiseUV = i.uv.zw + distortSample;

                fixed surfaceNoiseSample = tex2D(_NoiseMap, noiseUV).r;
                float surfaceNoise = smoothstep(_SurfaceNoiseCutoff - SMOOTHSTEP_AA, _SurfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);
                surfaceNoise *= _FoamColor.a;

                col = lerp(col, _FoamColor, surfaceNoise);
                
                return col;
            }
            ENDCG
        }
    }
}
