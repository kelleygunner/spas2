Shader "Graphic Power-Up/ Hightlights/Self-Illumination/Advanced" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Reflect (A)", 2D) = "white" {}
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_Glossiness ("Glossiness", Range (0.2, 3)) = 3
		_FresnelPower ("Fresnel Power", Float) = 2
		_FresnelBias ("Fresnel Bias", Range(0.015, 1)) = 0.1
		_BumpMap ("Normalmap", 2D) = "bump" { }
		_BumpAmount ("Bump Amount", Range (0, 1)) = 0.3	
		_IllumColor ("Illumination Color", Color) = (1,1,1,1)
		_IllumAmount ("Illumination Amount", Range (0, 8)) = 0
		_MaskMap("Reflect (R) Glossiness (G) Illumination (A)", 2D) = "white" {}
		_CubeHDR ("Reflection Cubemap (RGBM)", Cube) = "" { TexGen CubeReflect }
		_CubeBlurHDR ("Blurred Reflection Cubemap (RGBM)", Cube) = "" { TexGen CubeReflect }
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhongGsKit alpha
		#include "GraphicsKit.cginc"
		#pragma target 3.0

		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed _Glossiness;
		half _FresnelPower;
		half _FresnelBias;
		sampler2D _BumpMap;
		half _BumpAmount;	
		fixed4 _IllumColor;
		half _IllumAmount;
		sampler2D _MaskMap;	

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};
      
		void surf (Input IN, inout SurfaceOutputGsKit o) {
			fixed3 masks = tex2D(_MaskMap, IN.uv_MainTex).rga;
			
			fixed4 diffuse = tex2D (_MainTex, IN.uv_MainTex);			
			o.Albedo = diffuse.rgb * _Color.rgb;
			
			half3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));		
			o.Normal = lerp(half3(0.0f, 0.0f, 1.0f), normal, _BumpAmount);
			
			_Glossiness = _Glossiness * _Glossiness * 0.111111 * masks.g;
		
			o.Reflect = 0.0f;
			
			half fresnel = 1.0f - saturate(dot (normalize(IN.viewDir), o.Normal));
			fresnel = pow(fresnel, _FresnelPower);
  			fresnel = min(fresnel + _FresnelBias, 1.0f);
			o.Fresnel = fresnel * masks.r;
			
			o.Gloss = _Glossiness;
			o.Specular = _Glossiness;
			
			o.Emission = _IllumColor.rgb * masks.b * _IllumAmount;

			o.Alpha = _Color.a * diffuse.a + Luminance(o.Fresnel*_ReflectColor.rgb);
		}
		ENDCG
	} 
	Fallback "Self-Illumin/Bumped Specular"
}