using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	
	public GameObject MainPanel;
	public GameObject AuthorPanel;
	public GameObject AboutPanel;
	
	public RectTransform loadImage;
	
	void Start () 
	{
		loadImage.gameObject.SetActive(false);
		//for cam 
		//WebCamTexture cam;
		//cam.Play ();
		//cam.Stop ();
		/*
		isUserNotIdiot = PlayerPrefs.GetInt (ACCESS_TO_CAM);
		if (isUserNotIdiot != 0) {


		} else {


		}//*/
	}/*
	private int isUserNotIdiot = 0;
	private const string ACCESS_TO_CAM = "access_to_cam";
	IEnumerator Start() { 
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam); 
		if (Application.HasUserAuthorization(UserAuthorization.WebCam)) { 

		} else { 
		} 
	}*/
	void Update () 
	{
		loadImage.Rotate(Vector3.forward*Time.deltaTime*500);	
	}
	
	public void onStartBtnClick()
	{

		loadImage.gameObject.SetActive(true);
        if (PlayerPrefs.GetInt("ACTIVATED") == 0)
        {
            Loader.LEVEL = "activation";
            StartCoroutine(LoadScene("loader"));
        }
        else
        {
            Loader.LEVEL = "game";
            StartCoroutine(LoadScene("loader"));
        }
		
	}
	
	public void onAuthorsBtnClick()
	{
		MainPanel.SetActive(false);
		AuthorPanel.SetActive(true);
	}
	
	public void onAboutBtnClick()
	{
		MainPanel.SetActive(false);
		AboutPanel.SetActive(true);
	}
	
	public void onExitBtnClick()
	{
		Application.Quit();
	}
	
	public void onAuthorBackClick()
	{
		MainPanel.SetActive(true);
		AuthorPanel.SetActive(false);
	}
	
	public void onAboutBackClick()
	{
		MainPanel.SetActive(true);
		AboutPanel.SetActive(false);
	}
	
	IEnumerator LoadScene(string level)
	{
		Application.LoadLevelAsync(level);
		yield return null;
	}
}
