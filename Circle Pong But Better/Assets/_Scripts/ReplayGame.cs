using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayGame : MonoBehaviour
{
    // Start is called before the first frame update
    public int counter = 0;
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
        if (counter == 3 && adLoaded)
        {
            AdManager.instance.isReplay = true;
            AdManager.instance.ShowInterstitialAd();
            counter = 0;
        }
        else
        {
            StartCoroutine(HomeController.instance.continueAfterReplay());
        }
        
        
        
      
    }
}
