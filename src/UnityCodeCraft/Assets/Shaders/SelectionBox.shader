Shader "Custom/SelectionBox"
{
    Properties
    {
        _Rect ("Selection Rect", Vector) = (0.2, 0.2, 0.8, 0.8)

        _FillColor ("Fill Color", Color) = (0.2, 0.7, 1.0, 0.15)
        _BorderColor ("Border Color", Color) = (0.2, 0.7, 1.0, 1.0)
        _BorderWidth ("Border Width (Pixels)", Float) = 2.0
    }
    SubShader
    {
        Name "SelectionBox"
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Rect;
                float4 _FillColor;
                float4 _BorderColor;
                float _BorderWidth;
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
                
                float2 positionCS = input.uv * 2.0 - 1.0;
                
                output.positionHCS = float4(positionCS, 0.0, 1.0);
                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                
                float2 rectMin = _Rect.xy;
                float2 rectMax = _Rect.zw;
                
                float2 insideMin = step(rectMin, uv);
                float2 insideMax = step(uv, rectMax);
                
                float inside = 
                    insideMin.x * insideMin.y *
                    insideMax.x * insideMax.y;
                
                clip(inside - 0.5);
                
                float2 distanceToEdge = min(
                    (uv - rectMin) * _ScreenParams.xy,
                    (rectMax - uv) * _ScreenParams.xy
                );
                
                float distanceToBorder = min(
                    distanceToEdge.x,
                    distanceToEdge.y
                );
                
                float border = 1.0 - step(
                    _BorderWidth,
                    distanceToBorder
                );
                
                half4 color = lerp(
                    _FillColor,
                    _BorderColor,
                    border
                );
                
                return color;
            }
            ENDHLSL
        }
    }
}