Shader "Custom/Hologram" {
	Properties {
		_InnerColor ("Inner Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0, 0.0)
		_RimPower ("Rim Power", Range(0.0, 16.0)) = 3.0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		
		Cull Back
		Blend One One
		
		CGPROGRAM
			#pragma surface surf Unlit 
			fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten) {
				fixed4 c;
				c.rgb = s.Albedo; 
				c.a = s.Alpha;
				return c;
			}
			
			struct Input {
				float3 viewDir;
			};
			
			float4 _InnerColor;
			float4 _RimColor;
			float _RimPower;
			
			void surf(Input IN, inout SurfaceOutput o) {
				o.Albedo = _InnerColor.rgb;
				o.Alpha = _InnerColor.a;
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			}
		ENDCG
	} 
	Fallback "Diffuse"
}