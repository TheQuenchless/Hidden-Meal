Shader "Custom/Liquid"
{
    Properties
    {
        _Color ("Color", Color) = (0,0.5,1,0.5)
        _LiquidLevel ("Fill Amount", float) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _LiquidLevel;
            float _BoundsMinY;
            float _BoundsMaxY;
            float _WobbleX;
            float _WobbleZ;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float localY : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // normalize localY from 0 (bottom) to 1 (top)
                o.localY = (worldPos.y - _BoundsMinY) / (_BoundsMaxY - _BoundsMinY);

                return o;
            }

            fixed4 frag(float4 pos : SV_POSITION, float2 uv : TEXCOORD0, float localY : TEXCOORD1, float faceSign : VFACE) : SV_Target
            {
                fixed4 col = _Color;

                float sideAlpha = step(localY, _LiquidLevel);

                col.a *= sideAlpha;
                return col;
            }
            ENDCG
        }
    }
}