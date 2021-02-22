using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeMode : MonoBehaviour
{
    GameObject modeText;
    public GameObject homeController,modeArrowRight,highscoreText;
    public int counter = 0;
    public string[] levels;
    public  float speed;
    float x;

    // Start is called before the first frame update
    void Start()
    {
        x = modeArrowRight.transform.localPosition.x;
    }



    public IEnumerator selectMode(int direction)
    {
        GetComponentInChildren<changeModeLeft>().isPressed = true;
        GetComponentInChildren<changeModeRight>().isPressed = true;
        resetArrow();
        homeController.GetComponent<Animator>().enabled = false;
        if (counter > levels.Length-1)
            counter = 0;
        else if (counter < 0)
            counter = levels.Length - 1;
        
        homeController.GetComponent<HomeController>().level = counter;
        string nextText = levels[counter];
        modeText = GetComponentInChildren<TextMeshPro>().gameObject;
        GameObject nextMode;
        Color textColor = modeText.GetComponent<TextMeshPro>().color;
        Color textColorTransparent = new Color(textColor.r, textColor.g, textColor.b, 0);
        Vector3 nextPosition;
        Vector3 destination = modeText.transform.position;
        if (direction > 0)
        {
            nextMode = Instantiate(modeText, Vector3.zero, Quaternion.identity,transform);
            nextMode.transform.position = modeText.transform.position - new Vector3( 0.35f,0, 0);
            nextMode.GetComponent<TextMeshPro>().color = new Color(textColor.r, textColor.g, textColor.b, 0);
            nextPosition = modeText.transform.position + new Vector3(0.35f, 0,0);
            //Debug.Log("right");
        }
        else
        {
            nextMode = Instantiate(modeText, Vector3.zero, Quaternion.identity, transform);
            nextMode.transform.position = modeText.transform.position + new Vector3(0.35f, 0, 0);
            nextMode.GetComponent<TextMeshPro>().color = new Color(textColor.r, textColor.g, textColor.b, 0);
            nextPosition = modeText.transform.position - new Vector3(0.35f, 0,0);
            //Debug.Log("left");
        }
        nextMode.GetComponent<TextMeshPro>().text = nextText;
        changeHighscore();

        while (1 - modeText.GetComponent<TextMeshPro>().color.a < 0.99f  )
        {
            modeText.transform.position = Vector3.Lerp(modeText.transform.position, nextPosition, speed*0.1f);
            modeText.GetComponent<TextMeshPro>().color = Color.Lerp(modeText.GetComponent<TextMeshPro>().color, textColorTransparent, speed * 0.1f);
            //Debug.Log("color " + nextMode.GetComponent<TextMeshPro>().color + " " + textColor);
            yield return null;
        }
        
        while (0.95f - nextMode.GetComponent<TextMeshPro>().color.a > 0.01f)
        {
            nextMode.transform.position = Vector3.Lerp(nextMode.transform.position, destination, speed * 0.1f);
            nextMode.GetComponent<TextMeshPro>().color = Color.Lerp(nextMode.GetComponent<TextMeshPro>().color, textColor, speed * 0.1f);
            //Debug.Log("next " + nextMode.GetComponent<TextMeshPro>().color + " " + textColor);
            yield return null;
        }
        nextMode.transform.position = destination;                                                                                                                                              
        nextMode.GetComponent<TextMeshPro>().color = textColor;
        Destroy(modeText);
        GetComponentInChildren<changeModeLeft>().isPressed = false;
        GetComponentInChildren<changeModeRight>().isPressed = false;
        homeController.GetComponent<Animator>().enabled = true;
    }


    void changeHighscore()
    {
        int highscoreValue=0;
        switch (counter)
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

        highscoreText.GetComponent<TextMeshPro>().text = "Highscore : "+highscoreValue.ToString();
    }



    public IEnumerator modeChangePrompt()
    {
        var start = modeArrowRight.transform.localPosition;
        var end = new Vector3(0.85f, modeArrowRight.transform.localPosition.y, modeArrowRight.transform.localPosition.z);
        while (Vector3.Distance(modeArrowRight.transform.localPosition, end) > 0.0001f)
        {
            modeArrowRight.transform.localPosition = Vector3.Lerp(modeArrowRight.transform.localPosition, end, 0.3f);
            yield return null;
        }
        while (Vector3.Distance(modeArrowRight.transform.localPosition, start) > 0.0001f)
        {
            modeArrowRight.transform.localPosition = Vector3.Lerp(modeArrowRight.transform.localPosition, start, 0.3f);
            yield return null;
        }
        StartCoroutine(modeChangePrompt());
    }


    public void resetArrow()
    {
        StopAllCoroutines();
        
        modeArrowRight.transform.localPosition += new Vector3(-modeArrowRight.transform.localPosition.x + x, 0, 0);
       
    }
}
                                                                                               