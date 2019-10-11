using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour
{
    private AudioSource audioSource;
    [Range(0, 10)] public int blockDamage = 1;
    [Range(-10, 10)] public int playerDamage = 0;

    void Start()
    {
        float initialMaxForce = 0.8f;
        audioSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-initialMaxForce, initialMaxForce), Random.Range(-initialMaxForce, initialMaxForce)), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.pitch = Random.Range(.9f, 1.5f);
        audioSource.Play();

        if (collision.gameObject.tag == "Player")
        {
            if (playerDamage > 0)
                collision.gameObject.GetComponent<StatsRPG>().TakeDamage(playerDamage);
            else if (playerDamage < 0) collision.gameObject.GetComponent<StatsRPG>().Heal(-playerDamage);
        }
    }
}
