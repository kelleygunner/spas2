using UnityEngine;
using System.Collections;

public class StarsPanel : MonoBehaviour {

	public delegate void StarsEvent (int num);
	public static StarsEvent eventShow;
	public static StarsEvent eventHide;
	public static StarsEvent eventPlaySound;


	public Animator animator;
	public GameObject starsPanel;

	public GameObject[] stars;

	public AudioSource aSourse;
	public AudioClip[] sounds;

	// Use this for initialization
	void Start () {
	
		eventShow += showStars;
		eventHide += hideStars;
		eventPlaySound += vplaySound;
	}
	void OnDestroy(){
		eventShow -= showStars;
		eventHide -= hideStars;
		eventPlaySound -= vplaySound;
	}

	public static void show(int num){
		if (eventShow != null)
			eventShow (num);
	}
	public static void hide(){
		if (eventHide != null)
			eventHide (0);
	}
	public static void playSound(int num){
		if (eventPlaySound != null)
			eventPlaySound (num);
	}

	private void hideStars(int num){

		animator.SetInteger ("show_stars",0);

		starsPanel.SetActive (false);
		/*
		for (int j = 0; j < 3; j++) {
			stars[j].SetActive(false);
		}*/
	}

	private void showStars(int num){

		for (int j = 0; j < 3; j++) {
			stars[j].SetActive(false);
		}

		starsPanel.SetActive (true);
		animator.SetInteger ("show_stars",num);


		/*
		for (int i = 0; i < num; i++) {

			StartCoroutine( waitShowStar(i, i * 0.5f) );
		}*/
	}

	private void vplaySound(int n){
		if (n >= sounds.Length)
			n = sounds.Length -1;

		aSourse.clip = sounds [n];
		aSourse.Play ();
	}

	public void hideStars(){

		animator.SetInteger ("show_stars",0);
	}

	private IEnumerator waitShowStar( int num, float f ){

		yield return new WaitForSeconds( f );
		stars[num].SetActive (true);
		aSourse.clip = sounds [num];
		aSourse.Play ();
        if (num == 2)
        {
            yield return new WaitForSeconds(1f);
            aSourse.clip = sounds[Random.Range(3,5)];
            aSourse.Play();
        }
	}

}
