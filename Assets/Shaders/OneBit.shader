Shader "Hidden/Quant"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {

        CGINCLUDE
            #include "UnityCG.cginc"

            struct VertexData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Texture2D _MainTex;
            SamplerState point_clamp_sampler;
            float4 _MainTex_TexelSize;

            v2f vert(VertexData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            int _ColorCount;
            float _Threshold;
            fixed4 _Color1;
            fixed4 _Color2;
            int _Invert;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = _MainTex.Sample(point_clamp_sampler, i.uv);

                float lum = dot(color.rgb, float3(0.299, 0.587, 0.114));

                float bw = step(_Threshold, lum);

                bw = lerp(bw, 1.0 - bw, _Invert);

                return lerp(_Color2, _Color1, bw); // Replace grayscale with the chosen colors
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                return _MainTex.Sample(point_clamp_sampler, i.uv);
            }
            ENDCG
        }
    }
}