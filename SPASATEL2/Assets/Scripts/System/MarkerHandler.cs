using UnityEngine;
using System.Collections;
using Vuforia;

public class MarkerHandler : MonoBehaviour, ITrackableEventHandler
{

    //public GameObject Location;

    public string locationName;

    private TrackableBehaviour mTrackableBehaviour;

    public GameObject loadingImage;

    public Transform[] color_probes;
    Vector3[] ProbePosition;

    public static Color[] colors_recognized;

    private int lastPosLength = 9;

    int status = 0;
    const int NOT_FOUND = 0, WAIT_STABILIZE = 1, COLOR_RECOGNIZE = 2, FOUND = 3;
    //Vector3 campos;
    Vector3[] lastPositions;// = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    bool probesFound = false;

    ResourceRequest rR = null;

    void Start()
    {

        ProbePosition = new Vector3[color_probes.Length];

        initLastPositions();

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        /*
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED)
            {
                GameManager.Instance.currentSound = null;
                GameObject[] garbage = GameObject.FindGameObjectsWithTag("Location");
                foreach (GameObject garb in garbage) Destroy(garb.gameObject);
            }
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }*/
        if (newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }

        if (newStatus == TrackableBehaviour.Status.NOT_FOUND /*||
            newStatus == TrackableBehaviour.Status.UNDEFINED ||
            newStatus == TrackableBehaviour.Status.UNKNOWN*/
            )
        {
            OnTrackingLost();
        }

    }


    private void OnTrackingFound()
    {
        if (status != FOUND)
        {

            status = WAIT_STABILIZE;
            GameManager.Instance.holdCameraTips.SetActive(true);
            GameManager.Instance.targetCameraToTips.SetActive(false);
        }
        //Поиск пробников
    }

    private void spawnScene()
    {
        GameManager.Instance.currentSound = null;
        StartCoroutine(PasteLocation());
        GameManager.Instance.status = GameManager.STATUS.MarkerFound;
    }

    private void OnTrackingLost()
    {



        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.txtPause.SetActive(false);
        if (GameManager.Instance.scorePanel != null) GameManager.Instance.scorePanel.SetActive(false);
        if (loadingImage != null) loadingImage.GetComponent<MeshRenderer>().enabled = (false);
        status = NOT_FOUND;
        probesFound = false;

        if (GameManager.Instance.holdCameraTips != null) GameManager.Instance.holdCameraTips.SetActive(false);
        if (GameManager.Instance.targetCameraToTips != null) GameManager.Instance.targetCameraToTips.SetActive(true);

        //lastPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        initLastPositions();

        if (GameManager.Instance.status != GameManager.STATUS.MarkerSearching) GameManager.Instance.status = GameManager.STATUS.MarkerSearching;
        GameObject[] garbage = GameObject.FindGameObjectsWithTag("Location");
        foreach (GameObject garb in garbage) Destroy(garb.gameObject);
        GameManager.Instance.status = GameManager.STATUS.MarkerSearching;
        GetScreen.Instance.needRenderScreen = false;

        //if(rR!=null && rR.asset)Resources.UnloadAsset (rR.asset);
        rR = null;

        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    void Update()
    {
        //campos = Camera.main.transform.position;

        if (status == WAIT_STABILIZE)
        {
            FillPoses();


            for (int i = 0; i < color_probes.Length; i++)
            {
                ProbePosition[i] = Camera.main.WorldToViewportPoint(color_probes[i].position);
                ProbePosition[i].x *= Screen.width;
                ProbePosition[i].y = Screen.height - Screen.height * ProbePosition[i].y;
                //ProbePosition[i].z=0;

                if (ProbePosition[i].x < 0 || ProbePosition[i].x > Screen.width || ProbePosition[i].y < 0 || ProbePosition[i].y > Screen.height)
                    probesFound = false;
                else
                    probesFound = true;
            }

            if (lastPositions[lastPositions.Length - 1] != Vector3.zero && (Camera.main.transform.position - GetAvaregePosition()).magnitude < 0.1f && probesFound)
            {
                GameManager.Instance.holdCameraTips.SetActive(false);
                GameManager.Instance.targetCameraToTips.SetActive(false);
                //Debug.Log("ЗБС");
                status = COLOR_RECOGNIZE;
                //ебашим экранные координаты V
                Vuforia.VuforiaManager.Instance.Deinit(); //останавливаем вуфорию V
                spawnScene();//даем команду распознание цвета V
                //как распознали цвет, ебашим FOUND... в корутине
            }
        }
    }

    void FillPoses()
    {
        for (int i = lastPosLength - 1; i > 0; i--)
        {
            lastPositions[i] = lastPositions[i - 1];
        }
        lastPositions[0] = Camera.main.transform.position;
    }

    Vector3 GetAvaregePosition()
    {
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < lastPositions.Length; i++)
        {
            sum += lastPositions[i];
        }

        sum /= lastPositions.Length;

        return sum;
    }

    private void initLastPositions()
    {
        lastPositions = new Vector3[lastPosLength];
        for (int i = 0; i < lastPosLength; i++)
        {
            lastPositions[i] = Vector3.zero;
        }
    }

    IEnumerator PasteLocation()
    {

        GameObject[] garbage = GameObject.FindGameObjectsWithTag("Location");
        foreach (GameObject garb in garbage) Destroy(garb.gameObject);

        Resources.UnloadUnusedAssets();
        System.GC.Collect();




        //распознаем цвета
        //yield return new WaitForSeconds(0.2f);
        //Vuforia.VuforiaManager.Instance.Deinit();

        GetScreen.Instance.needRenderScreen = true;
        GetScreen.Instance.ProbePosition = ProbePosition;

        while (!GetScreen.Instance.texturePrepared) { yield return new WaitForSeconds(0.02f); }

        colors_recognized = GetScreen.Instance.GetColors();
        //Debug.Log(""+colors_recognized.Length);
        Vuforia.VuforiaManager.Instance.Init();

        /*
        for (int i=0; i<colors_recognized.Length;i++)
        {
            Debug.Log("Color: "+colors_recognized[i].ToString());
        }*/
        //---------------

        //yield return new WaitForSeconds(0.1f);

        loadingImage.GetComponent<MeshRenderer>().enabled = (true);

        if (rR != null)
        {
            try
            {
                Resources.UnloadAsset(rR.asset);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally
            {
                Debug.Log("!!!Check for loading shit...");
            }
        }
        rR = Resources.LoadAsync(locationName);
        /*while (rR.progress<1) 
        {
            Debug.Log(rR.progress.ToString());
        }*/
        yield return rR;
        Instantiate(rR.asset as GameObject);

        loadingImage.GetComponent<MeshRenderer>().enabled = (false);
        /*
        GameObject[] garbage = GameObject.FindGameObjectsWithTag("Location");
        if(garbage.Length>0)
        for (int i = 0; i < garbage.Length-1; i++)
        {
            Destroy(garbage[i].gameObject);
        }*/
        status = FOUND;

        yield return null;

    }

}
