Shader "Unlit/InfluenceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color0("Color 0", Color) = (0, 0, 0, 1)

        _Color1("Color 1", Color) = (10, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float3 colors[2];
            float maxWeight = 255;

            float3 chosenColor;

            float cVal0;
            float cVal1;
            float cVal2;

            float4 _Color0;
            float4 _Color1;

            void init()
            {
                colors[0] = _Color0;
                colors[1] = _Color1;
                chosenColor = _Color0;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                
                return false ? float4(0, 0, 0, 1) : float4(cVal0, cVal1, cVal2, 1);
            }
            ENDCG
        }
    }
}
