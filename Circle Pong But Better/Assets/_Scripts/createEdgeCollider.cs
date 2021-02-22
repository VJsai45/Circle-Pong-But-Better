using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createEdgeCollider : MonoBehaviour
{
    PolygonCollider2D poly;
    public float ThetaScale = 0.01f;
    public float radius = 3f;
    private int Size;
    private float Theta = 0f;
   
    void Start()
    {
        

        //if (gameObject.tag == "BGCircle")
        //    GetComponent<EdgeCollider2D>().isTrigger = true;

        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        Vector2[] points = new Vector2[Size+1];
        Debug.Log("size " + Size);
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            points[i] = new Vector2(x, y);
        }
        points[Size] = points[0];
        GetComponent<EdgeCollider2D>().points = points;
    }

   
}
