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


    public Intent intent = new Intent();

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Vector2 maxVelocity;
    [SerializeField] private float moveSpeed, jumpForce;
    [SerializeField] private int maxJumps;

    private GameObject ground;
    private Vector3 originalScale, flipScale;
    private bool isGrounded;
    [HideInInspector] public bool moving, jumping, falling, crouching;
    [HideInInspector] public int currJump;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        flipScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        moving = true;

        if (intent.crouch)
        {
            crouching = true;
            moving = false;
        }
        else
        {
            crouching = false;
        }

        if (!crouching)
        {
            if (intent.left)
            {
                transform.localScale = flipScale;
                rigid.addX(-(moveSpeed));
            }
            else if (intent.right)
            {
                transform.localScale = originalScale;
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
                rigid.addY(jumpForce);
                currJump += 1;
            }
        }

    }
    private void LateUpdate()
    {
        if (isGrounded)
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y >= 1)
            {
                isGrounded = true;
                ground = collision.gameObject;
                currJump = 0;
                break;

            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == ground)
        {
            isGrounded = false;
            ground = null;
        }
    }
}
