Shader "Custom/ShowAndCloseShader" // 셰이더 경로 이름 - 그냥 식별자
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0,1)) = 0
        _EdgeWidth ("Edge Width", Range(0,0.2)) = 0.05
        _BurnColor ("Burn Color", Color) = (1,0.4,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Progress;
            float _EdgeWidth;
            float4 _BurnColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float burnLine = _Progress;

                if (i.uv.y < burnLine)
                    discard;

                float edge = smoothstep(burnLine, burnLine + _EdgeWidth, i.uv.y);

                fixed4 col = tex2D(_MainTex, i.uv);

                col.rgb = lerp(_BurnColor.rgb, col.rgb, edge);

                return col;
            }
            ENDCG
        }
    }
}
