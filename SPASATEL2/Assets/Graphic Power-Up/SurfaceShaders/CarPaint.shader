Shader "Graphic Power-Up/Car Paint"
{
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_BackColor("Color (Chameleon Paint)", Color) = (0,0,0,0)
		_Paint("Paint: RED - Gloss, GREEN - Metallic", Color) = (0,1,0,0)
		_Blank("BLUE - Matte, YELLOW - Chameleon", Float) = 0
		_Flakes("Flakes",2D) = "black" {}
		_MetallicFalloff("Metallic Falloff", Float) = 0.9
		_ChameleonFalloff("Chameleon Falloff", Float) = 0.35
		_FresnelPower("Fresnel Power", Float) = 6.0
		_FresnelBias("Fresnel Bias", Float) = 0.075
		_BlurReflIntens("Blur Reflection Intensity", Float) = 0.1
//		_Glossiness ("Glossiness", Range (0.1, 1)) = 0.3
		_CubeHDR ("Reflection Cubemap (RGBM)", Cube) = "" { TexGen CubeReflect }
		_CubeBlurHDR ("Blurred Reflection Cubemap (RGBM)", Cube) = "" { TexGen CubeReflect }
	}
	
	SubShader 
	{
		Tags
		{
			"Queue"="Geometry"
			"IgnoreProjector"="False"
			"RenderType"="Opaque"
		}	
		LOD 600
	
	CGPROGRAM
	#pragma surface surf CarPaintGsKit
	#include "GraphicsKit.cginc"
	#pragma target 3.0
	
	fixed4 _Color;
	fixed4 _BackColor;
	fixed4 _Paint;
	fixed _MetallicFalloff;
	fixed _ChameleonFalloff;
	fixed _FresnelPower;
	fixed _FresnelBias;
	fixed _BlurReflIntens;
//	half _Glossiness;
	samplerCUBE _CubeHDR;
	samplerCUBE _CubeBlurHDR;
	sampler2D _Flakes; 
					 	
	struct Input 
	{
		fixed3 worldRefl;
		fixed3 viewDir;
		fixed3 worldNormal;
		fixed2 uv_Flakes;
		INTERNAL_DATA
	};
				
		void surf (Input IN, inout SurfaceOutputCarPaintGsKit o) 
		{				
			fixed3 FrontColorTex = _Color.rgb;
			fixed3 BackColorTex = _BackColor.rgb;
			
			fixed3 MixTex = _Paint.rgb;
			
			fixed GlossyA = max(0, MixTex.r - MixTex.g);
			fixed MetallicA = max(0, MixTex.g - MixTex.r - MixTex.b);
			fixed MatteA = MixTex.b;
			fixed ChameleonA = MixTex.r*MixTex.g;
			
			o.Normal = half3(0.0f, 0.0f, 1.0f);
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			
			half Fresnel = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				
			half FresnelRefl = pow(Fresnel, _FresnelPower);
			FresnelRefl = min(1, FresnelRefl + _FresnelBias );
			
			fixed FresnelColors = saturate(pow(Fresnel, _MetallicFalloff)*(MetallicA) +
											pow(Fresnel, _ChameleonFalloff)*(ChameleonA));
				
			half3 Cube = DecodeHDR(texCUBE(_CubeHDR, worldRefl));
			half3 BlurCube = DecodeHDR(texCUBE(_CubeBlurHDR, worldRefl));
						
			fixed3 MixColors = lerp(FrontColorTex,  BackColorTex*ChameleonA, FresnelColors);
				
			Flakes = tex2D(_Flakes, IN.uv_Flakes).rgb;		
//			Cube *= FresnelRefl;
//			BlurCube *= _BlurReflIntens;
//			half3 MetallicCube=Cube+BlurCube*(2*(Flakes)+1);
//		
//			
//			 
//			half3 MixRefl = lerp(Cube, MetallicCube, MetallicA+ChameleonA);
//			MixRefl = lerp(MixRefl, BlurCube, MatteA);
			
			o.Albedo = MixColors;
			o.PaintMasks = half3(MetallicA, MatteA, ChameleonA);
			o.Reflect = Cube;
			o.ReflectBlur = BlurCube;
//			o.Metallic = flakes;
			o.ReflectMasks = half2(FresnelRefl, _BlurReflIntens);
//			o.Gloss = _Glossiness;
			o.Specular = 1.0f;
		}
		
	ENDCG
		}
	
	Fallback "Specular"
}
