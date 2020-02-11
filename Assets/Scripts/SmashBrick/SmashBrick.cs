using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SmashBrick : MonoBehaviour
{

    [SerializeField] [Range(1, 6)] private int maxHP = 4;
    private int HP;
    private Color color;
    private SpriteRenderer spriteRend;
    private Animator animator;
    [SerializeField] private GameObject explosion;
    private AudioSource audioSource;
    private float damageCounter, damageTimeLimitInSecs = 0.3f;
    private bool damaged = false;

    private void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        HP = maxHP;
        UpdateColor();
    }

    private void Update()
    {
        if (damaged)
        {
            damageCounter += Time.deltaTime;

            if (damageCounter > damageTimeLimitInSecs)
            {
                damageCounter = 0;
                damaged = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Bouncy")
        {

            Rigidbody2D tempRigid = collision.gameObject.GetComponent<Rigidbody2D>();

            float tempBounceForce = 3.5f;
            bool fastEnoughToDamage = false;
            float minVelocityToDamage = 0.1f;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < 0 && tempRigid.velocity.y > minVelocityToDamage)
                {
                    fastEnoughToDamage = true;
                }
                if (contact.normal.y > 0 && tempRigid.velocity.y < -minVelocityToDamage)
                {
                    fastEnoughToDamage = true;
                }
                if (contact.normal.x > 0 && tempRigid.velocity.x < -minVelocityToDamage)
                {
                    fastEnoughToDamage = true;
                }
                if (contact.normal.x < 0 && tempRigid.velocity.x > minVelocityToDamage)
                {
                    fastEnoughToDamage = true;
                }
            }

            fastEnoughToDamage = true;

            if (fastEnoughToDamage && !damaged)
            {
                int damageToDeal = 0;

                damageToDeal = collision.gameObject.GetComponent<Ball>().blockDamage;
                
                audioSource.pitch = Random.Range(1.2f, 1.8f);
                audioSource.Play();
                animator.SetTrigger("Hit");
                //collision.gameObject.GetComponent<Rigidbody2D>().setX(collision.GetContact(0).normal.x * -collision.gameObject.GetComponent<Rigidbody2D>().velocity.x);
                //collision.gameObject.GetComponent<Rigidbody2D>().setY(collision.GetContact(0).normal.y * -collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);

                Damage(damageToDeal);
            }
        }
    }

    public void Damage(int damage)
    {
        HP -= damage;
        UpdateColor();
        damaged = true;

        if (HP <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void UpdateColor()
    {
        switch (HP)
        {
            case 6:
                color = Color.white;
                break;
            case 5:
                color = new Color32(0, 220, 255, 255); //Light blue
                break;
            case 4:
                color = Color.green;
                break;
            case 3:
                color = Color.yellow;
                break;
            case 2:
                color = new Color32(254, 161, 0, 255); //Orange
                break;
            case 1:
                color = Color.red;
                break;
        }

        spriteRend.color = color;
    }


}
