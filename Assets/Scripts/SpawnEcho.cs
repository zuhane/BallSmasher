using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEcho : MonoBehaviour
{
    private float timeBetweenSpawns;
    public float startTimeBetweenSpawns = 0.005f;
    private SpriteRenderer parentRenderer;
    public Sprite sprite;

    private void Update()
    {

        if (timeBetweenSpawns <= 0)
        {
            GameObject echo = Resources.Load<GameObject>("PlayerEcho");

            parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
            sprite = parentRenderer.sprite;
            echo.transform.localScale = transform.parent.localScale;
            echo.GetComponent<SpriteRenderer>().material = parentRenderer.material;
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
