using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOrb : MonoBehaviour
{

    private float rotateFrequency = 60f;

    public FacingDirection thisFacingDirection;

    [HideInInspector] public bool attacking;

    protected Vector2 flingDirection;
    public Vector2 finalFlingDirection;

    public float xOffset = 0f, yOffset = 0f;

    public int liveTimeCounter, liveTimeLimit = 30;
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
                    anim.SetBool("Returning", false);
                    GetComponent<TrailRenderer>().enabled = false;
                    break;
                case FireState.Charging:
                    anim.SetBool("Charging", true);
                    break;
                case FireState.Charged:
                    charged = true;
                    break;
                case FireState.Live:
                    anim.SetBool("Charging", false);
                    GetComponent<TrailRenderer>().enabled = true;
                    transform.GetComponentInChildren<SpawnEcho>().enabled = true;
                    break;
                case FireState.Returning:
                    anim.SetBool("Returning", true);
                    transform.GetComponentInChildren<SpawnEcho>().enabled = false;
                    liveTimeCounter = 0;
                    charged = false;
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

        SetDirection(thisFacingDirection);
    }

    public void Update()
    {
        if (fireState == FireState.Live)
        {
            liveTimeCounter++;

            if (liveTimeCounter >= liveTimeLimit)
                fireState = FireState.Returning;
            else
                transform.position += new Vector3(finalFlingDirection.normalized.x * 10, finalFlingDirection.normalized.y * 10) / 50;
        }

        if (fireState == FireState.Returning)
        {
            GetComponent<TrailRenderer>().time -= 0.08f;

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.parent.transform.position.x, returningTimeStrength), Mathf.Lerp(transform.position.y, transform.parent.transform.position.y, returningTimeStrength));

            if (transform.position.x < transform.parent.transform.position.x + 0.01f && transform.position.x > transform.parent.transform.position.x - 0.01f &&
                transform.position.y < transform.parent.transform.position.y + 0.01f && transform.position.y > transform.parent.transform.position.y - 0.01f)
            {
                //If returning orb is near the sender
                fireState = FireState.Idling;
                GetComponent<TrailRenderer>().time = 0.4f;
                GetComponent<TrailRenderer>().enabled = false;
                transform.position = transform.parent.transform.position;
            }
        }
    }

    public void Rotate()
    {
        float angle = (180 / rotateFrequency) * (thisFacingDirection == FacingDirection.Clockwise ? -1 : 1);
        transform.RotateAround(transform.parent.transform.position, new Vector3(0, 0, 1), angle);
        transform.parent.Find("AttackOrb").RotateAround(transform.parent.transform.position, new Vector3(0, 0, 1), angle);
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
                flingDirection = new Vector2(0.5f, 0.5f);
                if (transform.parent.GetComponent<Movement>().FacingLeft()) flingDirection.x *= -1;
                force *= 5;
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset - 0.1f, transform.localPosition.z);
                break;

            case FacingDirection.Clockwise:
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
            case FacingDirection.Anticlockwise:
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
        }
    }

    public void SetFlingDirection()
    {
        if (thisFacingDirection == FacingDirection.Clockwise || thisFacingDirection == FacingDirection.Anticlockwise)
        {
            float inputValue = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            flingDirection = new Vector2(-(float)Math.Sin(inputValue), (float)Math.Cos(inputValue)).normalized;
        }

    }

    public void MultiplyScale(float scale)
    {

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Trigger Stay");
        if (_fireState == FireState.Live)
        {
            //Debug.Log("Trigger Stay Live");

            Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (charged)
            {
                Instantiate(Resources.Load<GameObject>("Effects/ImpactHit"), transform.position, Quaternion.identity);
                AudioManager.PlaySound("StrikeHit" + UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(0.8f, 1.2f));
            }
            else
            {
                Instantiate(Resources.Load<GameObject>("Effects/ImpactHitWeak"), transform.position, Quaternion.identity);
                AudioManager.PlaySound("StrikeHitWeak" + UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(0.8f, 1.2f));
            }


            if (rigidBody != null && rigidBody.bodyType == RigidbodyType2D.Dynamic)
            {
                Ball ballHit = rigidBody.gameObject.GetComponent<Ball>();
                if (ballHit != null && charged)
                {
                    ballHit.ElectrifyBall();
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

            fireState = FireState.Returning;

        }

    }

}
