Shader "Custom/GlitchEffectRandomTiming"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _BlockSize("Block Size",Range(1,100)) = 20
        _GlitchAmount("Glitch Amount", Range(0,1)) = 0.1
        _GlitchFrequency("Glitch Frequency", Range(0.1,2.0)) = 1
        _GlitchDuration("Glitch Duration", Range(0.1,2.0))= 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        AlphaToMask Off


        Pass
        {
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

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float _BlockSize;
                float _GlitchAmount;
                float _GlitchFrequency;
                float _GlitchDuration;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            float Random(float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233))) * 43758.5453);
            }


            half4 frag(Varyings IN) : SV_Target
            {
               float2 uv = IN.uv;
               float glitchTime = floor(_Time.y / _GlitchFrequency) * _GlitchFrequency;
               float glitchActive = step(glitchTime, _Time.y) * step(_Time.y,glitchTime + _GlitchDuration);

               float2 block = floor(uv * _BlockSize) / _BlockSize;
               float offset = Random(float2(block.y,glitchTime))
               * _GlitchAmount 
               * glitchActive;

               half4 col = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,uv);
               col *= _BaseColor;

               return col;
            }
            ENDHLSL
        }
    }
}
