using UnityEngine;
using System.Collections;

public class StarsHackAnimEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hideStars(){

		StarsPanel.hide ();
	}

	public void playSound(int n){
		StarsPanel.playSound (n-1);
	}
}
