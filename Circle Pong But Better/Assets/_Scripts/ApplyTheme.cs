using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ApplyTheme : MonoBehaviour
{
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
        GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GetComponentInChildren<TextMeshPro>().color = Color.black;
        GetComponentInChildren<TextMeshPro>().text = "APPLIED";
        GetComponent<BoxCollider2D>().enabled = false;
        FindObjectOfType<ThemeShopController>().changeTheme();
    }
}
