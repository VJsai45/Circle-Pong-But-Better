using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public GameObject  BGPlayer;
    public float sizeControlSpeed;
    public bool isGameOn;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(changePlaygroundBig());
    }

    // Update is called once per frame
    void Update()
    {
        
            if (Input.touchCount > 0  && HomeController.instance.isGameOn && Input.GetTouch(0).position.y < Screen.height/2)
            {
                if (Input.GetTouch(0).position.x > Screen.width / 2)
                {
                   GetComponent<circlePlatform>().movePlatform(1);
                }                                                                                         // For main build
                else
                {
                    GetComponent<circlePlatform>().movePlatform(-1);
            }
            }
        if (Application.isEditor)
        {

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && HomeController.instance.isGameOn)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    GetComponent<circlePlatform>().movePlatform(1);
                }                                                                                       // for editor
                else
                {
                    GetComponent<circlePlatform>().movePlatform(-1);
                }
            }
        }



    }

    public IEnumerator changePlaygroundBig()
    {
        while (BGPlayer.transform.localScale.x < 4.5f)
        {
            BGPlayer.transform.localScale = Vector3.MoveTowards(BGPlayer.transform.localScale, new Vector3(4.5f, 4.5f, 1), Time.deltaTime * sizeControlSpeed);
            transform.localPosition = new Vector3(0, -BGPlayer.transform.localScale.x / 2, 0);
            yield return null;
        }
        StartCoroutine(changePlaygroundSmall());
    }


    public IEnumerator changePlaygroundSmall()
    {
        while (BGPlayer.transform.localScale.x > 3)
        {
            BGPlayer.transform.localScale = Vector3.MoveTowards(BGPlayer.transform.localScale, new Vector3(3, 3, 1), Time.deltaTime * sizeControlSpeed);
            transform.localPosition = new Vector3(0, -BGPlayer.transform.localScale.x / 2, 0);
            yield return null;
        }
        StartCoroutine(changePlaygroundBig());

    }

   




    }
