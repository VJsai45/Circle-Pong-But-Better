
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeController : MonoBehaviour
{
    public GameObject ball, animationHandler, platform, platformParent, score,score2, highScore, playButton, GameScreen, replayScreen,music,pause,
        modeChanger, playField,rippleEffect,continueTimer,spikeMode,classicBreatheMode,themeController,notification,
        rightScreenTutorial,leftScreenTutorial,tutorialScreen,themeOnePrompt,themeTwoPrompt;
    public Vector3 platformPositon;
    public Quaternion platformRotation;
    //AdManager adManager;
    private bool isRewardEarned,isContinued;
    public bool isGameOn;
    public int level = 0;
    public string[] highscoreStorage;
    int beforeContinueScore;
    public int currentMainScore;
    public int[] newlyUnlockedColors;
    public static HomeController instance;
    int tutCounter = 0;
    

    void Awake()
    {


        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        //adManager = FindObjectOfType<AdManager>();
        int scoreValue = checkAndSetHighscore();
        highScore.GetComponent<TextMeshPro>().text =  "Highscore : "+scoreValue.ToString();
        platformPositon = platform.transform.position;
        platformRotation = platform.transform.rotation;
        GameScreen.SetActive(false);

        PlayerPrefs.SetInt("firstTimePrompt0a", 1);
        PlayerPrefs.SetInt("firstTimePrompt1a", 1);
        PlayerPrefs.SetInt("firstTimeTutorial", 1);

    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Debug.Log("back pressed");
                if (FindObjectOfType<ThemeShopController>().isThemeShopOpen)
                   StartCoroutine(FindObjectOfType<ThemeShopController>().closeThemeShop());
            }
        }
     }


        public IEnumerator openHomeGameScreen(bool isPlayPressed)
    {
        if (isPlayPressed)
        {
            //adManager.HideBannerAd();
            GameScreen.SetActive(true);
            setGameMode();
            animationHandler.GetComponent<Animator>().SetBool("isPlayButtonPressed", true);
            //Debug.Log("play but");
            yield return new WaitForSeconds(0.5f);
           
            setGameScreenComponents();
            animationHandler.GetComponent<Animator>().SetBool("isPlayButtonPressed", false);
           

        }
        else
        {
         
            GameScreen.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            animationHandler.GetComponent<Animator>().SetBool("isBackButtonPressed", false); 
            setHomeScreenComponents();
            //Debug.Log("not play but");
            //adManager.ShowBannerAd();

        }
    }

    public int checkAndSetHighscore()
    {
        int highscoreValue = 0;
        switch (level)
        {
         case 0:
           {
             highscoreValue = PlayerPrefs.GetInt("highscore", 0);
             break;
           }
          case 1:
           {
               highscoreValue = PlayerPrefs.GetInt("breatheHighscore", 0);
               break;
           }
           case 2:
           {
               highscoreValue = PlayerPrefs.GetInt("spikesHighscore", 0);
               break;
           }
        }
        return highscoreValue;
    }




    void setGameScreenComponents()
    {

        pause.GetComponent<BoxCollider2D>().enabled = true;
        ball.GetComponent<CircleCollider2D>().enabled = true;
        ball.GetComponent<Ball>().enabled = true;
        ball.GetComponent<Ball>().isOut = false;
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
        platform.GetComponent<PlatformControl>().enabled = true;
        ball.GetComponent<SpriteRenderer>().color = new Color(ball.GetComponent<SpriteRenderer>().color.r, ball.GetComponent<SpriteRenderer>().color.g, ball.GetComponent<SpriteRenderer>().color.b, 1);

        //platform.GetComponent<PlatformControl>().isGameOn = true;
        isGameOn = true;
        if (PlayerPrefs.GetInt("firstTimeTutorial", 1) == 1)
        {
            setTutorialMode();
        }

    }

    void setTutorialMode()
    {
        ball.GetComponent<Ball>().enabled = false;
        tutorialScreen.SetActive(true);
        StartCoroutine(animateTutorialScreen());
        StartCoroutine(animateTutorialText());
    }
    IEnumerator animateTutorialText()  /// Animate Text Up and down
    {
        Vector3 start = new Vector3(0, 3.725f, 0);
        Vector3 end = new Vector3(0, 2, 0);
        while (Vector3.Distance(tutorialScreen.GetComponentInChildren<TextMeshPro>().transform.localPosition, start) > 0.01f)
        {
            tutorialScreen.GetComponentInChildren<TextMeshPro>().transform.localPosition = Vector3.Lerp(tutorialScreen.GetComponentInChildren<TextMeshPro>().transform.localPosition, start, 0.1f);
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        while (Vector3.Distance(tutorialScreen.GetComponentInChildren<TextMeshPro>().transform.localPosition, end) > 0.01f)
        {
            tutorialScreen.GetComponentInChildren<TextMeshPro>().transform.localPosition = Vector3.Lerp(tutorialScreen.GetComponentInChildren<TextMeshPro>().transform.localPosition, end, 0.1f);
            yield return null;
        }
    }

    IEnumerator fadeTutorialScreen()
    {
        
        Vector3 end = new Vector3(0, 0, 0);
        while (Vector3.Distance(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, end) > 0.01f)
        {
            rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale = Vector3.Lerp(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, end, 0.085f);
            leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale = Vector3.Lerp(leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, end, 0.085f);
            yield return null;
        }
        tutorialScreen.SetActive(false);
        ball.GetComponent<Ball>().enabled = true;
        PlayerPrefs.SetInt("firstTimeTutorial", 0);
    }

    IEnumerator animateTutorialScreen()
    {
       
        Color og = rightScreenTutorial.GetComponent<SpriteRenderer>().color;
        rightScreenTutorial.GetComponent<SpriteRenderer>().color = new Color(og.r, og.g, og.b, 1f);
        leftScreenTutorial.GetComponent<SpriteRenderer>().color = new Color(og.r, og.g, og.b, 1f);

        Vector3 start = new Vector3(0.75f, 0.75f, 0.75f);
        Vector3 end = new Vector3(1f, 1f, 1f);
        Color colorStart = new Color(og.r, og.g, og.b, 1f);
        Color colorEnd = new Color(og.r, og.g, og.b, 0);
 
        while (Vector3.Distance(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, end) > 0.01f)
        {
            rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale = Vector3.Lerp(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, end, 0.085f);
            leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale = Vector3.Lerp(leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, end, 0.085f);
            rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale = Vector3.Lerp(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale, end * 2f, 0.085f);
            leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale = Vector3.Lerp(leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale, end * 2f, 0.085f);
            rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color = Color.Lerp(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color, colorEnd, 0.1f);
            leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color = Color.Lerp(leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color, colorEnd, 0.1f);

            yield return null;
        }

        rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale = Vector3.zero;
        leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].transform.localScale = Vector3.zero;

        rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color = colorStart;
        leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[1].GetComponent<SpriteRenderer>().color = colorStart;

        while (Vector3.Distance(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, start) > 0.05f)
        {
            rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale = Vector3.Lerp(rightScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, start, 0.085f);
            leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale = Vector3.Lerp(leftScreenTutorial.GetComponentsInChildren<SpriteRenderer>()[0].transform.localScale, start, 0.085f);
            yield return null;
        }
        tutCounter++;
        if (tutCounter < 4)
            StartCoroutine(animateTutorialScreen());
        else
            StartCoroutine(fadeTutorialScreen());
    }

    void setHomeScreenComponents()
    {
        currentMainScore = 0;
        score.GetComponent<TextMeshPro>().text = 0.ToString();
        playButton.GetComponent<PlayButton>().isPressed = false;
        ball.GetComponent<CircleCollider2D>().enabled = false;
        ball.GetComponent<Ball>().ballDestination = new Vector3(0, -10, 0);
        ball.GetComponent<Ball>().previousCollison = Vector3.zero;
        ball.GetComponent<Rigidbody2D>().isKinematic = true;
        ball.GetComponent<Ball>().enabled = false;
        ball.transform.position = new Vector3(0,0,-3);
        ball.GetComponent<SpriteRenderer>().color = new Color(ball.GetComponent<SpriteRenderer>().color.r, ball.GetComponent<SpriteRenderer>().color.g, ball.GetComponent<SpriteRenderer>().color.b, 1);
        platformParent.transform.rotation = Quaternion.identity;
        platform.transform.position = platformPositon;
        platform.transform.rotation = platformRotation;
        playField.transform.localScale = new Vector3(3, 3, 1);
        pause.GetComponent<BoxCollider2D>().enabled = false;

        platform.GetComponent<PlatformControl>().enabled = false;
        if (PlayerPrefs.GetInt("firstTime", 1) == 1)
        {
            PlayerPrefs.SetInt("firstTime", 0);
            StartCoroutine(modeChanger.GetComponent<changeMode>().modeChangePrompt());
        }
        platform.GetComponent<PlatformControl>().StopAllCoroutines();

        music.GetComponentInChildren<MusicController>().updateMusicState();
    }

    public IEnumerator replayScreenOpenCLose(bool replayScreenAction)
    {
        checkUnlockTheme();
        if (replayScreenAction)
        {
            //platform.GetComponent<PlatformControl>().isGameOn = false;
            pause.GetComponent<BoxCollider2D>().enabled = false;
            isGameOn = false;
            //adManager.GetComponent<AdManager>().HideBannerAd();
            replayScreen.SetActive(true);
            string replayText = "";

            if (level == 0 && PlayerPrefs.GetInt("firstTimePrompt0a", 1) == 1)
            {
                PlayerPrefs.SetInt("firstTimePrompt0a", 0);
                isContinued = false;
                beforeContinueScore = 0;
                int targetScore = 0;
                string theme1 = "Pineapple";
                string theme2 = "Blueberry";
                targetScore = 40;
                replayText = "Score " + targetScore+" & Unlock 2 themes" ;
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[2];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[3];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
           
            else if (level == 1 && PlayerPrefs.GetInt("firstTimePrompt1a", 1) == 1)
            {
                PlayerPrefs.SetInt("firstTimePrompt1a", 0);
                isContinued = false;
                beforeContinueScore = 0;
                string theme1 = "";
                string theme2 = "";
                int targetScore = 0;
                theme1 = "Luna";
                theme2 = "Eclipse";
                targetScore = 50;
                replayText = "Score " + targetScore + " & Unlock 2 themes";
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[4];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[5];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;


            }
            else if (level == 2 && PlayerPrefs.GetInt("firstTimePrompt2a", 1) == 1)
            {
                PlayerPrefs.SetInt("firstTimePrompt2a", 0);
                isContinued = false;
                beforeContinueScore = 0;
                string theme1 = "";
                string theme2 = "";
                int targetScore = 0;
                theme1 = "Amethyst";
                theme2 = "Moonstone";
                targetScore = 60;
                replayText = "Score " + targetScore + " & Unlock 2 themes";
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[6];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[7];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;

            }
            else if (level == 0 && ((PlayerPrefs.GetInt("themename3") == 0 && currentMainScore >= 30) || (PlayerPrefs.GetInt("themename9") == 0 && currentMainScore >= 90)))
            {
                isContinued = false;
                beforeContinueScore = 0;
                string theme1;
                string theme2;
                int theme1Number, theme2Number;
                int difference = 0;
                if (currentMainScore <= 40)
                {
                    theme1 = "Pineapple";
                    theme2 = "Blueberry";
                    difference = 40 - currentMainScore;
                    theme1Number = 2;
                    theme2Number = 3;
                }
                else
                {
                    theme1 = "Marhsmallow";
                    theme2 = "Chocolate";
                    difference = 100 - currentMainScore;
                    theme1Number = 8;
                    theme2Number = 9;
                }
                replayText = "Score " + difference.ToString() + " more to unlock";
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[theme1Number];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[theme2Number];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;

            }
            else if (level == 0 && PlayerPrefs.GetInt("themename3") == 1 && PlayerPrefs.GetInt("firstTimePrompt0b", 1) == 1)
            {
                PlayerPrefs.SetInt("firstTimePrompt0b", 0);
                isContinued = false;
                beforeContinueScore = 0;
                string theme1 = "";
                string theme2 = "";
                int targetScore = 0;
                theme1 = "Marshmallow";
                theme2 = "Chocolate";
                targetScore = 100;
                replayText = "Unlock them after scoring "+targetScore;
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[8];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[9];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
            else if (level == 1 && ((PlayerPrefs.GetInt("themename5") == 0 && currentMainScore >= 40) || (PlayerPrefs.GetInt("themename11") == 0 && currentMainScore >= 90)))
            {
                isContinued = false;
                beforeContinueScore = 0;
                string theme1;
                string theme2;
                int difference = 0;
                int theme1Number, theme2Number;
                if (currentMainScore <= 50)
                {
                    theme1 = "Luna"; // to be changed
                    theme2 = "Eclipse";
                    difference = 50 - currentMainScore;
                    theme1Number = 4;
                    theme2Number = 5;
                }
                else
                {
                    theme1 = "Tangy Orange";
                    theme2 = "Matcha";
                    difference = 100 - currentMainScore;
                    theme1Number = 10;
                    theme2Number = 11;

                }
                replayText = "Score "+ difference.ToString() + " more to unlock ";
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[theme1Number];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[theme2Number];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;

            }
            else if (level == 1 && PlayerPrefs.GetInt("themename5") == 1 && PlayerPrefs.GetInt("firstTimePrompt1b", 1) == 1)
            {
                PlayerPrefs.SetInt("firstTimePrompt1b", 0);
                isContinued = false;
                beforeContinueScore = 0;
                string theme1 = "";
                string theme2 = "";
                int targetScore = 0;
                theme1 = "Tangy Orange";
                theme2 = "Matcha";
                targetScore = 100;
                replayText = "Unlock them after scoring " + targetScore;
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[10];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[11];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
            else if (level == 2 && ((PlayerPrefs.GetInt("themename7") == 0 && currentMainScore >= 50) || (PlayerPrefs.GetInt("themename13") == 0 && currentMainScore >= 90)))
            {
                isContinued = false;
                beforeContinueScore = 0;
                string theme1;
                string theme2;
                int difference = 0;
                int theme1Number, theme2Number;
                if (currentMainScore <= 60)
                {
                    theme1 = "Amethyst"; // to be changed
                    theme2 = "Moonstone";
                    difference = 60 - currentMainScore;
                    theme1Number = 6;
                    theme2Number = 7;
                }
                else
                {
                    theme1 = "Blue Lagoon";
                    theme2 = "Berry Smoothie";
                    difference = 100 - currentMainScore;
                    theme1Number = 12;
                    theme2Number = 13;
                }
                replayText = "Score " + difference.ToString() + " more to unlock";
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[theme1Number];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[theme2Number];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;

            }
            else if (level == 2 && PlayerPrefs.GetInt("themename7") == 1 && PlayerPrefs.GetInt("firstTimePrompt2b", 1) == 1)
            {
                PlayerPrefs.SetInt("firstTimePrompt2b", 0);
                isContinued = false;
                beforeContinueScore = 0;
                string theme1 = "";
                string theme2 = "";
                int targetScore = 0;
                theme1 = "Blue Lagoon";
                theme2 = "Berry Smoothie";
                targetScore = 100;
                replayText = "Unlock them after scoring " + targetScore;
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 4.85f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 3.75f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, 0f, 0);
                themeOnePrompt.SetActive(true);
                themeTwoPrompt.SetActive(true);
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().text = theme1;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().text = theme2;
                themeOnePrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[12];
                themeTwoPrompt.GetComponentInChildren<SpriteRenderer>().color = themeController.GetComponent<ThemeController>().themeColors[13];
                themeOnePrompt.GetComponentInChildren<TextMeshPro>().color = Color.black;
                themeTwoPrompt.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
            else if (isContinued && currentMainScore >= beforeContinueScore)
            {
                isContinued = false;
                beforeContinueScore = 0;
                replayText = "Bravo! now we challenge you to score 800" + ((currentMainScore / 10 + 2) * 10).ToString() ;
                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 0.95f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, -0.93f, 0);

            }
            else if (currentMainScore > 70)
            {
                if (Random.Range(0, 2) == 1)
                {
                    replayText = "Look at that score DAMN!. Continue to surprise us";
                    replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
                    replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 0.95f, 0);
                    replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, -0.93f, 0);
                }
                else
                {
                    replayText = "Are you truly human? 'cause your score is deceiving, keep deceiving us";
                    replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 1.7f, 0);
                    replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 1.35f, 0);
                    replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, -0.93f, 0);

                }
            }
            else
            {
                if (((currentMainScore / 10 + 2) * 10 - currentMainScore ) > 15 )
                    replayText = "Let's see if you can score " + ((currentMainScore / 10 + 2) * 10).ToString() ;
                else
                    replayText = "You are so close to scoring " + ((currentMainScore / 10 + 2) * 10).ToString() ;

                replayScreen.GetComponentsInChildren<TextMeshPro>()[0].gameObject.transform.localPosition = new Vector3(0, 1.34f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[5].gameObject.transform.localPosition = new Vector3(0, 0.6f, 0);
                replayScreen.GetComponentsInChildren<SpriteRenderer>()[6].gameObject.transform.localPosition = new Vector3(0, -0.93f, 0);
            }
             

            replayScreen.GetComponentsInChildren<TextMeshPro>()[0].text = replayText;
            replayScreen.GetComponentsInChildren<TextMeshPro>()[1].text = "Current Score : " + currentMainScore;
            animationHandler.GetComponent<Animator>().SetBool("isPlayerOut", true);
            yield return new WaitForSeconds(0.35f);
            animationHandler.GetComponent<Animator>().SetBool("isPlayerOut", false);
            Debug.Log("is active " + themeOnePrompt.activeSelf);
            if(themeOnePrompt.activeSelf)
                StartCoroutine(animateCards());
            int highscoreValue = checkAndSetHighscore();
            Leaderboard.instance.AddScoreToLeaderBoard(level, currentMainScore);
            if (int.Parse(score.GetComponent<TextMeshPro>().text) > highscoreValue)
            {
                PlayerPrefs.SetInt(highscoreStorage[level], currentMainScore);
                highScore.GetComponent<TextMeshPro>().text = "Highscore : "+currentMainScore.ToString();
            }
            
        }
        else
        {
            animationHandler.GetComponent<Animator>().SetBool("missOutReward", true);
            if (level == 2)
            {
                yield return new WaitForSeconds(0.3f);
                spikeMode.SetActive(false);
            }
            yield return new WaitForSeconds(0.35f);
            StartCoroutine(openHomeGameScreen(false));
            animationHandler.GetComponent<Animator>().SetBool("missOutReward", false);
            themeOnePrompt.SetActive(false);
            themeTwoPrompt.SetActive(false);
            themeOnePrompt.transform.localPosition = new Vector3(0, 1.5f, 0);
            themeTwoPrompt.transform.localPosition =  new Vector3(0, 1.5f, 0);
            themeOnePrompt.transform.localRotation = Quaternion.identity;
            themeTwoPrompt.transform.localRotation = Quaternion.identity;
            replayScreen.SetActive(false);
        }
        
    }


    public IEnumerator animateCards()
    {
        Quaternion theme1RotationEnd = Quaternion.Euler(0, 0, 15);
        Quaternion theme2RotationEnd = Quaternion.Euler(0, 0, -20);
        Vector3 theme2PositionEnd = new Vector3(0.25f, 2.1f, 0);
        while (Quaternion.Angle(themeTwoPrompt.transform.localRotation, theme2RotationEnd) > 0.01f)
        {
           themeOnePrompt.transform.localRotation =  Quaternion.Lerp(themeOnePrompt.transform.localRotation, theme1RotationEnd, 0.05f);
           themeTwoPrompt.transform.localRotation = Quaternion.Lerp(themeTwoPrompt.transform.localRotation, theme2RotationEnd, 0.05f);
           themeTwoPrompt.transform.localPosition = Vector3.Lerp(themeTwoPrompt.transform.localPosition, theme2PositionEnd, 0.1f);
           yield return null;
        }

        
    }

    public void continueAfterReward()
    {
        isRewardEarned = true;
        isContinued = true;
        ball.GetComponent<SpriteRenderer>().color = new Color(ball.GetComponent<SpriteRenderer>().color.r, ball.GetComponent<SpriteRenderer>().color.g, ball.GetComponent<SpriteRenderer>().color.b, 1);
        // Debug.Log("reward earned");
    }

    public void rewardClosed()
    {
        //adManager.LoadRewardedAd();
        if (isRewardEarned)
        {
            StartCoroutine(continueAnimation());
            isRewardEarned = false;
        }
        else
        {
            replayScreen.SetActive(true);
            //StartCoroutine(replayScreenOpenCLose(false));
        }
        themeOnePrompt.SetActive(false);
        themeTwoPrompt.SetActive(false);
        themeOnePrompt.transform.localPosition = new Vector3(0, 1.5f, 0);
        themeTwoPrompt.transform.localPosition =  new Vector3(0, 1.5f, 0);
        themeOnePrompt.transform.localRotation = Quaternion.identity;
        themeTwoPrompt.transform.localRotation = Quaternion.identity;
        //Debug.Log("reward closed");
    }

    IEnumerator continueAnimation()
    {
        GetComponent<Animator>().SetBool("replayGame", true);
        beforeContinueScore = (currentMainScore / 10 + 2)*10;
        yield return new WaitForSeconds(0.2f);
        var ballResetDest = new Vector3(0, 0, -3);
        while (Vector3.Distance(ball.transform.position, ballResetDest) > 0.01f)
        {
            ball.transform.position = Vector3.Lerp(ball.transform.position, ballResetDest, 0.2f);
            yield return null;
        }
        GetComponent<Animator>().SetBool("replayGame", false);
        while (Quaternion.Angle(platformParent.transform.rotation, Quaternion.identity) > 0.01f)
        {
            platformParent.transform.rotation = Quaternion.Lerp(platformParent.transform.rotation, Quaternion.identity, 0.2f);
            yield return null;
        }
        //platform.GetComponent<PlatformControl>().isGameOn = true;
        themeOnePrompt.SetActive(false);
        themeTwoPrompt.SetActive(false);
        themeOnePrompt.transform.localPosition = new Vector3(0, 1.5f, 0);
        themeTwoPrompt.transform.localPosition =  new Vector3(0, 1.5f, 0);
        themeOnePrompt.transform.localRotation = Quaternion.identity;
        themeTwoPrompt.transform.localRotation = Quaternion.identity;
        isGameOn = true;
        continueTimer.SetActive(false);
        ball.GetComponent<Ball>().enabled = true;
        ball.GetComponent<Rigidbody2D>().isKinematic = false;
        ball.GetComponent<Ball>().score2.text = (currentMainScore + 1).ToString();
        ball.GetComponent<Ball>().previousCollison = Vector3.zero;
        ball.GetComponent<CircleCollider2D>().enabled = true;
        ball.GetComponent<Ball>().isOut = false;
  
    }

    public IEnumerator continueAfterReplay()
    {
        pause.GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Animator>().SetBool("replayGame", true);
        currentMainScore = 0;
        score.GetComponent<TextMeshPro>().text = 0.ToString();
        score2.GetComponent<TextMeshPro>().text = 1.ToString();
        if (level == 2)
        {
            SpikeModeController.instance.StopAllCoroutines();
            StartCoroutine(SpikeModeController.instance.createRing());
            ball.GetComponent<SpriteRenderer>().color = new Color(ball.GetComponent<SpriteRenderer>().color.r, ball.GetComponent<SpriteRenderer>().color.g, ball.GetComponent<SpriteRenderer>().color.b, 1);
        }
        var ballResetDest = new Vector3(0, 0, -3);
        while (Vector3.Distance(ball.transform.position, ballResetDest) > 0.01f)
        {
            ball.transform.position = Vector3.Lerp(ball.transform.position, ballResetDest,0.2f);
            yield return null;
        }
        GetComponent<Animator>().SetBool("replayGame", false);
        while (Quaternion.Angle(platformParent.transform.rotation, Quaternion.identity) > 0.01f)
        {
            platformParent.transform.rotation = Quaternion.Lerp(platformParent.transform.rotation, Quaternion.identity, 0.2f);
            yield return null;
        }
        themeOnePrompt.SetActive(false);
        themeTwoPrompt.SetActive(false);
        themeOnePrompt.transform.localPosition = new Vector3(0,1.5f,0); 
        themeTwoPrompt.transform.localPosition =  new Vector3(0, 1.5f, 0);
        themeOnePrompt.transform.localRotation = Quaternion.identity;
        themeTwoPrompt.transform.localRotation = Quaternion.identity;
        StartCoroutine(continueAnimation());
    }

    public IEnumerator bounceRipple()
    {
        rippleEffect.SetActive(true);
        rippleEffect.transform.localScale = Vector3.one;
        Vector3 end = playField.transform.localScale *0.5f;
        Color color = playField.GetComponent<SpriteRenderer>().color;
        rippleEffect.GetComponent<SpriteRenderer>().color = color;
        Color colorEnd = new Color(color.r, color.g, color.b, 0);
        while (Vector3.Distance(rippleEffect.transform.localScale, end) > 0.1f)
        {
            rippleEffect.transform.localScale = Vector3.Lerp(rippleEffect.transform.localScale, end, 0.05f);
            rippleEffect.GetComponent<SpriteRenderer>().color = Color.Lerp(rippleEffect.GetComponent<SpriteRenderer>().color, colorEnd, 0.1f);
            yield return null;
        }
        rippleEffect.SetActive(false);
    }




    public void SetContinueGameMode()
    {
        replayScreen.SetActive(false);
        ball.transform.position = new Vector3(0,0,-3); 
        platformParent.transform.rotation = Quaternion.identity;
        platform.transform.position = platformPositon;
        platform.transform.rotation = platformRotation;
    }


    void setGameMode()
    {
        switch (level)
        {
            case 0:
                {
                    classicBreatheMode.SetActive(true);
                    spikeMode.SetActive(false);
                    platform.GetComponent<PlatformControl>().StopAllCoroutines();
                    break;
                }
            case 1:
                {
                    spikeMode.SetActive(false);
                    classicBreatheMode.SetActive(true);
                    StartCoroutine(platform.GetComponent<PlatformControl>().changePlaygroundBig());
                    break;
                }
            case 2:
                {
                    classicBreatheMode.SetActive(false);
                    platform.GetComponent<PlatformControl>().StopAllCoroutines();
                    spikeMode.SetActive(true);
                    GetComponentInChildren<SpikeModeController>().enabled = true;
                    break;
                }
        }
   
    }


    void checkUnlockTheme()
    {

            

        switch(level)
        {
            case 0:
                {
                    Debug.Log("case 0");

                    if (currentMainScore >= 40 && PlayerPrefs.GetInt("themename2", 0) == 0)
                    {
                        PlayerPrefs.SetInt("themename2", 1);
                        PlayerPrefs.SetInt("themename3", 1);
                        setThemeNotification();

                        Debug.Log("case 0 inside");
                    }
                    if (currentMainScore >= 100 && PlayerPrefs.GetInt("themename8", 0) == 0)
                    {
                        PlayerPrefs.SetInt("themename8", 1);
                        PlayerPrefs.SetInt("themename9", 1);
                        setThemeNotification();

                    }
                    break;
                }
            case 1:
                {
                    if (currentMainScore >= 50 && PlayerPrefs.GetInt("themename4", 0) == 0)
                    {
                        PlayerPrefs.SetInt("themename4", 1);
                        PlayerPrefs.SetInt("themename5", 1);
                        setThemeNotification();

                    }
                    if (currentMainScore >= 100 && PlayerPrefs.GetInt("themename10", 0) == 0)
                    {
                        PlayerPrefs.SetInt("themename10", 1);
                        PlayerPrefs.SetInt("themename11", 1);
                        setThemeNotification();

                    }
                    break;
                }
            case 2:
                {
                    if (currentMainScore >= 60 && PlayerPrefs.GetInt("themename6", 0) == 0)
                    {
                        PlayerPrefs.SetInt("themename6", 1);
                        PlayerPrefs.SetInt("themename7", 1);
                        setThemeNotification();
                    }
                    if (currentMainScore >= 100 && PlayerPrefs.GetInt("themename12", 0) == 0)
                    {
                        PlayerPrefs.SetInt("themename12", 1);
                        PlayerPrefs.SetInt("themename13", 1);
                        setThemeNotification();
                    }
                    break;
                }
                
        }
        themeController.GetComponent<ThemeController>().getUnlockedThemes();
    }

    void setThemeNotification()
    {
        notification.SetActive(true);
        notification.GetComponent<SpriteRenderer>().color = themeController.GetComponentsInChildren<SpriteRenderer>()[1].color;
        notification.GetComponentsInChildren<SpriteRenderer>()[1].color = themeController.GetComponentsInChildren<SpriteRenderer>()[1].color;
        notification.GetComponentInChildren<TextMeshPro>().color = new Color(themeController.GetComponentsInChildren<SpriteRenderer>()[0].color.r, themeController.GetComponentsInChildren<SpriteRenderer>()[0].color.g, themeController.GetComponentsInChildren<SpriteRenderer>()[0].color.b,1);
        notification.GetComponentInChildren<TextMeshPro>().text = (int.Parse(notification.GetComponentInChildren<TextMeshPro>().text) + 2).ToString();
        FindObjectOfType<ThemeShopIcon>().StopAllCoroutines();
        StartCoroutine(FindObjectOfType<ThemeShopIcon>().pulsateNotification());
    }
}
