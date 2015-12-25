using UnityEngine;
using System.Collections;

public class ColorRecognizer
{
	/*
	private static Color[] MainColors = new Color[]
	{
		
		new Color(0.0f,0.0f,0.0f),
		new Color(1.0f,1.0f,1.0f),
		new Color(1.0f,0.5f,0.0f),
		new Color(1.0f,0.0f,0.5f),
		new Color(0.5f,1.0f,0.0f),
		new Color(0.0f,1.0f,0.5f),
		new Color(0.5f,0.0f,1.0f),
		new Color(0.0f,0.5f,1.0f),
		new Color(0.3f,0.5f,0.7f),
		new Color(0.3f,0.7f,0.5f),
		new Color(0.5f,0.3f,0.7f),
		new Color(0.5f,0.7f,0.3f),
		new Color(0.7f,0.3f,0.5f),
		new Color(0.7f,0.5f,0.3f)
	};
	
	private static Vector3[] color_components;
	
	
	public static Color GetColor(Color color)
	{
		color_components = new Vector3[MainColors.Length];
		for (int i=0; i<color_components.Length;i++)
		{
			color_components[i] = setColorToVector(MainColors[i]);
		}
		
		Vector3 new_color = setColorToVector(color);
		
		float[] sum = new float[MainColors.Length];
		for (int i=0; i<sum.Length;i++)
		{
			sum[i] = (color_components[i]-new_color).magnitude;	
		}
		
		float val=10;
		int index=0;
		for (int i=0; i<sum.Length;i++)
		{
			if (sum[i]<val)
			{
				val = sum[i];
				index=i;
			}
		}
		
		return MainColors[index];
		
	}
	*/
	/*
	static Color[] MainColors = new Color[]
		{
			new Color(0.7f,0.08f,0.12f),
			new Color(0.98f,0.17f,0.21f),
			new Color(0.99f,0.65f,0.23f),
			new Color(0.99f,0.89f,0.31f),
			new Color(0.61f,0.87f,0.26f),
			new Color(0.13f,0.68f,0.24f),
			new Color(0.0f,0.65f,0.96f),
			new Color(0.0f,0.4f,0.8f),
			new Color(0.7f,0.13f,0.67f),
			new Color(0.5f,0.27f,0.72f),
			new Color(0.0f,0.0f,0.0f),
			new Color(1.0f,1.0f,1.0f)
		};
	
	public static Color GetColor(Color color)
	{
		
		Vector3[] components = new Vector3[MainColors.Length];
		for (int i=0; i<components.Length;i++)
		{
			components[i] = new Vector3(MainColors[i].r,MainColors[i].g,MainColors[i].b);
			
			if (components[i].x > components[i].y && components[i].x>components[i].z)components[i].x*=2;
			if (components[i].y > components[i].x && components[i].y>components[i].z)components[i].y*=2;
			if (components[i].z > components[i].x && components[i].z>components[i].y)components[i].z*=2;
		}
		
			
		Vector3 cur_col = new Vector3(color.r,color.g,color.b);
		if (cur_col.x > cur_col.y && cur_col.x>cur_col.z)cur_col.x*=2;
		if (cur_col.y > cur_col.x && cur_col.y>cur_col.z)cur_col.y*=2;
		if (cur_col.z > cur_col.x && cur_col.z>cur_col.y)cur_col.z*=2;
		
		
		float[] val = new float[MainColors.Length];
		for (int i=0; i<MainColors.Length; i++)
		{
			val[i] = (components[i]-cur_col).magnitude;
		}
		
		int index=0;
		float min=3;
		
		for (int i=0; i<MainColors.Length; i++)
		{
			if (val[i]<min)
			{
				index=i;
				min=val[i];
			}
		}
		
		return MainColors[index];
	}
	*/
	
	public static Color GetColor(Color color)
	{
		float grayscale = (0.212f*color.r + 0.715f*color.g + 0.072f*color.b) * 3 * 255;
		
		if (grayscale<150) return Color.black;
		if (grayscale>600) return Color.white;
		
		float rComp = color.r/(color.r+color.g+color.b);
		float gComp = color.g/(color.r+color.g+color.b);
		float bComp = color.b/(color.r+color.g+color.b);
		
		Color col = new Color(1.0f*rComp,1.0f*gComp,1.0f*bComp);
		
		if (col.r>col.g&&col.r>col.b)col.r *= 1.5f;
		if (col.g>col.r&&col.g>col.b)col.g *= 1.5f;
		if (col.b>col.r&&col.b>col.g)col.b *= 1.5f;
		
		return col;
	}
	
	private static Vector3 setColorToVector(Color color)
	{
		
		if (color.r>color.g)
		{
			if (color.r>color.b)
			{
				if (color.g > color.b) return new Vector3(color.r*2.0f,color.g*1.5f,color.b*1.0f);
				else return new Vector3(color.r*2.0f,color.g*1.0f,color.b*1.5f);
			}
			else return new Vector3(color.r*1.5f,color.g*1.0f,color.b*2.0f);
		}
		else
		{
			if (color.g>color.b)
			{
				if (color.r > color.b) return new Vector3(color.r*1.5f,color.g*2.0f,color.b*1.0f);
				else return new Vector3(color.r*1.0f,color.g*2.0f,color.b*1.5f);
			}
			else
			{
				return new Vector3(color.r*1.0f,color.g*1.5f,color.b*2.0f);
			}
			
		}
		
	}
	
	
	public static Color GetCommonCollor(Color[] args)
	{
		Vector3[] colors = new Vector3[args.Length];
		for (int i=0; i<colors.Length; i++)
		{
			colors[i] = new Vector3( args[i].r,args[i].g,args[i].b);
		}
		
		Vector3 color = Vector3.zero;
		for (int i=0; i<colors.Length; i++)
		{
			color += colors[i];
		}
		
		color /= (float)colors.Length;
		
		Color new_color = new Color(color.x,color.y,color.z);
		
		return new_color;
		
	}
	
	
	public static Color GetBlurColor(Texture2D texture, Vector3[] positions, int height)
	{
		
		Color[] colors = new Color[positions.Length*9];
		
		for (int i=0; i<positions.Length;i++)
		{
			colors[i*9+0] = texture.GetPixel((int)positions[i].x-2,height - ((int)positions[i].y-2));
			colors[i*9+1] = texture.GetPixel((int)positions[i].x-0,height - ((int)positions[i].y-2));
			colors[i*9+2] = texture.GetPixel((int)positions[i].x+2,height - ((int)positions[i].y-2));
			
			colors[i*9+3] = texture.GetPixel((int)positions[i].x-2,height - ((int)positions[i].y-0));
			colors[i*9+4] = texture.GetPixel((int)positions[i].x-0,height - ((int)positions[i].y-0));
			colors[i*9+5] = texture.GetPixel((int)positions[i].x+2,height - ((int)positions[i].y-0));
			
			colors[i*9+6] = texture.GetPixel((int)positions[i].x-2,height - ((int)positions[i].y+2));
			colors[i*9+7] = texture.GetPixel((int)positions[i].x-0,height - ((int)positions[i].y+2));
			colors[i*9+8] = texture.GetPixel((int)positions[i].x+2,height - ((int)positions[i].y+2));
		}
		
		
		
		Vector3 common_value=Vector3.zero;
		
		for (int i=0; i<colors.Length; i++)
		{
			common_value.x += colors[i].r;
			common_value.y += colors[i].g;
			common_value.z += colors[i].b;
		}
		
		//Color col = Color.white;
		
		
		
		common_value /= colors.Length;

        /*
        common_value.x *= 1.2f;
        common_value.x *= 1.3f;*/
		
		Color col = new Color(common_value.x,common_value.y,common_value.z);
		
		/*
		if (col.r+col.g+col.b>1.75f)return Color.white;
		if (col.r+col.g+col.b<0.5f)return Color.black;*/
		
		HSBColor colhsb = HSBColor.FromColor(col);


        colhsb.s = 1;

		/*
		if(colhsb.s>=0.7f)colhsb.s=1.0f;
		if(colhsb.s>=0.5f&&colhsb.s<0.7f)colhsb.s=0.95f;
		if(colhsb.s>=0.3f&&colhsb.s<0.5f)colhsb.s=0.85f;
		if(colhsb.s>=0.2f&&colhsb.s<0.3f)colhsb.s=0.7f;
		if(colhsb.s>=0.1f&&colhsb.s<0.2f)colhsb.s=0.55f;
		if(colhsb.s<0.1f)colhsb.s=0.1f;		
		
		if(colhsb.b>=0.7f)colhsb.b=0.7f;
		if(colhsb.b>=0.5f&&colhsb.b<0.7f)colhsb.b=0.7f;
		if(colhsb.b>=0.3f&&colhsb.b<0.5f)colhsb.b=0.6f;
		if(colhsb.b>=0.2f&&colhsb.b<0.3f)colhsb.b=0.5f;
		if(colhsb.b>=0.1f&&colhsb.b<0.2f)colhsb.b=0.4f;
		if(colhsb.b<0.1f)colhsb.b=0.2f;
		
		col = HSBColor.ToColor(colhsb);

        if ( col.r > 0.6f && col.r > col.g *2 && col.r > col.b*2)
        {
            col.r = 1.0f;
            col.g *= 0.75f;
        }
        */

		return col;
		
	}
	
}
