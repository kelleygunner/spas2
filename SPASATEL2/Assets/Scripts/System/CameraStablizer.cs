using UnityEngine;
using System.Collections;

public class CameraStablizer : MonoBehaviour 
{
	
	Vector3 oldPosition;
	Quaternion oldRotation;
	
	void Start () 
	{
		oldPosition = this.transform.position;
		oldRotation = this.transform.rotation;
	}
	
	
	void LateUpdate () 
	{
		this.transform.position = Vector3.Lerp(oldPosition,this.transform.position,Time.deltaTime*50);
		oldPosition = this.transform.position;
		
		this.transform.rotation = Quaternion.Lerp(oldRotation,this.transform.rotation,Time.deltaTime*50);
		oldRotation = this.transform.rotation;
	}
}
