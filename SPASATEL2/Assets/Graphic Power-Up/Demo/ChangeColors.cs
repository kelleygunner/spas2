using UnityEngine;
using System.Collections;

public class ChangeColors : MonoBehaviour {
	
	public MouseOrbitImproved orbitCam;
	public Material illumMat;
	public Material illumMat2;
	public float min = 1.2f;
	public float max = 2.0f;
	 
	public Color defColor;
	public Color color1;
	public Color color2;
	public Color color3;
	public Color color4;
	public Color color5;
	public Color color6;
	
	public Material carpaint;
	public Material paint1;
	public Material paint2;
	public Material paint3;
	public Color front1;
	public Color back1;
	public Color front2;
	public Color back2;
	public Color front3;
	public Color back3;
	public Color front4;
	public Color back4;
	public Color front5;
	public Color back5;
	public Color front6;
	public Color back6;
	
	Color colorPaint;
	Color backcolorPaint;
	
	float curIllum;
	Color color;
	Color color2nd;
	public bool lerpColors = false;
	
	// Use this for initialization
	void Start () {
		colorPaint = front1;
		backcolorPaint = back1;
		curIllum = 1.8f;
//	 Debug.Log(mat.GetFloat("_IllumAmount"));
	}
	
	// Update is called once per frame
	void Update () {
		illumMat.SetFloat("_IllumAmount", Mathf.Lerp(illumMat.GetFloat("_IllumAmount"), curIllum, 2.0f*Time.deltaTime));	
		illumMat2.SetFloat("_IllumAmount", Mathf.Lerp(illumMat2.GetFloat("_IllumAmount"), curIllum, 1.0f*Time.deltaTime));	
		
		if (lerpColors)	{
			if (illumMat.GetColor("_IllumColor") == color1 || illumMat.GetColor("_IllumColor") == defColor)
			{color = color2; color2nd = color4; }
			else if (illumMat.GetColor("_IllumColor") == color2 )
			{color = color3; color2nd = color5; }
			else if (illumMat.GetColor("_IllumColor") == color3 )
				{color = color4; color2nd = color6; }
			else if (illumMat.GetColor("_IllumColor") == color4 )
					{color = color5; color2nd = color1; }
			else if (illumMat.GetColor("_IllumColor") == color5 )
						{color = color6; color2nd = color2; }
			else if (illumMat.GetColor("_IllumColor") == color6 )
							{color = color1; color2nd = color3; }
		}
		else {
			color = defColor;
			color2nd = defColor; 
		}
		
		illumMat.SetColor("_IllumColor", Color.Lerp(illumMat.GetColor("_IllumColor"), color, 6f*Time.deltaTime));
		illumMat2.SetColor("_IllumColor", Color.Lerp(illumMat2.GetColor("_IllumColor"), color2nd, 6f*Time.deltaTime));
	
		carpaint.SetColor("_Color", Color.Lerp(carpaint.GetColor("_Color"), colorPaint, 8f*Time.deltaTime));
		carpaint.SetColor("_BackColor", Color.Lerp(carpaint.GetColor("_BackColor"), backcolorPaint, 4f*Time.deltaTime));
		
		paint1.SetColor("_Color", Color.Lerp(paint1.GetColor("_Color"), colorPaint*0.3f, 2f*Time.deltaTime));
		paint2.SetColor("_Color", Color.Lerp(paint2.GetColor("_Color"), colorPaint*0.3f, 2f*Time.deltaTime));
		paint3.SetColor("_Color", Color.Lerp(paint3.GetColor("_Color"), colorPaint*0.3f, 2f*Time.deltaTime));
	}
	
	
	void OnGUI() {        
        if (GUI.Button(new Rect(10, Screen.height - 40, 140, 30), "Illumination More/Less"))	{
			if (curIllum == min)
				curIllum = max;
			else
				curIllum = min;
		}
		
		if (GUI.Button(new Rect(160, Screen.height - 40, 140, 30), "Illumination Color"))	{
			if (!lerpColors)
				lerpColors = true;
			else 
				lerpColors = false;
		}
       		   	
 		if (GUI.Button(new Rect(310, Screen.height - 40, 140, 30), "Car Paint Color"))	{
			if (carpaint.GetColor("_Color") == front1){
				colorPaint = front2;
				backcolorPaint = back2;
			}
			else if (carpaint.GetColor("_Color") == front2){
				colorPaint = front3;
				backcolorPaint = back3;
			}
			else if (carpaint.GetColor("_Color") == front3){
				colorPaint = front4;
				backcolorPaint = back4;
			}
			else if (carpaint.GetColor("_Color") == front4){
				colorPaint = front5;
				backcolorPaint = back5;
			}
			else if (carpaint.GetColor("_Color") == front5){
				colorPaint = front6;
				backcolorPaint = back6;
			}
			else if (carpaint.GetColor("_Color") == front6){
				colorPaint = front1;
				backcolorPaint = back1;
			}
		} 
		
		GUI.Label(new Rect(10, 10, 360, 30), "Press Right-click to Rotate Camera(Scroll wheel to Zoom)");
		if (lerpColors)
			GUI.Label(new Rect(10, 40, 360, 30), "Illuminations-colors changing!");
		
		if (Input.GetMouseButton(1))
				orbitCam.enabled = true;
		else 
				orbitCam.enabled = false;
		
    }
}
