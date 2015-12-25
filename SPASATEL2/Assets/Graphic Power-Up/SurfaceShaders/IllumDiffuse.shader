Shader "Graphic Power-Up/Self-Illumination/Diffuse" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_IllumColor ("Illumination Color", Color) = (1,1,1,1)
		_IllumAmount ("Illumination Amount", Range (0, 8)) = 0
		_MaskMap ("Illumination (A)", 2D) = "white" { }
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed4 _IllumColor;
		half _IllumAmount;
		sampler2D _MaskMap;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_MaskMap;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 diffuse = tex2D (_MainTex, IN.uv_MainTex);			
			o.Albedo = diffuse.rgb * _Color.rgb;
			
			half illum = tex2D (_MaskMap, IN.uv_MaskMap).a;	
			o.Emission = _IllumColor.rgb * illum * _IllumAmount;
			
			o.Alpha = _Color.a * illum;
		}
		ENDCG
	} 
	
	FallBack "Self-Illumin/Diffuse"	
}
