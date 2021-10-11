Shader "Hidden/BlackWhite"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
        _NoiseTex ("-", 2D) = "" {}
        _TrashTex ("-", 2D) = "" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    sampler2D _MainTex;

    float2 _ColorDrift;

    float black_white_col(half4 src1)
    {
        return src1.r * 0.3f + src1.g * 0.59f + src1.b * 0.11f;
    }

    float4 frag(v2f_img i) : SV_Target
    {
        half4 src = tex2D(_MainTex, i.uv);
        src = black_white_col(src);

        return src;
    }
    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }
    }
}