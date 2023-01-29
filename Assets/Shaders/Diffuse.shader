Shader "Redguard/Diffuse"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1,1,1,0)
		_Texture("Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
        #pragma surface surf Lambert fullforwardshadows
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		
		
		void surf (Input i, inout SurfaceOutput o)
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			o.Albedo = ( _MainColor * tex2D( _Texture, uv_Texture ) ).rgb;
			o.Alpha = 1;
        }

		ENDCG
	}
	Fallback "Diffuse"
}