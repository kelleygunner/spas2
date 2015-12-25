using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CatalogMenu : MonoBehaviour//, IPointerClickHandler, IPointerEnterHandler
{
	public GameObject ui;
	public GameObject loading;
	[Space(5)]
	public Scrollbar scroll;
	[Space(5)]
	public RectTransform moving;
	public GameObject itemPrefab;
	[Space(5)]
	//public Scrollbar scroll;

	private bool isContentSpawned = false;
	private bool isContentLoaded = false;
	private bool isActive = false;

	private string[] links;
	private string[] names;
	private Texture2D[] imgs;
	private string[] description;
	private string[] outlink;

	//private string testURLImg = "http://2.bp.blogspot.com/-y6oN4ph9tg4/Ub8JNeltqeI/AAAAAAAAADE/qLSIUCWrEos/s1600/KEmulator.png";
	private Texture2D testImg;

	private GameObject[] items;
	private float contentHeight = 0f;
	private float contentHeightOffset = 0f;
	
	private string server = "http://hotlinns.bget.ru/";
	

	void Start ()
    {
        StartCoroutine( GetCatalogAdresses());
	}
	
	
	void Update ()
    {
		if ( isActive && !isContentSpawned && isContentLoaded)
			spawnContent ();


		if (isPointerMove)
			moveSwip ();
	}

	//private Vector2 lastPoint;
	private Vector2 curPoint;
	private void moveSwip(){


	}
	private Vector2 startPoint;
	private Vector2 lastPoint;
	private float deltaPos = 0.0f;
	private float minMove = 3f;
	public void onPointerDown(UnityEngine.EventSystems.BaseEventData baseEvent) {

		isPointerDown = true;
		if (Application.isEditor)
			startPoint = Input.mousePosition;
		else
			startPoint = Input.touches[0].position;

		lastPoint = startPoint;
	}
	
	private void onPointerDrag(UnityEngine.EventSystems.BaseEventData baseEvent)
	{
		Vector2 curPos = Vector3.zero;
		if (Application.isEditor)
			curPos = Input.mousePosition;
		else
			curPos = Input.touches[0].position;
		
		float speedRot = lastPoint.y - curPos.y;

		if (Mathf.Abs (Mathf.Abs (startPoint.y) - Mathf.Abs (curPos.y)) > minMove)
			isPointerMove = true;

		deltaPos = speedRot / contentHeight;
		//move here
		if (isPointerMove)
			moveScroll (deltaPos);

		//spawnedModel.transform.Rotate(0f, speedRot, 0f);
		lastPoint = curPos;
	}   

	
	private void spawnContent(){
		
		contentHeightOffset = Screen.height * 0.904f;//0.86f;
		items = new GameObject[links.Length];
		Vector3 posPanel = moving.transform.position;
		//float xDecrem = 200f;//posPanel.x -
		float deltaScale = 625f / 200f;
		GameObject go;
		RectTransform rt = moving.GetComponent<RectTransform>();
		Vector2 size = new Vector2( Screen.width * 0.77f, Screen.height/deltaScale );
		CatalogItem item;
		EventTrigger et;

		for (int i = 0; i < links.Length; i++) {

			posPanel = moving.transform.position;
			posPanel.y = posPanel.y -(size.y * i + size.y / 2f);
			contentHeight = posPanel.y;
			//posPanel.x = 0f;

			go = Instantiate( itemPrefab, posPanel, Quaternion.identity ) as GameObject;
			rt = go.GetComponent<RectTransform>();
			rt.sizeDelta = size;
			

			item = go.GetComponent<CatalogItem>();

			//*******************Add Values in Item*********************
			item.initItem( i, names[i], description[i], outlink[i], imgs[i] );
			//**********************************************************

			//-----------------------Add Listener Click---------------------
			et = go.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			
			UnityEngine.Events.UnityAction<BaseEventData> listener;
			listener = new UnityEngine.Events.UnityAction<BaseEventData>(itemClick);

			entry.callback.AddListener( listener );
			et.triggers.Add(entry);
			//-------------------------Down----------------------------------
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			
			//UnityEngine.Events.UnityAction<BaseEventData> listener;
			listener = new UnityEngine.Events.UnityAction<BaseEventData>(onPointerDown);

			entry.callback.AddListener( listener );
			et.triggers.Add(entry);
			//--------------------------Drag---------------------------------
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			
			//UnityEngine.Events.UnityAction<BaseEventData> listener;
			listener = new UnityEngine.Events.UnityAction<BaseEventData>(onPointerDrag);
			
			entry.callback.AddListener( listener );
			et.triggers.Add(entry);
			//--------------------------------------------------------------
			
			//go.transform.SetParent( moving.transform, true );
			go.GetComponent<RectTransform>().SetParent(moving.transform);// = new Rect( posPanel, size );
						
			items[i] = go;
		}
		contentHeight -= size.y;
		isContentSpawned = true;
		showLoading (!isContentLoaded);
	}

	private bool isPointerMove = false;
	private bool isPointerDown = false;
	private void itemDown(UnityEngine.EventSystems.BaseEventData baseEvent){
		Debug.Log ("itemDown ");
	}

	private void itemClick(UnityEngine.EventSystems.BaseEventData baseEvent){

		if(!isPointerMove){ //Handle Click
			CatalogItem item = getItemInParents (baseEvent.selectedObject);//baseEvent.selectedObject.GetComponentInParent<CatalogItem> ();//GetComponent<CatalogItem> ();
			//int num = (int)baseEvent.selectedObject.GetComponent<CatalogItem> ().num;
			Debug.Log ("itemClick " + item.num.ToString ());
		}

		isPointerDown = false;
		isPointerMove = false;
	}
	//Рекурсия.. бу-бу-буууу
	private CatalogItem getItemInParents(GameObject go){

		if (go == null)
			return null;

		CatalogItem item = go.GetComponent<CatalogItem> ();
		if( item != null )
			return item;
		else if (go.transform.parent != null)
			return getItemInParents(go.transform.parent.gameObject);
		else
			return null;
	}


	public void onScrollChange(Scrollbar sb){

		Vector3 pos = moving.transform.position;
		pos.y = -contentHeight * sb.value + contentHeightOffset;
		moving.transform.position = pos;
		//Debug.Log ("onScrollChange " + val.ToString ());
		//Debug.Log ("onScrollChange " + sb.value.ToString ());
	}

	public void moveScroll(float f){
		//if (f < 0f)	f = 0f;
		//if (f > 1f)	f = 1f;

		scroll.value += f;

		Vector3 pos = moving.transform.position;
		pos.y = -contentHeight * scroll.value + contentHeightOffset;
		moving.transform.position = pos;

	}

    IEnumerator GetCatalogAdresses()
    {
        WWW www = new WWW(server + "catalog/catalog.txt");
        yield return www;
        links = www.text.Split('\n');
        names = new string[links.Length];
		imgs = new Texture2D[links.Length];
		outlink = new string[links.Length];
		description = new string[links.Length];
        www.Dispose();
        StartCoroutine(InitNames());
    }

    IEnumerator InitNames()
    {
        for (int i = 0; i < links.Length; i++)
        {
            //StartCoroutine(GetName("http://anigames.besaba.com/catalog/" + links[i] + "/title.txt", i));
            //yield
            WWW name_www = new WWW(server + "catalog/" + links[i] + "/title.txt");
            yield return name_www;
            names[i] = name_www.text;
			
			WWW img_www = new WWW(server + "catalog/" + links[i] + "/icon.jpg");
            yield return img_www;
            imgs[i] = img_www.texture;
			
			WWW desc_www = new WWW(server + "catalog/" + links[i] + "/description.txt");
            yield return desc_www;
            description[i] = desc_www.text;
			
			WWW outl_www = new WWW(server + "catalog/" + links[i] + "/link.txt");
            yield return outl_www;
            outlink[i] = outl_www.text;
        }

		

		/*
		//test
		Texture2D imggg = imgs [0];
		int nn = 30;
		names = new string[nn];
		outlink = new string[nn];
		links = new string[nn];
		description = new string[nn];
		imgs = new Texture2D[nn];

		for(int j = 0; j < nn; j++){
			names[j] = "Title " + j.ToString();
			outlink[j] = "Description " + j.ToString();
			description[j] = "Description " + j.ToString();
			imgs[j] = imggg;
		}
		//test */

		isContentLoaded = true;
    }
	
	/*
    IEnumerator GetName(string link,int pos)
    {
        WWW name_www = new WWW(link);
        yield return name_www;
        names[pos] = name_www.text;
        name_www.Dispose();
    }*/
	private void showLoading(bool b){
		
		loading.SetActive (b);
	}
	public void setVisible(bool b){

		ui.SetActive (b);
		isActive = b;
	}



}
