Shader "Custom/Noise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StepTime ("Speed", Float) = 1
    }
    SubShader
    {
        Pass
        {
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
                float steppedTime = steppedTick / tickRate;

                uv += float2(steppedTime, steppedTime);

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}