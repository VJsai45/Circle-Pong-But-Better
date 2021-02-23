


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeShopController : MonoBehaviour
{
    public Color[] bgColors, themeColors, darkThemeColors,playfieldColors;
    public GameObject cardsParent,cardTemplate,bg,themeIcon,scrollerPointsParent,scrollerPointTemplate,applyButton,themeConditionText,themeConditionParent,
        newTag;
    public string[] themeNames,themeConditions;
    public List<GameObject> cards = new List<GameObject>();
    public GameObject[] scrollerPoints;
    Vector3 pos,startTouchPos;
    Quaternion rot,cardsStartRotation;
    bool isTouchedCard=false;
    public bool isRotating;
    public int counter=0,actualCounter=0;
    public int[] themeConditionFlags;
    public bool isFirstTime = true;
    public int previousCounter = 0;
    Quaternion initialCardRotaion;
    Quaternion dest = Quaternion.identity;
    public bool isClosing = false;
    public bool isThemeShopOpen=false;

    // Start is called before the first frame update
    void Start()
    {
        isFirstTime = true;
        pos = cardTemplate.transform.position;
        rot = cardTemplate.transform.rotation;
        initialCardRotaion = cardsParent.transform.rotation;
        createCards();
        //var tc = FindObjectOfType<ThemeController>();
        //bgColors = tc.bgColorsUnlocked;
        //themeColors = tc.themeColorsUnlocked;
        //darkThemeColors = tc.darkThemeColorsUnlocked;
        //playfieldColors = tc.playfieldColorsUnlocked;


        counter = PlayerPrefs.GetInt("theme");
        previousCounter = counter;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && !isRotating && GetComponent<BoxCollider2D>().enabled )
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                        if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                            {
                            //cardsStartRotation = cardsParent.transform.rotation;
                            startTouchPos = touchPos;
                            isTouchedCard = true;
                            Debug.Log("touch started");

                            
                            }
                            break;
                        }
                    //case TouchPhase.Moved:
                    //    {
                    //        if (touchPos.x > startTouchPos.x)
                    //            moveCards(-1);
                    //        else
                    //            moveCards(1);
                    //        Debug.Log("touch moving");
                    //        break;
                    //    }
                    case TouchPhase.Ended:
                        {
                            
                            if (Vector3.Distance(startTouchPos, touchPos) > 0.25f && isTouchedCard)
                            {
                                StopAllCoroutines();
                                if (touchPos.x > startTouchPos.x)
                                    StartCoroutine(rotateCards(-1));
                                else
                                    StartCoroutine(rotateCards(1));

                                Debug.Log("touch ended");
                                isTouchedCard = false;
                            }
                            //else {

                            //   StartCoroutine(rotateCards(10));
                            //    Debug.Log("touch ended default");
                            //}
                            break;
                        }

                }
            
        }
        
    }

  

    IEnumerator rotateCards(int direction)
    {
        newTag.SetActive(false);
        cardsParent.transform.rotation = dest;
        //Quaternion preDest = Quaternion.identity;
        isRotating = true;
        var destEulers = cardsParent.transform.rotation.eulerAngles;
    
        dest = Quaternion.Euler(new Vector3(destEulers.x, destEulers.y, destEulers.z + direction * (360f / themeColors.Length)));
        //preDest = Quaternion.Euler(new Vector3(destEulers.x, destEulers.y, destEulers.z + direction / (360f / themeColors.Length)));
        counter += direction;
        if (counter > themeColors.Length - 1)
                counter = 0;
        if (counter < 0)
                counter = themeColors.Length - 1;
        
        Debug.Log("dest rot " + cardsParent.transform.rotation+" "+dest + " " + counter+" "+ (destEulers.z + direction * (360f / themeColors.Length)));
        var bgColorDest = bgColors[counter];
        var themeIconColorDest = playfieldColors[counter];
        StartCoroutine(animateScrollerPoints(direction));
        StartCoroutine(changeConditionText());
        //cardsParent.transform.rotation = preDest;
        while (Quaternion.Angle(cardsParent.transform.rotation, dest) > 0.1f)
        {
            cardsParent.transform.rotation = Quaternion.Lerp(cardsParent.transform.rotation, dest, 0.2f);
            bg.GetComponent<SpriteRenderer>().color = Color.Lerp(bg.GetComponent<SpriteRenderer>().color, bgColorDest, 0.16f);
            themeIcon.GetComponentsInChildren<SpriteRenderer>()[1].color = Color.Lerp(themeIcon.GetComponentsInChildren<SpriteRenderer>()[1].color, themeIconColorDest, 0.16f);
            
            yield return null;
        }
        //cardsParent.transform.rotation = dest;
        isRotating = false;
    }
     



    void createCards()
    {
        int i = 0;
        scrollerPoints = new GameObject[themeColors.Length];
        foreach (Color c in themeColors)
        {
            
            var card = Instantiate(cardTemplate,pos,rot, cardsParent.transform);
            cards.Add(card);
            card.GetComponentsInChildren<SpriteRenderer>()[0].color = c;
            card.GetComponentsInChildren<SpriteRenderer>()[1].color = c;
            cardsParent.transform.Rotate(new Vector3(0, 0, (360f / themeColors.Length)));
            themeIcon.GetComponent<SpriteRenderer>().color = darkThemeColors[0];
            themeIcon.GetComponentsInChildren<SpriteRenderer>()[1].color = playfieldColors[counter];
            card.GetComponentInChildren<TextMeshPro>().text = themeNames[i];
            if (i % 2 == 0)
                card.GetComponentInChildren<TextMeshPro>().color = Color.black;
            else
                card.GetComponentInChildren<TextMeshPro>().color = Color.white;
            card.SetActive(false);
            i++;
        }
        cardsParent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (360f/themeColors.Length) * counter));
      
        themeConditionParent.transform.localPosition = new Vector3(0, 3.5f, 0);
        //cardsParent.transform.rotation = Quaternion.identity;
        Destroy(cardTemplate);
        createPoints();
    }

    void createPoints()
    {
        //scrollerPoints.Add(scrollerPointsParent.GetComponentInChildren<SpriteRenderer>().gameObject);
        float offset = 0;
        int i = 0;
        foreach (Color c in themeColors)
        {
            var point = Instantiate(scrollerPointTemplate, scrollerPointsParent.transform);
            scrollerPoints[i]=point;
            point.GetComponent<SpriteRenderer>().color = Color.black;
            point.transform.localPosition = new Vector3(point.transform.localPosition.x + offset, point.transform.localPosition.y, point.transform.localPosition.z);
            offset += 0.15f;
            i++;
        }
        scrollerPoints[counter].GetComponent<SpriteRenderer>().color = themeColors[counter];
        scrollerPoints[counter].transform.localScale = new Vector3(0.125f, 0.125f, 1);
        
        Destroy(scrollerPointTemplate);
        setInitialThemeShop();
        Debug.Log("isfirstime");
    }


    IEnumerator animateScrollerPoints(int direction)
    {
        GameObject currentPoint = scrollerPoints[counter];
        int lastCounter = counter - direction;
        if (lastCounter < 0)
            lastCounter = themeColors.Length - 1;
        if (lastCounter > themeColors.Length - 1)
            lastCounter = 0;
        GameObject lastpoint = scrollerPoints[lastCounter];
        //scrollerPoints.Remove(lastpoint);
        Vector3 destSizeCurrent = new Vector3(0.125f, 0.125f, 1);
        Vector3 destSizeLast = new Vector3(0.075f, 0.075f, 1);
        Color lastColorAndAll;
        if (counter % 2 == 0)
            lastColorAndAll = Color.black;
        else
            lastColorAndAll = Color.white;
        Color currentPointColor = themeColors[counter];
        Debug.Log("current counter " + counter + " " + (counter - direction) + " " + Vector3.Distance(currentPoint.transform.localScale, destSizeCurrent));




        while (Vector3.Distance(currentPoint.transform.localScale, destSizeCurrent) > 0.01f)
        {
            currentPoint.transform.localScale = Vector3.Lerp(currentPoint.transform.localScale, destSizeCurrent, 0.1f);
            currentPoint.GetComponent<SpriteRenderer>().color = Color.Lerp(currentPoint.GetComponent<SpriteRenderer>().color, currentPointColor, 0.1f);
            lastpoint.transform.localScale = Vector3.Lerp(lastpoint.transform.localScale, destSizeLast, 0.1f);
            for (int i = 0; i < scrollerPoints.Length; i++)
            {
                if(i != counter)
                    scrollerPoints[i].GetComponent<SpriteRenderer>().color = Color.Lerp(scrollerPoints[i].GetComponent<SpriteRenderer>().color, lastColorAndAll, 0.25f);
            }

            yield return null;
        }

    }


    //void OnMouseDown()
    //{
    //    Debug.Log("down mouse");
    //    if (!isRotating)
    //    {
    //        StopAllCoroutines();
    //        //cardsStartRotation = cardsParent.transform.rotation;
    //        StartCoroutine(rotateCards(1));
    //    }
    //}

    public IEnumerator closeThemeShop()
    {
        if (!isClosing)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            isClosing = true;
            var dest = new Vector3(1.625f, -3.125f, 0);
            var homeDest = new Vector3(0, 0, 0);
            foreach (GameObject g in cards)
                g.SetActive(false);
            cards[counter].SetActive(true);

            while (Vector3.Distance(transform.position, dest) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, dest, 0.2f);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.3f);
                HomeController.instance.gameObject.transform.position = Vector3.Lerp(HomeController.instance.gameObject.transform.position, homeDest, 0.2f);
                yield return null;
            }
            transform.position = dest;
            cardsParent.transform.rotation = initialCardRotaion;
            cards[counter].SetActive(false);
            //gameObject.SetActive(false);
           // AdManager.instance.ShowBannerAd();
            isClosing = false;
            isThemeShopOpen = false;
        }
    }

    public void setInitialThemeShop()
    {
        isThemeShopOpen = true;
        counter = PlayerPrefs.GetInt("theme");
        Debug.Log("before counter " + counter);
        
        
        setConditionFlags();
        Debug.Log("after counter " + counter);
        themeConditionFlags[counter] = 2;
        cards[counter].SetActive(true);
        bg.GetComponent<SpriteRenderer>().color = bgColors[counter];
        themeIcon.GetComponentsInChildren<SpriteRenderer>()[1].color = playfieldColors[counter];
        scrollerPoints[counter].GetComponent<SpriteRenderer>().color = themeColors[counter];
        scrollerPoints[counter].transform.localScale = new Vector3(0.125f, 0.125f, 1);
        setConditionText();
        foreach (GameObject g in scrollerPoints)
        {
            g.GetComponent<SpriteRenderer>().color = Color.black;
            g.transform.localScale = new Vector3(0.075f, 0.075f, 1);
        }
        scrollerPoints[counter].GetComponent<SpriteRenderer>().color = themeColors[counter];
        scrollerPoints[counter].transform.localScale = new Vector3(0.125f, 0.125f, 1);
        cardsParent.transform.rotation = Quaternion.identity;
        cardsParent.transform.Rotate(new Vector3(0, 0, (360f / themeColors.Length) * counter));
        dest = cardsParent.transform.rotation;
        previousCounter = counter;
        
    }

    void setConditionFlags()
    {
        string themename = "themename";
        themeConditionFlags = new int[themeColors.Length];
        
        //if (PlayerPrefs.GetInt("firsttimetheme", 1) == 1)
        //{

        //    for (int i = 0; i < themeConditionFlags.Length; i++)
        //    {
        //        themeConditionFlags[i] = 0;
        //        PlayerPrefs.SetInt(themename + i.ToString(), 0);
        //    }
        //    themeConditionFlags[0] = 1;
        //    themeConditionFlags[1] = 1;
        //    PlayerPrefs.SetInt(themename + 0.ToString(), 1);
        //    PlayerPrefs.SetInt(themename + 1.ToString(), 1);
        //    PlayerPrefs.SetInt("firsttimetheme", 0);
        //}
        //else
        //{
            for (int i = 0; i < themeConditionFlags.Length; i++)
            {
                themeConditionFlags[i] = PlayerPrefs.GetInt(themename + i.ToString()); ;
                   Debug.Log("theme unlock status "+i+" " + PlayerPrefs.GetInt(themename + i.ToString()));
        }
        //}
     
    }

    void setConditionText()
    {
        if (themeConditionFlags[counter] == 1)
        {
            applyButton.SetActive(true);
            applyButton.GetComponent<SpriteRenderer>().color = Color.black;
            applyButton.GetComponentInChildren<TextMeshPro>().color = Color.white;
            applyButton.GetComponentInChildren<TextMeshPro>().text = "APPLY";
            applyButton.GetComponent<BoxCollider2D>().enabled = true;
            themeConditionText.SetActive(false);
            Debug.Log("isapplied " + PlayerPrefs.GetInt("isapplied" + counter.ToString()));
            if (PlayerPrefs.GetInt("isapplied" + counter.ToString()) == 0)
                StartCoroutine(openCloseNewTag(1));
        }
        else if (themeConditionFlags[counter] == 2)
        {
            applyButton.SetActive(true);
            applyButton.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
            applyButton.GetComponentInChildren<TextMeshPro>().color = Color.black;
            applyButton.GetComponentInChildren<TextMeshPro>().text = "APPLIED";
            applyButton.GetComponent<BoxCollider2D>().enabled = false;
            themeConditionText.SetActive(false);
            newTag.SetActive(false);
            Debug.Log("isapplied " + PlayerPrefs.GetInt("isapplied" + counter.ToString()));

        }
        else
        {
            applyButton.SetActive(false);
            themeConditionText.SetActive(true);
            themeConditionText.GetComponent<TextMeshPro>().text = themeConditions[counter];
        }

        //if (HomeController.instance.newlyUnlockedColors[counter] == 1)
        //{

        //}
    }

    IEnumerator openCloseNewTag(int flag)
    {
        newTag.SetActive(true);
        Color destCard, destText, destCardInvisible, destTextInvisible;
        if (bgColors[counter] == Color.white)
        {
            destCard = Color.black;
            destText = Color.white;
          
        }
        else
        {
            destCard = Color.white;
            destText = Color.black;
        }
        destCardInvisible = new Color(destCard.r, destCard.g, destCard.b, 0);
        destTextInvisible = new Color(destText.r, destText.g, destText.b, 0);
        int i = 0;
        if (flag == 0)
        {
            while (i<25)
            {
                newTag.GetComponent<SpriteRenderer>().color = Color.Lerp(newTag.GetComponent<SpriteRenderer>().color, destCardInvisible, 0.5f);
                newTag.GetComponentInChildren<TextMeshPro>().color = Color.Lerp(newTag.GetComponentInChildren<TextMeshPro>().color, destTextInvisible, 0.5f);
                i++;
                yield return null;
            }
        }
        else
        {
            while (i<25)
            {
                newTag.GetComponent<SpriteRenderer>().color = Color.Lerp(newTag.GetComponent<SpriteRenderer>().color, destCard, 0.3f);
                newTag.GetComponentInChildren<TextMeshPro>().color = Color.Lerp(newTag.GetComponentInChildren<TextMeshPro>().color, destText, 0.3f);
                i++;
                yield return null;
            }
        }
    }


    IEnumerator changeConditionText()
    {
        var destUp = new Vector3(0, 3.5f, 0);
        var destDown = new Vector3(0, 2.75f, 0);
        while (Vector3.Distance(themeConditionParent.transform.localPosition, destDown) > 0.01f)
        {
            themeConditionParent.transform.localPosition = Vector3.Lerp(themeConditionParent.transform.localPosition, destDown, 0.3f);
            yield return null;
        }
        setConditionText();
        while (Vector3.Distance(themeConditionParent.transform.localPosition, destUp) > 0.01f)
        {
            themeConditionParent.transform.localPosition = Vector3.Lerp(themeConditionParent.transform.localPosition, destUp, 0.3f);
            yield return null;
        }
    }

    public void changeTheme()
    {
        for (int i = 0; i < themeConditionFlags.Length; i++)
        {
            if(themeConditionFlags[i] == 2)
                themeConditionFlags[i] = 1;
        }
        themeConditionFlags[counter] = 2;
        int c = getActualCounterForThemeDrop(counter);

        FindObjectOfType<ThemeController>().firstThemeChange(c);
    }

    int  getActualCounterForThemeDrop(int a)
    {
        int c = 0;
        for (int i = 0; i < themeConditionFlags.Length; i++)
        {
            if (themeConditionFlags[i] >= 1)
            {
                if (i == a)
                    break;
                c++;

            }
        }
        return c;
    }

    //int getActualCounterForThemeShop(int a)
    //{
    //    int c = 0;
    //    int final=0;
    //    for (int i = 0; i < themeConditionFlags.Length; i++)
    //    {
    //        if (themeConditionFlags[i] >= 1)
    //        {
    //            if (c == a)
    //            {
    //                final = i;
    //                break;
    //            }
    //            c++;
    //        }
            
    //    }
    //    return final;
    //}
}
