using UnityEngine;
using System.Collections;

public class Migalka : MonoBehaviour
{


    public MeshRenderer rend;
    Color color;

    float componenta = 1;

    public int mat = 0;

	void Start ()
    {
	
	}
	
	
	void Update ()
    {
        componenta = Mathf.PingPong(Time.time*3, 1.0f);
        color = new Color(componenta, componenta, componenta, 1.0f);
        rend.materials[mat].SetColor("_EmissionColor", color);
    }

    void OnEnable()
    {
        color = rend.materials[mat].GetColor("_EmissionColor");
    }
}
