Shader "Custom/UnitSelectionInstanced"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
        _Thickness ("Thickness", Float) = 0.2
        _Size ("Size", Float) = 1
    }
    SubShader
    {
        Name "UnitSelectionInstanced"
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnitSelection.hlsl"

            float _Thickness;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _Size)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 centerWS : TEXCOORD1;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                half size = UNITY_ACCESS_INSTANCED_PROP(Props, _Size);

                float3 scaledPositionOS = input.positionOS.xyz * size;

                output.positionWS = TransformObjectToWorld(scaledPositionOS);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.centerWS = TransformObjectToWorld(float3(0, 0, 0));

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                half size = UNITY_ACCESS_INSTANCED_PROP(Props, _Size);
                half4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _BaseColor);

                return DrawSelectionCircle(
                    input.positionWS,
                    input.centerWS,
                    size * 0.5,
                    _Thickness,
                    color
                );
            }
            ENDHLSL
        }
    }
}