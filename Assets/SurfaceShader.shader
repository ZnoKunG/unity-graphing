Shader "Graph/Point Surface" {

	Properties {
			_Smoothness ("Smoothness", Float) = 0.5
	}

	SubShader {
		CGPROGRAM
		#pragma surface ConfigureSurface Standard fullforwardshadows
		#pragma target 3.0

		struct Input {
			float3 worldPos;
		};

		float _Smoothness;

		void ConfigureSurface(Input input, inout SurfaceOutputStandard surface) {
			surface.Smoothness = _Smoothness;
			surface.Albedo.rg = saturate(input.worldPos.xy * 1/2 + 1/2);
		}
		ENDCG
	}

	Fallback "Diffuse"

	}