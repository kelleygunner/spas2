using UnityEngine;
using System.Collections;

public class PointToCamera : MonoBehaviour {

	void Update () 
    {
        this.transform.LookAt(GameManager.Instance.ARCamera.transform);
	}
}
