Shader "Custom/UnitHealthInstanced"
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
        Name "UnitHealthInstanced"

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
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BackgroundColor)
                UNITY_DEFINE_INSTANCED_PROP(float4, _FillColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _Fill)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BarRect)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct Attributes
            {
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                float3 centerWS = TransformObjectToWorld(float3(0, 0, 0));
                float4 centerCS = TransformWorldToHClip(centerWS);

                float4 barRect = UNITY_ACCESS_INSTANCED_PROP(Props, _BarRect);

                float2 offsetPixels = (input.uv - 0.5) * barRect.xy + barRect.zw;
                float2 offsetNDC = offsetPixels * 2.0 / _ScreenParams.xy;

                centerCS.xy += offsetNDC * centerCS.w;
                output.positionHCS = centerCS;
                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                half4 backgroundColor = UNITY_ACCESS_INSTANCED_PROP(Props, _BackgroundColor);
                half4 fillColor = UNITY_ACCESS_INSTANCED_PROP(Props, _FillColor);
                float fill = UNITY_ACCESS_INSTANCED_PROP(Props, _Fill);

                half fillMask = step(input.uv.x, fill);

                return lerp(backgroundColor, fillColor, fillMask);
            }

            ENDHLSL
        }
    }
}
