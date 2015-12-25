using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highlight : MonoBehaviour 
{
    Image img = null;

    Color color = Color.black;
	
	void Update () 
    {
        if (img != null)
        {
            color.a -= Time.deltaTime;
            if (color.a < 0.05f) this.gameObject.SetActive(false);
            img.color = color;
        }
        
	}

    void OnEnable()
    {
        if (img == null)
        {
            img = this.GetComponent<Image>();
            color = img.color;
        }
        color.a = 1;
        img.color = color;
    }
}
