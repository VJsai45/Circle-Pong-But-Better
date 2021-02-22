using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeShopBack : MonoBehaviour
{
   

    void Start()
    {
        
    }

    void OnMouseDown()
    {
        if (!GetComponentInParent<ThemeShopController>().isClosing  && !FindObjectOfType<ThemeShopIcon>().isOpening)
        {
            Debug.Log("theme shop back");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponentInParent<ThemeShopController>().StopAllCoroutines();
            StartCoroutine(GetComponentInParent<ThemeShopController>().closeThemeShop());
        }
    }
}
