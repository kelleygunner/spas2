using UnityEngine;
using System.Collections;

public static class MyMath
{
	
	public static void NormalizeRotation(ref Vector3 rot)
	{
		if (rot.x>180)rot.x-=360;
		if (rot.x<-180)rot.x+=360;
		if (rot.y>180)rot.y-=360;
		if (rot.y<-180)rot.y+=360;
		if (rot.z>180)rot.z-=360;
		if (rot.z<-180)rot.z+=360;
	}
	
}
