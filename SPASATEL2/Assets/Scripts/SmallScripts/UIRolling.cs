using UnityEngine;
using System.Collections;

public class UIRolling : MonoBehaviour {

	
	void Start () {
	
	}
	
	
	void Update () 
	{
		this.transform.Rotate(Vector3.forward*Time.deltaTime*100);
	}
}
