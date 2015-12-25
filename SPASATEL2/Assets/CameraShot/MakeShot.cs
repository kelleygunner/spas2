using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using System.Text;
using Vuforia;

public class MakeShot : MonoBehaviour {

	//public Text text;
	public GameObject panelUI;
	public GameObject imgDone;
	[Space(5)]
	public UnityEngine.UI.Image savedIMG;
	public Text savedText;

	private string screenPath = "";
	private CaptureAndSave snapShot;
	// Use this for initialization

    float oldScale = 0;

    public AudioSource auShot;

	void Start () {
	
		//Application.targetFrameRate = 10;
		snapShot = GameObject.FindObjectOfType<CaptureAndSave>();
		screenPath = Application.persistentDataPath;

        if (VuforiaManager.Instance != null && VuforiaManager.Instance.Initialized)
		    if(!CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
			    CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);

		addListeners ();
		//CameraDevice.Instance.SetFrameFormat (Vuforia.Image.PIXEL_FORMAT.GRAYSCALE, true);
	}

	public void onRotateCam(){
		
		CameraDevice.Instance.Stop();
		if (CameraDevice.Instance.GetCameraDirection () == CameraDevice.CameraDirection.CAMERA_BACK) {
			CameraDevice.Instance.Init (CameraDevice.CameraDirection.CAMERA_FRONT);
		} else {
			CameraDevice.Instance.Init (CameraDevice.CameraDirection.CAMERA_BACK);
		}
		CameraDevice.Instance.Start();

	}

	// Update is called once per frame
	void Update () {
	
	}


	private Texture2D materialRenderer;

	//private Vuforia.Image.PIXEL_FORMAT pFomat = 
	public void shotRaw(){

		waitScreen = true;
		//doRawShot ();
		StartCoroutine (RPixels ());
	}

	private Vuforia.Image vImg;
	private bool waitScreen = false;
	private IEnumerator RPixels(){

		yield return new WaitForEndOfFrame();
		doRawShot ();
	}
	private void doRawShot(){

		waitScreen = false;

		int width = Screen.width;
		int height = Screen.height;


		Texture2D tex = new Texture2D (width, height);//, TextureFormat.RGB565, false);

		vImg = CameraDevice.Instance.GetCameraImage (Vuforia.Image.PIXEL_FORMAT.GRAYSCALE);
		vImg.CopyToTexture (tex);

		tex.Apply();
		
		DateTime dt = DateTime.Now;
		
		string path;
		string imgPath;
		if(Application.isEditor)
			path = "/AR_Screen/";
		else
			path = "/../../../../DCIM/AR_Screen/";
		
		string allDirPath = Application.persistentDataPath + path;
		if (!Directory.Exists(allDirPath))
			Directory.CreateDirectory (allDirPath);
		
		string str = string.Format ("_RawScreenshot_{0:D4}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.jpg", dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);
		imgPath = allDirPath + str;
		Debug.Log (str);
		
		byte[] ba = tex.EncodeToJPG ();
		File.WriteAllBytes (imgPath, ba);
		
		if (File.Exists (imgPath)) {
			Debug.Log ("saved " + imgPath);
			savedText.text = "Сохранено : " + imgPath;
		} else {
			savedText.text = "Не удалось сохранить : " + imgPath;
			Debug.Log ("not found " + imgPath);
		}
	}

	public void shot(){

		hideUIandShot ();
        if (Application.loadedLevelName == "game")
        {
            GameManager.Instance.PauseSound();
            oldScale = Time.timeScale;
            Time.timeScale = 0;
        }
        auShot.Play();

		//StartCoroutine (waitShot ());
	}
	 
	private void hideUIandShot(){

		panelUI.SetActive (false);
		snapShot.GetFullScreenShot ();
	}

	void OnScreenShot(Texture2D tex2D)
	{
		// assign screenshot
		tex = tex2D;
		savedIMG.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f));
		snapShot.SaveTextureToGallery (tex2D);
		isWaitShot = true;
		waitShotTime = 1f;
		imgDone.SetActive (true);
		//StartCoroutine (waitShowShot ());
	}

	private bool isWaitShot = false;
	private float waitShotTime = 1f;

	private void OnGUI(){

		if (isWaitShot) {

			waitShotTime -= 0.02f;
			if(waitShotTime <= 0f){
			
				isWaitShot = false;
				waitShotTime = 0f;
				imgDone.SetActive (false);
				panelUI.SetActive (true);

                if (Application.loadedLevelName == "game")
                {
                    if( oldScale != 0 )	GameManager.Instance.ContinueSound();
                    Time.timeScale = oldScale;
                }

			}
		}
	}

	private IEnumerator waitShowShot(){
						
		imgDone.SetActive (true);
		yield return new WaitForSeconds (1f); //Show saved img
		imgDone.SetActive (false);
		panelUI.SetActive (true);
		
	}

	private IEnumerator waitShot(){


		panelUI.SetActive (false);
		yield return new WaitForEndOfFrame();

		makeScreenShot ();
		yield return new WaitForSeconds (0.05f);
		imgDone.SetActive (true);
		yield return new WaitForSeconds (1f); //Show saved img
		imgDone.SetActive (false);
		panelUI.SetActive (true);

	}
	/*
	private Texture2D[] imgs = new Texture2D[100];
	private int imgIndex = 0;
	private bool isRec = false;
	public void rec(){

		isRec = true;
		//byte[]

		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		tex.Apply ();
		byte[] ba = tex.EncodeToJPG ();

		File.WriteAllBytes ("C://video.avi", ba);
		//File.WriteAllBytes(
	}*/

	private void makeScreenShot(){
		DateTime dt = DateTime.Now;

		Debug.Log(dt.ToString());
		Debug.Log(dt.ToShortDateString());
		Debug.Log(dt.ToShortTimeString());
		Debug.Log(dt.ToLongDateString());
		Debug.Log(dt.ToLongTimeString());

		string path = "";
		string imgPath;
		if(Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer)
			path = "/AR_Screen/";
		else if(Application.platform == RuntimePlatform.Android)
			path = "/../../../../DCIM/AR_Screen/";
		//else if(Application.platform == RuntimePlatform.Android)


		string allDirPath = Application.persistentDataPath + path;
		if (!Directory.Exists(allDirPath))
			Directory.CreateDirectory (allDirPath);

		string str = string.Format ("Screenshot_{0:D4}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.jpg", dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);
		imgPath = allDirPath + str;
		Debug.Log (str);

		/*
		if(Application.isEditor)
			Application.CaptureScreenshot(allPath + str);
		else
			Application.CaptureScreenshot(path + str);
		*/

		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		tex.Apply ();
		/*
		if(Application.isEditor)
			tex = loadPNG(allPath + str);
		else
			tex = loadPNG(path + str);
		*/

		savedIMG.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f));
		byte[] ba = tex.EncodeToJPG ();
		File.WriteAllBytes (imgPath, ba);

		if (File.Exists (imgPath)) {
			Debug.Log ("saved " + imgPath);
			savedText.text = "Сохранено : " + imgPath;
		} else {
			savedText.text = "Не удалось сохранить : " + imgPath;
			Debug.Log ("not found " + imgPath);
		}
		//AndroidGalleryPlugin.updateGallery ();
		//AndroidGalleryPlugin.shareText ("Share Title", "Text Text Text Text Text Text .");


		//Application.CaptureScreenshot (screenPath + "/screenshot.png", 2);

		//string path = System.IO.Path.Combine (Application.persistentDataPath,"image.png");

		//if(File. Exists(path)) WWW loadedImage = new WWW("file://" +path);


	}
	private Texture2D tex;
	void OnError(string error)
	{
		Debug.Log ("Error : "+error);
	}
	
	void OnSuccess(string msg)
	{
		Debug.Log ("Success : "+msg);
	}
	

	void addListeners()
	{
		CaptureAndSaveEventListener.onError += OnError;
		CaptureAndSaveEventListener.onSuccess += OnSuccess;
		CaptureAndSaveEventListener.onScreenShotInvoker += OnScreenShot;
	}

	void OnDestroy(){
		removeListeners ();
	}
	void removeListeners()
	{
		CaptureAndSaveEventListener.onError -= OnError;
		CaptureAndSaveEventListener.onSuccess -= OnSuccess;
		CaptureAndSaveEventListener.onScreenShotInvoker -= OnScreenShot;
	}


	public static Texture2D loadPNG(string filePath) {
		
		Texture2D tex = null;
		byte[] fileData;
		
		if (File.Exists(filePath)) {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}
}
