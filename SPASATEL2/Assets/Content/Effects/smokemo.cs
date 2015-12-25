using UnityEngine;
using System.Collections;

public class smokemo : MonoBehaviour
{

    int u = 0;
    int v = 0;

    float timer = 0;

    public float frame_time = 0.05f;
    public MeshRenderer myRend;

	void Start ()
    {
	
	}
	
	
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= frame_time)
        {
            timer = 0;
            u++;
            if (u > 8)
            {
                u = 0;
                v++;
                if (v > 4) v = 0;
            }

            myRend.material.mainTextureOffset = new Vector2(0.11111111f*(float)u,0.2f* (float)(4-v));
        }
	}

    
}
