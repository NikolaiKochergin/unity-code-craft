Shader "Custom/UnitHealth"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0.15, 0.02, 0.02, 1)
        _FillColor ("Fill Color", Color) = (0.1, 1.0, 0.15, 1)
        _BarRect ("Bar Rect (Size, Offset)", Vector) = (100, 12, 0, 0)
        _Fill ("Fill", Range(0, 1)) = 1
    }
    SubShader
    {
        Name "UnitHealth"
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BackgroundColor;
                half4 _FillColor;
                float4 _BarRect;
                float _Fill;
            CBUFFER_END

            struct Attributes
            {
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                float3 centerWS = TransformObjectToWorld(float3(0, 0, 0));
                float4 centerCS = TransformWorldToHClip(centerWS);

                float2 offsetPixels = (input.uv - 0.5) * _BarRect.xy + _BarRect.zw;
                float2 offsetNDC = offsetPixels * 2.0 / _ScreenParams.xy;

                centerCS.xy += offsetNDC * centerCS.w;

                output.positionHCS = centerCS;
                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half fillMask = step(input.uv.x, _Fill);
                return lerp(_BackgroundColor, _FillColor, fillMask);
            }
            ENDHLSL
        }
    }
}