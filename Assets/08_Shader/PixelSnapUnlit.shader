Shader "Custom/URP/PixelSnapUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "DisableBatching"="True"
        }

        LOD 100

        Pass
        {
            Name "Unlit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;

            Varyings vert(Attributes input)
            {
                Varyings output;
                float4 positionHCS = TransformObjectToHClip(input.positionOS);

                // «»ºø Ω∫≥¿ (»≠∏È ¡¬«• ±‚¡ÿ Ω∫≥¿)
                float2 screenUV = (positionHCS.xy / positionHCS.w * 0.5 + 0.5) * _ScreenParams.xy;
                screenUV = floor(screenUV + 0.5);
                screenUV /= _ScreenParams.xy;
                positionHCS.xy = (screenUV * 2.0 - 1.0) * positionHCS.w;

                output.positionHCS = positionHCS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return tex * _Color;
            }

            ENDHLSL
        }
    }
}
