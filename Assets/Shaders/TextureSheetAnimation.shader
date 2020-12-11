// ================= License ====================
//
// TextureSheetAnimation.shader
//
// Copyright (c) 2020 hatuxes
//
// Released under the MIT license.
// Check README.md when you use this shader.
//
// ==============================================

Shader "HC/Unlit/TextureSheetAnimation"
{
    Properties
    {
        _Color ("MainColor", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
        [Space]
        _DivisionX ("X Division number", Float) = 1
        _DivisionY ("Y Division number", Float) = 1
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            float _DivisionX, _DivisionY, _Speed;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 uv = v.uv;

                uv.x /= _DivisionX;
                uv.x += fmod(1 / _DivisionX * floor(_Time.y * _Speed), _DivisionX);

                uv.y /= _DivisionY;
                uv.y += fmod(1 / _DivisionY * floor(_Time.y * _Speed / _DivisionX), _DivisionY);

                o.uv = uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
