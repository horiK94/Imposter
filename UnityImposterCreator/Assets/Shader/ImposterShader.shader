Shader "Unlit/ImposterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            int _WidthCount;
            int _HeightCount;
            float2 _UsePoint;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvSize = float2(_WidthCount, _HeightCount);
                float2 uvStartPoint = float2(_UsePoint.x / _WidthCount, _UsePoint.y / _HeightCount);
                
                fixed4 col = tex2D(_MainTex, uvStartPoint + i.uv / uvSize);
                clip(col.a - 0.01);
                return col;
            }
            ENDCG
        }
    }
}
