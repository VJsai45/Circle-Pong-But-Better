using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeModeLeft : MonoBehaviour
{
    public bool isPressed;
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
        if (!isPressed)
        {
            isPressed = true;
            GetComponentInParent<changeMode>().counter--;
            StartCoroutine(GetComponentInParent<changeMode>().selectMode(-1));

        }
    }
}
