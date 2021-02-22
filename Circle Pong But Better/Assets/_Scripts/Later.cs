using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Later : MonoBehaviour
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
        if(gameObject.name == "Later")
            StartCoroutine(FindObjectOfType<SharePrompt>().closeSharePrompt());
        else
            StartCoroutine(FindObjectOfType<RatePrompt>().closeRatePrompt());
    }
}
