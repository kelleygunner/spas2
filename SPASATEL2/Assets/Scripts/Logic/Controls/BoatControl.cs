using UnityEngine;
using System.Collections;

public class BoatControl : MonoBehaviour 
{

	
	int NewSide=0;
	int CarSide=0;
	
	Vector3 MouseStartPos=Vector3.zero;
	
	Vector3 TargetPos = Vector3.zero;
	
	Vector3 TargetRotation = Vector3.zero;
	Vector3 CurrentRot = Vector3.zero;
	
	float side_step=50;
	float target_rotation_value=10;
	
	public Animator boat_anim;
	
	void Start () 
	{
		TargetPos = this.transform.localPosition;
	}
	
	
	void Update () 
	{
		NewSide = DetectSlide();
		
		if (NewSide != 0)
        {
			if (NewSide == 1 && CarSide < 1)
			{
				CarSide++;
				TargetPos += Vector3.right * side_step;
				TargetRotation = Vector3.up * target_rotation_value;
			}
			if (NewSide == -1 && CarSide > -1)
			{
				CarSide--;
				TargetPos -= Vector3.right * side_step;
				TargetRotation = Vector3.up * -target_rotation_value;
				MyMath.NormalizeRotation(ref TargetRotation);
			}
		}
		
		
		if ((this.transform.localPosition - TargetPos).magnitude < side_step*0.5f && TargetRotation != Vector3.zero) 
		{
			TargetRotation = Vector3.Lerp( TargetRotation, Vector3.zero,Time.deltaTime*10);
		}

        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, TargetPos, Time.deltaTime * 5);

        CurrentRot = this.transform.rotation.eulerAngles;
        MyMath.NormalizeRotation(ref CurrentRot);

        this.transform.rotation = Quaternion.Euler(Vector3.Lerp(CurrentRot, TargetRotation, Time.deltaTime * 10));
		
	}
	
	
	
	
	
	
	int DetectSlide()
	{
		if (Input.GetMouseButtonDown(0))MouseStartPos = Input.mousePosition;
		
		
		if (Input.GetMouseButtonUp(0))
		{
			if (Mathf.Abs(Input.mousePosition.x - MouseStartPos.x)>Screen.width*0.05f)
			{
				if (Input.mousePosition.x>MouseStartPos.x)return 1;
				else return -1;
			}
		}
		
		return 0;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("assdddff");
		if (collider.tag == "icerock")
		{
			BoatDrivingScene.Instance.speed /= 2;
			boat_anim.CrossFade("strike"+Random.Range(1,4),0.1f);
		}
	}
	
	
}
