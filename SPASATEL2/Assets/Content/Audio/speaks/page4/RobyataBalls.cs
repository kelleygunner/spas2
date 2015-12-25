using UnityEngine;
using System.Collections;

public class RobyataBalls : MonoBehaviour {


    public AudioSource auBall1;
    public AudioSource auBall2;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Ball1()
    {
        auBall1.Play();
    }

    public void Ball2()
    {
        auBall2.Play();
    }
}
