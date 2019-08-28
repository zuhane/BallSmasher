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
