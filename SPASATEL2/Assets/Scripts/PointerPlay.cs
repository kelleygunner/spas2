using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointerPlay : MonoBehaviour 
{

    public Image highlight;


	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void Activate()
    {
        highlight.gameObject.SetActive(true);
    }
}
