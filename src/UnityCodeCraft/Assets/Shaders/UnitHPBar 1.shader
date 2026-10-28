Shader "Custom/UnitHPBar"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0.15, 0.02, 0.02, 1)
        _FillColor ("Fill Color", Color) = (0.1, 1.0, 0.15, 1)
        _BarRect ("Bar Rect (Size, Offset)", Vector) = (1.5, 0.15, 0, 0)
        _Fill ("Fill", Range(0, 1)) = 1
    }
    SubShader
    {
        Name "UnitHPBar"
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
            #include "UnitSelection.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BackgroundColor;
                half4 _FillColor;
                half4 _BarRect;
                half _Fill;
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
                
                float3 centerWS = TransformObjectToWorld(float3(0,0,0));
                float2 offset = (input.uv - 0.5) * _BarRect.xy + _BarRect.zw;
                
                float3 positionWS = centerWS 
                    + UNITY_MATRIX_I_V._m00_m10_m20 * offset.x
                    + UNITY_MATRIX_I_V._m01_m11_m21 * offset.y;                    
                
                output.positionHCS = TransformWorldToHClip(positionWS);
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