﻿Shader "Custom/lavaShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		 _LavaTexture("Texture de lave",2D) = "white"{}
		_Scroll("Scroll", Range(0,100)) = 10
	    _RockTexture("Texture de Roche",2D) = "white"{}
		_DisplacementFactor("Displacement factor", Range(0,1)) = 10
		_DispTexture("Texture de déplacement",2D) = "white"{}
		 }
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:disp

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _LavaTexture;
		sampler2D _RockTexture;
		sampler2D _DispTexture;
		half _DisplacementFactor;
		half _Scroll;
		struct Input {
			float2 uv_LavaTexture;
			float2 uv_RockTexture;
			float2 uv_DispTexture;
		};
		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent: TANGENT;
			float3 normal: NORMAL;
			float2 texcoord: TEXCOORD0;

		};
		void disp(inout appdata v)
		{
			fixed2 scrolledUV = v.texcoord.xy;
			fixed scrollValue = _Time * _Scroll;
			scrolledUV += fixed2(scrollValue, scrollValue);
			float splat = tex2Dlod(_DispTexture, float4(scrolledUV, 0, 0)).r;
			v.vertex.xyz -= v.normal * splat;
			v.vertex.xyz += v.normal * _DisplacementFactor;
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		
		

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed2 scrolledUV = IN.uv_LavaTexture;
			fixed scrollValue = _Time * _Scroll;
			scrolledUV += fixed2(scrollValue, scrollValue);
			scrollValue = _Time * _Scroll / 3;
			fixed4 c = tex2D(_LavaTexture, scrolledUV) ;
			fixed4 c2 = tex2D(_RockTexture, scrolledUV)*2.5;
			o.Albedo = c.rgb-c2.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
