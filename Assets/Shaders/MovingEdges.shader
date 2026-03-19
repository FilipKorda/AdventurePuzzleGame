Shader "Custom/MovingEdges"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowStrength ("Glow Strength", Range(0,5)) = 1
        _EdgeWidth ("Edge Width", Range(0.01,0.5)) = 0.15
        _NoiseStrength ("Noise Strength", Range(0,0.1)) = 0.03
        _NoiseScale ("Noise Scale", Range(1,50)) = 20
        _NoiseSpeed ("Noise Speed", Range(0,10)) = 2
        _BlockSize ("Edge Block Size", Range(2,64)) = 8
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowStrength;
            float _EdgeWidth;
            float _NoiseStrength;
            float _NoiseScale;
            float _NoiseSpeed;
            float _BlockSize;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash(i);
                float b = hash(i + float2(1,0));
                float c = hash(i + float2(0,1));
                float d = hash(i + float2(1,1));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) +
                       (c - a) * u.y * (1.0 - u.x) +
                       (d - b) * u.x * u.y;
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV *= _ScreenParams.xy / _BlockSize;
                screenUV = floor(screenUV) * _BlockSize / _ScreenParams.xy;

                fixed alpha = tex2D(_MainTex, i.texcoord).a;

                float edgeMask =
                    step(0.0, alpha) *
                    step(alpha, _EdgeWidth) +
                    step(1.0 - _EdgeWidth, alpha);

                float n = noise(i.texcoord * _NoiseScale + _Time.y * _NoiseSpeed);
                float2 noiseOffset = (n - 0.5) * _NoiseStrength * edgeMask;

                float2 uv = lerp(i.texcoord, screenUV, edgeMask) + noiseOffset;

                fixed4 tex = tex2D(_MainTex, uv) * i.color;

                fixed glow = tex.a * _GlowStrength;
                fixed3 result = tex.rgb + _GlowColor.rgb * glow;

                return fixed4(result, tex.a);
            }
            ENDCG
        }
    }
}