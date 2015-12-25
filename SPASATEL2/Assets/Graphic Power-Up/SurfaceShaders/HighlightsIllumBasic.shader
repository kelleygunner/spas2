Shader "Graphic Power-Up/ Hightlights/Self-Illumination/Basic" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_Glossiness ("Glossiness", Range (0.2, 3)) = 3
		_FresnelPower ("Fresnel Power", Float) = 2
		_FresnelBias ("Fresnel Bias", Range(0.015, 1)) = 0.1
		_IllumColor ("Illumination Color", Color) = (1,1,1,1)
		_IllumAmount ("Illumination Amount", Range (0, 8)) = 0
		_MaskMap ("Illumination (A)", 2D) = "white" { }
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhongGsKit
		#include "GraphicsKit.cginc"
		#pragma target 3.0

		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed _Glossiness;
		half _FresnelPower;
		half _FresnelBias;
		fixed4 _IllumColor;
		half _IllumAmount;
		sampler2D _MaskMap;		

		struct Input {
			float2 uv_MainTex;
			float2 uv_MaskMap; 
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};
      
		void surf (Input IN, inout SurfaceOutputGsKit o) {
			fixed4 diffuse = tex2D (_MainTex, IN.uv_MainTex);			
			o.Albedo = diffuse.rgb * _Color.rgb;
		
			o.Normal = half3(0.0f, 0.0f, 1.0f);
			
			_Glossiness = _Glossiness * _Glossiness * 0.111111;
			o.Gloss = _Glossiness;
			
			o.Reflect = 0.0f;
			
			half fresnel = 1.0f - saturate(dot (normalize(IN.viewDir), o.Normal));
			fresnel = pow(fresnel, _FresnelPower);
  			fresnel = min(fresnel + _FresnelBias, 1.0f);
			o.Fresnel = fresnel;
			
			o.Gloss = _Glossiness;
			o.Specular = _Glossiness;
				
			half illum = tex2D (_MaskMap, IN.uv_MainTex).a;	
			o.Emission = _IllumColor.rgb * illum * _IllumAmount;	
			
			o.Alpha = _Color.a * diffuse.a * illum + Luminance(o.Fresnel*_ReflectColor.rgb);
		}
		ENDCG
	} 
	Fallback "Self-Illumin/Specular"
}
