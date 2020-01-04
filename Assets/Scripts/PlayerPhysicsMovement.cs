using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IntentToAction))]
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


    public IntentToAction.Intent intent;
    //TODO:
    public IntentToAction.State state;

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

        movingLeft = false;
        movingRight = false;

        if (!crouching)
        {
            if (intent.left)
            {
                if (targetVelocity.x > -maxRunSpeedX)
                    targetVelocity.x -= acceleration;
                movingLeft = true;
            }
            else if (intent.right)
            {
                if (targetVelocity.x < maxRunSpeedX)
                    targetVelocity.x += acceleration;
                movingRight = true;
            }
        }

        if (intent.crouch && brushingFeet) crouching = true; else crouching = false;

        if ((pushingLeft || pushingRight) && aerial) wallSliding = true; else wallSliding = false;

        if ((movingLeft && !brushingLeft) || (movingRight && !brushingRight) && brushingFeet)
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

        if (intent.jump && currJump < maxJumps)
        {
            velocity.y = jumpTakeOffSpeed;

            //Wall jumping
            if (brushingLeft && !brushingFeet && !pushingLeft)
            {
                movingRight = true;
                targetVelocity.x += jumpTakeOffSpeed;
            }
            if (brushingRight && !brushingFeet && !pushingRight)
            {
                movingLeft = true;
                targetVelocity.x -= jumpTakeOffSpeed;
            }
        }
        else if (intent.releaseJump)
        {
            currJump++;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (movingLeft) facingLeft = true;
        if (movingRight) facingLeft = false;

    }

}
