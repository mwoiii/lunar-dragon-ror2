Shader "MinigameRing"
{
    // this is not the same as the shadergraph - vertex color multiplication killing the material rgb ingame so that is gone in here
    // selectionring is using shadergraph to flash as it has no texture in the material
    Properties
    {
        _BorderWidth("BorderWidth", Range(0, 0.33)) = 0
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _AlphaOffset("AlphaOffset", Range(0, 1)) = 0
        _OutlineWidth("OutlineWidth", Range(0, 1)) = 0
        _OutlineColor("OutlineColor", Color) = (0, 0, 0, 0)
        [HideInInspector]_BUILTIN_QueueOffset("Float", Float) = 0
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1_Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            // RenderPipeline: <None>
            "RenderType"="Transparent"
            "BuiltInMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="BuiltInUnlitSubTarget"
        }
        Pass
        {
            Name "Pass"
            Tags
            {
                "LightMode" = "ForwardBase"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        Stencil{
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile_fwdbase
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _BorderWidth;
        float4 _MainTex_TexelSize;
        float _AlphaOffset;
        float _OutlineWidth;
        float4 _OutlineColor;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Subtract_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A - B;
        }
        
        void Unity_Clamp_float4(float4 In, float4 Min, float4 Max, out float4 Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.tex, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.samplerstate, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_R_4 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.r;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_G_5 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.g;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_B_6 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.b;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.a;
            float2 _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3);
            float _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2;
            Unity_Distance_float2(_TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3, float2(0, 0), _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2);
            float _Split_72e18ffc983846f6b3173c085c9318f4_R_1 = IN.VertexColor[0];
            float _Split_72e18ffc983846f6b3173c085c9318f4_G_2 = IN.VertexColor[1];
            float _Split_72e18ffc983846f6b3173c085c9318f4_B_3 = IN.VertexColor[2];
            float _Split_72e18ffc983846f6b3173c085c9318f4_A_4 = IN.VertexColor[3];
            float _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2);
            float _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0 = _OutlineWidth;
            float _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2);
            float _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2);
            float _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2, _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2);
            float _Property_008208fe448e4d879ba9db95284f48b6_Out_0 = _BorderWidth;
            float _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_008208fe448e4d879ba9db95284f48b6_Out_0, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2);
            float _Add_af3c2b73b26749a39384b60f8b00144a_Out_2;
            Unity_Add_float(_Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2);
            float _Step_30500afbcab542928b2e5b9e2550b98c_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2, _Step_30500afbcab542928b2e5b9e2550b98c_Out_2);
            float _Step_d44b370db6a24527a83140f571b836dd_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2);
            float _Subtract_82ad6c4eef694db8803502099b39d647_Out_2;
            Unity_Subtract_float(_Step_30500afbcab542928b2e5b9e2550b98c_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2);
            float _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2;
            Unity_Add_float(_Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2);
            float4 _Subtract_0688ed720bac4f4c90e0974b77693e62_Out_2;
            Unity_Subtract_float4(_SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0, (_Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2.xxxx), _Subtract_0688ed720bac4f4c90e0974b77693e62_Out_2);
            float4 _Clamp_98f51cef831443439da789ed54b4f83f_Out_3;
            Unity_Clamp_float4(_Subtract_0688ed720bac4f4c90e0974b77693e62_Out_2, float4(0, 0, 0, 0), float4(1, 1, 1, 1), _Clamp_98f51cef831443439da789ed54b4f83f_Out_3);
            float4 _Property_69e5eb267ffa4ec68199bc04bb86f4da_Out_0 = _OutlineColor;
            float4 _Multiply_168beea5f59344c190b0ddd3a68ee490_Out_2;
            Unity_Multiply_float4_float4((_Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2.xxxx), _Property_69e5eb267ffa4ec68199bc04bb86f4da_Out_0, _Multiply_168beea5f59344c190b0ddd3a68ee490_Out_2);
            float4 _Add_84231e9224ce4ba89284efb27364c00b_Out_2;
            Unity_Add_float4(_Clamp_98f51cef831443439da789ed54b4f83f_Out_3, _Multiply_168beea5f59344c190b0ddd3a68ee490_Out_2, _Add_84231e9224ce4ba89284efb27364c00b_Out_2);
            float _Subtract_a3545003acdc49c28497999f69606e49_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_a3545003acdc49c28497999f69606e49_Out_2);
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1;
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors = float (1);
            Unity_InvertColors_float(_Subtract_a3545003acdc49c28497999f69606e49_Out_2, _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1);
            float _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2;
            Unity_Subtract_float(_SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1, _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2);
            float _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0 = _AlphaOffset;
            float _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2;
            Unity_Subtract_float(_Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2, _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0, _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2);
            float _Add_79bfdd00da91499abfef159028202f06_Out_2;
            Unity_Add_float(_Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2, _Add_79bfdd00da91499abfef159028202f06_Out_2);
            float _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            Unity_Clamp_float(_Add_79bfdd00da91499abfef159028202f06_Out_2, 0, 1, _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3);
            surface.BaseColor = (_Add_84231e9224ce4ba89284efb27364c00b_Out_2.xyz);
            surface.Alpha = _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.color      = attributes.color;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_shadowcaster
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _BorderWidth;
        float4 _MainTex_TexelSize;
        float _AlphaOffset;
        float _OutlineWidth;
        float4 _OutlineColor;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.tex, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.samplerstate, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_R_4 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.r;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_G_5 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.g;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_B_6 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.b;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.a;
            float2 _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3);
            float _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2;
            Unity_Distance_float2(_TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3, float2(0, 0), _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2);
            float _Split_72e18ffc983846f6b3173c085c9318f4_R_1 = IN.VertexColor[0];
            float _Split_72e18ffc983846f6b3173c085c9318f4_G_2 = IN.VertexColor[1];
            float _Split_72e18ffc983846f6b3173c085c9318f4_B_3 = IN.VertexColor[2];
            float _Split_72e18ffc983846f6b3173c085c9318f4_A_4 = IN.VertexColor[3];
            float _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2);
            float _Property_008208fe448e4d879ba9db95284f48b6_Out_0 = _BorderWidth;
            float _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_008208fe448e4d879ba9db95284f48b6_Out_0, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2);
            float _Step_d44b370db6a24527a83140f571b836dd_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2);
            float _Subtract_a3545003acdc49c28497999f69606e49_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_a3545003acdc49c28497999f69606e49_Out_2);
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1;
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors = float (1);
            Unity_InvertColors_float(_Subtract_a3545003acdc49c28497999f69606e49_Out_2, _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1);
            float _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2;
            Unity_Subtract_float(_SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1, _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2);
            float _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0 = _AlphaOffset;
            float _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2;
            Unity_Subtract_float(_Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2, _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0, _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2);
            float _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0 = _OutlineWidth;
            float _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2);
            float _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2);
            float _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2, _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2);
            float _Add_af3c2b73b26749a39384b60f8b00144a_Out_2;
            Unity_Add_float(_Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2);
            float _Step_30500afbcab542928b2e5b9e2550b98c_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2, _Step_30500afbcab542928b2e5b9e2550b98c_Out_2);
            float _Subtract_82ad6c4eef694db8803502099b39d647_Out_2;
            Unity_Subtract_float(_Step_30500afbcab542928b2e5b9e2550b98c_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2);
            float _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2;
            Unity_Add_float(_Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2);
            float _Add_79bfdd00da91499abfef159028202f06_Out_2;
            Unity_Add_float(_Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2, _Add_79bfdd00da91499abfef159028202f06_Out_2);
            float _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            Unity_Clamp_float(_Add_79bfdd00da91499abfef159028202f06_Out_2, 0, 1, _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3);
            surface.Alpha = _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.color      = attributes.color;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SceneSelectionPass
        #define BUILTIN_TARGET_API 1
        #define SCENESELECTIONPASS 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _BorderWidth;
        float4 _MainTex_TexelSize;
        float _AlphaOffset;
        float _OutlineWidth;
        float4 _OutlineColor;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.tex, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.samplerstate, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_R_4 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.r;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_G_5 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.g;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_B_6 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.b;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.a;
            float2 _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3);
            float _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2;
            Unity_Distance_float2(_TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3, float2(0, 0), _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2);
            float _Split_72e18ffc983846f6b3173c085c9318f4_R_1 = IN.VertexColor[0];
            float _Split_72e18ffc983846f6b3173c085c9318f4_G_2 = IN.VertexColor[1];
            float _Split_72e18ffc983846f6b3173c085c9318f4_B_3 = IN.VertexColor[2];
            float _Split_72e18ffc983846f6b3173c085c9318f4_A_4 = IN.VertexColor[3];
            float _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2);
            float _Property_008208fe448e4d879ba9db95284f48b6_Out_0 = _BorderWidth;
            float _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_008208fe448e4d879ba9db95284f48b6_Out_0, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2);
            float _Step_d44b370db6a24527a83140f571b836dd_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2);
            float _Subtract_a3545003acdc49c28497999f69606e49_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_a3545003acdc49c28497999f69606e49_Out_2);
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1;
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors = float (1);
            Unity_InvertColors_float(_Subtract_a3545003acdc49c28497999f69606e49_Out_2, _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1);
            float _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2;
            Unity_Subtract_float(_SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1, _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2);
            float _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0 = _AlphaOffset;
            float _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2;
            Unity_Subtract_float(_Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2, _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0, _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2);
            float _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0 = _OutlineWidth;
            float _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2);
            float _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2);
            float _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2, _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2);
            float _Add_af3c2b73b26749a39384b60f8b00144a_Out_2;
            Unity_Add_float(_Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2);
            float _Step_30500afbcab542928b2e5b9e2550b98c_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2, _Step_30500afbcab542928b2e5b9e2550b98c_Out_2);
            float _Subtract_82ad6c4eef694db8803502099b39d647_Out_2;
            Unity_Subtract_float(_Step_30500afbcab542928b2e5b9e2550b98c_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2);
            float _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2;
            Unity_Add_float(_Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2);
            float _Add_79bfdd00da91499abfef159028202f06_Out_2;
            Unity_Add_float(_Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2, _Add_79bfdd00da91499abfef159028202f06_Out_2);
            float _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            Unity_Clamp_float(_Add_79bfdd00da91499abfef159028202f06_Out_2, 0, 1, _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3);
            surface.Alpha = _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.color      = attributes.color;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS ScenePickingPass
        #define BUILTIN_TARGET_API 1
        #define SCENEPICKINGPASS 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _BorderWidth;
        float4 _MainTex_TexelSize;
        float _AlphaOffset;
        float _OutlineWidth;
        float4 _OutlineColor;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0 = SAMPLE_TEXTURE2D(_Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.tex, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.samplerstate, _Property_fd99efbb7e2c4798b3b2d5530ee06f52_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_R_4 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.r;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_G_5 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.g;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_B_6 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.b;
            float _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7 = _SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_RGBA_0.a;
            float2 _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), float2 (-0.5, -0.5), _TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3);
            float _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2;
            Unity_Distance_float2(_TilingAndOffset_79d37864521b404a80365a7f24c4d134_Out_3, float2(0, 0), _Distance_c2e083963919498a8948f9fa2249bbcd_Out_2);
            float _Split_72e18ffc983846f6b3173c085c9318f4_R_1 = IN.VertexColor[0];
            float _Split_72e18ffc983846f6b3173c085c9318f4_G_2 = IN.VertexColor[1];
            float _Split_72e18ffc983846f6b3173c085c9318f4_B_3 = IN.VertexColor[2];
            float _Split_72e18ffc983846f6b3173c085c9318f4_A_4 = IN.VertexColor[3];
            float _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2);
            float _Property_008208fe448e4d879ba9db95284f48b6_Out_0 = _BorderWidth;
            float _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_008208fe448e4d879ba9db95284f48b6_Out_0, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2);
            float _Step_d44b370db6a24527a83140f571b836dd_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2);
            float _Subtract_a3545003acdc49c28497999f69606e49_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_a3545003acdc49c28497999f69606e49_Out_2);
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1;
            float _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors = float (1);
            Unity_InvertColors_float(_Subtract_a3545003acdc49c28497999f69606e49_Out_2, _InvertColors_1714c15aa5b440639cccf06b21f14c74_InvertColors, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1);
            float _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2;
            Unity_Subtract_float(_SampleTexture2D_8057b4b84f3a45a6af3d60818bc7802b_A_7, _InvertColors_1714c15aa5b440639cccf06b21f14c74_Out_1, _Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2);
            float _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0 = _AlphaOffset;
            float _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2;
            Unity_Subtract_float(_Subtract_9b193cf36485486bb4109322fe6ed8ba_Out_2, _Property_a3aecc616b0c45dcbf9b9ec0fae9dfa0_Out_0, _Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2);
            float _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0 = _OutlineWidth;
            float _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2;
            Unity_Subtract_float(_Split_72e18ffc983846f6b3173c085c9318f4_A_4, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2);
            float _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Subtract_7afd352b0b7d48da8a9197102eae8e4f_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2);
            float _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2;
            Unity_Subtract_float(_Step_6c0d8b016f294d3aa0ca6bb535ec6e66_Out_2, _Step_c2e989ea5207438da452bfaaf7cc8e76_Out_2, _Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2);
            float _Add_af3c2b73b26749a39384b60f8b00144a_Out_2;
            Unity_Add_float(_Subtract_128a3946beac4d4fa8084bfc3699acaf_Out_2, _Property_a9d80494149f4e6e84e0c5b6c8855ca6_Out_0, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2);
            float _Step_30500afbcab542928b2e5b9e2550b98c_Out_2;
            Unity_Step_float(_Distance_c2e083963919498a8948f9fa2249bbcd_Out_2, _Add_af3c2b73b26749a39384b60f8b00144a_Out_2, _Step_30500afbcab542928b2e5b9e2550b98c_Out_2);
            float _Subtract_82ad6c4eef694db8803502099b39d647_Out_2;
            Unity_Subtract_float(_Step_30500afbcab542928b2e5b9e2550b98c_Out_2, _Step_d44b370db6a24527a83140f571b836dd_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2);
            float _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2;
            Unity_Add_float(_Subtract_d93f08e316fd4e03868cac34ac9d4d4f_Out_2, _Subtract_82ad6c4eef694db8803502099b39d647_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2);
            float _Add_79bfdd00da91499abfef159028202f06_Out_2;
            Unity_Add_float(_Subtract_399a7886a94e42b3ba2b5bb80edb8c50_Out_2, _Add_ca104270fae544aaa6d9d1e877eac8d8_Out_2, _Add_79bfdd00da91499abfef159028202f06_Out_2);
            float _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            Unity_Clamp_float(_Add_79bfdd00da91499abfef159028202f06_Out_2, 0, 1, _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3);
            surface.Alpha = _Clamp_30cd06ebb1d8421a8ddb37a2340236d3_Out_3;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.color      = attributes.color;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        ENDHLSL
        }
    }
    CustomEditorForRenderPipeline "UnityEditor.Rendering.BuiltIn.ShaderGraph.BuiltInUnlitGUI" ""
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}