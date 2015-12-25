using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour 
{


    Camera cam;

    RaycastHit hit;
    Ray ray;

    bool isCatched = false;
    Vector3 originPos = Vector3.zero;

    public Renderer rend;
    Color defCol = Color.white;
    Color selCol = Color.white;

    public Collider table;
    Collider colToTouch;

	void Start () 
    {
        cam = Camera.main;

        colToTouch = this.GetComponent<Collider>();

        defCol = rend.material.color;

        table.enabled = false;
        colToTouch.enabled = true;

	}
	
	
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 5000))
            {
                if (hit.collider.tag == "Drag")
                {
                    isCatched = true;
                    originPos = hit.point;
                    originPos.y = this.transform.position.y;
                    this.transform.position = originPos;
                    rend.material.color = selCol;

                    table.enabled = true;
                    colToTouch.enabled = false;
                    Debug.Log(this.gameObject.name );
                }
            }
        }


        if (isCatched && Input.GetMouseButton(0))
        { 
             ray = cam.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit, 5000))
             {
                 originPos = hit.point;
                 originPos.y = this.transform.position.y;
                 this.transform.position = originPos;
             }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isCatched)
            {
                isCatched = false;
                rend.material.color = defCol;

                table.enabled = false;
                colToTouch.enabled = true;
            }
        }
	}
}
