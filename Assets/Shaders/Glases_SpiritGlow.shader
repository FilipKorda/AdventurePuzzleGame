Shader "Custom/Glases_SpiritGlow"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0,1,1,1)
        _GlowSize ("Glow Size", Float) = 1
        _GlowPower ("Glow Power", Float) = 1
        _Hidden ("Hidden", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _GlowColor;
            float _GlowSize;
            float _GlowPower;
            float _Hidden;
            float _SpiritVision;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 s = _MainTex_TexelSize.xy * _GlowSize;
                float a = 0;
                a += tex2D(_MainTex, i.uv + float2( s.x, 0)).a;
                a += tex2D(_MainTex, i.uv + float2(-s.x, 0)).a;
                a += tex2D(_MainTex, i.uv + float2(0,  s.y)).a;
                a += tex2D(_MainTex, i.uv + float2(0, -s.y)).a;

                float glow = saturate(a - col.a) * _GlowPower * _SpiritVision;

                float visible = lerp(1, 0, _Hidden) * (1 - _SpiritVision);

                fixed3 rgb = col.rgb * visible + _GlowColor.rgb * glow;
                float alpha = max(visible * col.a, glow);

                return fixed4(rgb, alpha);
            }
            ENDCG
        }
    }
}
