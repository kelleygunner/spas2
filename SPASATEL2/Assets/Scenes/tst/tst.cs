using UnityEngine;
using System.Collections;
using Vuforia;

public class tst : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;

    int status = 0;

    Vector3 campos;

    Vector3[] lastTenPositions = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};

    string log;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (//newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED //||
               /* newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED*/)
        {
            OnTrackingFound();
        }

        if (newStatus == TrackableBehaviour.Status.NOT_FOUND ||
            newStatus == TrackableBehaviour.Status.UNDEFINED ||
            newStatus == TrackableBehaviour.Status.UNKNOWN
            )
        {
            log += " бля, пиздец! ";
            OnTrackingLosta();
        }

        log += newStatus.ToString() + "=>";
    }


    private void OnTrackingFound()
    {
        status = 1;
    }

    private void OnTrackingLosta()
    {
        status = 0;
        lastTenPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
    }

    void Update()
    {
        campos = Camera.main.transform.position;

        if (status == 1)
        {
            FillPoses();
            if (Vuforia.VuforiaManager.Instance.Initialized) Vuforia.VuforiaManager.Instance.Deinit();
            else Vuforia.VuforiaManager.Instance.Init();
        }

        if (lastTenPositions[lastTenPositions.Length - 1] != Vector3.zero && (Camera.main.transform.position - GetAvaregePosition()).magnitude<0.005f)
        {
            status = 2;
            if (Vuforia.VuforiaManager.Instance.Initialized) Vuforia.VuforiaManager.Instance.Deinit();
            else Vuforia.VuforiaManager.Instance.Init();
        }
    }

    void FillPoses()
    {
        for (int i = 9; i > 0; i--)
        {
            lastTenPositions[i] = lastTenPositions[i - 1];
        }
        lastTenPositions[0] = Camera.main.transform.position;          
    }

    Vector3 GetAvaregePosition()
    {
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < lastTenPositions.Length; i++)
        { 
            sum += lastTenPositions[i];
        }

        sum /= lastTenPositions.Length;

        return sum;
    }

    IEnumerator PasteLocation()
    {

        yield return null;
    }


    void OnGUI()
    {
        GUI.Label(new Rect (50,50,800,30),"status: " + status + " " + (Camera.main.transform.position - GetAvaregePosition()).magnitude);

        GUI.Label(new Rect(50, 100, 800, 800), log);
    }
}
