// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Redguard/Skybox"
{
	Properties
	{
		_HorizonColor2("Horizon Color", Color) = (0.8039216,0.7764706,0.9960784,0)
		[NoScaleOffset]_SkyTexture2("Sky Texture", 2D) = "white" {}
		_SphericalSky2("Spherical Sky", Range( 0 , 1)) = 0
		_CloudSpeed2("Cloud Speed", Range( 0 , 1)) = 0
		[IntRange]_MoveX2("Move X", Range( -1 , 1)) = 1
		[IntRange]_MoveZ2("Move Z", Range( -1 , 1)) = 1

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _SkyTexture2;
			uniform float _CloudSpeed2;
			uniform float _MoveX2;
			uniform float _MoveZ2;
			uniform float _SphericalSky2;
			uniform float4 _HorizonColor2;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float4 appendResult65 = (float4(( _MoveX2 * -1.0 ) , ( _MoveZ2 * -1.0 ) , 0.0 , 0.0));
				float3 normalizeResult51 = normalize( WorldPosition );
				float3 break53 = normalizeResult51;
				float2 appendResult60 = (float2(break53.x , break53.z));
				float2 panner70 = ( ( _Time.y * _CloudSpeed2 ) * appendResult65.xy + ( appendResult60 / (( _SphericalSky2 / 5.0 ) + (abs( break53.y ) - 0.0) * (1.0 - ( _SphericalSky2 / 5.0 )) / (1.0 - 0.0)) ));
				float3 gammaToLinear75 = GammaToLinearSpace( tex2D( _SkyTexture2, panner70 ).rgb );
				float4 blendOpSrc77 = float4( gammaToLinear75 , 0.0 );
				float4 blendOpDest77 = _HorizonColor2;
				float3 normalizeResult68 = normalize( WorldPosition );
				float clampResult76 = clamp( ( normalizeResult68.y * 4.0 ) , 0.0 , 1.0 );
				float4 lerpBlendMode77 = lerp(blendOpDest77,(( blendOpDest77 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest77 ) * ( 1.0 - blendOpSrc77 ) ) : ( 2.0 * blendOpDest77 * blendOpSrc77 ) ),clampResult76);
				
				
				finalColor = lerpBlendMode77;
				return finalColor;
			}
			ENDCG
		}
	}
	//CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18712
343;73;1169;662;763.0941;409.868;1.23667;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;50;-2066.459,-267.589;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;51;-1857.875,-261.662;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1624.741,30.09473;Inherit;False;Property;_SphericalSky2;Spherical Sky;2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;53;-1656.392,-266.4036;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;54;-1258.055,-417.7567;Inherit;False;Property;_MoveX2;Move X;4;1;[IntRange];Create;True;0;0;0;False;0;False;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1259.055,-329.7567;Inherit;False;Property;_MoveZ2;Move Z;5;1;[IntRange];Create;True;0;0;0;False;0;False;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;56;-1251.944,-69.20589;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;57;-1445.649,-242.6642;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;63;-1163.312,39.59958;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-855.0551,-418.7567;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-925.0551,-325.7567;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;62;-867.6569,-583.0293;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;59;-1108.282,-145.8729;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-989.5416,-496.3541;Inherit;False;Property;_CloudSpeed2;Cloud Speed;3;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;-1426.834,-343.5811;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;65;-671.055,-368.7567;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-654.2043,-485.1587;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;67;-849.405,-208.2481;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;68;-978.55,45.96472;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;69;-801.2912,40.51514;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.PannerNode;70;-483.739,-397.8837;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-812.722,171.63;Inherit;False;Constant;_Float2;Float 0;2;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-610.7547,46.45171;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;73;-354.0979,-221.8285;Inherit;True;Property;_SkyTexture2;Sky Texture;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;74;-260.6312,75.31849;Inherit;False;Property;_HorizonColor2;Horizon Color;0;0;Create;True;0;0;0;False;0;False;0.8039216,0.7764706,0.9960784,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;76;-467.4343,46.41385;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;75;-11.94703,-372.1734;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendOpsNode;77;3.660652,-187.7014;Inherit;True;Overlay;False;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;288,-192;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom Skybox 1;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;51;0;50;0
WireConnection;53;0;51;0
WireConnection;56;0;52;0
WireConnection;57;0;53;1
WireConnection;64;0;54;0
WireConnection;61;0;55;0
WireConnection;59;0;57;0
WireConnection;59;3;56;0
WireConnection;60;0;53;0
WireConnection;60;1;53;2
WireConnection;65;0;64;0
WireConnection;65;1;61;0
WireConnection;66;0;62;0
WireConnection;66;1;58;0
WireConnection;67;0;60;0
WireConnection;67;1;59;0
WireConnection;68;0;63;0
WireConnection;69;0;68;0
WireConnection;70;0;67;0
WireConnection;70;2;65;0
WireConnection;70;1;66;0
WireConnection;72;0;69;1
WireConnection;72;1;71;0
WireConnection;73;1;70;0
WireConnection;76;0;72;0
WireConnection;75;0;73;0
WireConnection;77;0;75;0
WireConnection;77;1;74;0
WireConnection;77;2;76;0
WireConnection;0;0;77;0
ASEEND*/
//CHKSM=A1924B715EF2AE1BAEC7139010968BF90A7B624C