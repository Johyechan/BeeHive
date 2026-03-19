Shader "UI/StencilHole"
{
    Properties
    {
        _Color ("Dimmer Color", Color) = (0,0,0,0.6)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _StencilRef ("Stencil Ref", Float) = 1
        _HoleRadius ("Hole Radius", Float) = 0.45
        _OutlineWidth ("Outline Width", Float) = 0.02
        _HoleCenter("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleScale ("Hole Scale (X,Y)", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            // dimmer + 테두리 그리기
            Stencil
            {
                Ref [_StencilRef]
                Comp NotEqual
                Pass Keep
            }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            half4 _Color;
            half4 _OutlineColor;
            float _HoleRadius;
            float _OutlineWidth;
            float4 _HoleCenter;
            float4 _HoleScale;

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.positionOS.xyz);
                o.uv  = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 center = _HoleCenter.xy; // 중복 제거
                float2 aspect = float2(_ScreenParams.x / _ScreenParams.y, 1.0);
                float2 uvOffset = i.uv - center;
                float2 ellipse = uvOffset * aspect / _HoleScale.xy;
                float dist = length(ellipse);
            
                half4 color;
            
                if (dist < _HoleRadius)
                {
                    color = half4(0,0,0,0); // 구멍 안쪽
                }
                else if (dist < _HoleRadius + _OutlineWidth)
                {
                    color = _OutlineColor;   // 테두리
                }
                else
                {
                    color = _Color;          // dimmer
                }
            
                return color;
            }

            ENDHLSL
        }
    }
}