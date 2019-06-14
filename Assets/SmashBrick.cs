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
    private GameObject explosion;
    private AudioSource audioSource;

    private void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        explosion = Resources.Load<GameObject>("BrickExplosion");
        audioSource = GetComponentInChildren<AudioSource>();
        HP = maxHP;
        UpdateColor();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bouncy")
        {
            audioSource.pitch = Random.Range(1.2f, 1.8f);
            audioSource.Play();
            animator.SetTrigger("Hit");
            HP -= collision.gameObject.GetComponent<BallHit>().damage;
            UpdateColor();
        }

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
