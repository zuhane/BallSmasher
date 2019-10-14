using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEcho : MonoBehaviour
{
    private float timeBetweenSpawns;
    private float startTimeBetweenSpawns = 0.05f;
    private SpriteRenderer thisRenderer, playerRenderer;

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
            Instantiate(Resources.Load<GameObject>("PlayerEcho"), transform.position, Quaternion.identity);
            timeBetweenSpawns = startTimeBetweenSpawns;
        }
        else
        {
            timeBetweenSpawns -= Time.deltaTime;    
        }
    }
}
