using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] protected float topSpeedX = 6f, topSpeedY = 5f;
    [SerializeField] [Range(0, 1)] protected float velocityXDecayGrounded = 0.87f;
    [SerializeField] [Range(0, 1)] protected float velocityXDecayAerial = 0.97f;

    public bool enabled = true;

    protected BoxCollider2D boxCollider;

    //Collision data
    [HideInInspector] public bool brushingHead, brushingFeet, brushingLeft, brushingRight, pushingLeft, pushingRight;
    private bool touchingAtLeastOneleft, touchingAtLeastOneRight;

    //TEMPORARY
    [HideInInspector] public bool intent_moveLeft, intent_moveRight;

    [HideInInspector] public float minGroundNormalY = 0.65f, minSideNormalX = 0.65f;
    [Range(0, 10)] public float gravityModifier = 0.1f;

    [HideInInspector] public Vector2 targetVelocity;
    protected Vector2 groundNormal;
    [HideInInspector] public Vector2 velocity;
    protected Rigidbody2D rb2d;

    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(9));
        contactFilter.useLayerMask = true;
        boxCollider = GetComponent<BoxCollider2D>();
        Debug.Log(boxCollider);
    }

    // Update is called once per frame
    void Update()
    {
        //targetVelocity = Vector2.zero;
        if (enabled)
        {
            ComputeVelocity();

            if (!intent_moveLeft && !intent_moveRight)
            {
                if (brushingFeet) targetVelocity.x *= velocityXDecayGrounded;
                else targetVelocity.x *= velocityXDecayAerial;
            }

            if (targetVelocity.x > topSpeedX) targetVelocity.x = topSpeedX;
            if (targetVelocity.x < -topSpeedX) targetVelocity.x = -topSpeedX;
            if (targetVelocity.y > topSpeedY) targetVelocity.y = topSpeedY;
            if (targetVelocity.y < -topSpeedY) targetVelocity.y = -topSpeedY;

            DebugContacts();
        }
    }

    private void DebugContacts()
    {
        //DEBUG STUFF!
        if (brushingFeet) Debug.DrawLine(transform.position, transform.position + new Vector3(0, -1, 0));
        if (brushingHead) Debug.DrawLine(transform.position, transform.position + new Vector3(0, 1, 0));
        if (brushingLeft) Debug.DrawLine(transform.position, transform.position + new Vector3(-1, 0, 0));
        if (brushingRight) Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0, 0));

        if (pushingLeft)
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(-1.4f, 0.1f, 0), Color.red);
            Debug.DrawLine(transform.position, transform.position + new Vector3(-1.4f, -0.1f, 0), Color.red);
        }
        if (pushingRight)
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(1.4f, 0.1f, 0), Color.red);
            Debug.DrawLine(transform.position, transform.position + new Vector3(1.4f, -0.1f, 0), Color.red);
        }



    }

    protected virtual void ComputeVelocity()
    {

    }

    private void FixedUpdate()
    {
        brushingFeet = false; brushingHead = false;
        pushingLeft = false; pushingRight = false;

        //This way seems the correct way. Needs more checks to cancel these out though.
        /*if (brushingLeft && targetVelocity.x > 0)*/
        brushingLeft = false;
        brushingRight = false;

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        brushingFeet = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

        brushingLeft = PostSideCheck(-1f);
        brushingRight = PostSideCheck(1f);
    }

    private bool PostSideCheck(float x)
    {
        RaycastHit2D[] hitBufferLeft = new RaycastHit2D[16];
        int count = rb2d.Cast(new Vector2(x, 0), contactFilter, hitBufferLeft, 0.01f);

        List<RaycastHit2D> hitListLeft = new List<RaycastHit2D>(16);
        hitListLeft.Clear();

        bool touching = false;

        for (int i = 0; i < count; i++)
        {
            if (hitListLeft[i].transform.gameObject.layer == 9)
            {
                hitListLeft.Add(hitBufferLeft[i]);
            }
        }

        for (int i = 0; i < hitListLeft.Count; i++)
        {
            Vector2 currentNormal = hitListLeft[i].normal;
            if (-currentNormal.x * x > -minSideNormalX * x)
            {
                touching = true;
            }
        }

        return touching;
    }

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        touchingAtLeastOneleft = false; touchingAtLeastOneRight = false;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                //if not a ball
                if (hitBuffer[i].transform.gameObject.layer == 9)
                {
                    hitBufferList.Add(hitBuffer[i]);
                }
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    brushingFeet = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                if (currentNormal.y < -minGroundNormalY)
                {
                    brushingHead = true;
                }



                if (currentNormal.x > minSideNormalX)
                {
                    touchingAtLeastOneleft = true;

                    if (intent_moveLeft)
                    {
                        pushingLeft = true;
                    }

                    if (velocity.x < 0)
                    {
                        velocity.x = 0;
                        targetVelocity.x = 0;
                    }
                }

                if (currentNormal.x < -minSideNormalX)
                {
                    touchingAtLeastOneRight = true;

                    if (intent_moveRight)
                    {
                        pushingRight = true;
                    }

                    if (velocity.x > 0)
                    {
                        velocity.x = 0;
                        targetVelocity.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        if (touchingAtLeastOneleft) brushingLeft = true;
        if (touchingAtLeastOneRight) brushingRight = true;

        rb2d.position = rb2d.position + move.normalized * distance;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x < 0)
            {
                brushingRight = true;
            }
            if (contact.normal.x > 0)
            {
                brushingLeft = true;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exited!");

        brushingLeft = false; brushingRight = false;
    }
}
