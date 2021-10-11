Shader "Unlit/SpriteGlitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorDrift ("Color Drift", Range(-0.3,0.3)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent" "Queue" = "Transparent"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ColorDrift;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, input.uv);

                float x = input.uv.x;
                float y = input.uv.y;

                fixed4 col1 = tex2D(_MainTex, frac(float2(x, y + _ColorDrift)));
                fixed4 col2 = tex2D(_MainTex, frac(float2(x + _ColorDrift, y)));

                col = fixed4(col2.r, col1.g, col1.b, col.a);
                return col;
            }
            ENDCG
        }
    }
}