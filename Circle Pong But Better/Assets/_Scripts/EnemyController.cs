using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject parent;
    public float enemySpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator movePlatform(Vector2 destination)
    {
        Debug.Log("dist"+ Vector2.Distance((Vector2)transform.position, destination));
        int clockwise = 0, antiClockwise = 0, direction ;
        Quaternion originalPos = parent.transform.rotation;
        float offset1 = Random.Range(0.2f, 0.5f);
        float offset2 = Random.Range(0.1f, 0.2f);
        float offset;
        if (Random.Range(0, 2) == 0)
            offset = offset1;
        else
            offset = offset2;

        while (Vector2.Distance((Vector2)transform.position, destination) > offset)
            {
                parent.transform.Rotate(1 * new Vector3(0, 0, enemySpeed));
                clockwise++;
            }
        parent.transform.rotation = originalPos;
        while (Vector2.Distance((Vector2)transform.position, destination) > offset)
        {
            parent.transform.Rotate(-1 * new Vector3(0, 0, enemySpeed));
            antiClockwise++;
        }
        parent.transform.rotation = originalPos;
        if (clockwise > antiClockwise)
            direction = -1;
        else
            direction = 1;

        while (Vector2.Distance((Vector2)transform.position, destination) > offset)
        {
            parent.transform.Rotate(direction * new Vector3(0, 0, enemySpeed));
            yield return null;
            Debug.Log("diff" + (Vector2)transform.position + " " + destination);
        }


    }
}
