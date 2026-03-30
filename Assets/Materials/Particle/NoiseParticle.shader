Shader "Custom/NoiseParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StepTime ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _StepTime;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float tickRate = 60.0;
                float tick = floor(_Time.y * tickRate);
                float steppedTick = floor(tick / _StepTime) * _StepTime;

                // Rotate in 90-degree steps
                float rotationStep = steppedTick * 1.57079632679; 

                float2 center = float2(0.5, 0.5);
                float2 uvCentered = uv - center;

                float cosR = cos(rotationStep);
                float sinR = sin(rotationStep);

                float2 uvRotated;
                uvRotated.x = uvCentered.x * cosR - uvCentered.y * sinR;
                uvRotated.y = uvCentered.x * sinR + uvCentered.y * cosR;

                uv = uvRotated + center;

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}