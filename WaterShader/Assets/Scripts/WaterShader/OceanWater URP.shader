Shader "Unlit/OceanWater"
{
    Properties
    {
        _Color ("Water Color", Color) = (.2,.2,.8,1)
        _Amplitude("Amplitude", Range(0,5)) = 1
        _WaveSize("WaveSize", Range(0,3)) = .6
        _WaveSpeed("WaveSpeed", Float) = 1
        _Gloss("Gloss", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "UniversalForward"}
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            //#pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            //#pragma target 2.0

            //#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            //#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            //#pragma multi_compile _ _SHADOWS_SOFT


            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            //CBUFFER_START(UnityPerMaterial)
            //float4 _ShadowColor;
            //CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
               // float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                LIGHTING_COORDS(1, 2)
            };

            float4 _Color;
            float _Amplitude;
            float _WaveSize;
            float _WaveSpeed;
            float _Gloss;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); //worldPos


                float f = _WaveSize * (v.vertex.x - _WaveSpeed * _Time.y);
                v.vertex.y = _Amplitude * sin(f);


                float3 tangent = normalize(float3(1, _WaveSize * _Amplitude * cos(f), 0));
                o.normal = float3(-tangent.y, tangent.x, 0);
                 
                o.vertex = UnityObjectToClipPos( v.vertex ); //clipSpacePos


                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                // -------------------------- #START Direct specular Light
                float3 normal = normalize(i.normal);
                float lightDir = _WorldSpaceLightPos0.xyz;

                float3 camPos = _WorldSpaceCameraPos;
                float3 fragToCam = camPos - i.worldPos;
                float3 viewDir = normalize(fragToCam);

                float3 viewReflect = reflect(-viewDir, normal);

                float specularFalloff = max(0, dot(viewReflect, lightDir));                
                specularFalloff = pow(specularFalloff, _Gloss); // Blinn-Phong
                // -------------------- #END

                fixed atten = LIGHT_ATTENUATION(i); //Shadows

                return col + specularFalloff * atten;
            }
            ENDCG
        }
    }
}
