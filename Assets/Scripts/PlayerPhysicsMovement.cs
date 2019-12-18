using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsMovement : PhysicsObject
{
    //Animator stuff
    [HideInInspector] public bool running;
    [HideInInspector] public bool ascending;
    [HideInInspector] public bool descending;
    [HideInInspector] public bool aerial;
    [HideInInspector] public bool crouching;
    [HideInInspector] public bool wallSliding;
    [HideInInspector] public bool sliding;

    //Modifiable
    [SerializeField] [Range(0, 10)] private int maxJumps = 2;
    [HideInInspector] public int currJump;
    [HideInInspector] public bool facingLeft;
    [Range(0, 100)] public float jumpTakeOffSpeed = 30f;
    [Range(0, 10)] public float acceleration = 0.0013f;

    public void AddVelocity(Vector2 inVelocity)
    {
        targetVelocity = inVelocity;
    }

    protected override void ComputeVelocity()
    {
        if (brushingFeet || brushingLeft || brushingRight)
        {
            currJump = 0;
        }

        intent_moveLeft = false;
        intent_moveRight = false;

        if (!crouching)
        {
            if (Input.GetKey(KeyCode.A))
            {
                targetVelocity.x -= acceleration;
                intent_moveLeft = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                targetVelocity.x += acceleration;
                intent_moveRight = true;
            }
        }

        if (Input.GetKey(KeyCode.S) && brushingFeet) crouching = true; else crouching = false;

        if ((pushingLeft || pushingRight) && aerial) wallSliding = true; else wallSliding = false;

        if ((intent_moveLeft && !brushingLeft) || (intent_moveRight && !brushingRight) && brushingFeet)
        {
            running = true;
        }
        else
        {
            running = false;
        }

        if (!brushingFeet)
        {
            aerial = true;

            if (velocity.y < 0) descending = true;
            else descending = false;

            if (velocity.y > 0) ascending = true;
            else ascending = false;
        }
        else
        {
            ascending = false;
            descending = false;
            aerial = false;
        }

        if (Input.GetButtonDown("JumpP1") && currJump < maxJumps)
        {
            velocity.y = jumpTakeOffSpeed;

            //Wall jumping
            if (brushingLeft && !brushingFeet && !pushingLeft)
            {
                intent_moveRight = true;
                targetVelocity.x += jumpTakeOffSpeed;
            }
            if (brushingRight && !brushingFeet && !pushingRight)
            {
                intent_moveLeft = true;
                targetVelocity.x -= jumpTakeOffSpeed;
            }
        }
        else if (Input.GetButtonUp("JumpP1"))
        {
            currJump++;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (intent_moveLeft) facingLeft = true;
        if (intent_moveRight) facingLeft = false;

    }

}
