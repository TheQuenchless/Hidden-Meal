Shader "Custom/InvertParticle"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }

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
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.grabPos.xy / i.grabPos.w;
                fixed4 bgCol = tex2D(_BackgroundTex, uv);

                // Invert background
                bgCol.rgb = 1.0 - bgCol.rgb;

                fixed4 texCol = tex2D(_MainTex, i.uv);

                // Apply particle alpha to inverted background
                bgCol.a = texCol.a;         // preserve alpha for transparency
                bgCol.rgb *= texCol.a;      // particle alpha masks the inverted color

                return bgCol;
            }
            ENDCG
        }
    }
}