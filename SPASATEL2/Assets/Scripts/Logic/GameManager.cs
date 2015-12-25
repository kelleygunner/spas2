using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    { get { return _instance; } }

    public STATUS status
    {
        get { return _status; }
        set { _status = value; OnStatusChanged(); }
    }

    private STATUS _status = STATUS.MarkerSearching;

    #region Public data
    public Text txtStatus;
    public GameObject Scene2Panel;
    public GameObject scorePanel;
    public Text txtScore;
    public Text txtRecord;
    public Animator scorePanelAnimator;
    public GameObject Scene3Panel;

    public AvatarPanel avatarPanel;

    public GameObject TutorialStr6;

    [Space(10)]
    public GameObject ARCamera;
    #endregion

    //Button btnExit;

    public GameObject txtPomogiPotushit;


    bool isPause = false;
    public float oldTimeScale = 1;

    [Space(5)]
    public GameObject ControlPanel;
    public GameObject ControlPanelSmall;

    public GameObject tipPanel;

    public GameObject txtPause;

    public Button btnPlay;
    public Button btnPause;
    public Button btnPauseSmall;

    public Button btnReplay;
    public Button btnReplaySmall;

    public Button btnExit;
    public Button btnExitSmall;

    public GameObject holdCameraTips;
    public GameObject targetCameraToTips;


    public AudioSource currentSound = null;

    public GameObject allMarkers;

    void Awake()
    {
        _instance = this;
        //btnExit = GameObject.Find("ButtonExit").GetComponent<Button>();
        btnExit.onClick.AddListener(OnButtonExitClick);
        btnExitSmall.onClick.AddListener(OnButtonExitClick);
        Time.timeScale = 1f;
        //StartCoroutine (StartDelay(3f));
    }

    IEnumerator StartDelay(float fDelay)
    {
        yield return new WaitForSeconds(fDelay);
        allMarkers.SetActive(true);
        yield return null;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Loader.LEVEL = "menu"; Application.LoadLevel("loader"); }
    }

    void OnStatusChanged()
    {
        switch (_status)
        {
            case STATUS.MarkerSearching:
                {
                    if (txtStatus != null) txtStatus.text = "Наведите камеру на маркер...";
                    break;
                };
            case STATUS.MarkerFound:
                {
                    if (txtStatus != null) txtStatus.text = "";
                    break;
                };
            case STATUS.MarkerLost:
                {
                    if (txtStatus != null) txtStatus.text = "Маркер потерян!";
                    break;
                };
        }
    }

    public enum STATUS
    {
        MarkerSearching,
        MarkerFound,
        MarkerLost
    }

    public void OnBack()
    {
        Destroy(GameObject.FindGameObjectWithTag("Location"));
        status = STATUS.MarkerSearching;
        //Vuforia.TrackableBehaviour tb = Vuforia.TrackerManager.Instance.GetTracker<Vuforia.TrackableBehaviour>();
        //tb.OnTrackerUpdate(Vuforia.TrackableBehaviour.Status.NOT_FOUND);

    }

    private void OnButtonExitClick()
    {
        Loader.LEVEL = "menu";
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        Application.LoadLevelAsync("loader");
        yield return null;
    }

    public void Pause()
    {
        isPause = !isPause;
        if (isPause) { oldTimeScale = Time.timeScale; Time.timeScale = 0; txtPause.SetActive(true); }
        else { Time.timeScale = oldTimeScale; txtPause.SetActive(false); }
    }

    public void PauseSound()
    {
        if (currentSound != null && currentSound.isPlaying) currentSound.Pause();
    }

    public void ContinueSound()
    {
        if (currentSound != null) currentSound.UnPause();
    }

}
