using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoomerang : MonoBehaviour
{

    [Range(1, 10)] public float chargeLimit = 2f;
    [Range(0, 1)] public float velocityModifier = 0.1f;
    [Range(0, 3)] public float enabledTime = 1f;
    [Range(5, 40)] public float impactForce = 10f;

    private float lerpReturnSpeed;
    private Rect rekt;
    protected Vector3 velocity;
    private Timer enabledTimer;

    private Vector2 previousPos;
    protected GameObject player, attackContainer;

    [SerializeField] private AudioClip attackReleaseSound, hitSoundWeak, hitSoundStrong;
    [SerializeField] public AudioClip chargeUpSound;
    [SerializeField] public AudioClip chargedFullySound;
    [SerializeField] private GameObject smashEffectWeak, smashEffectStrong;

    private Vector3 playerPos;

    [HideInInspector] public int lifeTimeCounter, lifeTimeLimit;

    [Range(0.1f, 20)] public float distancefactor = 3;
    [Range(0f, 1f)] public float speed = 0.4f;

    private bool _charged;
    protected bool charged
    {
        get
        {
            return _charged;
        }
        set
        {
            _charged = value;
            if (_charged) finalDamage = damage * 2;
            else finalDamage = damage;
        }
    }

    public int damage = 1;
    protected int finalDamage;

    public virtual void Start()
    {
        //anim = transform.GetComponentInChildren<Animator>()
        playerPos = transform.parent.transform.position;
        player = transform.root.gameObject;
        attackContainer = transform.parent.gameObject;
        //physicsObject = GetComponent<PhysicsObject>();
        previousPos = transform.position;

        rekt = new Rect(transform.position, new Vector2(0.1f, 0.1f));
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
                    //GetComponent<TrailRenderer>().enabled = false;
                    transform.SetParent(attackContainer.transform);
                    transform.localPosition = Vector2.zero;
                    lerpReturnSpeed = 0.01f;
                    break;
                case FireState.Charging:
                    break;
                case FireState.FullyCharged:
                    charged = true;
                    break;
                case FireState.Live:
                    //GetComponent<TrailRenderer>().enabled = true;
                    transform.SetParent(null);
                    break;
                case FireState.Returning:
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

            if (lerpReturnSpeed >= .2f) Returned();

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

        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(previousPos, new Vector2(tempRend.size.x / 2, tempRend.size.y / 2), CapsuleDirection2D.Horizontal, 0, velocity, (currentPos - previousPos).magnitude, PhysicsLayers.LayerMask((int)Layer.AttackOrb));

        for (int i = 0; i < hits.Length; i++)
        {
            HitTrigger(hits[i].collider);
        }


    }

    public virtual void Release(Vector3 attackDirectionVector, float charge)
    {
        AudioManager.PlaySound(attackReleaseSound);
        fireState = FireState.Live;
        charge = Mathf.Clamp(charge, 1, chargeLimit);
        velocity = attackDirectionVector * velocityModifier * charge;
        enabledTimer = Timer.CreateComponent(gameObject, enabledTime);
    }


    private void HitTrigger(Collider2D collision)
    {
        GameObject goCollisionRoot = collision.transform.root.gameObject;
        if (goCollisionRoot == player) { return; } //If ball hits self, ignore it.

        Rigidbody2D rigidBody = goCollisionRoot.GetComponent<Rigidbody2D>();
        if (rigidBody != null)
        {
            transform.position = collision.transform.position;

            if (charged)
            {
                if (smashEffectStrong != null) Instantiate(smashEffectStrong, goCollisionRoot.transform.position, Quaternion.identity);
                if (hitSoundStrong != null) AudioManager.PlaySound(hitSoundStrong);
            }
            else
            {
                if (smashEffectWeak != null) Instantiate(smashEffectWeak, goCollisionRoot.transform.position, Quaternion.identity);
                if (hitSoundWeak != null) AudioManager.PlaySound(hitSoundWeak);
            }

            CollisionReaction(collision, goCollisionRoot, rigidBody);

            fireState = FireState.Returning;
        }
        else
        {
            Debug.LogError($"Missing rigidbody!!!! You fucking idiot!! {goCollisionRoot.name}");
        }

    }

    protected virtual void CollisionReaction(Collider2D collision, GameObject collisionRoot, Rigidbody2D rigidbody)
    {

    }
    protected virtual void Returned()
    {
        fireState = FireState.Idling;
    }
}
