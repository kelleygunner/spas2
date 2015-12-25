using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fireBattleManager : MonoBehaviour 
{
	public Button pauseBtn;
	public GameObject pauseMenu;
	// Use this for initialization
	void Start () {
	

	}



    public void Exit()
    {
        Loader.LEVEL = "game";
        Application.LoadLevel("loader");
    }
	[HideInInspector()]
	public bool isPause = false;
	public void onPause(){

		isPause = !isPause;
		if (isPause) {
			pauseMenu.SetActive(true);
			Time.timeScale = 0f;
		} else {
			pauseMenu.SetActive(false);
			Time.timeScale = 1f;
		}
	}
    

}
