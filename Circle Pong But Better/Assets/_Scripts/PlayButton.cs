using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayButton : MonoBehaviour
{
    public GameObject ring,ringBG,changeMode,shareScreen,rateScreen,overlayShare,overlayRate;
    public  bool isPressed;
    public int speed;
    public string[] shareTexts,rateTexts;

    void Update()
    {
        ring.transform.Rotate(Time.deltaTime * Vector3.forward * speed * 10);
        ringBG.transform.Rotate(Time.deltaTime * Vector3.forward * speed * 10);
    }


    void OnMouseDown()
    {
        int count = PlayerPrefs.GetInt("prompt", 0);
        count++;
        PlayerPrefs.SetInt("prompt", count);

        var theme = HomeController.instance.gameObject.GetComponentInChildren<ThemeController>().isThemeChanging;
        if (!isPressed && !theme)
        {
            isPressed = true;
           
            if (PlayerPrefs.GetInt("prompt") % 3 == 0 && PlayerPrefs.GetInt("prompt") % 6 != 0 && PlayerPrefs.GetInt("isShared", 0) == 0)
            {
                isPressed = true;
                StartCoroutine(openSharePrompt());
                return;
            }

            if (PlayerPrefs.GetInt("prompt") % 6 == 0 && PlayerPrefs.GetInt("isRated", 0) == 0)
            {
                isPressed = true;
                StartCoroutine(openRatePrompt());
                return;
            }
            Debug.Log("play inside");
            changeMode.GetComponent<changeMode>().resetArrow();
            StartCoroutine(HomeController.instance.openHomeGameScreen(true));
        }

       

    }

    IEnumerator openSharePrompt()
    {
        //AdManager.instance.HideBannerAd();
        shareScreen.SetActive(true);
        shareScreen.GetComponentsInChildren<TextMeshPro>()[0].text = shareTexts[Random.Range(0, 2)];
        shareScreen.GetComponentsInChildren<TextMeshPro>()[1].color = Color.white;
        Vector3 end = new Vector3(0, -3.3f, 0);
        var color = new Color(0, 0, 0, 0.6f);
        while (Vector3.Distance(shareScreen.transform.localPosition, end) > 0.001f)
        {
            shareScreen.transform.localPosition = Vector3.Lerp(shareScreen.transform.localPosition, end, 0.2f);
            overlayShare.GetComponent<SpriteRenderer>().color = Color.Lerp(overlayShare.GetComponent<SpriteRenderer>().color, color, 0.2f);
            yield return null;
        }
    }

    IEnumerator openRatePrompt()
    {
       // AdManager.instance.HideBannerAd();
        rateScreen.SetActive(true);
        rateScreen.GetComponentsInChildren<TextMeshPro>()[0].text = rateTexts[Random.Range(0, 3)];
        rateScreen.GetComponentsInChildren<TextMeshPro>()[1].color = Color.white;
        Vector3 end = new Vector3(0, -3.3f, 0);
        var color = new Color(0, 0, 0, 0.6f);
        while (Vector3.Distance(rateScreen.transform.localPosition, end) > 0.001f)
        {
            rateScreen.transform.localPosition = Vector3.Lerp(rateScreen.transform.localPosition, end, 0.2f);
            overlayRate.GetComponent<SpriteRenderer>().color = Color.Lerp(overlayRate.GetComponent<SpriteRenderer>().color, color, 0.2f);
            yield return null;
        }
    }

}
