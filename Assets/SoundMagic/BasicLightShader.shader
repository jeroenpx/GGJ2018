Shader "Custom/BasicLightShader" {
	Properties {
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

		struct Input {
			float4 vertexColor : COLOR;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = fixed4(1, 1, 1, 1);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = IN.vertexColor;
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
