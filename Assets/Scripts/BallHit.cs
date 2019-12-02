using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour
{
    private AudioSource audioSource;
    [Range(0, 10)] public int startingBlockDamage = 1;
    [Range(-10, 10)] public int startingPlayerDamage = 0;

    public int blockDamage { get; private set; }
    public int playerDamage { get; private set; }

    [HideInInspector] private bool electrified;
    private GameObject electricBall;

    void Start()
    {
        float initialMaxForce = 0.8f;
        audioSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-initialMaxForce, initialMaxForce), Random.Range(-initialMaxForce, initialMaxForce)), ForceMode2D.Impulse);
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
        electrified = true;
        blockDamage = startingBlockDamage;
        playerDamage = startingPlayerDamage;
        Destroy(electricBall);
    }

    private void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 3 && electrified)
        {
            UnelectrifyBall();
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
