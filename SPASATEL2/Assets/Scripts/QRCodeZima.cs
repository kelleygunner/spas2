using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QRCodeZima : MonoBehaviour 
{
    WebCamTexture cameraTexture;

    Material cameraMat;
    public GameObject plane;

    WebCamDecoder decoder;

    bool isRecognized = false;

    string code = "";

    public Text txtStatus;

    private string server = "http://hotlinns.bget.ru/";
    private string request = "spasatel1_codes/index.php?pin=";

    int tries = 0;
    bool isActivated = false;

    public GameObject resultPanel;
    public GameObject waitText;

    IEnumerator Start() 
    {
        cameraMat = plane.GetComponent<MeshRenderer>().material;
        decoder = GetComponent<WebCamDecoder>();

        var devices = WebCamTexture.devices;
        var deviceName = devices[0].name;
        cameraTexture = new WebCamTexture(deviceName, (int)(Screen.width*0.5f), (int)(Screen.height*0.5f));
        cameraTexture.Play();

        yield return StartCoroutine(decoder.StartDecoding(cameraTexture));

        cameraMat.mainTexture = cameraTexture;

        // adjust texture orientation;
        plane.transform.rotation = plane.transform.rotation *
            Quaternion.AngleAxis(cameraTexture.videoRotationAngle, Vector3.up);


	}
	

	void Update () 
    {
        if (!isRecognized && decoder.Result.Success)
        {
            isRecognized = true;
            code = decoder.Result.Text;
            plane.SetActive(false);
            StartCoroutine(SendRequest(server + request + code));
            resultPanel.SetActive(true);
            waitText.SetActive(false);
        }
	}

    IEnumerator SendRequest(string data)
    {
        WWW www = new WWW(data);
        yield return www;

        int res = -1;

        if (int.TryParse(www.text, out res))
        {
            if (res > 0)
            {
                isActivated = true;
                PlayerPrefs.SetInt("ACTIVATED", 1);
                tries = res;
                ShowResult(Status.ACTIVATED);
            }
            else
            {
                if (res == -1) ShowResult(Status.WRONG_PIN);
                if (res == 0) ShowResult(Status.LIMIT_OVERFLOW);
            }
        }
        else
        {

            ShowResult(Status.ERROR);
        }

        yield return null;
    }

    void ShowResult(Status status)
    {
        //MainPanel.SetActive(false);
        //ResultPanel.SetActive(true);

        switch (status)
        {
            case Status.ERROR: { txtStatus.text = "Ошибка запроса. Проверьте подкодение к сети интернет"; break; }
            case Status.LIMIT_OVERFLOW: { txtStatus.text = "Количество активаций кода исчерпано."; break; }
            case Status.WRONG_PIN: { txtStatus.text = "Код не опознан. Попробуйте еще раз"; break; }
            case Status.ACTIVATED: { txtStatus.text = "Активировано! Вы можете использовать данный код еще " + (tries - 1).ToString() + " раз(а)...\n\n При повторной установке необходимо будет активировать код снова."; break; }
        }

    }

    public void OnOkClick()
    {
        //ResultPanel.SetActive(false);

        if (isActivated)
        {
            Loader.LEVEL = "game";
            Application.LoadLevel("loader");
        }
        else
        {
            waitText.SetActive(true);
            plane.SetActive(true);
            Application.LoadLevel(Application.loadedLevel);
            //decoder.Result.
        }
    }

    public void OnQuit()
    {
        Application.LoadLevel("menu");
    }

    public enum Status
    {
        WAIT,
        ERROR,
        ACTIVATED,
        WRONG_PIN,
        LIMIT_OVERFLOW
    }

}
