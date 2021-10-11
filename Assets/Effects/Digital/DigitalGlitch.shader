Shader "Hidden/Glitch/Digital"
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
    sampler2D _NoiseTex;
    sampler2D _TrashTex;
    float _Intensity;
    float _SafeArea;
    float _Shift;

    float2 _ColorDrift;

    int _R;
    int _G;
    int _B;

    float4 frag(v2f_img i) : SV_Target
    {
        float2 uv = i.uv;

        if (uv.y > 1 - _SafeArea &&
            uv.y < _SafeArea &&
            uv.x > 1 - _SafeArea &&
            uv.x < _SafeArea)
        {
            return tex2D(_MainTex, i.uv);
        }

        const float thresh = 1.001 - _Intensity;
        
        float4 noise = tex2D(_NoiseTex, i.uv);
        float glStep = step(thresh, pow(noise.w, 3.5));
        uv = frac(i.uv + noise.xy * _Shift * glStep);

        float4 noiseDrift = tex2D(_NoiseTex, uv + _ColorDrift);
        float driftStep = step(thresh, pow(noiseDrift.w, 3.5));
        float2 uvDrift = frac(uv + noiseDrift.xy * _ColorDrift * 2 * max(driftStep, glStep));

        half4 src1 = tex2D(_MainTex, uv);
        half4 src2 = tex2D(_MainTex, uvDrift);

        if (_R == 1)
            src1.r = src2.r;
        if (_G == 1)
            src1.g = src2.g;
        if (_B == 1)
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