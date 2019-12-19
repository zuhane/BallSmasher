using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private AudioSource audioSource;
    [Range(0, 10)] public int blockDamage = 1, maxHealth = 1;
    [Range(-10, 10)] public int playerDamage = 0;
    [Range(0, 10)] public int startingBlockDamage = 1;
    [Range(-10, 10)] public int startingPlayerDamage = 0;
    [Range(1, 3)] public int goalPoints = 1;

    [HideInInspector] public bool electrified;
    private GameObject electricBall;

    private int health;
    public int lifespan = 0;
    public GameObject explosionParticle;
    public AudioClip explodeNoise;
    private GoalManager goalManager;
    private Timer lifeTimer;
    void Start()
    {
        float initialMaxForce = 0.8f;
        audioSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-initialMaxForce, initialMaxForce), Random.Range(-initialMaxForce, initialMaxForce)), ForceMode2D.Impulse);
        health = maxHealth;
        goalManager = GameObject.Find("GoalManager").GetComponent<GoalManager>();

        if (lifespan > 0)
        {
            lifeTimer = Timer.CreateComponent(gameObject, lifespan);
        }

        if (Random.Range(0, 10) == 1)
        {
            GetComponent<Rigidbody2D>().gravityScale *= -0.5f;
        }

    }

    private void Update()
    {

        if (maxHealth > 0 && health <= 0)
        {
            DestroyBall();
        }

        if (lifeTimer?.LimitReached() == true)
        {
            DestroyBall();
        }

        if (GetComponent<Rigidbody2D>().velocity.magnitude < 3 && electrified)
        {
            UnelectrifyBall();
        }
    }

    public void DestroyBall()
    {
        if (explodeNoise != null)
        {
            AudioManager.PlaySound(explodeNoise);
        }
        if (explosionParticle != null)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        goalManager.SpawnRandomBall();
        blockDamage = startingBlockDamage;
        playerDamage = startingPlayerDamage;
    }

    public void ElectrifyBall()
    {
        if (electricBall != null) Destroy(electricBall);
        electrified = true;
        blockDamage = 10;
        playerDamage = startingPlayerDamage + 1;

        electricBall = Instantiate(Resources.Load<GameObject>("Effects/ElectricField"), transform.position, Quaternion.identity, gameObject.transform);
    }

    public void UnelectrifyBall()
    {
        electrified = false;
        blockDamage = startingBlockDamage;
        playerDamage = startingPlayerDamage;
        Destroy(electricBall);
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                DestroyBall();
            }
        }
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
