Shader "UI/RoundedCornersCustom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WidthHeightRadius ("Width, Height, Radius", Vector) = (0, 0, 0, 0)
        _OuterUV ("Outer UV", Vector) = (0, 0, 1, 1)

        // --- Stencil properties (for masking support) ---
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        // Stencil block for masking support
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "./Includes/SDFUtils.cginc"
            #include "./Includes/ShaderSetup.cginc"

            float4 _WidthHeightRadius; // Width, Height, Corner Radius
            float4 _OuterUV;
            sampler2D _MainTex;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uvSample = i.uv;
                uvSample.x = (uvSample.x - _OuterUV.x) / (_OuterUV.z - _OuterUV.x);
                uvSample.y = (uvSample.y - _OuterUV.y) / (_OuterUV.w - _OuterUV.y);

                half4 color = tex2D(_MainTex, uvSample);

                // Apply the alpha based on the corner radius calculation
                float alpha = CalcAlpha(i.uv, _WidthHeightRadius.xy, _WidthHeightRadius.z);
                color.a *= alpha;

                return color;
            }
            ENDCG
        }
    }
}
