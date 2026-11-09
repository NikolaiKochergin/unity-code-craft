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
                float2 uv : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                                
                float2 positionCS = lerp(
                    _Rect.xy, 
                    _Rect.zw, 
                    input.uv
                );
                
                output.positionHCS = float4(
                    positionCS * 2.0 - 1.0,
                    0.0,
                    1.0
                );
                
                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 rectSizePixels = 
                    (_Rect.zw - _Rect.xy) * _ScreenParams.xy;
                
                float2 edgeDistance = min(
                    input.uv,
                    1.0 - input.uv
                ) * rectSizePixels;
                
                float distanceToBorder = min(
                    edgeDistance.x,
                    edgeDistance.y
                );
                
                float border = 1.0 - step(
                    _BorderWidth,
                    distanceToBorder
                );
                
                return  lerp(
                    _FillColor,
                    _BorderColor,
                    border
                );
            }
            
            ENDHLSL
        }
    }
}