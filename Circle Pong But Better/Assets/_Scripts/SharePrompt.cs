using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SharePrompt : MonoBehaviour
{
    public GameObject sprite, sprite2,share,shareScreen,overlay;
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
        share.GetComponent<Share>().shareHighscoreAndSS();
        StartCoroutine(closeSharePrompt());
        PlayerPrefs.SetInt("isShared", 1);
    }

    public IEnumerator closeSharePrompt()
    {
        
        Vector3 end = new Vector3(0, -8f, 0);
        var color = new Color(0, 0, 0, 0);
        
        while (Vector3.Distance(shareScreen.transform.localPosition, end) > 0.1f)
        {
            shareScreen.transform.localPosition = Vector3.Lerp(shareScreen.transform.localPosition, end, 0.4f);
            overlay.GetComponent<SpriteRenderer>().color = Color.Lerp(overlay.GetComponent<SpriteRenderer>().color, color, 0.2f);
            yield return null;
        }
        yield return (StartCoroutine(HomeController.instance.openHomeGameScreen(true)));
        shareScreen.SetActive(false);
    }
}
