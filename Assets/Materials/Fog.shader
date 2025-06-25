Shader "Custom/Fog"
{
    Properties
    {
        _CloudColor("Cloud Color", Color) = (1, 1, 1, 1)
        _SkyColor("Sky Color", Color) = (0.5, 0.7, 1.0, 1)
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _Scale("Noise Scale", Float) = 1.0
        _Speed("Scroll Speed", Float) = 0.1
        _Cutoff("Cloud Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float4 _CloudColor;
            float4 _SkyColor;
            float _Scale;
            float _Speed;
            float _Cutoff;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _NoiseTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                uv += _Time * _Speed;

                float noiseVal = tex2D(_NoiseTex, uv * _Scale).r;

                float cloudMask = smoothstep(_Cutoff, 1.0, noiseVal);

                float3 color = lerp(_SkyColor.rgb, _CloudColor.rgb, cloudMask);
                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
