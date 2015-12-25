using UnityEngine;
using System.Collections;

public class topPlane : MonoBehaviour {

	//SkinnedMeshRenderer render;
	void Start () {
		this.GetComponent<SkinnedMeshRenderer>().material.renderQueue = 5002;		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
}
