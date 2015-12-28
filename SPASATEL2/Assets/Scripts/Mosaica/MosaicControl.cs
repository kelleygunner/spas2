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

    public Transform zahvat;
    float zahvat_y_pos = 0;
    Vector3 zahvatTarget = Vector3.zero;

    public Animator zahvatAnimator;

	void Start () 
    {
        cam = Camera.main;
        zahvat_y_pos = zahvatTarget.y = zahvat.position.y;
        
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

        if (isCatched && Input.GetMouseButtonUp(0)) Lost();

        if (isCatched)
        {
            Move();
        }
        else
        {
            zahvat.position = Vector3.Lerp( zahvat.position, zahvatTarget, Time.deltaTime * 10);
        }
	}

    private void Catch()
    {
        isCatched = true;
        element_y_pos = currentElement.position.y;
        currentElement.gameObject.GetComponent<Collider>().enabled = false;
        targetPosition = currentElement.gameObject.GetComponent<MosaicElement>().TargetPosition;
        zahvatAnimator.CrossFade("zahvat_action", 0.1f);
    }

    private void Lost()
    {
        isCatched = false;
        currentElement.gameObject.GetComponent<Collider>().enabled = true;
        zahvatTarget = new Vector3(0, 2, 0);
        zahvatAnimator.CrossFade("zahvat_animation",0.1f);
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

            if (Mathf.Abs((currentElement.localPosition - targetPosition).magnitude) < 0.2f)
            {
                currentElement.localPosition = targetPosition;
                Lost();
                return;
            }
        }
        else
        {
            Lost();
        }
        zahvatTarget.y = Mathf.Lerp(zahvatTarget.y,0,Time.deltaTime*100);
        zahvatTarget.x = currentElement.position.x;
        zahvatTarget.z = currentElement.position.z;
        zahvat.position = zahvatTarget;
    }


}
