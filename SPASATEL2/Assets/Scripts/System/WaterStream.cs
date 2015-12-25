using UnityEngine;
using System.Collections;

public class WaterStream : MonoBehaviour {

	public SkinnedMeshRenderer render;
	public Transform cam;
	
	public Transform[] bones;
	
	Vector3 oldRotation;
	Vector3 curRotation;
	
	void Start () 
	{		
		oldRotation = cam.rotation.eulerAngles;
	}
	

	void Update () 
	{
		
		curRotation = cam.rotation.eulerAngles;
		NormalizeRotation(ref curRotation);
		
		oldRotation = new Vector3(
			Mathf.LerpAngle(oldRotation.x,curRotation.x,Time.deltaTime*1.0f),
			Mathf.LerpAngle(oldRotation.y,curRotation.y,Time.deltaTime*1.0f),
			Mathf.LerpAngle(oldRotation.z,curRotation.z,Time.deltaTime*1.0f)
			);
		NormalizeRotation(ref oldRotation);
		
		for (int i=1; i<bones.Length;i++)
		{
			bones[i].localRotation = Quaternion.Euler((curRotation - oldRotation)*-0.5f);
		}
		
		render.material.mainTextureOffset += Vector2.up*Time.deltaTime*-2;
	}
	
	void OnGUI()
	{
		//GUI.Label(new Rect(0,0,800,100),bones[0].localRotation.eulerAngles.ToString());
	}
	
	void NormalizeRotation(ref Vector3 rot)
	{
		if (rot.x>180)rot.x-=360;
		if (rot.x<-180)rot.x+=360;
		if (rot.y>180)rot.y-=360;
		if (rot.y<-180)rot.y+=360;
		if (rot.z>180)rot.z-=360;
		if (rot.z<-180)rot.z+=360;
	}
	
	void OnEnabled()
	{
		/*
		for (int i=0; i<bones.Length;i++)
		{
			bones[i].localRotation = Quaternion.Euler(Vector3.zero);
		}*/
	}
}
