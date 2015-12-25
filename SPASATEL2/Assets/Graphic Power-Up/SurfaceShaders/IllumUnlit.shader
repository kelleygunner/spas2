Shader "Graphic Power-Up/Self-Illumination/Unlit" {
	Properties 
	{
		_IllumColor ("Illumination Color", Color) = (1,1,1,1)
		_IllumAmount ("Illumination Amount", Range (0, 8)) = 0
		_MaskMap ("Illumination (A)", 2D) = "white" { }		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 250
	
		Pass 
		{ 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			struct v2f {
				fixed4 pos	: SV_POSITION;
				fixed2	uv	: TEXCOORD0;
			};

			fixed4 _MaskMap_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MaskMap);	
					
				return o; 
			}
			
			fixed4 _IllumColor;
			sampler2D _MaskMap;
			half _IllumAmount;

			half4 frag (v2f i) : COLOR
			{
				fixed illum = tex2D(_MaskMap, i.uv).a;	
				return illum * _IllumColor * _IllumAmount;
			}
			ENDCG  
		} 
	}	
	
	FallBack off
	
}

