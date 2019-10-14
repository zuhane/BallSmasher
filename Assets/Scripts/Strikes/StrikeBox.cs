using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeBox : MonoBehaviour
{

    public FacingDirection thisFacingDirection;

    protected Vector2 flingDirection;
    public Vector2 finalFlingDirection;
    public float xOffset = 0f, yOffset = 0f;

    [SerializeField] public AudioClip chargeUpSound, chargedFullySound, attackReleaseSound;
    private GameObject smashEffect;

    private BoxCollider2D boxCollider;

    protected float _force = 10;

    public float force
    {
        get
        {
            return _force;
        }
        set
        {
            _force = value;
            transform.localScale *= 0.1f + (_force / 4);
            finalFlingDirection = flingDirection * _force;
        }
    }

    protected bool used = false;
    protected Vector3 startPos;
    protected Animator anim;

    protected float lifeLimit = 0.2f;
    protected bool _released;
    public bool released
    {
        get
        {
            return _released;
        }
        set
        {
            _released = value;
            anim.SetBool("Firing", _released);
            GameObject temp = Instantiate(smashEffect, transform.position, transform.rotation);
            Destroy(temp, 1);
            //temp.transform.localPosition = new Vector3(boxCollider.bounds.size.x / 2, 0, 0);
            Destroy(gameObject, lifeLimit);
        }
    }

    protected bool _charged;
    public bool charged
    {
        get
        {
            return _charged;
        }
        set
        {
            _charged = value;
            anim.SetBool("FullyCharged", _charged);
        }
    }

    public int damage = 1;


    public virtual void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        smashEffect = Resources.Load<GameObject>("AttackEffect");
        boxCollider = GetComponent<BoxCollider2D>();
        float offset = 0.3f;

        SetDirection(thisFacingDirection);
    }

    public virtual void Update()
    {

    }

    public virtual void SetDirection(FacingDirection facingDirection)
    {

    }

    public void MultiplyScale(float scale)
    {

    }

    protected  void OnTriggerStay2D(Collider2D collision)
    {
        if (_released && !used)
        {
            //Debug.Log("StrikeBox Collision: " + collision.name);
            Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody != null && rigidBody.bodyType == RigidbodyType2D.Dynamic)
            {
                if (charged)
                {
                    Instantiate(Resources.Load<GameObject>("Effects/ImpactHit"), collision.gameObject.transform.position, Quaternion.identity);
                    AudioManager.PlaySound("StrikeHit" + UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(0.8f, 1.2f));
                }
                else
                {
                    Instantiate(Resources.Load<GameObject>("Effects/ImpactHitWeak"), collision.gameObject.transform.position, Quaternion.identity);
                    AudioManager.PlaySound("StrikeHitWeak" + UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(0.8f, 1.2f));
                }



                used = true;

                if (rigidBody.velocity.y < 0) rigidBody.setY(0);

                if (collision.transform.position.x < transform.parent.transform.position.x && thisFacingDirection == FacingDirection.DownOut) { finalFlingDirection.x *= -1; }

                if (this is SpinningStrikeBox)
                {

                    //finalFlingDirection = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));

                    //finalFlingDirection = new Vector2(-transform.rotation.z, 0);

                    float inputValue = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

                    finalFlingDirection = new Vector2(-(float)Math.Sin(inputValue), (float)Math.Cos(inputValue)).normalized;

                    //Debug.Log(transform.rotation.eulerAngles);
                    

                    finalFlingDirection *= 10;
                }

                rigidBody.addX(finalFlingDirection.x);
                rigidBody.addY(finalFlingDirection.y);

                Destroy(gameObject);

                StatsRPG stats = collision.gameObject.GetComponent<StatsRPG>();

                if (stats != null)
                {
                    Debug.Log(damage);
                    stats.TakeDamage(damage);
                }
            }

        }

    }

}
