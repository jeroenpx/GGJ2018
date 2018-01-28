﻿Shader "Custom/BasicLightShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _EmitMult("Emit Multiplier", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "RenderType"="CaveMaterial" }
		LOD 200
		Cull Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float4 vertexColor : COLOR;
        	float2 uv_MainTex;
        	float2 uv_BumpMap;
		};

		fixed4 _EmitMult;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = fixed4(1, 1, 1, 1);
			fixed4 albedo = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = IN.vertexColor*_EmitMult*albedo.a;
        	o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		UsePass "custom/Unity - Particles - Alpha Blended"
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "custom/Cg two-sided per-vertex lighting"
	}
	FallBack "Diffuse"
}
