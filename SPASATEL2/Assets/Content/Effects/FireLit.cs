using UnityEngine;
using System.Collections;

public class FireLit : MonoBehaviour
{

    float timer = 0;

    Vector3 startPos;
    Vector3 targPos;

	void Start ()
    {
	    startPos = this.transform.position;   
	}
	
	
	void Update ()
    {
	    timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            targPos = startPos + Vector3.right * Random.Range(-0.5f, 0.5f) + Vector3.up * Random.Range(-0.5f, 0.5f) + Vector3.forward * Random.Range(-0.5f, 0.5f);
            timer = 0;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, targPos, Time.deltaTime*5);
    }
}
