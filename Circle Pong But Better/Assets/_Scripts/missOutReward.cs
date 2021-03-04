using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;

public class missOutReward : MonoBehaviour
{

    public int counter;

    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        counter++;
        var adLoaded = AdManager.instance.IsInterstitialLoaded();
        if (counter == 2 && adLoaded)
        {
            AdManager.instance.isHome = true;
            AdManager.instance.ShowInterstitialAd();
            counter = 0;
        }
        else
        {
            GoToHome();
        }

    }

    public void GoToHome()
    {
        StartCoroutine(HomeController.instance.replayScreenOpenCLose(false));
    }
}
