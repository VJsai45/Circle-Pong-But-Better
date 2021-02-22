using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameScreen : MonoBehaviour
{
    public GameObject highlight;
    public float speed;
    private bool isTransitioning;

    void Start()
    {
        updateMusicSlider();
    }

    void OnMouseDown()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            float x = highlight.transform.localPosition.x;
            x *= -1;
            var dest = new Vector3(x, highlight.transform.localPosition.y, highlight.transform.localPosition.z);
            StartCoroutine(slideMusicButton(dest));
            if (PlayerPrefs.GetInt("music") == 1)
            {
                PlayerPrefs.SetInt("music", 0);
                MusicController.instance.updateMusicState();
            }
            else
            {
                PlayerPrefs.SetInt("music", 1);
                MusicController.instance.updateMusicState();
            }
        }
    }

    IEnumerator slideMusicButton(Vector3 dest)
    {

        while (Vector3.Distance(highlight.transform.localPosition ,dest) > 0.001f)
        {
            highlight.transform.localPosition = Vector3.Lerp(highlight.transform.localPosition,dest,speed*0.1f);
            yield return null;
        }
        isTransitioning = false;
    }

    public void updateMusicSlider()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            highlight.transform.localPosition += new Vector3(-highlight.transform.localPosition.x - 0.35f, 0, 0);
        }
        else
            highlight.transform.localPosition += new Vector3(-highlight.transform.localPosition.x + 0.35f, 0, 0);
    }


}
