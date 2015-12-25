using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TapIcon : MonoBehaviour {

	
	public Sprite[] tapTexture;
	int index=1;
	
	Image img;
	
	void Start () 
	{
		img = this.GetComponent<Image>();
		StartCoroutine(tick());
	}
	
	
	IEnumerator tick()
	{
		while(true)
		{
			index+=1;
			if (index>1)index=0;
			img.sprite =  tapTexture[index];
			yield return new WaitForSeconds(1.5f);
		}
	}
}
