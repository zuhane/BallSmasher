using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoomerang : MonoBehaviour
{

    protected Animator anim;

    [Range(1, 10)] public float chargeLimit = 2f;
    [Range(0, 1)] public float velocityModifier = 0.1f;
    [Range(0, 3)] public float enabledTime = 1f;
    [Range(5, 20)] public float impactForce = 10f;

    private float lerpReturnSpeed;
    private Rect rekt;
    private Vector3 velocity;
    private Timer enabledTimer;

    private Vector2 previousPos;
    private SpawnEcho spawnEcho;
    private GameObject player, attackContainer;

    [SerializeField] public AudioClip chargeUpSound, chargedFullySound, attackReleaseSound;
    private GameObject smashEffect;

    private Vector3 playerPos;

    [HideInInspector] public int lifeTimeCounter, lifeTimeLimit;

    [Range(1, 20)] public int distancefactor = 5;
    [Range(0f, 1f)] public float speed;

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
        player = transform.root.gameObject;
        attackContainer = transform.parent.gameObject;
        //physicsObject = GetComponent<PhysicsObject>();
        previousPos = transform.position;

        rekt = new Rect(transform.position, new Vector2(0.1f, 0.1f));
        enabledTimer = Timer.CreateComponent(gameObject, enabledTime);
    }

    public enum FireState
    {
        Idling,
        Charging,
        FullyCharged,
        Live,
        Returning
    }

    private FireState _fireState = FireState.Idling;

    protected float _force = 10;

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
                    //GetComponent<TrailRenderer>().enabled = false;
                    transform.SetParent(attackContainer.transform);
                    transform.localPosition = Vector2.zero;
                    lerpReturnSpeed = 0.01f;
                    break;
                case FireState.Charging:
                    anim.SetBool("Charging", true);
                    break;
                case FireState.FullyCharged:
                    charged = true;
                    break;
                case FireState.Live:
                    anim.SetBool("Charging", false);
                    //GetComponent<TrailRenderer>().enabled = true;
                    //if (spawnEcho != null) spawnEcho.enabled = true;
                    transform.SetParent(null);
                    break;
                case FireState.Returning:
                    anim.SetBool("Returning", true);
                    //if (spawnEcho != null) spawnEcho.enabled = false;
                    lifeTimeCounter = 0;
                    charged = false;
                    velocity = Vector2.zero;
                    break;
            }
        }
    }


    private void Update()
    {

        previousPos = transform.position;
        rekt.center = transform.position;
        if (enabledTimer?.LimitReached() == true)
        {
            Return();
        }

        transform.position += velocity;



        CheckForHits();

        if (fireState == FireState.Returning)
        {
            TrailRenderer trail = GetComponent<TrailRenderer>();
            //GetComponent<TrailRenderer>().time -= 0.08f;
            //trail.startColor = new Color(trail.startColor.r, trail.startColor.g, trail.startColor.b, 0f);

            lerpReturnSpeed += 0.01f; //TODO: Implement proper delta time
            Vector2 lerp = new Vector2(Mathf.Lerp(transform.position.x, attackContainer.transform.position.x, lerpReturnSpeed), Mathf.Lerp(transform.position.y, attackContainer.transform.position.y, lerpReturnSpeed));

            //Debug.Log($"BallPos: {transform.position}, ContainerPos: {attackContainer.transform.position}, lerp: {lerp} lerpSpeed {lerpReturnSpeed}");

            transform.position = lerp;

            if (lerpReturnSpeed >= 1) fireState = FireState.Idling;

        }

    }

    private void Return()
    {
        fireState = FireState.Returning;
        Destroy(enabledTimer);
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

        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(previousPos, new Vector2(tempRend.size.x / 2, tempRend.size.y / 2), CapsuleDirection2D.Horizontal, 0, velocity, (currentPos - previousPos).magnitude, (int)LayerMask.Ball | (int)LayerMask.Player | (int)LayerMask.AttackOrb);

        for (int i = 0; i < hits.Length; i++)
        {
            HitTrigger(hits[i].collider);
            ///if (hookshot != null)
            ///{
            ///hookshot.BallhitTrigger(hits[i].collider);
            ///}
        }


    }

    public virtual void Release(Vector3 attackDirectionVector, float charge)
    {
        fireState = FireState.Live;
        charge = Mathf.Clamp(charge, 1, chargeLimit);
        velocity = attackDirectionVector * velocityModifier * charge;
        enabledTimer = Timer.CreateComponent(gameObject, enabledTime);
    }


    protected virtual void HitTrigger(Collider2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot == player) { return; } //If ball hits self, ignore it.

        Rigidbody2D rigidBody = goCollisionRoot.GetComponent<Rigidbody2D>();
        if (rigidBody != null)
        {
            transform.position = collision.transform.position;

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

            if (collision.gameObject.layer == (int)Layer.AttackOrb)
            {
                //TODO: Maybe add attack orb shields?
            }

            Ball ballHit = goCollisionRoot.GetComponent<Ball>();
            if (ballHit != null)
            {
                rigidBody.AddForce(velocity.normalized * impactForce);
                ballHit.TakeDamage(damage);
                if (charged)
                {
                    ballHit.ElectrifyBall();
                }
            }

            PlayerPhysicsMovement playerPhysics = goCollisionRoot.GetComponent<PlayerPhysicsMovement>();
            if (playerPhysics != null)
            {
                playerPhysics.AddVelocity(velocity.normalized * impactForce / 5);
            }

            StatsRPG stats = goCollisionRoot.GetComponent<StatsRPG>();
            if (stats != null)
            {
                Debug.Log(damage);
                stats.TakeDamage(damage);
            }
            fireState = FireState.Returning;
        }
        else
        {
            Debug.LogError($"Missing rigidbody!!!! You fucking idiot!! {goCollisionRoot.name}");
        }

    }
}
