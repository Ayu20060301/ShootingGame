Shader "Custom/GlitchShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _GlitchIntensity("Glitch Intensity", Range(0,1)) = 0.1  //グリッチの強さ
        _BlockScale("Block Scale", Range(1,50)) = 10 //ノイズの細かさ
        _NoiseSpeed("Noise Speed",Range(1,10)) = 10 //ノイズの速度
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent"}

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

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
                float _GlitchIntensity;
                float _BlockScale;
                float _NoiseSpeed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            //疑似乱数を生成
            float Random(float2 seeds)
            {
                return frac(sin(dot(seeds,float2(12.9898,78.233))) * 43758.5453);
            }

            //ブロック単位のノイズ
            float BlockNoise(float2 seeds)
            {
                return Random(floor(seeds));
            }

            //-1～1のランダム値を生成
            float NoiseRandom(float2 seeds)
            {
                return -1.0 + 2.0 * BlockNoise(seeds);
            }


            half4 frag(Varyings IN) : SV_Target
            {
               //元UV
               float2 gv = IN.uv;

               //ブロックノイズを生成
               float noise = BlockNoise(float2(IN.uv.y * _BlockScale, _Time.y * _NoiseSpeed));
               
               //ランダム性を追加
               noise += Random(float2(IN.uv.x,_Time.y)) * 0.3;
              
               //横方向へずらす値
               float2 randomValue = NoiseRandom(float2(IN.uv.y * _BlockScale,_Time.y * _NoiseSpeed));
               
               //UVを横方向へずらしてグリッチを表現
               gv.x += randomValue 
                     * _GlitchIntensity
                     *sin(noise * 6.28);

               //元画像を取得
               half4 base = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, gv);

               half4 color;
               
               //RGBを少しずらして色分離
               color.r = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, gv + float2(0.006,0)).r;
               color.g = base.g;
               color.b = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,gv - float2(0.008,0)).b;
               
               //元画像のアルファを保持
               color.a = base.a;

               //カラーを乗算して出力
               return color * _BaseColor;
            }
            ENDHLSL
        }
    }
}
