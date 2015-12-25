using UnityEngine;
using System.Collections;

public class FireBatleSerg : MonoBehaviour 
{
	/*
	public static Color COLOR1 = Color.white;
	public static Color COLOR2 = Color.white;
	public static Color COLOR3 = Color.white;
	public static Color COLOR4 = Color.white;
	public static Color COLOR5 = Color.white;
	public static Color COLOR6 = Color.white;
	public static Color COLOR7 = Color.white;*/
	
	public SkinnedMeshRenderer seroja;
	public SkinnedMeshRenderer shlen;
	
	void Start () 
	{

        seroja.materials[0].color = MarkerHandler.colors_recognized[0];
        seroja.materials[1].color = MarkerHandler.colors_recognized[1];
        seroja.materials[2].color = MarkerHandler.colors_recognized[2];
        seroja.materials[3].color = MarkerHandler.colors_recognized[3];
        seroja.materials[5].color = MarkerHandler.colors_recognized[4];
        seroja.materials[6].color = MarkerHandler.colors_recognized[5];
        seroja.materials[7].color = MarkerHandler.colors_recognized[6];

        shlen.materials[0].color = MarkerHandler.colors_recognized[12];

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
