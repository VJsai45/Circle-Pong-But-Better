using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlePlatform : MonoBehaviour
{
    float posX, posY, angle;
    public float radius, platformSpeed;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       

    }


    public void movePlatform(float direction)
    {

        parent.transform.Rotate(direction*new Vector3(0, 0, platformSpeed));
        
    }
}

   
