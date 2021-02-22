using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    public AudioSource adSource;
    public AudioClip[] adClips;
    public GameObject inGameMusicSlider;


    void Awake()
    {


        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject turnOffLine,turnOffLine2;
    private bool isChanging;

    void Start()
    {
        updateMusicState();
        //StartCoroutine(playAudioSequentially());
    }

    public void Update()
    {

    }

    void OnMouseDown()
    {
        int isMusicOn = PlayerPrefs.GetInt("music");
        if(!isChanging)
        { if (isMusicOn == 1)
            {

                StartCoroutine(musicOnOff(0));
            }
            else
            {

                StartCoroutine(musicOnOff(1));
            }
        }
    }

    public IEnumerator musicOnOff(int on)
    {
        isChanging = true;
        if (on == 0)
        {
            PlayerPrefs.SetInt("music", 0);
            GetComponent<AudioSource>().volume=0;
            while (Vector3.Distance(turnOffLine.transform.localScale,new Vector3(1,1,1)) > 0.0001f)
            {
                turnOffLine.transform.localScale = Vector3.Lerp(turnOffLine.transform.localScale,new Vector3(1,1,1),0.5f);
                

                yield return null;
            }
            while (Vector3.Distance(turnOffLine2.transform.localScale, new Vector3(1, 1, 1)) > 0.0001f)
            {
                turnOffLine2.transform.localScale = Vector3.Lerp(turnOffLine2.transform.localScale, new Vector3(1, 1, 1), 0.5f);

                yield return null;
            }
        }
        else
        {
            PlayerPrefs.SetInt("music", 1);
            GetComponent<AudioSource>().volume=0.3f;
            while (Vector3.Distance(turnOffLine.transform.localScale, new Vector3(1, 0, 1)) > 0.0001f)
            {
                turnOffLine.transform.localScale  = Vector3.Lerp(turnOffLine.transform.localScale, new Vector3(1, 0, 1), 0.5f); ;
               

                yield return null;
            }
            while (Vector3.Distance(turnOffLine2.transform.localScale, new Vector3(1, 0, 1)) > 0.0001f)
            {

                turnOffLine2.transform.localScale = Vector3.Lerp(turnOffLine2.transform.localScale, new Vector3(1, 0, 1), 0.5f); ;
                yield return null;
            }
        }
        isChanging = false;

        inGameMusicSlider.GetComponent<MusicGameScreen>().updateMusicSlider();
    }

    public void updateMusicState()
    {
        if (PlayerPrefs.GetInt("music", 1) == 0)
        {
            PlayerPrefs.SetInt("music", 0);
            GetComponent<AudioSource>().volume = 0;
            turnOffLine.transform.localScale = Vector3.one;
            turnOffLine2.transform.localScale = Vector3.one;
            
        }
        else
        {
            PlayerPrefs.SetInt("music", 1);
            GetComponent<AudioSource>().volume = 0.3f;
            turnOffLine.transform.localScale = new Vector3(1, 0, 1);
            turnOffLine2.transform.localScale = new Vector3(1, 0, 1);
           
        }

    }


    //IEnumerator playAudioSequentially()
    //{
    //    yield return null;


    //    for (int i = 0; i < adClips.Length; i++)
    //    {
    //        adSource.clip = adClips[i];
    //        adSource.Play();
    //        while (adSource.isPlaying)
    //        {
    //            yield return null;
    //        }

    //    }
    //    StartCoroutine(playAudioSequentially());
    //}





}
