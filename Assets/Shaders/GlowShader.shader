Shader "Custom/GlowShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _GlowColor ("Glow Color", Color) = (0, 0, 0, 0)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            float4 _GlowColor;
            float _GlowIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _GlowColor * _GlowIntensity;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
