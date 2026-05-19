Shader "Redguard/Basic Surface"
{
	Properties
	{
		[MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
		[MainTexture] _BaseMap("Base Map", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

		Pass
		{
			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#pragma multi_compile _ _FORWARD_PLUS
			#pragma multi_compile _ _CLUSTER_LIGHT_LOOP
			#pragma multi_compile _ _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _USE_RT_DEFORMATION

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				half4 color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				half fogFactor : TEXCOORD1;
				half4 color : COLOR;
				float3 normal : NORMAL;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			CBUFFER_START(UnityPerMaterial)
				half4 _BaseColor;
				float4 _BaseMap_ST;
			CBUFFER_END

			float3 SampleLighting(float3 normalWS, Light light)
			{
				float NdotL = dot(normalWS, normalize(light.direction));
				NdotL = (NdotL + 1) * 0.5;
				return saturate(NdotL) * light.color * light.distanceAttenuation * light.shadowAttenuation;
			}

			Varyings vert(Attributes v)
			{
				Varyings o;

				UNITY_SETUP_INSTANCE_ID(v);
   				UNITY_TRANSFER_INSTANCE_ID(v, o);

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal, v.tangent);

				o.fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
				o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
				o.normal = vertexNormalInput.normalWS.xyz;
				o.color = v.color;

				float3 lighting = 0;
				float4 shadowCoords = GetShadowCoord(vertexInput);
				Light mainLight = GetMainLight(shadowCoords);

				lighting = SAMPLE_GI(shadowCoords, v.positionOS, vertexNormalInput.normalWS) * unity_AmbientSky;
				lighting += SampleLighting(vertexNormalInput.normalWS, mainLight);

				int additionalLightsCount = GetAdditionalLightsCount();

				LIGHT_LOOP_BEGIN(additionalLightsCount)

				Light light;
				
				#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
				half4 shadowMask = CalculateShadowMask(inputData);
				light = GetAdditionalLight(lightIndex, vertexInput.positionWS, shadowMask);
				#else
				light = GetAdditionalLight(lightIndex, vertexInput.positionWS);
				#endif
				
				#ifdef _LIGHT_LAYERS
				if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
				#endif
				{
					light.shadowAttenuation = AdditionalLightRealtimeShadow(lightIndex, vertexInput.positionWS, light.direction);

					float NdotL = dot(vertexNormalInput.normalWS, normalize(light.direction));
					NdotL = (NdotL + 1) * 0.5;
					lighting += NdotL * light.color * light.distanceAttenuation * light.shadowAttenuation;
				}
				
				LIGHT_LOOP_END

				o.color.rgb *= lighting;

				return o;
			}

			half4 frag(Varyings i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _BaseColor * i.color;
				color.rgb = MixFog(color.rgb, i.fogFactor);
				return color;
			}
			ENDHLSL
		}
	}
}
