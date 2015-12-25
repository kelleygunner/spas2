inline half4 EncodeHDR(half3 color)	{			
	half4 rgbm;
	color *= 1.0 / 8.0;
	rgbm.a = saturate( max( max( color.r, color.g ), max( color.b, 1e-6 ) ) );
	rgbm.a = ceil( rgbm.a * 255.0 ) / 255.0;
	rgbm.rgb = color / rgbm.a;
	return rgbm;
}

inline half3 DecodeHDR(half4 color)	{			
	return color.rgb*color.a*8.0f;
}

#include "Lighting.cginc"

struct SurfaceOutputGsKit {
	fixed3 Albedo;
	fixed3 Normal;
	half3 Emission;
	half Specular;
	half Gloss;
	half3 Reflect;
	half Fresnel;
	fixed Alpha;
};

half4 _ReflectColor;

inline half4 LightingBlinnPhongGsKit (SurfaceOutputGsKit s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	fixed diff = max (0, dot (s.Normal, lightDir));
	
	float nh = max (0, dot (s.Normal, h));
	float spec = min(pow(nh, s.Gloss*512.0) * 8.0f * min(s.Gloss+0.05f, 1.0f), 1.0f);	
	half3 finalSpec = _LightColor0.rgb * spec * (atten * 6);
		
	#ifdef UNITY_PASS_FORWARDADD
		s.Reflect = 0;
	#endif	
	
	half3 finalReflect = s.Reflect + finalSpec;
	
	finalReflect = (finalReflect + saturate(finalReflect-1)) * s.Fresnel * _ReflectColor.rgb;									
	
	half4 c;	
	c.rgb = (s.Albedo * _LightColor0.rgb * diff) * (atten * 2) * (1 - s.Fresnel * Luminance(finalReflect)) + finalReflect;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingBlinnPhongGsKit_PrePass (SurfaceOutputGsKit s, half4 light)
{
	half spec = min(pow(light.a, 4.0f) * 8.0f * min(s.Gloss+0.05f, 1.0f), 1.0f);
	half3 finalSpec = light.rgb * spec * 3;
		
	half3 finalReflect = s.Reflect + finalSpec;	
		
	finalReflect = (finalReflect + max(0.0f, finalReflect-1)) * s.Fresnel * _ReflectColor.rgb;	
		
	half4 c;
	c.rgb = (s.Albedo * light.rgb) * (1 - s.Fresnel * Luminance(_ReflectColor.rgb)) + finalReflect;
	c.a = s.Alpha;
	return c;
}

inline half4 LightingBlinnPhongGsKit_DirLightmap (SurfaceOutputGsKit s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	
	half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
	
	half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
	half3 h = normalize (lightDir + viewDir);

	float nh = max (0, dot (s.Normal, h));
	
	float spec = min(pow(nh, s.Gloss*512.0) * 8.0f * min(s.Gloss+0.05f, 1.0f), 1.0f);	
	half finalSpec = spec * 3;
		
	#ifdef UNITY_PASS_FORWARDADD
		s.Reflect = 0;
	#endif	
	
	half3 finalReflect = s.Reflect + finalSpec;
	
	finalReflect = (finalReflect + saturate(finalReflect-1)) * s.Fresnel * _ReflectColor.rgb;		
	
	specColor = lm * finalReflect;
	
	return half4(lm, finalSpec);
}

struct SurfaceOutputCarPaintGsKit {
	fixed3 Albedo;
	fixed3 Normal;
	half3 Emission;
	half Specular;
	half Gloss;
	half3 ReflectBlur;
	half3 Reflect;
	half3 PaintMasks;
	half2 ReflectMasks;
	fixed Alpha;
};

half3 Flakes;

inline half4 LightingCarPaintGsKit (SurfaceOutputCarPaintGsKit s, fixed3 lightDir, half3 viewDir, fixed atten)
{
	half3 h = normalize (lightDir + viewDir);
	
	fixed diff = max (0, dot (s.Normal, lightDir));
	
	float nh = max (0, dot (s.Normal, h));
	float spec = min(pow(nh, 1024.0) * 8.0f, 1.0f);	
	float specBlur = min(pow(nh, 51.2f), 1.0f);
	
	#ifdef UNITY_PASS_FORWARDADD
		s.Reflect = 0;
		s.ReflectBlur = 0;
	#endif	
		
	half3 reflBlur = s.ReflectBlur + _LightColor0.rgb * specBlur * (atten * 8);
	reflBlur *= s.ReflectMasks.y;
	
	half3 refl = s.Reflect + _LightColor0.rgb * spec * (atten * 8);
	refl *= s.ReflectMasks.x;
	
	half3 metallicRefl = refl + reflBlur * (2*Flakes+1);	
				 
	half3 mixRefl = lerp(refl, metallicRefl, s.PaintMasks.x+s.PaintMasks.z);
	mixRefl = lerp(mixRefl, reflBlur, s.PaintMasks.y);							
	
	half4 c;	
	c.rgb = (s.Albedo * _LightColor0.rgb * diff) * (atten * 2) + mixRefl;
	c.a = s.Alpha;
	return c;
}
inline fixed4 LightingCarPaintGsKit_PrePass (SurfaceOutputCarPaintGsKit s, half4 light)
{
	half spec = min(pow(light.a, 3.5f) * 8.0f, 1.0f);
	half specBlur = min(pow(light.a, 0.4f), 1.0f);
	
	half3 reflBlur = s.ReflectBlur + light.rgb * specBlur * 4;
	reflBlur *= s.ReflectMasks.y;
	
	half3 refl = s.Reflect + light.rgb * spec * 4;
	refl *= s.ReflectMasks.x;
	
	half3 metallicRefl = refl + reflBlur * (2*Flakes+1);	
				 
	half3 mixRefl = lerp(refl, metallicRefl, s.PaintMasks.x+s.PaintMasks.z);
	mixRefl = lerp(mixRefl, reflBlur, s.PaintMasks.y);		
	
	fixed4 c;
	c.rgb = s.Albedo * light.rgb + mixRefl;
	c.a = s.Alpha;
	return c;
}

inline half4 LightingCarPaintGsKit_DirLightmap (SurfaceOutputCarPaintGsKit s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	
	half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
	
	half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
	half3 h = normalize (lightDir + viewDir);	
	
	float nh = max (0, dot (s.Normal, h));
	float spec = min(pow(nh, 1024.0) * 8.0f, 1.0f);	
	float specBlur = min(pow(nh, 51.2f), 1.0f);
	
	#ifdef UNITY_PASS_FORWARDADD
		s.Reflect = 0;
		s.ReflectBlur = 0;
	#endif	
		
	half3 reflBlur = s.ReflectBlur + specBlur * 4;
	reflBlur *= s.ReflectMasks.y;
	
	half3 refl = s.Reflect + spec * 4;
	refl *= s.ReflectMasks.x;
	
	half3 metallicRefl = refl + reflBlur * (2*Flakes+1);	
				 
	half3 mixRefl = lerp(refl, metallicRefl, s.PaintMasks.x+s.PaintMasks.z);
	mixRefl = lerp(mixRefl, reflBlur, s.PaintMasks.y);			
				
	specColor = lm * mixRefl;
	
	return half4(lm, Luminance(mixRefl));
}
//#endif
