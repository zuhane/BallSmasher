using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEcho : MonoBehaviour
{
    private float timeBetweenSpawns;
    public float startTimeBetweenSpawns = 0.05f;
    private SpriteRenderer thisRenderer, playerRenderer;
    public Sprite sprite;

    private void Start()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
        playerRenderer = transform.parent.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        //thisRenderer.sprite = playerRenderer.sprite;

        if (timeBetweenSpawns <= 0)
        {
            GameObject echo = Resources.Load<GameObject>("PlayerEcho");

            if (sprite == null)
            {
                sprite = playerRenderer.sprite;
                echo.GetComponent<SpriteRenderer>().flipX = playerRenderer.flipX;
                echo.GetComponent<SpriteRenderer>().color = playerRenderer.color;
            }

            echo.GetComponent<SpriteRenderer>().sprite = sprite;
            Instantiate(echo, transform.position, Quaternion.identity);

            timeBetweenSpawns = startTimeBetweenSpawns;
        }
        else
        {
            timeBetweenSpawns -= Time.deltaTime;    
        }
    }
}
