using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuOptions : MonoBehaviour {

	public GameObject ui;
	public GameObject[] borders;

	private int[] graphycsLevels = {2,3,4}; // Fastests, Good, Fantastic
	private int currentLevel = 0;

	private const string GRAPHYCS_Q = "graphycs_q";


	// Use this for initialization
	void Start () {
	
		init ();
	}

	private void init(){

		if (PlayerPrefs.HasKey (GRAPHYCS_Q))
			currentLevel = PlayerPrefs.GetInt (GRAPHYCS_Q);
		else {
			int ii = QualitySettings.GetQualityLevel ();
			for (int i = 0; i < borders.Length; i++) {
				if(ii == graphycsLevels [i])
					currentLevel = i;
			}
		}
		showBorder (currentLevel);
		setGfxLevel (currentLevel);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onGraphycs(int i){

		currentLevel = i;
		setGfxLevel (currentLevel);
		showBorder (currentLevel);

		PlayerPrefs.SetInt (GRAPHYCS_Q, currentLevel);
		PlayerPrefs.Save ();

		Debug.Log ("onGraphycs " + i.ToString ());

	}
	private void setGfxLevel(int gfxLevel){

		int level = graphycsLevels [gfxLevel];
		QualitySettings.SetQualityLevel (level, true);
		/*
		switch (gfxLevel) {

		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 0:
			break;
		case 0:
			break;
		}//*/
	}

	private void showBorder(int i){

		foreach (GameObject go in borders)
			go.GetComponent<Image>().color = new Color(255f,255f,255f,0.3f);
		borders [i].GetComponent<Image>().color = new Color(255f,255f,255f,1f);

			//go.SetActive (false);
		//borders [i].SetActive (true);

	}

	public void onBack(){

		setVisible (false);
	}

	public void setVisible(bool b){
		
		ui.SetActive (b);
	}
}
