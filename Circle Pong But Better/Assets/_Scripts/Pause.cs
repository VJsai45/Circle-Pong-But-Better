using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    private bool paused;
    public Sprite play,pause;
    public GameObject playbg;
    private Color color;
    Vector3 startSize;

    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
        startSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        
            if (HomeController.instance.isGameOn)
            {            
                GetComponent<SpriteRenderer>().sprite = play;
                Time.timeScale = 0;
                StartCoroutine(playPauseAnimation());
                HomeController.instance.isGameOn = false;
            }
            else
            {
                HomeController.instance.isGameOn = true;
                StopAllCoroutines();
                playbg.transform.localScale = Vector3.zero;
                playbg.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
                transform.localScale = startSize;
                GetComponent<SpriteRenderer>().sprite = pause;
                Time.timeScale = 1;
            }
        

    }

    IEnumerator playPauseAnimation()
    {
        
        var start = new Vector3(1.75f,1.75f,1);
        var colorStart = GetComponent<SpriteRenderer>().color;
        var colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0);
        var end = new Vector3(2.25f, 2.25f, 1);
        while (Vector3.Distance(transform.localScale, end) > 0.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, end, 0.085f);
            playbg.transform.localScale = Vector3.Lerp(playbg.transform.localScale, end * 2, 0.085f);
            playbg.GetComponent<SpriteRenderer>().color = Color.Lerp(playbg.GetComponent<SpriteRenderer>().color, colorEnd, 0.1f);
            yield return null;
        }
        playbg.transform.localScale = Vector3.zero;
        playbg.GetComponent<SpriteRenderer>().color = colorStart;
        while (Vector3.Distance(transform.localScale, start) > 0.05f)
        {
           transform.localScale = Vector3.Lerp(transform.localScale, start, 0.085f);
            yield return null;
        }
        StartCoroutine(playPauseAnimation());
    }

    
}
