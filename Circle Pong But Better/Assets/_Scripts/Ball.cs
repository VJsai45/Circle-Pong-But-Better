

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Ball : MonoBehaviour
{
    public Vector3 ballDestination;
    public float ballSpeed,scoreTickerSpeed;
    public GameObject enemyController,homeController,upcover,downcover,destroyablePad,ballBurstParticle;
    public TextMeshPro score1,score2;
    public Vector2 previousCollison = Vector2.zero;
    Vector2 lowerOffset = new Vector2(-0.05f,-0.05f);
    Vector2 upperOffset = new Vector2(0.05f, 0.05f);
    public bool isOut,isEmpty;
    Rigidbody2D rb;
    AudioSource bounceSound;
    public int spikePadLayer;
    public float ballZPosition;
    public Gradient trailColor;
    GradientColorKey[] colorKeys;
    GradientAlphaKey[] alphaKeys;

    // Start is called before the first frame update
    void Start()
    {
        //ballDestination = new Vector3(0, -10, 0);
        //GetComponent<Rigidbody2D>().velocity = new Vector3(0, -10, 0);
        upcover.SetActive(true);
        downcover.SetActive(true);
        score2.gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        bounceSound = GetComponent<AudioSource>();
        spikePadLayer = 1 << 12;
        ballZPosition = transform.position.z;
      
    }
    void OnEnable()
    {
        colorKeys = trailColor.colorKeys;
        Debug.Log("color keys length " + colorKeys.Length);
        alphaKeys = trailColor.alphaKeys;
        colorKeys[0].color = GetComponent<SpriteRenderer>().color;
        trailColor.SetKeys(colorKeys, alphaKeys);
        GetComponent<TrailRenderer>().colorGradient = trailColor;
        ballDestination = new Vector3(0, -10, -3);
        ballSpeed = 1.5f;
        spikePadLayer = 1 << 12;
        isOut = false;
    }

    void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, (ballDestination*10) + new Vector3(0,0,-(ballDestination*10).z -3 ) , Time.deltaTime * ballSpeed);
        //Vector3 destination = ballDestination * Time.deltaTime * ballSpeed;
        //rb.MovePosition(transform.position + destination);

        if (transform.position.x > 3f || transform.position.x < -3f || transform.position.y > 5.2f || transform.position.y < -5.2f)
        {
            playerOut();
        }
        //Debug.Log("destination " + destination+" "+ballDestination.normalized);
        
    }

    void OnCollisionEnter2D(Collision2D Collision)
    {
        //Debug.Log("collided platform "+ Collision.collider.gameObject.layer+" "+Collision.GetContact(0).point+" "+previousCollison);
        if (!isOut)
        {
           
            switch (Collision.collider.gameObject.layer)
            {
                case 9:
                    {
                        if (Vector2.Distance(previousCollison, Collision.GetContact(0).point) > 0.5f)
                        {
                            ballSpeed = 2.35f;
                            bounceSound.Play();
                            StopAllCoroutines();
                            StartCoroutine(squishBall());
                            upcover.SetActive(true);
                            downcover.SetActive(true);
                            score2.gameObject.SetActive(true);
                            Debug.Log("collided insides " + Vector3.Distance(Collision.collider.gameObject.transform.position, Collision.GetContact(0).point) + " " + previousCollison);
                            previousCollison = Collision.GetContact(0).point;
                            if (Vector3.Distance(Collision.collider.gameObject.transform.position, Collision.GetContact(0).point) > 0.25f)
                                ballDestination = -ballDestination + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f),0);
                            else
                                ballDestination = Vector3.Reflect(transform.position, Collision.GetContact(0).normal);

                            //enemyController.GetComponent<EnemyController>().StopAllCoroutines();
                            //int currentScore = int.Parse(score.text);
                            //currentScore++;
                            //score.text = currentScore.ToString();
                            ballDestination = new Vector3(ballDestination.x, ballDestination.y, -4);
                            Debug.Log("classic/breathe " + Collision.GetContact(0).point + " " + ballDestination);
                            StartCoroutine(HomeController.instance.bounceRipple());
                            StartCoroutine(updateScore());
                            
                        }
                        break;
                    }
                case 10:
                    {
                        if (!isOut && Vector2.Distance(previousCollison, Collision.GetContact(0).point) > 0.5f)
                        {
                            
                            if (Physics2D.OverlapArea(Collision.GetContact(0).point+lowerOffset, Collision.GetContact(0).point+upperOffset,spikePadLayer))
                            {
                                //Debug.Log("spike pad name " + Physics2D.OverlapArea(Collision.GetContact(0).point + lowerOffset, Collision.GetContact(0).point + upperOffset, spikePadLayer).gameObject.name);
                                destroyablePad = Physics2D.OverlapArea(Collision.GetContact(0).point + lowerOffset, Collision.GetContact(0).point + upperOffset, spikePadLayer).gameObject;
                                ballSpeed = 2.35f;
                                bounceSound.Play();
                                StopAllCoroutines();
                                StartCoroutine(squishBall());
                                isEmpty = true;
                                upcover.SetActive(true);
                                downcover.SetActive(true);
                                score2.gameObject.SetActive(true);
                                ballDestination = Vector3.Reflect(Collision.GetContact(0).point, Collision.GetContact(0).normal) + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f),0);
                                ballDestination = new Vector3(ballDestination.x, ballDestination.y, -4);
                                //ballDestination= ballDestination * 10;
                                Debug.Log("spike pad collided "+Collision.GetContact(0).point+" "+ballDestination);
                                previousCollison = Collision.GetContact(0).point;
                                StartCoroutine(HomeController.instance.bounceRipple());
                                StartCoroutine(updateScore());
                                SpikeModeController.instance.StopAllCoroutines();
                                StartCoroutine(SpikeModeController.instance.createNewSpikes(destroyablePad));
                            }


                        }
                        break;
                    }
                case 11:
                    {
                        
                        Debug.Log("spike  collided ");
                        
                        StartCoroutine(ballBurstAnimation());
                        break;
                    }
                //case 12:
                //    {
                //        Debug.Log(" pad collided ");
                //        destroyablePad = Collision.collider.gameObject;
                //        isEmpty = false;

                //        break;
                //    }
                    //case 13:
                    //    {
                    //        Debug.Log("empty");
                    //        SpikeModeController.instance.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
                    //        break;
                    //    }
            }
        }
  
    
        

    }

    IEnumerator updateScore()
    {
        var dest1 = new Vector3(0, 3.15f, 0);
        var dest2 = new Vector3(0, 2.75f, 0);

        HomeController.instance.currentMainScore = int.Parse(score1.text) + 1;
        while (Vector3.Distance(score1.transform.localPosition, dest1) > 0.001f)
        {
            score1.transform.localPosition = Vector3.Lerp(score1.transform.localPosition, dest1, scoreTickerSpeed * 0.1f);
            score2.transform.localPosition = Vector3.Lerp(score2.transform.localPosition, dest2, scoreTickerSpeed * 0.1f);
            yield return null;
        }
        //Debug.Log("updating score");
        int currentScore = int.Parse(score1.text);
        currentScore++;
        score1.GetComponent<TextMeshPro>().text = currentScore.ToString();
        int nextScore = currentScore + 1;
        score2.GetComponent<TextMeshPro>().text = nextScore.ToString();
        score1.transform.localPosition = new Vector3(0, 2.75f, 0);
        score2.transform.localPosition = new Vector3(0, 2.35f, 0);
    }

    void playerOut()
    {
        isOut = true;
        ballSpeed = 0;
        upcover.SetActive(false);
        downcover.SetActive(false);
        score2.gameObject.SetActive(false);
        StartCoroutine(HomeController.instance.replayScreenOpenCLose(true));
        score2.GetComponent<TextMeshPro>().text = "1";
        rb.isKinematic = true;
        GetComponent<Ball>().enabled = false;
    }


    IEnumerator squishBall()
    {
        var horSquish = new Vector3(1.15f, 1.15f, 0);
        var verSquish = new Vector3(0.75f, 0.75f, 0);
        while (Vector3.Distance(transform.localScale, horSquish) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, horSquish, 0.45f);
            yield return null;
        }
        //while (Vector3.Distance(transform.localScale, verSquish) > 0.01f)
        //{
        //    transform.localScale = Vector3.Lerp(transform.localScale, verSquish, 0.75f);
        //    yield return null;
        //}
        while (Vector3.Distance(transform.localScale, Vector3.one) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.45f);
            yield return null;
        }
    }


    IEnumerator ballBurstAnimation()
    {
        isOut = true;
        ballSpeed = 0;
        ballBurstParticle.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0);
        List<GameObject> particles = new List<GameObject>();
        
        List<Vector3> destinations = new List<Vector3>();
        for (int i = 0; i < 15; i++)
        {
            var particle = Instantiate(ballBurstParticle, transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0),Quaternion.identity);
            var destination = particle.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            particles.Add(particle);
            destinations.Add(destination);
        }
        
        while (Vector3.Distance(particles[0].transform.localScale,Vector3.zero) > 0.1f)
        {
            Debug.Log("bursting "+ Vector3.Distance(particles[0].transform.localScale, Vector3.zero));
            for (int counter = 0; counter < 15; counter++)
            {
                particles[counter].transform.position = Vector3.Lerp(particles[counter].transform.position, destinations[counter], 0.2f);
                particles[counter].transform.localScale = Vector3.Lerp(particles[counter].transform.localScale, Vector3.zero, 0.05f);
            }
            yield return null;
        }
        foreach (GameObject p in particles)
            Destroy(p);
        playerOut();
    }
}
