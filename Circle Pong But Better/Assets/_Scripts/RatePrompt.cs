using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatePrompt : MonoBehaviour
{
    public GameObject sprite, sprite2, rateScreen, overlay;
    // Start is called before the first frame update
    void Start()
    {
        float worldSpriteWidth = sprite.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;
        Vector3 newScale = sprite.transform.localScale;
        newScale.x = worldScreenWidth / worldSpriteWidth;
        newScale.y = worldScreenWidth / worldSpriteWidth;
        sprite.transform.localScale = newScale;
        worldSpriteWidth = sprite2.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;
        newScale = sprite2.transform.localScale;
        newScale.x = worldScreenWidth / worldSpriteWidth;
        newScale.y = worldScreenWidth / worldSpriteWidth;
        sprite2.transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.IdleMindsGames.CirclePongButBetter");
        StartCoroutine(closeRatePrompt());
        PlayerPrefs.SetInt("isRated", 1);
    }

    public IEnumerator closeRatePrompt()
    {

        Vector3 end = new Vector3(0, -8f, 0);
        var color = new Color(0, 0, 0, 0);
        while (Vector3.Distance(rateScreen.transform.localPosition, end) > 0.1f)
        {
            rateScreen.transform.localPosition = Vector3.Lerp(rateScreen.transform.localPosition, end, 0.4f);
            overlay.GetComponent<SpriteRenderer>().color = Color.Lerp(overlay.GetComponent<SpriteRenderer>().color, color, 0.2f);
            yield return null;
        }

        yield return(StartCoroutine(HomeController.instance.openHomeGameScreen(true)));
        rateScreen.SetActive(false);
    }
}
