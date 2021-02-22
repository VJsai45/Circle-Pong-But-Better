using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeController : MonoBehaviour
{
    // Start is called before the first frame update

    public Color[] bgColors,themeColors,darkThemeColors,playfieldColors;
    public Color[] bgColorsUnlocked, themeColorsUnlocked, darkThemeColorsUnlocked, playfieldColorsUnlocked;
    public int counter;
    public GameObject gameScreen,playField,rippleEffect,replayScreen, upcover, downcover,score2,themeColorFill,spikeScreen,defaultScreen,spikeModePad;
    GameObject[] darkObjects, darkConflictObjects, themeObjects, themeConflictObjects;
    TextMeshPro[] texts;
    public  float speed;
    public bool isThemeChanging, isFirstTime;
    public int[] unlockedThemes;

    void Start()
    {
        //PlayerPrefs.SetInt("firsttimetheme", 1);
        counter = PlayerPrefs.GetInt("theme", 0); 
        getUnlockedThemes();
        counter = getActualCounterForThemeDrop(counter);
        firstThemeChange(counter);
    
    }

    public void getUnlockedThemes()
    {
        string themename = "themename";
        string isapplied = "isapplied";
        unlockedThemes = new int[themeColors.Length];
        if (PlayerPrefs.GetInt("firsttimetheme", 1) == 1)
        {
            
            for (int i = 0; i < themeColors.Length; i++)
            {
                PlayerPrefs.SetInt(themename + i.ToString(), 0);
                PlayerPrefs.SetInt(isapplied + i.ToString(), 0);
            }
            PlayerPrefs.SetInt(themename + 0.ToString(), 1);
            PlayerPrefs.SetInt(themename + 1.ToString(), 1);
            PlayerPrefs.SetInt(isapplied + 0.ToString(), 1);
            PlayerPrefs.SetInt(isapplied + 1.ToString(), 1);
            PlayerPrefs.SetInt("firsttimetheme", 0);
            Debug.Log("data check");
        }
        Debug.Log("data check out");
        for (int i = 0; i < themeColors.Length; i++)
        {
            unlockedThemes[i] = PlayerPrefs.GetInt("themename"+ i.ToString());
        }
        int unlockCount = 0;
        foreach (int u in unlockedThemes)
        {
            if (u >= 1)
                unlockCount++;
        }
        bgColorsUnlocked = new Color[unlockCount];
        themeColorsUnlocked = new Color[unlockCount];
        darkThemeColorsUnlocked = new Color[unlockCount];
        playfieldColorsUnlocked = new Color[unlockCount];
        int count = 0;
        for(int i=0;i<unlockedThemes.Length;i++)
        {
            if (unlockedThemes[i] >= 1)
            {
                bgColorsUnlocked[count] = bgColors[i];
                themeColorsUnlocked[count] = themeColors[i];
                darkThemeColorsUnlocked[count] = darkThemeColors[i];
                playfieldColorsUnlocked[count] = playfieldColors[i];
                count++;
            }
        }
    }

   

    void OnMouseDown()
    {
     

        if (!isThemeChanging)
        {
            counter++;
            if (counter > themeColorsUnlocked.Length-1)
                counter = 0;
            StopAllCoroutines();
            StartCoroutine(ChangeThemeColor());
        }

    }

    IEnumerator ChangeThemeColor()
    {
            
            isThemeChanging = true;
            themeColorFill.GetComponent<SpriteRenderer>().size = new Vector2(0.32f,0);
            if (bgColorsUnlocked[counter] == Color.white)
            {
                
                int i = 0;
                while (i < 40)
                {
                    themeColorFill.GetComponent<SpriteRenderer>().size = Vector2.Lerp(themeColorFill.GetComponent<SpriteRenderer>().size, new Vector2(0.32f, 0.38f), 0.1f);
                    Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, bgColorsUnlocked[counter], speed * 0.1f);
                    texts = HomeController.instance.gameObject.GetComponentsInChildren<TextMeshPro>();

                    foreach (TextMeshPro text in texts)
                        text.color = Color.Lerp(text.color, darkThemeColorsUnlocked[counter], speed * 0.1f);

                    darkObjects = GameObject.FindGameObjectsWithTag("dark");
                    darkConflictObjects = GameObject.FindGameObjectsWithTag("dark_conflict");
                    themeObjects = GameObject.FindGameObjectsWithTag("theme");
                    themeConflictObjects = GameObject.FindGameObjectsWithTag("theme_conflict");

                    foreach (GameObject g in darkObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, darkThemeColorsUnlocked[counter], speed * 0.1f);
                    foreach (GameObject g in darkConflictObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, darkThemeColorsUnlocked[counter], speed * 0.1f);
                    foreach (GameObject g in themeObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, themeColorsUnlocked[counter], speed * 0.1f); ;
                    foreach (GameObject g in themeConflictObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, themeColorsUnlocked[counter], speed * 0.1f);

                    yield return null;
                    i++;
                }
                gameScreen.SetActive(true);
                spikeScreen.SetActive(true);
                spikeModePad.SetActive(true);
                defaultScreen.SetActive(true);
                replayScreen.SetActive(true);
                score2.SetActive(true);
                upcover.SetActive(true);
                downcover.SetActive(true);

                texts = HomeController.instance.gameObject.GetComponentsInChildren<TextMeshPro>();
                darkObjects = GameObject.FindGameObjectsWithTag("dark");
                darkConflictObjects = GameObject.FindGameObjectsWithTag("dark_conflict");
                themeObjects = GameObject.FindGameObjectsWithTag("theme");
                themeConflictObjects = GameObject.FindGameObjectsWithTag("theme_conflict");
                foreach (TextMeshPro text in texts)
                    text.color = darkThemeColorsUnlocked[counter];
                foreach (GameObject g in darkObjects)
                    g.GetComponent<SpriteRenderer>().color = darkThemeColorsUnlocked[counter];
                foreach (GameObject g in darkConflictObjects)
                    g.GetComponent<SpriteRenderer>().color = darkThemeColorsUnlocked[counter];
                foreach (GameObject g in themeObjects)
                    g.GetComponent<SpriteRenderer>().color = themeColorsUnlocked[counter];
                foreach (GameObject g in themeConflictObjects)
                    g.GetComponent<SpriteRenderer>().color = themeColorsUnlocked[counter];

           
                score2.SetActive(false);
                upcover.SetActive(false);
                downcover.SetActive(false);
                gameScreen.SetActive(false);
                spikeScreen.SetActive(false);
            spikeModePad.SetActive(false);
            defaultScreen.SetActive(false);


        }
            else
            {
                int i = 0;

                while (i < 40)
                {
                    themeColorFill.GetComponent<SpriteRenderer>().size = Vector2.Lerp(themeColorFill.GetComponent<SpriteRenderer>().size, new Vector2(0.32f, 0.38f), 0.1f);
                    Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, bgColorsUnlocked[counter], speed * 0.1f);
                     texts = HomeController.instance.gameObject.GetComponentsInChildren<TextMeshPro>();

                    foreach (TextMeshPro text in texts)
                        text.color = Color.Lerp(text.color, darkThemeColorsUnlocked[counter], speed * 0.1f);
                    foreach (TextMeshPro text in gameScreen.GetComponentsInChildren<TextMeshPro>())
                        text.color = Color.Lerp(text.color, Color.white, speed * 0.1f); ;
                    foreach (TextMeshPro text in replayScreen.GetComponentsInChildren<TextMeshPro>())
                        text.color = Color.Lerp(text.color, darkThemeColorsUnlocked[counter], speed * 0.1f); ;

                    darkObjects = GameObject.FindGameObjectsWithTag("dark");
                    themeObjects = GameObject.FindGameObjectsWithTag("theme");
                    darkConflictObjects = GameObject.FindGameObjectsWithTag("dark_conflict");
                    themeConflictObjects = GameObject.FindGameObjectsWithTag("theme_conflict");

                    foreach (GameObject g in darkObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, darkThemeColorsUnlocked[counter], speed * 0.1f); ;
                    foreach (GameObject g in themeObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, themeColorsUnlocked[counter], speed * 0.1f); ;
                    foreach (GameObject g in darkConflictObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, Color.white, speed * 0.1f); ;
                    foreach (GameObject g in themeConflictObjects)
                        g.GetComponent<SpriteRenderer>().color = Color.Lerp(g.GetComponent<SpriteRenderer>().color, Color.white, speed * 0.1f); ;
                    yield return null;
                    i++;

                }
                gameScreen.SetActive(true);
                spikeScreen.SetActive(true);
            spikeModePad.SetActive(true);
            defaultScreen.SetActive(true);
                replayScreen.SetActive(true);
                score2.SetActive(true);
                upcover.SetActive(true);
                downcover.SetActive(true);

                texts = HomeController.instance.gameObject.GetComponentsInChildren<TextMeshPro>();
                darkObjects = GameObject.FindGameObjectsWithTag("dark");
                darkConflictObjects = GameObject.FindGameObjectsWithTag("dark_conflict");
                themeObjects = GameObject.FindGameObjectsWithTag("theme");
                themeConflictObjects = GameObject.FindGameObjectsWithTag("theme_conflict");

                foreach (TextMeshPro text in texts)
                    text.color = darkThemeColorsUnlocked[counter];
                foreach (TextMeshPro text in gameScreen.GetComponentsInChildren<TextMeshPro>())
                    text.color = Color.white;
                foreach (TextMeshPro text in replayScreen.GetComponentsInChildren<TextMeshPro>())
                    text.color = darkThemeColorsUnlocked[counter];
                foreach (GameObject g in darkObjects)
                    g.GetComponent<SpriteRenderer>().color = darkThemeColorsUnlocked[counter];
                foreach (GameObject g in themeObjects)
                    g.GetComponent<SpriteRenderer>().color = themeColorsUnlocked[counter];
                foreach (GameObject g in darkConflictObjects)
                    g.GetComponent<SpriteRenderer>().color = Color.white;
                foreach (GameObject g in themeConflictObjects)
                    g.GetComponent<SpriteRenderer>().color = Color.white;

                score2.SetActive(false);
                upcover.SetActive(false);
                downcover.SetActive(false);
                replayScreen.GetComponentsInChildren<TextMeshPro>()[2].color = Color.white;
            }
        playField.GetComponent<SpriteRenderer>().color = playfieldColorsUnlocked[counter];
            rippleEffect.GetComponent<SpriteRenderer>().color = playfieldColorsUnlocked[counter];
        
        replayScreen.SetActive(true);
            gameScreen.SetActive(false);
            spikeScreen.SetActive(false);
        spikeModePad.SetActive(false);
        defaultScreen.SetActive(false);
            isThemeChanging = false;

        int storedCounter = getActualCounterForThemeShop(counter);
        PlayerPrefs.SetInt("theme", storedCounter);
        PlayerPrefs.SetInt("isapplied" + storedCounter.ToString(), 1);
    }

    public void firstThemeChange(int counterValue)
    {
        if (bgColorsUnlocked[counterValue] == Color.white)
        {

            Camera.main.backgroundColor = bgColorsUnlocked[counterValue];

            gameScreen.SetActive(true);
            spikeScreen.SetActive(true);
            spikeModePad.SetActive(true);
            defaultScreen.SetActive(true);

            replayScreen.SetActive(true);
            score2.SetActive(true);
            upcover.SetActive(true);
            downcover.SetActive(true);

            texts = HomeController.instance.gameObject.GetComponentsInChildren<TextMeshPro>();
            darkObjects = GameObject.FindGameObjectsWithTag("dark");
            darkConflictObjects = GameObject.FindGameObjectsWithTag("dark_conflict");
            themeObjects = GameObject.FindGameObjectsWithTag("theme");
            themeConflictObjects = GameObject.FindGameObjectsWithTag("theme_conflict");
            foreach (TextMeshPro text in texts)
                text.color = darkThemeColorsUnlocked[counterValue];
            foreach (GameObject g in darkObjects)
                g.GetComponent<SpriteRenderer>().color = darkThemeColorsUnlocked[counterValue];
            foreach (GameObject g in darkConflictObjects)
                g.GetComponent<SpriteRenderer>().color = darkThemeColorsUnlocked[counterValue];
            foreach (GameObject g in themeObjects)
                g.GetComponent<SpriteRenderer>().color = themeColorsUnlocked[counterValue];
            foreach (GameObject g in themeConflictObjects)
                g.GetComponent<SpriteRenderer>().color = themeColorsUnlocked[counterValue];


            score2.SetActive(false);
            upcover.SetActive(false);
            downcover.SetActive(false);
            gameScreen.SetActive(false);
            spikeModePad.SetActive(false);
            spikeScreen.SetActive(false);
            defaultScreen.SetActive(false);


        }
        else
        {

           Camera.main.backgroundColor = bgColorsUnlocked[counterValue];

            gameScreen.SetActive(true);
            spikeScreen.SetActive(true);
            spikeModePad.SetActive(true);
            defaultScreen.SetActive(true);
            replayScreen.SetActive(true);
            score2.SetActive(true);
            upcover.SetActive(true);
            downcover.SetActive(true);

            texts = HomeController.instance.gameObject.GetComponentsInChildren<TextMeshPro>();
            darkObjects = GameObject.FindGameObjectsWithTag("dark");
            darkConflictObjects = GameObject.FindGameObjectsWithTag("dark_conflict");
            themeObjects = GameObject.FindGameObjectsWithTag("theme");
            themeConflictObjects = GameObject.FindGameObjectsWithTag("theme_conflict");

            foreach (TextMeshPro text in texts)
                text.color = darkThemeColorsUnlocked[counterValue];
            foreach (TextMeshPro text in gameScreen.GetComponentsInChildren<TextMeshPro>())
                text.color = Color.white;
            foreach (TextMeshPro text in replayScreen.GetComponentsInChildren<TextMeshPro>())
                text.color = darkThemeColorsUnlocked[counterValue];
            foreach (GameObject g in darkObjects)
                g.GetComponent<SpriteRenderer>().color = darkThemeColorsUnlocked[counterValue];
            foreach (GameObject g in themeObjects)
                g.GetComponent<SpriteRenderer>().color = themeColorsUnlocked[counterValue];
            foreach (GameObject g in darkConflictObjects)
                g.GetComponent<SpriteRenderer>().color = Color.white;
            foreach (GameObject g in themeConflictObjects)
                g.GetComponent<SpriteRenderer>().color = Color.white;

            score2.SetActive(false);
            upcover.SetActive(false);
            downcover.SetActive(false);
            replayScreen.GetComponentsInChildren<TextMeshPro>()[2].color = Color.white;
        }
        playField.GetComponent<SpriteRenderer>().color = playfieldColorsUnlocked[counterValue];
        rippleEffect.GetComponent<SpriteRenderer>().color = playfieldColorsUnlocked[counterValue];
        replayScreen.SetActive(true);
        gameScreen.SetActive(false);
        spikeModePad.SetActive(true);
        spikeScreen.SetActive(false);
        defaultScreen.SetActive(false);
        counter = counterValue;
        int storedCounter = getActualCounterForThemeShop(counterValue);
        PlayerPrefs.SetInt("theme", storedCounter);
        PlayerPrefs.SetInt("isapplied" + storedCounter.ToString(), 1);
    }

    int getActualCounterForThemeDrop(int a)
    {
        int c = 0;
        for (int i = 0; i < unlockedThemes.Length; i++)
        {
            if (unlockedThemes[i] >= 1)
            {
                if (i == a)
                    break;
                c++;

            }
        }
        return c;
    }

    int getActualCounterForThemeShop(int a)
    {
        int c = 0;
        int final = 0;
        for (int i = 0; i < unlockedThemes.Length; i++)
        {
            if (unlockedThemes[i] >= 1)
            {
                if (c == a)
                {
                    final = i;
                    break;
                }
                c++;
            }

        }
        return final;
    }

}
