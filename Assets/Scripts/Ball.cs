using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private AudioSource audioSource;
    [Range(0, 10)] public int blockDamage = 1, maxHealth = 1;
    [Range(-10, 10)] public int playerDamage = 0;

    private int health, lifespanTick;
    public int lifespan = 0;

    private GoalManager goalManager;
    void Start()
    {
        float initialMaxForce = 0.8f;
        audioSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-initialMaxForce, initialMaxForce), Random.Range(-initialMaxForce, initialMaxForce)), ForceMode2D.Impulse);
        health = maxHealth;
        goalManager = GameObject.Find("GoalManager").GetComponent<GoalManager>();
    }

    private void Update()
    {
        lifespanTick++;

        if (maxHealth > 0 && health <= 0) {
            DestroyBall();
        }
        if (lifespan > 0 && lifespanTick > lifespan)
        {
            DestroyBall();
        }
    }

    public void DestroyBall()
    {
        AudioManager.PlaySound("PlasmaBallExplode", Random.Range(0.8f, 1.2f));
        Instantiate(Resources.Load<GameObject>("Effects/PlasmaBallExplosion"), transform.position, Quaternion.identity);
        Destroy(gameObject);
        goalManager.SpawnRandomBall();
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
