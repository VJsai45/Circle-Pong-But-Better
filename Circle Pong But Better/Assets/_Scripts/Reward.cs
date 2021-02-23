

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reward : MonoBehaviour
{
   
    public GameObject replaySprite,replaySprite2;
    // Start is called before the first frame update
    void Start()
    {
       
        float worldSpriteWidth = replaySprite.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;
        Vector3 newScale = replaySprite.transform.localScale;
        newScale.x = worldScreenWidth / worldSpriteWidth;
        newScale.y = worldScreenWidth / worldSpriteWidth;
        replaySprite.transform.localScale = newScale;
        worldSpriteWidth = replaySprite2.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;
        newScale = replaySprite2.transform.localScale;
        newScale.x = worldScreenWidth / worldSpriteWidth;
        newScale.y = worldScreenWidth / worldSpriteWidth;
        replaySprite2.transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //if (AdManager.instance.IsRewardedAdLoaded())
        //{
        //    AdManager.instance.ShowRewardedAd();
        //    not included//HomeController.instance.GetComponent<Animator>().SetBool("replayGame",true);
        //}
        //else
        //{
            StartCoroutine(HomeController.instance.replayScreenOpenCLose(false));
        //}

            
    }

}
