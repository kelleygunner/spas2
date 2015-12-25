using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public class Touchable3D : MonoBehaviour 
{
	
	public delegate void TouchDetector(string name);	
	public static event TouchDetector detector;
	
	RaycastHit hit;
	Ray ray;
	
	public float MaxDistance=5000;
	
	Camera cam;
	
	void Start () 
	{
		cam = GameObject.Find("MainCamera").GetComponent<Camera>();
	}
	
	
	void Update () 
	{
		
		if (Input.GetMouseButtonDown(0))
		{
			ray=cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray,out hit,MaxDistance))
			{
				//Debug.Log(hit.collider.name.ToString());
				detector(hit.collider.name);
			}
		}
		
	}
}
