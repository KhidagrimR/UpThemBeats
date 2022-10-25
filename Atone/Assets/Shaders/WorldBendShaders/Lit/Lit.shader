Shader "Atone/WorldBend/Lit"
{
    // C'est une copie du Lit.shader de Unity, avec des modification pour incorporer le WorldBend de Atone
    Properties
    {
        // Specular (0) vs Metallic (1) workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _Cull("__cull", Float) = 2.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0

        _ReceiveShadows("Receive Shadows", Float) = 1.0
        // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0

        // ObsoleteProperties
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        [HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
        [HideInInspector] _Glossiness("Smoothness", Float) = 0.0
        [HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0
    }
    SubShader
    {
        // Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
        // this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
        // material work with both Universal Render Pipeline and Builtin Unity Pipeline
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 300

        // ----------------------------------------------------------------------------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            // All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
            // #pragma prefer_hlslcc gles
            // #pragma exclude_renderers d3d11_9x
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _OCCLUSIONMAP

            #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

    #pragma shader_feature_local WORLDBEND_DISABLED_ON
    #pragma shader_feature_local WORLDBEND_NORMAL_TRANSFORMATION_ON
    #include "../Core/TransformAliases.hlsl"
    // Source de l'erreur "Redefinition of _Time at File [...] (40)".

            #include "LitForwardPass.hlsl"
            ENDHLSL
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------
        //  ShadowCaster

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            // #pragma prefer_hlslcc gles
            // #pragma exclude_renderers d3d11_9x
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

    #pragma shader_feature_local WORLDBEND_DISABLED_ON
    #pragma shader_feature_local WORLDBEND_NORMAL_TRANSFORMATION_ON
    #include "../Core/TransformAliases.hlsl"

            #include "ShadowCasterPass.hlsl"
            ENDHLSL
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------
        //  DepthOnly

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            //#pragma prefer_hlslcc gles
            // #pragma exclude_renderers d3d11_9x
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            
    #pragma shader_feature_local WORLDBEND_DISABLED_ON
    // #pragma shader_feature_local WORLDBEND_NORMAL_TRANSFORMATION_ON    //Depth does need Normal Transform
    #include "../Core/TransformAliases.hlsl"

            #include "DepthOnlyPass.hlsl"
            ENDHLSL
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------
        //  Meta
        //  This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            // #pragma prefer_hlslcc gles
            // #pragma exclude_renderers d3d11_9x
            #pragma only_renderers gles gles3 glcore d3d11

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMetaLit // previously UniversalFragmentMeta

            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma shader_feature _SPECGLOSSMAP

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"

            ENDHLSL
        }
        // ----------------------------------------------------------------------------------------------------------------------------------------
        //  Universal2D
        //  Utilisé pour l'éclairage 2D, pas utile en principe
        Pass
        {            
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            // #pragma prefer_hlslcc gles
            // #pragma exclude_renderers d3d11_9x
            #pragma only_renderers gles gles3 glcore d3d11

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
            ENDHLSL
        }

        // NOTE: Les pass scene picking et scene selection causent des erreurs

    //     // ----------------------------------------------------------------------------------------------------------------------------------------
    //     //  ScenePickingPass
    //     //  PassName "ScenePickingPass"	
    //     // Pour la sélection des objets dans la scène. Voir: https://light11.hatenadiary.com/entry/2021/11/04/195940	
    //     Pass
    //     {
            
    //         Name "ScenePickingPass"
    //         Tags { "LightMode" = "Picking" }

    //         BlendOp Add
    //         Blend One Zero
    //         ZWrite On
    //         Cull Off

    //         CGPROGRAM
	// 		#include "HLSLSupport.cginc"
	// 		#include "UnityShaderVariables.cginc"
	// 		#include "UnityShaderUtilities.cginc"


    //         #pragma target 3.5

    //         #pragma shader_feature _ALPHATEST_ON
    //         #pragma shader_feature _ALPHAPREMULTIPLY_ON
    //         #pragma multi_compile_instancing

    //         #pragma vertex vertEditorPass
    //         #pragma fragment fragScenePickingPass


    // #pragma shader_feature_local WORLDBEND_DISABLED_ON


    //         #include "../Core/SceneSelection.cginc" 
    //         ENDCG
    //     }	//Pass "ScenePickingPass"	

    //     // ----------------------------------------------------------------------------------------------------------------------------------------
    //     //  SceneSelectionPass
    //     //PassName "SceneSelectionPass"        		
	// 	Pass
    //     {
    //         Name "SceneSelectionPass"
    //         Tags { "LightMode" = "SceneSelectionPass" }

    //         BlendOp Add
    //         Blend One Zero
    //         ZWrite On
    //         Cull Off

    //         CGPROGRAM
	// 		#include "HLSLSupport.cginc"
	// 		#include "UnityShaderVariables.cginc"
	// 		#include "UnityShaderUtilities.cginc"


    //         #pragma target 3.5

    //         #pragma shader_feature _ALPHATEST_ON
    //         #pragma shader_feature _ALPHAPREMULTIPLY_ON
    //         #pragma multi_compile_instancing

    //         #pragma vertex vertEditorPass
    //         #pragma fragment fragSceneHighlightPass


    // #pragma shader_feature_local WORLDBEND_DISABLED_ON


    //         #include "../Core/SceneSelection.cginc" 
    //         ENDCG
    //     }	//Pass "SceneSelectionPass"		


    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.AtoneWorldBend_LitShader" // LitShader"
}
