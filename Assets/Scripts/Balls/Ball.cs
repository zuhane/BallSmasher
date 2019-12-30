using UnityEngine;

public class Ball : MonoBehaviour
{
    [Range(0, 10)] public int blockDamage = 1, maxHealth = 1, startingBlockDamage = 1;
    [Range(-10, 10)] public int playerDamage = 0, startingPlayerDamage = 0;
    [Range(1, 3)] public int goalPoints = 1;

    public int lifespan = 0;
    public bool canElectrify = true;
    public GameObject explosionParticle;
    public AudioClip explodeNoise;

    [HideInInspector] public bool electrified;

    private int health;
    private AudioSource audioSource;
    private GameObject electricBall;
    private Timer electricTimer;
    private GoalManager goalManager;
    private Rigidbody2D rigid;
    private Timer lifeTimer;


    void Start()
    {
        float initialMaxForce = 0.8f;

        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        goalManager = GameObject.Find("GoalManager").GetComponent<GoalManager>();
        rigid = GetComponent<Rigidbody2D>();

        if (lifespan > 0)
        {
            lifeTimer = Timer.CreateComponent(gameObject, lifespan);
        }

        if (Random.Range(0, 10) == 1)
        {
            rigid.gravityScale *= -0.5f;
        }

        rigid.AddForce(new Vector2(Random.Range(-initialMaxForce, initialMaxForce), Random.Range(-initialMaxForce, initialMaxForce)), ForceMode2D.Impulse);

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

        //TODO: check if this is possible with AddForce
        //if (rigid.velocity.magnitude < 3 && electrified)
        //{
        //    UnelectrifyBall();
        //}
        if(electricTimer?.LimitReached() == true)
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
        goalManager.SpawnRandomBall(transform.position);
        blockDamage = startingBlockDamage;
        playerDamage = startingPlayerDamage;
    }

    public void ElectrifyBall()
    {
        if (canElectrify)
        {
            if (electricBall != null) Destroy(electricBall);
            electrified = true;
            blockDamage = 10;
            playerDamage = startingPlayerDamage + 1;

            electricBall = Instantiate(Resources.Load<GameObject>("Effects/ElectricField"), transform.position, Quaternion.identity, gameObject.transform);
            electricTimer = Timer.CreateComponent(electricBall, 3);
        }
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
