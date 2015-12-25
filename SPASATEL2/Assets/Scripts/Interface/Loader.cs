using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	
	public static string LEVEL
	{
		set {targetLevel = value; oldLevel = Application.loadedLevel;}
		private get {return targetLevel;}
	}

	static string targetLevel;
	static int oldLevel;

	public RectTransform loadImage;
	
	void Start () 
	{
		StartCoroutine(LoadLevel());
	}
	
	
	void Update () 
	{
		loadImage.Rotate(Vector3.forward*Time.deltaTime*500);
	}
	
	IEnumerator LoadLevel()
	{

		Application.LoadLevelAsync(targetLevel);
		yield return null;
	}

	void OnDestroy()
	{
		targetLevel = null;
		//oldLevel = null;

		Application.UnloadLevel (oldLevel);
		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();

		Debug.Log ("onDestroy Loader");
	}
}
