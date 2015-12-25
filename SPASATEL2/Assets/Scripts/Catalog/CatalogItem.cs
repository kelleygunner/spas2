using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatalogItem : MonoBehaviour {

	public Text title;
	public Text description;
	public Image img;

	public string link = "";
	public int num = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void initItem( int num, string title, string description, string link, Texture2D img){

		this.num = num;
		this.title.text = title;
		this.description.text = description;
		this.link = link;

		if (img != null) {
			//Sprite sp = Sprite.Create (img, this.img.rectTransform.rect, this.img.rectTransform.pivot);
			Sprite sp = Sprite.Create (img,  new Rect(0, 0, img.width, img.height), new Vector2(0.5f,0.5f));
			this.img.sprite = sp;
		}
	}

	public override string ToString(){
		return "num " + num.ToString() + "\ntitle" + title + "\ndescription" + description + "\nlink" + link;
	}
}
