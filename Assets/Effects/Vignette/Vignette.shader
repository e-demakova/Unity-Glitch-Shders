
Shader "Hidden/Vignetting"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _VignetteTex ("Vignette", 2D) = "white" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct v2f
    {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;
        float2 uv2 : TEXCOORD1;
    };

    float4 _MainTex_TexelSize;
    sampler2D _MainTex;
    sampler2D _VignetteTex;

    half _Blur;
    float _ChromaticAberration;
    float _ChromaticAberrationMultiplier;

    v2f vert(appdata_img v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        o.uv2 = v.texcoord.xy;

        #if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y < 0)
            o.uv2.y = 1.0 - o.uv2.y;
        #endif

        return o;
    }

    half4 frag(v2f i) : SV_Target
    {
        half2 coords = i.uv;
        
        const half coordDot = dot(coords, coords);

        coords.x += 0.1;

        coords = (coords - 0.5) * 2.0;
        half4 color = tex2D(_MainTex, i.uv);

        half4 colorBlur = tex2D(_VignetteTex, i.uv2);

        coords = (coords - 0.5) * _ChromaticAberrationMultiplier;

        const half2 uvG = i.uv - _MainTex_TexelSize.xy * _ChromaticAberration * coords * coordDot;
        const half2 uvR = uvG - _MainTex_TexelSize.xy * _ChromaticAberration * coords * coordDot;

        color.g = tex2D(_MainTex, uvG).g;
        color.r = tex2D(_MainTex, uvR).r;

        colorBlur.g = tex2D(_VignetteTex, uvG).g;
        colorBlur.r = tex2D(_VignetteTex, uvR).r;
        
        color = lerp(color, colorBlur, saturate(_Blur * coordDot));

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
}