using UnityEngine;
using System.Collections;

public class BoatDrivingScene : MonoBehaviour 
{
	#region singleton
	private static BoatDrivingScene _instance;
	public static BoatDrivingScene Instance
	{
		get {return _instance;}
	}
	#endregion
	
	#region Params
	//[HideInInspector]
	public float speed=0;
	
	float max_speed=0.1f;
	float common_acceleration = 0.02f;
	float boat_quick_acceleration_speed=0.08f;
	float acceleration=0.02f;
	#endregion
	
	public MeshRenderer waterRenderer;
	
	public Transform[] IceRocks;
    public Transform[] Berega;
	
	void Start () 
	{
		_instance = this;
	}
	
	
	void Update () 
	{
		if(speed<boat_quick_acceleration_speed)speed+=Time.deltaTime*acceleration;
		if(speed<max_speed)speed+=Time.deltaTime*common_acceleration;
        Debug.Log(speed.ToString());
		//waterRenderer.materials[0].SetVector("_waveSpeed",new Vector4(0,speed*1000,0,speed*500));
		
		IceMovement();
	}
	
	void IceMovement()
	{
		for (int i=0; i<IceRocks.Length;i++)
		{
			IceRocks[i].position += Vector3.forward * -speed *50 * Time.deltaTime;
            

			if (IceRocks[i].localPosition.z<-200)
			{
				IceRocks[i].localPosition = new Vector3(50*Random.Range(-1,2)+Random.Range(-2.0f,2.0f),IceRocks[i].localPosition.y,800-(200+IceRocks[i].localPosition.z));
			}
		}

        for (int i = 0; i < Berega.Length; i++)
        {
            Berega[i].position += Vector3.forward * -speed * 50 * Time.deltaTime;
            if (Berega[i].localPosition.z < -500)
            {
                Berega[i].localPosition = new Vector3(Berega[i].localPosition.x, Berega[i].localPosition.y, 1590 - (500 + Berega[i].localPosition.z));
            }
        }
	}
}
