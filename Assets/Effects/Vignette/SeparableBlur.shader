
Shader "Hidden/Blur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct v2f
    {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;

        float4 uv01 : TEXCOORD1;
        float4 uv02 : TEXCOORD2;
        float4 uv03 : TEXCOORD3;
        float4 uv04 : TEXCOORD4;
        float4 uv05 : TEXCOORD5;
        float4 uv06 : TEXCOORD6;
    };

    float4 _Offsets;

    v2f vert(appdata_img v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);

        o.uv.xy = v.texcoord.xy;

        const float4 f = float4(1, 1, -1, -1);

        o.uv01 = v.texcoord.xyxy + _Offsets.yxyx * f ;
        o.uv02 = v.texcoord.xyxy - _Offsets.yxyx * f * 2;
        
        o.uv03 = v.texcoord.xyxy + _Offsets.yxyx * f + _Offsets.xyxy * f ;

        o.uv04 = v.texcoord.xyxy + _Offsets.xyxy * f * 2;
        o.uv05 = v.texcoord.xyxy - _Offsets.xyxy * f;
        
        o.uv06 = v.texcoord.xyxy - _Offsets.xyxy * f + _Offsets.yxyx* f ;

        return o;
    }

    sampler2D _MainTex;

    half4 frag(v2f input) : SV_Target
    {
        half4 color = float4(0, 0, 0, 0);

        color += 0.30 * tex2D(_MainTex, input.uv);
        color += 0.10 * tex2D(_MainTex, input.uv01.xy);
        color += 0.10 * tex2D(_MainTex, input.uv01.zw);
        color += 0.05 * tex2D(_MainTex, input.uv02.xy);
        color += 0.05 * tex2D(_MainTex, input.uv02.zw);
        
        color += 0.05 * tex2D(_MainTex, input.uv03.zw);
        color += 0.05 * tex2D(_MainTex, input.uv03.zw);

        color += 0.05 * tex2D(_MainTex, input.uv04.xy);
        color += 0.05 * tex2D(_MainTex, input.uv04.zw);
        color += 0.05 * tex2D(_MainTex, input.uv05.xy);
        color += 0.05 * tex2D(_MainTex, input.uv05.zw);
        
        color += 0.05 * tex2D(_MainTex, input.uv06.zw);
        color += 0.05 * tex2D(_MainTex, input.uv06.zw);

        return color;
    }
    ENDCG

    Subshader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }

    Fallback off


} // shader