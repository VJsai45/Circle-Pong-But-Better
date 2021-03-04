using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeShopIcon : MonoBehaviour
{
    public GameObject themeshop, themeShopBack,notification;
    public bool isOpening = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if (!isOpening && !themeshop.GetComponent<ThemeShopController>().isClosing)
        {
            Debug.Log("theme shop open");
            StopAllCoroutines();
            StartCoroutine(openThemeShop());
        }
    }

    IEnumerator openThemeShop()
    {
            notification.GetComponentInChildren<TextMeshPro>().text = 0.ToString();
            notification.SetActive(false);
            isOpening = true;
            AdManager.instance.HideBannerAd();
            var dest = Vector3.zero;
            var homeDest = new Vector3(-8, 0, 0);
            var tsc = themeshop.GetComponent<ThemeShopController>();
            tsc.isRotating = true;
            tsc.setInitialThemeShop();
            tsc.isFirstTime = false;
            while (Vector3.Distance(themeshop.transform.position, dest) > 0.01f)
            {
                themeshop.transform.position = Vector3.Lerp(themeshop.transform.position, dest, 0.2f);
                themeshop.transform.localScale = Vector3.Lerp(themeshop.transform.localScale, Vector3.one, 0.3f);
                HomeController.instance.gameObject.transform.position = Vector3.Lerp(HomeController.instance.gameObject.transform.position, homeDest, 0.2f);
                yield return null;
            }
            themeshop.transform.position = dest;
            foreach (GameObject c in tsc.cards)
                c.SetActive(true);
            tsc.isRotating = false;
            tsc.isFirstTime = false;
            tsc.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            themeShopBack.GetComponent<BoxCollider2D>().enabled = true;
            isOpening = false;
        
        
    }

    public IEnumerator pulsateNotification()
    {
        var ripple = notification.GetComponentsInChildren<SpriteRenderer>()[1].gameObject;
        ripple.GetComponent<SpriteRenderer>().color = new Color(ripple.GetComponent<SpriteRenderer>().color.r, ripple.GetComponent<SpriteRenderer>().color.g, ripple.GetComponent<SpriteRenderer>().color.b, 0.35f);
        var dest = new Vector3(1.4f, 1.4f, 1);
        while (Vector3.Distance(ripple.transform.localScale, dest) > 0.01f)
        {
            ripple.transform.localScale = Vector3.Lerp(ripple.transform.localScale, dest, 0.1f);
            yield return null;
        }
        while (Vector3.Distance(ripple.transform.localScale, Vector3.one) > 0.01f)
        {
            ripple.transform.localScale = Vector3.Lerp(ripple.transform.localScale, Vector3.one, 0.1f);
            yield return null;
        }
        StartCoroutine(pulsateNotification());
    }

}
