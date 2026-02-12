Shader "UI/UI_CircleHole"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,0.7)
        _Center("Hole Center (Normalized)", Vector) = (0.5,0.5,0,0)
        _Radius("Hole Radius (Normalized)", Float) = 0.15
        _Softness("Edge Softness", Float) = 0.02
        _RectSize("Rect Size", Vector) = (1080,1920,0,0)
    }

        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _Center;
            float _Radius;
            float _Softness;
            float4 _RectSize;

            struct appdata
            {
                float4 vertex : POSITION; // LOCAL SPACE
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 localPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Convert local space normalized 0–1
                float2 uv = (i.localPos / _RectSize.xy) + 0.5;

                // Aspect correction
                float aspect = _RectSize.x / _RectSize.y;
                uv.x *= aspect;
                float2 center = _Center.xy;
                center.x *= aspect;

                float dist = distance(uv, center);
                float alpha = smoothstep(_Radius, _Radius + _Softness, dist);

                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}