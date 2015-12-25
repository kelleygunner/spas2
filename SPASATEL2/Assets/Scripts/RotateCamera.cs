using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour 
{
	
	#region Rects
    Rect rectBtnRepeat;
    Rect rectBtnPlay;
    #endregion
	
	public SkinnedMeshRenderer seroja;
	public SkinnedMeshRenderer shlen;

    public SkinnedMeshRenderer pukan;
    public SkinnedMeshRenderer kot;
	
	void Start () 
	{
        Init();

        seroja.materials[0].color = MarkerHandler.colors_recognized[0];
        seroja.materials[1].color = MarkerHandler.colors_recognized[3];
        seroja.materials[3].color = MarkerHandler.colors_recognized[1];
        seroja.materials[2].color = MarkerHandler.colors_recognized[2];
        seroja.materials[5].color = MarkerHandler.colors_recognized[4];
        seroja.materials[6].color = MarkerHandler.colors_recognized[5];
        seroja.materials[7].color = MarkerHandler.colors_recognized[6];

        shlen.material.color = MarkerHandler.colors_recognized[12];

        kot.materials[1].color = MarkerHandler.colors_recognized[7];

        pukan.materials[3].color = MarkerHandler.colors_recognized[8];
        pukan.materials[4].color = MarkerHandler.colors_recognized[9];
        pukan.materials[5].color = MarkerHandler.colors_recognized[10];
        pukan.materials[6].color = MarkerHandler.colors_recognized[11];

	}
	
	
	void Update () 
	{
		this.transform.Rotate(Vector3.up*Time.deltaTime*-10);
	}
	
	void Init()
	{
		#region Rects
        rectBtnRepeat = new Rect(Screen.width * 0.5f - Screen.height * 0.25f, Screen.height * 0.1f, Screen.height * 0.2f, Screen.height * 0.2f);
        rectBtnPlay = new Rect(Screen.width * 0.5f + Screen.height * 0.05f, Screen.height * 0.1f, Screen.height * 0.2f, Screen.height * 0.2f);
        #endregion
	}
	
	void OnGUI()
	{
        /*
		if (GUI.Button(rectBtnRepeat, "", Styles.Instance.btnRepeat))
        {
            Loader.LEVEL="fire_battle";
			StartCoroutine( LoadLevel("loader"));
        }
		
		if (GUI.Button(rectBtnPlay, "", Styles.Instance.btnClose))
        {
            Loader.LEVEL="game";
			StartCoroutine( LoadLevel("loader"));
        }*/
		
	}
	
	IEnumerator LoadLevel(string Level)
	{
		
		Application.LoadLevelAsync(Level);
		yield return null;
	}

    

}
