Shader "Unlit/OceanWaterURP"
{
    Properties
    {
        _BaseMap("Base Texture", 2D) = "white" {}
        _ShadowColor("Shadow Color", Color) = (0.35,0.4,0.45,1.0)
        _BaseColor("Water Color", Color) = (.2,.2,.8,1)

        _WaterFogColor("Water Fog Color", Color) = (0, 0, 0, 0)
        _WaterFogDensity("Water Fog Density", Range(0, 2)) = 0.1
      
        _Gloss("Gloss", Float) = 1
        _Smoothness("Smoothness", Float) = 1


        _WaveA("Wave A (dir [x,y], steepness, wavelength)", Vector) = (1,0,0.5,10)
        _WaveB("Wave B", Vector) = (0,1,0.25,20)
        _WaveC("Wave C", Vector) = (1,1,0.15,10)

        [Toggle(_ALPHATEST_ON)] _EnableAlphaTest("Enable Alpha Cutoff", Float) = 0.0
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5

        [Toggle(_NORMALMAP)] _EnableBumpMap("Enable Normal/Bump Map", Float) = 0.0
        _BumpMap("Normal/Bump Texture", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1

        [Toggle(_EMISSION)] _EnableEmission("Enable Emission", Float) = 0.0
        _EmissionMap("Emission Texture", 2D) = "white" {}
        _EmissionColor("Emission Colour", Color) = (0, 0, 0, 0)

        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
        [Enum(Off, 0, On, 1)] _ZWrite("Z Write", Float) = 1
    }

        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent+0" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
        LOD 300

        Pass
        {
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

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

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define SHADERPASS_FORWARD

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl" // This is now Grabpass output texture

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


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

            float _OutlineThickness;
            float _DepthSensitivity;

            float4 _WaterFogColor;
            float _WaterFogDensity;

            CBUFFER_END

            float4 _CameraDepthTexture_TexelSize;


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
                //#ifdef _NORMALMAP
                float4 tangentWS                : TEXCOORD4;
                //#endif
                float3 viewDirectionWS                : TEXCOORD5;
                half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light
                float3 normalOS : TEXCOORD7;
               

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO

             };

            static float TAU = 6.28318530718f;

            float3 GerstnerWaveOLD( float4 wave, float3 p, inout float3 tangent, inout float3 binormal )
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

            float3 GerstnerWave(float4 wave, float3 p, inout float3 tangent, inout float3 bitangent, inout float3 normal)
            {
                float steepness = wave.z;
                float wavelength = wave.w;
                float k = TAU / wavelength;
                float a = steepness / k; //amplitude

                float c = sqrt(9.8 / k);
                float2 d = normalize(wave.xy);
                float f = k * (dot(d, p.xz) - c * _Time.y);

                //p.x += d.x * (a * cos(f));
                //p.y = a * sin(f);
                //p.z += d.y * (a * cos(f));

                float3 _bitangent = float3( //flipped????
                    1 - ( d.x * d.x * steepness * cos(f)),
                    d.x * steepness * sin(f),
                    - ( d.x * d.y * steepness * cos(f) )
                    );
                float3 _tangent = float3(
                    - ( d.x * d.y * steepness * cos(f) ),
                    d.y * steepness * sin(f),
                    1 - ( d.y * d.y * steepness * cos(f) )
                    );

                _bitangent = normalize(_bitangent);
                _tangent = normalize(_tangent);

                float3 _normal = cross(_tangent, _bitangent);
                normal += _normal;

                bitangent += _bitangent;
                tangent += _tangent;
                return float3(
                    d.x * (a * sin(f)),
                    a * cos(f),
                    d.y * (a * sin(f))
                    );
            }

            float3 ColorBelowWater(float4 screenPos, float3 tangentSpaceNormal)
            {

                // Refraction
                float2 uvOffset = tangentSpaceNormal.xy * 0.4;
                float2 uv = (screenPos.xy + uvOffset) / screenPos.w;
                uvOffset.y *= _CameraDepthTexture_TexelSize.z * abs(_CameraDepthTexture_TexelSize.y);


              //  float2 uv = screenPos.xy / screenPos.w;                   
                float depth = SampleSceneDepth( uv ); // sample depth buffer
                
                float backgroundDepth = LinearEyeDepth(depth, _ZBufferParams); // Get Dist from Camera in Eye Units
                //float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.w); //Get Camera Far Plance
   
                float depthDifference = backgroundDepth - _ProjectionParams.w;
                float fogFactor = exp2(-_WaterFogDensity * depthDifference);

                float3 backgroundColor = SampleSceneColor( uv ); // sample scene prerender pass
                
                return lerp(_WaterFogColor, backgroundColor, fogFactor);              
            }

            #if SHADER_LIBRARY_VERSION_MAJOR < 9
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
                 //float3 gridPoint = input.positionOS.xyz;
                 //float3 tangent = 0; 
                 //float3 binormal = 0;
                 //float3 normal = 0;
                 //float3 p = gridPoint;

                 //p += GerstnerWave(_WaveA, gridPoint, tangent, binormal, normal);
                 //p += GerstnerWave(_WaveB, gridPoint, tangent, binormal, normal);
                 //p += GerstnerWave(_WaveC, gridPoint, tangent, binormal, normal);

                 //input.positionOS.xyz = p;
                 //input.normalOS.xyz = normal;
                 //input.tangentOS.xyz = tangent;

                 // Object Space -> Clip Space
                 VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                 VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                 // Normals & Tangents
                 output.normalWS = normalInput.normalWS;
                 output.positionCS = vertexInput.positionCS;
                 output.positionWS = vertexInput.positionWS;

                 output.normalOS = input.normalOS;

                 // View Direction
                 output.viewDirectionWS = GetWorldSpaceViewDir(vertexInput.positionWS);

               //  output.tangentWS = half4(normalInput.tangentWS, output.viewDirectionWS.y);

                 #ifdef _NORMALMAP
                 real sign = input.tangentOS.w * GetOddNegativeScale();
                 output.tangentWS = half4(normalInput.tangentWS.xyz, sign);
                 #endif


                 
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

                 half3 viewDirWS = SafeNormalize(IN.viewDirectionWS);

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

                 float4 screenPos = ComputeScreenPos(TransformWorldToHClip(IN.positionWS), _ProjectionParams.x);

                 half4 albedoAlpha = SampleAlbedoAlpha(IN.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                 surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
                 
                 surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;

                 // Not supporting the metallic/specular map or occlusion map
                 // for an example of that see : https://github.com/Unity-Technologies/Graphics/blob/master/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

                 surfaceData.smoothness = _Smoothness;
                 surfaceData.normalTS = SampleNormal(IN.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
                 surfaceData.emission = SampleEmission(IN.uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
                 surfaceData.emission = ColorBelowWater(screenPos, IN.normalOS);

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
                 //  col.a = saturate(col.a);
                   return col;
                 //  return float4(inputData.normalWS, 0);
     /*              return col;
                   col = _BaseColor;*/

                   float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS.xyz);
                   Light light = GetMainLight(shadowCoord);

                   half3 diffuse = LightingLambert(light.color, light.direction, input.normalWS);

                   return half4(col.rgb * diffuse * diffuse * light.shadowAttenuation, col.a);

                   // col = _BaseColor;
                   

                   half4 colorShadow = half4(.2, .2, .6, 1);
                   Light mainLight = GetMainLight(shadowCoord);


                   //Lighting Calculate(Lambert)              
                   float NdotL = saturate(dot(normalize(_MainLightPosition.xyz), input.normalWS));
                   float3 ambient = SampleSH(input.normalWS);
                   
                    half receiveshadow = MainLightRealtimeShadow(shadowCoord);
                    //return receiveshadow;
                   // col.rgb *= NdotL * _MainLightColor.rgb * receiveshadow + ambient;

                   col.rgb *= NdotL * _MainLightColor.rgb * mainLight.shadowAttenuation + ambient;
                   col.rgb *= NdotL * _MainLightColor.rgb + ambient;
                   return col;

                   #ifdef _MAIN_LIGHT_SHADOWS //  #START Direct specular Light --------------------------


                //   float4 shadowCoord = input.shadowCoord;

                   ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
                   half shadowStrength = GetMainLightShadowStrength();
                   half shadowAttenuation = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
                       sampler_MainLightShadowmapTexture),
                       shadowSamplingData, shadowStrength, false);

                   //half shadowAttenuation = MainLightRealtimeShadow(shadowCoord);
                  // colorShadow = lerp(half4(1, 1, 1, 1), _ShadowColor, (1.0 - shadowAttenuation) * _ShadowColor.a);
                   float attenuatedLightColor = mainLight.color * mainLight.distanceAttenuation * shadowAttenuation;
                 //  colorShadow.rgb = MixFogColor(colorShadow.rgb, half3(1, 1, 1), input.fogCoord);
                   return attenuatedLightColor;

                   #endif
                   float3 normal = input.normalWS;
                   float3 lightDir = mainLight.direction;

                   float3 camPos = _WorldSpaceCameraPos;
                   float3 fragToCam = camPos - input.positionWS;
                   //float3 viewDir = normalize(fragToCam);
                   float3 viewDir = input.viewDirectionWS;

                   float3 viewReflect = reflect(-viewDir, normal);

                   float specularFalloff = max(0, dot(viewReflect, lightDir));
                   specularFalloff = pow(specularFalloff, _Gloss); // Blinn-Phong     
                   half4 lightRes = specularFalloff * colorShadow;
                   // -------------------- #END -------------------------------------------

                  float atten = mainLight.distanceAttenuation; //Shadows
                 
                  return col;
                  return col + lightRes * atten;
              }

              ENDHLSL


          }

          UsePass "Universal Render Pipeline/Lit/DepthOnly"

    }
}
