using UnityEngine;
using System.Collections;

public class MosaicControl : MonoBehaviour 
{

    Camera cam;
    RaycastHit hit;
    Ray ray;


    bool isCatched = false;

    Transform currentElement;

    float element_y_pos = 0;

    Vector3 targetPosition = Vector3.zero;

	void Start () 
    {
        cam = Camera.main;
	}
	

	void Update () 
    {
        if (!isCatched && Input.GetMouseButtonDown(0))
        { 
            ray=cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 5000))
            {
                if (hit.collider.name.Contains("Element"))
                {
                    currentElement = hit.collider.gameObject.transform;
                    Catch();
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) Lost();

        if (isCatched) Move();
	}

    private void Catch()
    {
        isCatched = true;
        element_y_pos = currentElement.position.y;
        currentElement.gameObject.GetComponent<Collider>().enabled = false;
        targetPosition = currentElement.gameObject.GetComponent<MosaicElement>().TargetPosition;
    }

    private void Lost()
    {
        isCatched = false;
        currentElement.gameObject.GetComponent<Collider>().enabled = true;
    }

    private void Move()
    {
        if (Input.GetMouseButton(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 5000))
            {
                if (hit.collider.name.Contains("Platform"))
                {
                    currentElement.position = new Vector3(hit.point.x, element_y_pos, hit.point.z);
                }
            }

            if (Mathf.Abs((currentElement.localPosition - targetPosition).magnitude) < 0.1f)
            {
                currentElement.localPosition = targetPosition;
                Lost();
            }
        }
        else
        {
            Lost();
        }
    }


}
