Shader "Custom/FogOfWar" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_FogRadius ("FogRadius", Float) = 1.0
		_FogMaxRadius ("FogMaxRadius", Float) = 0.5
		_Player_Pos ("_Player_Pos", Vector) = (0,0,0,1)
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert
		
		sampler2D _MainTex;
		fixed4 _Color;
		float _FogRadius;
		float _FogMaxRadius;
		float4 _Player_Pos;
		
		struct Input 
		{
			float2 uv_MainTex;
			float2 location;
		};
		
		void vert(inout appdata_full vertexData, out Input outData)
		{
			float4 pos = mul(UNITY_MATRIX_MVP, vertexData.vertex);
			float4 posWorld = mul(_Object2World, vertexData.vertex);
			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xz;
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 baseColor = tex2D (_MainTex, IN.uv_MainTex) * _Color;		
			o.Albedo = baseColor.rgb;
			o.Alpha = baseColor.a;
		}
		float powerForPos(float4 pos, float2 nearVertex)
		{
			return 0;
		}	
		ENDCG
	} 
	FallBack "Transparent/VertexLit"
}
