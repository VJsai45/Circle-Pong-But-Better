using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeModeController : MonoBehaviour
{
    public int counter,count = 0;
    public int spikeCounter = 6;

    public GameObject pad, padEmpty;
    Vector3 startPos;
    List<GameObject> spikes = new List<GameObject>();
    List<GameObject> pads = new List<GameObject>();
    List<GameObject> remainingSpikes = new List<GameObject>();
    List<GameObject> lastRandomSpikes = new List<GameObject>();
    Color startColor;
    public static SpikeModeController instance;
    public float platformSpeed = 3;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //void Start()
    //{
    //  //  Debug.Log("start");
    //    startPos = pad.transform.position;
    //    startColor = pad.GetComponentInChildren<PolygonCollider2D>().gameObject.GetComponent<SpriteRenderer>().color;
    //    //StartCoroutine(createRing());
    //}

    void OnEnable()
    {
        startPos = pad.transform.position;
        startColor = pad.GetComponentInChildren<PolygonCollider2D>().gameObject.GetComponent<SpriteRenderer>().color;
        StartCoroutine(createRing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && HomeController.instance.isGameOn && Input.GetTouch(0).position.y < Screen.height / 2)
        {
            if (Input.GetTouch(0).position.x > Screen.width / 2)
            {
                moveRing(1);
            }                                                                                         // For main build
            else
            {
                moveRing(-1);
            }
        }
        if (Application.isEditor)
        {

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && HomeController.instance.isGameOn)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    moveRing(1);
                }                                                                                       // for editor
                else
                {
                    moveRing(-1);
                }
            }
        }
    }

    public IEnumerator createRing()
    {
        
        spikes.Clear();
        lastRandomSpikes.Clear();
        foreach (BoxCollider2D b in GetComponentsInChildren<BoxCollider2D>())
            Destroy(b.gameObject);
        pads.Clear();
        count = 0;
        spikeCounter = 6;
        counter = 0;
        GetComponent<EdgeCollider2D>().enabled = true;
        int i = 0;
        while (counter < 16 )
        {
            transform.Rotate(new Vector3(0, 0, 22.5f));
            var padItem = Instantiate(pad, startPos,Quaternion.identity);
            padItem.transform.SetParent(transform);
            padItem.SetActive(true);
            pads.Add(padItem);
            foreach (PolygonCollider2D p in padItem.GetComponentsInChildren<PolygonCollider2D>())
            {
                if (p.gameObject.layer == 11)
                {
                    p.gameObject.name = i.ToString();
                    spikes.Add(p.gameObject);
                    i++;
                }
            }      
            counter++;
        }

        pad.SetActive(false);
        System.Random rand = new System.Random();
        lastRandomSpikes = spikes.OrderBy(x => rand.Next()).Take(spikeCounter).ToList();
        //Debug.Log("count "+lastRandomSpikes.Count);
        var dest = new Vector3(0, 0.45f, 0);
        while (Vector3.Distance(lastRandomSpikes[0].transform.localPosition, dest) > 0.01f)
        {
            foreach (GameObject l in lastRandomSpikes)
            {
                l.transform.localPosition = Vector3.Lerp(l.transform.localPosition, dest, 0.4f);     
            }
            yield return null;
        }
  
    }

    public IEnumerator createNewSpikes(GameObject padObject)
    {
        count++;
        switch (count)
        {
            case 10:
                spikeCounter = 7;
                //StartCoroutine(destroyPad(padObject));
                break;
            case 20:
                spikeCounter = 8;
                StartCoroutine(destroyPad(padObject));
                break;
            case 30:
                spikeCounter = 9;
                //StartCoroutine(destroyPad(padObject));
                break;
            case 40:
                spikeCounter = 10;
                StartCoroutine(destroyPad(padObject));
                break;
            case 50:
                spikeCounter = 11;
                //StartCoroutine(destroyPad(padObject)); ;
                break;
            case 60:
                spikeCounter = 12;
                StartCoroutine(destroyPad(padObject)); ;
                break;
            case 70:
                spikeCounter = 13;
                //StartCoroutine(destroyPad(padObject)); ;
                break;
            case 80:
                spikeCounter = 14;
                StartCoroutine(destroyPad(padObject)); ;
                break;
  
            default:
                //Debug.Log("default");
                break;
        }
        //Debug.Log("spike count "+spikes.Count);
      
        System.Random rand = new System.Random();
        var randomSpikes = spikes.OrderBy(x => rand.Next()).Take(spikeCounter).ToList();

       // Debug.Log("size before " + spikes.Count + " " + remainingSpikes.Count);
        remainingSpikes.Clear();
        foreach (GameObject lrs in lastRandomSpikes)
        {
            bool flag = false;
            foreach (GameObject rs in randomSpikes)
            {
                if (rs.name == lrs.name)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                remainingSpikes.Add(lrs);
        }
        //Debug.Log(spikes.Count + "size  " + remainingSpikes.Count);

        var lastSpikesDest = new Vector3(0,-1,0); ;
        var dest = new Vector3(0, 0.45f, 0);
   
        while (Vector3.Distance(remainingSpikes[0].transform.localPosition, lastSpikesDest) > 0.01f)
        {
            foreach (GameObject rs in remainingSpikes)
            {
                rs.transform.localPosition = Vector3.Lerp(rs.transform.localPosition, lastSpikesDest, 0.2f);
                //Debug.Log("names  rs " + rs.name);
            }

            foreach (GameObject r in randomSpikes)
            {
                r.transform.localPosition = Vector3.Lerp(r.transform.localPosition, dest, 0.2f);
                //Debug.Log("names  r " + r.name);
            }
            yield return null;
        }

        lastRandomSpikes.Clear();
        lastRandomSpikes = randomSpikes;
    }

    IEnumerator destroyPad(GameObject padColllided)
    {
        //Debug.Log("lrs before " + lastRandomSpikes.Count);
        var pos = padColllided.transform.position;
        var rot = padColllided.transform.rotation;
        foreach (PolygonCollider2D c in padColllided.GetComponentsInChildren<PolygonCollider2D>())
            {
                    spikes.Remove(c.gameObject);
                    if (lastRandomSpikes.Contains(c.gameObject))
                        lastRandomSpikes.Remove(c.gameObject);
            }
        Destroy(padColllided);
        //var empty = Instantiate(padEmpty, pos, rot);
        //empty.transform.SetParent(transform);
        //empty.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        //empty.GetComponent<BoxCollider2D>().enabled = true;
       
       

        //Debug.Log("lrs after " + lastRandomSpikes.Count);

        
    }

    public void moveRing(float direction)
    {

        transform.Rotate(direction * new Vector3(0, 0, platformSpeed));

    }
}
