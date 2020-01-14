using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEcho : MonoBehaviour
{
    private float timeBetweenSpawns;
    public float startTimeBetweenSpawns = 0.005f;
    private SpriteRenderer parentRenderer, echoRenderer;
    public Sprite sprite;
    private GameObject echo;

    private void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        echo = Resources.Load<GameObject>("PlayerEcho");
        echoRenderer = echo.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        if (timeBetweenSpawns <= 0)
        {
            echoRenderer.sprite = parentRenderer.sprite;
            echo.transform.localScale = transform.parent.localScale;
            //echo.GetComponent<SpriteRenderer>().material = parentRenderer.material;
            echoRenderer.flipX = parentRenderer.flipX;
            Instantiate(echo, transform.position, Quaternion.identity);

            timeBetweenSpawns = startTimeBetweenSpawns;
        }
        else
        {
            timeBetweenSpawns -= Time.deltaTime;    
        }
    }
}
