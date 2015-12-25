using UnityEngine;
using System.Collections;

public class BattleSfx : MonoBehaviour 
{
	
	private static BattleSfx _instance;
	
	public bool needShipenie=false;
	
	public static BattleSfx Instance
	{
		get {return _instance;}
	}
	
	public AudioSource auTresk;
	public AudioSource auShipenie;
	
	void Start () 
	{
		_instance = this;
	}
	
	
	void Update () 
	{
		if (!auShipenie.isPlaying && needShipenie)
		{
			auShipenie.Play();
			needShipenie=false;
		}
	}
	
	public void SetTreskVolume(float vol)
	{
		auTresk.volume = vol;
	}
}
