using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOrb : MonoBehaviour
{
    Vector2 previousPos;



    [SerializeField][Range(1, 10)] private float rotateFrequency = 2;
    [Range(1f, 10f)] public float strikeForce = 5f;
    [HideInInspector] public FacingDirection thisFacingDirection;

    [HideInInspector] public bool attacking;

    [HideInInspector] protected Vector2 flingDirection;
    [HideInInspector] public Vector2 finalFlingDirection;

    [HideInInspector] public float xOffset = 0f, yOffset = 0f;

    [HideInInspector] public int lifeTimeCounter, lifeTimeLimit;
    [Range(1, 20)] public int distancefactor = 5;
    [Range(0f, 1f)] public float speed;

    private PhysicsObject physicsObject;

    private SpawnEcho spawnEcho;

    private GameObject player;

    private float returningTimeStrength = 0.2f;

    [SerializeField] public AudioClip chargeUpSound, chargedFullySound, attackReleaseSound;
    private GameObject smashEffect;

    protected float _force = 10;

    private Vector3 playerPos;

    private HookshotGO hookshot;

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
                    if (spawnEcho != null) spawnEcho.enabled = true;
                    transform.SetParent(null);
                    break;
                case FireState.Returning:
                    anim.SetBool("Returning", true);
                    if (spawnEcho != null) spawnEcho.enabled = false;
                    lifeTimeCounter = 0;
                    charged = false;
                    transform.SetParent(player.transform);
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
        playerPos = transform.parent.transform.position;
        spawnEcho = transform.GetComponentInChildren<SpawnEcho>();
        SetDirection(thisFacingDirection);
        player = transform.root.gameObject;
        //physicsObject = GetComponent<PhysicsObject>();
        previousPos = transform.position;
        hookshot = GetComponent<HookshotGO>();
    }

    public void Update()
    {
        previousPos = transform.position;

        if (fireState == FireState.Live)
        {
            lifeTimeCounter++;

            if (lifeTimeCounter >= lifeTimeLimit)
                fireState = FireState.Returning;
            else
                transform.position += new Vector3(finalFlingDirection.normalized.x * speed, finalFlingDirection.normalized.y * speed);
        }

        if (fireState == FireState.Returning)
        {
            GetComponent<TrailRenderer>().time -= 0.08f;

            transform.position = new Vector2(Mathf.Lerp(transform.position.x, transform.parent.transform.position.x, speed), Mathf.Lerp(transform.position.y, transform.parent.transform.position.y, speed));

            if (transform.position.x < transform.parent.transform.position.x + 0.01f && transform.position.x > transform.parent.transform.position.x - 0.01f &&
                transform.position.y < transform.parent.transform.position.y + 0.01f && transform.position.y > transform.parent.transform.position.y - 0.01f)
            {
                //If returning orb is near the sender
                fireState = FireState.Idling;
                GetComponent<TrailRenderer>().time = 0.4f;
                GetComponent<TrailRenderer>().enabled = false;
                transform.position = transform.parent.transform.position;

                if (hookshot != null)
                {
                    hookshot.HookshotReturned();
                }
            }
        }

        CheckForHits();
    }

    private void CheckForHits()
    {
        //Simply checks the distance between the ball before and after moving in this same update cycle.
        //If the ball hits something, the first thing hit is checked, then the next, etc. If it hits something,
        //it returns, not hitting any other targets.
        if (fireState != FireState.Live) return;

        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
        //RaycastHit2D[] hits = Physics2D.RaycastAll(previousPos, (currentPos - previousPos).normalized, (currentPos - previousPos).magnitude);

        SpriteRenderer tempRend = GetComponentInChildren<SpriteRenderer>();

        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(previousPos, new Vector2(tempRend.size.x / 2, tempRend.size.y / 2), CapsuleDirection2D.Horizontal, 0, finalFlingDirection, (currentPos - previousPos).magnitude);

            
        for (int i = 0; i < hits.Length; i++)
        {
            BallhitTrigger(hits[i].collider);
            if (hookshot != null)
            {
                hookshot.BallhitTrigger(hits[i].collider);
            }
        }          
    }

    public void Rotate()
    {
        float angle = rotateFrequency * (thisFacingDirection == FacingDirection.Clockwise ? -1 : 1);
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
                flingDirection = new Vector2(0, 1f);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
            case FacingDirection.Right:
                flingDirection = new Vector2(1, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                transform.localPosition = new Vector3(transform.localPosition.x + xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Left:
                flingDirection = new Vector2(-1, 0);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                transform.localPosition = new Vector3(transform.localPosition.x - xOffset, transform.localPosition.y, transform.localPosition.z);
                break;
            case FacingDirection.Down:
                flingDirection = new Vector2(0, -1);
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset, transform.localPosition.z);
                break;
            case FacingDirection.DownOut:
                flingDirection = new Vector2(1, .85f);
                if (transform.parent.GetComponent<IntentToAction>().state.facingLeft) flingDirection.x *= -1;
                //force *= 5;
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - yOffset - 0.1f, transform.localPosition.z);
                break;

            case FacingDirection.Clockwise:
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
                break;
            case FacingDirection.Anticlockwise:
                //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
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

    private void BallhitTrigger(Collider2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot == player) { return; }
        

        if (_fireState == FireState.Live)
        {
            Rigidbody2D rigidBody = goCollisionRoot.GetComponent<Rigidbody2D>();
            if (rigidBody != null)
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

                if (collision.gameObject.layer == (int)Layer.AttackOrb) { return; }

                Ball ballHit = goCollisionRoot.GetComponent<Ball>();
                if (ballHit != null)
                {
                    ballHit.TakeDamage(damage);
                    if (charged)
                    {
                        ballHit.ElectrifyBall();
                    }
                }

                rigidBody.AddForce(new Vector2(finalFlingDirection.x, finalFlingDirection.y) * strikeForce);

                if (rigidBody.gameObject.layer == (int)Layer.Player) 
                    rigidBody.gameObject.GetComponent<PlayerPhysicsMovement>().AddVelocity(new Vector2(finalFlingDirection.x, finalFlingDirection.y) * strikeForce / 5);

                StatsRPG stats = goCollisionRoot.GetComponent<StatsRPG>();

                if (stats != null)
                {
                    Debug.Log(damage);
                    stats.TakeDamage(damage);
                }
                fireState = FireState.Returning;
            }
        }
    }
}
