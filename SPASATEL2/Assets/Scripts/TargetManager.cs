using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour 
{

	
	public GameObject[] SCENES;
	
	void Awake () 
	{
		foreach (GameObject go in SCENES)
		{
			go.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))Application.LoadLevel("menu");
	}
}
