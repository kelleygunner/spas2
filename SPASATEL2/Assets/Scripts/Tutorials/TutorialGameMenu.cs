using UnityEngine;
using System.Collections;

public class TutorialGameMenu : MonoBehaviour {

	public GameObject tutorialPause;
	public GameObject tutorialPhoto;
	public GameObject tutorialRepeat;
	public GameObject tutorialPlay;
	public GameObject tutorialExit;

	private const string TUTORIAL_PAUSE = "tutorial_pause";
	private const string TUTORIAL_PHOTO = "tutorial_photo";
	private const string TUTORIAL_REPEAT= "tutorial_repeat";
	private const string TUTORIAL_PLAY  = "tutorial_play";
	private const string TUTORIAL_EXIT  = "tutorial_exit";

	void Start () {
	
		//PlayerPrefs.DeleteAll ();
	}

	public void showPauseTutorial(bool show){

		if (show) {
			if (PlayerPrefs.GetInt (TUTORIAL_PAUSE) == 0){
				tutorialPause.SetActive (true);
				//GameManager.Instance.onPause();
			}
		}else {
			tutorialPause.SetActive (false);
			//GameManager.Instance.onPause();
			//PlayerPrefs.SetInt (TUTORIAL_PAUSE, 1);
		}
	}

	public void showPhotoTutorial(bool show){
		
		if (show) {
			if (PlayerPrefs.GetInt (TUTORIAL_PHOTO) == 0){
				tutorialPhoto.SetActive (true);
				//GameManager.Instance.onPause();
			}
		}else {
			tutorialPhoto.SetActive (false);
			//GameManager.Instance.onPause();
			//PlayerPrefs.SetInt (TUTORIAL_PHOTO, 1);
		}
	}

	public void showRepeatTutorial(bool show){
		
		if (show) {
			if (PlayerPrefs.GetInt (TUTORIAL_REPEAT) == 0)
				tutorialRepeat.SetActive (true);
		}else {
			tutorialRepeat.SetActive (false);
			//GameManager.Instance.onPause ();
			//PlayerPrefs.SetInt (TUTORIAL_REPEAT, 1);
		}
	}

	void Update () {
	
	}
}
