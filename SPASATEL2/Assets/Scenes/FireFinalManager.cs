using UnityEngine;
using System.Collections;

public class FireFinalManager : MonoBehaviour 
{


    public AudioSource[] winx;

    void Start()
    {
        StartCoroutine(win());
    }

    IEnumerator win()
    {
        yield return new WaitForSeconds(1);
        StarsPanel.show(3);
        yield return new WaitForSeconds(4);
        winx[Random.Range(0, winx.Length)].Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Loader.LEVEL = "game"; Application.LoadLevel("loader"); }
    }

    public void Exit()
    {
        Loader.LEVEL = "game";
        Application.LoadLevel("loader");
        StarsPanel.hide();
    }

    public void Repeat()
    {
        Loader.LEVEL = "fire_battle";
        Application.LoadLevel("loader");
        StarsPanel.hide();
    }

    void OnDestroy()
    {

        StarsPanel.hide();
    }


}
