using UnityEngine;
using System.Collections;

public class WaterCollider : MonoBehaviour {

	
	public ParticleSystem splash;
	
//	float timer=0;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider col)
	{
		/*
		if (timer<=0)
		{
			splash.Emit(3);
			timer=1;
		}
		else timer -= Time.deltaTime;*/
	}
}
