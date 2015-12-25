using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivationManager : MonoBehaviour 
{
    public GameObject MainPanel;
    public GameObject ResultPanel;
    public InputField iField;
    public Text txtStatus;

    private string server = "http://hotlinns.bget.ru/";
    private string request = "spasatel1_codes/index.php?pin=";

    int tries=0;
    bool isActivated = false;

	void Start () 
    {
        if (PlayerPrefs.GetInt ("ACTIVATED") == 1) {
			Loader.LEVEL = "menu";
			Application.LoadLevelAsync ("loader");
		} else 
		{
			MainPanel.SetActive(true);
		}

    }
    /*
    IEnumerator LoadMenu()
    {

        yield return null;
    }*/
	
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

    public void Submit()
    {
        StartCoroutine( SendRequest(server + request + iField.text.ToUpper()));
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

    //Коды результатов:
    // null - Ошибка запроса
    // (-1) - пин-код не верен
    // (0) - лимит попыток использования пин-кода исчерпан
    // (1,2,...,n) - активация успешна, осталось столько попыток

    public enum Status
    { 
        WAIT,
        ERROR,
        ACTIVATED,
        WRONG_PIN,
        LIMIT_OVERFLOW
    }

    void ShowResult(Status status)
    {
        MainPanel.SetActive(false);
        ResultPanel.SetActive(true);

        switch (status)
        {
            case Status.ERROR: {txtStatus.text = "Ошибка запроса. Проверьте подкодение к сети интернет"; break;}
            case Status.LIMIT_OVERFLOW: { txtStatus.text = "Количество активаций кода исчерпано."; break; }
            case Status.WRONG_PIN: { txtStatus.text = "Вы ввели недействующий код"; break; }
            case Status.ACTIVATED: { txtStatus.text = "Активировано! Вы можете использовать данный код еще " + (tries-1).ToString() + " раз(а)...\n\n При повторной установке необходимо будет активировать код снова."; break; }
        }

    }

    public void OnOkButtonClick()
    {
        ResultPanel.SetActive(false);

        if (isActivated)
        {
            Loader.LEVEL = "menu";
            Application.LoadLevel("loader");
        }
        else 
        {
            MainPanel.SetActive(true);
        }
    }


    public void Exit()
    {
        Application.Quit();
    }

}
