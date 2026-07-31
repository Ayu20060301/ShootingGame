Shader "Custom/GlitchShader_Logo"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _GlitchStrength("Glitch Strength",Range(0,0.1)) = 0.02
        _GlitchSpeed("Glitch Speed",float) = 8.0
        _RGBSplit("RGB Split",Range(0,0.02)) = 0.003
        _Glitch("Glitch",Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

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

                float _GlitchStrength;
                float _GlitchSpeed;
                float _RGBSplit;
                float _Glitch;

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
               //時間
               float time = _Time.y * _GlitchSpeed;

               //ラインごとのノイズ
               float noise = Random(float2(
                   floor(IN.uv.y * 250.0),
                   floor(time * 30.0f)));


                //UV
                float2 uv = IN.uv;

                //グリッチ中だけ横にずらす
                uv.x += (noise - 0.5) * _GlitchStrength * _Glitch;

                //RGBずらし
                float split = _RGBSplit * _Glitch;

                half r = SAMPLE_TEXTURE2D(
                    _BaseMap,
                    sampler_BaseMap,
                    uv + float2(split,0)).r;

                half g = SAMPLE_TEXTURE2D(
                    _BaseMap,
                    sampler_BaseMap,
                    uv).g;

                half b = SAMPLE_TEXTURE2D(
                    _BaseMap,
                    sampler_BaseMap,
                    uv - float2(split,0)).b;

                half a = SAMPLE_TEXTURE2D(
                    _BaseMap,
                    sampler_BaseMap,
                    uv).a;

                return half4(r,g,b,a) * _BaseColor;
            }
            ENDHLSL
        }
    }
}
