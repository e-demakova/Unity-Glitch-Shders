Shader "Hidden/Glitch/Color"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
    }
    CGINCLUDE
    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float2 _MainTex_TexelSize;

    float _ScanLineJitter;
    float _JitterStep;
    float2 _ColorDrift;
    
    int _IsVertical;
    int _IsHorizontal;
    int _Distortion;
    
    int _R;
    int _G;
    int _B;

    float nrand(float x, float y)
    {
        return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
    }

    half4 frag(v2f_img i) : SV_Target
    {
        const float x = i.uv.x;
        const float y = i.uv.y;;

        const float drift = sin(y * _Distortion + _ColorDrift.y) * _ColorDrift.x;

        half4 src1 = tex2D(_MainTex, frac(float2(x, y)));
        half4 src2 = tex2D(_MainTex, frac(float2(x, y)));
        
        if (_IsVertical == 1)
            src1 = tex2D(_MainTex, frac(float2(x, y + drift)));
        if (_IsHorizontal == 1)
            src2 = tex2D(_MainTex, frac(float2(x + drift, y)));

        if(_R == 1)
        src1.r = src2.r;
        if(_G == 1)
        src1.g = src2.g;
        if(_B == 1)
        src1.b = src2.b;
        
        return src1;
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