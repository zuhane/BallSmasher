using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOrb : MonoBehaviour
{

    public FacingDirection thisFacingDirection;
    [HideInInspector] public bool attacking;
    protected Vector2 flingDirection;
    public Vector2 finalFlingDirection;
    public float xOffset = 0f, yOffset = 0f;

    private int liveTimeCounter, liveTimeLimit = 30;
    private float returningTimeStrength = 0.2f;

    [SerializeField] public AudioClip chargeUpSound, chargedFullySound, attackReleaseSound;
    private GameObject smashEffect;

    private BoxCollider2D boxCollider;

    protected float _force = 10;

    private Vector3 playerPos;

    public float force
    {
        get
        {
            return _force;
        }
        set
        {
            _force = value;
            //transform.localScale *= 0.1f + (_force / 4);
            finalFlingDirection = flingDirection * _force;
        }
    }

    protected Vector3 startPos;
    protected Animator anim;

    protected float lifeLimit = 0.2f;

    public enum FireState
    {
        Idling,
        Charging,
        Charged,
        Live,
        Returning
    }

    private FireState _fireState = FireState.Idling;

    public FireState fireState
    {
        get
        {
            return _fireState;
        }
        set
        {
            _fireState = value;

            switch (_fireState)
            {
                case FireState.Idling:

                    break;
                case FireState.Charging:

                    break;
                case FireState.Charged:
                    charged = true;
                    break;
                case FireState.Live:

                    break;
                case FireState.Returning:

                    break;
            }
        }
    }

    private bool _charged;
    private bool charged
    {
        get
        {
            return _charged;
        }
        set
        {
            _charged = value;
            if (_charged) damage *= 2;
            anim.SetBool("FullyCharged", _charged);
        }
    }

    public int damage = 1;


    public virtual void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        smashEffect = Resources.Load<GameObject>("AttackEffect");
        boxCollider = GetComponent<BoxCollider2D>();
        playerPos = transform.parent.transform.position;
        float offset = 0.3f;

        SetDirection(thisFacingDirection);
    }

    public void Update()
    {
        if (fireState == FireState.Live)
        {
            transform.position += new Vector3(finalFlingDirection.x * _force, finalFlingDirection.y * _force) / 500;


            liveTimeCounter++;

            if (liveTimeCounter >= liveTimeLimit)
            {
                liveTimeCounter = 0;
                fireState = FireState.Returning;
            }
        }

        if (fireState == FireState.Returning)
        {

            GetComponent<SpawnEcho>().enabled = false;

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.parent.transform.position.x, returningTimeStrength), Mathf.Lerp(transform.position.y, transform.parent.transform.position.y, returningTimeStrength));

            if (transform.position.x < transform.parent.transform.position.x + 0.01f && transform.position.x > transform.parent.transform.position.x - 0.01f &&
                transform.position.y < transform.parent.transform.position.y + 0.01f && transform.position.y > transform.parent.transform.position.y - 0.01f)
            {
                fireState = FireState.Idling;
                GetComponent<TrailRenderer>().enabled = false;
                transform.position = transform.parent.transform.position;
            }
        }
    }


    public void SetDirection(FacingDirection facingDirection)
    {
        thisFacingDirection = facingDirection;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        switch (facingDirection)
        {
            case FacingDirection.Up:
                flingDirection = new Vector2(0, 1);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = new Vector3(transform.localPosition.x + xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = new Vector3(transform.localPosition.x - xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset, transform.localPosition.z);
                break;
            case FacingDirection.DownOut:
                flingDirection = new Vector2(0.5f, 1.5f);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset, transform.localPosition.z);
                _force /= 3;
                break;
        }
    }

    public void MultiplyScale(float scale)
    {

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (_fireState == FireState.Live)
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

                fireState = FireState.Returning;

                if (rigidBody.velocity.y < 0) rigidBody.setY(0);
                if (collision.transform.position.x < transform.parent.transform.position.x && thisFacingDirection == FacingDirection.DownOut) { finalFlingDirection.x *= -1; }

                if (this is SpinningStrikeBox)
                {
                    float inputValue = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                    finalFlingDirection = new Vector2(-(float)Math.Sin(inputValue), (float)Math.Cos(inputValue)).normalized;
                    finalFlingDirection *= 10;
                }

                rigidBody.addX(finalFlingDirection.x);
                rigidBody.addY(finalFlingDirection.y);
                
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
