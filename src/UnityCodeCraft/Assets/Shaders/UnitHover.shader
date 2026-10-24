Shader "Custom/UnitHover"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
        _Thickness ("Thickness", Float) = 0.2
        _Dash ("Dash", Float) = 8
        _Speed ("Speed", Float) = 2.0
        _Radius ("Radius", Float) = 2
    }
    SubShader
    {
        Name "UnitHover"
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
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnitSelection.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half _Thickness;
                half _Dash;
                half _Speed;
                half _Radius;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 centerWS : TEXCOORD1;
            };
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.centerWS = TransformObjectToWorld(float3(0,0,0));
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {                
                return  DrawHoverCircle(
                    input.positionWS,
                    input.centerWS,
                    _Radius,
                    _Thickness,
                    _BaseColor,
                    _Dash,
                    _Speed
                );
            }
            ENDHLSL
        }
    }
}