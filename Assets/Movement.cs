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

    }
    private bool logCollisions = false;
    public bool bumpingHead, bumpingFeet, bumpingLeft, bumpingRight;

    public Intent intent = new Intent();

    public CapsuleCollider2D capsule;

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Vector2 maxVelocity;
    [SerializeField] private Vector2 maxMoveSpeed;
    [SerializeField] private float moveSpeed, jumpForce;
    [SerializeField] private int maxJumps;

    private Vector3 originalScale, flipScale;
    [HideInInspector] public bool moving, jumping, falling, crouching, wallSliding;
    [HideInInspector] public int currJump;
    private object hit;

    private CapsuleCollider2D bodyBox, crouchBox;

    // Start is called before the first frame update
    void Start()
    {
        bodyBox = transform.Find("BodyBox").gameObject.GetComponent<CapsuleCollider2D>();
        crouchBox = transform.Find("CrouchBox").gameObject.GetComponent<CapsuleCollider2D>();
        //transform.Find("Graphics").gameObject.AddComponent<PolygonCollider2D>();
        originalScale = transform.localScale;
        flipScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        capsule = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moving = true;

        //Destroy(transform.Find("Graphics").GetComponent<PolygonCollider2D>());
        //transform.Find("Graphics").gameObject.AddComponent<PolygonCollider2D>();
        if (intent.crouch && bumpingFeet)
        {
            bodyBox.enabled = false;
            crouchBox.enabled = true;
            crouching = true;
            moving = false;

        }
        else
        {
            if (crouching)
            {
                bodyBox.enabled = true;
                crouchBox.enabled = false;
            }
            crouching = false;
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
            jumping = false;
            falling = false;
        }
        else
        {
            falling = (rigid.velocity.y < 0);
            jumping = (rigid.velocity.y > 0);
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
                if (logCollisions) Debug.Log("Hit top");
                currJump = 0;
                break;

            }
            if (contact.normal.y < 0)
            {
                bumpingHead = true;
                if (logCollisions) Debug.Log("Hit bottom");
            }
            if (contact.normal.x < 0)
            {
                bumpingRight = true;
                if (logCollisions) Debug.Log("Hit left");
            }
            if (contact.normal.x > 0)
            {
                bumpingLeft = true;
               if (logCollisions) Debug.Log("Hit right");
            }
        }

        //lastContactPoint = 

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        bumpingFeet = false; bumpingHead = false; bumpingLeft = false; bumpingRight = false;
    }
}
