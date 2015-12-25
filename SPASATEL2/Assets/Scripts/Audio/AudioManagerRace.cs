using UnityEngine;
using System.Collections;

public class AudioManagerRace : MonoBehaviour {

	[Header("Audio Sources")]
	public AudioSource musicSource;
	public AudioSource speechSource;
	public AudioSource sfxSource;
	[Space(5)]
	[Header("Single AudioClips")]
	public AudioClip musicLoopSound;
	public AudioClip lenaHelpDriveSound;
	public AudioClip lenaSwipeFingerSound;
	public AudioClip lenaFinishSound;
	[Space(5)]
	[Header("Array AudioClips")]
	public AudioClip[] stepaInteractStartSounds;
	public AudioClip[] crashSounds;
	public AudioClip[] crashLenaSounds;
	public AudioClip[] passBySounds;
	public AudioClip[] passByLenaSounds;


	public static AudioManagerRace instance = null;
	/*
	public static AudioManager instance {

		get{ if( _instance == null )
				_instance = new AudioManager();
			return _instance;
			}
	}
	//*/
	void Start(){

		instance = this;
		musicSource.loop = true;
	}
	/*
	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		init ();
		
		DontDestroyOnLoad (gameObject);
	}
	*/
	private float minPichScale = 0.95f;
	private float maxPichScale = 1.05f;

	private void init(){
		//musicSourse = new AudioSource ();
		//sfxSourse = new AudioSource ();
	}

	void OnDestroy(){

		stopAllSounds ();
	}

	public void stopAllSounds(){

		musicSource.Stop ();
		speechSource.Stop ();
		sfxSource.Stop ();
	}

	// Sfx-------------------------------------------
	public void stopSfx(){

		sfxSource.Stop ();
	}
	public void playSingleSfx( AudioClip clip ){

		sfxSource.clip = clip;
		sfxSource.Play ();
	}
	public void playShotSfx( AudioClip clip ){
		
		sfxSource.PlayOneShot (clip);
	}
	// Speech----------------------------------------
	public void stopSpeech(){
		
		speechSource.Stop ();
	}
	public void playSingleSpeech( AudioClip clip ){
		
		speechSource.clip = clip;
		speechSource.Play ();
	}
	// Music ----------------------------------------
	public void stopMusic(){
		
		musicSource.Stop ();
	}
	public void playSingleMusic( AudioClip clip ){
		
		musicSource.clip = clip;
		musicSource.Play ();
	}
	//-----------------------------------------------

	public void playSfxRandom( AudioClip[] clip, bool randomPich = false, float pitch = 1f ){

		int randIndex = Random.Range (0, clip.Length);
		sfxSource.clip = clip[randIndex];
		if (randomPich)
			sfxSource.pitch = Random.Range (minPichScale, maxPichScale);
		else
			sfxSource.pitch = pitch;
		//sfxSource.PlayOneShot (clip[randIndex]);
		sfxSource.Play ();
	}
	public void playSpeechRandom( AudioClip[] clip ){
		
		int randIndex = Random.Range (0, clip.Length);
		speechSource.clip = clip[randIndex];
		speechSource.pitch = Random.Range (minPichScale, maxPichScale);
		speechSource.Play ();
	}

}
