using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public class Intent
    {
        public bool right { get; set; }
        public bool left { get; set; }
        public bool up { get; set; }
        public bool down { get; set; }
        public bool jump { get; set; }
        public bool crouch { get; set; }
        public bool hitRight { get; set; }
        public bool hitLeft { get; set; }
        public bool hitUp { get; set; }
        public bool hitDown { get; set; }
        public bool dash { get; set; }
    }

    public bool bumpingHead, bumpingFeet, bumpingLeft, bumpingRight;

    public Intent intent = new Intent();

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Vector2 maxVelocity;
    [SerializeField] private Vector2 maxMoveSpeed;
    [SerializeField] private float moveSpeed, jumpForce;
    [SerializeField] private int maxJumps;

    private Vector3 originalScale, flipScale;
    [HideInInspector] public bool moving, ascending, descending, crouching, wallSliding;
    [HideInInspector] public int currJump;
    private object hit;

    private float dashSpeed = 8;

    private Vector4 standingScale, crouchingScale;

    [SerializeField] private CapsuleCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        standingScale = new Vector4(-0.02f, 0.04f, 0.3f, 0.45f);
        crouchingScale = new Vector4(-0.02f, -0.01f, 0.3f, 0.35f);
        //transform.Find("Graphics").gameObject.AddComponent<PolygonCollider2D>();
        originalScale = transform.localScale;
        flipScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moving = true;

        //Destroy(transform.Find("Graphics").GetComponent<PolygonCollider2D>());
        //transform.Find("Graphics").gameObject.AddComponent<PolygonCollider2D>();
        if (intent.crouch && bumpingFeet)
        {
            if (gameObject.tag == "Player")
            {
                collider.offset = new Vector2(crouchingScale.x, crouchingScale.y);
                collider.size = new Vector2(crouchingScale.z, crouchingScale.w);
            }

            crouching = true;
            moving = false;

        }
        else
        {
            if (gameObject.tag == "Player")
            {
                collider.offset = new Vector2(standingScale.x, standingScale.y);
                collider.size = new Vector2(standingScale.z, standingScale.w);
            }


            crouching = false;
        }

        if (intent.dash)
        {
            if (intent.left) rigid.addX(-dashSpeed);
            else if (intent.right) rigid.addX(dashSpeed);
        }

        if (!crouching)
        {
            if (intent.left)
            {
                transform.localScale = flipScale;

                if (rigid.velocity.x > -maxMoveSpeed.x)
                    rigid.addX(-(moveSpeed));
            }
            else if (intent.right)
            {
                transform.localScale = originalScale;
                if (rigid.velocity.x < maxMoveSpeed.x)
                    rigid.addX(moveSpeed);
            }
            else
            {
                moving = false;
            }
        }

        checkMaxSpeed();

        if (intent.jump)
        {
            if (currJump < maxJumps)
            {
                float wallKickForce = 6;

                if (rigid.velocity.y < 0) rigid.setY(0);
                rigid.addY(jumpForce);

                if (!bumpingFeet)
                {
                    if (bumpingLeft) rigid.addX(wallKickForce);
                    if (bumpingRight) rigid.addX(-wallKickForce);
                }

                currJump += 1;
            }
        }

        if ((bumpingLeft && intent.left) || (bumpingRight && intent.right))
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }
    }

    private void LateUpdate()
    {
        if (bumpingFeet)
        {
            ascending = false;
            descending = false;
        }
        else
        {
            descending = (rigid.velocity.y < 0);
            ascending = (rigid.velocity.y > 0);
        }
    }

    private void checkMaxSpeed()
    {
        if (rigid.velocity.x > maxVelocity.x)
            rigid.setX(maxVelocity.x);
        if (rigid.velocity.x < -(maxVelocity.x))
            rigid.setX(-(maxVelocity.x));
        if (rigid.velocity.y > maxVelocity.y)
            rigid.setY(maxVelocity.y);
        if (rigid.velocity.y < -(maxVelocity.y))
            rigid.setY(-(maxVelocity.y));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0 || contact.normal.x < 0 || contact.normal.x > 0)
            {
                currJump = 0;
            }
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        //if (capsule != null)
        //{
        //    if ((bumpingLeft || bumpingRight))
        //    {
        //        capsule.sharedMaterial.friction = 0f;
        //        capsule.enabled = false;
        //        capsule.enabled = true;
        //    }
        //    else
        //    {
        //        capsule.sharedMaterial.friction = 0.04f;
        //        capsule.enabled = false;
        //        capsule.enabled = true;
        //    }
        //}

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0)
            {
                bumpingFeet = true;
                //Debug.Log("Hit top");
                break;

            }
            if (contact.normal.y < 0)
            {
                bumpingHead = true;
                //Debug.Log("Hit bottom");
            }
            if (contact.normal.x < 0)
            {
                bumpingRight = true;
                //Debug.Log("Hit left");
            }
            if (contact.normal.x > 0)
            {
                bumpingLeft = true;
                //Debug.Log("Hit right");
            }
        }

        //lastContactPoint = 

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        bumpingFeet = false; bumpingHead = false; bumpingLeft = false; bumpingRight = false;
    }
}
