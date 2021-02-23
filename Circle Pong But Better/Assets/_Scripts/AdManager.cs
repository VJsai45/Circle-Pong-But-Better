
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Placement;

//public class AdManager : MonoBehaviour
//{
//    BannerAdGameObject bannerAd;
//    InterstitialAdGameObject interstitialAd;
//    RewardedAdGameObject rewardedAd;
//    public bool isHome,isReplay = false;


//    public static AdManager instance;
//    // Start is called before the first frame update
//    void Awake()
//    {
//        instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    void Start()
//    {
//        bannerAd = MobileAds.Instance.GetAd<BannerAdGameObject>("Banner Ad");
//        interstitialAd = MobileAds.Instance.GetAd<InterstitialAdGameObject>("Interstitial Ad");
//        rewardedAd = MobileAds.Instance.GetAd<RewardedAdGameObject>("Rewarded Ad");
  


//        MobileAds.Initialize(initStatus => {


//        });
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void LoadBannerAd()
//    {
//        bannerAd.LoadAd();
//    }
//    public void ShowBannerAd()
//    {
//        bannerAd.Show();
//    }
//    public void HideBannerAd()
//    {
       
//        if(bannerAd.BannerView != null)
//            bannerAd.Hide();
//    }
//    public void LoadInterstitialAd()
//    {
//        interstitialAd.LoadAd();
//        if(isHome)
//            StartCoroutine(HomeController.instance.replayScreenOpenCLose(false));
//        else if(isReplay)
//            StartCoroutine(HomeController.instance.continueAfterReplay());
//        isHome = false;
//        isReplay = false;

//    }
//    public void ShowInterstitialAd()
//    {
//        interstitialAd.ShowIfLoaded();
//    }
  
//    public void LoadRewardedAd()
//    {
//        rewardedAd.LoadAd();
//    }
//    public void ShowRewardedAd()
//    {
//        rewardedAd.ShowIfLoaded();
//    }

//    public bool IsInterstitialLoaded()
//    {
//        return interstitialAd.InterstitialAd.IsLoaded();
//    }

//    public bool IsRewardedAdLoaded()
//    {
//        return rewardedAd.RewardedAd.IsLoaded();
//    }
//}
