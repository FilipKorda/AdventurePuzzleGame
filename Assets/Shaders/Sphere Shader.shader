Shader "Custom/SphereMask"
{
     SubShader
    {
        Tags { "Queue"="Geometry" }

        ColorMask 0
        ZWrite Off

        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }

        Pass {}
    }
}