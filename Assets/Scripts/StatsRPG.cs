using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsRPG : MonoBehaviour
{

    [SerializeField] private int MaxHP = 3;
    [SerializeField] private int MaxMP = 5;
    [SerializeField] private int armour = 0;
    [SerializeField] public int attackDamage = 1;
    [SerializeField] private float knockbackForce = 6;

    [HideInInspector] public int damageCounter, damageLimit = 5;
    [HideInInspector] public bool hurt;

    [SerializeField] private AudioClip hurtNoise, armourNoise, deathNoise;
    private AudioSource audioSource;

    [SerializeField] private GameObject explosion;

    private int HP, MP;
    private SpriteRenderer spriteRenderer;

    private bool isPlayer = false;

    private Color originalColour = Color.white;

    // Start is called before the first frame update
    void Start()
    {       
        audioSource = transform.GetChild(0).GetComponent<AudioSource>();
        audioSource.clip = hurtNoise;
        //explosion = Resources.Load<GameObject>("PlayerExplosion");

        ResetStats();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        PlayerProfile profile = GetComponent<PlayerProfile>();

        if (profile != null) originalColour = profile.colour;

        if (gameObject.tag == "Player") isPlayer = true;
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = damage - armour;

        if (finalDamage < 0) finalDamage = 0;

        HP -= finalDamage;

        if (audioSource != null && finalDamage > 0)
        {
            audioSource.clip = hurtNoise;
            audioSource.pitch = Random.Range(0.7f, 1.6f);
            audioSource.Play();

            spriteRenderer.color = Color.red;
            hurt = true;
        }
        else
        {
            audioSource.clip = armourNoise;
            audioSource.pitch = Random.Range(0.7f, 1.6f);
            audioSource.Play();

            spriteRenderer.color = Color.grey;
            hurt = true;
        }

        if (HP <= 0) Die();
    }

    public void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        if (isPlayer)
        {
            GameObject.Find("PlayerManager").GetComponent<PlayerSpawnManager>().HandleDeath(gameObject.GetComponent<PlayerManager>().playerNumber);
            ResetStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetStats()
    {
        HP = MaxHP;
        MP = MaxMP;
    }

    // Update is called once per frame
    void Update()
    {
        if (hurt)
        {
            damageCounter++;
        }

        if (damageCounter >= damageLimit)
        {
            hurt = false;
            damageCounter = 0;
            spriteRenderer.color = originalColour;
        }
    }
}
