using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Explosion : MonoBehaviour
{
    public int damage;
    public float radius = 3;
    public float knockback;

    public GameObject explosionEffect;
    public AudioClip audioClip;

    private bool kill = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (kill)
        {
            if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
            if (audioClip != null) AudioManager.PlaySound(audioClip);
            Destroy(gameObject);
        }

        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        //foreach (Collider2D hit in colliders)
        //{
        //    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

        //    if (rb == null) return;

        //    rb.AddForceAtPosition()
        //}

        kill = true;
    }
}
