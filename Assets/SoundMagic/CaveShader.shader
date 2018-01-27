Shader "Custom/CaveShader" {
	Properties {
		// Basic Shader
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		// Color of the sine
		_Front ("Front", Color) = (1,1,1,1)
		_Back ("Back", Color) = (0,0,0,1)

		// Settings for the sine
		_SourcePoint ("SourcePoint", Vector) = (0, 0, 0, 0)
        _SineDistance ("Sine Distance (units)", Float) = 1
        _Speed ("Speed (units/second)", Float) = 1
        _FadeOutDistance ("Fade Out Distance (units)", Float) = 10

        _WaveEffectTime ("How long does the Burst last? (seconds)", Float) = 2
        _AfterEffectFadeOutTime ("After Burst Energy Fade Out (seconds)", Float) = 10

        // Burst information
        _TimeManual ("Time (set by script)", Float) = 0
        _TimeStart ("Time Start (set by script)", Float) = 0
        _PreviousTimeStart ("Time Start of Previous Effect (set by script)", Float) = 0

        // Local Blackness
        _LocalAlwaysLightRange ("Range which is somewhat visible always (units)", Float) = 5
        _LocalAlwaysLightColor ("Color for the somewhat visible range", Color) = (1,1,1,1)
	}
	SubShader {
        Lighting Off
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

		Blend DstColor Zero
		//ZWrite Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		//#pragma surface surf Lambert
		#pragma surface surf Unlit

         #include "UnityCG.cginc" 
         
         half4 LightingUnlit (SurfaceOutput s, half3 lightDir, half atten) {
           half4 c;
           c.rgb = s.Albedo;
           c.a = s.Alpha;
           return c;
         }

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
            float3 worldPos;
		};

		float4 _SourcePoint;
		fixed4 _Front;
		fixed4 _Back;
		float _SineDistance;
		float _Speed;
		float _FadeOutDistance;
		float _TimeManual;
		float _TimeStart;
		float _PreviousTimeStart;
		float _WaveEffectTime;

		float _AfterEffectFadeOutTime;

		float _LocalAlwaysLightRange;
		fixed4 _LocalAlwaysLightColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
			// 2*PI = 6.28

			// Distance from source
			float curDistance = distance(_SourcePoint.xyz, IN.worldPos);

			float timeMovement = _TimeManual * _Speed / _SineDistance;

			// It looks more black in the distance
			float fadeOutFactor = saturate(1-curDistance/_FadeOutDistance);

			// Sine over time
			half4 c = lerp(_Front, _Back, sin(curDistance/_SineDistance*6.28-timeMovement * 6.28)/2+.5)*fadeOutFactor;

			half4 minBlackness = saturate(1-curDistance/_LocalAlwaysLightRange)*_LocalAlwaysLightColor;

			// Sine start & end
			float timeEnd = _TimeStart + _WaveEffectTime;
			float startPointDist = _Speed*(_TimeManual-_TimeStart);
			float endPointDist = _Speed*(_TimeManual-timeEnd);
			if(curDistance<startPointDist && curDistance>endPointDist) {
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			} else if(curDistance>=startPointDist) {
				float previousTimeEnd = _PreviousTimeStart + _WaveEffectTime;
				float previousEndPointDist = _Speed*(_TimeManual-previousTimeEnd);
				if(curDistance<previousEndPointDist) {
					// Old after effect
					float timeWavePassed = _TimeManual-previousTimeEnd;
					float percentAfterEffect = saturate(1-timeWavePassed/_AfterEffectFadeOutTime);
					o.Albedo = minBlackness.rgb + _Back.rgb * percentAfterEffect;
					o.Alpha = minBlackness.a + _Back.a * percentAfterEffect;
				} else {
					// Too far away
					o.Albedo = minBlackness.rgb;
					o.Alpha = minBlackness.a;
				}
			} else {
				// New after effect
				float timeWavePassed = _TimeManual-timeEnd;
				float percentAfterEffect = saturate(1-timeWavePassed/_AfterEffectFadeOutTime);
				o.Albedo = minBlackness.rgb + _Back.rgb * percentAfterEffect;
				o.Alpha = minBlackness.a + _Back.a * percentAfterEffect;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
