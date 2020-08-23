Shader "Unlit/OceanWaterURP"
{
    Properties
    {
        _BaseMap("Base Texture", 2D) = "white" {}
        _ShadowColor("Shadow Color", Color) = (0.35,0.4,0.45,1.0)
        _BaseColor("Water Color", Color) = (.2,.2,.8,1)
      
        _Gloss("Gloss", Float) = 1
        _Smoothness("Smoothness", Float) = 1

        _WaveA("Wave A (dir, steepness, wavelength)", Vector) = (1,0,0.5,10)
        _WaveB("Wave B", Vector) = (0,1,0.25,20)
        _WaveC("Wave C", Vector) = (1,1,0.15,10)

        [Toggle(_ALPHATEST_ON)] _EnableAlphaTest("Enable Alpha Cutoff", Float) = 0.0
        _Cutoff("Alpha Cutoff", Float) = 0.5

        [Toggle(_NORMALMAP)] _EnableBumpMap("Enable Normal/Bump Map", Float) = 0.0
        _BumpMap("Normal/Bump Texture", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1

        [Toggle(_EMISSION)] _EnableEmission("Enable Emission", Float) = 0.0
        _EmissionMap("Emission Texture", 2D) = "white" {}
        _EmissionColor("Emission Colour", Color) = (0, 0, 0, 0)
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline"}
        LOD 200

        Pass
        {
            Tags{"LightMode" = "UniversalForward"}

 /*           Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]*/

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

             // Material Keywords
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION

            #pragma shader_feature _RECEIVE_SHADOWS_OFF

            // LIGHT & SHADOWS FUNCTIONS IMPORTS URP
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

            CBUFFER_START(UnityPerMaterial)

            float4 _ShadowColor;
            float4 _BaseColor;
            float4 _BaseMap_ST; // Texture tiling & offset inspector values

            float4 _WaveA, _WaveB, _WaveC;


            float _BumpScale;
            float4 _EmissionColor;

            float _Gloss;
            half _Smoothness;
            float _Cutoff;

            CBUFFER_END

            struct Attributes // [appdata]
            {
                float4 positionOS : POSITION; 
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
                float2 lightmapUV : TEXCOORD1;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings // [v2f]
            {
                float4 positionCS   : SV_POSITION;
                float3 normalWS     : NORMAL;
                float4 color        : COLOR;

                float2 uv           : TEXCOORD0;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                float3 positionWS   : TEXCOORD2; 
                float4 shadowCoord  : TEXCOORD3;
                #ifdef _NORMALMAP
                float4 tangentWS                : TEXCOORD4;
                #endif
                float3 viewDirWS                : TEXCOORD5;
                half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO

             };

            static float TAU = 6.28318530718f;

            float3 GerstnerWave( float4 wave, float3 p, inout float3 tangent, inout float3 binormal )
            {
                float steepness = wave.z;
                float wavelength = wave.w;
                float k = TAU / wavelength;
                float c = sqrt(9.8 / k);
                float2 d = normalize(wave.xy);
                float f = k * (dot(d, p.xz) - c * _Time.y);
                float a = steepness / k;

                //p.x += d.x * (a * cos(f));
                //p.y = a * sin(f);
                //p.z += d.y * (a * cos(f));

                tangent += float3(
                    -d.x * d.x * (steepness * sin(f)),
                    d.x * (steepness * cos(f)),
                    -d.x * d.y * (steepness * sin(f))
                    );
                binormal += float3(
                    -d.x * d.y * (steepness * sin(f)),
                    d.y * (steepness * cos(f)),
                    -d.y * d.y * (steepness * sin(f))
                    );
                return float3(
                    d.x * (a * cos(f)),
                    a * sin(f),
                    d.y * (a * cos(f))
                    );
            }


            #if SHADER_LIBRARY_VERSION_MAJOR < 9
            // This function was added in URP v9.x.x versions
            // If we want to support URP versions before, we need to handle it instead.
            // Computes the world space view direction (pointing towards the viewer).
            float3 GetWorldSpaceViewDir(float3 positionWS)
            {
                if (unity_OrthoParams.w == 0)
                {
                    // Perspective
                    return _WorldSpaceCameraPos - positionWS;
                }
                else
                {
                    // Orthographic
                    float4x4 viewMat = GetWorldToViewMatrix();
                    return viewMat[2].xyz;
                }
            }
            #endif

             Varyings vert(Attributes input)
             {
                 Varyings output = (Varyings)0; // [v2f o]

                 UNITY_SETUP_INSTANCE_ID(input);
                 UNITY_TRANSFER_INSTANCE_ID(input, output);
                 UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);


                 // GERSTNER WAVES
                 float3 gridPoint = input.positionOS.xyz;
                 float3 tangent = 0; 
                 float3 binormal = 0;
                 float3 p = gridPoint;

                 p += GerstnerWave(_WaveA, gridPoint, tangent, binormal);
                 p += GerstnerWave(_WaveB, gridPoint, tangent, binormal);
                 p += GerstnerWave(_WaveC, gridPoint, tangent, binormal);

                 float3 normal = normalize(cross(binormal, tangent));

                 input.positionOS.xyz = p;
                 input.normalOS.xyz = normal;
                 input.tangentOS.xyz = tangent;
                 #ifdef _NORMALMAP
                 real sign = input.tangentOS.w * GetOddNegativeScale();
                 output.tangentWS = half4(normalInputs.tangentWS.xyz, sign);
                 #endif

                 // Object Space -> Clip Space
                 VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                 VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                 output.normalWS = normalInput.normalWS;
                 output.positionCS = vertexInput.positionCS;
                 output.positionWS = vertexInput.positionWS;

                 // View Direction
                 output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                 
                 // UVs & Vertex Colour
                 output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                 output.color = input.color;

                 // Vertex Lighting & Fog
                 half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
                 half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                 output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

                 // Baked Lighting & SH (used for Ambient if there is no baked)
                 OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                 OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

                 // Shadows
                 #ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
                 output.shadowCoord = GetShadowCoord(vertexInput);
                 #endif
                 return output;
             }

             InputData InitializeInputData(Varyings IN, half3 normalTS)
             {
                 InputData inputData = (InputData)0;

                 #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                 inputData.positionWS = IN.positionWS;
                 #endif

                 half3 viewDirWS = SafeNormalize(IN.viewDirWS);

                 #ifdef _NORMALMAP
                 float sgn = IN.tangentWS.w; // should be either +1 or -1
                 float3 bitangent = sgn * cross(IN.normalWS.xyz, IN.tangentWS.xyz);
                 inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(IN.tangentWS.xyz, bitangent.xyz, IN.normalWS.xyz));
                 #else
                 inputData.normalWS = IN.normalWS;
                 #endif

                 inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                 inputData.viewDirectionWS = viewDirWS;

                 #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                 inputData.shadowCoord = IN.shadowCoord;
                 #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                 inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                 #else
                 inputData.shadowCoord = float4(0, 0, 0, 0);
                 #endif

                 inputData.fogCoord = IN.fogFactorAndVertexLight.x;
                 inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
                 inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, inputData.normalWS);
                 return inputData;
             }


             // Surface Data Standin for organizing data
             SurfaceData InitializeSurfaceData(Varyings IN)
             {
                 SurfaceData surfaceData = (SurfaceData)0;
                 // Note, we can just use SurfaceData surfaceData; here and not set it.
                 // However we then need to ensure all values in the struct are set before returning.
                 // By casting 0 to SurfaceData, we automatically set all the contents to 0.

                 half4 albedoAlpha = SampleAlbedoAlpha(IN.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                 surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
                 surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb ;

                 // Not supporting the metallic/specular map or occlusion map
                 // for an example of that see : https://github.com/Unity-Technologies/Graphics/blob/master/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

                 surfaceData.smoothness = _Smoothness;
                 surfaceData.normalTS = SampleNormal(IN.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
                 surfaceData.emission = SampleEmission(IN.uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
                 surfaceData.occlusion = 1;
                 return surfaceData;
             }

               half4 frag(Varyings input) : SV_Target
               {
                   UNITY_SETUP_INSTANCE_ID(input);
                   UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                   SurfaceData surfaceData = InitializeSurfaceData(input);
                   InputData inputData = InitializeInputData(input, surfaceData.normalTS);
                  
                   half4 col = UniversalFragmentPBR(inputData, surfaceData.albedo, surfaceData.metallic,
                       surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion,
                       surfaceData.emission, surfaceData.alpha);
                   
                   // apply fog
                   col.rgb = MixFog(col.rgb, inputData.fogCoord);
                   col.a = saturate(col.a);
                   return col;

                   half4 colorShadow = half4(1, 1, 1, 1);
                   #ifdef _MAIN_LIGHT_SHADOWS //  #START Direct specular Light --------------------------


                   VertexPositionInputs vertexInput = (VertexPositionInputs)0;
                   vertexInput.positionWS = input.positionWS;

                   float4 shadowCoord = input.shadowCoord;
                   half shadowAttenutation = MainLightRealtimeShadow(shadowCoord);
                   colorShadow = lerp(half4(1, 1, 1, 1), _ShadowColor, (1.0 - shadowAttenutation) * _ShadowColor.a);
                 //  colorShadow.rgb = MixFogColor(colorShadow.rgb, half3(1, 1, 1), input.fogCoord);
                   #endif
                   float3 normal = normalize(input.normalWS);
                   Light mainLight = GetMainLight();
                   float3 lightDir = mainLight.direction;

                   float3 camPos = _WorldSpaceCameraPos;
                   float3 fragToCam = camPos - input.positionWS;
                   float3 viewDir = normalize(fragToCam);

                   float3 viewReflect = reflect(-viewDir, normal);

                   float specularFalloff = max(0, dot(viewReflect, lightDir));
                   specularFalloff = pow(specularFalloff, _Gloss); // Blinn-Phong     
                   half4 lightRes = specularFalloff * colorShadow;
                   // -------------------- #END -------------------------------------------

                  float atten = mainLight.distanceAttenuation; //Shadows

                  return col + lightRes * atten;
              }
              ENDHLSL
          }
    }
}
