Shader "Custom/InvertBehind"
{
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        GrabPass { "_BackgroundTex" }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _BackgroundTex;

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.grabPos.xy / i.grabPos.w;
                fixed4 col = tex2D(_BackgroundTex, uv);

                col.rgb = 1.0 - col.rgb;

                return col;
            }
            ENDCG
        }
    }
}