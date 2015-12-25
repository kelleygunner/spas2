using UnityEngine;
using System.Collections;

public class RotateWheel : MonoBehaviour {

	public RectTransform loadingWhille;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		loadingWhille.Rotate (0f, 0f, 1.5f, Space.Self);
	}
}
